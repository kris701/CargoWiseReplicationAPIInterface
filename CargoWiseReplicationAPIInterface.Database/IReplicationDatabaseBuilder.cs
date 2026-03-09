using CargoWiseReplicationAPIInterface.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Database
{
	/// <summary>
	/// An interface that can be used to build a SQL query for a replication database
	/// </summary>
	public interface IReplicationDatabaseBuilder
	{
		/// <summary>
		/// The schema to put all the tables, stps and types in
		/// </summary>
		public string Schema { get; set; }
		/// <summary>
		/// Build a SQL query from a namespace that contains tables of <seealso cref="BaseReturnData"/>
		/// </summary>
		/// <param name="tableNamespace"></param>
		/// <returns></returns>
		public string Build(string tableNamespace);
		/// <summary>
		/// Build a SQL query from a list of tables based on <seealso cref="BaseReturnData"/>
		/// </summary>
		/// <param name="tables"></param>
		/// <returns></returns>
		public string Build(List<Type> tables);
	}
}
