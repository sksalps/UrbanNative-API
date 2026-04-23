
document.addEventListener("DOMContentLoaded", function () {
    //alert("add address");
    const btn = document.getElementById("btnAddAddress");

    if (!btn) {
        console.warn("btnAddAddress not found");
        return;
    }

    btn.addEventListener("click", async function () {

        if (!isVerified) {
            showToast("Verify mobile/email first", "error");
            return;
        }
        await  ensureTempUser();

         //alert("okk 23"); // debug

        const pincode = document.getElementById("txtPincode").value;

        if (!pincode || pincode.length < 6) {
            showToast("Enter valid pincode", "error");
            return;
        }

        loadAddressEngine("NATIVE", null, pincode);
    });
});


async function ensureTempUser() {

    try {
        let sessionId = localStorage.getItem("UN_SESSION");

        // 🔥 Always ensure session exists
        if (!sessionId) {
            sessionId = crypto.randomUUID();
            localStorage.setItem("UN_SESSION", sessionId);
        }

        const csrf = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        const res = await fetch("/Auth/Register?handler=Insert", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": csrf
            },
            body: JSON.stringify({
                sessionID: sessionId
            })
        });

        if (!res.ok) {
            console.error("Temp user API failed:", res.status);
            return null;
        }

        const text = await res.text();

        if (!text) {
            console.warn("Empty response from temp user API");
            return null;
        }

        let data;
        try {
            data = JSON.parse(text);
        } catch (e) {
            console.error("Invalid JSON:", text);
            return null;
        }
        //alert(data.tempID);
        if (data.success) {
            localStorage.setItem("TEMP_ID", data.tempID);
            localStorage.setItem("TEMP_TOKEN", data.tempToken);
            localStorage.setItem("TEMP_SESSION", sessionId);
        }

        return data;

    } catch (err) {
        console.error("ensureTempUser error:", err);
        return null;
    }
}
function onAddressSelected(address) {

    console.log("Selected Address:", address);

    document.getElementById("selectedAddressId").value = address.addressID;
    document.getElementById("selectedAddressText").value = address.fullAddress;

    document.getElementById("addressPreview").innerText =
        address.fullAddress;
}


//User registration permanent 
document.getElementById("btnRegister")?.addEventListener("click", async function () {
    alert("OKK script register");
    const btn = this;
    if (!isVerified) { return; }
    
    // ==========================
    // 🔍 READ INPUTS
    // ==========================
    const urlParams = new URLSearchParams(window.location.search);

    var tempIdRaw = urlParams.get("tempId");
    var tempToken = urlParams.get("token");
    if (!tempIdRaw) {
        await ensureTempUser();
        tempIdRaw = localStorage.getItem("TEMP_ID", data.tempID);
        tempToken = localStorage.getItem("TEMP_TOKEN", data.tempToken);
    }
    const name = document.getElementById("Name").value.trim();
    const mobile = document.getElementById("Mobile").value.trim();
    const email = document.getElementById("Email").value.trim();
    const pincode = document.getElementById("txtPincode").value.trim();

    const addressId = document.getElementById("selectedAddressId").value;

    const referredByUserId = document.getElementById("referredByUserId")?.value;

    // ==========================
    // 🔐 VALIDATION
    // ==========================
    if (!name) {
        showToast("Enter full name", "error");
        return;
    }

    if (!mobile || mobile.length < 10) {
        showToast("Enter valid mobile", "error");
        return;
    }
    //if (!addressId) {
    //    showToast("Please select your native address", "error");
        //return;    }

    // ==========================
    // 📦 BUILD REQUEST
    // ==========================
    const data = {
        TempID: tempIdRaw ? parseInt(tempIdRaw) : null,
        TempToken: tempToken || null,

        Name: name,
        Mobile: mobile,
        Email: email || null,
        Pincode: pincode || null,

        AddressID: addressId ? parseInt(addressId) : 0,
        ReferredByUserID: referredByUserId ? parseInt(referredByUserId) : 0
    };
    //alert(data.TempID + " okk " + data.TempToken);
    console.log("Register Payload:", data);

    // ==========================
    // 🚫 PREVENT DOUBLE CLICK
    // ==========================
    btn.disabled = true;
    btn.innerText = "Saving...";
    try {

        const csrf = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const res = await fetch("/Auth/Register?handler=Complete", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": csrf
            },
            body: JSON.stringify(data)
        });

        // ==========================
        // 🧠 HANDLE NON-JSON RESPONSE
        // ==========================
        const text = await res.text();

        let result;

        try {
            result = JSON.parse(text);
        } catch {
            console.error("Invalid JSON response:", text);
            showToast("Server error. Try again", "error");
            return;
        }

        console.log("Register Response:", result);

        const status = result.status || result.Status;

        // ==========================
        // ✅ SUCCESS
        // ==========================
        if (status === "SUCCESS") {
            showToast("Registration successful");

            setTimeout(() => {
                window.location.href = "/Dashboard/Index";
            }, 2000);
            return;
        }

        // ==========================
        // ❌ ERROR FROM API
        // ==========================
        else {
            showToast(result.message || result.Message || "Registration failed", "error");
        }

    } catch (err) {
        console.error("Register Error:", err);
        showToast("Something went wrong", "error");
    }

    // ==========================
    // 🔁 RESET BUTTON
    // ==========================
    btn.disabled = false;
    btn.innerText = "Continue";

});

//document.getElementById("Mobile").addEventListener("input", resetVerification);
//document.getElementById("Email").addEventListener("input", resetVerification);

function resetVerification() {
    isVerified = false;
    document.getElementById("verifiedBadge").style.display = "none";
    document.getElementById("btnAddAddress").disabled = true;
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

document.addEventListener("DOMContentLoaded", function () {

    document.getElementById("btnRegSendOtp")?.addEventListener("click", async () => {
        //alert("OTP clicked"); // debug
    //console.log("JS loaded");
        console.log(document.getElementById("btnRegSendOtp"));

    const mobile = document.getElementById("Mobile").value.trim();
    const email = document.getElementById("Email").value.trim();
    if (!mobile && !email) {
        showToast("Enter Mobile or Email", "error");
        return;
    }

    // 🔥 Mobile mandatory if email empty
    if (!email && mobile.length < 10) {
        showToast("Enter valid mobile", "error");
        return;
    }

        const identifier = mobile || email;
        
        console.log("JS identifier" + identifier);
        //if (typeof sendAuthOtp !== 'function') { console.error('sendAuthOtp not available'); showToast('Internal error', 'error'); return; }
    // call send OTP API
        const res = await sendAuthOtp(identifier);
        //console.log(res+"JS after call" + identifier);
        if (!res.success) {
            showToast("Failed to send OTP", "error");
            return { success: false };
        }

        //const data = await res.json();
        var otpValue = res.otp || res.OTP;

        showToast("OTP: " + otpValue); //temp - remove in prod

    document.getElementById("otpSection").style.display = "block";
});
});
function resendRegOtp() {
    document.getElementById("btnRegSendOtp").click();
}

async function verifyRegOtp() {
    alert("Reg Vierify");

    const mobile = document.getElementById("Mobile").value.trim();
    const email = document.getElementById("Email").value.trim();
    const identifier = mobile || email;
    const otp = document.getElementById("txtOtp").value;

    if (otp.length !== 6) {
        showToast("Enter valid OTP", "error");
        return;
    }

    //const res = await verifyOtp(); // your API
    const result = await verifyAuthOTP(identifier, otp);
    const data = result.result;
    if (!data.success) {
        showToast("Verification failed. Try again", "error");
        this.disabled = false;
        this.innerText = "Verify OTP";
        return;
    }
    //var data = await result.json();
    if (data?.status === "NEW_USER") {

        isVerified = true;

        document.getElementById("verifiedBadge").style.display = "inline-block";

        document.getElementById("btnAddAddress").disabled = false;
        document.getElementById("txtOtp").value = '';
        document.getElementById("otpSection").style.display = "none";

        showToast("Verified successfully");

    } else if (data?.status === "LOGIN") {
        //logic to ask user for redirect to dashboard, due to verified identifier exist

        isVerified = true;

        document.getElementById("verifiedBadge").style.display = "inline-block";

        //document.getElementById("btnAddAddress").disabled = false;
        document.getElementById("txtOtp").value = '';
        document.getElementById("otpSection").style.display = "none";

        showToast("Verified successfully");

    }

    else {
        showToast("Invalid OTP", "error");
    }
}

document.getElementById("btnRegVerifyOtp1")?.addEventListener("click", async () => {
    //alert("Reg Vierify");

    const mobile = document.getElementById("Mobile").value.trim();
    const email = document.getElementById("Email").value.trim();
    const identifier = mobile || email;
    const otp = document.getElementById("txtOtp").value;

    if (otp.length !== 6) {
        showToast("Enter valid OTP", "error");
        return;
    }

    //const res = await verifyOtp(); // your API
    const result = verifyAuthOTP(identifier, parseInt(otp));
    if (!result.ok) {
        showToast("Verification failed. Try again", "error");
        this.disabled = false;
        this.innerText = "Verify OTP";
        return;
    }
    var data = await result.json();
    if (data?.type === "NEW_USER") {

        isVerified = true;

        document.getElementById("verifiedBadge").style.display = "inline-block";

        document.getElementById("btnAddAddress").disabled = false;

        showToast("Verified successfully");

    } else if (data?.type === "LOGIN") {
        //logic to ask user for redirect to dashboard, due to verified identifier exist
        
        isVerified = true;

        document.getElementById("verifiedBadge").style.display = "inline-block";

        //document.getElementById("btnAddAddress").disabled = false;

        showToast("Verified successfully");

    }

    else {
        showToast("Invalid OTP", "error");
    }
});

    