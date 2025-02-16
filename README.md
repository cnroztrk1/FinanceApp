## 📌 Kurulum

### 1️⃣ **Gereksinimler**
- .NET SDK 6+
- MSSQL Server

### 2️⃣ **Projeyi Klonlayın**
```sh
git clone https://github.com/cnroztrk1/FinanceApp.git
```

### 3️⃣ **Bağımlılıkları Yükleyin**
```sh
dotnet restore
```

### 4️⃣ **Veritabanını Yapılandırın**
**Arayüzdeki ve API'deki `appsettings.json` içerisinde veritabanı bağlantı dizesini kendi localhost bağlantınıza göre düzenleyin:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=FinanceAppDb;User Id=sa;Password=yourpassword;"
}
```

### 5️⃣ **Uygulamayı Çalıştırın**
- Uygulamayı çalıştırdıktan sonra **FinanceAppDb** otomatik olarak oluşturulacak ve bazı test verileri (trash data) eklenecektir.
- Ardından giriş ekranına yönlendirileceksiniz.

📌 **Varsayılan Kullanıcılar ve Şifreler:**
- **Caner - 123**
- **HDI - 123**

---

### 6️⃣ **API Kullanımı**
- **FinanceApp.API'yi çalıştırın.**
- **Finance.API'den istek atmadan önce arayüzü bir kez F5 ile yenileyerek Hub bağlantısını sağlayın. Aksi takdirde bildirimler düşmeyecektir.**
- **Swagger yapılandırmasında 'Authorize' kısmından kullanıcıyı girmelisiniz. Aksi takdirde API'den istek atamazsınız.**
- **Örnek bir API isteği aşağıdaki JSON formatında olabilir:**

```json
{
  "title": "Kredi Risk Değerlendirmesi",
  "description": "Banka kredileri için risk analizi yapılacaktır.",
  "businessPartnerId": 1,
  "agreementId": 2
}
```

📌 **Test Verisi:**
- **Caner için TenantId'ler 1 - 5**
- **HDI için TenantId'ler 6 - 10**
- **Kiracısı olmayan iş ortağı veya anlaşmaya istek atamazsınız.**

---

### 7️⃣ **Gerçek Zamanlı Bildirimler (SignalR)**
- **API'den atılan istek, hem API'den hem de arayüzden giriş yapan kullanıcı aynı ise ekranda bildirim olarak gösterilecek ve 5 saniye sonra kaybolacaktır.**
- **Sayfa yenilenmedikçe kaybolmayan bir bildirim de footer bölümüne eklendi.**
- **Eğer API'de giriş yaptığınız kullanıcı, arayüzde giriş yaptığınız kullanıcıdan farklı ise risk analizi ve iş konusu kaydedilecek ancak bildirim gösterilmeyecektir.**

---

# FinanceApp

## 🚀 Proje Yapısı

**FinanceApp**, katmanlı mimari prensipleri ile geliştirilmiştir:

- **Presentation**: Web uygulamasını ve API'yi içerir.
- **Business**: İş mantığını yönetir.
- **Data**: Veri erişim katmanını içerir (Entities, Repositories, UnitOfWork).
- **Common**: Ortak yardımcı sınıfları barındırır.

---

## 📦 Kullanılan Teknolojiler

- **.NET Core**
- **Entity Framework Core**
- **SignalR** (Gerçek zamanlı bildirimler için)
- **MSSQL** (Veritabanı)
- **ASP.NET Core Web API**
- **HTML, CSS, JavaScript**

---

## ⚡ Özellikler

- 🌍 **Multi-Tenant Desteği**  
  - Farklı firmalar için ayrı tenant yapısı.

- 🔐 **Kimlik Doğrulama ve Yetkilendirme**  
  - Kullanıcı girişleri ve oturum yönetimi.

- 📈 **Risk Analizi ve İş Takibi**  
  - İş konuları oluşturma ve risk hesaplamaları.

- 🔔 **Gerçek Zamanlı Bildirimler**  
  - SignalR kullanılarak anlık bildirimler sağlanır.

- 📊 **Veri Görselleştirme**  
  - Finansal verilerin analizi ve raporlaması için çeşitli gösterimler sunulur.

Bu rehber, FinanceApp'in sorunsuz bir şekilde kurulup çalıştırılması için gerekli tüm adımları kapsamaktadır. Eğer herhangi bir sorun yaşarsanız, lütfen bizimle iletişime geçin! 🚀
