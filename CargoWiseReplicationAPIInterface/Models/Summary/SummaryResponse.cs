using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Summary
{
	public class SummaryResponse
	{
		[JsonPropertyName("data")]
		public SummaryResponseData Data { get; set; }
	}
}
