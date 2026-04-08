async function openAddressModal(addType) {
    alert("Please select an existing address or add a new one.");
    alert(addType);
    const res = await fetch(`/Warehouse/AddEdit?handler=AddressList&type=WAREHOUSE`);

    if (!res.ok) {
        alert("Failed to load addresses");
        return;
    }

    const data = await res.json();

    let ddl = document.getElementById("existingAddress");
    ddl.innerHTML = '<option value="">--Select--</option>';

    data.forEach(x => {
        ddl.innerHTML += `<option value="${x.addressID}">
            ${x.addressLine1} (${x.pincode})
        </option>`;
    });

    new bootstrap.Modal(document.getElementById('addressModal')).show();
}


async function saveAddress() {

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    const dto = {
        addressLine1: document.getElementById("line1").value,
        addressLine2: document.getElementById("line2").value,
        landmark: document.getElementById("landmark").value,
        pincode: document.getElementById("pincode").value,
        addressType: "WAREHOUSE"
    };

    const res = await fetch(`/Warehouse/AddEdit?handler=SaveAddress`, {
        method: 'POST',
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": token
        },
        body: JSON.stringify(dto)
    });

    const result = await res.json();

    if (!result.success) {
        alert(result.message);
        return;
    }

    await refreshAddressDropdown(result.addressId);

    bootstrap.Modal.getInstance(document.getElementById('addressModal')).hide();
}