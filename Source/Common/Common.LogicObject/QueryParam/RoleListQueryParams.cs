using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class RoleListQueryParams
    {
        public string Kw = "";
        public PagedListQueryParams PagedParams;

        public RoleListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
        }
    }
}
