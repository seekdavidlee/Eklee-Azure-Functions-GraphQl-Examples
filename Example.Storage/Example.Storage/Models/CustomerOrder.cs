using Eklee.Azure.Functions.GraphQl.Connections;
using System.ComponentModel;

namespace Example.Storage.Models
{
	public class CustomerOrder
	{
		[ConnectionEdgeDestinationKey]
		[Description("Customer Order Id.")]
		public string Id { get; set; }

		[ConnectionEdgeDestination]
		[Description("Order.")]
		public Order Order { get; set; }
	}
}
