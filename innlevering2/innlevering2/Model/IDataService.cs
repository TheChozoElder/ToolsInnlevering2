﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace innlevering2.Model
{
	public interface IDataService
	{
		void GetData(Action<DataItem, Exception> callback);
	}
}
