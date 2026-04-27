let isVerified = false;
let isMobileVerified = false;
let isEmailVerified = false;


// ==========================
// 🔹 INIT
// ==========================
document.addEventListener("DOMContentLoaded", () => {

    const identifier = localStorage.getItem("VERIFIED_IDENTIFIER");
    const type = localStorage.getItem("VERIFIED_TYPE");

    if (!identifier || !type) return;
    
    if (type === "MOBILE") {
        const mobileInput = document.getElementById("Mobile");

        if (mobileInput) {
            mobileInput.value = identifier;
            setMobileVerified();
        } else {
            console.warn("Mobile input not found");
        }
    }

    if (type === "EMAIL") {
        const emailInput = document.getElementById("Email");

        if (emailInput) {
            emailInput.value = identifier;
            setEmailVerified();
        }
    }

    // 🔥 enable address button
    const btnAddress = document.getElementById("btnAddAddress");

    if (!btnAddress) {
        console.warn("btnAddAddress not found");
        return;
    }
    btnAddress.disabled = false;


    const tncContent = document.getElementById("tncContent");
    const btnAccept = document.getElementById("btnAcceptTnC");

    if (!tncContent || !btnAccept) return;

    // enable Accept after scroll
    tncContent.addEventListener("scroll", function () {
        if (tncContent.scrollTop + tncContent.clientHeight >= tncContent.scrollHeight - 5) {
            btnAccept.disabled = false;
        }
    });

    // Accept click
    btnAccept.addEventListener("click", function () {

        document.getElementById("chkTnC").disabled = false;
        document.getElementById("chkTnC").checked = true;

        bootstrap.Modal.getInstance(document.getElementById('tncModal')).hide();
    });
});


// ==========================
// 🔹 VERIFY STATE
// ==========================
function setMobileVerified() {

    isVerified = true;
    isMobileVerified = true;

    document.getElementById("verifiedBadge").style.display = "inline-block";
    document.getElementById("changeMobile").style.display = "inline";


    // 🔥 Disable field
    document.getElementById("Mobile").disabled = true;
    document.getElementById("btnRegSendOtp").style.display = "none";


    document.getElementById("btnAddAddress").disabled = false;
}

function resetMobile() {

    isVerified = false;
    isMobileVerified = false;

    document.getElementById("verifiedBadge").style.display = "none";
    document.getElementById("changeMobile").style.display = "none";
    // 🔥 Enable field again
    document.getElementById("Mobile").disabled = false;
    document.getElementById("btnRegSendOtp").style.display = "block";
    document.getElementById("btnAddAddress").disabled = true;

    document.getElementById("txtOtp").value = "";
    document.getElementById("otpSection").style.display = "none";
}

// ==========================
// 🔹 EMAIL VERIFY STATE
// ==========================
function setEmailVerified() {

    isVerified = true;
    isEmailVerified = true;

    document.getElementById("emailVerifiedBadge").style.display = "inline-block";
    document.getElementById("changeEmail").style.display = "inline";

    // 🔥 Disable field
    document.getElementById("Email").disabled = true;
}

function resetEmail() {

    isVerified = false;
    isEmailVerified = false;

    document.getElementById("emailVerifiedBadge").style.display = "none";
    document.getElementById("changeEmail").style.display = "none";

    // 🔥 Enable field
    document.getElementById("Email").disabled = false;
}

// ==========================
// 🔹 EMAIL VALIDATION
// ==========================
document.addEventListener("DOMContentLoaded", function () {

    const emailInput = document.getElementById("Email");

    if (!emailInput) {
        console.warn("Email input not found");
        return;
    }

    emailInput.addEventListener("input", function () {

        const email = this.value.trim();
        const err = document.getElementById("err_email");

        if (!email) {
            err.innerText = "";
            return;
        }

        const isValid = checkEmail(email);

        if (!isValid) {
            err.innerText = "Invalid email format";
        } else {
            err.innerText = "";
            document.getElementById("btnRegSendOtp").style.display = "block";
        }
    });

});



function resendRegOtp() {
    document.getElementById("btnRegSendOtp").click();
}

// ==========================
// 🔹 VALIDATION
// ==========================
function validateForm() {

    let valid = true;

    const name = document.getElementById("Name").value.trim();
    const mobile = document.getElementById("Mobile").value.trim();
    const email = document.getElementById("Email").value.trim();

    document.getElementById("err_name").innerText = "";
    document.getElementById("err_mobile").innerText = "";
    document.getElementById("err_email").innerText = "";

    if (!name) {
        document.getElementById("err_name").innerText = "Name is required";
        valid = false;
    }

    if (!mobile && !email) {
        document.getElementById("err_mobile").innerText = "Mobile or Email required";
        valid = false;
    }
    if (mobile && mobile.includes("@") && !checkEmail(mobile )) {
        document.getElementById("err_mobile").innerText = "Invalid email entered";
        valid = false;
    }

    if (mobile && !mobile.includes("@") && mobile.length < 10) {
        document.getElementById("err_mobile").innerText = "Invalid mobile entered";
        valid = false;
    }
    if (email && !checkEmail(email)) {
        document.getElementById("err_email").innerText = "Invalid email entered";
        valid = false;
    }
    //if email is entered in mobile field as well as email field, show error
    if (mobile && mobile.includes("@") && email) {
        document.getElementById("err_mobile").innerText = "Invalid mobile entered";
        valid = false;
    }
    return valid;
}
function checkEmail(email) {
    if (!email) return true; // allow empty (optional field)

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}
// ==========================
// 🔹 ADD ADDRESS
// ==========================
document.addEventListener("DOMContentLoaded", function () {

    const btn = document.getElementById("btnAddAddress");
    if (!btn) return;

    btn.addEventListener("click", async function () {

        if (!isVerified) {
            showToast("Verify mobile/email first", "error");
            return;
        }

        await ensureTempUser();

        const pincode = document.getElementById("txtPincode").value;

        if (!pincode || pincode.length < 6) {
            showToast("Enter valid pincode", "error");
            return;
        }

        loadAddressEngine("NATIVE", null, pincode);
    });
});

// ==========================
// 🔹 TEMP USER
// ==========================
async function ensureTempUser() {

    try {
        let sessionId = localStorage.getItem("UN_SESSION");

        if (localStorage.getItem("TEMP_ID") && localStorage.getItem("TEMP_TOKEN")) {
            return;
        }

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
            body: JSON.stringify({ sessionID: sessionId })
        });

        const data = await res.json();

        if (data.success) {
            localStorage.setItem("TEMP_ID", data.tempID);
            localStorage.setItem("TEMP_TOKEN", data.tempToken);
        }

    } catch (err) {
        console.error(err);
    }
}

// ==========================
// 🔹 ADDRESS SELECT
// ==========================
window.onAddressSelected = function (data) {

    document.getElementById("selectedAddressId").value = data.addressID;

    previewAddressById(data.addressID, "addressPreview");

    const pincodeInput = document.getElementById("txtPincode");
    if (pincodeInput) {
        pincodeInput.dispatchEvent(new Event("change"));
    }
};

// ==========================
// 🔹 SEND OTP
// ==========================
document.addEventListener("DOMContentLoaded", function () {

    document.getElementById("btnRegSendOtp")?.addEventListener("click", async () => {
        const mobile = document.getElementById("Mobile").value.trim();
        const email = document.getElementById("Email").value.trim();

        if (!mobile && !email) {
            showToast("Enter Mobile or Email", "error");
            return;
        }

        //If email entered in mobile field, validate it
        if (mobile && mobile.includes("@") && !checkEmail(mobile)) {        
                showToast("Enter valid email/mobile", "error");
                return;
        }
        //if email is entered in mobile field as well as email field, show error
        if (mobile && mobile.includes("@") && email) {
            showToast("Enter correct mobile number ", "error");
            return;
        }

        //if mobile is entered and it's less than 10 digits, show error
        if (mobile && !mobile.includes("@") && mobile.length < 10) {
            showToast("Enter valid mobile", "error");
            return;
            }
        //Validate email if entered in email field

        if (email && !checkEmail(email)) {
            showToast("Enter valid email", "error");
            return;
        }
        

        const identifier = mobile || email;

        const res = await sendAuthOtp(identifier);

        if (!res.success) {
            showToast("Failed to send OTP", "error");
            return;
        }

        showToast("OTP sent successfully to "+identifier);

        document.getElementById("otpSection").style.display = "block";
        startResendTimer();
    });
});

// ==========================
// 🔹 VERIFY OTP
// ==========================
async function verifyRegOtp() {

    const mobile = document.getElementById("Mobile").value.trim();
    const email = document.getElementById("Email").value.trim();

    if (!mobile && !email) {
        showToast("Enter Mobile or Email", "error");
        return;
    }

    //If email entered in mobile field, validate it
    if (mobile && mobile.includes("@") && !checkEmail(mobile)) {
        showToast("Enter valid email/mobile", "error");
        return;
    }
    //if email is entered in mobile field as well as email field, show error
    if (mobile && mobile.includes("@") && email) {
        showToast("Enter correct mobile number ", "error");
        return;
    }

    //if mobile is entered and it's less than 10 digits, show error
    if (mobile && !mobile.includes("@") && mobile.length < 10) {
        showToast("Enter valid mobile", "error");
        return;
    }
    //Validate email if entered in email field

    if (email && !checkEmail(email)) {
        showToast("Enter valid email", "error");
        return;
    }


    let identifier = mobile || email;

    if (identifier && identifier.includes("@")) {
        identifier = identifier.toLowerCase();
    }

    const otp = document.getElementById("txtOtp").value;

    if (!otp || otp.length !== 6) {
        showToast("Enter valid OTP", "error");
        return;
    }

    const res = await verifyAuthOTP(identifier, otp);

    const data = res.result;

    if (!data || !data.success) {
        showToast("Verification failed. Try again", "error");
        this.disabled = false;
        this.innerText = "Verify OTP";
        return;
    }
    document.getElementById("otpSection").style.display = "none";
    if (identifier.includes("@")) {
        setEmailVerified();
    } else {
        setMobileVerified();
    }

    if (data.status === "LOGIN") {
        const user = data.userExist;


        if (!user) {
            showToast("User data missing", "error");
            return;
        }

        const confirmRedirect = confirm(
            "Already registered.\n\nOK → Dashboard\nCancel → Change number"
        );

        if (confirmRedirect) {
            window.location.href = "/Dashboard/Index";
        } else {
            resetMobile();
        }

        return;
    }

    document.getElementById("txtOtp").value = "";
    document.getElementById("otpSection").style.display = "none";

    showToast("Verified successfully");
}

// ==========================
// 🔹 REGISTER
// ==========================


document.addEventListener("DOMContentLoaded", function () {
    //alert("ookk");

    
    const btn = document.getElementById("btnRegister");

    if (!btn) {
        console.warn("btnRegister not found");
        return;
    }

    btn.addEventListener("click", async function () {
        

        //console.log("Register clicked"); // 🔥 debug

        if (!validateForm()) return;

        const name = document.getElementById("Name").value.trim();
        const mobile = document.getElementById("Mobile").value.trim();
        const email = document.getElementById("Email").value.trim();
        

        if (!isMobileVerified && !isEmailVerified) {
            showToast("Verify mobile or email to continue", "error");
            return;
        }
        // Terms & Conditions check
        const chkd = document.getElementById("chkTnC");
        if (!chkd) {
            console.warn("chkTnC not found");
            return;
        }
        if (!chkd.checked) {
            showToast("Please accept Terms & Conditions", "error");
            return;
        }
        const pincode = document.getElementById("txtPincode").value.trim();
        const addressId = document.getElementById("selectedAddressId").value;
        const referredBy = document.getElementById("referredByUserId").value;
        var tempID = localStorage.getItem("TEMP_ID").trim();
        var tempToken = localStorage.getItem("TEMP_TOKEN").trim();


        const data = {
            TempID: tempID ? parseInt(tempID) : 0, 
            TempToken: tempToken || "",
            Name: name,
            Mobile: mobile,
            Email: email || null,
            //Pincode: pincode || null,
            AddressID: addressId ? parseInt(addressId) : 0,
            ReferredByUserID: referredBy ? parseInt(referredBy) : 0
        };

        const csrf = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const res = await fetch("/Auth/Register?handler=Complete", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": csrf
            },
            body: JSON.stringify(data)
        });

        const result = await res.json();
        //const text = await result.text();
        console.log("RAW ERROR:", result);

        if (result.status === "SUCCESS") {
            showToast("Registration successful");
            localStorage.removeItem("VERIFIED_IDENTIFIER");
            localStorage.removeItem("VERIFIED_TYPE");

            setTimeout(() => {
                window.location.href = "/Dashboard/Index";
            }, 1500);
        } else {
            showToast("Registration failed", "error");
        }
    });

});




function openTnCModal() {
    const modal = new bootstrap.Modal(document.getElementById('tncModal'));
    modal.show();
}

