using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class AttachFileListQueryParams
    {
        public Guid ArticleId;
        public string CultureName;
        public string Kw;
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public AttachFileListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }
    }
}
