using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class DeptListQueryParams
    {
        public string Kw = "";
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public DeptListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }
    }
}
