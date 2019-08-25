using GraphQL.Types;
using Eklee.Azure.Functions.GraphQl;
using Microsoft.Extensions.Logging;
using Example.Storage.Models;

namespace Example.Storage.Core
{
	public class QueryConfigObjectGraphType : ObjectGraphType<object>
	{
		public QueryConfigObjectGraphType(QueryBuilderFactory queryBuilderFactory, ILogger logger)
		{
			Name = "query";

			//--Begin--

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
