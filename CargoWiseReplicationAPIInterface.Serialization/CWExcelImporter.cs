using CargoWiseReplicationAPIInterface.Database.Services;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Collections;

namespace CargoWiseReplicationAPIInterface.Serialization
{
	/// <summary>
	/// Small helper method to import Excel data into a replication database
	/// </summary>
	public class CWExcelImporter
	{
		/// <summary>
		/// The merger service to use
		/// </summary>
		public DatabaseMergerService MergerService { get; set; }
		/// <summary>
		/// A logger to output progress
		/// </summary>
		public ILogger Logger { get; set; }
		/// <summary>
		/// What namespace to find models in
		/// </summary>
		public string ModelNamespace { get; set; }

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="mergerService"></param>
		/// <param name="logger"></param>
		/// <param name="modelNamespace"></param>
		public CWExcelImporter(DatabaseMergerService mergerService, ILogger logger, string modelNamespace)
		{
			MergerService = mergerService;
			Logger = logger;
			ModelNamespace = modelNamespace;
		}

		/// <summary>
		/// Import data from an excel data stream
		/// </summary>
		/// <param name="str"></param>
		/// <param name="forceupdate"></param>
		/// <returns></returns>
		public async Task ImportExcel(Stream str, bool forceupdate)
		{
			Logger.LogInformation($"Loading Excel file...");

			ExcelPackage.License.SetNonCommercialPersonal("kristian");
			var source = new CancellationTokenSource();

			int total = 0;
			using (ExcelPackage excelPackage = new ExcelPackage())
			{
				excelPackage.Load(str);
				foreach (var sheet in excelPackage.Workbook.Worksheets)
				{
					if (sheet.Hidden == eWorkSheetHidden.Hidden || sheet.Hidden == eWorkSheetHidden.VeryHidden)
						continue;
					Logger.LogInformation($"Parsing worksheet '{sheet.Name}'");
					var targetTypeName = ModelNamespace + sheet.Name;
					var targetType = ByName(targetTypeName);

					var data = GetList(targetType, sheet);

					foreach (var item in data)
					{
						if (forceupdate)
							((dynamic)item).OpCode = 3;
						else
							((dynamic)item).OpCode = 2;
					}
					await MergerService.Merge((dynamic)data, Logger, source.Token);
					total += data.Count;
				}
			}

			Logger.LogInformation($"Import completed! {total} rows imported in total");
		}

		private IList GetList(Type targetType, ExcelWorksheet sheet)
		{
			var list = CreateList(targetType);
			//first row is for knowing the properties of object
			var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns).ToList().Select(n =>

				new { Index = n, ColumnName = sheet.Cells[1, n].Value?.ToString() }
			).ToList();
			columnInfo.RemoveAll(x => x.ColumnName == null);
			var columnInfoDict = columnInfo.ToDictionary(x => x.ColumnName!, x => x.Index);

			var targetProps = targetType.GetProperties();
			for (int row = 2; row < sheet.Dimension.Rows; row++)
			{
				var obj = Activator.CreateInstance(targetType);//generic object
				foreach (var prop in targetProps)
				{
					if (!columnInfoDict.ContainsKey(prop.Name))
						continue;
					var val = sheet.Cells[row, columnInfoDict[prop.Name]].Value;
					var propType = prop.PropertyType;
					var underlying = Nullable.GetUnderlyingType(propType);
					if (underlying == null)
						prop.SetValue(obj, ObjectToType(propType, val));
					else
						prop.SetValue(obj, ObjectToType(underlying, val));
				}
				list.Add(obj);
			}

			return list;
		}

		private dynamic? ObjectToType(Type asType, object getObj)
		{
			if (getObj == null)
				return null;
			if (asType == typeof(bool))
			{
				if (getObj.ToString() == "Y")
					return true;
				if (getObj.ToString() == "N")
					return false;
			}
			else if (asType == typeof(DateTime))
				return DateTime.FromOADate(double.Parse(getObj.ToString()));
			else if (asType == typeof(TimeSpan))
				return TimeSpan.Parse(getObj.ToString());
			else if (asType == typeof(Guid))
				return Guid.Parse(getObj.ToString());
			else if (asType == typeof(double))
				return double.Parse(getObj.ToString());
			else if (asType == typeof(byte[]))
				return Convert.ChangeType(getObj, asType, System.Globalization.CultureInfo.InvariantCulture);

			return Convert.ChangeType(getObj.ToString(), asType, System.Globalization.CultureInfo.InvariantCulture);
		}

		// https://stackoverflow.com/a/20008954
		private Type ByName(string name)
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
			{
				var tt = assembly.GetType(name);
				if (tt != null)
				{
					return tt;
				}
			}

			return null;
		}

		private IList CreateList(Type myType)
		{
			Type genericListType = typeof(List<>).MakeGenericType(myType);
			return (IList)Activator.CreateInstance(genericListType);
		}
	}
}
