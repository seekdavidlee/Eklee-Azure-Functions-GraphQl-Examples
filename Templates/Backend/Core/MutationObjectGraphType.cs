using GraphQL.Types;
using Microsoft.Extensions.Configuration;
using Eklee.Azure.Functions.GraphQl;

namespace Example.Core
{
	public class MutationObjectGraphType : ObjectGraphType
	{
		public MutationObjectGraphType(InputBuilderFactory inputBuilderFactory, IConfiguration configuration)
		{
			Name = "mutation";

			//--Begin--

			//--End--
		}
	}
}
