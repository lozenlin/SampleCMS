using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// angular-FileManager result
    /// </summary>
    public class AfmResult
    {
        public object result { get; set; }
    }

    /// <summary>
    /// angular-FileManager result of result
    /// </summary>
    public class AfmResultOfResult
    {
        public bool success { get; set; }
        public string error { get; set; }
    }
}
