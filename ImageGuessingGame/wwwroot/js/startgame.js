"use strict";

var connection = new signalR.HubConnectionBuilder()
  .withUrl("/startGameHub")
  .build();

//Disable send button until connection is established
if (document.getElementById("startGame")) {
  document.getElementById("startGame").disabled = true;
}

window.onbeforeunload = function () {
  if (document.getElementById("startGame")) {
    var gameId = document.getElementById("gameId").value;
    connection.invoke("HostLeft", gameId).catch(function (err) {
      return console.error(err.toString());
    });
  } else {
    connection.invoke("PlayerLeft").catch(function (err) {
      return console.error(err.toString());
    });
  }
  WebSocket.close();
  window.location.replace("./Lobby");
};

connection.on("HostLeft", function () {
  var baseText = "Host left, leaving in: ";
  document.getElementById("info").innerHTML = baseText + "3";
  setTimeout(() => {
    document.getElementById("info").innerHTML = baseText + "2";
  }, 1000);
  setTimeout(() => {
    document.getElementById("info").innerHTML = baseText + "1";
  }, 2000);
  setTimeout(() => {
    window.location.replace("../Index");
  }, 3000);
});

connection.on("ReceiveStart", function (gameId) {
  window.location.replace("./MultiplayerGame?gameId=" + gameId);
});

connection.on("ClearList", function () {
  var userName = document.getElementById("username").innerHTML;
  document.getElementById("playerList").innerHTML = "";
  connection.invoke("ProvideName", userName).catch(function (err) {
    return console.error(err.toString());
  });
});

connection.on("AddNameToList", function (userName) {
  var li = document.createElement("li");
  document.getElementById("playerList").appendChild(li);
  li.textContent = userName;
});

connection
  .start()
  .then(function () {
    if (document.getElementById("startGame")) {
      document.getElementById("startGame").disabled = false;
    }
    connection.invoke("UpdateUsers").catch(function (err) {
      return console.error(err.toString());
    });
  })
  .catch(function (err) {
    return console.error(err.toString());
  });

if (document.getElementById("startGame")) {
  document
    .getElementById("startGame")
    .addEventListener("click", function (event) {
      var gameId = document.getElementById("gameId").value;
      var players = document.getElementById("playerList").getElementsByTagName("li");
      var playernames = [];
      for (let i=0; i<players.length;i++){
        playernames.push(players[i].textContent);
      }
      connection.invoke("StartGame", gameId, playernames).catch(function (err) {
        return console.error(err.toString());
      });
      event.preventDefault();
    });
}
