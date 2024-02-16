// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

if (typeof updatedConnection === 'undefined') {
    updatedConnection = new signalR.HubConnectionBuilder()
        .withUrl("/TransactionHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();
}

async function start() {
    try {
        if (updatedConnection.state === signalR.HubConnectionState.Disconnected) {
            await updatedConnection.start();
            console.log("SignalR Connected.");
        } else {
            console.log("HubConnection is not in Disconnected state.");
        }
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000); // Hata oluşursa tekrar denemek için setTimeout kullanılabilir
    }
}

updatedConnection.onclose(async () => {
    console.log("SignalR Connection closed. Reconnecting...");
    await start();
});

updatedConnection.on("receiveMessage", (message) => {
    console.log("Received message: " + message);
    // Gelen mesajı işleme koyma işlemleri burada gerçekleştirilir
});

start();
