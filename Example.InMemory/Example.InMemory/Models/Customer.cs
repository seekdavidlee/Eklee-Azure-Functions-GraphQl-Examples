using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.InMemory.Models
{
	public class Customer
	{
		[Key]
		[Description("Id of the Product.")]
		public string Id { get; set; }

		[Description("Name of the Product.")]
		public string Name { get; set; }
	}
}
