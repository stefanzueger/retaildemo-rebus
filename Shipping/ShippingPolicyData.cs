using System;
using Rebus.Sagas;

namespace Shipping
{
    public class ShippingPolicyData : ISagaData
    {
        public Guid Id { get; set; }

        public int Revision { get; set; }

        public string OrderId { get; set; }

        public bool IsOrderPlaced { get; set; }

        public bool IsOrderBilled { get; set; }
    }
}