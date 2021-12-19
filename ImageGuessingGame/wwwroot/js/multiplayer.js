"use strict";

var connection = new signalR.HubConnectionBuilder()
  .withUrl("/multiplayerHub")
  .build();

document.getElementById("submit").disabled = true;

connection.on("Winner", function () {
  window.location.replace("./WinMulti");
});

connection.on("WrongGuess", function () {
  document.getElementById("submit").disabled = false;
  window.location.reload();
});

window.onbeforeunload = function () {
  connection.invoke("PlayerLeft").catch(function (err) {
    return console.error(err.toString());
  });
  WebSocket.close();
  window.location.replace("./Lobby");
};

connection
  .start()
  .then(function () {
    document.getElementById("submit").disabled = false;
  })
  .catch(function (err) {
    return console.error(err.toString());
  });

document.getElementById("submit").addEventListener("click", function (event) {
  var gameId = document.getElementById("gameId").innerHTML;
  var guess = document.getElementById("guess").value;
  var userName = document.getElementById("userName").innerHTML;
  connection
    .invoke("SubmitGuess", gameId, guess, userName)
    .catch(function (err) {
      return console.error(err.toString());
    });
  document.getElementById("submit").disabled = true;
});
