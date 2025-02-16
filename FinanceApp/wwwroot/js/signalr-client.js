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

const tenantId = getCookie("TenantId") || 1;
console.log(tenantId);

// API'ye uygun SignalR bağlantısını ayarla
const apiBaseUrl = "https://localhost:7286/riskhub";

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${apiBaseUrl}?tenantId=${tenantId}`, {
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
    console.log(tenantId);
    showNotification(message);
    addNotificationToFooter(message);
});

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
