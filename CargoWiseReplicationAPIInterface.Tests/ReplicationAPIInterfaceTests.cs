using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Models.Summary;
using CargoWiseReplicationAPIInterface.Tests.Models;
using System.Text.Json;

namespace CargoWiseReplicationAPIInterface.Tests
{
	[TestClass]
	public class ReplicationAPIInterfaceTests
	{
		[TestMethod]
		[DataRow("TestFiles/Changes/input1.json", "TestFiles/Changes/expected1.json")]
		[DataRow("TestFiles/Changes/input3.json", "TestFiles/Changes/expected3.json")]
		public void Can_ConvertChanges(string inputFile, string expectedFile)
		{
			// ARRANGE
			var repicationInterface = new ReplicationAPIInterface("", "", "");
			var changes = JsonSerializer.Deserialize<ChangesModel>(File.ReadAllText(inputFile));
			Assert.IsNotNull(changes);

			// ACT
			var actual = repicationInterface.ConvertChanges(changes.Changes, typeof(GlbCompanyModel));

			// ASSERT
			var expectedObject = JsonSerializer.Deserialize<List<GlbCompanyModel>>(File.ReadAllText(expectedFile));
			var expectedText = JsonSerializer.Serialize(expectedObject);
			var actualText = JsonSerializer.Serialize(actual);
			Assert.AreEqual(expectedText, actualText);
		}

		[TestMethod]
		[DataRow("TestFiles/Summary/input1.json", "TestFiles/Summary/expected1.json")]
		public void Can_ParseSummaryResponse(string inputFile, string expectedFile)
		{
			// ARRANGE
			// ACT
			var actualObject = JsonSerializer.Deserialize<SummaryResponse>(File.ReadAllText(inputFile));

			// ASSERT
			var expectedObject = JsonSerializer.Deserialize<SummaryResponse>(File.ReadAllText(expectedFile));
			var expectedText = JsonSerializer.Serialize(expectedObject);
			var actualText = JsonSerializer.Serialize(actualObject);
			Assert.AreEqual(expectedText, actualText);
		}

		[TestMethod]
		[DataRow("TestFiles/Changes/input2.json", "TestFiles/Changes/expected2.json")]
		public void Can_ParseDetailsResponse(string inputFile, string expectedFile)
		{
			// ARRANGE
			// ACT
			var actualObject = JsonSerializer.Deserialize<ChangesResponse>(File.ReadAllText(inputFile));

			// ASSERT
			var expectedObject = JsonSerializer.Deserialize<ChangesResponse>(File.ReadAllText(expectedFile));
			var expectedText = JsonSerializer.Serialize(expectedObject);
			var actualText = JsonSerializer.Serialize(actualObject);
			Assert.AreEqual(expectedText, actualText);
		}
	}
}
