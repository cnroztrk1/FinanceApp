const tenantId = 1; // Örnek Tenant ID

// API'ye uygun SignalR bağlantısını ayarla
const protocol = window.location.protocol === "https:" ? "https" : "http";
const baseUrl = "https://localhost:7286/riskhub"; // API'nin çalıştığı port

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${baseUrl}?tenantId=${tenantId}`)
    .withAutomaticReconnect()
    .build();

// Bağlantıyı başlat
connection.start()
    .then(() => console.log(`SignalR bağlantısı kuruldu (${baseUrl}).`))
    .catch(err => console.error("SignalR bağlantı hatası:", err));

// Bildirim alınca çalışacak olay
connection.on("ReceiveGlobalNotification", function (message) {
    showNotification(message);
});

// Bildirim gösterme fonksiyonu
function showNotification(message) {
    const notification = document.createElement("div");
    notification.classList.add("notification");
    notification.innerText = message;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 5000);
}

// Bildirim CSS Stili
const style = document.createElement("style");
style.innerHTML = `
    .notification {
        position: fixed;
        top: 20px;
        right: 20px;
        background-color: rgba(0, 0, 0, 0.8);
        color: white;
        padding: 15px;
        border-radius: 5px;
        z-index: 9999;
        transition: opacity 0.5s ease-in-out;
    }
`;
document.head.appendChild(style);
