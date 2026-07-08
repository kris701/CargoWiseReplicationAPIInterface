using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Models.Summary;
using CargoWiseReplicationAPIInterface.Services;

namespace CargoWiseReplicationAPIInterface.Tests.Mocks
{
	public class MockReplicationAPIService : IReplicationAPIService
	{
		public int ChangesIndex { get; set; } = 0;
		public List<ChangesData> ChangesToReturn { get; set; } = new List<ChangesData>();

		public async Task<ChangesData> GetChanges(string afterLsn, string maxLsn, string schemaName, string tableName)
		{
			return ChangesToReturn[ChangesIndex++];
		}

		public async Task<ChangesData?> GetChangesFromLast(ChangesData last, string maxLsn, string schemaName, string tableName)
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
