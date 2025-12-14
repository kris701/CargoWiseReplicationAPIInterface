using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataDataItemsColumns
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("type")]
		public string Type { get; set; }
	}
}
