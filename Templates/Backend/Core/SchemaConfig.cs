using GraphQL;
using GraphQL.Types;

namespace Example.Core
{
	public class SchemaConfig : Schema
	{
		public SchemaConfig(IDependencyResolver resolver, QueryConfigObjectGraphType query, MutationObjectGraphType mutation) : base(resolver)
		{
			Query = query;
			Mutation = mutation;
		}
	}
}
