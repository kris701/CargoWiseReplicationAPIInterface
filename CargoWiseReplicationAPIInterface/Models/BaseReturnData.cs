using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Models
{
	public class BaseReturnData
	{
		public OperationTypes Operation { get; set; } = OperationTypes.None;
	}
}
