// =============================
// 🔍 VALIDATION
// =============================
function validateIdentifier(val) {
    //alert("Validating: " + val);
    if (!val) return false;

    if (/^[6-9]\d{9}$/.test(val)) return true;
    if (/^\S+@\S+\.\S+$/.test(val)) return true;

    return false;
}

// =============================
// 📲 SEND OTP
// =============================
document.addEventListener("DOMContentLoaded", function () {
    
    const sendBtn = document.getElementById("btnSendOtp");
    const verifyBtn = document.getElementById("btnVerifyOtp");
    const passwordBtn = document.getElementById("btnPasswordLogin");
    
    if (sendBtn) {
        sendBtn.addEventListener("click", async function () {
            document.getElementById("txtIdentifier").value = '9878886567';
            var identifier = document.getElementById("txtIdentifier").value;

            // 🔍 Validation
            if (!validateIdentifier(identifier)) {
                showToast("Enter valid mobile or email", "error");
                return;
            }

            this.disabled = true;
            this.innerText = "Sending...";

            try {

                const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
                const res = await fetch(`/Auth/Login?handler=SendOtp&identifier=${identifier}`, {
                //var res = await fetch("/Auth/Login?handler=SendOtp", {
                    method: 'POST',
                    credentials: 'same-origin',
                    headers: {
                        "RequestVerificationToken": token
                    }
                });
                
                // 🔥 Handle HTTP error
                if (!res.ok) {
                    showToast("Failed to send OTP", "error");
                    this.disabled = false;
                    this.innerText = "Send OTP";
                    return;
                }

               
                //alert("OTP send response received"); 
                //var data = await res.json();
                
                // ==========================
                // ✅ SUCCESS
                // ==========================
                
                const data = await res.json();
                //alert("okk" + data.success);
                if (data.success) {

                    var otpValue = data.otp || data.OTP;

                    showToast("OTP: " + otpValue); //temp - remove in prod
                    showToast(data.message || "OTP sent successfully");
                    document.getElementById("step1").style.display = "none";
                    document.getElementById("step2").style.display = "block";
                    document.getElementById("displayIdentifier").innerText = identifier;
                    document.getElementById("txtOtp").value = otpValue;//remove after testing
                    startResendTimer();
                    document.getElementById("txtOtp").focus();
                }
                else {
                    showToast(data.message, "error");
                }
                

            } catch (err) {
                showToast("Something went wrong", "error");
            }

            this.disabled = false;
            this.innerText = "Send OTP";
        });
    }
    if (verifyBtn) {
        //alert("OKK script loaded");
        verifyBtn.addEventListener("click", async function () {
            
            var identifier = document.getElementById("txtIdentifier").value;
            var otp = document.getElementById("txtOtp").value;

            // 🔍 Validation
            if (!otp || otp.length !== 6) {
                showToast("Enter valid 6-digit OTP", "error");
                return;
            }

            this.disabled = true;
            this.innerText = "Verifying...";

            try {

                const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

                var res = await fetch("/Auth/Login?handler=VerifyOtp", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "RequestVerificationToken": token
                    },
                    body: JSON.stringify({
                        Identifier: identifier,
                        OTP: parseInt(otp)
                    })
                });

                // 🔥 Handle HTTP error
                if (!res.ok) {
                    showToast("Verification failed. Try again", "error");
                    this.disabled = false;
                    this.innerText = "Verify OTP";
                    return;
                }
                
                var data = await res.json();
                console.log("Verify Response:", data);

                // Normalize response
                var status = data.status || data.Status;

                if (status === "SUCCESS") {
                    showToast("Login successful");

                    setTimeout(function () {
                        //window.location.href = "/Dashboard/Index";
                    }, 800);
                }
                else if (status === "NEW_USER") {

                    showToast("Redirecting to registration");

                    var tempId = data.tempID || data.TempID;
                    var tempToken = data.tempToken || data.TempToken;

                    window.location.href =
                        "/Auth/Register?tempId=" + tempId +
                        "&token=" + tempToken;
                }
                else {
                    showToast("Invalid OTP", "error");
                }

            } catch (err) {
                showToast("Something went wrong", "error");
            }

            this.disabled = false;
            this.innerText = "Verify OTP";
        });
    }

    if (passwordBtn) {
        passwordBtn.addEventListener("click", async function () {

            var identifier = document.getElementById("txtLoginId").value;
            var password = document.getElementById("txtPassword").value;

            if (!identifier || !password) {
                alert("Enter login ID and password");
                return;
            }

            this.disabled = true;
            this.innerText = "Logging in...";

            var res = await fetch("/api/customer/auth/login-password", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    Identifier: identifier,
                    Password: password
                })
            });

            if (!res.ok) {
                alert("Invalid credentials");
                this.disabled = false;
                this.innerText = "Login";
                return;
            }

            var data = await res.json();

            if (data.status === "SUCCESS") {
                window.location.href = "/Dashboard/Index";
            } else {
                alert("Login failed");
            }

            this.disabled = false;
            this.innerText = "Login";
        });
    }

});

// =============================
// 🔁 CHANGE NUMBER
// =============================
function changeNumber() {
    document.getElementById("step1").style.display = "block";
    document.getElementById("step2").style.display = "none";
}
function resendOtp() {
    document.getElementById("btnSendOtp").click();
}
// =============================
// 🔐 PASSWORD TOGGLE
// =============================
function togglePassword() {
    var el = document.getElementById("passwordSection");

    if (el.style.display === "none")
        el.style.display = "block";
    else
        el.style.display = "none";
}


function showToast(message, type = "success") {

    var container = document.getElementById("toastContainer");

    var toast = document.createElement("div");

    var bgColor = type === "error" ? "#dc3545" : "#28a745";

    toast.style.background = bgColor;
    toast.style.color = "#fff";
    toast.style.padding = "12px 16px";
    toast.style.marginBottom = "10px";
    toast.style.borderRadius = "8px";
    toast.style.boxShadow = "0 4px 10px rgba(0,0,0,0.1)";
    toast.style.minWidth = "220px";
    toast.style.fontSize = "14px";
    toast.style.opacity = "0";
    toast.style.transition = "opacity 0.3s ease";

    toast.innerText = message;

    container.appendChild(toast);

    // Fade in
    setTimeout(() => {
        toast.style.opacity = "1";
    }, 100);

    // Auto remove
    setTimeout(() => {
        toast.style.opacity = "0";
        setTimeout(() => {
            toast.remove();
        }, 300);
    }, 3000);
}

let resendSeconds = 30;
let timerInterval;

// Start timer
function startResendTimer() {

    resendSeconds = 30;

    document.getElementById("resendLink").style.display = "none";
    document.getElementById("resendTimer").style.display = "inline";

    timerInterval = setInterval(function () {

        resendSeconds--;

        document.getElementById("resendTimer").innerText =
            "Resend in " + resendSeconds + "s";

        if (resendSeconds <= 0) {
            clearInterval(timerInterval);

            document.getElementById("resendTimer").style.display = "none";
            document.getElementById("resendLink").style.display = "inline";
        }

    }, 1000);
}