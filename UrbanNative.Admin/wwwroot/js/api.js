// wwwroot/js/api.js

window.apiFetch = async function (url, options = {}) {

    const defaults = {
        credentials: "include",
        headers: {
            "Content-Type": "application/json"
        }
    };

    const finalOptions = Object.assign({}, defaults, options);
    alert('API.js');
    alert(window.API_BASE);
    alert(url);
    const res = await fetch(`${window.API_BASE}${url}`, finalOptions);
    alert(res);
    if (!res.ok) {
        const text = await res.text();
        throw new Error(`API ${res.status}: ${text}`);
    }

    return res;
};
