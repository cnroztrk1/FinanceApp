const tenantId = 1; // Örnek Tenant ID

// Protokole göre doğru URL'yi seç
const protocol = window.location.protocol === "https:" ? "https" : "http";
const port = protocol === "https" ? "7286" : "5054";
const baseUrl = `${protocol}://localhost:${port}/riskhub`;

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${baseUrl}?tenantId=${tenantId}`)
    .withAutomaticReconnect()
    .build();

// Bağlantıyı başlat
connection.start()
    .then(() => console.log(`SignalR bağlantısı kuruldu (${baseUrl}).`))
    .catch(err => console.error("SignalR bağlantı hatası:", err));

// Risk bildirimi alınca çalışacak olay
connection.on("ReceiveRiskNotification", function (message) {
    console.log("Yeni Risk Bildirimi:", message);
});
