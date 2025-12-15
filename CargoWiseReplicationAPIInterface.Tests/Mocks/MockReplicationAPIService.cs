using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Models.Summary;
using CargoWiseReplicationAPIInterface.Services;

namespace CargoWiseReplicationAPIInterface.Tests.Mocks
{
	public class MockReplicationAPIService : IReplicationAPIService
	{
		public int ChangesIndex { get; set; } = 0;
		public List<ChangesResponse> ChangesToReturn { get; set; } = new List<ChangesResponse>();

		public async Task<ChangesResponse> GetChanges(string afterLsn, string maxLsn, string schemaName, string tableName)
		{
			return ChangesToReturn[ChangesIndex++];
		}

		public async Task<ChangesResponse?> GetChangesFromLast(ChangesResponse last, string maxLsn, string schemaName, string tableName)
		{
			if (ChangesIndex >= ChangesToReturn.Count)
				return null;
			return ChangesToReturn[ChangesIndex++];
		}

		public Task<SummaryResponse> GetSummary(string afterLsn)
		{
			throw new NotImplementedException();
		}
	}
}
