## Install DotNet run time 6
1. Dotnet Core 6: Install using msdn guide (only run time. No need for SDK).
2. Install unzip in order to download and unzip the source code

## install nginx web server
nginx: https://hbhhathorn.medium.com/install-an-asp-net-core-web-api-on-linux-ubuntu-18-04-and-host-with-nginx-and-ssl-2ed9df7371fb
3. install it on ubuntu.
4. execute command to edit the below file
sudo vim /etc/nginx/sites-available/default

add the remaining lines to the file:
---
server {
    listen        80;
    server_name   vps39767.publiccloud.com.br;
    root /root/lupanh20/app;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
---
5. Create service file
sudo vim /etc/systemd/system/apilupanh20.service

add thec content below
---
[Unit]
Description=This is api for lupanh20

[Service]
WorkingDirectory=/root/lupanh20/api
ExecStart=dotnet /root/lupanh20/api/TE.BE.City.Api.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
---
6. Now enable and start service file
sudo systemctl enable apilupanh20.service
sudo systemctl start apilupanh20.service
sudo systemctl status apilupanh20.service

7. disable or add thengix paths to firewall.

## install mysql
mysql: https://www.digitalocean.com/community/tutorials/how-to-install-mysql-on-ubuntu-20-04
8. sudo apt update
9. sudo apt install mysql-server
10. sudo systemctl start mysql.service
11. sudo mysql_secure_installation
12. mysql -u root -p
