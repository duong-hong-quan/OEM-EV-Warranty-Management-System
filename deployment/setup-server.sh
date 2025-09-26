#!/bin/bash

# Server Setup Script for SSH Password Authentication
# Usage: sudo bash setup-server.sh

set -e

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Logging function
log() {
    echo -e "${BLUE}[$(date '+%Y-%m-%d %H:%M:%S')]${NC} $1"
}

success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

error() {
    echo -e "${RED}[ERROR]${NC} $1"
    exit 1
}

warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

# Check if running as root
if [ "$EUID" -ne 0 ]; then
    error "Please run this script as root (use sudo)"
fi

log "🔧 Setting up server for CI/CD with password authentication"

# Configure SSH for password authentication
log "🔐 Configuring SSH password authentication..."

# Backup SSH config
cp /etc/ssh/sshd_config /etc/ssh/sshd_config.backup

# Configure SSH settings
sed -i 's/#PasswordAuthentication yes/PasswordAuthentication yes/' /etc/ssh/sshd_config
sed -i 's/PasswordAuthentication no/PasswordAuthentication yes/' /etc/ssh/sshd_config
sed -i 's/#PermitRootLogin yes/PermitRootLogin no/' /etc/ssh/sshd_config
sed -i 's/PermitRootLogin yes/PermitRootLogin no/' /etc/ssh/sshd_config

# Restart SSH service
systemctl restart sshd
success "✅ SSH configured for password authentication"

# Install required packages
log "📦 Installing required packages..."
apt-get update
apt-get install -y nginx curl wget unzip tar lxd sshpass

success "✅ Required packages installed"

# Configure LXD if not already done
log "🐳 Configuring LXD..."
if ! lxd --version > /dev/null 2>&1; then
    lxd init --auto
fi

# Create LXC container if not exists
log "📦 Setting up LXC container..."
if ! lxc list demo-net-seminar | grep -q "demo-net-seminar"; then
    log "Creating LXC container 'demo-net-seminar'..."
    lxc launch ubuntu:22.04 demo-net-seminar
    
    # Wait for container to start
    sleep 10
    
    # Update container
    lxc exec demo-net-seminar -- apt-get update
    lxc exec demo-net-seminar -- apt-get upgrade -y
    
    success "✅ LXC container created"
else
    success "✅ LXC container already exists"
fi

# Install .NET and dependencies in container
log "🔧 Installing .NET and dependencies in container..."
lxc exec demo-net-seminar -- bash -c "
    apt-get update &&
    apt-get install -y wget curl nginx sudo &&
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb &&
    dpkg -i packages-microsoft-prod.deb &&
    rm packages-microsoft-prod.deb &&
    apt-get update &&
    apt-get install -y dotnet-runtime-8.0 aspnetcore-runtime-8.0
"

success "✅ .NET and dependencies installed in container"

# Setup application directories in container
log "📁 Setting up application directories..."
lxc exec demo-net-seminar -- bash -c "
    mkdir -p /opt/warranty-system/{backend,frontend,logs,data} &&
    mkdir -p /opt/backups/warranty-system &&
    chown -R www-data:www-data /opt/warranty-system &&
    chmod -R 755 /opt/warranty-system
"

success "✅ Application directories created"

# Copy deployment scripts to container
log "📋 Setting up deployment scripts..."
if [ -d "./deployment" ]; then
    lxc file push deployment/ demo-net-seminar/opt/warranty-system/ -r
    
    # Set permissions for scripts
    lxc exec demo-net-seminar -- chmod +x /opt/warranty-system/deploy.sh
    lxc exec demo-net-seminar -- chmod +x /opt/warranty-system/backup.sh
    lxc exec demo-net-seminar -- chmod +x /opt/warranty-system/restore.sh
    
    success "✅ Deployment scripts uploaded"
else
    warning "⚠️  Deployment directory not found, scripts will need to be uploaded manually"
fi

# Configure Nginx in container
log "🌐 Configuring Nginx in container..."
lxc exec demo-net-seminar -- bash -c "
    systemctl enable nginx &&
    systemctl start nginx
"

success "✅ Nginx configured and started"

# Configure firewall (optional)
log "🔥 Configuring firewall..."
if command -v ufw > /dev/null; then
    ufw allow ssh
    ufw allow http
    ufw allow https
    ufw --force enable
    success "✅ Firewall configured"
else
    warning "⚠️  UFW not installed, firewall not configured"
fi

# Test LXC container
log "🧪 Testing LXC container..."
if lxc exec demo-net-seminar -- echo "Container test successful"; then
    success "✅ LXC container is working"
else
    error "❌ LXC container test failed"
fi

# Create deployment user in container (optional)
log "👤 Setting up deployment user..."
lxc exec demo-net-seminar -- bash -c "
    if ! id 'deploy' &>/dev/null; then
        useradd -m -s /bin/bash deploy &&
        usermod -aG sudo deploy &&
        echo 'deploy ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers
    fi
"

success "✅ Deployment user configured"

# Display connection information
log "📊 Server setup completed successfully!"
echo ""
echo -e "${GREEN}🎉 Server Setup Summary${NC}"
echo "========================="
echo "Server IP: $(curl -s ifconfig.me 2>/dev/null || echo 'Unable to detect')"
echo "SSH Port: 22"
echo "LXC Container: demo-net-seminar"
echo "Application Path: /opt/warranty-system"
echo ""
echo -e "${BLUE}📝 GitHub Secrets to add:${NC}"
echo "SERVER_HOST=$(curl -s ifconfig.me 2>/dev/null || echo 'your-server-ip')"
echo "SERVER_USER=$(whoami)"
echo "SERVER_PASSWORD=your-server-password"
echo ""
echo -e "${BLUE}🧪 Test Connection:${NC}"
echo "ssh $(whoami)@$(curl -s ifconfig.me 2>/dev/null || echo 'your-server-ip')"
echo ""
echo -e "${BLUE}📋 Next Steps:${NC}"
echo "1. Add GitHub secrets to your repository"
echo "2. Test SSH connection with password"
echo "3. Push code to trigger deployment"
echo "4. Monitor deployment in GitHub Actions"

log "✅ Setup completed successfully!"
