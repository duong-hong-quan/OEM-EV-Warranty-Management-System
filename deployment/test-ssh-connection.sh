#!/bin/bash

# SSH Password Connection Test Script
# Usage: ./test-ssh-connection.sh

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}🔧 SSH Password Connection Test${NC}"
echo "=================================="

# Read connection details
read -p "Server Host: " SERVER_HOST
read -p "Username: " SERVER_USER
read -s -p "Password: " SERVER_PASSWORD
echo ""

# Test basic SSH connection
echo -e "${YELLOW}📡 Testing SSH connection...${NC}"

# Install sshpass if not available
if ! command -v sshpass &> /dev/null; then
    echo -e "${YELLOW}📦 Installing sshpass...${NC}"
    
    # Check OS and install accordingly
    if [[ "$OSTYPE" == "linux-gnu"* ]]; then
        sudo apt-get update && sudo apt-get install -y sshpass
    elif [[ "$OSTYPE" == "darwin"* ]]; then
        brew install sshpass
    else
        echo -e "${RED}❌ Please install sshpass manually${NC}"
        exit 1
    fi
fi

# Test SSH connection
echo -e "${BLUE}🔐 Testing SSH authentication...${NC}"
if sshpass -p "$SERVER_PASSWORD" ssh -o StrictHostKeyChecking=no -o ConnectTimeout=10 "$SERVER_USER@$SERVER_HOST" "echo 'SSH connection successful!'" 2>/dev/null; then
    echo -e "${GREEN}✅ SSH connection successful!${NC}"
else
    echo -e "${RED}❌ SSH connection failed!${NC}"
    echo -e "${YELLOW}💡 Please check:${NC}"
    echo "   1. Server host is correct and reachable"
    echo "   2. Username and password are correct"
    echo "   3. SSH password authentication is enabled on server"
    echo "   4. Firewall allows SSH connections"
    exit 1
fi

# Test LXC container access
echo -e "${BLUE}🐳 Testing LXC container access...${NC}"
if sshpass -p "$SERVER_PASSWORD" ssh -o StrictHostKeyChecking=no "$SERVER_USER@$SERVER_HOST" "lxc list demo-net-seminar" 2>/dev/null; then
    echo -e "${GREEN}✅ LXC container 'demo-net-seminar' is accessible!${NC}"
else
    echo -e "${YELLOW}⚠️  LXC container 'demo-net-seminar' not found or not accessible${NC}"
    echo -e "${BLUE}📋 Available LXC containers:${NC}"
    sshpass -p "$SERVER_PASSWORD" ssh -o StrictHostKeyChecking=no "$SERVER_USER@$SERVER_HOST" "lxc list" 2>/dev/null || echo "   No LXC containers found or no access"
fi

# Test deployment directory
echo -e "${BLUE}📁 Testing deployment directory...${NC}"
if sshpass -p "$SERVER_PASSWORD" ssh -o StrictHostKeyChecking=no "$SERVER_USER@$SERVER_HOST" "lxc exec demo-net-seminar -- test -d /opt/warranty-system" 2>/dev/null; then
    echo -e "${GREEN}✅ Deployment directory exists in container${NC}"
else
    echo -e "${YELLOW}⚠️  Deployment directory does not exist${NC}"
    echo -e "${BLUE}🔧 Creating deployment directory...${NC}"
    sshpass -p "$SERVER_PASSWORD" ssh -o StrictHostKeyChecking=no "$SERVER_USER@$SERVER_HOST" "lxc exec demo-net-seminar -- mkdir -p /opt/warranty-system" 2>/dev/null && echo -e "${GREEN}✅ Directory created${NC}" || echo -e "${RED}❌ Failed to create directory${NC}"
fi

# Test sudo access in container
echo -e "${BLUE}👤 Testing sudo access in container...${NC}"
if sshpass -p "$SERVER_PASSWORD" ssh -o StrictHostKeyChecking=no "$SERVER_USER@$SERVER_HOST" "lxc exec demo-net-seminar -- sudo whoami" 2>/dev/null | grep -q "root"; then
    echo -e "${GREEN}✅ Sudo access available in container${NC}"
else
    echo -e "${YELLOW}⚠️  Sudo access may not be available in container${NC}"
fi

echo ""
echo -e "${GREEN}🎉 Connection test completed!${NC}"
echo ""
echo -e "${BLUE}📝 GitHub Secrets to add:${NC}"
echo "SERVER_HOST=$SERVER_HOST"
echo "SERVER_USER=$SERVER_USER"
echo "SERVER_PASSWORD=<your-password>"
echo ""
echo -e "${BLUE}📋 Next steps:${NC}"
echo "1. Add the above secrets to your GitHub repository"
echo "2. Ensure deployment scripts are uploaded to /opt/warranty-system"
echo "3. Test deployment with: git push origin main"
