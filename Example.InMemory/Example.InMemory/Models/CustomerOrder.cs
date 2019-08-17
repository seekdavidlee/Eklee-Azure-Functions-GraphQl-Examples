using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.InMemory.Models
{
	public class CustomerOrder
	{
		[Key]
		[Description("Id of the Product.")]
		public string Id { get; set; }

		[Description("Order date.")]
		public DateTime Ordered { get; set; }

		[Description("True if order is made and False if order is cancelled.")]
		public bool IsActive { get; set; }

		[Description("Customer who made this order.")]
		public Customer Customer { get; set; }

		[Description("List of Products beloging to Order.")]
		public List<Product> Products { get; set; }
	}
}
