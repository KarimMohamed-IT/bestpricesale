namespace bestpricesale.Services
{
    using System.Threading.Tasks;

    public class PaymentService : IPaymentService
    {
        // This is a mock implementation of a payment process.
        public async Task<bool> ProcessPaymentAsync(decimal amount)
        {
            // Simulate a payment processing delay.
            await Task.Delay(1000);
            // Always return true for a successful payment in this mock.
            return true;
        }
    }
}
