using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// angular-FileManager service handler
    /// </summary>
    public interface IAfmServiceHandler
    {
        AfmResult ProcessRequest();
    }
}
