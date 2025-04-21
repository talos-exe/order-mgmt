//document.addEventListener('DOMContentLoaded', () => {
//    const form = document.getElementById('payment-form');
//    const stripePublishableKey = form.dataset.stripeKey;
//    const stripe = Stripe(stripePublishableKey);

//    const elements = stripe.elements();
//    const cardElement = elements.create('card');
//    cardElement.mount('#card-element');

//    const amount = parseInt(form.dataset.amount);
//    let customerId = null;

//    function showToast(title, message) {
//        const toastEl = document.getElementById('resultToast');
//        if (!toastEl) {
//            console.warn('Toast element not found.');
//            alert(`${title}: ${message}`);
//            return;
//        }

//        const toastHeader = toastEl.querySelector('.toast-header-text');
//        const toastBody = toastEl.querySelector('.toast-body');
//        const toast = new bootstrap.Toast(toastEl, { delay: 3000 });

//        toastHeader.textContent = title;
//        toastBody.textContent = message;
//        toast.show();
//    }

//    function showLoader(show) {
//        const loader = document.getElementById('loading-overlay');
//        if (loader) loader.style.display = show ? 'flex' : 'none';
//    }

//    // Save payment method
//    form.addEventListener('submit', async (event) => {
//        event.preventDefault();
//        showLoader(true);

//        const { paymentMethod, error } = await stripe.createPaymentMethod({
//            type: 'card',
//            card: cardElement,
//        });

//        if (error) {
//            showLoader(false);
//            document.getElementById('card-errors').textContent = error.message;
//            return;
//        }

//        try {
//            const response = await fetch('/Invoice/SavePaymentMethod', {
//                method: 'POST',
//                headers: { 'Content-Type': 'application/json' },
//                body: JSON.stringify({ paymentMethodId: paymentMethod.id })
//            });

//            const data = await response.json();
//            showLoader(false);

//            if (data.success) {
//                customerId = data.customerId;
//                showToast('Success!', 'Payment Method Saved Successfully.');
//                document.getElementById('checkout-card').disabled = false;
//                document.getElementById('checkout-ach').disabled = false;
//            } else {
//                showToast('Something went wrong', data.message || 'Unknown error');
//            }
//        } catch (err) {
//            showLoader(false);
//            showToast('Something went wrong', err.message);
//        }
//    });

//    // Checkout with card
//    document.getElementById('checkout-card').addEventListener('click', async () => {
//        if (!customerId) {
//            alert('Please save a payment method first.');
//            return;
//        }

//        showLoader(true);
//        try {
//            const response = await fetch('/Invoice/CreateCheckoutSession', {
//                method: 'POST',
//                headers: { 'Content-Type': 'application/json' },
//                body: JSON.stringify({
//                    customerId: customerId,
//                    amount: amount,
//                    paymentType: 'card'
//                })
//            });

//            const data = await response.json();
//            showLoader(false);

//            if (data.success) {
//                stripe.redirectToCheckout({ sessionId: data.sessionId });
//            } else {
//                console.error('Checkout session creation failed:', data);
//                showToast('Something went wrong', data.message || 'Unable to create checkout session');
//            }
//        } catch (err) {
//            showLoader(false);
//            showToast('Something went wrong', err.message);
//        }
//    });

//    // Checkout with ACH
//    document.getElementById('checkout-ach').addEventListener('click', async () => {
//        if (!customerId) {
//            alert('Please save a payment method first.');
//            return;
//        }

//        showLoader(true);
//        try {
//            const response = await fetch('/Invoice/CreateCheckoutSession', {
//                method: 'POST',
//                headers: { 'Content-Type': 'application/json' },
//                body: JSON.stringify({
//                    customerId: customerId,
//                    amount: amount,
//                    paymentType: 'ach'
//                })
//            });

//            const data = await response.json();
//            showLoader(false);

//            if (data.success) {
//                stripe.redirectToCheckout({ sessionId: data.sessionId });
//            } else {
//                console.error('Checkout session creation failed:', data);
//                showToast('Something went wrong', data.message || 'Unable to create checkout session');
//            }
//        } catch (err) {
//            showLoader(false);
//            showToast('Something went wrong', err.message);
//        }
//    });
//});
