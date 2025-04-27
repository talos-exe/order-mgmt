using Stripe;
using Stripe.Checkout;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace OrderMgmtRevision.Services
{
    public class StripeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        public StripeService(HttpClient httpClient, IConfiguration configuration)
        {
             _httpClient = httpClient;
            var _secretKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<(bool Success, string Error, string CustomerId)> SavePaymentMethodAsync(string paymentMethodId, string email)
        {
            try
            {
                var customerService = new CustomerService();
                
                var customerOptions = new CustomerCreateOptions
                {
                    Email = email,
                    PaymentMethod = paymentMethodId
                };
                
                var customer = await customerService.CreateAsync(customerOptions);

                var paymentMethodService = new PaymentMethodService();
                await paymentMethodService.AttachAsync(paymentMethodId, new PaymentMethodAttachOptions
                {
                    Customer = customer.Id
                });

                var customerUpdateOptions = new CustomerUpdateOptions
                {
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = paymentMethodId
                    }
                };
                await customerService.UpdateAsync(customer.Id, customerUpdateOptions);

                return (true, null, customer.Id);
            }
            catch (StripeException ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<string> CreateDirectCheckoutSessionAsync(string email, long amount, string successUrl, string cancelUrl, string currency = "usd")
        {
            try
            {
                // Create or retrieve a customer first
                var customerService = new CustomerService();
                var customers = await customerService.ListAsync(new CustomerListOptions
                {
                    Email = email,
                    Limit = 1
                });

                string customerId;
                if (customers.Data.Count > 0)
                {
                    // Use existing customer
                    customerId = customers.Data[0].Id;
                }
                else
                {
                    // Create new customer
                    var customer = await customerService.CreateAsync(new CustomerCreateOptions
                    {
                        Email = email
                    });
                    customerId = customer.Id;
                }

                // Create checkout session with the customer ID
                var sessionService = new SessionService();
                var sessionOptions = new SessionCreateOptions
                {
                    Customer = customerId,
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = amount, // in cents
                        Currency = currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Account Balance Payment"
                        }
                    },
                    Quantity = 1
                }
            },
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl
                };

                var session = await sessionService.CreateAsync(sessionOptions);
                return session.Url;
            }
            catch (StripeException ex)
            {
                throw new Exception($"Failed to create checkout session: {ex.Message}");
            }
        }

        // bank account
        public async Task<string> CreateACHCheckoutSessionAsync(string customerId, long amount, string currency = "usd")
        {
            try
            {
                var sessionService = new SessionService();
                var sessionOptions = new SessionCreateOptions
                {
                    Customer = customerId,
                    PaymentMethodTypes = new List<string> { "us_bank_account" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = amount, // in cents
                                Currency = currency,
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "ACH Payment"
                                }
                            },
                            Quantity = 1
                        }
                    },
                    Mode = "payment",
                    PaymentMethodOptions = new SessionPaymentMethodOptionsOptions
                    {
                        UsBankAccount = new SessionPaymentMethodOptionsUsBankAccountOptions
                        {
                            VerificationMethod = "automatic"
                        }
                    }
                };

                var session = await sessionService.CreateAsync(sessionOptions);
                return session.Id;
            }
            catch (StripeException ex)
            {
                throw new Exception($"Failed to create ACH checkout session: {ex.Message}");
            }
        }
    }
}