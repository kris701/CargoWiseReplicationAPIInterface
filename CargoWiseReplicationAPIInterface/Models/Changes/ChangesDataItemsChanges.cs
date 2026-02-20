using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataItemsChanges
	{
		[JsonPropertyName("__$operation")]
		public int Operation { get; set; }
		[JsonPropertyName("data")]
		public List<ChangesDataItemsChangesData> Data { get; set; }
	}
}
