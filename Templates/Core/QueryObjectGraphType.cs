using GraphQL.Types;
using Eklee.Azure.Functions.GraphQl;
using Microsoft.Extensions.Logging;

namespace Example.Core
{
	public class QueryConfigObjectGraphType : ObjectGraphType<object>
	{
		public QueryConfigObjectGraphType(QueryBuilderFactory queryBuilderFactory, ILogger logger)
		{
			Name = "query";

			//--Begin--

			//--End--
		}
	}
}
