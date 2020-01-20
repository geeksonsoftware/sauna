let connection = new signalR.HubConnectionBuilder()
    .withUrl("/sauna")
    .build();

connection.start().then(function () {
    connection.invoke("GetSaunaStatus").then(function (state) {
        console.log(state);
    });
});

connection.on("NewTemperature", function (temperature) {
    console.log(temperature);
    document.getElementById('temperature').innerHTML(temperature);
});
