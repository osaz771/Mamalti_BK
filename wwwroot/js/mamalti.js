/* =========================
   SIGN UP VALIDATION
   ========================= */
function handleSignup() {

    var fullName = document.getElementById("signup-fullname").value.trim();
    var email = document.getElementById("signup-email").value.trim();
    var password = document.getElementById("signup-password").value;
    var confirmPassword = document.getElementById("signup-confirm").value;

    if (fullName === "") {
        alert("Full name must be filled out");
        return false;
    }

    if (email === "") {
        alert("E-mail address must be filled out");
        return false;
    }

    if (password === "" || confirmPassword === "") {
        alert("Password fields must be filled out");
        return false;
    }

    if (password !== confirmPassword) {
        alert("Passwords do not match");
        return false;
    }

    // ✅ allow submit to server
    return true;
}

/* =========================
   LOGIN VALIDATION
   ========================= */
function handleLogin() {

    var email = document.getElementById("login-email").value.trim();
    var password = document.getElementById("login-password").value;

    if (email === "") {
        alert("Please enter your e-mail address");
        return false;
    }

    if (password === "") {
        alert("Please enter your password");
        return false;
    }

    // ✅ allow submit to server
    return true;
}

/* =========================
   FORGET PASSWORD – STEP 1
   ========================= */
function sendCode() {

    var email = document.getElementById("fp-email").value.trim();

    if (email === "") {
        alert("Please enter your e-mail address");
        return false;
    }

    alert("A reset code has been sent to: " + email);

    document.getElementById("fp-step1").style.display = "none";
    document.getElementById("fp-step2").style.display = "block";

    // ❗ stay on page
    return false;
}

/* =========================
   FORGET PASSWORD – STEP 2
   ========================= */
function changePassword() {

    var newPass = document.getElementById("fp-newpass").value;
    var confirmPass = document.getElementById("fp-confirmpass").value;

    if (newPass === "" || confirmPass === "") {
        alert("Password fields cannot be empty");
        return false;
    }

    if (newPass !== confirmPass) {
        alert("Passwords do not match");
        return false;
    }

    // ✅ allow submit to server
    return true;
}

/* =========================
   EXTRA SIGNUP VALIDATION
   ========================= */
function validateSignup() {

    var fullName = document.getElementById("signup-fullname").value.trim();
    var email = document.getElementById("signup-email").value.trim();
    var phone = document.getElementById("signup-phone").value.trim();
    var password = document.getElementById("signup-password").value;
    var confirmPassword = document.getElementById("signup-confirm").value;

    if (fullName === "" || email === "" || phone === "" || password === "" || confirmPassword === "") {
        alert("Please fill in all fields.");
        return false;
    }

    if (email.indexOf("@") === -1 || email.indexOf(".") === -1) {
        alert("Please enter a valid e-mail address.");
        return false;
    }

    if (password.length < 8) {
        alert("Password must be at least 8 characters.");
        return false;
    }

    if (password !== confirmPassword) {
        alert("Passwords do not match.");
        return false;
    }

    return true;
}

/* =========================
   COOKIE FUNCTIONS
   ========================= */
function writeCookie(name, value, days) {

    var expires = "";

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }

    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {

    var nameEQ = name + "=";
    var ca = document.cook
