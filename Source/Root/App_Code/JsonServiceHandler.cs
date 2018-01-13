using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JsonService
{
    /// <summary>
    /// 取得附縮圖的網頁內容清單
    /// </summary>
    public class Article_GetListWithThumb : JsonServiceHandlerAbstract
    {
        protected OtherArticlePageCommon c;
        protected ArticlePublisherLogic artPub;
        protected ArticleData articleData;

        private DataPagerLogic dataPager = new DataPagerLogic();

        /// <summary>
        /// 取得附縮圖的網頁內容清單
        /// </summary>
        public Article_GetListWithThumb(HttpContext context)
            : base(context)
        {
            c = new OtherArticlePageCommon(context, null);
            c.InitialLoggerOfUI(this.GetType());

            artPub = new ArticlePublisherLogic();
        }

        public override ClientResult ProcessRequest()
        {
            int minMs = 500;    //最少讓client等?毫秒
            DateTime start = DateTime.Now;

            ClientResult cr = null;

            Guid articleId;
            string artId;
            int p;

            artId = GetParamValue("artid");
            articleId = new Guid(artId);
            p = Convert.ToInt32(GetParamValue("p"));

            if (p == 1)
            {
                minMs = 0;  //第一頁快一點
            }

            // get article data
            if (c.RetrieveArticleIdAndData(articleId))
            {
                articleData = c.GetArticleData();
            }
            else
            {
                cr = new ClientResult() { b = false, err = "no data" };

                System.Threading.Thread.Sleep(minMs);
                return cr;
            }

            // get sub-items
            ArticleValidListQueryParams param = new ArticleValidListQueryParams()
            {
                ParentId = articleData.ArticleId.Value,
                CultureName = c.qsCultureNameOfLangNo,
                Kw = ""
            };

            param.PagedParams = new PagedListQueryParams()
            {
                BeginNum = 0,
                EndNum = 0,
                SortField = articleData.SortFieldOfFrontStage,
                IsSortDesc = articleData.IsSortDescOfFrontStage
            };

            // get total of items
            artPub.GetArticleValidListForFrontend(param);

            // setting pager rule
            if (string.Compare(articleData.ControlName, "ListBlocks", true) == 0)
            {
                dataPager.MaxItemCountOfPage = 6;
                dataPager.MaxDisplayCountInPageCodeArea = 5;
            }
            else
            {
                dataPager.MaxItemCountOfPage = 10;
                dataPager.MaxDisplayCountInPageCodeArea = 5;
            }

            // update pager and get begin end of item numbers
            dataPager.ItemTotalCount = param.PagedParams.RowCount;
            dataPager.SetCurrentPageCodeAndRecalc(p);

            param.PagedParams = new PagedListQueryParams()
            {
                BeginNum = dataPager.BeginItemNumberOfPage,
                EndNum = dataPager.EndItemNumberOfPage,
                SortField = articleData.SortFieldOfFrontStage,
                IsSortDesc = articleData.IsSortDescOfFrontStage
            };

            DataSet dsSubitems = artPub.GetArticleValidListForFrontend(param);

            if (dsSubitems == null)
            {
                ArticlePagedInfo info = new ArticlePagedInfo() { pageCode = 0, pageTotal = 0 };
                cr = new ClientResult() { b = true, o = info };

                System.Threading.Thread.Sleep(minMs);
                return cr;
            }

            // get thumbs
            DataTable dtSubitems = dsSubitems.Tables[0];

            dtSubitems.Columns.Add("PicId", typeof(string));
            dtSubitems.Columns.Add("PicSubject", typeof(string));

            foreach (DataRow dr in dtSubitems.Rows)
            {
                Guid itemId = (Guid)dr["ArticleId"];

                DataSet dsArtPic = artPub.GetArticlePictureListForFrontend(itemId, c.qsCultureNameOfLangNo);
                string picId = "";
                string picSubject = "";

                if (dsArtPic != null && dsArtPic.Tables[0].Rows.Count > 0)
                {
                    DataRow drFirst = dsArtPic.Tables[0].Rows[0];

                    picId = drFirst.ToSafeStr("PicId");
                    picSubject = drFirst.ToSafeStr("PicSubject");
                }

                dr["PicId"] = picId;
                dr["PicSubject"] = picSubject;
            }

            ArticlePagedInfo pagedInfo = new ArticlePagedInfo()
            {
                pageCode = p,
                pageTotal = dataPager.PageTotalCount,
                itemTotal = dataPager.ItemTotalCount,
                itemList = dtSubitems
            };

            cr = new ClientResult()
            {
                b = true,
                o = pagedInfo
            };

            TimeSpan ts = DateTime.Now - start;

            if (ts.TotalMinutes < minMs)
            {
                System.Threading.Thread.Sleep(minMs - (int)ts.TotalMinutes);
            }

            return cr;
        }

        public class ArticlePagedInfo
        {
            public int pageCode { get; set; }
            public int pageTotal { get; set; }
            public int itemTotal = 0;
            public DataTable itemList { get; set; }
        }
    }

    /// <summary>
    /// 取得搜尋關鍵字
    /// </summary>
    public class Keyword_GetList : JsonServiceHandlerAbstract
    {
        protected FrontendPageCommon c;
        protected ArticlePublisherLogic artPub;

        /// <summary>
        /// 取得搜尋關鍵字
        /// </summary>
        public Keyword_GetList(HttpContext context)
            : base(context)
        {
            c = new FrontendPageCommon(context, null);
            c.InitialLoggerOfUI(this.GetType());

            artPub = new ArticlePublisherLogic();
        }

        public override ClientResult ProcessRequest()
        {
            ClientResult cr = null;
            string term = GetParamValue("term");
            int topCount = 5;

            DataSet dsKeywords = artPub.GetKeywordListForFrontend(c.qsCultureNameOfLangNo, term, topCount);

            if (dsKeywords != null)
            {
                List<string> kwList = new List<string>(topCount);

                foreach (DataRow dr in dsKeywords.Tables[0].Rows)
                {
                    kwList.Add(dr.ToSafeStr("Kw"));
                }

                cr = new ClientResult()
                {
                    b = true,
                    o = kwList
                };
            }
            else
            {
                cr = new ClientResult()
                {
                    b = false,
                    err = "exception error."
                };
            }

            return cr;
        }
    }
}