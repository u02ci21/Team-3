
let tokens = Number(localStorage.getItem("tokens")) || 0; //initalising local storage or pulling from local storage


function addTokens(amount) { //function to add tokens
  tokens += amount;
  saveTokens();
}


function spendTokens(amount) { //function to remove tokane Or alert the player that they have not got enough
  if (tokens >= amount) {
    tokens -= amount;
    saveTokens();
  } else {
    alert("Not enough tokens");
  }
}


function resetTokens() { //function to reset the tokens
  tokens = 0;
  saveTokens();
}


function saveTokens() { //saves tokens to local storage which allows the player to return at a later date
  localStorage.setItem("tokens", tokens);
  updateDisplay();
}


function updateDisplay() { //updates display
  const display = document.getElementById("tokenDisplay");
  if (display) {
    display.textContent = `Tokens: ${tokens}`;
  }
}


window.addEventListener("DOMContentLoaded", updateDisplay); //event listener for if the HTML is loaded and updateDisplay function


