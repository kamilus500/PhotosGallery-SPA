const connection = new signalR.HubConnectionBuilder().withUrl("/accessDenied").build();

connection.start().then(function () {
    console.log("SignalR connection established.");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("AccessDeniedNotification", function () {
    alert("You don't have permission");
    loadPartial('/Home/_Index');
});