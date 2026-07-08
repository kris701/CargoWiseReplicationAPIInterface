using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataItems
	{
		[JsonPropertyName("tableDefinitionSchemaVersion")]
		public string? TableDefinitionSchemaVersion { get; set; }
		[JsonPropertyName("version")]
		public string? OldVersion { get; set; }

		public string? Version { get {
				if (TableDefinitionSchemaVersion != null)
					return TableDefinitionSchemaVersion;
				if (OldVersion != null)
					return OldVersion;
				return null;
			} 
		}
		[JsonPropertyName("changes")]
		public List<ChangesDataItemsChanges> Changes { get; set; }
		[JsonPropertyName("columns")]
		public List<ChangesDataItemsColumns> Columns { get; set; }
	}
}
