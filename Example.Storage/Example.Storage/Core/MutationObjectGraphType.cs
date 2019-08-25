using GraphQL.Types;
using Microsoft.Extensions.Configuration;
using Eklee.Azure.Functions.GraphQl;
using Eklee.Azure.Functions.GraphQl.Connections;
using Example.Storage.Models;

namespace Example.Storage.Core
{
	public class MutationObjectGraphType : ObjectGraphType
	{
		public MutationObjectGraphType(InputBuilderFactory inputBuilderFactory, IConfiguration configuration)
		{
			Name = "mutation";

			//--Begin--
			inputBuilderFactory.Create<ConnectionEdge>(this)
				.ConfigureTableStorage<ConnectionEdge>()
				.AddConnectionString(configuration["Storage:ConnectionString"])
				.AddPartition(x => x.SourceId)
				.BuildTableStorage()
				.Build();

			this.Add<Product, string>(inputBuilderFactory, configuration, x => x.Category);
			this.Add<Customer, string>(inputBuilderFactory, configuration, x => x.State);
			this.Add<Order, bool>(inputBuilderFactory, configuration, x => x.IsActive);
			//--End--
		}
	}
}
