using CargoWiseReplicationAPIInterface.Database.Models;
using DatabaseSharp;
using DatabaseSharp.Tools;
using ToolsSharp.Serialization;

namespace Helvion.Helvware.Plugins.TrendhimIntegration.Interfaces.Internal
{
	internal class SetLSNModel(IDBClient dbClient, string schema) : BaseSingleSerializerModel<LastLSNModel, EmptyModel, EmptyModel>(dbClient, $"{schema}.SetLastLSN")
	{
	}
}
