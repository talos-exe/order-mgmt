
    function showLoader(show) {
        const loader = document.getElementById('loading-overlay');
    if (loader) loader.style.display = show ? 'flex' : 'none';
    }

    function updateProductFields() {
        // Instead of using data attributes, fetch product details from server
        const selectedProductId = $('#productSelect').val();
    if (selectedProductId) {
        showLoader(true);

    $.ajax({
        url: '@Url.Action("GetProductDetails", "Products")', // Create this endpoint
    type: 'GET',
    data: {productId: selectedProductId },
    success: function(product) {
                    if (product) {
        $('[name="Weight"]').val(product.weight);
    $('[name="Length"]').val(product.length);
    $('[name="Width"]').val(product.width);
    $('[name="Height"]').val(product.height);
                    }
    showLoader(false);
                },
    error: function() {
        alert('Error loading product details');
    showLoader(false);
                }
            });
        }
    }

    $('#productSelect').on('change', updateProductFields);

    $('#warehouseSelect').on('change', function () {
        // No need to store warehouse data in client-side hidden fields
        // The server will look up the warehouse data when the form is submitted
    });

    let typingTimer;
    const doneTypingInterval = 500;

    $('#toStreetInput').on('input', function() {
        clearTimeout(typingTimer);

    const addressInput = $(this).val();
        if (addressInput.length > 5) {
        typingTimer = setTimeout(function () {
            validateAddress(addressInput);
        }, doneTypingInterval);
        } else {
        $('#addressSuggestions').hide();
        }
    });

    function validateAddress(addressInput) {
        const city = $('#ToCity').val();
    const state = $('#ToState').val();
    const zip = $('#ToZip').val();
    const country = $('#ToCountryCode').val();

    showLoader(true);

    $.ajax({
        url: '@Url.Action("ValidateAddress", "Shipping")',
    type: 'POST',
    data: {
        street: addressInput,
    city: city,
    state: state,
    zip: zip,
    country: country
            },
    headers: {
        'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
    success: function(result) {
                if (result && result.suggestions && result.suggestions.length > 0) {
        displayAddressSuggestions(result.suggestions);
                } else {
        $('#addressSuggestions').empty().show().append('<div class="list-group-item">No address found</div>');
                }
    showLoader(false);
            },
    error: function(error) {
        console.error('Address validation error:', error);
    showLoader(false);
            }
        });
    }

    function displayAddressSuggestions(suggestions) {
        const container = $('#addressSuggestions');
    container.empty();

        suggestions.forEach(suggestion => {
            const formatted = suggestion.street1 +
    (suggestion.street2 ? ', ' + suggestion.street2 : '') +
    ', ' + suggestion.city +
    ', ' + suggestion.state +
    ' ' + suggestion.zip;

    const item = $('<button>')
        .addClass('list-group-item list-group-item-action')
        .text(formatted)
        .on('click', function(e) {
            e.preventDefault();
        // Fill in all address fields with the selected address
        $('#toStreetInput').val(suggestion.street1);
        $('#ToCity').val(suggestion.city);
        $('#ToState').val(suggestion.state);
        $('#ToZip').val(suggestion.zip);
        $('#ToCountryCode').val(suggestion.country);

        // Hide suggestions
        container.hide();

        return false;
                });

        container.append(item);
        });

        container.show();
    }

        $('#nextButton').on('click', function () {
        // Validate manually
        if ($('#warehouseSelect').val() === '' || $('#productSelect').val() === '') {
            alert('Please select a warehouse and product.');
        return;
        }

        showLoader(true);

        // Add CSRF token to AJAX headers for all requests
        $.ajaxSetup({
            headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            }
        });

        // Serialize form data
        const formData = $('#createShippingRequestForm').serialize();

        $.ajax({
            url: '@Url.Action("GetRates", "Shipping")',
        type: 'POST',
        data: formData,
        success: function (rates) {
            $('#createShippingRequestForm').hide();

        $('#rateOptions').empty();

                rates.forEach(rate => {
            $('#rateOptions').append(`
                        <div class="col-md-4">
                            <div class="card p-3">
                                <p><strong>Provider:</strong> ${rate.provider}</p>
                                <p><strong>Service:</strong> ${rate.service}</p>
                                <p><strong>Amount:</strong> $${rate.amount}</p>
                                <button class="btn btn-sm btn-success select-rate" 
                                        data-rate-id="${rate.rateObjectId}">
                                        Select
                                </button>
                            </div>
                        </div>
                    `);
                });
        $('#rateSelectionSection').show();
        showLoader(false);
            },
        error: function () {
            alert('Error retrieving shipping rates.');
        showLoader(false);
            }
        });
    });

        // Handler for rate selection
        $(document).on('click', '.select-rate', function () {
        // Only store the rate ID, not the entire rate object
        const selectedRateId = $(this).data('rate-id');

        // Set the selected rate ID in the form
        $('#selectedRateId').val(selectedRateId);

        // Submit the form
        $('#createShippingRequestForm').submit();
    });
