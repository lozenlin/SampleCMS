using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LogicObject
{
    public class SearchResultListQueryParams
    {
        /// <summary>
        /// 多項關聯字用逗號串接; Multiple related words concatenated with commas(,), e.g., one,two,three
        /// </summary>
        public string Keywords;
        public string CultureName;
        public PagedListQueryParams PagedParams;

        public SearchResultListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
        }
    }
}
