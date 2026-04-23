function showToastAlert(message, type = "success") {

    let el = document.getElementById("globalToast");

    if (!el) {
        el = document.createElement("div");
        el.id = "globalToast";
        el.style.position = "fixed";
        el.style.top = "20px";
        el.style.right = "20px";
        el.style.zIndex = "9999";
        document.body.appendChild(el);
    }

    el.className = `alert ${type === "error" ? "alert-danger" : "alert-success"}`;
    el.innerText = message;
    el.style.display = "block";



    setTimeout(() => {
        el.style.display = "none";
    }, 5000);
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
    }, 4000);
}