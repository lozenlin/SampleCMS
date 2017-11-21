using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class OpParams
    {
        public int OpId;
        public int ParentId;
        public string OpSubject;
        public string LinkUrl;
        public bool IsNewWindow;
        public string IconImageFile;
        public int SortNo;
        public bool IsHideSelf;
        public string CommonClass;
        public string PostAccount;
        public string EnglishSubject;
        /// <summary>
        /// return:是否有子項目
        /// </summary>
        public bool IsThereSubitemOfOp = false;
    }
}
