let tokens = Number(localStorage.getItem("tokens")) || 0;

function saveTokens() {
  localStorage.setItem("tokens", tokens);
}

function addTokens(amount) {
  tokens += amount;
  saveTokens();
}

function spendTokens(amount) {
  if (tokens >= amount) {
    tokens -= amount;
    saveTokens();
    return true;
  }
  return false;
}



function resetTokens() {
  tokens = 0;
  saveTokens();
}
