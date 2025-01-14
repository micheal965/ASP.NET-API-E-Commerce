
using Talabat.Core.Entities.Order_Aggregate;


namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        //get all orders
        public OrderSpecifications(string email)
            : base(o => o.BuyerEmail == email)
        {
            AddIncludes();
            AddOrderDesc(o => o.OrderDate);
        }
        //get specific order
        public OrderSpecifications(int orderId, string email)
            : base(o => o.Id == orderId && o.BuyerEmail == email)
        {
            AddIncludes();
        }
        public void AddIncludes()
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);
        }
    }
}
//SELECT[o].[Id], [o].[BuyerEmail], [o].[DeliveryMethodId], [o].[OrderDate], [o].[OrderStatus], [o].[PaymentIntentId], [o].[Subtotal], [o].[ShippingAddress_City], [o].[ShippingAddress_Country], [o].[ShippingAddress_FirstName], [o].[ShippingAddress_LastName], [o].[ShippingAddress_Street], [d].[Id], [d].[Cost], [d].[DeliveryTime], [d].[Description], [d].[ShortName]
//      FROM[Orders] AS[o]
//      INNER JOIN[DeliveryMethods] AS [d] ON[o].[DeliveryMethodId] = [d].[Id]
//      WHERE[o].[BuyerEmail] = @__email_0
//      ORDER BY[o].[OrderDate] DESC

//SELECT[o].[Id], [o].[BuyerEmail], [o].[DeliveryMethodId], [o].[OrderDate], [o].[OrderStatus], [o].[PaymentIntentId], [o].[Subtotal], [o].[ShippingAddress_City], [o].[ShippingAddress_Country], [o].[ShippingAddress_FirstName], [o].[ShippingAddress_LastName], [o].[ShippingAddress_Street], [d].[Id], [o0].[Id], [o0].[OrderId], [o0].[Price], [o0].[Quantity], [o0].[Product_PictureUrl], [o0].[Product_ProductId], [o0].[Product_ProductName], [d].[Cost], [d].[DeliveryTime], [d].[Description], [d].[ShortName]
//FROM[Orders] AS[o]
//INNER JOIN[DeliveryMethods] AS [d] ON[o].[DeliveryMethodId] = [d].[Id]
//LEFT JOIN[orderItems] AS [o0] ON[o].[Id] = [o0].[OrderId]
//WHERE[o].[Id] = @__orderId_0 AND[o].[BuyerEmail] = @__email_1
//ORDER BY[o].[Id], [d].[Id]