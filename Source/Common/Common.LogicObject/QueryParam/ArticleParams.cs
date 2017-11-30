using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class ArticleParams
    {
        public Guid ArticleId;
        public Guid ParentId;
        public string ArticleAlias;
        public string BannerPicFileName;
        public int LayoutModeId;
        public int ShowTypeId;
        public string LinkUrl;
        public string LinkTarget;
        public string ControlName;
        public string SubItemControlName;
        public bool IsHideSelf;
        public bool IsHideChild;
        public DateTime StartDate;
        public DateTime EndDate;
        public int SortNo;
        public bool DontDelete;
        public string PostAccount;
        /// <summary>
        /// return:網頁代碼是否重覆
        /// </summary>
        public bool HasIdBeenUsed = false;
        /// <summary>
        /// return:網址別名是否重覆
        /// </summary>
        public bool HasAliasBeenUsed = false;
    }
}
