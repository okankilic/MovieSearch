﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApi.Intefaces
{
    public interface IJobFactory
    {
        IJob CreateNew(string jobName);
    }
}
