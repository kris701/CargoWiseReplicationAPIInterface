using CargoWiseReplicationAPIInterface.Database.Models;
using CargoWiseReplicationAPIInterface.Database.Services;
using DatabaseSharp;
using Helvion.Helvware.Plugins.TrendhimIntegration.Interfaces.Internal;
using Microsoft.Extensions.Logging;
using ToolsSharp.Serialization;

namespace CargoWiseReplicationAPIInterface.Database
{
	/// <summary>
	/// A basic implementation of a full database depending replication service
	/// </summary>
	public class DatabaseReplicator
	{
		private readonly GetLSNModel _getLSNModel;
		private readonly SetLSNModel _setLSNModel;
		private readonly ReplicationAPI _replicationInterface;
		private readonly DatabaseMergerService _mergerService;
		
		private readonly List<Type> _dataTableTypes;

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="dbClient"></param>
		/// <param name="schema"></param>
		/// <param name="batchSizes"></param>
		/// <param name="dataTableTypes"></param>
		/// <param name="replicationUrl"></param>
		/// <param name="replicationUsername"></param>
		/// <param name="replicationPassword"></param>
		public DatabaseReplicator(
			IDBClient dbClient, 
			string schema, 
			int batchSizes,
			List<Type> dataTableTypes,
			string replicationUrl,
			string replicationUsername,
			string replicationPassword)
		{
			_dataTableTypes = dataTableTypes;

			_getLSNModel = new GetLSNModel(dbClient, schema);
			_setLSNModel = new SetLSNModel(dbClient, schema);
			_replicationInterface = new ReplicationAPI(replicationUrl, replicationUsername, replicationPassword);
			_mergerService = new DatabaseMergerService(dbClient, batchSizes, schema);
		}

		/// <summary>
		/// Replicate
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="stoppingToken"></param>
		/// <returns></returns>
		public async Task Replicate(ILogger logger, CancellationToken stoppingToken)
		{
			var lastLsns = await _getLSNModel.ExecuteAsync(new EmptyModel());
			// Check if the summary LSN value is set.
			var summaryLsn = lastLsns.FirstOrDefault(x => x.TableCode == "SUMMARY");
			if (summaryLsn == null)
			{
				// If not, set the "LastLSN" for SUMMARY in the database to be the current max LSN for the newest summary.
				summaryLsn = new LastLSNModel() { TableCode = "SUMMARY", LastLSN = "0x00000000000000000000" };
				var maxSummaryCheck = await _replicationInterface.GetSummary(summaryLsn.LastLSN);
				summaryLsn.LastLSN = maxSummaryCheck.MaxLSN;
			}

			var summary = await _replicationInterface.GetSummary(summaryLsn.LastLSN);
			summary.Items.RemoveAll(x => !_dataTableTypes.Any(y => y.Name == x.TableName));

			var updateTime = DateTime.UtcNow;

			if (summary.Items.Count > 0)
			{
				logger.LogInformation($"A total of {summary.Items.Count} tables to replicate...");
				foreach (var table in summary.Items)
				{
					logger.LogInformation($"Replicating {table.TableName}...");
					var targetLsn = lastLsns.FirstOrDefault(x => x.TableCode == table.TableName);
					if (targetLsn == null)
						targetLsn = new LastLSNModel() { TableCode = table.TableName, LastLSN = summaryLsn.LastLSN };

					await ProcessReplication(table.SchemaName, table.TableName, targetLsn.LastLSN, summary.MaxLSN, logger, stoppingToken);

					targetLsn.LastLSN = summary.MaxLSN;
					targetLsn.UpdatedAt = updateTime;
					await _setLSNModel.ExecuteAsync(targetLsn);
				}
				logger.LogInformation($"Replication complete!");
			}

			summaryLsn.LastLSN = summary.MaxLSN;
			summaryLsn.UpdatedAt = updateTime;
			await _setLSNModel.ExecuteAsync(summaryLsn);
		}

		private async Task ProcessReplication(string schemaName, string tableName, string afterLSN, string maxLSN, ILogger logger, CancellationToken cancellationToken)
		{
			var targetType = _dataTableTypes.FirstOrDefault(x => x.Name == tableName);
			if (targetType == null)
				return;

			var data = await _replicationInterface.GetDetails(afterLSN, maxLSN, schemaName, tableName, targetType);
			await _mergerService.Merge(data, logger, cancellationToken);
		}
	}
}
