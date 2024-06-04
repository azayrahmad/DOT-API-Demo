# DOT API Demo - Pustaka
Demo REST API menggunakan .NET 8 dan Entity Framework. Demo ini menampilkan proses CRUD pada model, relasi antar entitas, serta implementasi caching sederhana.

## Instalasi
Cara termudah menjalankan API ini adalah menggunakan GitHub Codespace, namun Anda juga dapat clone repository ke komputer dan menjalankan secara lokal. Mulai dengan menekan tombol hijau bertuliskan `<> Code` di kanan atas repositori.

### GitHub Codespace
1. Klik pada tombol `Code` dan pilih `Codespaces` lalu `Create codespace on main`.
2. Tunggu proses Setup. Anda akan diarahkan ke tab baru yang menampilkan antarmuka Visual Studio Code.
3. Pastikan ada Terminal di VS Code. Jika tidak, pilih menu `View` lalu `Terminal`.
4. Jalankan perintah berikut di Terminal
  ```bash
  cd Pustaka
  dotnet build
  dotnet run --project Pustaka
  ```
5. Anda akan mendapat pemberitahuan bahwa aplikasi telah berjalan. Pilih `Open in browser`.
6. Tambahkan `/swagger` di akhir alamat aplikasi lalu Refresh.
7. Anda akan melihat tampilan antarmuka Swagger.

### Menjalankan Secara Lokal
1. Clone repository:
    ```bash
    git clone https://github.com/azayrahmad/DOT-API-Demo.git
    cd DOT-API-Demo
    ```
    Jika Anda sudah memiliki aplikasi GitHub Desktop di komputer Anda, Anda juga dapat memilih `Open in GitHub Desktop`.
2. Pastikan Anda sudah menginstal [.NET SDK](https://dotnet.microsoft.com/download) di mesin Anda. 
3. Jalankan perintah berikut untuk menginstal dependensi dan membangun proyek:
    ```bash
    cd Pustaka
    dotnet build
    dotnet run --project Pustaka
    ```
4. Buka browser dan akses URL `https://localhost:5001/swagger` atau `http://localhost:5000/swagger` untuk membuka tampilan Swagger.

## Struktur Proyek
- **Models**: Berisi model `Penulis` dan `Buku`.
  - Model `Penulis` dan `Buku` dihubungkan menggunakan Eager Loading.
- **Program.cs**: Berisi konfigurasi aplikasi dan endpoint API.
- **Data/AppDbContext**: Berisi konfigurasi database menggunakan Entity Framework Core.

## Fitur
- CRUD untuk Penulis dan Buku.
- Validasi untuk memastikan buku dan penulis valid sebelum menyimpan ke database.
  - Untuk keperluan demonstrasi, caching tidak diterapkan pada API `Buku`. 
- Caching untuk meningkatkan performa saat mengambil daftar penulis.
- Swagger untuk dokumentasi API interaktif.

