using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataItems
	{
		[JsonPropertyName("tableDefinitionSchemaVersion")]
		public string Version { get; set; }
		[JsonPropertyName("changes")]
		public List<ChangesDataItemsChanges> Changes { get; set; }
		[JsonPropertyName("columns")]
		public List<ChangesDataItemsColumns> Columns { get; set; }
	}
}
