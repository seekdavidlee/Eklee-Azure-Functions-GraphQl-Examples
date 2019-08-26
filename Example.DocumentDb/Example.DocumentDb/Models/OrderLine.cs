using System;
using System.Collections.Generic;
using Eklee.Azure.Functions.GraphQl.Connections;
using System.ComponentModel;

namespace Example.DocumentDb.Models
{
	public class OrderLine
	{
		[ConnectionEdgeDestinationKey]
		[Description("Order line Id.")]
		public string Id { get; set; }

		[Description("Quantity.")]
		public int Quantity { get; set; }

		[Description("Sell price.")]
		public double SellPrice { get; set; }

		[Description("Line description.")]
		public string Description { get; set; }

		[ConnectionEdgeDestination]
		[Description("Product.")]
		public Product Product { get; set; }
	}
}
