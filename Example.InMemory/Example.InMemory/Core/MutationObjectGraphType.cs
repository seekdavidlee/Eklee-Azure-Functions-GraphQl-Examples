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

			//--Begin--

			inputBuilderFactory.Create<Product>(this)
				.ConfigureInMemory<Product>()
				.BuildInMemory();

			inputBuilderFactory.Create<Customer>(this)
				.ConfigureInMemory<Customer>()
				.BuildInMemory();

			inputBuilderFactory.Create<Order>(this)
				.ConfigureInMemory<Order>()
				.BuildInMemory();

			//--End--
		}
	}
}
