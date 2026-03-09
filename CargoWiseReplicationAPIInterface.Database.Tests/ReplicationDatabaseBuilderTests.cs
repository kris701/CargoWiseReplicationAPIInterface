using CargoWiseReplicationAPIInterface.Database.Tests.Models;
using CargoWiseReplicationAPIInterface.Models;

namespace CargoWiseReplicationAPIInterface.Database.Tests
{
	[TestClass]
	public class ReplicationDatabaseBuilderTests
	{
		public static IEnumerable<object[]> InputData()
		{
			yield return new object[] {
				new List<BaseReturnData>(){ new GlbGroup() },
				"TestFiles/expected1.sql"
			};
			yield return new object[] {
				new List<BaseReturnData>(){ new GlbGroup(), new GlbCompany(), new GenCustomAddOnValue() },
				"TestFiles/expected2.sql"
			};
		}

		[TestMethod]
		[DynamicData(nameof(InputData))]
		public void Can_Build(List<BaseReturnData> tables, string expectedFile)
		{
			// ARRANGE
			var builder = new ReplicationDatabaseBuilder("", "CWO");
			var expectedText = File.ReadAllText(expectedFile);

			// ACT
			var actualText = builder.Build(tables);

			// ASSERT
			Assert.AreEqual(expectedText, actualText);
		}
	}
}
