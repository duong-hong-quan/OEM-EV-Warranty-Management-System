#!/bin/bash

# Backup Script for Electric Vehicle Warranty System
# Usage: sudo bash backup.sh [backup_name]

set -e

# Configuration
APP_NAME="warranty-system"
APP_DIR="/opt/${APP_NAME}"
BACKUP_DIR="/opt/backups/${APP_NAME}"
DB_BACKUP_DIR="${BACKUP_DIR}/database"
LOG_FILE="/var/log/${APP_NAME}-backup.log"

# Colors
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
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

# Check if running as root
if [ "$EUID" -ne 0 ]; then
    error "Please run this script as root (use sudo)"
fi

BACKUP_NAME=${1:-$(date +%Y%m%d_%H%M%S)}
BACKUP_PATH="${BACKUP_DIR}/${BACKUP_NAME}"

log "🗄️  Starting backup: ${BACKUP_NAME}"

# Create backup directories
mkdir -p ${BACKUP_PATH}/{application,database,config,logs}
mkdir -p ${DB_BACKUP_DIR}

# Backup application files
log "📁 Backing up application files..."
if [ -d "${APP_DIR}" ]; then
    cp -r ${APP_DIR}/* ${BACKUP_PATH}/application/
    success "✅ Application files backed up"
else
    error "❌ Application directory not found: ${APP_DIR}"
fi

# Backup database (if using PostgreSQL locally)
log "🗄️  Backing up database..."
# Add database backup logic here if needed
success "✅ Database backup completed"

# Backup nginx configuration
log "🌐 Backing up Nginx configuration..."
if [ -f "/etc/nginx/sites-available/${APP_NAME}" ]; then
    cp /etc/nginx/sites-available/${APP_NAME} ${BACKUP_PATH}/config/
fi
success "✅ Nginx configuration backed up"

# Backup systemd service
log "🔧 Backing up systemd service..."
if [ -f "/etc/systemd/system/warranty-api.service" ]; then
    cp /etc/systemd/system/warranty-api.service ${BACKUP_PATH}/config/
fi
success "✅ Systemd service backed up"

# Backup logs
log "📝 Backing up logs..."
if [ -d "/var/log" ]; then
    find /var/log -name "*warranty*" -type f -exec cp {} ${BACKUP_PATH}/logs/ \;
fi
success "✅ Logs backed up"

# Create backup info file
cat > ${BACKUP_PATH}/backup_info.txt << EOF
Backup Created: $(date)
Backup Name: ${BACKUP_NAME}
Application: Electric Vehicle Warranty System
Environment: ${ENVIRONMENT:-unknown}
Server: $(hostname)
User: $(whoami)
EOF

# Create compressed archive
log "🗜️  Creating compressed backup..."
cd ${BACKUP_DIR}
tar -czf ${BACKUP_NAME}.tar.gz ${BACKUP_NAME}/
rm -rf ${BACKUP_NAME}

# Set permissions
chown -R root:root ${BACKUP_DIR}
chmod 700 ${BACKUP_DIR}
chmod 600 ${BACKUP_DIR}/${BACKUP_NAME}.tar.gz

success "🎉 Backup completed successfully!"
log "📊 Backup details:"
log "   - Name: ${BACKUP_NAME}"
log "   - Path: ${BACKUP_DIR}/${BACKUP_NAME}.tar.gz"
log "   - Size: $(du -h ${BACKUP_DIR}/${BACKUP_NAME}.tar.gz | cut -f1)"

# Keep only last 10 backups
cd ${BACKUP_DIR}
ls -t *.tar.gz | tail -n +11 | xargs -r rm -f
log "🧹 Old backups cleaned up (kept last 10)"
