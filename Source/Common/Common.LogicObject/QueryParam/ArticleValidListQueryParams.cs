using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class ArticleValidListQueryParams
    {
        public Guid ParentId;
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;

        public ArticleValidListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
        }
    }
}
