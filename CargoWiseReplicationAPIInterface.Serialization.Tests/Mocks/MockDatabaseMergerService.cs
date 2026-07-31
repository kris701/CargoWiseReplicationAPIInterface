using CargoWiseReplicationAPIInterface.Database.Services;
using CargoWiseReplicationAPIInterface.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Serialization.Tests.Mocks
{
	public class MockDatabaseMergerService : IDatabaseMergerService
	{
		public List<dynamic> Data { get; internal set; } = new List<dynamic>();

		public async Task Merge<T>(List<T> data, ILogger logger, CancellationToken cancellationToken) where T : BaseReturnData
		{
			Data = new List<dynamic>(data);
			return;
		}
	}
}
