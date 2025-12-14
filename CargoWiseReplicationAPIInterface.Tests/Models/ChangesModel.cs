using CargoWiseReplicationAPIInterface.Models.Changes;
using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Tests.Models
{
	public class ChangesModel
	{
		[JsonPropertyName("changes")]
		public List<ChangesResponse> Changes { get; set; } = new List<ChangesResponse>();
	}
}
