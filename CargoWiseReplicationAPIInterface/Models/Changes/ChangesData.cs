using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesData
	{
		[JsonPropertyName("data")]
		public ChangesDataData Data { get; set; }
	}
}
