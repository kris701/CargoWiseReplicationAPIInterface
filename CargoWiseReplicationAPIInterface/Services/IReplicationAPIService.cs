using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Models.Summary;

namespace CargoWiseReplicationAPIInterface.Services
{
	public interface IReplicationAPIService
	{
		public Task<SummaryResponse> GetSummary(string afterLsn);

		public Task<ChangesResponse> GetChanges(string afterLsn, string maxLsn, string schemaName, string tableName);
		public Task<ChangesResponse?> GetChangesFromLast(ChangesResponse last, string maxLsn, string schemaName, string tableName);
	}
}
