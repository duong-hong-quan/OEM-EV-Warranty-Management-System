#!/bin/bash

# Restore Script for Electric Vehicle Warranty System
# Usage: sudo bash restore.sh [backup_name]

set -e

# Configuration
APP_NAME="warranty-system"
APP_DIR="/opt/${APP_NAME}"
BACKUP_DIR="/opt/backups/${APP_NAME}"
LOG_FILE="/var/log/${APP_NAME}-restore.log"
SERVICE_NAME="warranty-api"

# Colors
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

# Logging function
log() {
    echo -e "${BLUE}[$(date '+%Y-%m-%d %H:%M:%S')]${NC} $1" | tee -a ${LOG_FILE}
}

success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1" | tee -a ${LOG_FILE}
}

error() {
    echo -e "${RED}[ERROR]${NC} $1" | tee -a ${LOG_FILE}
    exit 1
}

warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1" | tee -a ${LOG_FILE}
}

# Check if running as root
if [ "$EUID" -ne 0 ]; then
    error "Please run this script as root (use sudo)"
fi

# Function to list available backups
list_backups() {
    echo "📋 Available backups:"
    cd ${BACKUP_DIR}
    ls -la *.tar.gz 2>/dev/null | awk '{print "   " $9 " (" $5 " bytes, " $6 " " $7 " " $8 ")"}' || echo "   No backups found"
}

# Get backup name
BACKUP_NAME=$1

if [ -z "${BACKUP_NAME}" ]; then
    list_backups
    echo ""
    read -p "Enter backup name (without .tar.gz): " BACKUP_NAME
fi

if [ -z "${BACKUP_NAME}" ]; then
    error "❌ Backup name is required"
fi

BACKUP_FILE="${BACKUP_DIR}/${BACKUP_NAME}.tar.gz"

# Check if backup exists
if [ ! -f "${BACKUP_FILE}" ]; then
    error "❌ Backup file not found: ${BACKUP_FILE}"
fi

log "🔄 Starting restore from backup: ${BACKUP_NAME}"

# Show backup info if available
TEMP_DIR="/tmp/restore_${BACKUP_NAME}"
mkdir -p ${TEMP_DIR}
cd ${TEMP_DIR}
tar -xzf ${BACKUP_FILE}

if [ -f "${TEMP_DIR}/${BACKUP_NAME}/backup_info.txt" ]; then
    log "📊 Backup information:"
    cat ${TEMP_DIR}/${BACKUP_NAME}/backup_info.txt | while read line; do
        log "   ${line}"
    done
fi

# Confirm restore
echo ""
echo -e "${YELLOW}⚠️  WARNING: This will restore the application to the backed up state.${NC}"
echo -e "${YELLOW}⚠️  Current application data will be backed up before restore.${NC}"
echo ""
read -p "Do you want to continue? (y/N): " -n 1 -r
echo ""

if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    log "❌ Restore cancelled by user"
    rm -rf ${TEMP_DIR}
    exit 1
fi

# Create backup of current state before restore
log "📦 Creating backup of current state..."
CURRENT_BACKUP_NAME="pre_restore_$(date +%Y%m%d_%H%M%S)"
sudo bash backup.sh ${CURRENT_BACKUP_NAME} || warning "⚠️  Failed to create current backup"

# Stop services
log "🛑 Stopping services..."
if systemctl is-active --quiet ${SERVICE_NAME}; then
    systemctl stop ${SERVICE_NAME}
    success "✅ Service stopped: ${SERVICE_NAME}"
fi

if systemctl is-active --quiet nginx; then
    systemctl stop nginx
    success "✅ Nginx stopped"
fi

# Restore application files
log "📁 Restoring application files..."
if [ -d "${TEMP_DIR}/${BACKUP_NAME}/application" ]; then
    # Backup current application
    if [ -d "${APP_DIR}" ]; then
        mv ${APP_DIR} ${APP_DIR}.old.$(date +%Y%m%d_%H%M%S)
    fi
    
    # Restore application
    mkdir -p ${APP_DIR}
    cp -r ${TEMP_DIR}/${BACKUP_NAME}/application/* ${APP_DIR}/
    
    # Set permissions
    chown -R www-data:www-data ${APP_DIR}
    chmod -R 755 ${APP_DIR}
    chmod +x ${APP_DIR}/backend/BE.API 2>/dev/null || true
    
    success "✅ Application files restored"
else
    error "❌ Application files not found in backup"
fi

# Restore nginx configuration
log "🌐 Restoring Nginx configuration..."
if [ -f "${TEMP_DIR}/${BACKUP_NAME}/config/${APP_NAME}" ]; then
    cp ${TEMP_DIR}/${BACKUP_NAME}/config/${APP_NAME} /etc/nginx/sites-available/
    ln -sf /etc/nginx/sites-available/${APP_NAME} /etc/nginx/sites-enabled/
    success "✅ Nginx configuration restored"
fi

# Restore systemd service
log "🔧 Restoring systemd service..."
if [ -f "${TEMP_DIR}/${BACKUP_NAME}/config/warranty-api.service" ]; then
    cp ${TEMP_DIR}/${BACKUP_NAME}/config/warranty-api.service /etc/systemd/system/
    systemctl daemon-reload
    success "✅ Systemd service restored"
fi

# Test configurations
log "🧪 Testing configurations..."

# Test nginx config
if nginx -t; then
    success "✅ Nginx configuration is valid"
else
    error "❌ Nginx configuration is invalid"
fi

# Start services
log "🚀 Starting services..."

# Start nginx
systemctl start nginx
if systemctl is-active --quiet nginx; then
    success "✅ Nginx started"
else
    error "❌ Failed to start Nginx"
fi

# Start application service
systemctl start ${SERVICE_NAME}
sleep 10

if systemctl is-active --quiet ${SERVICE_NAME}; then
    success "✅ Application service started"
else
    warning "⚠️  Application service may have issues, check logs"
fi

# Health check
log "🏥 Performing health check..."
sleep 5

if curl -f -s http://localhost:5000/health > /dev/null 2>&1; then
    success "✅ Application is responding"
else
    warning "⚠️  Application health check failed"
fi

# Cleanup
log "🧹 Cleaning up temporary files..."
rm -rf ${TEMP_DIR}

success "🎉 Restore completed successfully!"
log "📊 Restore summary:"
log "   - Backup: ${BACKUP_NAME}"
log "   - Restore time: $(date)"
log "   - Services: nginx, ${SERVICE_NAME}"
log ""
log "📝 Next steps:"
log "   1. Check application logs: journalctl -u ${SERVICE_NAME} -f"
log "   2. Verify application is working: curl http://localhost:5000/health"
log "   3. Check nginx status: systemctl status nginx"
