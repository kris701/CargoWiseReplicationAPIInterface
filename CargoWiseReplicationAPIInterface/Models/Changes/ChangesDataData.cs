using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataData
	{
		[JsonPropertyName("currentItemCount")]
		public int CurrentItemCount { get; set; }
		[JsonPropertyName("itemsPerPage")]
		public int ItemsPerPage { get; set; }
		[JsonPropertyName("nextRequestParams")]
		public ChangesDataDataNextRequestParams NextRequestParams { get; set; }
		[JsonPropertyName("items")]
		public List<ChangesDataDataItems> Items { get; set; }
	}
}
