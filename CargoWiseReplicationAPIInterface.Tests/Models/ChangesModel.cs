using CargoWiseReplicationAPIInterface.Models.Changes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Tests.Models
{
	public class ChangesModel
	{
		[JsonPropertyName("changes")]
		public List<ChangesResponse> Changes { get; set; } = new List<ChangesResponse>();
	}
}
