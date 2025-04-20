const form = document.getElementById('payment-form');
const stripePublishableKey = form.dataset.stripeKey;
const stripe = Stripe(stripePublishableKey);

const elements = stripe.elements();
const cardElement = elements.create('card');
cardElement.mount('#card-element');

const amount = parseInt(form.dataset.amount);

let customerId = null;

// Save payment method
form.addEventListener('submit', async (event) => {
    event.preventDefault();

    const { paymentMethod, error } = await stripe.createPaymentMethod({
        type: 'card',
        card: cardElement,
    });

    if (error) {
        document.getElementById('card-errors').textContent = error.message;
    } else {
        try {
            const response = await fetch('/Invoice/SavePaymentMethod', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ paymentMethodId: paymentMethod.id })
            });
            const data = await response.json();

            if (data.success) {
                customerId = data.customerId;

                const toastEl = document.getElementById('resultToast');
                const toastHeader = toast.El.querySelector('.toast-header-text');
                const toastBody = toastEl.querySelector('.toast-body');
                const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
                toastHeader.textContent = 'Success!'
                toastBody.textContent = `Payment Method Saved Successfully.`;
                toast.show();

                document.getElementById('checkout-card').disabled = false;
                document.getElementById('checkout-ach').disabled = false;
            } else {
                const toastEl = document.getElementById('resultToast');
                const toastHeader = toast.El.querySelector('.toast-header-text');
                const toastBody = toastEl.querySelector('.toast-body');
                const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
                toastHeader.textContent = 'Something went wrong';
                toastBody.textContent = error.message;
                toast.show();
            }
        } catch (err) {
            const toastEl = document.getElementById('resultToast');
            const toastHeader = toast.El.querySelector('.toast-header-text');
            const toastBody = toastEl.querySelector('.toast-body');
            const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
            toastHeader.textContent = 'Something went wrong';
            toastBody.textContent = error.message;
            toast.show();
        }
    }
});

// Checkout with card
document.getElementById('checkout-card').addEventListener('click', async () => {
    if (!customerId) {
        alert('Please save a payment method first.');
        return;
    }

    try {
        const response = await fetch('/Invoice/CreateCheckoutSession', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                customerId: customerId,
                amount: amount,
                paymentType: 'card'
            })
        });
        const data = await response.json();

        if (data.success) {
            stripe.redirectToCheckout({ sessionId: data.sessionId });
        } else {
            const toastEl = document.getElementById('resultToast');
            const toastHeader = toast.El.querySelector('.toast-header-text');
            const toastBody = toastEl.querySelector('.toast-body');
            const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
            toastHeader.textContent = 'Something went wrong';
            toastBody.textContent = error.message;
            toast.show();
        }
    } catch (err) {
        const toastEl = document.getElementById('resultToast');
        const toastHeader = toast.El.querySelector('.toast-header-text');
        const toastBody = toastEl.querySelector('.toast-body');
        const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
        toastHeader.textContent = 'Something went wrong';
        toastBody.textContent = error.message;
        toast.show();
    }
});

// Checkout with ACH
document.getElementById('checkout-ach').addEventListener('click', async () => {
    if (!customerId) {
        alert('Please save a payment method first.');
        return;
    }

    try {
        const response = await fetch('/Invoice/CreateCheckoutSession', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                customerId: customerId,
                amount: amount,
                paymentType: 'ach'
            })
        });
        const data = await response.json();

        if (data.success) {
            stripe.redirectToCheckout({ sessionId: data.sessionId });
        } else {
            const toastEl = document.getElementById('resultToast');
            const toastHeader = toast.El.querySelector('.toast-header-text');
            const toastBody = toastEl.querySelector('.toast-body');
            const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
            toastHeader.textContent = 'Something went wrong';
            toastBody.textContent = error.message;
            toast.show();
        }
    } catch (err) {
        const toastEl = document.getElementById('resultToast');
        const toastHeader = toast.El.querySelector('.toast-header-text');
        const toastBody = toastEl.querySelector('.toast-body');
        const toast = new bootstrap.Toast(toastEl, { delay: 1000 });
        toastHeader.textContent = 'Something went wrong';
        toastBody.textContent = error.message;
        toast.show();
    }
});