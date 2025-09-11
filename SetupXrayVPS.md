# 1. Prepare VPS
1. Get a fresh VPS (Ubuntu 20.04/22.04 recommended).
2. Update packages:
```bash   
sudo apt update && sudo apt upgrade -y
```
3. Install basic tools:
```bash
sudo apt install curl wget unzip socat -y
```

# 2. Install Xray
The easiest way is via the official script:
```bash
bash <(curl -L https://github.com/XTLS/Xray-install/raw/main/install-release.sh) install
```
- This will place the binary at /usr/local/bin/xray
- Config at /usr/local/etc/xray/config.json
- Systemd service: xray.service
  
Check:
```bash
xray -version
```

# 3. Generate Keys

Reality transport needs a private–public key pair.  
For some reasons `xray x25519` doesn't work well, instead we using a python script, directly paste the command into bash and run. 
```py
python3 - <<'PY'
import os, base64
# X25519 uses 32-byte keys. We'll produce a valid private scalar and its public.
# We'll use the same derivation as WireGuard: clamp private -> X25519 public via scalar mult with base point.
# Requires Python 3.8+ with built-in X25519 from cryptography? Not available by default.
# So we implement a minimal libsodium-compatible call via pynacl if available; else fall back to os.urandom + wg pubkey style is easier.
# Simpler approach: write seed, call `wg pubkey` if available would defeat the no-tool goal.
# We'll use the stdlib 'cryptography' is not guaranteed; so here's a tiny fallback using subprocess if wg exists:
try:
    from cryptography.hazmat.primitives.asymmetric import x25519
    from cryptography.hazmat.primitives import serialization
    def b64url_nopad(b): return base64.urlsafe_b64encode(b).decode().rstrip("=")
    priv = x25519.X25519PrivateKey.generate()
    pub  = priv.public_key()
    # Raw 32-byte keys:
    priv_raw = priv.private_bytes(
        encoding=serialization.Encoding.Raw,
        format=serialization.PrivateFormat.Raw,
        encryption_algorithm=serialization.NoEncryption()
    )
    pub_raw = pub.public_bytes(
        encoding=serialization.Encoding.Raw,
        format=serialization.PublicFormat.Raw
    )
    print("Private:", b64url_nopad(priv_raw))
    print("Public :", b64url_nopad(pub_raw))
except Exception as e:
    print("This method needs the 'cryptography' package. Either:")
    print("  pip install cryptography")
    print("or use Method A (WireGuard) or Method C (sing-box).")
PY
```

You’ll get:
```
Private key: XXXXXXXXXXXXXXXXXX
Public key:  YYYYYYYYYYYYYYYYYY
```

Private key → used in server config  
Public key → given to client config

# 4. Configure Xray (Reality)

Edit `/usr/local/etc/xray/config.json` using `nano /usr/local/etc/xray/config.json`:
```
{
  "inbounds": [
    {
      "port": 443,
      "protocol": "vless",
      "settings": {
        "clients": [
          {
            "id": "UUID-GOES-HERE",
            "flow": "xtls-rprx-vision"
          }
        ],
        "decryption": "none"
      },
      "streamSettings": {
        "network": "tcp",
        "security": "reality",
        "realitySettings": {
          "show": false,
          "dest": "www.microsoft.com:443",
          "xver": 0,
          "serverNames": [
            "www.microsoft.com"
          ],
          "privateKey": "YOUR_PRIVATE_KEY",
          "shortIds": [
            ""
          ]
        }
      }
    }
  ],
  "outbounds": [
    {
      "protocol": "freedom"
    }
  ]
}
```
Notes:

- Replace UUID-GOES-HERE with a generated UUID:
```bash
cat /proc/sys/kernel/random/uuid
```
- dest & serverNames: choose a popular CDN/TLS site (e.g. www.microsoft.com, www.cloudflare.com).
- shortIds: leave [""] (empty string) or up to 16 hex characters.
- privateKey: from step 3.

# 5. Enable & Start Service
```bash
sudo systemctl enable xray
sudo systemctl restart xray
sudo systemctl status xray
```
If error, check logs:
```bash
journalctl -u xray -e
```

# 6. Configure Client (Example: Clash Meta / V2RayN)

Example client JSON for VLESS + Reality:
```
{
  "protocol": "vless",
  "tag": "reality",
  "settings": {
    "vnext": [
      {
        "address": "your.server.ip",
        "port": 443,
        "users": [
          {
            "id": "UUID-GOES-HERE",
            "encryption": "none",
            "flow": "xtls-rprx-vision"
          }
        ]
      }
    ]
  },
  "streamSettings": {
    "network": "tcp",
    "security": "reality",
    "realitySettings": {
      "fingerprint": "chrome",
      "serverName": "www.microsoft.com",
      "publicKey": "YOUR_PUBLIC_KEY",
      "shortId": "",
      "spiderX": "/"
    }
  }
}
```
Key client fields:
- address: your VPS IP / domain
- publicKey: from step 3
- serverName: must match serverNames in server config
- shortId: same as server’s shortId

Here is another client config example of Clash Mihomo
```
proxies:
- name: My-Vultr-3
  type: trojan
  server: t3.yuren527.com
  port: 443
  password: tmwfiawc
  sni: t3.yuren527.com
  skip-cert-verify: true
  udp: true
- name: "Reality-1"
  type: vless
  server: 139.180.193.197
  port: 443
  uuid: 34876234-4fd5-4084-b723-4300a7bdf776
  network: tcp
  udp: true
  tls: true
  flow: xtls-rprx-vision
  servername: www.microsoft.com      # <-- SNI must match serverNames
  client-fingerprint: chrome          # optional, but recommended
  reality-opts:
    public-key: ONhWkfHEdhRQdlh9yHcS8k9wzenfge0ljoP7W_u8bCE
    short-id: ""
    spider-x: "www.microsoft.com"
proxy-groups:
- name: PROXY
  type: load-balance
  strategy: round-robin
  proxies:
  - My-Vultr-3
  - Reality-1
rules:
- DOMAIN-SUFFIX,cn,DIRECT
- DOMAIN-SUFFIX,qq.com,DIRECT
- DOMAIN-SUFFIX,baidu.com,DIRECT
- GEOIP,CN,DIRECT
- MATCH,PROXY
ipv6: true
tun:
  enable: true
  stack: system      # or gvisor (depends on your OS/core)
  auto-route: true
  auto-detect-interface: true
```

# 7. Firewall & Ports
Ensure port 443 is open:
```
sudo ufw allow 443/tcp
sudo ufw reload
```

# 8. Test Connection

On the client side, try connecting. If it fails:
- Check VPS firewall
- Check journalctl -u xray -e
- Verify UUID, public key, serverName match exactly

# Note
It's better to using a domain instead of IP address, add a sub-domain pointing to the VPS, and set the Type to A for IPv4 or AAAA for IPv6.
