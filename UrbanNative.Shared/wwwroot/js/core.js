// ==========================
// 🌐 CORE UTILITIES
// ==========================

// 🔐 Anti-forgery token
function getAntiForgeryToken() {
    return document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';
}

// 📦 Safe JSON parse
function safeParseJson(text) {
    try {
        return JSON.parse(text);
    } catch (e) {
        console.error("Invalid JSON:", text);
        return null;
    }
}

// ⏳ Button loader
function setButtonLoading(btn, isLoading, text = "Processing...") {
    if (!btn) return;

    if (isLoading) {
        btn.dataset.originalText = btn.innerText;
        btn.innerText = text;
        btn.disabled = true;
    } else {
        btn.innerText = btn.dataset.originalText || "Submit";
        btn.disabled = false;
    }
}

// 📍 Query param helper
function getQueryParam(name) {
    return new URLSearchParams(window.location.search).get(name);
}

// 📣 Global event helper
function dispatchEventGlobal(name, detail = {}) {
    document.dispatchEvent(new CustomEvent(name, { detail }));
}