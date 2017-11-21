using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class OpListQueryParams
    {
        public int ParentId;	// 0:root
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;

        public OpListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
        }
    }
}
