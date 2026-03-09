using CargoWiseReplicationAPIInterface.Database.Attributes;
using CargoWiseReplicationAPIInterface.Models;
using System.Reflection;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Database
{
	/// <summary>
	/// An interface that can be used to build a SQL query for a replication database
	/// </summary>
	public class DatabaseQueryBuilder : IDatabaseQueryBuilder
	{
		/// <summary>
		/// The schema to put all the tables, stps and types in
		/// </summary>
		public string Schema { get; set; }

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="schema"></param>
		public DatabaseQueryBuilder(string schema)
		{
			Schema = schema;
		}

		/// <summary>
		/// Build a SQL query from a namespace that contains tables of <seealso cref="BaseReturnData"/>
		/// </summary>
		/// <param name="tableNamespace"></param>
		/// <returns></returns>
		public string Build(string tableNamespace)
		{
			var assm = Assembly.GetEntryAssembly();
			if (assm == null)
				throw new Exception("Could not get the entry assembly!");
			var tables = GetTypesInNamespace(assm, tableNamespace).ToList();
			return Build(tables);
		}

		/// <summary>
		/// Build a SQL query from a list of tables based on <seealso cref="BaseReturnData"/>
		/// </summary>
		/// <param name="tables"></param>
		/// <returns></returns>
		public string Build(List<Type> tables)
		{
			var sb = new StringBuilder();
			sb.AppendLine(BuildSchemaSQL());
			foreach (var table in tables)
			{
				sb.AppendLine(BuildDropSQL(table));
				sb.AppendLine(BuildTableSQL(table));
				sb.AppendLine(BuildTVPSQL(table));
				sb.AppendLine(BuildMergeSQL(table));
			}
			var query = sb.ToString();
			return query;
		}

		private string BuildSchemaSQL()
		{
			var sb = new StringBuilder();

			sb.AppendLine($"IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{Schema}')");
			sb.AppendLine($"\tEXEC('CREATE SCHEMA [{Schema}]')");
			sb.AppendLine($"GO");

			return sb.ToString();
		}

		private string BuildDropSQL(Type table)
		{
			var sb = new StringBuilder();

			sb.AppendLine($"DROP PROCEDURE IF EXISTS [{Schema}].[Merge{table.Name}]");
			sb.AppendLine("GO");
			sb.AppendLine($"IF type_id('[{Schema}].[{table.Name}_TVP]') IS NOT NULL");
			sb.AppendLine($"\tDROP TYPE [{Schema}].[{table.Name}_TVP];");
			sb.AppendLine("GO");

			return sb.ToString();
		}

		private string BuildTableSQL(Type table)
		{
			var sb = new StringBuilder();

			sb.AppendLine($"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{table.Name}' AND xtype='U')");
			sb.AppendLine($"\tCREATE TABLE [{Schema}].[{table.Name}] (");
			var props = table.GetProperties();
			var subSb = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.Name.ToUpper() == "OPCODE")
					continue;

				var isPrimaryKey = prop.GetCustomAttribute<ReplicationPrimaryKey>() != null;
				var isNotNull = false;
				if (prop.GetCustomAttribute<ReplicationNotNull>() != null || isPrimaryKey)
					isNotNull = true;

				subSb.Append($"\t\t[{prop.Name}] {TypeToDBType(prop.PropertyType, prop.GetCustomAttribute<ReplicationTypeOverride>())}");
				if (isNotNull)
					subSb.Append(" NOT NULL");
				else
					subSb.Append(" NULL");
				if (isPrimaryKey)
					subSb.Append($" CONSTRAINT [PK_{table.Name}] PRIMARY KEY ({prop.Name})");
				subSb.AppendLine(",");
			}
			sb.Append(RemoveTrailingComma(subSb.ToString()));

			sb.AppendLine("\t)");
			sb.AppendLine("GO");

			return sb.ToString();
		}

		private string BuildTVPSQL(Type table)
		{
			var sb = new StringBuilder();

			sb.AppendLine($"CREATE TYPE [{Schema}].[{table.Name}_TVP] AS TABLE (");
			var props = table.GetProperties();
			var subSb = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.GetCustomAttribute<ReplicationUpdateIgnore>() != null)
					continue;

				var isNotNull = prop.Name.ToUpper() == "OPCODE";
				if (!isNotNull && prop.GetCustomAttribute<ReplicationNotNull>() != null || prop.GetCustomAttribute<ReplicationPrimaryKey>() != null)
					isNotNull = true;

				subSb.Append($"\t[{prop.Name}] {TypeToDBType(prop.PropertyType, prop.GetCustomAttribute<ReplicationTypeOverride>())}");
				if (isNotNull)
					subSb.AppendLine(" NOT NULL,");
				else
					subSb.AppendLine(" NULL,");
			}
			sb.Append(RemoveTrailingComma(subSb.ToString()));

			sb.AppendLine(")");
			sb.AppendLine("GO");

			return sb.ToString();
		}

		private string BuildMergeSQL(Type table)
		{
			var sb = new StringBuilder();

			var props = table.GetProperties();
			var primary = props.FirstOrDefault(x => x.GetCustomAttribute<ReplicationPrimaryKey>() != null);
			if (primary == null)
				throw new Exception("Table definition has no primary key!");

			sb.AppendLine($"CREATE PROCEDURE [{Schema}].[Merge{table.Name}]");
			sb.AppendLine($"\t @Items [{Schema}].[{table.Name}_TVP] READONLY");
			sb.AppendLine($"AS");
			sb.AppendLine($"BEGIN");
			sb.AppendLine($"\tSET NOCOUNT ON;");
			sb.AppendLine($"\tSET XACT_ABORT ON;");
			sb.AppendLine($"\tBEGIN TRY");
			sb.AppendLine($"\t\tBEGIN TRANSACTION;");
			sb.AppendLine($"\t\tMERGE INTO [{Schema}].[{table.Name}] AS T");
			sb.AppendLine($"\t\tUSING @Items AS S ON T.{primary.Name} = S.{primary.Name}");
			sb.AppendLine($"\t\tWHEN MATCHED AND S.OpCode = 1 THEN DELETE");
			sb.AppendLine($"\t\tWHEN MATCHED AND S.OpCode IN (3,4) THEN UPDATE SET");

			var subSb = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.Name.ToUpper() == "OPCODE")
					continue;

				if (prop.GetCustomAttribute<ReplicationUpdateIgnore>() != null)
					continue;

				var setValue = $"S.{prop.Name}";
				var overrideValue = prop.GetCustomAttribute<ReplicationUpdateValueOverride>();
				if (overrideValue != null)
					setValue = overrideValue.Value;

				subSb.AppendLine($"\t\t\tT.{prop.Name} = {setValue},");
			}
			sb.Append(RemoveTrailingComma(subSb.ToString()));
			sb.AppendLine($"\t\tWHEN NOT MATCHED BY TARGET AND S.OpCode IN (2,3,4) THEN INSERT (");

			subSb = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.Name.ToUpper() == "OPCODE")
					continue;

				subSb.AppendLine($"\t\t\t{prop.Name},");
			}
			sb.Append(RemoveTrailingComma(subSb.ToString()));
			sb.AppendLine("\t\t) VALUES (");

			subSb = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.Name.ToUpper() == "OPCODE")
					continue;

				var setValue = $"S.{prop.Name}";
				var overrideValue = prop.GetCustomAttribute<ReplicationCreateValueOverride>();
				if (overrideValue != null)
					setValue = overrideValue.Value;
				subSb.AppendLine($"\t\t\t{setValue},");
			}
			sb.Append(RemoveTrailingComma(subSb.ToString()));
			sb.AppendLine("\t\t);");
			sb.AppendLine("\t\tCOMMIT TRANSACTION;");
			sb.AppendLine("\tEND TRY");
			sb.AppendLine("\tBEGIN CATCH");
			sb.AppendLine("\t\tIF @@TRANCOUNT > 0");
			sb.AppendLine("\t\t\tROLLBACK;");
			sb.AppendLine("\t\tTHROW;");
			sb.AppendLine("\tEND CATCH");
			sb.AppendLine("END");
			sb.AppendLine("GO");

			return sb.ToString();
		}

		private string TypeToDBType(Type type, ReplicationTypeOverride? overrideType)
		{
			if (overrideType != null)
				return overrideType.SQLDataType.ToUpper();

			var argUnderlying = Nullable.GetUnderlyingType(type);
			if (argUnderlying != null)
				return TypeToDBType(argUnderlying, null);

			switch (type.Name.ToUpper())
			{
				case "STRING":
					return "NVARCHAR(MAX)";
				case "GUID":
					return "UNIQUEIDENTIFIER";
				case "INT32":
					return "INT";
				case "DATETIME":
					return "DATETIME";
				case "BYTE[]":
					return "VARBINARY(MAX)";
				case "DOUBLE":
				case "FLOAT":
					return "DECIMAL(9,3)";
				case "BOOLEAN":
					return "BIT";
			}
			return "NVARCHAR(MAX)";
		}

		private string RemoveTrailingComma(string input)
		{
			return input.Remove(input.LastIndexOf(','), 1);
		}

		// https://stackoverflow.com/a/949285
		private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
		{
			return
				assembly.GetTypes()
						.Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
						.ToArray();
		}
	}
}
