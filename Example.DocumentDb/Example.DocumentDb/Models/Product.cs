using Example.DocumentDb.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.DocumentDb.Models
{
	public class Product : IIdEntity
	{
		[Key]
		[Description("Id of the Product.")]
		public string Id { get; set; }

		[Description("Name of the Product.")]
		public string Name { get; set; }

		[Description("Description of the Product.")]
		public string Description { get; set; }

		[Description("Category of the Product.")]
		public string Category { get; set; }

		[Description("Sell price of the Product.")]
		public double SellPrice { get; set; }

		[Description("Cost price of the Product.")]
		public double CostPrice { get; set; }
	}
}
