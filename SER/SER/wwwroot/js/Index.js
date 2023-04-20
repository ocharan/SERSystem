let passwordIcon = document.getElementById("icon-password");

passwordIcon.addEventListener("click", function () {
  let passwordInput = document.getElementById("input-password");

  if (passwordInput.type === "password") {
    passwordInput.type = "text";
    passwordIcon.className = "fa fa-eye-slash fa-sm px-3 fs-6";
  } else {
    passwordInput.type = "password";
    passwordIcon.className = "fa fa-eye fa-lg px-3";
  }
});
