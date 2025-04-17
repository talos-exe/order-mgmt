function toggleButtons() {
    let checkboxes = document.querySelectorAll(".product-checkbox:checked");
    let btnEdit = document.getElementById("btnEdit");
    let btnDelete = document.getElementById("btnDelete");

    if (checkboxes.length === 1) {
        let productId = checkboxes[0].value;
        btnEdit.setAttribute("data-bs-target", `#editProductModal-${productId}`);
        btnDelete.setAttribute("data-bs-target", `#deleteProductModal-${productId}`);
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

    // Load the product details via fetch
    fetch(`/Products/GetProductDetails?productId=${productId}`)
        .then(response => response.text())
        .then(data => {
            $('#' + tabId).html(data);
        })
        .catch(() => {
            console.log("Product ID: " + productId);
            $('#' + tabId).html('<div class="ms-5 me-5 text-danger">Error loading product details</div>');
        });
}

function openCreateProductPane() {
    closeCreateProduct();

    var tabId = 'createProduct';
    var newTab = $('<li class="nav-item">' +
        '<a class="nav-link" id="' + tabId + 'Tab" data-bs-toggle="tab" href="#' + tabId + '">' +
        'Create Product' +
        '</a>' +
        '</li>');

    $('#productTabs').append(newTab);

    var newPane = $('<div class="tab-pane fade" id="' + tabId + '">' +
        '<div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>' +
        '</div>');
    $('.tab-content').append(newPane);

    $('#' + tabId + 'Tab').tab('show');

    fetch('/Products/_CreateProduct')
        .then(response => response.text())
        .then(data => {
            $('#' + tabId).html(data);
        })
        .catch(() => {
            $('#' + tabId).html('<div class="ms-5 me-5 text-danger">Error loading Create Product form</div>');
        });
}

function closeCreateProduct() {
    // Find the create product tab
    var tab = $('#createProductTab');
    if (tab.length) {
        var tabId = tab.attr('href');
        // Remove the tab and its content
        tab.parent().remove();
        $(tabId).remove();
    }

    // Activate the product list tab
    $('#productListTab').tab('show');
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

function filterTable() {
    var input = document.getElementById("searchBar");
    var filter = input.value.toLowerCase();
    var table = document.getElementById("productTable");
    var rows = table.getElementsByTagName("tr");

    for (var i = 1; i < rows.length; i++) {
        var productCell = rows[i].getElementsByTagName("td")[1]; // Get the product cell
        if (productCell) {
            var product = productCell.textContent || productCell.innerText;
            if (product.toLowerCase().includes(filter)) {
                rows[i].style.display = "";
            } else {
                rows[i].style.display = "none";
            }
        }
    }
}
