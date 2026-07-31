using CargoWiseReplicationAPIInterface.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Database.Services
{
	public interface IDatabaseMergerService
	{
		public Task Merge<T>(List<T> data, ILogger logger, CancellationToken cancellationToken) where T : BaseReturnData;
	}
}
