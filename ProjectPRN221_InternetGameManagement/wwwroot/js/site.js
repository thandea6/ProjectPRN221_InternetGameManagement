"user strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/SignalRServer")
    .build();


connection.on("ReceiveMessage", (user, message) => {
    // Kiểm tra user và message có giá trị hợp lệ
    console.log(`Received message from ${user}: ${message}`);
    if (user && message) {
        // Tạo phần tử <li> để hiển thị tin nhắn
        const li = document.createElement("li");
        li.textContent = `${user}: ${message}`;
        document.getElementById("messagesList").appendChild(li);
    } else {
        // Hiển thị lỗi ra console nếu thiếu user hoặc message
        console.error("Invalid data received. User or message is missing.");
    }
});

connection.start()
    .then(() => console.log('SignalR connected.'))
    .catch(err => console.error('Error connecting to SignalR:', err));


