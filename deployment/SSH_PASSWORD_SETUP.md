# 🔐 SSH Password Authentication - Quick Setup Guide

## 🚀 Quick Setup Commands

### 1. **Setup Server (Run on server)**
```bash
# Download and run server setup script
wget https://raw.githubusercontent.com/your-repo/setup-server.sh
sudo bash setup-server.sh
```

### 2. **Test SSH Connection (Run locally)**
```bash
# Download and run connection test
wget https://raw.githubusercontent.com/your-repo/test-ssh-connection.sh
chmod +x test-ssh-connection.sh
./test-ssh-connection.sh
```

### 3. **GitHub Secrets Setup**
```bash
# Go to: GitHub Repository → Settings → Secrets and variables → Actions
# Add these secrets:

Name: SERVER_HOST
Value: your-server-ip-address

Name: SERVER_USER  
Value: your-username

Name: SERVER_PASSWORD
Value: your-server-password
```

## 🔧 Manual Server Configuration

### Enable SSH Password Authentication:
```bash
# Edit SSH config
sudo nano /etc/ssh/sshd_config

# Ensure these lines:
PasswordAuthentication yes
PermitRootLogin no

# Restart SSH
sudo systemctl restart sshd
```

### Setup LXC Container:
```bash
# Install LXD
sudo apt install lxd

# Initialize LXD
sudo lxd init --auto

# Create container
lxc launch ubuntu:22.04 demo-net-seminar

# Install .NET in container
lxc exec demo-net-seminar -- bash -c "
  apt update &&
  apt install -y wget curl nginx &&
  wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb &&
  dpkg -i packages-microsoft-prod.deb &&
  apt update &&
  apt install -y dotnet-runtime-8.0
"
```

### Create Application Directory:
```bash
# In LXC container
lxc exec demo-net-seminar -- bash -c "
  mkdir -p /opt/warranty-system &&
  chown -R www-data:www-data /opt/warranty-system
"
```

## 🧪 Testing & Verification

### Test SSH Connection:
```bash
# Test basic connection
ssh username@server-ip

# Test with sshpass (like GitHub Actions)
sshpass -p "password" ssh -o StrictHostKeyChecking=no username@server-ip
```

### Test LXC Access:
```bash
# List containers
ssh username@server-ip "lxc list"

# Test container execution
ssh username@server-ip "lxc exec demo-net-seminar -- echo 'test'"
```

### Test Deployment Path:
```bash
# Check deployment directory
ssh username@server-ip "lxc exec demo-net-seminar -- ls -la /opt/warranty-system"
```

## 🚀 Deployment Commands

### Trigger Deployment:
```bash
# Development
git push origin develop

# Production  
git push origin main
```

### Manual Deployment:
```bash
# SSH to server
ssh username@server-ip

# Enter container
lxc exec demo-net-seminar -- bash

# Run deployment
cd /opt/warranty-system
sudo bash deploy.sh production
```

## 🔍 Troubleshooting

### SSH Connection Issues:
```bash
# Check SSH service
sudo systemctl status sshd

# Check SSH config
sudo grep PasswordAuthentication /etc/ssh/sshd_config

# Test connectivity
telnet server-ip 22
```

### LXC Container Issues:
```bash
# Check container status
lxc list

# Start container if stopped
lxc start demo-net-seminar

# Check container logs
lxc info demo-net-seminar --show-log
```

### GitHub Actions Debugging:
```bash
# Check workflow logs in GitHub Actions tab
# Common issues:
# - Wrong SERVER_HOST (check IP/domain)
# - Wrong SERVER_USER (check username)  
# - Wrong SERVER_PASSWORD (check password)
# - SSH not allowing password auth
# - LXC container not running
```

## 📊 Security Notes

### Password Authentication:
- ✅ Use strong passwords
- ✅ Disable root login
- ✅ Consider IP restrictions
- ✅ Monitor SSH logs
- ✅ Regular password rotation

### GitHub Secrets:
- ✅ Never commit passwords to code
- ✅ Use GitHub encrypted secrets
- ✅ Limit repository access
- ✅ Rotate secrets regularly

## 🎯 Quick Checklist

- [ ] Server has SSH password authentication enabled
- [ ] LXC container 'demo-net-seminar' exists and running
- [ ] .NET 8 runtime installed in container
- [ ] Application directory `/opt/warranty-system` exists
- [ ] Deployment scripts uploaded and executable
- [ ] GitHub secrets configured correctly
- [ ] SSH connection tested successfully
- [ ] LXC access tested successfully

## 📞 Need Help?

1. **Connection Issues**: Run `test-ssh-connection.sh`
2. **Server Setup**: Run `setup-server.sh`  
3. **GitHub Actions**: Check workflow logs
4. **Application Issues**: Check container logs with `lxc exec demo-net-seminar -- journalctl -f`
