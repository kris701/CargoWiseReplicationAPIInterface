using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesData
	{
		[JsonPropertyName("currentItemCount")]
		public int CurrentItemCount { get; set; }
		[JsonPropertyName("itemsPerPage")]
		public int ItemsPerPage { get; set; }
		[JsonPropertyName("nextRequestParams")]
		public ChangesDataNextRequestParams NextRequestParams { get; set; }
		[JsonPropertyName("items")]
		public List<ChangesDataItems> Items { get; set; }
	}
}
