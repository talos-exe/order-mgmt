﻿function toggleButtons() {
    let checkboxes = document.querySelectorAll(".user-checkbox:checked");
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
    let checkboxes = document.querySelectorAll(".user-checkbox");
    checkboxes.forEach(cb => cb.checked = source.checked);
    toggleButtons();
}

// Table Sorting Function

var isAscending = false;
function sortTable(columnIndex) {
    var table = document.getElementById("userTable");
    var rows = Array.from(table.getElementsByTagName("tr")).slice(1); // Get all rows except the header
    isAscending = !isAscending; // Toggle sorting order

    var caret = document.getElementById("userCaret");
    if (isAscending) {
        caret.classList.remove("fa-caret-down");
        caret.classList.add("fa-caret-up");
    } else {
        caret.classList.remove("fa-caret-up");
        caret.classList.add("fa-caret-down");
    }

    rows.sort((a, b) => {
        var cellA = a.cells[columnIndex].textContent.trim();
        var cellB = b.cells[columnIndex].textContent.trim();
        return isAscending
            ? cellA.localeCompare(cellB) // Ascending sort
            : cellB.localeCompare(cellA); // Descending sort
    });

    // Re-append the sorted rows to the table body to maintain styling
    var tbody = table.querySelector('tbody');
    rows.forEach(row => tbody.appendChild(row)); // Append sorted rows
}

// Search Function
function filterTable() {
    var input = document.getElementById("searchBar");
    var filter = input.value.toLowerCase();
    var table = document.getElementById("userTable");
    var rows = table.getElementsByTagName("tr");

    for (var i = 1; i < rows.length; i++) {
        var usernameCell = rows[i].getElementsByTagName("td")[1]; // Get the username cell (2nd column)
        if (usernameCell) {
            var username = usernameCell.textContent || usernameCell.innerText;
            if (username.toLowerCase().includes(filter)) {
                rows[i].style.display = "";
            } else {
                rows[i].style.display = "none";
            }
        }
    }
}
