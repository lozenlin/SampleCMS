using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// Article data for front-stage
    /// </summary>
    public class ArticleData
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
        public bool SubjectAtBannerArea;
        public DateTime PublishDate;
        public bool IsShowInUnitArea;
        public bool IsShowInSitemap;
        public string SortFieldOfFrontStage;
        public bool IsSortDescOfFrontStage;
        public bool IsListAreaShowInFrontStage;
        public bool IsAttAreaShowInFrontStage;
        public bool IsPicAreaShowInFrontStage;
        public bool IsVideoAreaShowInFrontStage;
        public string SubItemLinkUrl;
        public string ArticleSubject;
        public string ArticleContext;
        public bool IsShowInLang;
        public string Subtitle;
        public string PublisherName;
        public string PostAccount;
        public DateTime PostDate;
        public string MdfAccount;
        public DateTime MdfDate;
        public Guid Lv1Id;
        public Guid Lv2Id;
        public Guid Lv3Id;
    }
}
