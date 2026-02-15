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
    FAILED_HOLD: 7,
    RETURN_TO_ORIGIN: 8
};

/* =========================================================
   MODAL STATE
   ========================================================= */
let CURRENT_MODAL_MODE = null; // "CREATE" | "UPDATE"

/* =========================================================
   GRID-2 LOADER (Order → Items)
   ========================================================= */
function loadGrid2(orderId, city,orderNo)
{

    if (!orderId) {
        console.error("orderId missing for Grid-2");
        return;
    }
    
    //fetch(`/Vendors/Logistics?handler=Items&orderId=${orderId}`)
    fetch(`${LOGISTICS_BASE_URL}?handler=Items&orderId=${orderId}`)

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

            // Sync hidden field OrderID for modal use
            const hidden = document.getElementById("DispatchOrderId");
            if (hidden) hidden.value = orderId;
            

            // Sync dispatch button dataset
            
            const btn = document.getElementById("btnDispatch");
            if (btn) {
                btn.dataset.orderId = orderId;
                btn.dataset.city = city || "";
                btn.dataset.orderNo = orderNo || "";
            }
            // Auto load shipments grid
            loadGrid3(orderId);

            grid.scrollIntoView({ behavior: "smooth" });
        })
        .catch(err => console.error(err));
}

/* =========================================================
   GRID-3 LOADER (Shipments)
   ========================================================= */
function loadGrid3(orderId) {

    if (!orderId) return;

    //fetch(`/Logistics?handler=Shipments&orderId=${orderId}`)
    fetch(`${LOGISTICS_BASE_URL}?handler=Shipments&orderId=${orderId}`)
        .then(r => r.text())
        .then(html => {
            const grid = document.getElementById("grid3Container");
            if (grid) grid.innerHTML = html;
        });
}

/* =========================================================
   DISPATCH BUTTON CLICK (CREATE MODE)
   ========================================================= */
function onDispatchClick(btn) {

    const orderId = btn.dataset.orderId;
    const city = btn.dataset.city || "";
    const orderNo = btn.dataset.orderNo || "";

    const checked = document.querySelectorAll(".dispatch-checkbox:checked");

    if (!checked.length) {
        alert("Select at least one item");
        return;
    }

    openCreateShipmentModal(parseInt(orderId, 10), checked.length,  city, orderNo );
}

/* =========================================================
   OPEN CREATE SHIPMENT MODAL
   ========================================================= */
async function openCreateShipmentModal(orderId, itemCount, city, orderNo) {
    await loadWarehouseDropdown();
    CURRENT_MODAL_MODE = "CREATE";

    document.getElementById("UpdateShipment_ShipmentID").value = "";
    document.getElementById("ddlShipmentStatus").value = "";
    document.getElementById("ddlShipmentStatus").dataset.oldStatus = "";
    document.getElementById("ddlLogisticsProvider").value = "";
    document.getElementById("ddlLogisticsProvider").disabled = false;
    document.getElementById("ddlItemWarehouse").disabled = false;
    document.getElementById("UpdateShipment_TrackingNo").value = "";
    document.getElementById("ddlShipmentStatus").value = "READY_TO_SHIP";

    document.querySelector(".modal-title").innerText =
    `Ship ${itemCount} Item(s), Order#${orderNo} to ${city}`;

    toggleAwbFields();

    const modal = new bootstrap.Modal(document.getElementById("updateShipmentModal")    
    );

    modal.show();
}

/* =========================================================
   OPEN UPDATE SHIPMENT MODAL
   ========================================================= */
async function openUpdateShipmentModal(shipmentId,shipmentType, status, trackingNo, providerId,warehouseId, city,orderNo) {

    await loadWarehouseDropdown();
    CURRENT_MODAL_MODE = "UPDATE";
    
    document.getElementById("UpdateShipment_ShipmentID").value = shipmentId;
    document.getElementById("UpdateShipment_ShipmentType").dataset.shipmentType = shipmentType || ""; 
    document.getElementById("ddlShipmentStatus").value = status || "";
    document.getElementById("ddlShipmentStatus").dataset.oldStatus = status || "";
    document.getElementById("ddlLogisticsProvider").value = providerId || "";
    document.getElementById("ddlItemWarehouse").value = warehouseId || "";
    document.getElementById("UpdateShipment_TrackingNo").value = trackingNo || "";

    if (trackingNo) {
        document.querySelector(".modal-title").innerText =
            `AWB: ${trackingNo}, Order#${orderNo} to ${city}`;
    } else {
        document.querySelector(".modal-title").innerText =
        `Update Shipment for Order #${orderNo} to ${city}`;
    }

    toggleAwbFields();
}

/* =========================================================
   STATUS RULE ENGINE (Provider + AWB Logic)
   ========================================================= */
function toggleAwbFields() {
    
    //await loadWarehouseDropdown();
    const status = document.getElementById("ddlShipmentStatus").value;
    const awbBlock = document.getElementById("awbBlock");
    const providerDDL = document.getElementById("ddlLogisticsProvider");
    const warehouseDDL = document.getElementById("ddlItemWarehouse");

    if (!awbBlock || !providerDDL || !warehouseDDL) return;

    awbBlock.style.display = "none";
    providerDDL.disabled = false;
    warehouseDDL.disabled = false;

    // PICKUP_SCHEDULED → Provider required & Warehouse required
    if (status === "PICKUP_SCHEDULED") {
        providerDDL.disabled = false;
        warehouseDDL.disabled = false;
    }

    // PICKED_UP and above → AWB required & Provider locked
    if (
        status === "PICKED_UP" ||
        status === "IN_TRANSIT" ||
        status === "OUT_FOR_DELIVERY" ||
        status === "DELIVERED" ||
        status === "FAILED_HOLD"
    ) {
        awbBlock.style.display = "block";
        providerDDL.disabled = true;
        warehouseDDL.disabled = true;
    }
}
/* =========================================================
    WAREHOUSE LOADER (based on selected items in create mode)
========================================================= */
async function loadWarehouseDropdown() {
    const orderId = document.getElementById("DispatchOrderId")?.value;
    const res = await fetch(`/Logistics?handler=ItemWarehouses&orderId=${orderId}`);
    const data = await res.json();

    const ddl = document.getElementById("ddlItemWarehouse");
    ddl.innerHTML = '<option value="">-- Select Warehouse --</option>';

    data.forEach(w => {
        ddl.innerHTML += `<option value="${w.warehouseID}">${w.addressLine1City}</option>`;
    });

    return data; // ✅ important
}


/* =========================================================
   STATUS UPGRADE CHECK
   ========================================================= */
function isStatusUpgradeAllowed(oldStatus, newStatus) {

    if (!oldStatus) return true;

    const oldOrder = STATUS_ORDER[oldStatus];
    const newOrder = STATUS_ORDER[newStatus];

    if (!oldOrder || !newOrder) return false;

    return newOrder >= oldOrder;
}

/* =========================================================
   VALIDATION RULES
   ========================================================= */
function validateShipmentRules(status, providerId, warehouseId, trackingNo) {

    if (status === "PICKUP_SCHEDULED") {
        if (!providerId) {
            alert("Logistics Provider is mandatory for Pickup Scheduled.");
            return false;
        }
        if (!warehouseId) {
            alert("Pickup Warehouse selection is mandatory for Pickup Scheduled.");
            return false;
        }
    }

    if (
        status === "PICKED_UP" ||
        status === "IN_TRANSIT" ||
        status === "OUT_FOR_DELIVERY" ||
        status === "DELIVERED" ||
        status === "FAILED_HOLD"
    ) {
        if (!trackingNo) {
            alert("AWB / Tracking No is mandatory.");
            return false;
        }
    }

    return true;
}

/* =========================================================
   SUBMIT MODAL (CREATE + UPDATE)
   ========================================================= */
async function submitShipmentForm() {

    const status = document.getElementById("ddlShipmentStatus").value;
    const providerId = document.getElementById("ddlLogisticsProvider").value;
    const warehouseId = document.getElementById("ddlItemWarehouse").value;
    const trackingNo = document.getElementById("UpdateShipment_TrackingNo").value;

    if (!status) {
        alert("Select shipment status");
        return;
    }

    if (!validateShipmentRules(status, providerId, warehouseId, trackingNo)) {
        return;
    }

    /* ================= CREATE MODE ================= */
    if (CURRENT_MODAL_MODE === "CREATE") {

        const orderId = document.getElementById("DispatchOrderId")?.value;
        const selected = document.querySelectorAll(".dispatch-checkbox:checked");

        if (!selected.length) {
            alert("Select at least one item");
            return;
        }

        const formData = new FormData();

        formData.append("OrderID", orderId);
        formData.append("InitialShipmentStatus", status);
        formData.append("LogisticsProviderID", providerId || 0);
        formData.append("WarehouseID", warehouseId || 0);
        formData.append("TrackingNo", trackingNo || "");

        selected.forEach(cb => {
            formData.append("OrderItemIds", cb.value);
        });

        //const url = `${LOGISTICS_BASE_URL}?handler=CreateShipment`;
        const url = `/Logistics?handler=CreateShipment`;
        //alert(url);
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const res = await fetch(url, {
            method: "POST",
            headers: {
                "RequestVerificationToken": token
            },
            body: formData
        });
              
        if (!res.ok) {

            let msg = "";

            try {
                msg = await res.text();
            } catch { }

            if (!msg) {
                msg = "Shipment not created. Please check required fields.";
            }

            alert(msg);
            return;
        }


        bootstrap.Modal.getInstance(
            document.getElementById("updateShipmentModal")
        ).hide();

        refreshAfterShipmentChange();
        return;
    }

    /* ================= UPDATE MODE ================= */
    if (CURRENT_MODAL_MODE === "UPDATE") {

        const shipmentId = document.getElementById("UpdateShipment_ShipmentID").value;
        const oldStatus = document.getElementById("ddlShipmentStatus").dataset.oldStatus;
        const shipmentType = document.getElementById("UpdateShipment_ShipmentType").dataset.shipmentType;

        if (!isStatusUpgradeAllowed(oldStatus, status)) {
            alert("Status downgrade is not allowed.");
            return;
        }

        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const formData = new FormData();
        formData.append("UpdateShipment.ShipmentID", shipmentId);
        formData.append("UpdateShipment.ShipmentType", shipmentType);
        formData.append("UpdateShipment.NewShipmentStatus", status);
        formData.append("UpdateShipment.LogisticsProviderID", providerId || "");
        formData.append("UpdateShipment.WarehouseID", warehouseId || "");
        formData.append("UpdateShipment.TrackingNo", trackingNo || "");

        //const url = `${LOGISTICS_BASE_URL}?handler=UpdateShipment`; 
        const url = `/Logistics?handler=UpdateShipment`;
        //alert(url);
        const res = await fetch(url, {
            method: "POST",
            headers: {
                "RequestVerificationToken": token
            },
            body: formData
        });

        if (!res.ok) {
            let msg = "";
            try { msg = await res.text(); } catch { }
            if (!msg) msg = "Shipment update failed.";
            alert(msg);
            return;
        }

        bootstrap.Modal.getInstance(
            document.getElementById("updateShipmentModal")
        ).hide();

        refreshAfterShipmentChange();
    }
}

/* =========================================================
   REFRESH AFTER STATUS UPDATE
   ========================================================= */
function refreshAfterShipmentChange() {

    const orderId = document.getElementById("DispatchOrderId")?.value;

    if (orderId) {
        loadGrid2(orderId);
        loadGrid3(orderId);
    }
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
   SMART SEARCH AUTOCOMPLETE
   ========================================================= */
document.addEventListener("DOMContentLoaded", function () {
    
    const input = document.getElementById("logisticsSearchInput");
    const list = document.getElementById("logisticsSearchDropdown");
    
    if (!input || !list) return;

    input.addEventListener("input", async function () {
        const q = this.value.trim();
        
        if (!q || q.length < 2) {
            list.classList.add("d-none");
            return;
        }
        //alert("okk" + q);
        const res = await fetch(`/Logistics?handler=LogisticsSuggestions&term=${encodeURIComponent(q)}`);


        if (!res.ok) return;

        const data = await res.json();
        list.innerHTML = "";

        if (!data.length) {
            list.classList.add("d-none");
            return;
        }

        data.forEach(x => {
            const li = document.createElement("li");
            li.className = "list-group-item list-group-item-action";
            li.innerHTML = `
                <strong>${x.displayText}</strong>
                <small class="text-muted"> | ${x.suggestionType}</small>
            `;
            li.addEventListener("click", () => {
                input.value = x.searchValue;
                list.classList.add("d-none");
                input.focus();
            });
            list.appendChild(li);
        });

        list.classList.remove("d-none");
    });

    input.addEventListener("keydown", e => {
        if (e.key === "Enter") {
            e.preventDefault();
            input.form.submit();
        }
    });

    document.addEventListener("click", e => {
        if (!e.target.closest("#logisticsSearchDropdown") &&
            e.target !== input) {
            list.classList.add("d-none");
        }
    });
});