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

    /// <summary>
    /// angular-FileManager file info
    /// </summary>
    public class AfmFileInfo
    {
        public string name { get; set; }    // magento, index.php
        public string rights { get; set; }  // -rw-r--r--
        public string size { get; set; }    // 549923
        public string date { get; set; }    // 2016-03-03 15:31:40
        public string type { get; set; }    // dir, file
    }
}
