window.apiFetch = async function(url, options = { }) {
    const defaults = {
        credentials: "include",
        headers:
    {
        "Content-Type": "application/json"
        }
}
;

const res = await fetch(
        `${window.API_BASE}${ url}`,
        Object.assign(defaults, options)
    );

if (!res.ok)
{
    const text = await res.text();
    throw new Error(`API ${ res.status }: ${ text}`);
}

return res;
};
