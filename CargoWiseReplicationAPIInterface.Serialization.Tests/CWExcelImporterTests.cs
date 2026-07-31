using CargoWiseReplicationAPIInterface.Serialization.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CargoWiseReplicationAPIInterface.Serialization.Tests
{
	[TestClass]
	public class CWExcelImporterTests
	{
		[TestMethod]
		[DataRow("TestFiles/TestModelData.xlsx")]
		public async Task Can_Import(string excelFile)
		{
			// ARRANGE
			var merger = new MockDatabaseMergerService();
			var importer = new CWExcelImporter(merger, NullLogger<CWExcelImporterTests>.Instance, "CargoWiseReplicationAPIInterface.Serialization.Tests.Models");

			// ACT
			using (var str = File.OpenRead(excelFile))
			{
				await importer.ImportExcel(str, false);
			}

			// ASSERT
			Assert.AreEqual(3, merger.Data.Count);
		}
	}
}
