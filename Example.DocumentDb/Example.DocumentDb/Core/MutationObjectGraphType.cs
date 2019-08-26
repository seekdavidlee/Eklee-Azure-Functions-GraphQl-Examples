using GraphQL.Types;
using Microsoft.Extensions.Configuration;
using Eklee.Azure.Functions.GraphQl;
using Eklee.Azure.Functions.GraphQl.Connections;
using Example.DocumentDb.Models;
using System;

namespace Example.DocumentDb.Core
{
	public class MutationObjectGraphType : ObjectGraphType
	{
		public MutationObjectGraphType(InputBuilderFactory inputBuilderFactory, IConfiguration configuration)
		{
			Name = "mutation";

			//--Begin--
			inputBuilderFactory.Create<ConnectionEdge>(this)
				.ConfigureDocumentDb<ConnectionEdge>()
				.AddDatabase(configuration["Db:Name"])
				.AddPartition(x => x.SourceId)
				.AddRequestUnit(Convert.ToInt32(configuration["Db:RequestUnits"]))
				.AddKey(configuration["Db:Key"])
				.AddUrl(configuration["Db:Url"])
				.BuildDocumentDb()
				.Build();

			this.Add<Product, string>(inputBuilderFactory, configuration, x => x.Category);
			this.Add<Customer, string>(inputBuilderFactory, configuration, x => x.State);
			this.Add<Order, bool>(inputBuilderFactory, configuration, x => x.IsActive);
			//--End--
		}
	}
}
