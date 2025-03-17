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