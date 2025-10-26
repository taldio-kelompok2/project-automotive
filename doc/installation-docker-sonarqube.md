# 🐋 Docker Basics - Khusus untuk SonarQube

## 🎯 Anda TIDAK Perlu Jadi Expert Docker!

Panduan ini cukup untuk menjalankan SonarQube. Hanya 5-10 menit belajar!

---

## 📚 Apa itu Docker? (30 detik penjelasan)

**Docker** = Aplikasi yang menjalankan software dalam "container" (seperti box/kotak virtual).

**Keuntungan:**
- ✅ Install sekali jalan (tidak ada "works on my machine")
- ✅ Tidak bentrok dengan aplikasi lain
- ✅ Mudah hapus kalau tidak pakai lagi
- ✅ Setup database otomatis

**Analogi Sederhana:**
- Docker seperti **microwave** 🍱
- Container seperti **makanan dalam kotak**
- Anda tinggal tekan tombol, jadi!

---

## 🚀 Install Docker (5 menit)

### **macOS:**
1. Download [Docker Desktop for Mac](https://www.docker.com/products/docker-desktop)
2. Install seperti aplikasi biasa (drag & drop)
3. Buka Docker Desktop
4. Tunggu sampai icon whale muncul di menu bar atas

### **Windows:**
1. Download [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop)
2. Install dan restart komputer
3. Buka Docker Desktop
4. Pastikan WSL2 sudah enable (biasanya otomatis)

### **Linux (Ubuntu/Debian):**
```bash
sudo apt update
sudo apt install docker.io docker-compose
sudo systemctl start docker
sudo usermod -aG docker $USER
# Logout dan login lagi
```

### **Verifikasi Instalasi:**
```bash
docker --version
# Output: Docker version 24.x.x

docker-compose --version
# Output: Docker Compose version 2.x.x
```

✅ Kalau muncul versi, berarti sudah berhasil!

---

## 🎓 5 Command Docker Yang Perlu Anda Tahu

Hanya 5 command ini yang akan Anda pakai untuk SonarQube:

### 1️⃣ **Start Container** (Jalankan SonarQube)
```bash
docker-compose up -d
```
- `up` = jalankan
- `-d` = jalankan di background (detached mode)

**Kapan pakai:** Setiap kali mau nyalain SonarQube

---

### 2️⃣ **Stop Container** (Matikan SonarQube)
```bash
docker-compose down
```
- Mematikan dan hapus container (tapi data tetap ada)

**Kapan pakai:** Selesai kerja, mau matikan SonarQube

---

### 3️⃣ **Lihat Status** (Apakah SonarQube jalan?)
```bash
docker-compose ps
```
**Output kalau jalan:**
```
NAME         STATUS    PORTS
sonarqube    Up        0.0.0.0:9000->9000/tcp
sonardb      Up        5432/tcp
```

**Kapan pakai:** Untuk cek apakah SonarQube sudah nyala

---

### 4️⃣ **Lihat Log** (Troubleshooting)
```bash
docker-compose logs -f sonarqube
```
- `-f` = follow (terus update real-time)
- Tekan `Ctrl+C` untuk keluar

**Kapan pakai:** Kalau SonarQube error atau lambat startup

---

### 5️⃣ **Restart Container** (Kalau ada masalah)
```bash
docker-compose restart
```

**Kapan pakai:** Kalau SonarQube hang atau bermasalah

---

## 🔧 File Yang Anda Butuhkan

### **docker-compose.yml** (Copy-paste saja!)

Buat file ini di root project Anda:

```yaml
version: '3.7'

services:
  sonarqube:
    image: sonarqube:community
    container_name: sonarqube
    ports:
      - "9000:9000"
    environment:
      - SONAR_JDBC_URL=jdbc:postgresql://db:5432/sonar
      - SONAR_JDBC_USERNAME=sonar
      - SONAR_JDBC_PASSWORD=sonar
    depends_on:
      - db
    volumes:
      - sonarqube_data:/opt/sonarqube/data
      - sonarqube_extensions:/opt/sonarqube/extensions
      - sonarqube_logs:/opt/sonarqube/logs
    restart: unless-stopped
    mem_limit: 4g

  db:
    image: postgres:15
    container_name: sonardb
    environment:
      - POSTGRES_USER=sonar
      - POSTGRES_PASSWORD=sonar
      - POSTGRES_DB=sonar
    volumes:
      - postgresql_data:/var/lib/postgresql/data
    restart: unless-stopped

volumes:
  sonarqube_data:
  sonarqube_extensions:
  sonarqube_logs:
  postgresql_data:
```

**Penjelasan singkat:**
- `image: sonarqube:community` → Download SonarQube versi gratis
- `ports: 9000:9000` → Akses lewat http://localhost:9000
- `db: postgres:15` → Database otomatis setup
- `volumes:` → Data disimpan permanen (tidak hilang kalau restart)
- `mem_limit: 4g` → Batasi RAM maksimal 4GB

---

## 📝 Step-by-Step: Jalankan SonarQube

### **Langkah 1: Siapkan File**
```bash
cd /Users/krisnafirdaus/Desktop/default-project
# Pastikan ada file docker-compose.yml di sini
ls docker-compose.yml
```

### **Langkah 2: Start SonarQube**
```bash
docker-compose up -d
```

**Output yang diharapkan:**
```
[+] Running 2/2
 ✔ Container sonardb     Started
 ✔ Container sonarqube   Started
```

### **Langkah 3: Tunggu Startup (2-3 menit)**
```bash
# Cara 1: Cek status
docker-compose ps

# Cara 2: Lihat log sampai muncul "SonarQube is operational"
docker-compose logs -f sonarqube
```

**Tunggu sampai muncul:**
```
SonarQube is operational
```

### **Langkah 4: Akses Dashboard**
1. Buka browser
2. Ketik: `http://localhost:9000`
3. Login dengan:
   - Username: `admin`
   - Password: `admin`
4. Ganti password (wajib di first login)

### **Langkah 5: Setup Token**
1. Di dashboard, klik nama Anda (kanan atas)
2. Pilih **My Account** → **Security**
3. Klik **Generate Token**
4. Nama: `MyApp`
5. Klik **Generate**
6. **COPY TOKEN** (hanya muncul 1x!)
7. Paste ke `sonar-scan.sh` (ganti `your-token-here`)

---

## 🎯 Workflow Sehari-hari

### **Pagi - Mulai Kerja:**
```bash
cd /Users/krisnafirdaus/Desktop/default-project
docker-compose up -d
# Tunggu 2 menit
# Buka http://localhost:9000
```

### **Analisis Kode:**
```bash
./sonar-scan.sh
```

### **Sore - Selesai Kerja:**
```bash
docker-compose down
```

---

## 🔧 Troubleshooting

### **Problem 1: Port 9000 sudah dipakai**
**Error:**
```
Error: bind: address already in use
```

**Solusi:**
```bash
# Cari siapa yang pakai port 9000
lsof -i :9000

# Kill process tersebut
kill -9 <PID>

# Atau ganti port di docker-compose.yml
# Ubah "9000:9000" jadi "9001:9000"
```

---

### **Problem 2: Docker tidak jalan**
**Error:**
```
Cannot connect to Docker daemon
```

**Solusi:**
```bash
# macOS/Windows: Buka Docker Desktop dulu!

# Linux: Start docker service
sudo systemctl start docker
```

---

### **Problem 3: SonarQube lambat startup**
**Solusi:**
```bash
# Lihat log untuk cek progress
docker-compose logs -f sonarqube

# Biasanya karena:
# - RAM kurang (butuh minimal 4GB)
# - Disk penuh
# - Database belum ready
```

---

### **Problem 4: Lupa token**
**Solusi:**
```bash
# Generate token baru di dashboard:
# http://localhost:9000 → My Account → Security → Generate Token
```

---

### **Problem 5: Data hilang setelah restart**
**Solusi:**
```bash
# Pastikan pakai volumes di docker-compose.yml
# Jangan pakai 'docker-compose down -v' (ini hapus data!)
# Pakai 'docker-compose down' saja
```

---

## 📊 Monitoring

### **Cek Resource Usage:**
```bash
docker stats
```

**Output:**
```
NAME       CPU %   MEM USAGE / LIMIT   
sonarqube  15%     2.5GB / 4GB
sonardb    5%      150MB / unlimited
```

### **Lihat Disk Space:**
```bash
docker system df
```

---

## 🧹 Cleanup (Kalau Tidak Pakai Lagi)

### **Hapus Container (Data Tetap Ada):**
```bash
docker-compose down
```

### **Hapus Container + Data:**
```bash
docker-compose down -v
# WARNING: Ini hapus semua data SonarQube!
```

### **Hapus Image (Hemat Disk):**
```bash
docker images
docker rmi sonarqube:community
docker rmi postgres:15
```

---

## 🎓 Konsep Penting

### **Container vs Image**
- **Image** = Template/cetakan (seperti file installer)
- **Container** = Running instance (seperti aplikasi yang jalan)

Analogi:
- Image = Resep kue 📄
- Container = Kue yang sudah jadi 🍰

### **Volumes**
- Tempat simpan data permanen
- Data tidak hilang walau container dihapus
- Seperti external hard disk untuk container

### **Network**
- Container bisa komunikasi satu sama lain
- SonarQube container bisa connect ke PostgreSQL container
- Otomatis di-setup oleh Docker Compose

---

## ✅ Checklist: Apakah Anda Sudah Siap?

- [ ] Docker Desktop terinstall
- [ ] `docker --version` menampilkan versi
- [ ] `docker-compose --version` menampilkan versi
- [ ] File `docker-compose.yml` sudah ada
- [ ] `docker-compose up -d` berhasil jalan
- [ ] `http://localhost:9000` bisa diakses
- [ ] Login admin berhasil
- [ ] Token sudah di-generate

✅ Kalau semua checklist ✓, Anda siap analisis kode dengan SonarQube!

---

## 🚀 Next Steps

Setelah SonarQube jalan:
1. Generate token di dashboard
2. Update token di `sonar-scan.sh`
3. Run: `./sonar-scan.sh`
4. Lihat hasil analisis di dashboard

---

## 📚 Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Docs](https://docs.docker.com/compose/)
- [SonarQube Docker Image](https://hub.docker.com/_/sonarqube)

---

## 💡 Tips Pro

1. **Jangan hapus volumes** kalau mau keep data
2. **Restart Docker Desktop** kalau ada masalah aneh
3. **Tutup aplikasi lain** kalau RAM kurang
4. **Gunakan SSD** untuk performa lebih baik
5. **Backup token** di tempat aman (password manager)

---

## 🎉 Kesimpulan

**Yang Perlu Anda Kuasai:**
- ✅ `docker-compose up -d` → Start
- ✅ `docker-compose down` → Stop
- ✅ `docker-compose ps` → Status
- ✅ `docker-compose logs -f` → Lihat log

**Hanya 4 command!** Sisanya akan otomatis. 🚀

**Waktu belajar:** 10 menit  
**Waktu setup:** 5 menit  
**Complexity:** ⭐⭐☆☆☆ (Mudah!)

---

**Selamat! Anda sekarang bisa jalankan SonarQube dengan Docker! 🎉**


=========================================================================================================


# 🔑 Cara Mendapatkan Token SonarQube (100% Gratis!)

## ✅ Token SonarQube itu GRATIS dan Generate Sendiri!

**PENTING:** Token BUKAN dari website atau bayar. Anda generate sendiri dari SonarQube yang sudah Anda install di localhost!

---

## 🚀 Step-by-Step: Generate Token (3 Menit)

### **Prerequisite:**
SonarQube harus sudah running. Kalau belum:
```bash
cd /Users/krisnafirdaus/Desktop/default-project
docker-compose up -d
# Tunggu 2-3 menit
```

---

### **Step 1: Akses Dashboard** 🌐

1. Buka browser
2. Ketik: **http://localhost:9000**
3. Tunggu sampai halaman login muncul

![SonarQube Login](https://via.placeholder.com/800x400?text=SonarQube+Login+Page)

---

### **Step 2: Login Pertama Kali** 🔐

**Default Credentials:**
- **Username:** `admin`
- **Password:** `admin`

Klik **Log in**

![Login Form](https://via.placeholder.com/600x300?text=Username:+admin+Password:+admin)

---

### **Step 3: Ganti Password (Wajib First Time)** 🔒

Setelah login pertama, SonarQube akan paksa Anda ganti password.

**Form yang muncul:**
- Old password: `admin`
- New password: `[password baru Anda]` (minimal 8 karakter)
- Confirm password: `[ulangi password baru]`

Klik **Update**

**💡 TIP:** Simpan password di password manager atau notepad!

---

### **Step 4: Masuk ke Account Settings** ⚙️

Setelah login berhasil:

1. Klik **icon profile Anda** (pojok kanan atas)
2. Pilih **My Account**

![My Account](https://via.placeholder.com/400x200?text=Click+Profile+→+My+Account)

---

### **Step 5: Buka Tab Security** 🔐

1. Di halaman My Account
2. Klik tab **Security**
3. Anda akan lihat section "Generate Tokens"

![Security Tab](https://via.placeholder.com/800x400?text=Security+Tab+→+Generate+Tokens)

---

### **Step 6: Generate Token** 🎟️

**Form Generate Token:**

**Name:** `MyApp` (atau nama project Anda)

**Type:** Pilih salah satu:
- ✅ **User Token** (recommended untuk development)
- Project Analysis Token (untuk specific project)
- Global Analysis Token (untuk multiple projects)

**Expires in:** Pilih durasi:
- ✅ **No expiration** (recommended untuk local dev)
- 30 days
- 90 days
- 1 year

Klik **Generate**

![Generate Token Form](https://via.placeholder.com/700x300?text=Name:+MyApp+|+Type:+User+Token+|+Generate)

---

### **Step 7: COPY TOKEN! (PENTING!)** 📋

Setelah klik Generate, muncul popup dengan TOKEN.

**⚠️ SUPER PENTING:**
- Token hanya muncul **SEKALI**
- Kalau window ditutup, token hilang selamanya
- Harus generate ulang kalau lupa

**TOKEN terlihat seperti:**
```
squ_1234567890abcdefghijklmnopqrstuvwxyz123456
```

**Langsung COPY token tersebut!**

![Token Generated](https://via.placeholder.com/800x200?text=Token+Generated!+COPY+NOW!)

---

### **Step 8: Paste Token ke Script** 📝

Buka file `sonar-scan.sh` dan edit baris ini:

**SEBELUM:**
```bash
SONAR_TOKEN="your-token-here"  # ← Ganti ini!
```

**SESUDAH:**
```bash
SONAR_TOKEN="squ_1234567890abcdefghijklmnopqrstuvwxyz123456"
```

**Untuk Windows (sonar-scan.ps1):**
```powershell
$SONAR_TOKEN = "squ_1234567890abcdefghijklmnopqrstuvwxyz123456"
```

**Save file!** 💾

---

## ✅ Verifikasi Token Bekerja

Test dengan run analysis:

```bash
cd /Users/krisnafirdaus/Desktop/default-project
./sonar-scan.sh
```

**Kalau token benar, akan muncul:**
```
✅ Analysis success
✅ View results at: http://localhost:9000
```

**Kalau token salah, akan muncul:**
```
❌ Error: Unauthorized (401)
❌ Invalid authentication token
```

---

## 🔄 Generate Token Baru (Kalau Lupa/Hilang)

**JANGAN PANIK!** Token bisa di-generate ulang kapan saja.

### **Cara 1: Generate Token Baru**

1. Login ke http://localhost:9000
2. My Account → Security
3. Generate token baru dengan nama berbeda (misal: `MyApp-2`)
4. Copy token baru
5. Update di `sonar-scan.sh`

### **Cara 2: Revoke Token Lama (Opsional)**

Kalau mau hapus token lama:

1. My Account → Security
2. Lihat list "Existing tokens"
3. Klik **Revoke** di token yang mau dihapus
4. Token tersebut tidak bisa dipakai lagi

![Revoke Token](https://via.placeholder.com/700x200?text=Existing+Tokens+→+Revoke)

---

## 🎯 Best Practices untuk Token

### ✅ DO:
- Generate 1 token per project
- Simpan di environment variable atau secret manager
- Beri nama descriptive (misal: `MyApp-Development`)
- Set expiration untuk production tokens
- Revoke token yang tidak dipakai

### ❌ DON'T:
- Commit token ke Git! (Add ke `.gitignore`)
- Share token di public (Slack, email)
- Gunakan 1 token untuk semua project
- Biarkan expired tokens aktif

---

## 🔐 Keamanan Token

### **Local Development (Aman):**
```bash
# Di sonar-scan.sh (local)
SONAR_TOKEN="squ_xxxxx"  # ✅ OK untuk local
```

### **Production/CI-CD (Lebih Aman):**

**GitHub Actions:**
```yaml
# Simpan token di GitHub Secrets
env:
  SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
```

**GitLab CI:**
```yaml
# Simpan token di GitLab CI/CD Variables
variables:
  SONAR_TOKEN: $SONAR_TOKEN
```

**Azure DevOps:**
```yaml
# Simpan token di Azure Pipeline Variables (secured)
variables:
  - name: SONAR_TOKEN
    value: $(sonarToken)
```

**Environment Variable (macOS/Linux):**
```bash
# Di ~/.zshrc atau ~/.bashrc
export SONAR_TOKEN="squ_xxxxx"

# Di script
SONAR_TOKEN="${SONAR_TOKEN}"
```

---

## 🔧 Troubleshooting Token

### **Problem 1: "Invalid authentication token"**

**Penyebab:**
- Token salah copy (ada spasi/newline)
- Token sudah expired
- Token sudah di-revoke

**Solusi:**
```bash
# 1. Generate token baru
# 2. Copy dengan teliti (no extra spaces!)
# 3. Update script
# 4. Test lagi
```

---

### **Problem 2: Token tidak muncul saat generate**

**Penyebab:**
- Popup blocker browser
- JavaScript error

**Solusi:**
```bash
# 1. Disable popup blocker untuk localhost
# 2. Refresh page (F5)
# 3. Try different browser (Chrome/Firefox)
# 4. Clear browser cache
```

---

### **Problem 3: Lupa password admin**

**Penyebab:**
- Password sudah diganti, tapi lupa

**Solusi:**
```bash
# Reset dengan restart container
docker-compose down
docker-compose up -d

# Login dengan admin/admin lagi
# Ganti password yang baru
```

---

### **Problem 4: Token expired di production**

**Penyebab:**
- Token set expiration date

**Solusi:**
```bash
# 1. Generate token baru dengan "No expiration"
# 2. Update secret di CI/CD
# 3. Test pipeline
```

---

## 📊 Token Management Tips

### **Naming Convention:**
```
[Project]-[Environment]-[Purpose]

Examples:
- MyApp-Development-Scan
- MyApp-Production-CI
- MyApp-QA-Manual
```

### **Token Rotation:**
```
Production: Ganti setiap 90 hari
Staging: Ganti setiap 6 bulan
Development: No expiration OK
```

### **Audit:**
```bash
# Check active tokens regularly
My Account → Security → Review "Existing tokens"

# Revoke unused tokens
```

---

## 📝 Quick Commands Summary

```bash
# 1. Start SonarQube
docker-compose up -d

# 2. Access dashboard
open http://localhost:9000

# 3. Login
# Username: admin
# Password: admin (or your new password)

# 4. Generate Token
# Profile → My Account → Security → Generate Token

# 5. Copy token to script
# Edit: sonar-scan.sh
# Line: SONAR_TOKEN="paste-here"

# 6. Test
./sonar-scan.sh
```

---

## 🎓 Video Tutorial (Jika Perlu)

Jika masih bingung, berikut step-by-step visual:

1. **SonarQube Login:** http://localhost:9000
2. **Click Profile** (pojok kanan atas, icon user)
3. **My Account** → **Security** tab
4. **Generate Token:**
   - Name: `MyApp`
   - Type: `User Token`
   - Expiration: `No expiration`
5. **Click "Generate"**
6. **COPY TOKEN** (klik copy button atau select all + Ctrl+C)
7. **Open `sonar-scan.sh`**
8. **Paste token** di baris `SONAR_TOKEN="..."`
9. **Save & Run:** `./sonar-scan.sh`

---

## ✅ Checklist: Token Sudah Benar?

- [ ] SonarQube running di http://localhost:9000
- [ ] Login berhasil dengan admin
- [ ] Password sudah diganti
- [ ] Token sudah di-generate
- [ ] Token sudah di-copy (no extra spaces!)
- [ ] Token sudah di-paste ke `sonar-scan.sh`
- [ ] File `sonar-scan.sh` sudah di-save
- [ ] Test run: `./sonar-scan.sh` → Success! ✅

---

## 🎉 Kesimpulan

**Token SonarQube itu:**
- ✅ **100% GRATIS** - tidak bayar sama sekali
- ✅ **Generate sendiri** - dari dashboard localhost
- ✅ **Unlimited** - bisa generate sebanyak-banyaknya
- ✅ **Sekali pakai** - 1 token bisa untuk banyak scan
- ✅ **Revocable** - bisa dihapus kapan saja

**Tidak perlu:**
- ❌ Registrasi online
- ❌ Credit card
- ❌ Email verification
- ❌ Bayar subscription

**Cukup:**
1. Install SonarQube (Docker - GRATIS)
2. Generate token (3 menit)
3. Run analysis (unlimited!)

---

## 📚 Resources

- [SonarQube User Token Docs](https://docs.sonarqube.org/latest/user-guide/user-token/)
- [Docker Setup Guide](./DOCKER_BASICS_FOR_SONARQUBE.md)
- [Quick Start](../DOCKER_QUICKSTART.md)

---

**🔑 Sekarang Anda tahu cara dapat token GRATIS! Selamat mencoba! 🚀**
