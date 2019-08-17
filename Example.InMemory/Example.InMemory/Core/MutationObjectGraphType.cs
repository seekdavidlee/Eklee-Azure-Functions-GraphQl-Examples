using GraphQL.Types;
using Microsoft.Extensions.Configuration;
using Eklee.Azure.Functions.GraphQl;
using Example.InMemory.Models;

namespace Example.InMemory.Core
{
	public class MutationObjectGraphType : ObjectGraphType
	{
		public MutationObjectGraphType(InputBuilderFactory inputBuilderFactory, IConfiguration configuration)
		{
			Name = "mutation";

			// 1) Use in memory repository to store Product information.
			inputBuilderFactory.Create<Product>(this)
				.ConfigureInMemory<Product>()
				.BuildInMemory()    // 2) Nothing to configure.
				.Build();           // 3) Build the type.

			// Use in memory repository to store Customer information.
			inputBuilderFactory.Create<Customer>(this)
				.ConfigureInMemory<Customer>()
				.BuildInMemory()
				.Build(); ;

			// Use in memory repository to store Order information.
			inputBuilderFactory.Create<Order>(this)
				.ConfigureInMemory<Order>()
				.BuildInMemory()
				.Build(); ;
		}
	}
}
