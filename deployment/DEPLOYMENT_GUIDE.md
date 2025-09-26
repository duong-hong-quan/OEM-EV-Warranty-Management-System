# GitHub Actions CI/CD Setup Guide

## 🔧 Prerequisites

### Server Requirements:
- Ubuntu/Debian server with Docker/LXC
- SSH access
- Nginx installed
- .NET 8 runtime installed in LXC container
- Node.js 18+ installed

### GitHub Repository Secrets:
Bạn cần thêm các secrets sau vào GitHub repository:

1. **SERVER_HOST** - IP hoặc domain của server
   ```
   your-server-ip.com
   ```

2. **SERVER_USER** - Username để SSH vào server
   ```
   ubuntu
   ```

3. **SERVER_PASSWORD** - Password để SSH vào server
   ```
   your-server-password
   ```

## 🚀 Setup Instructions

### 1. Thêm GitHub Secrets:
```bash
# Vào GitHub repository > Settings > Secrets and variables > Actions
# Thêm các secrets như trên
```

### 2. Chuẩn bị Server:

#### Chuẩn bị SSH Password Authentication:
```bash
# Đảm bảo SSH password authentication được enable trên server
# Kiểm tra file /etc/ssh/sshd_config
sudo nano /etc/ssh/sshd_config

# Đảm bảo các dòng sau được set:
# PasswordAuthentication yes
# PermitRootLogin no (khuyến nghị)

# Restart SSH service
sudo systemctl restart sshd
```

#### Setup LXC Container:
```bash
# Trên server
lxc launch ubuntu:22.04 demo-net-seminar

# Vào container
lxc exec demo-net-seminar -- bash

# Cài đặt dependencies trong container
apt update && apt upgrade -y
apt install -y nginx dotnet-runtime-8.0 curl

# Tạo thư mục ứng dụng
mkdir -p /opt/warranty-system
chown -R www-data:www-data /opt/warranty-system
```

#### Copy deployment files:
```bash
# Copy các file deployment vào container
lxc file push deployment/ demo-net-seminar/opt/warranty-system/ -r
lxc file push deployment/deploy.sh demo-net-seminar/opt/warranty-system/
```

### 3. Workflow Triggers:

#### Development Deployment:
- **Trigger**: Push to `develop` branch
- **Environment**: Development
- **URL**: `dev.your-domain.com`

#### Production Deployment:
- **Trigger**: Push to `main` branch  
- **Environment**: Production
- **URL**: `your-domain.com`

#### Manual Deployment:
```bash
# Trigger manual deployment via GitHub Actions
# Go to Actions tab > CI/CD Pipeline > Run workflow
```

## 📋 Deployment Process

### Automated Steps:
1. **Build & Test**: Compile backend và frontend
2. **Security Scan**: Kiểm tra lỗ hổng bảo mật
3. **Deploy**: Upload và deploy lên server
4. **Health Check**: Kiểm tra ứng dụng hoạt động

### Manual Steps on Server:
```bash
# SSH vào server
ssh user@your-server

# Vào LXC container
lxc exec demo-net-seminar -- bash

# Chạy deployment script
cd /opt/warranty-system
sudo bash deploy.sh production
```

## 🔍 Monitoring & Logs

### Check Deployment Status:
```bash
# Kiểm tra service status
systemctl status warranty-api

# Xem logs
journalctl -u warranty-api -f

# Kiểm tra nginx
nginx -t
systemctl status nginx
```

### Application Logs:
```bash
# Backend logs
tail -f /opt/warranty-system/logs/warranty-api.log

# Deployment logs  
tail -f /var/log/warranty-system-deploy.log
```

## 🛠️ Troubleshooting

### Common Issues:

#### 1. SSH Connection Failed:
```bash
# Test SSH password connection
ssh user@your-server

# Debug SSH connection
ssh -v user@your-server

# Check SSH configuration
sudo nano /etc/ssh/sshd_config
```

#### 2. LXC Container Issues:
```bash
# Kiểm tra container status
lxc list

# Restart container
lxc restart demo-net-seminar
```

#### 3. Database Connection:
```bash
# Test database connection
dotnet ef database update --dry-run
```

#### 4. Nginx Configuration:
```bash
# Test nginx config
nginx -t

# Reload nginx
systemctl reload nginx
```

## 🔄 Rollback Process

### Automatic Rollback:
- GitHub Actions sẽ tự động rollback nếu deployment fail
- Backup được tạo tự động trước mỗi lần deploy

### Manual Rollback:
```bash
# Xem danh sách backups
ls -la /opt/backups/warranty-system/

# Restore từ backup
sudo bash restore.sh 20241225_120000
```

## 📊 Environment Configuration

### Development Environment:
- **URL**: `dev.your-domain.com`
- **Database**: Development database
- **Debug**: Enabled
- **SSL**: Optional

### Production Environment:
- **URL**: `your-domain.com` 
- **Database**: Production database
- **Debug**: Disabled
- **SSL**: Required
- **Monitoring**: Enabled

## 🎯 Next Steps

1. ✅ Setup GitHub Secrets
2. ✅ Configure server and LXC container
3. ✅ Test SSH connection
4. ✅ Push code to trigger deployment
5. ✅ Monitor deployment process
6. ✅ Verify application is working

## 📞 Support

Nếu có vấn đề với deployment:
1. Check GitHub Actions logs
2. Check server logs
3. Verify environment configuration
4. Contact DevOps team
