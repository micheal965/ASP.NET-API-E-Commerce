using StackExchange.Redis;
using System.Reflection.Metadata.Ecma335;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.IServices;
using Talabat.Core.Specifications.OrderSpecifications;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IBasketRepository _basketRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitofwork, IBasketRepository basketRepository, IPaymentService paymentService)
        {
            _unitofwork = unitofwork;
            _basketRepository = basketRepository;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliverymethodId, Address shippingaddress)
        {
            //1.Get basket from baskets repo
            var Basket = await _basketRepository.GetBasketAsync(basketId);

            //2. Get selected Items at basket from products repo
            var OrderItems = new List<OrderItem>();
            if (Basket?.BasketProducts?.Count() > 0)
            {
                var ProductRepo = _unitofwork.Repository<Product>();
                foreach (var item in Basket.BasketProducts)
                {
                    var Product = await ProductRepo.GetAsync(item.Id);
                    var productitemordered = new ProductItemOrdered(item.Id, Product.Name, Product.PictureUrl);
                    var orderitem = new OrderItem(productitemordered, item.Quantity, Product.Price);
                    OrderItems.Add(orderitem);
                }
            }

            //3. Calculate subtotal
            var subtotal = OrderItems.Sum(oi => oi.Quantity * oi.Price);

            //4. Get delivery method from delivery methods repo
            DeliveryMethod Deliverymethod = await _unitofwork.Repository<DeliveryMethod>().GetAsync(deliverymethodId);

            /*in case there are 2 order with the same paymentintent
            so delete the old order and change paymentintent amount*/
            var OrderRepo = _unitofwork.Repository<Order>();

            var orderspec = new OrderWithPaymentIntentSpecification(Basket.PaymentIntentId);
            var existingorder = await OrderRepo.GetWithSpecAsync(orderspec);
            if (existingorder != null)
            {
                OrderRepo.Delete(existingorder);
                await _paymentService.CreateOrUpdatePaymentIntentAsync(Basket.Id);
            }

            //5. Create order
            var Order = new Order(buyerEmail, shippingaddress, Deliverymethod, OrderItems, subtotal, Basket.PaymentIntentId);
            await OrderRepo.AddAsync(Order);

            //6. Save to Database 
            int result = await _unitofwork.SaveChangesAsync();
            if (result <= 0) return null;
            return Order;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string email)
        => await _unitofwork.Repository<Order>().GetWithSpecAsync(new OrderSpecifications(orderId, email));
        public async Task<IReadOnlyList<Order?>> GetOrdersForUserAsync(string buyerEmail)
        => await _unitofwork.Repository<Order>().GetAllWithSpecAsync(new OrderSpecifications(buyerEmail));
        public async Task<IReadOnlyList<DeliveryMethod?>> GetDeliveryMethods()
        => await _unitofwork.Repository<DeliveryMethod>().GetAllAsync();

    }
}
