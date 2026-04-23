let countryTS = null, stateTS = null, cityTS = null;
let currentAddressType = '';

/* =========================
   🔹 CONFIG (CREATE ENABLED)
========================= */
function getTomConfig() {
    return {
        create: function (input) {
            if (!input || input.trim().length < 2) return false;

            return {
                value: input.trim(),
                text: input.trim()
            };
        },

        createOnBlur: true,
        persist: false,

        maxOptions: 500,
        valueField: 'value',
        labelField: 'text',
        searchField: ['text'],

        placeholder: "Type to search...",
        selectOnTab: true,

        render: {
            option_create: function (data, escape) {
                return `<div class="create">➕ Add "<strong>${escape(data.input)}</strong>"</div>`;
            }
        }
    };
}

/* =========================
   🔹 INIT ONCE
========================= */
function initTomSelects() {

    if (!countryTS) {
        countryTS = new TomSelect("#country", getTomConfig());

        countryTS.on('change', function (value) {
            loadStates(value);
        });
    }

    if (!stateTS) {
        stateTS = new TomSelect("#state", getTomConfig());

        stateTS.on('change', function (value) {
            loadCities(value);
        });
    }

    if (!cityTS) {
        cityTS = new TomSelect("#city", getTomConfig());
    }
}
/*
function setAddressMode(isEdit) {

    const title = document.getElementById("addressModeTitle");
    const btn = document.getElementById("btnSave");

    if (isEdit) {
        title.innerText = "Edit Address";
        btn.innerText = "Update Address";
    } else {
        title.innerText = "Add New Address";
        btn.innerText = "Save Address";
    }
}
*/
/* =========================
   🔹 MODE HANDLING
========================= */
function setAddressMode(mode) {

    currentMode = mode;

    const title = document.getElementById("addressModeTitle");
    const btnSave = document.getElementById("btnSave");
    const btnSelectGo = document.getElementById("btnSelectGo");

    switch (mode) {

        case "NEW":
            title.innerText = "Add New Address";
            btnSave.innerText = "Save Address";
            btnSave.disabled = false;
            btnSelectGo.classList.add("d-none");
            break;

        case "VIEW":
            title.innerText = "Select Address";
            btnSave.innerText = "Edit Address";
            btnSave.disabled = false;
            btnSelectGo.classList.remove("d-none");
            break;

        case "EDIT":
            title.innerText = "Edit Address";
            btnSave.innerText = "Update Address";
            btnSave.disabled = false;
            btnSelectGo.classList.add("d-none");
            break;
    }
}
/* =========================
   🔹 MODAL OPEN
========================= */

async function openAddressModal(type, selectedId = null, pincode = null) {
    
    currentAddressType = type;
    document.getElementById("addressTypeDisplay").value = type;

    // 🔥 SET MODE HERE
    setAddressMode(!!selectedId);

    const modalEl = document.getElementById('addressModal');
    const modal = new bootstrap.Modal(modalEl);

    modal.show();

    modalEl.addEventListener('shown.bs.modal', async function handler() {

        modalEl.removeEventListener('shown.bs.modal', handler);

        initTomSelects();

        await loadExistingAddresses(type);

        if (selectedId) {
            document.getElementById("existingAddress").value = selectedId;
            setAddressMode("VIEW");
            await loadAddressById(id);
        } else {
            clearAddressForm();
            setAddressMode("NEW");
        }

        await loadCountries();
    });

    

    // 🔥 PINCODE PREFILL
    if (pincode && pincode.length === 6) {
        document.getElementById("pincode").value = pincode;
        fetchPincodeDetails();
        //setTimeout(fetchPincodeDetails, 200);
    }
}
/* =========================
   🔹 LOAD EXISTING ADDRESS
========================= */
async function loadExistingAddresses(type) {


    const res = await fetch(`/Common/AddressEngineHandler?handler=AddressLookup&type=${type}`);
    
    const data = await res.json();
    if (!data) return;
    let ddl = document.getElementById("existingAddress");
    ddl.innerHTML = '<option value="">-- Select Address --</option>';

    data.forEach(x => {
        ddl.innerHTML += `<option value="${x.addressID}">
            ${x.addressNickName} ${x.addressLine1} (${x.pincode})
        </option>`;
    });
}

/* =========================
   🔹 LOAD COUNTRIES
========================= */
async function loadCountries(selectedId = null) {
    //alert("cntry");
    const res = await fetch(`/Common/AddressEngineHandler?handler=Countries`);
    const data = await res.json();
    if (!data) return;
    countryTS.clearOptions();

    countryTS.addOptions(
        data.map(x => ({
            value: x.countryID,
            text: x.countryName
        }))
    );

    countryTS.refreshOptions(false);

    if (selectedId) {
        countryTS.setValue(selectedId);
    } else {
        countryTS.clear();
    }

    // 🔥 Reset cascade
    stateTS.clear();
    stateTS.clearOptions();
    cityTS.clear();
    cityTS.clearOptions();
}

/* =========================
   🔹 LOAD STATES
========================= */
async function loadStates(countryId, selectedId = null) {
    //alert("state");
    stateTS.clear();
    stateTS.clearOptions();

    cityTS.clear();
    cityTS.clearOptions();

    if (!countryId) return;

    const res = await fetch(`/Common/AddressEngineHandler?handler=States&countryId=${countryId}`);
    const data = await res.json();
    if (!data) return;
    stateTS.addOptions(
        data.map(x => ({
            value: x.stateID,
            text: x.stateName
        }))
    );

    stateTS.refreshOptions(false);

    if (selectedId) stateTS.setValue(selectedId);
}

/* =========================
   🔹 LOAD CITIES
========================= */
async function loadCities(stateId, selectedId = null) {

    cityTS.clear();
    cityTS.clearOptions();

    if (!stateId) return;

    const res = await fetch(`/Common/AddressEngineHandler?handler=Cities&stateId=${stateId}`);
    const data = await res.json();
    if (!data) return;
    cityTS.addOptions(
        data.map(x => ({
            value: x.cityID,
            text: x.cityName
        }))
    );

    cityTS.refreshOptions(false);

    if (selectedId) cityTS.setValue(selectedId);
}

/* =========================
   🔹 ADDRESS CHANGE
========================= */
async function onAddressChange() {

    const id = document.getElementById("existingAddress").value;

    if (!id) {
        clearAddressForm();
        return;
    }
    clearMap();
    setAddressMode(!!id);
    await loadAddressById(id);
    loadMapFromLocation(`${cityTS.value}, ${stateTS.value}, India`);
}

/* =========================
   🔹 LOAD ADDRESS
========================= */
async function loadAddressById(id) {

    const res = await fetch(`/Common/AddressEngineHandler?handler=AddressById&addressId=${id}`);
    const a = await res.json();

    document.getElementById("line1").value = a.addressLine1 || '';
    document.getElementById("line2").value = a.addressLine2 || '';
    document.getElementById("landmark").value = a.landmark || '';
    document.getElementById("pincode").value = a.pincode || '';
    document.getElementById("nickname").value = a.addressNickName || '';
    document.getElementById("isPrimary").checked = a.isPrimary || false;

    await loadCountries(a.countryID);
    await loadStates(a.countryID, a.stateID);
    await loadCities(a.stateID, a.cityID);
}

/* =========================
   🔹 CLEAR FORM
========================= */
function clearAddressForm() {
    //alert("reset");
    document.getElementById("existingAddress").innerHTML = '<option value="">-- Select Address --</option>';
    loadExistingAddresses(currentAddressType);
    document.getElementById("line1").value = '';
    document.getElementById("line2").value = '';
    document.getElementById("landmark").value = '';
    document.getElementById("pincode").value = '';
    document.getElementById("nickname").value = '';
    document.getElementById("isPrimary").checked = false;

    countryTS?.clear();
    stateTS?.clear();
    stateTS?.clearOptions();
    cityTS?.clear();
    cityTS?.clearOptions();

    setAddressMode(false);
    //document.getElementById("btnSave").innerText = 'Save Address';
    document.getElementById("btnSave").disabled = false;
    clearMap();
    // Clear errors
    document.querySelectorAll("[id^='err_']").forEach(x => {
        x.innerText = '';
        x.classList.add("d-none");
    });

}

/* =========================
   🔹 SAVE (TEXT OR ID)
========================= */
async function saveAddress() {
    if (!validateAddressForm()) return;

    const btn = document.getElementById("btnSave");

    // 🔥 Save original text (important)
    const originalText = btn.innerText;

    // 🔥 Loading state
    btn.disabled = true;
    btn.innerText = "Saving...";



    try {
        let UserTempID = 0;          // ✅ FIXED
        let TempEntityType = null;   // ✅ FIXED

        if (currentAddressType == "NATIVE") {

            const UserTempSession = localStorage.getItem("TEMP_SESSION");

            if (UserTempSession) {
                UserTempID = localStorage.getItem("TEMP_ID");
                TempEntityType = currentAddressType;
            } else {
                await ensureTempUser(); // 🔥 important
                UserTempID = localStorage.getItem("TEMP_ID");
                TempEntityType = currentAddressType;
            }
        }

        //alert(currentAddressType); // ✅ now will execute

        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const dto = {
            addressID: document.getElementById("existingAddress").value || null,
            addressLine1: document.getElementById("line1").value,
            addressLine2: document.getElementById("line2").value,
            landmark: document.getElementById("landmark").value,
            pincode: document.getElementById("pincode").value,
            addressNickName: document.getElementById("nickname").value,
            isPrimary: document.getElementById("isPrimary").checked,
            addressType: currentAddressType,
            entityID: UserTempID,
            entityType: TempEntityType,

            country: countryTS?.getValue()?.toString() || '',
            state: stateTS?.getValue()?.toString() || '',
            city: cityTS?.getValue()?.toString() || ''
        };

        const res = await fetch(`/Common/AddressEngineHandler?handler=SaveAddress`, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            body: JSON.stringify(dto)
        });

        const result = await res.json();
        btn.innerText = "Save Address";
        if (result.success) {
            //alert(result.message);
            showToast(result.message);
            document.dispatchEvent(new Event("addressSaved"));
            //await clearAddressForm();      
            //btn.innerText = originalText;
        }
        else {
            alert(result.message || "Address Save failed");
            showToast(result.message || "Address Save failed");
            btn.disabled = false;
            btn.innerText = originalText;
        }

        //await clearAddressForm();

        await loadExistingAddresses(currentAddressType,UserTempID);

        //bootstrap.Modal.getInstance(document.getElementById('addressModal')).hide();
    } catch (e) {
        btn.disabled = false;
        console.error(e);
    }
}


function validateAddressForm() {

    let isValid = true;

    function showError(id, msg) {
        const el = document.getElementById(id);
        el.innerText = msg;
        el.classList.remove("d-none");
    }

    function clearError(id) {
        const el = document.getElementById(id);
        el.innerText = "";
        el.classList.add("d-none");
    }

    // Clear all
    ["err_nickname", "err_line1", "err_country", "err_state", "err_city"]
        .forEach(clearError);

    const nickname = document.getElementById("nickname").value.trim();
    const line1 = document.getElementById("line1").value.trim();

    const country = countryTS?.getValue();
    const state = stateTS?.getValue();
    const city = cityTS?.getValue();

    if (!nickname) {
        showError("err_nickname", "Nickname is required");
        isValid = false;
    }

    if (!line1 || line1.length < 7) {
        showError("err_line1", "Minimum 7 characters required");
        isValid = false;
    }

    if (!country) {
        showError("err_country", "Select or enter country");
        isValid = false;
    }

    if (!state) {
        showError("err_state", "Select or enter state");
        isValid = false;
    }

    if (!city) {
        showError("err_city", "Select or enter city");
        isValid = false;
    }

    return isValid;
}

async function fetchPincodeDetails() {
    
    const pincodeInput = document.getElementById("pincode");
    const pincode = pincodeInput.value.trim();
    //alert(pincode);
    // 🔥 Clear previous error
    showPincodeError("");

    if (!pincode) return;

    // 🔥 BASIC VALIDATION
    if (!/^[1-9][0-9]{5}$/.test(pincode)) {
        showPincodeError("Invalid pincode format");
        clearMap();
        return;
    }

    clearMap();
    
    try {
        const res = await fetch(`https://api.postalpincode.in/pincode/${pincode}`);
        const data = await res.json();
        //if (!data) return;

        if (!data || data[0].Status !== "Success") {
            showPincodeError("Pincode not found");
            return;
        }
        
        const post = data[0].PostOffice[0];

        const area = post.Name;        // 🔥 Area
        const stateName = post.State;
        const cityName = post.District;
        const countryName = "India";
        
        // 🔥 Autofill Address Line 2 (only if empty)
        const line2 = document.getElementById("line2");
        if (!line2.value) {
            line2.value = area;
        }
        
        // 🔥 Dropdown handling (match or create)
        await loadCountries();
        setOrCreateValue(countryTS, countryName);
        await loadStates(countryTS.getValue());
        setOrCreateValue(stateTS, stateName);
        await loadCities(stateTS.getValue());
        setOrCreateValue(cityTS, cityName);
        //alert("city read");

        // 🔥 Map
        loadMapFromLocation(`${area}, ${cityName}, ${stateName}`);

    } catch (err) {
        showToast("Pincode fetch failed, " + err);
        showPincodeError("Unable to fetch pincode details");
    }
}

function showPincodeError(msg) {

    const el = document.getElementById("err_pincode");

    if (!msg) {
        el.innerText = "";
        el.classList.add("d-none");
    } else {
        el.innerText = msg;
        el.classList.remove("d-none");
    }
}
function setOrCreateValue(ts, text) {

    if (!text) return;

    // 🔍 Try match existing option
    const match = Object.values(ts.options)
        .find(x => x.text.toLowerCase() === text.toLowerCase());

    if (match) {
        ts.setValue(match.value); // existing ID
    } else {
        // 🔥 Create new option dynamically
        ts.addOption({ value: text, text: text });
        ts.setValue(text); // set as TEXT
    }
}
function loadMapFromLocation(locationText) {

    const mapContainer = document.getElementById("mapContainer");
    const iframe = document.getElementById("mapFrame");

    const encoded = encodeURIComponent(locationText);

    iframe.src = `https://www.google.com/maps?q=${encoded}&output=embed`;

    mapContainer.classList.remove("d-none");
}
function clearMap() {

    const mapContainer = document.getElementById("mapContainer");
    const iframe = document.getElementById("mapFrame");

    if (iframe) {
        iframe.src = ""; // 🔥 clear map
    }

    if (mapContainer) {
        mapContainer.classList.add("d-none"); // 🔥 hide
    }
}

function showToast1(message) {
    //alert (message);
    const el = document.getElementById("toastMsg");
    el.innerText = message;
    el.classList.remove("d-none");

    setTimeout(() => {
        el.classList.add("d-none");
    }, 7000);
}