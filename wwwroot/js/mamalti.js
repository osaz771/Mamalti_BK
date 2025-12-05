
function handleSignup() {

    var fullName = document.getElementById("signup-fullname").value;
    var email = document.getElementById("signup-email").value;
    var password = document.getElementById("signup-password").value;
    var confirmPassword = document.getElementById("signup-confirm").value;

    if (fullName == "") {
        alert("Full name must be filled out");
        return false; 
    }

    if (email == "") {
        alert("E-mail address must be filled out");
        return false;
    }

    if (password == "" || confirmPassword == "") {
        alert("Password fields must be filled out");
        return false;
    }

    if (password != confirmPassword) {
        alert("Passwords do not match");
        return false;
    }

    alert("Welcome " + fullName + " , your account has been created.");

    return false;
}

function handleLogin() {

    var email = document.getElementById("login-email").value;
    var password = document.getElementById("login-password").value;

    if (email == "") {
        alert("Please enter your e-mail address");
        return false;
    }

    if (password == "") {
        alert("Please enter your password");
        return false;
    }

    alert("Welcome back to Muamalati Platform!");

    return false;
}

function sendCode() {

    var email = document.getElementById("fp-email").value;

    if (email == "") {
        alert("Please enter your e-mail address");
        return false;
    }

    alert("A reset code has been sent to: " + email);

    document.getElementById("fp-step1").style.display = "none";
    document.getElementById("fp-step2").style.display = "block";

    return false;
}

function changePassword() {

    var newPass = document.getElementById("fp-newpass").value;
    var confirmPass = document.getElementById("fp-confirmpass").value;

    if (newPass == "" || confirmPass == "") {
        alert("Password fields cannot be empty");
        return false;
    }

    if (newPass != confirmPass) {
        alert("Passwords do not match");
        return false;
    }

    alert("Password changed successfully. You can log in with your new password.");


    return false;
}

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

