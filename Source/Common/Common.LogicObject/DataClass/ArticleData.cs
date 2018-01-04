using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// Article data for front-stage
    /// </summary>
    public class ArticleData
    {
        public Guid? ArticleId;
        public Guid? ParentId;
        public int ArticleLevelNo;
        public string ArticleAlias;
        public string BannerPicFileName;
        public int LayoutModeId;
        public int ShowTypeId;
        public string LinkUrl;
        public string LinkTarget;
        public string ControlName;
        public bool IsHideSelf;
        public bool IsHideChild;
        public DateTime StartDate;
        public DateTime EndDate;
        public int SortNo;
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
        public string ArticleSubject;
        public string ArticleContext;
        public bool IsShowInLang;
        public string Subtitle;
        public string PublisherName;
        public string PostAccount;
        public DateTime PostDate;
        public string MdfAccount;
        public DateTime MdfDate;
        public Guid? Lv1Id;
        public Guid? Lv2Id;
        public Guid? Lv3Id;
        public bool IsPreviewMode = false;

        public void ImportDataFrom(DataRow drArticle)
        {
            if (!Convert.IsDBNull(drArticle["ParentId"]))
            {
                ParentId = (Guid)drArticle["ParentId"];
            }

            ArticleLevelNo = Convert.ToInt32(drArticle["ArticleLevelNo"]);
            ArticleAlias = drArticle.ToSafeStr("ArticleAlias");
            BannerPicFileName = drArticle.ToSafeStr("BannerPicFileName");
            LayoutModeId = Convert.ToInt32(drArticle["LayoutModeId"]);
            ShowTypeId = Convert.ToInt32(drArticle["ShowTypeId"]);
            LinkUrl = drArticle.ToSafeStr("LinkUrl");
            LinkTarget = drArticle.ToSafeStr("LinkTarget");
            ControlName = drArticle.ToSafeStr("ControlName");
            IsHideSelf = Convert.ToBoolean(drArticle["IsHideSelf"]);
            IsHideChild = Convert.ToBoolean(drArticle["IsHideChild"]);
            StartDate = Convert.ToDateTime(drArticle["StartDate"]);
            EndDate = Convert.ToDateTime(drArticle["EndDate"]);
            SortNo = Convert.ToInt32(drArticle["SortNo"]);
            SubjectAtBannerArea = Convert.ToBoolean(drArticle["SubjectAtBannerArea"]);
            PublishDate = Convert.ToDateTime(drArticle["PublishDate"]);
            IsShowInUnitArea = Convert.ToBoolean(drArticle["IsShowInUnitArea"]);
            IsShowInSitemap = Convert.ToBoolean(drArticle["IsShowInSitemap"]);
            SortFieldOfFrontStage = drArticle.ToSafeStr("SortFieldOfFrontStage");
            IsSortDescOfFrontStage = Convert.ToBoolean(drArticle["IsSortDescOfFrontStage"]);
            IsListAreaShowInFrontStage = Convert.ToBoolean(drArticle["IsListAreaShowInFrontStage"]);
            IsAttAreaShowInFrontStage = Convert.ToBoolean(drArticle["IsAttAreaShowInFrontStage"]);
            IsPicAreaShowInFrontStage = Convert.ToBoolean(drArticle["IsPicAreaShowInFrontStage"]);
            IsVideoAreaShowInFrontStage = Convert.ToBoolean(drArticle["IsVideoAreaShowInFrontStage"]);
            ArticleSubject = drArticle.ToSafeStr("ArticleSubject");
            ArticleContext = drArticle.ToSafeStr("ArticleContext");
            IsShowInLang = Convert.ToBoolean(drArticle["IsShowInLang"]);
            Subtitle = drArticle.ToSafeStr("Subtitle");
            PublisherName = drArticle.ToSafeStr("PublisherName");
            PostAccount = drArticle.ToSafeStr("PostAccount");
            PostDate = Convert.ToDateTime(drArticle["PostDate"]);

            if (Convert.IsDBNull(drArticle["MdfDate"]))
            {
                MdfAccount = PostAccount;
                MdfDate = PostDate;
            }
            else
            {
                MdfAccount = drArticle.ToSafeStr("MdfAccount");
                MdfDate = Convert.ToDateTime(drArticle["MdfDate"]);
            }
        }
    }
}
