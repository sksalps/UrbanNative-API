let resendInterval = null;
let resendTime = 30;
let referralCode = null;


//handle referral code both at Login page and Registration page. Priority: URL > localStorage > Cookie
function resolveReferralCode() {
    console.warn("Resolving referral code");
    const urlParams = new URLSearchParams(window.location.search);
    const urlRef = urlParams.get("ref");

    const localRef = localStorage.getItem("REF_CODE");

    const cookieRef = getCookie("REF_CODE");

    let finalRef = null;

    if (urlRef) {
        finalRef = urlRef;

        // 🔥 always override with latest
        localStorage.setItem("REF_CODE", urlRef);
        setCookie("REF_CODE", urlRef, 7); // 7 days
    }
    else if (localRef) {
        finalRef = localRef;
        setCookie("REF_CODE", localRef, 7); // 7 days
    }
    else if (cookieRef) {
        finalRef = cookieRef;

        // sync to localStorage
        localStorage.setItem("REF_CODE", cookieRef);
    }
    finalRef = finalRef ? finalRef.trim() : null;
    return finalRef;
}

function setCookie(name, value, days) {
    const expires = new Date(Date.now() + days * 86400000).toUTCString();
    document.cookie = `${name}=${value}; expires=${expires}; path=/`;
}

function getCookie(name) {
    return document.cookie.split('; ')
        .find(row => row.startsWith(name + '='))
        ?.split('=')[1];
}


//Login page load - check for ref code
function initLoginPage() {

    if (!document.getElementById("txtIdentifier")) return;

    console.warn("Login Page INIT running");

    const urlParams = new URLSearchParams(window.location.search);
    const ref = urlParams.get("ref");

    if (ref) {
        localStorage.setItem("REF_CODE", ref);
    }

    window.referralCode = resolveReferralCode();
}

if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initLoginPage);
} else {
    initLoginPage();
}

// 🔥 handle both cases alert("Login Page INIT - Referral code: " + window.referralCode);
if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initLoginPage);
} else {
    initLoginPage();
}


// =============================
// 🔍 VALIDATION
// =============================
function validateIdentifier(identifier) {
    //alert("Validating: " + val);

    if (!identifier) return false;

    //If email entered, validate it
    if (identifier.includes("@") && !checkEmail(identifier)) {
        showToast("Enter valid email/mobile", "error");
        return false;
    }

    //if mobile is entered and it's less than 10 digits, show error
    if (!identifier.includes("@") && identifier.length < 10) {
        showToast("Enter valid mobile", "error");
        return false;
    }
    if (identifier.includes("@") && !checkEmail(email)) {
        showToast("Enter valid email", "error");
        return false;
    }

    if (/^[6-9]\d{9}$/.test(identifier)) return true;
    if (/^\S+@\S+\.\S+$/.test(identifier)) return true;
    showToast("Enter valid mobile or email", "error");
    return false;
}
// =============================
// 🔁 CHANGE IDENTIFIER
// =============================
function resetIdentifier() {

    // clear input
    //document.getElementById("txtIdentifier").value = "";
    document.getElementById("txtIdentifier").disabled = false;
    document.getElementById("changeIdentifier").style.display = "none";

    // reset UI
    document.getElementById("btnSendOtp").style.display = "block";
    document.getElementById("step1").style.display = "block";
    document.getElementById("step2").style.display = "none";

    document.getElementById("txtOtp").value = "";
}
function resendOtp() {
    document.getElementById("txtOtp").value = "";
    document.getElementById("btnSendOtp").click();
}
function setIdentifierVerified(identifier) {
    // show identifier in step2
    //document.getElementById("displayIdentifier").innerText = identifier;
    document.getElementById("changeIdentifier").style.display = "inline";

    // keep identifier visible but disable editing
    document.getElementById("txtIdentifier").value = identifier;
    document.getElementById("txtIdentifier").disabled = true;

    // toggle UI
    //document.getElementById("step1").style.display = "none";
    document.getElementById("btnSendOtp").style.display = "none";
    document.getElementById("step2").style.display = "block";
}
// =============================
// 📲 SEND OTP
// =============================
document.addEventListener("DOMContentLoaded", function () {
    
    const sendBtn = document.getElementById("btnSendOtp");
    const verifyBtn = document.getElementById("btnVerifyOtp");
    const passwordBtn = document.getElementById("btnPasswordLogin"); 
    const regOTP = document.getElementById("txtOtp"); 
    
    if (sendBtn) {
        sendBtn.addEventListener("click", async function () {
        //document.getElementById("txtIdentifier").value = '9878886567';
        var identifier = document.getElementById("txtIdentifier").value;
        if (identifier && identifier.includes("@")) {
            identifier = identifier.toLowerCase();
        }
        // 🔍 Validation
        if (!validateIdentifier(identifier)) {
            return;
        }

        this.disabled = true;
        this.innerText = "Sending...";

            try {

                const res = await sendAuthOtp(identifier);

                if (!res.success) {
                    showToast("Failed to send OTP", "error");
                    this.disabled = false;
                    this.innerText = "Send OTP";
                    return;
                }

                // ==========================
                // ✅ SUCCESS (LOGIN PAGE)
                // ==========================
                setIdentifierVerified(identifier);
                
                // auto fill OTP (only for testing)
                document.getElementById("txtOtp").value = res.otp || "";

                startResendTimer();

                document.getElementById("txtOtp").focus();
                referralCode = resolveReferralCode();

            } catch (err) {
                console.error(err);
                showToast("Something went wrong", "error");
            }

        this.disabled = false;
        this.innerText = "Send OTP";
        });
    }
    if (verifyBtn) {
        //alert("OKK script loaded");
        verifyBtn.addEventListener("click", async function () {
             {
                var identifier = document.getElementById("txtIdentifier").value;
                if (identifier && identifier.includes("@")) {
                    identifier = identifier.toLowerCase();
                }
                // 🔍 Validation
                if (!validateIdentifier(identifier)) {
                    return;
                }
                var otp = document.getElementById("txtOtp").value;

                // 🔍 Validation
                if (!otp || otp.length !== 6) {
                    showToast("Enter valid 6-digit OTP", "error");
                    return;
                }
            }

            this.disabled = true;
            this.innerText = "Verifying...";

            try {
                const res = await verifyAuthOTP(identifier, otp); //call verification at time of Login

                if (!res.success) {
                    showToast("Verification failed. Try again", "error");
                    this.disabled = false;
                    this.innerText = "Verify OTP";
                    return;
                }
               
                const data = res.result;

                // 🔥 Handle HTTP error
                if (!data || !data.success) {
                    showToast("Verification failed. Try again", "error");
                    this.disabled = false;
                    this.innerText = "Verify OTP";
                    return;
                }
                
                // Normalize response
                var status = data.status || data.Status;
                

                if (status === "LOGIN") {
                    const user = data.userExist;
                    if (!user) {
                        showToast("User data missing", "error");
                        return;
                    }

                    var loginStatus = user.loginStatus || user.LogiStatus;
                    var token = setUserLoginToken(user);
                    if (loginStatus !== "SUCCESS" || !token) {
                        showToast("Login failed", "error");
                    }

                    showToast("Login successful");

                    // 🚀 Redirect
                    setTimeout(function () {
                        window.location.replace("/Customer/Dashboard");
                    }, 800);
                }
                if (status === "NEW_USER") {
                    showToast("Verified successfully");
                    {
                        showToast("Redirecting to registration");

                        // store temp user
                        localStorage.setItem("TEMP_ID", data.tempID);
                        localStorage.setItem("TEMP_TOKEN", data.tempToken);

                        // 🔥 store verified identifier
                        localStorage.setItem("VERIFIED_IDENTIFIER", identifier);

                        localStorage.setItem("REF_CODE", data.byReferralCode);
                        setCookie("REF_CODE", data.byReferralCode, 7); // 7 days

                        // 🔥 mark verified type
                        if (identifier.includes("@")) {
                            localStorage.setItem("VERIFIED_TYPE", "EMAIL");
                        } else {
                            localStorage.setItem("VERIFIED_TYPE", "MOBILE");
                        }

                        window.location.href = "/Auth/Register";
                    }
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
// 🔐 PASSWORD TOGGLE
// =============================
function togglePassword() {
    var el = document.getElementById("passwordSection");

    if (el.style.display === "none")
        el.style.display = "block";
    else
        el.style.display = "none";
}

function showToast1(message, type = "success") {

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



// ==========================
// 🔹 RESEND TIMER
// ==========================
function startResendTimer() {

    const timerEl = document.getElementById("resendTimer");
    const link = document.getElementById("resendLink");

    //resendTime = 30;
    link.style.display = "none";

    if (resendInterval) clearInterval(resendInterval);

    resendInterval = setInterval(() => {

        resendTime--;
        timerEl.innerText = `Resend in ${resendTime}s`;

        if (resendTime <= 0) {
            clearInterval(resendInterval);
            timerEl.innerText = "";
            link.style.display = "inline";
        }

    }, 1000);
}
async function sendAuthOtp(identifier) {
     try {
        if (!identifier) {
            showToast("Enter mobile or email", "error");
            return { success: false };
        }
        referralCode = resolveReferralCode();
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        const res = await fetch("/Auth/Login?handler=SendOtp", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            body: JSON.stringify({
                Identifier: identifier,
                ReferralCode: referralCode   
            })

        });

        if (!res.ok) {
            showToast("Failed to send OTP", "error");
            return { success: false };
        }

        const data = await res.json();

        if (data?.success) {
            var otpValue = data.otp || data.OTP;
            showToast("OTP: " + otpValue); //temp - remove in prod
            showToast(data.message || "OTP sent successfully");
            localStorage.setItem("REF_CODE", data.byReferralCode);
            // store temp user
            localStorage.setItem("TEMP_ID", data.tempID);
            localStorage.setItem("TEMP_TOKEN", data.tempToken);
            setCookie("REF_CODE", data.byReferralCode, 7); // 7 days

            return data;// { success: true,  };
        }

        showToast(data?.message || "Failed to send OTP", "error");
        return { success: false };

    } catch (err) {
        console.error("sendOtp error:", err);
        showToast("Something went wrong", "error");
        return { success: false };
    }
}

async function verifyAuthOTP(identifier, otp) {
    try {
        if (!otp || otp.length !== 6) {
            showToast("Enter valid OTP", "error");
            return { success: false };
        } 
        referralCode = resolveReferralCode();
        //alert("Verifying OTP: " + otp + " for " + identifier + " with referral: " + referralCode);
        const sessionId = localStorage.getItem("UN_SESSION");
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        const res = await fetch("/Auth/Login?handler=VerifyOtp", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            body: JSON.stringify({
                Identifier: identifier,
                OTP: parseInt(otp),
                SessionID: sessionId,
                ByReferralCode: referralCode
            })
        });

        if (!res.ok) {
            showToast("Verification failed", "error");
            return { success: false };
        }

        const result = await res.json();
        
        // ==========================
        // ✅ SUCCESS
        // ==========================
        if (result?.status === "LOGIN") {
            showToast("Verified successfully");
            //result.status = "LOGIN";
            return {result};
        }

        // ==========================
        // 🆕 NEW USER
        // ==========================
        else if (result?.status === "NEW_USER") {
            localStorage.setItem("TEMP_SESSION", sessionId);
            localStorage.setItem("IS_VERIFIED", "true");

            localStorage.setItem("REF_CODE", result.byReferralCode);
            setCookie("REF_CODE", result.byReferralCode, 7); // 7 days
            //showToast("Verified successfully");
            return {  result };
        }

        // ==========================
        // ❌ INVALID OTP
        // ==========================
        showToast("Invalid OTP", "error");

        return { success: false };

    } catch (err) {
        console.error("verifyOtp error:", err);
        showToast("Something went wrong", "error");
        return { success: false };
    }
}

async function setUserLoginToken(setUser) {

    localStorage.setItem("token", setUser.token);

    // 👤 Store User Object (structured)
    const user = {
        userID: setUser.userID,
        userRandomID: setUser.userRandomID,
        fullName: setUser.fullName,
        nickName: setUser.userNickName,
        mobile: setUser.mobile,
        email: setUser.email,
        referralCode: setUser.referralCode,
        sponsorName: setUser.sponsorName,
        sponsorReferralCode: setUser.sponsorReferralCode,
        role: setUser.role
    };

    localStorage.setItem("user", JSON.stringify(user));
    return  setUser.token;

}

function isLoggedIn() {
    return !!localStorage.getItem("token");
}


function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    localStorage.clear();
    window.location.replace("/Customer/Login");
}