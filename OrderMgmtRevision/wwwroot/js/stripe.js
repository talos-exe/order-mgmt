function showToast(message, type = 'success') {
    const toastContainer = document.getElementById('toast-container');

    // Create toast element
    const toastEl = document.createElement('div');
    toastEl.className = `toast align-items-center text-white bg-${type === 'success' ? 'success' : 'danger'} border-0`;
    toastEl.setAttribute('role', 'alert');
    toastEl.setAttribute('aria-live', 'assertive');
    toastEl.setAttribute('aria-atomic', 'true');

    // Create the toast structure
    const flexDiv = document.createElement('div');
    flexDiv.className = 'd-flex';

    const toastBody = document.createElement('div');
    toastBody.className = 'toast-body';
    toastBody.textContent = message; // Using textContent instead of innerHTML

    const closeButton = document.createElement('button');
    closeButton.className = 'btn-close btn-close-white me-2 m-auto';
    closeButton.setAttribute('type', 'button');
    closeButton.setAttribute('data-bs-dismiss', 'toast');
    closeButton.setAttribute('aria-label', 'Close');

    // Assemble the elements
    flexDiv.appendChild(toastBody);
    flexDiv.appendChild(closeButton);
    toastEl.appendChild(flexDiv);

    // Add to container
    toastContainer.appendChild(toastEl);

    // Initialize and show toast
    const toast = new bootstrap.Toast(toastEl, {
        autohide: true,
        delay: 5000
    });
    toast.show();

    // Remove from DOM after hiding
    toastEl.addEventListener('hidden.bs.toast', () => {
        toastEl.remove();
    });
}

function showToastFromTempData(message, type) {
    document.addEventListener('DOMContentLoaded', function () {
        showToast(message, type);
    });
}