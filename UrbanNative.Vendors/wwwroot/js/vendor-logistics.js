/* =========================================================
   GLOBAL STATUS ORDER
   ========================================================= */
const STATUS_ORDER = {
    READY_TO_SHIP: 1,
    PICKUP_SCHEDULED: 2,
    PICKED_UP: 3,
    IN_TRANSIT: 4,
    OUT_FOR_DELIVERY: 5,
    DELIVERED: 6,
    FAILED: 7,
    RETURN_TO_ORIGIN: 8
};

/* =========================================================
   GRID-2 LOADER (Order → Items)
   ========================================================= */
function loadGrid2(orderId) {
    alert(orderId);
    if (!orderId) {
        console.error("orderId missing for Grid-2");
        return;
    }

    fetch(`/Logistics?handler=Items&orderId=${orderId}`)
        .then(r => {
            if (!r.ok) throw new Error("Grid-2 fetch failed");
            return r.text();
        })
        .then(html => {
            const grid = document.getElementById("grid2Container");
            if (!grid) {
                console.error("grid2Container not found");
                return;
            }

            grid.innerHTML = html;
            grid.scrollIntoView({ behavior: "smooth" });

            // sync dispatch button
            const btn = document.getElementById("btnDispatch");
            if (btn) btn.dataset.orderId = orderId;
        })
        .catch(err => console.error(err));
}

/* =========================================================
   GRID-3 LOADER (Shipments)
   ========================================================= */
function loadGrid3(orderId) {
    if (!orderId) return;

    fetch(`/Logistics?handler=Shipments&orderId=${orderId}`)
        .then(r => r.text())
        .then(html => {
            const grid = document.getElementById("grid3Container");
            if (grid) grid.innerHTML = html;
        });
}

/* =========================================================
   DISPATCH BUTTON → CREATE MODAL
   ========================================================= */
function onDispatchClick(btn) {
    const orderId = btn.dataset.orderId;
    const city = btn.dataset.city || "";

    const checked = document.querySelectorAll(".dispatch-checkbox:checked");
    if (!checked.length) {
        alert("Select at least one item");
        return;
    }

    openCreateShipmentModal(
        parseInt(orderId, 10),
        checked.length,
        city
    );
}

/* =========================================================
   SELECTION COUNT
   ========================================================= */
function updateDispatchCount() {
    const checked = document.querySelectorAll(".dispatch-checkbox:checked");
    const btn = document.getElementById("btnDispatch");
    const countSpan = document.getElementById("selectedCount");

    if (countSpan) countSpan.innerText = checked.length;
    if (btn) btn.disabled = checked.length === 0;
}

/* =========================================================
   STATUS RULE ENGINE
   ========================================================= */
function toggleAwbFields() {
    applyShipmentRules();
}
