using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderWithPaymentIntentSpecification : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecification(string paymentintentid)
        : base(o => o.PaymentIntentId == paymentintentid)
        {

        }
    }
}
