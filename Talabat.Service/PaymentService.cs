using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.IServices;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;

        public PaymentService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IConfiguration configuration)
        {
            _unitofwork = unitOfWork;
            _basketRepository = basketRepository;
            _configuration = configuration;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            //checking for basket if null
            if (Basket == null) return null;

            //checking basket price truthfulness
            var ProductRepository = _unitofwork.Repository<Product>();
            if (Basket.BasketProducts.Count() > 0)
            {
                foreach (var item in Basket.BasketProducts)
                {
                    var product = await ProductRepository.GetAsync(item.Id);
                    //ensuring the basketitem price equals product price from db
                    item.Price = product.Price;

                }
            }
            //checking deliverymethod price truthfulness
            if (Basket.DeliveryMethodId != null)
            {
                var deliverymethod = await _unitofwork.Repository<DeliveryMethod>().GetAsync(Basket.DeliveryMethodId);
                //ensuring the ShippingPrice equals deliverymethod price from db
                Basket.ShippingPrice = deliverymethod.Cost;
            }

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (Basket.PaymentIntentId == null)//creating new paymentintent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)Basket.BasketProducts.Sum(item => (item.Price * 100 * item.Quantity)) + (long)(Basket.ShippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },

                };
                paymentIntent = await service.CreateAsync(options);
                // return PaymentIntentId,ClientSecret

                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)Basket.BasketProducts.Sum(item => (item.Price * 100 * item.Quantity)) + (long)(Basket.ShippingPrice * 100),

                };
                paymentIntent = await service.UpdateAsync(Basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }
    }
}
