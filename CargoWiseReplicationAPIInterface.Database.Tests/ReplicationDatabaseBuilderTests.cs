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
				new List<Type>(){ typeof(GlbGroup) },
				"TestFiles/expected1.sql"
			};
			yield return new object[] {
				new List<Type>(){ typeof(GlbGroup), typeof(GlbCompany), typeof(GenCustomAddOnValue) },
				"TestFiles/expected2.sql"
			};
		}

		[TestMethod]
		[DynamicData(nameof(InputData))]
		public void Can_Build(List<Type> tables, string expectedFile)
		{
			// ARRANGE
			var builder = new DatabaseQueryBuilder("CWO");
			var expectedText = File.ReadAllText(expectedFile);

			// ACT
			var actualText = builder.Build(tables);

			File.WriteAllText(expectedFile, actualText);

			// ASSERT
			Assert.AreEqual(expectedText, actualText);
		}
	}
}
