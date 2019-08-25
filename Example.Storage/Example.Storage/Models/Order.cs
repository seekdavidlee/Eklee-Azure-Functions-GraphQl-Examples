using Eklee.Azure.Functions.GraphQl.Connections;
using Example.Storage.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Storage.Models
{
	public class Order : IIdEntity
	{
		[Key]
		[Description("Id of the Order.")]
		public string Id { get; set; }

		[Description("True if order is made and False if order is cancelled.")]
		public bool IsActive { get; set; }

		[Description("Order date.")]
		public DateTime Ordered { get; set; }

		[Connection]
		[Description("List of products on order.")]
		public List<Product> Products { get; set; }
	}
}
