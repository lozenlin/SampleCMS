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
        public AuthenticationQueryParams AuthParams;

        public RoleListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }
    }
}
