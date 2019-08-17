﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Models
{
	public class Product
	{
		[Key]
		[Description("Id of the Product.")]
		public string Id { get; set; }

		[Description("Name of the Product.")]
		public string Name { get; set; }

		[Description("Price of the Product.")]
		public decimal Price { get; set; }
	}
}
