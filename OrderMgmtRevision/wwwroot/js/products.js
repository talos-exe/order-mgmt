function toggleButtons() {
    let checkboxes = document.querySelectorAll(".product-checkbox:checked");
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
    let checkboxes = document.querySelectorAll(".product-checkbox");
    checkboxes.forEach(cb => cb.checked = source.checked);
    toggleButtons();
}

function openProductDetailsTab(productId, productName) {
    // Remove existing product details tab if it exists
    closeProductDetailsTab();

    // Create new tab
    var tabId = 'productDetails-' + productId;
    var newTab = $('<li class="nav-item">' +
        '<a class="nav-link" id="' + tabId + 'Tab" data-bs-toggle="tab" href="#' + tabId + '">' +
        'Details: ' + productName +
        '</a>' +
        '</li>');

    // Add the new tab to the tab list
    $('#productTabs').append(newTab);

    // Create new tab pane
    var newPane = $('<div class="tab-pane fade" id="' + tabId + '"><div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div></div>');
    $('.tab-content').append(newPane);

    // Activate the new tab
    $('#' + tabId + 'Tab').tab('show');

    // Load the product details via AJAX
    $.ajax({
        url: '/Products/GetProductDetails',
        type: 'GET',
        data: { productId: productId },
        success: function (data) {
            $('#' + tabId).html(data);
        },
        error: function () {
            console.log("Product ID: " + productId);
            $('#' + tabId).html('<div class="ms-5 me-5 text-danger">Error loading product details</div>');
        }
    });
}

function closeProductDetailsTab() {
    // Find any product details tab
    $('a[id^="productDetails-"][id$="Tab"]').each(function () {
        var tabId = $(this).attr('href');
        // Remove the tab and its content
        $(this).parent().remove();
        $(tabId).remove();
    });

    // Activate the product list tab
    $('#productListTab').tab('show');
}
