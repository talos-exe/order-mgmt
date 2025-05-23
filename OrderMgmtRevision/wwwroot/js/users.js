﻿document.addEventListener('DOMContentLoaded', function () {
    // Set the active tab based on URL parameter
    const urlParams = new URLSearchParams(window.location.search);
    const activeTab = urlParams.get('activeTab') || 'userList';
    const userListTab = document.getElementById('userListTab');
    const userLogsTab = document.getElementById('userLogsTab');

    if (activeTab === 'userLogs') {
        userLogsTab.click();
    } else {
        userListTab.click();
    }

    // Add click handlers for updating URL...


    // Add click handlers to update the active tab in URL
    userListTab.addEventListener('click', function (e) {
        updateUrlParameter('activeTab', 'userList');
    });

    userLogsTab.addEventListener('click', function (e) {
        updateUrlParameter('activeTab', 'userLogs');
    });

    // Function to update URL parameters without refreshing the page
    function updateUrlParameter(key, value) {
        const url = new URL(window.location.href);
        url.searchParams.set(key, value);
        window.history.replaceState({}, '', url);
    }
});


function toggleButtons() {
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

function filterLogs() {
    var input = document.getElementById("logSearchBar");
    var filter = input.value.toLowerCase();
    var table = document.getElementById("logTable");
    var rows = table.getElementsByTagName("tr");

    for (var i = 1; i < rows.length; i++) {
        var logId = rows[i].getElementsByTagName("td")[0].textContent.toLowerCase();
        var username = rows[i].getElementsByTagName("td")[1].textContent.toLowerCase();

        if (logId.includes(filter) || username.includes(filter)) {
            rows[i].style.display = "";
        } else {
            rows[i].style.display = "none";
        }
    }
}

function openUserDetailsTab(userId, userName) {
    // Remove existing user details tab if it exists
    closeUserDetailsTab();

    // Create new tab
    var tabId = 'userDetails-' + userId;
    var newTab = $('<li class="nav-item">' +
        '<a class="nav-link" id="' + tabId + 'Tab" data-bs-toggle="tab" href="#' + tabId + '">' +
        'Details: ' + userName +
        '</a>' +
        '</li>');

    // Add the new tab to the tab list
    $('#adminTabs').append(newTab);

    // Create new tab pane
    var newPane = $('<div class="tab-pane fade" id="' + tabId + '"><div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div></div>');
    $('.tab-content').append(newPane);

    // Activate the new tab
    $('#' + tabId + 'Tab').tab('show');

    // Fetch user details
    fetch(`/UserManagement/GetUserDetails?userId=${userId}`)
        .then(response => response.text())
        .then(data => {
            $('#' + tabId).html(data);
        })
        .catch(() => {
            $('#' + tabId).html('<div class="ms-5 me-5 text-danger">Error loading user details</div>');
        });
}

function closeUserDetailsTab() {
    // Find any user details tab
    $('a[id^="userDetails-"][id$="Tab"]').each(function () {
        var tabId = $(this).attr('href');
        // Remove the tab and its content
        $(this).parent().remove();
        $(tabId).remove();
    });

    // Activate the user list tab
    $('#userListTab').tab('show');
}


function openCreateUserPane() {
    closeCreateUser();

    var tabId = 'createUser';
    var newTab = $('<li class="nav-item">' +
        '<a class="nav-link" id="' + tabId + 'Tab" data-bs-toggle="tab" href="#' + tabId + '">' +
        'Create User' +
        '</a>' +
        '</li>');

    $('#adminTabs').append(newTab);

    var newPane = $('<div class="tab-pane fade" id="' + tabId + '">' +
        '<div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>' +
        '</div>');
    $('.tab-content').append(newPane);

    $('#' + tabId + 'Tab').tab('show');

    // Fetch the Create User form
    fetch('/UserManagement/_CreateUser')
        .then(response => response.text())
        .then(data => {
            $('#' + tabId).html(data);
        })
        .catch(() => {
            $('#' + tabId).html('<div class="ms-5 me-5 text-danger">Error loading Create User form</div>');
        });
}

function closeCreateUser() {
    // Find the create user tab
    var tab = $('#createUserTab');
    if (tab.length) {
        var tabId = tab.attr('href');
        // Remove the tab and its content
        tab.parent().remove();
        $(tabId).remove();
    }

    // Activate the user list tab
    $('#userListTab').tab('show');
}

$(document).ready(function () {
    // Check if there's an active tab parameter
    const urlParams = new URLSearchParams(window.location.search);
    const activeTab = urlParams.get('activeTab');

    // Set active tab if specified
    if (activeTab === 'userLogs') {
        $('#adminTabs a[href="#userLogs"]').tab('show');
    }
});





// NOT IN USE

//// Table Sorting Function

//var isAscending = false;
//function sortTable(columnIndex) {
//    var table = document.getElementById("userTable");
//    var rows = Array.from(table.getElementsByTagName("tr")).slice(1); // Get all rows except the header
//    isAscending = !isAscending; // Toggle sorting order

//    var caret = document.getElementById("userCaret");
//    if (isAscending) {
//        caret.classList.remove("fa-caret-down");
//        caret.classList.add("fa-caret-up");
//    } else {
//        caret.classList.remove("fa-caret-up");
//        caret.classList.add("fa-caret-down");
//    }

//    rows.sort((a, b) => {
//        var cellA = a.cells[columnIndex].textContent.trim();
//        var cellB = b.cells[columnIndex].textContent.trim();
//        return isAscending
//            ? cellA.localeCompare(cellB) // Ascending sort
//            : cellB.localeCompare(cellA); // Descending sort
//    });

//    // Re-append the sorted rows to the table body to maintain styling
//    var tbody = table.querySelector('tbody');
//    rows.forEach(row => tbody.appendChild(row)); // Append sorted rows
//}
