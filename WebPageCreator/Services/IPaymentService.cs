namespace bestpricesale.Services
{
    using System.Threading.Tasks;

        public interface IPaymentService
        {
            Task<bool> ProcessPaymentAsync(decimal amount);
        }
}
