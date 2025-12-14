using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataDataItemsChanges
	{
		[JsonPropertyName("__$operation")]
		public int Operation { get; set; }
		[JsonPropertyName("data")]
		public List<ChangesDataDataItemsChangesData> Data { get; set; }
	}
}
