using GraphQL.Types;
using Eklee.Azure.Functions.GraphQl;
using Microsoft.Extensions.Logging;
using Example.DocumentDb.Models;

namespace Example.DocumentDb.Core
{
	public class QueryConfigObjectGraphType : ObjectGraphType<object>
	{
		public QueryConfigObjectGraphType(QueryBuilderFactory queryBuilderFactory, ILogger logger)
		{
			Name = "query";

			//--Begin--
			queryBuilderFactory.Create<Product>(this, "GetAllProducts", "Get all products.")
				.WithParameterBuilder()
				.BuildQuery()
				.BuildWithListResult();

			queryBuilderFactory.Create<Customer>(this, "GetCustomerById", "Get Customer By Id.")
				.WithParameterBuilder()
				.WithKeys()
				.BuildQuery()
				.BuildWithSingleResult();

			queryBuilderFactory.Create<Order>(this, "GetOrderById", "Get Order By Id.")
				.WithParameterBuilder()
				.WithKeys()
				.BuildQuery()
				.BuildWithSingleResult();

			//--End--
		}
	}
}
