using CargoWiseReplicationAPIInterface.Models;
using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Models.Summary;
using CargoWiseReplicationAPIInterface.Tests.Mocks;
using CargoWiseReplicationAPIInterface.Tests.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CargoWiseReplicationAPIInterface.Tests
{
	[TestClass]
	public class ReplicationAPIInterfaceTests
	{
		[TestMethod]
		[DataRow("TestFiles/Changes/input1.json", "TestFiles/Changes/expected1.json")]
		[DataRow("TestFiles/Changes/input3.json", "TestFiles/Changes/expected3.json")]
		public async Task Can_ConvertChanges_GlbCompanyModel(string inputFile, string expectedFile)
		{
			// ARRANGE
			var mockApi = new MockReplicationAPIService();
			var repicationInterface = new ReplicationAPI(mockApi);
			var changes = JsonSerializer.Deserialize<ChangesModel>(File.ReadAllText(inputFile));
			Assert.IsNotNull(changes);
			mockApi.ChangesToReturn = changes.Changes;

			// ACT
			var actual = await repicationInterface.GetDetails<GlbCompanyModel>("0x00000000000000000000", "0xFFFFFFFFFFFFFFFFFFFF", "dbo", "GlbCompany");

			// ASSERT
			var expectedObject = JsonSerializer.Deserialize<List<GlbCompanyModel>>(File.ReadAllText(expectedFile));
			var expectedText = JsonSerializer.Serialize(expectedObject);
			var actualText = JsonSerializer.Serialize(actual);
			Assert.AreEqual(expectedText, actualText);
		}

		[TestMethod]
		[DataRow("TestFiles/Changes/input4.json", "TestFiles/Changes/expected4.json")]
		public async Task Can_ConvertChanges_GenCustomAddOnValue(string inputFile, string expectedFile)
		{
			// ARRANGE
			var mockApi = new MockReplicationAPIService();
			var repicationInterface = new ReplicationAPI(mockApi);
			var fileText = File.ReadAllText(inputFile);
			fileText = fileText.Replace("\u001e","");
			var changes = JsonSerializer.Deserialize<ChangesModel>(fileText);
			Assert.IsNotNull(changes);
			mockApi.ChangesToReturn = changes.Changes;

			// ACT
			var actual = await repicationInterface.GetDetails<GenCustomAddOnValue>("0x00000000000000000000", "0xFFFFFFFFFFFFFFFFFFFF", "dbo", "GenCustomAddOnValue");

			// ASSERT
			var expectedObject = JsonSerializer.Deserialize<List<GenCustomAddOnValue>>(File.ReadAllText(expectedFile));
			var expectedText = JsonSerializer.Serialize(expectedObject);
			var actualText = JsonSerializer.Serialize(actual);
			Assert.AreEqual(expectedText, actualText);
		}

		[TestMethod]
		[DataRow("TestFiles/Changes/input5.json", "TestFiles/Changes/expected5.json")]
		public async Task Can_ConvertChanges_GlbGroup(string inputFile, string expectedFile)
		{
			// ARRANGE
			var mockApi = new MockReplicationAPIService();
			var repicationInterface = new ReplicationAPI(mockApi);
			var fileText = File.ReadAllText(inputFile);
			fileText = fileText.Replace("\u001e", "");
			var changes = JsonSerializer.Deserialize<ChangesModel>(fileText);
			Assert.IsNotNull(changes);
			mockApi.ChangesToReturn = changes.Changes;

			// ACT
			var actual = await repicationInterface.GetDetails<GlbGroup>("0x00000000000000000000", "0xFFFFFFFFFFFFFFFFFFFF", "dbo", "GlbGroup");

			// ASSERT
			var expectedObject = JsonSerializer.Deserialize<List<GlbGroup>>(File.ReadAllText(expectedFile));
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