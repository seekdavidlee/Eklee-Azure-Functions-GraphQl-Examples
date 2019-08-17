using GraphQL.Types;
using Eklee.Azure.Functions.GraphQl;
using Microsoft.Extensions.Logging;
using Example.InMemory.Models;
using System.Linq;
using System.Collections.Generic;

namespace Example.InMemory.Core
{
	public class QueryConfigObjectGraphType : ObjectGraphType<object>
	{
		public QueryConfigObjectGraphType(QueryBuilderFactory queryBuilderFactory, ILogger logger)
		{
			Name = "query";

			// 1) Defines a simple way to get a single product by using the Key
			// which is the Id property in this case.
			queryBuilderFactory.Create<Product>(this, "GetProductById")
				.WithParameterBuilder()     // 2) Starts the query builder.
					.WithKeys()             // 3) Specify the use of Key attribute. This will create a query parameter on the client side to send in.
				.BuildQuery()               // 4) Finalize the query builder.
				.BuildWithSingleResult();   // 5) Specify that we are expected to get back a single entity.

			queryBuilderFactory.Create<CustomerOrder>(this, "GetOrderById")
				.WithParameterBuilder()
				.BeginQuery<Order>()
					.WithProperty(x => x.Id)
					.BuildQueryResult((ctx) =>
					{
						var results = ctx.GetQueryResults<Order>();

						// Only a single order should match.
						var order = results.Single();

						ctx.SetResults(new List<CustomerOrder>
						{
							new CustomerOrder
							{
								Id = order.Id,
								IsActive = order.IsActive,
								Ordered = order.Ordered
							}
						});
						ctx.Items["CustomerIdList"] = results.Select(x => (object)x.CustomerId).ToList();
						ctx.Items["ProductIdList"] = order.ProductIdList.Select(x => (object)x).ToList();
					})
				.ThenWithQuery<Product>()
					.WithPropertyFromSource(y => y.Id, ctx => (List<object>)ctx.Items["ProductIdList"])
					.BuildQueryResult(ctx =>
					{
						var customerOrder = ctx.GetResults<CustomerOrder>().Single();
						customerOrder.Products = ctx.GetQueryResults<Product>();
					})
				.ThenWithQuery<Customer>()
					.WithPropertyFromSource(y => y.Id, ctx => (List<object>)ctx.Items["CustomerIdList"])
					.BuildQueryResult(ctx =>
					{
						var customerOrder = ctx.GetResults<CustomerOrder>().Single();
						customerOrder.Customer = ctx.GetQueryResults<Customer>().Single();
					})
				.BuildQuery()
				.BuildWithSingleResult();

			//--End--
		}
	}
}
