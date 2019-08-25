using Eklee.Azure.Functions.GraphQl.Connections;
using Example.Storage.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Storage.Models
{
	public class Customer : IIdEntity
	{
		[Key]
		[Description("Id of the Product.")]
		public string Id { get; set; }

		[Description("Name of the Customer.")]
		public string Name { get; set; }

		[Description("State of the Customer.")]
		public string State { get; set; }

		[Connection]
		[Description("Orders made by customer.")]
		public List<CustomerOrder> Orders { get; set; }
	}
}
