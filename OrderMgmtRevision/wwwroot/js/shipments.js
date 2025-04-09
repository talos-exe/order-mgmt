function toggleButtons() {
    let checkboxes = document.querySelectorAll(".shipment-checkbox:checked");
    let btnEdit = document.getElementById("btnEdit");
    let btnDelete = document.getElementById("btnDelete");

    if (checkboxes.length === 1) {
        let userId = checkboxes[0].value;
        btnEdit.setAttribute("data-bs-target", `#editUserModal-${userId}`);
        btnDelete.setAttribute("data-bs-target", `#deleteUserModal-${userId}`);
        btnEdit.disabled = false;
        btnDelete.disabled = false;
    } else if (checkboxes.length > 1) {
        btnEdit.removeAttribute("data-bs-target");
        btnEdit.disabled = true;
    } else {
        btnEdit.removeAttribute("data-bs-target");
        btnDelete.removeAttribute("data-bs-target");
        btnEdit.disabled = true;
        btnDelete.disabled = true;
    }
}

function toggleAll(source) {
    let checkboxes = document.querySelectorAll(".shipment-checkbox");
    checkboxes.forEach(cb => cb.checked = source.checked);
    toggleButtons();
}

function openCreateShipmentPane() {
    closeCreateShipment();

    var tabId = 'createShippingRequest';
    var newTab = $('<li class="nav-item">' +
        '<a class="nav-link" id="' + tabId + 'Tab" data-bs-toggle="tab" href="#' + tabId + '">' +
        'Create Shipping Request' +
        '</a>' +
        '</li>');

    $('#shippingTabs').append(newTab);

    var newPane = $('<div class="tab-pane fade" id="' + tabId + '">' +
        '<div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>' +
        '</div>');
    $('.tab-content').append(newPane);

    $('#' + tabId + 'Tab').tab('show');

    $.ajax({
        url: '/Shipping/_CreateShipmentRequest',
        type: 'GET',
        success: function (data) {
            $('#' + tabId).html(data);
        },
        error: function () {
            $('#' + tabId).html('<div class="ms-5 me-5 text-danger">Error loading Create Shipping Request form</div>');
        }
    });

}


function closeCreateShipment() {
    // Find the create user tab
    var tab = $('#createShippingRequestTab');
    if (tab.length) {
        var tabId = tab.attr('href');
        // Remove the tab and its content
        tab.parent().remove();
        $(tabId).remove();
    }

    // Activate the shipping list tab
    $('#shippingListTab').tab('show');
}

document.addEventListener('DOMContentLoaded', () => {
    const createSubmitButton = document.getElementById('createShipmentSubmit');
    createSubmitButton.addEventListener('click', () => {
        const form = document.getElementById('createShipmentForm');
        const shipmentData = {
            RecipientName: form.querySelector('#recipientName').value,
            Address: form.querySelector('#address').value,
            City: form.querySelector('#city').value,
            State: form.querySelector('#state').value,
            PostalCode: form.querySelector('#postalCode').value,
            CountryCode: form.querySelector('#countryCode').value,
            PhoneNumber: form.querySelector('#phoneNumber').value
        };

        console.log('Sending shipment data:', shipmentData);

        fetch('/Shipping/CreateShipment', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(shipmentData)
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(err => { throw new Error(err.message); });
                }
                const trackingNumber = shipmentData.RecipientName; // Placeholder; ideally get from headers or response
                return response.blob().then(blob => ({ blob, trackingNumber }));
            })
            .then(({ blob, trackingNumber }) => {
                const createModal = bootstrap.Modal.getInstance(document.getElementById('createShipmentModal'));
                createModal.hide();

                // Trigger PDF download
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.download = `label_${trackingNumber}.pdf`; // Use tracking number from response if possible
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                window.URL.revokeObjectURL(url);

                const toastEl = document.getElementById('resultToast');
                const toastHeader = toast.El.querySelector('.toast-header-text');
                const toastBody = toastEl.querySelector('.toast-body');
                const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
                toastHeader.textContent = 'Success!'
                toastBody.textContent = `Shipment created successfully. Tracking: ${trackingNumber}`;
                toast.show();

                location.reload();
            })
            .catch(error => {
                const toastEl = document.getElementById('resultToast');
                const toastHeader = toast.El.querySelector('.toast-header-text');
                const toastBody = toastEl.querySelector('.toast-body');
                const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
                toastHeader.textContent = 'Something went wrong';
                toastBody.textContent = error.message;
                toast.show();
            });
    });

    // Cancel Shipment - Setup cancel buttons
    const cancelButtons = document.querySelectorAll('.btn-cancel');
    cancelButtons.forEach(button => {
        button.addEventListener('click', () => {
            const trackingNumber = button.dataset.trackingNumber;
            document.getElementById('cancelTrackingNumber').textContent = trackingNumber;
            document.getElementById('confirmCancel').dataset.trackingNumber = trackingNumber;
        });
    });

    // Confirm Cancel
    const confirmCancelButton = document.getElementById('confirmCancel');
    confirmCancelButton.addEventListener('click', () => {
        const trackingNumber = confirmCancelButton.dataset.trackingNumber;

        fetch('/Shipping/CancelShipment', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: `trackingNumber=${encodeURIComponent(trackingNumber)}`
        })
            .then(response => response.json())
            .then(result => {
                const cancelModal = bootstrap.Modal.getInstance(document.getElementById('cancelShipmentModal'));
                cancelModal.hide();

                const toastEl = document.getElementById('resultToast');
                const toastHeader = toast.El.querySelector('.toast-header-text');
                const toastBody = toastEl.querySelector('.toast-body');
                const toast = new bootstrap.Toast(toastEl, { delay: 1000 });

                if (result.success) {
                    toastHeader.textContent = 'Success!'
                    toastBody.textContent = result.message;
                    toast.show();
                    location.reload();
                } else {
                    toastHeader.textContent = 'Something went wrong';
                    toastBody.textContent = result.message;
                    toast.show();
                }
            })
            .catch(error => {
                const toastEl = document.getElementById('resultToast');
                const toastHeader = toast.El.querySelector('.toast-header-text');
                const toastBody = toastEl.querySelector('.toast-body');
                const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
                toastHeader.textContent = 'Something went wrong';
                toastBody.textContent = error.message;
                toast.show();
            });
    });
});