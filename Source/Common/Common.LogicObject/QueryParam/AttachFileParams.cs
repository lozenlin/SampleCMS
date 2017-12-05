using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class AttachFileParams
    {
        public Guid AttId;
        public Guid ArticleId;
        public string FilePath;
        public string FileSavedName;
        public int FileSize;
        public int SortNo;
        public string FileMIME;
        public bool DontDelete;
        public string PostAccount;
    }
}
