using System.ComponentModel;

namespace Example.DocumentDb.Models
{
	public class Status
	{
		[Description("Message describing the status of the request.")]
		public string Message { get; set; }
	}
}
