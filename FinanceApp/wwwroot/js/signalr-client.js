function getTenantId() {
    // Önce HTML'deki div içinden TenantId'yi al
    const tenantDiv = document.getElementById("tenant-id-container");
    if (tenantDiv) {
        return tenantDiv.getAttribute("data-tenant-id") || "1";
    }

    // Eğer HTML'den alınamazsa, cookie'den al
    return getCookie("TenantId") || "1";
}

function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].trim();
        if (cookie.startsWith(name + '=')) {
            return cookie.substring(name.length + 1);
        }
    }
    return null;
}

const tenantId = getTenantId();

// API'ye uygun SignalR bağlantısını ayarla
const signalRUrl = "https://localhost:7286/riskhub"; // SignalR Hub URL
const cacheClearUrl = "https://localhost:5001/api/cache/clear"; // 5001 portundaki API

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${signalRUrl}?tenantId=${tenantId}`, {
        withCredentials: true
    })
    .withAutomaticReconnect()
    .build();

// Bağlantıyı başlat
connection.start()
    .then(() => console.log(`SignalR bağlantısı kuruldu (TenantId: ${tenantId}).`))
    .catch(err => console.error("SignalR bağlantı hatası:", err));

// Bildirim alınca çalışacak olay
connection.on("ReceiveRiskNotification", function (message) {
    showNotification(message);
    addNotificationToFooter(message);
    clearCacheForTenant();
});

// Cache temizleme fonksiyonu (5001 portundaki API'yi çağırıyor)
function clearCacheForTenant() {
    const tenantId = getTenantId(); // Güncel TenantId'yi tekrar al

    fetch(`${cacheClearUrl}?tenantId=${tenantId}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.ok) {
                console.log(`Cache başarıyla temizlendi (TenantId: ${tenantId}).`);
            } else {
                console.error("Cache temizleme başarısız oldu.");
            }
        })
        .catch(error => console.error("Cache temizleme hatası:", error));
}

function showNotification(message) {
    const notification = document.createElement("div");
    notification.classList.add("notification");
    notification.innerText = message;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 5000);
}

function addNotificationToFooter(message) {
    let footer = document.getElementById("notification-footer");

    if (!footer) {
        footer = document.createElement("div");
        footer.id = "notification-footer";
        document.body.appendChild(footer);
    }

    const notificationItem = document.createElement("div");
    notificationItem.classList.add("footer-notification");
    notificationItem.innerText = message;

    footer.appendChild(notificationItem);
}

// CSS ile bildirimin stilini ayarlayalım
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

    #notification-footer {
        position: fixed;
        bottom: 0;
        left: 0;
        width: 100%;
        background-color: #333;
        color: white;
        padding: 10px;
        font-size: 14px;
        overflow-y: auto;
        max-height: 150px;
        text-align: left;
    }

    .footer-notification {
        padding: 5px;
        border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    }
`;
document.head.appendChild(style);
