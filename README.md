## 📌 Kurulum

### 1️⃣ **Gereksinimler**
- .NET SDK 6+
- MSSQL Server
### 2️⃣ **Projeyi Klonlayın**
git clone https://github.com/cnroztrk1/FinanceApp.git
### 3️⃣  Bağımlılıkları Yükleyin
dotnet restore
### 4️⃣  Veritabanın Configure Edin
Arayüzdeki ve API daki
appsettings.json içerisinde veritabanı bağlantı dizesini düzenleyin kendi localhost bağlantınıza göre düzenleyin:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=FinanceAppDb;User Id=sa;Password=yourpassword;"
}
### 5️⃣  Uygulamayı Çalıştırın
Uygulamayı çalıştırdıktan sonra otomatik sizin için FinanceAppDb oluşacak ve bir kaç trash data eklenecek. Ve giriş ekranına yönlendirecek.
Default Kullanıcılar ve şifreleri: 
- **Caner - 123**
- **HDI - 123**

### 6️⃣  API Kullanımı
FinanceApp.API i Çalıştırın 
swagger yapılandırmasında authorize kısmından kullanıcıyı girmelisiniz aksi takdirde apiden istek atamazsınız.
Girdikten sonra örnek request json
{
  "title": "Kredi Risk Değerlendirmesi",
  "description": "Banka kredileri için risk analizi yapılacaktır.",
  "businessPartnerId": 1,
  "agreementId": 2
}
Trash datada Caner için 1 den 5 e HDI için 6 dan 10 a TenantId ler tanımlanmıştır. Kiracısı olmayan iş ortağı veya anlaşmaya istek atamazsınız.
### 7️⃣  Gerçek Zamanlı Bildirimler SignalR
API den atılan istek apiden ve arayüzden giren kullanıcı aynı ise ekranda bildirim olarak belirecek 5 saniye sonra kaybolacak şekilde ayarlandı. Footera sayfa yenilenmedikçe kaybolmayan bir notifikasyonda eklendi.
Eğer apiden login olduğunuz kullanıcı arayüzde login olduğunuz kullanıcıdan farklı ise risk analizi ve iş konusu kaydedilecek ama bildirim gösterilmeyecektir.
# FinanceApp
## 🚀 Proje Yapısı

**FinanceApp** katmanlı mimari ile geliştirilmiştir:

- **Presentation**: Web uygulamasını içerir ve API içerir
- **Business**: İş mantığını içerir.
- **Data**: Veri katmanını içerir (Entities, Repositories, UnitOfWork).
- **Common**: Ortak yardımcı sınıfları içerir.

## 📦 Kullanılan Teknolojiler

- **.NET Core**
- **Entity Framework Core**
- **SignalR** (Gerçek zamanlı bildirimler için)
- **MSSQL** (Veritabanı)
- **ASP.NET Core Web API**
- **HTML**
- **CSS**
- **Javascript**
## ⚡ Özellikler

- 🌍 **Multi-Tenant Desteği**  
  Farklı firmalar için ayrı tenant yapısı.

- 🔐 **Kimlik Doğrulama ve Yetkilendirme**  
  Kullanıcı girişleri ve oturum yönetimi.

- 📈 **Risk Analizi ve İş Takibi**  
  İş konuları oluşturma ve risk hesaplamaları.

- 🔔 **Gerçek Zamanlı Bildirimler**  
  SignalR kullanılarak risk analizi bildirimleri.

- 📊 **Veri Görselleştirme**  
  Finansal verilerin analizi ve raporlaması.


