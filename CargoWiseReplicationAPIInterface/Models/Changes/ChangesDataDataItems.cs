using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataDataItems
	{
		[JsonPropertyName("version")]
		public string Version { get; set; }
		[JsonPropertyName("schemaChangeCount")]
		public int SchemaChangeCount { get; set; }
		[JsonPropertyName("changes")]
		public List<ChangesDataDataItemsChanges> Changes { get; set; }
		[JsonPropertyName("columns")]
		public List<ChangesDataDataItemsColumns> Columns { get; set; }
	}
}
