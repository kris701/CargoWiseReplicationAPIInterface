using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Models
{
	public class RepData
	{
		public object? Value { get; set; }
		public bool IsSet { get; set; }

		public RepData(object? value)
		{
			Value = value;
			IsSet = true;
		}

		public RepData()
		{
			IsSet = false;
		}

		public override string ToString()
		{
			if (IsSet)
				return $"{Value}";
			return "Not set";
		}
	}

	public class RepData<T> : RepData
	{
		public RepData(T value) : base(value) { }
		public RepData() : base() { }
	}
}
