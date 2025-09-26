#!/bin/bash

# Electric Vehicle Warranty System - Deployment Script
# Usage: sudo bash deploy.sh [environment]
# Environment: development | production

set -e  # Exit on any error

# Configuration
ENVIRONMENT=${1:-production}
APP_NAME="warranty-system"
APP_DIR="/opt/${APP_NAME}"
BACKUP_DIR="/opt/backups/${APP_NAME}"
LOG_FILE="/var/log/${APP_NAME}-deploy.log"
SERVICE_NAME="warranty-api"
NGINX_CONFIG="/etc/nginx/sites-available/${APP_NAME}"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Logging function
log() {
    echo -e "${BLUE}[$(date '+%Y-%m-%d %H:%M:%S')]${NC} $1" | tee -a ${LOG_FILE}
}

error() {
    echo -e "${RED}[ERROR]${NC} $1" | tee -a ${LOG_FILE}
    exit 1
}

success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1" | tee -a ${LOG_FILE}
}

warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1" | tee -a ${LOG_FILE}
}

# Check if running as root
if [ "$EUID" -ne 0 ]; then
    error "Please run this script as root (use sudo)"
fi

log "🚀 Starting deployment for environment: ${ENVIRONMENT}"

# Function to check service status
check_service() {
    local service_name=$1
    if systemctl is-active --quiet ${service_name}; then
        return 0
    else
        return 1
    fi
}

# Function to backup current deployment
backup_current_deployment() {
    log "📦 Creating backup of current deployment..."
    
    if [ -d "${APP_DIR}" ]; then
        local backup_timestamp=$(date +%Y%m%d_%H%M%S)
        local backup_path="${BACKUP_DIR}/${backup_timestamp}"
        
        mkdir -p ${BACKUP_DIR}
        cp -r ${APP_DIR} ${backup_path}
        
        success "✅ Backup created at ${backup_path}"
        
        # Keep only last 5 backups
        cd ${BACKUP_DIR}
        ls -t | tail -n +6 | xargs -r rm -rf
    else
        warning "⚠️  No existing deployment to backup"
    fi
}

# Function to setup directories
setup_directories() {
    log "📁 Setting up application directories..."
    
    mkdir -p ${APP_DIR}/{backend,frontend,logs,data}
    mkdir -p ${BACKUP_DIR}
    
    # Set permissions
    chown -R www-data:www-data ${APP_DIR}
    chmod -R 755 ${APP_DIR}
    
    success "✅ Directories setup completed"
}

# Function to deploy backend
deploy_backend() {
    log "🔧 Deploying backend application..."
    
    # Stop existing service
    if check_service ${SERVICE_NAME}; then
        log "🛑 Stopping ${SERVICE_NAME} service..."
        systemctl stop ${SERVICE_NAME}
    fi
    
    # Extract backend files from /tmp/backend-artifacts
    if [ -d "/tmp/backend-artifacts" ]; then
        cp -r /tmp/backend-artifacts/* ${APP_DIR}/backend/
        
        # Set permissions
        chown -R www-data:www-data ${APP_DIR}/backend
        chmod +x ${APP_DIR}/backend/BE.API
        
        success "✅ Backend files deployed"
    else
        error "❌ Backend artifacts not found in /tmp/backend-artifacts"
    fi
    
    # Update configuration based on environment
    update_backend_config
    
    # Run database migrations
    run_database_migrations
    
    # Create/update systemd service
    create_systemd_service
    
    # Start service
    log "🚀 Starting ${SERVICE_NAME} service..."
    systemctl daemon-reload
    systemctl enable ${SERVICE_NAME}
    systemctl start ${SERVICE_NAME}
    
    # Wait for service to start
    sleep 10
    
    if check_service ${SERVICE_NAME}; then
        success "✅ Backend service started successfully"
    else
        error "❌ Failed to start backend service"
    fi
}

# Function to deploy frontend
deploy_frontend() {
    log "🎨 Deploying frontend application..."
    
    # Extract frontend files from /tmp/frontend-artifacts
    if [ -d "/tmp/frontend-artifacts" ]; then
        cp -r /tmp/frontend-artifacts/* ${APP_DIR}/frontend/
        
        # Set permissions
        chown -R www-data:www-data ${APP_DIR}/frontend
        chmod -R 644 ${APP_DIR}/frontend
        find ${APP_DIR}/frontend -type d -exec chmod 755 {} \;
        
        success "✅ Frontend files deployed"
    else
        error "❌ Frontend artifacts not found in /tmp/frontend-artifacts"
    fi
    
    # Update Nginx configuration
    update_nginx_config
    
    # Test and reload Nginx
    if nginx -t; then
        systemctl reload nginx
        success "✅ Nginx reloaded successfully"
    else
        error "❌ Nginx configuration test failed"
    fi
}

# Function to update backend configuration
update_backend_config() {
    log "⚙️  Updating backend configuration..."
    
    local config_file="${APP_DIR}/backend/appsettings.${ENVIRONMENT}.json"
    
    # Create environment-specific configuration
    cat > ${config_file} << EOF
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "${DATABASE_CONNECTION_STRING}"
  },
  "Jwt": {
    "Key": "${JWT_SECRET_KEY}",
    "Issuer": "ElectricVehicleWarrantyAPI",
    "Audience": "ElectricVehicleWarrantyAPI"
  }
}
EOF
    
    # Set proper permissions
    chown www-data:www-data ${config_file}
    chmod 600 ${config_file}
    
    success "✅ Backend configuration updated"
}

# Function to run database migrations
run_database_migrations() {
    log "🗄️  Running database migrations..."
    
    cd ${APP_DIR}/backend
    
    # Run as www-data user
    sudo -u www-data dotnet ef database update --no-build || {
        warning "⚠️  Database migration failed, attempting to continue..."
    }
    
    success "✅ Database migrations completed"
}

# Function to create systemd service
create_systemd_service() {
    log "🔧 Creating systemd service..."
    
    cat > /etc/systemd/system/${SERVICE_NAME}.service << EOF
[Unit]
Description=Electric Vehicle Warranty API
After=network.target
Wants=network.target

[Service]
Type=notify
User=www-data
Group=www-data
WorkingDirectory=${APP_DIR}/backend
ExecStart=/usr/bin/dotnet ${APP_DIR}/backend/BE.API.dll
Restart=always
RestartSec=10
SyslogIdentifier=${SERVICE_NAME}
Environment=ASPNETCORE_ENVIRONMENT=${ENVIRONMENT}
Environment=ASPNETCORE_URLS=http://localhost:5000
KillSignal=SIGINT
TimeoutStopSec=30

[Install]
WantedBy=multi-user.target
EOF

    success "✅ Systemd service created"
}

# Function to update Nginx configuration
update_nginx_config() {
    log "🌐 Updating Nginx configuration..."
    
    # Determine domain based on environment
    local domain
    if [ "${ENVIRONMENT}" = "production" ]; then
        domain="your-domain.com"
    else
        domain="dev.your-domain.com"
    fi
    
    cat > ${NGINX_CONFIG} << EOF
server {
    listen 80;
    server_name ${domain};
    
    # Redirect HTTP to HTTPS
    return 301 https://\$server_name\$request_uri;
}

server {
    listen 443 ssl http2;
    server_name ${domain};
    
    # SSL Configuration (update paths as needed)
    ssl_certificate /etc/ssl/certs/${domain}.pem;
    ssl_certificate_key /etc/ssl/private/${domain}.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers ECDHE-RSA-AES128-GCM-SHA256:ECDHE-RSA-AES256-GCM-SHA384;
    ssl_prefer_server_ciphers off;
    
    # Security headers
    add_header X-Frame-Options DENY;
    add_header X-Content-Type-Options nosniff;
    add_header X-XSS-Protection "1; mode=block";
    add_header Strict-Transport-Security "max-age=63072000; includeSubDomains; preload";
    
    # Frontend
    location / {
        root ${APP_DIR}/frontend;
        index index.html;
        try_files \$uri \$uri/ /index.html;
        
        # Cache static assets
        location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg)$ {
            expires 1y;
            add_header Cache-Control "public, immutable";
        }
    }
    
    # Backend API
    location /api/ {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
        proxy_cache_bypass \$http_upgrade;
        
        # Timeouts
        proxy_connect_timeout 60s;
        proxy_send_timeout 60s;
        proxy_read_timeout 60s;
    }
    
    # Health check endpoint
    location /health {
        access_log off;
        return 200 "healthy\n";
        add_header Content-Type text/plain;
    }
}
EOF

    # Enable site
    ln -sf ${NGINX_CONFIG} /etc/nginx/sites-enabled/
    
    success "✅ Nginx configuration updated"
}

# Function to perform health checks
health_check() {
    log "🏥 Performing health checks..."
    
    # Check backend service
    if check_service ${SERVICE_NAME}; then
        success "✅ Backend service is running"
    else
        error "❌ Backend service is not running"
    fi
    
    # Check backend API endpoint
    local max_retries=30
    local retry_count=0
    
    while [ ${retry_count} -lt ${max_retries} ]; do
        if curl -f -s http://localhost:5000/health > /dev/null 2>&1; then
            success "✅ Backend API is responding"
            break
        else
            retry_count=$((retry_count + 1))
            log "⏳ Waiting for backend API... (${retry_count}/${max_retries})"
            sleep 2
        fi
    done
    
    if [ ${retry_count} -eq ${max_retries} ]; then
        error "❌ Backend API health check failed"
    fi
    
    # Check Nginx
    if systemctl is-active --quiet nginx; then
        success "✅ Nginx is running"
    else
        error "❌ Nginx is not running"
    fi
}

# Function to cleanup temporary files
cleanup() {
    log "🧹 Cleaning up temporary files..."
    
    rm -rf /tmp/backend-artifacts
    rm -rf /tmp/frontend-artifacts
    rm -f /tmp/deployment-*.tar.gz
    
    success "✅ Cleanup completed"
}

# Load environment variables
if [ -f "${APP_DIR}/.env.${ENVIRONMENT}" ]; then
    source ${APP_DIR}/.env.${ENVIRONMENT}
else
    warning "⚠️  Environment file not found: ${APP_DIR}/.env.${ENVIRONMENT}"
fi

# Main deployment process
main() {
    log "🎯 Starting deployment process..."
    
    backup_current_deployment
    setup_directories
    deploy_backend
    deploy_frontend
    health_check
    cleanup
    
    success "🎉 Deployment completed successfully for environment: ${ENVIRONMENT}"
    log "📊 Deployment summary:"
    log "   - Environment: ${ENVIRONMENT}"
    log "   - Backend service: ${SERVICE_NAME}"
    log "   - Application directory: ${APP_DIR}"
    log "   - Log file: ${LOG_FILE}"
}

# Error handling
trap 'error "❌ Deployment failed! Check logs at ${LOG_FILE}"' ERR

# Run main function
main "$@"
