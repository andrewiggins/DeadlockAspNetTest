using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadlockAspNetTest.Models
{
    public class ViewModel
    {
        public ViewModel(string dataResult)
        {
            this.DataResult = dataResult;
        }

        public string DataResult { get; private set; }
    }
}