using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CargoWiseReplicationAPIInterface.Tests
{
	[TestClass]
	public class ReplicationAPIInterfaceTests
	{
		[TestMethod]
		[DataRow("TestFiles/input1.json", "TestFiles/expected1.json")]
		public void Can_ConvertChanges(string inputFile, string expectedFile)
		{
			// ARRANGE
			var repicationInterface = new ReplicationAPIInterface("","","");
			var changes = JsonSerializer.Deserialize<ChangesModel>(File.ReadAllText(inputFile));
			Assert.IsNotNull(changes);

			// ACT
			var converted = repicationInterface.ConvertChanges<GlbCompanyModel>(changes.Changes);

			// ASSERT

		}
	}
}
