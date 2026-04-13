async function loadAddressList() {

    //alert("okk");
    const res = await fetch(`/Common/AddressEngineHandler?handler=AddressList`); 
    const data = await res.json();

    const container = document.getElementById("addressList");
    container.innerHTML = '';

    const template = document.getElementById("addressCardTemplate").innerHTML;

    data.forEach(a => {

        let html = template
            .replace(/{{id}}/g, a.addressID)
            .replace('{{nickname}}', a.addressNickName || 'Address')
            .replace(/{{addressType}}/g, a.addressType || '')
            .replace('{{line1}}', a.addressLine1 || '')
            .replace('{{line2}}', a.addressLine2 || '')
            .replace('{{city}}', a.cityName || '')
            .replace('{{state}}', a.stateName || '')
            .replace('{{pincode}}', a.pincode || '')
            .replace('{{primaryBadge}}',
                a.isPrimary
                    ? `<span class="badge bg-success">Primary</span>`
                    : '')
            .replace('{{setPrimaryBtn}}',
                !a.isPrimary
                    ? `<button class="btn btn-sm btn-outline-success"
                                onclick="setPrimary(${a.addressID})">
                           Set Primary
                       </button>`
                    : '')
            .replace('{{deleteBtn}}',
                a.isPrimary
                    ? `<button class="btn btn-sm btn-outline-danger" disabled>        Delete              </button>`
                    : `<button class="btn btn-sm btn-outline-danger" onclick="deleteAddress(${a.addressID})">
                           Delete
                       </button>`
            );

        container.innerHTML += html;
    });

    // 🔥 Add New Card
    container.innerHTML += `
    <div class="col-md-4">
        <div class="card h-100 d-flex align-items-center justify-content-center add-address-card"
             onclick="loadAddressEngine('Warehouse')"
             style="cursor:pointer; border:2px dashed #ccc;">
            <div class="text-center text-muted py-5">
                <div style="font-size:28px;">＋</div>
                <div>Add New Address</div>
            </div>
        </div>
    </div>`;
}

/* =========================
   ACTIONS
========================= */

function editSelectedAddress(type,id) {

    //alert(type+"okk " + id);
    if (!id) {
        alert("Please select an address first.");
        return;
    }

    loadAddressEngine(type,id); // 🔥 PASS ID call _AddressModal
    loadAddressList();
}

async function deleteAddress(id) {

    if (!confirm("Are you sure you want to delete this address?")) return;

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;


    const response = await fetch(`/Common/AddressEngineHandler?handler=DeleteAddress&id=${id}`, {
        method: 'POST',
        credentials: 'same-origin',
        headers: {
            "RequestVerificationToken": token
        }
    });

    const result = await response.json();
    if (result.success) {
        showToast(result.message);
        setTimeout(() => location.reload(), 4000);
        await loadAddressList();
    } else {
        alert(result.message || "Delete failed");
        showToast(result.message);
    }
    
}

async function setPrimary(id) {

    if (!confirm("Set this as primary Address?")) return;

    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

    try {
        const response = await fetch(`/Common/AddressEngineHandler?handler=SetPrimary&id=${id}`, {
            method: 'POST',
            headers: {
                "RequestVerificationToken": token
            }
        });


        // 🔴 Handle server error properly
        if (!response.ok) {
            const text = await response.text();
            alert("Server Error: " + text);
            return;
        }

        const result = await response.json(); // ✅ correct order

        //alert(result.message); // now safe

        if (result.success) {
            showToast(`Successfully set as primary`);
            setTimeout(() => location.reload(), 4000);
            await loadAddressList();
        } else {
            alert(result.message || "Failed to update");
        }

    } catch (err) {
        alert("JS Error: " + err.message);
    }
    
}

function showToast(message) {
    const el = document.getElementById("toastMsg");
    el.innerText = message;
    el.classList.remove("d-none");

    setTimeout(() => {
        el.classList.add("d-none");
    }, 4000);
}