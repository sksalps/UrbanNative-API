// ==========================
// 🚀 API HELPER
// ==========================

async function apiGet(url) {
    try {
        const res = await fetch(url);

        if (!res.ok) {
            throw await res.text();
        }

        const text = await res.text();
        return safeParseJson(text);

    } catch (err) {
        console.error("GET Error:", err);
        showToast("Failed to load data", "error");
        return null;
    }
}

async function apiPost(url, data) {
    try {
        const res = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": getAntiForgeryToken()
            },
            body: JSON.stringify(data)
        });

        const text = await res.text();
        const json = safeParseJson(text);

        if (!res.ok) {
            throw json?.message || "API error";
        }

        return json;

    } catch (err) {
        console.error("POST Error:", err);
        showToast(err || "Something went wrong", "error");
        return null;
    }
}

async function apiPostForm(url, formData) {
    try {
        const res = await fetch(url, {
            method: "POST",
            headers: {
                "RequestVerificationToken": getAntiForgeryToken()
            },
            body: formData
        });

        const text = await res.text();
        return safeParseJson(text);

    } catch (err) {
        console.error("FORM Error:", err);
        showToast("Upload failed", "error");
        return null;
    }
}