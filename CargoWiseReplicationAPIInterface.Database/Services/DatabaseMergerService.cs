using CargoWiseReplicationAPIInterface.Database.Attributes;
using CargoWiseReplicationAPIInterface.Database.Models;
using CargoWiseReplicationAPIInterface.Models;
using DatabaseSharp;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Database.Services
{
	/// <summary>
	/// Service for merging data fetched from the replication API
	/// </summary>
	public class DatabaseMergerService
	{
		/// <summary>
		/// The schema to look for the STP in
		/// </summary>
		public string Schema { get; set; }
		/// <summary>
		/// How many items to merge at a time
		/// </summary>
		public int BatchSize { get; set; }
		/// <summary>
		/// If something fails, retry this many times before throwing
		/// </summary>
		public int RetryTimes { get; set; } = 5;

		private readonly IDBClient _dbClient;

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="dbClient"></param>
		/// <param name="batchSize"></param>
		/// <param name="schema"></param>
		public DatabaseMergerService(IDBClient dbClient, int batchSize, string schema)
		{
			_dbClient = dbClient;
			BatchSize = batchSize;
			Schema = schema;
		}

		/// <summary>
		/// Merge a list of data into a database
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="logger"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task Merge<T>(List<T> data, ILogger logger, CancellationToken cancellationToken) where T : BaseReturnData
		{
			var tableName = typeof(T).Name;
			var stp = $"[{Schema}].[Merge{tableName}]";

			var properties = typeof(T).GetProperties();
			var pkProp = properties.First(x => x.GetCustomAttribute<ReplicationPrimaryKey>() != null);

			var toDelete = data.Where(x => x.OpCode == 1).ToList();
			var pksToDelete = toDelete.Select(x => pkProp.GetValue(x));

			var toInsert = data.Where(x => x.OpCode == 2).Where(x => !pksToDelete.Contains(pkProp.GetValue(x))).ToList();
			var toUpdate = data.Where(x => x.OpCode == 3 || x.OpCode == 4).Where(x => !pksToDelete.Contains(pkProp.GetValue(x))).GroupBy(x => pkProp.GetValue(x)).Select(x => x.Last()).ToList();

			if (toInsert.Count == 0 && toUpdate.Count == 0 && toDelete.Count == 0)
				return;

			var sb = new StringBuilder($"'{tableName}' executing ");
			if (toInsert.Count > 0)
				sb.Append($"{toInsert.Count} insert(s), ");
			if (toUpdate.Count > 0)
				sb.Append($"{toUpdate.Count} update(s), ");
			if (toDelete.Count > 0)
				sb.Append($"{toDelete.Count} delete(s)");
			logger.LogInformation(sb.ToString());

			if (toInsert.Count > 0)
				await MergeSet(toInsert, stp, logger, tableName, cancellationToken);

			if (toUpdate.Count > 0)
				await MergeSet(toUpdate, stp, logger, tableName, cancellationToken);

			if (toDelete.Count > 0)
				await MergeSet(toDelete, stp, logger, tableName, cancellationToken);
		}

		private async Task MergeSet<T>(List<T> data, string stp, ILogger logger, string tableName, CancellationToken cancellationToken)
		{
			var retryCount = 0;

			while (data.Count > 0 && !cancellationToken.IsCancellationRequested)
			{
				while (retryCount < RetryTimes)
				{
					try
					{
						var inputList = data.Take(BatchSize).ToList();
						await _dbClient.ExecuteAsync(stp, new DatabaseInputModel<T>(inputList));
						if (data.Count <= BatchSize)
							data.Clear();
						else
							data.RemoveRange(0, BatchSize);
						break;
					}
					catch (Exception e)
					{
						retryCount++;
						if (retryCount >= RetryTimes)
						{
							logger.LogError($"Error on '{tableName}': {e.Message}. Tried {RetryTimes} times.");
							throw new Exception($"Error on '{tableName}': {e.Message}. Tried {RetryTimes} times.");
						}
						else
						{
							logger.LogError($"Error on '{tableName}': {e.Message}. Retrying {retryCount}/{RetryTimes} in 30 seconds...");
							await Task.Delay(30000, cancellationToken);
						}
					}
				}
			}
		}
	}
}
