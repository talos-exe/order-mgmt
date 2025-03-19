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
