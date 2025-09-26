# 🚀 Electric Vehicle Warranty System - CI/CD Pipeline

## 📋 Tổng quan

Hệ thống CI/CD tự động cho ứng dụng Electric Vehicle Warranty System sử dụng GitHub Actions để build, test, và deploy lên server qua SSH và LXC container.

## 🏗️ Kiến trúc CI/CD

```
GitHub Repository
     ↓
GitHub Actions (CI/CD)
     ↓
Build & Test (Backend + Frontend)
     ↓
Security Scan
     ↓
Deploy via SSH
     ↓
LXC Container (demo-net-seminar)
     ↓
Application Running
```

## 🔄 Workflow Process

### 1. **Continuous Integration (CI)**
- ✅ Build Backend (.NET 8)
- ✅ Build Frontend (React + Vite)
- ✅ Run Tests
- ✅ Security Scan
- ✅ Create Artifacts

### 2. **Continuous Deployment (CD)**
- ✅ SSH vào server
- ✅ Chạy `lxc exec demo-net-seminar -- bash`
- ✅ Execute `sudo bash deploy.sh`
- ✅ Health Check
- ✅ Rollback nếu fail

## 🚀 Deployment Commands

### Automatic (via GitHub Actions):
```bash
# Push to develop branch → Deploy to dev environment
git push origin develop

# Push to main branch → Deploy to production
git push origin main
```

### Manual (on server):
```bash
# SSH vào server
ssh user@your-server

# Vào LXC container  
lxc exec demo-net-seminar -- bash

# Deploy production
sudo bash deploy.sh production

# Deploy development
sudo bash deploy.sh development
```

## 📁 File Structure

```
.github/
└── workflows/
    └── ci-cd.yml           # GitHub Actions workflow

deployment/
├── deploy.sh              # Main deployment script
├── backup.sh              # Backup script
├── restore.sh             # Restore script
├── docker-compose.yml     # Local development
├── .env.development       # Dev environment vars
├── .env.production        # Prod environment vars
└── DEPLOYMENT_GUIDE.md    # Detailed guide

be/
└── Dockerfile             # Backend container

fe/
├── Dockerfile             # Frontend container
└── nginx.conf             # Nginx config for frontend
```

## 🔧 Setup Instructions

### 1. GitHub Secrets Configuration
```bash
# Vào GitHub Repository → Settings → Secrets and variables → Actions
# Thêm các secrets sau:

SERVER_HOST=your-server-ip.com
SERVER_USER=ubuntu  
SERVER_PASSWORD=your-server-password
```

### 2. Server Setup
```bash
# Tạo LXC container
lxc launch ubuntu:22.04 demo-net-seminar

# Cài đặt dependencies
lxc exec demo-net-seminar -- apt update
lxc exec demo-net-seminar -- apt install -y nginx dotnet-runtime-8.0

# Tạo thư mục app
lxc exec demo-net-seminar -- mkdir -p /opt/warranty-system
```

### 3. Deploy Scripts Setup
```bash
# Copy deployment files
lxc file push deployment/ demo-net-seminar/opt/warranty-system/ -r

# Set permissions
lxc exec demo-net-seminar -- chmod +x /opt/warranty-system/deploy.sh
lxc exec demo-net-seminar -- chmod +x /opt/warranty-system/backup.sh
lxc exec demo-net-seminar -- chmod +x /opt/warranty-system/restore.sh
```

## 🌍 Environments

### Development Environment
- **Branch**: `develop`
- **URL**: `dev.your-domain.com`
- **Auto Deploy**: ✅ On push
- **SSL**: Optional

### Production Environment  
- **Branch**: `main`
- **URL**: `your-domain.com`
- **Auto Deploy**: ✅ On push (with approval)
- **SSL**: Required

## 📊 Monitoring & Logging

### Application Logs:
```bash
# Backend API logs
journalctl -u warranty-api -f

# Nginx logs
tail -f /var/log/nginx/access.log

# Deployment logs
tail -f /var/log/warranty-system-deploy.log
```

### Health Checks:
```bash
# API health
curl http://localhost:5000/health

# Service status
systemctl status warranty-api nginx
```

## 🔄 Backup & Restore

### Create Backup:
```bash
sudo bash backup.sh backup_name
```

### List Backups:
```bash
ls -la /opt/backups/warranty-system/
```

### Restore:
```bash
sudo bash restore.sh backup_name
```

## 🛠️ Local Development

### Using Docker Compose:
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### Direct Development:
```bash
# Backend
cd be/BE.API
dotnet run

# Frontend  
cd fe
npm run dev
```

## 🚨 Troubleshooting

### Common Issues:

#### 1. **SSH Connection Failed**
```bash
# Test SSH password connection
ssh user@server

# Debug connection
ssh -v user@server

# Check password authentication is enabled
sudo grep PasswordAuthentication /etc/ssh/sshd_config
```

#### 2. **LXC Container Issues**
```bash
# Check container
lxc list

# Restart container
lxc restart demo-net-seminar
```

#### 3. **Deployment Failed**
```bash
# Check logs
tail -f /var/log/warranty-system-deploy.log

# Manual deployment
sudo bash deploy.sh production
```

#### 4. **Service Not Starting**
```bash
# Check service status
systemctl status warranty-api

# View service logs  
journalctl -u warranty-api -f

# Restart service
systemctl restart warranty-api
```

## 📈 Performance Optimization

### Backend:
- ✅ .NET 8 optimizations
- ✅ Database connection pooling
- ✅ JWT token caching
- ✅ Response compression

### Frontend:
- ✅ React production build
- ✅ Asset optimization
- ✅ Code splitting
- ✅ CDN ready

### Infrastructure:
- ✅ Nginx reverse proxy
- ✅ SSL/TLS termination
- ✅ Static file caching
- ✅ Gzip compression

## 🔐 Security Features

### Application Security:
- ✅ JWT authentication
- ✅ Role-based authorization
- ✅ Input validation
- ✅ SQL injection protection

### Infrastructure Security:
- ✅ SSH key authentication
- ✅ Firewall configuration
- ✅ SSL certificates
- ✅ Security headers

## 📞 Support

### Deployment Issues:
1. Check GitHub Actions logs
2. SSH vào server kiểm tra
3. Xem application logs
4. Contact DevOps team

### Application Issues:
1. Check service status
2. Review application logs  
3. Verify database connection
4. Contact development team

---

## 🎯 Quick Commands Reference

```bash
# Deploy to production
git push origin main

# SSH to server
ssh user@server

# Enter LXC container
lxc exec demo-net-seminar -- bash

# Deploy manually
sudo bash deploy.sh production

# Check status
systemctl status warranty-api nginx

# View logs
journalctl -u warranty-api -f

# Create backup
sudo bash backup.sh

# Restore backup
sudo bash restore.sh backup_name
```
