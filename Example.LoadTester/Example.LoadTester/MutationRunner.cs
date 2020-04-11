using Example.LoadTester.Values;
using GraphQL;
using GraphQL.Client.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.LoadTester
{
	public class MutationRunner
	{
		private readonly GraphQLHttpClient _client;
		private readonly Mutation _mutation;
		private readonly ValuesProvider _valuesProvider = new ValuesProvider();

		public MutationRunner(GraphQLHttpClient client, Mutation mutation)
		{
			_client = client;
			_mutation = mutation;
		}

		public string CreatePayload()
		{
			var sb = new StringBuilder("mutation {");
			sb.AppendLine($"\t{_mutation.Name}(");

			_mutation.Inputs.ForEach(mi =>
			{
				sb.Append(mi.Type);
				if (mi.BatchCount.HasValue)
				{
					sb.Append(":[");

					int lastIndex = mi.BatchCount.Value - 1;

					for (var i = 0; i < mi.BatchCount.Value; i++)
					{
						CreateEntity(sb, mi.Value);
						if (i != lastIndex)
						{
							sb.Append(",");
						}
					}

					sb.AppendLine(" ]");
				}
			});

			sb.AppendLine("){");
			sb.AppendLine(_mutation.Output);
			sb.AppendLine("}");
			sb.AppendLine("}");

			return sb.ToString();
		}

		private void CreateEntity(StringBuilder sb, Dictionary<string, object> defs)
		{
			sb.AppendLine("{");

			defs.Keys.ToList().ForEach(key =>
			{
				string value = _valuesProvider.Generate((string)defs[key]);

				sb.AppendLine($"{key}: {value}");
			});

			sb.AppendLine("}");
		}

		public async Task<MutationRunStats> RunAsync()
		{
			var result = new MutationRunStats();

			var payload = CreatePayload();

			result.PayloadSizeInKb = Encoding.UTF8.GetByteCount(payload) / (double)1024;
			result.Start = DateTime.UtcNow;

			var response = await _client.SendMutationAsync<object>(new GraphQLRequest
			{
				Query = payload
			});

			result.Errors = response.Errors;
			result.End = DateTime.UtcNow;

			return result;
		}
	}
}
