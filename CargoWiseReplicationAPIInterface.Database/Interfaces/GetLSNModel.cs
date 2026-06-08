using CargoWiseReplicationAPIInterface.Database.Models;
using DatabaseSharp;
using DatabaseSharp.Tools;
using ToolsSharp.Serialization;

namespace Helvion.Helvware.Plugins.TrendhimIntegration.Interfaces.Internal
{
	internal class GetLSNModel(IDBClient dbClient, string schema) : BaseListSerializerModel<EmptyModel, LastLSNModel, EmptyModel>(dbClient, $"{schema}.GetLastLSN")
	{
	}
}
