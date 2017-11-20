using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class BackEndLogListQueryParams
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string Account;  // 空字串:全部
        public bool IsAccKw;
        public string IP;   // 空字串:全部
        public bool IsIpHeadKw;
        public string DescKw;   // 空字串:全部
        public int RangeMode;   // 0:全部, 1:登入相關
        public PagedListQueryParams PagedParams;
        public AuthenticationQueryParams AuthParams;

        public BackEndLogListQueryParams()
        {
            PagedParams = new PagedListQueryParams();
            AuthParams = new AuthenticationQueryParams();
        }
    }
}
