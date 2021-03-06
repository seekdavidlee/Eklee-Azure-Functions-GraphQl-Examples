using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.InMemory.Models
{
	public class Order
	{
		[Key]
		[Description("Id of the Product.")]
		public string Id { get; set; }

		[Description("Order date.")]
		public DateTime Ordered { get; set; }

		[Description("True if order is made and False if order is cancelled.")]
		public bool IsActive { get; set; }

		[Description("Customer who made this order.")]
		public string CustomerId { get; set; }

		[Description("Id of the Product.")]
		public List<string> ProductIdList { get; set; }
	}
}
