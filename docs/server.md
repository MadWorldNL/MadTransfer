# Configure ubuntu server
### Clean Ubuntu install
Start a fresh install of ubuntu 24.04

### Latest updates
Download the latest updates:
```shell
sudo apt update
sudo apt upgrade
```

### Install tools
Install useful networking tools:
```shell
# Install net-tools to use ifconfig for checking your IP address
sudo apt install net-tools
```

### Install VPN provider
See [install guide](https://www.cyberciti.biz/faq/howto-setup-openvpn-server-on-ubuntu-linux-14-04-or-16-04-lts/) of OpenVPN

Copy your.ovpn profile to your local PC:
```shell
scp <source> <destination>
scp username@b:/path/to/file /path/to/destination
```

Now your can connect through vpn in your server.

### Configure Firewall
By default the firewall is already installed. Open these ports in the firewall:

```shell
sudo ufw allow from 10.8.0.0/24 to any port 22 # SSH
sudo ufw allow 80   # HTTP
sudo ufw allow 443  # HTTPS
sudo ufw allow 1194 # OpenVPN
sudo ufw allow from 10.8.0.0/24 to any port 10443 # Kubernetes Dashboard
```

Now enable the firewall:
```shell
sudo ufw enable
```

Or reload when firewall is already enabled:
```shell
sudo ufw reload
```

Show added ports:
```shell
sudo ufw show added
```

Connect with ssh over the IP of the VPN by:
Show added ports:
```shell
ssh username@10.8.0.1
```

### Install kubernetes
[See Install Guide](kubernetes.md#install-on-production)