using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;

namespace Common.LogicObject
{
    /// <summary>
    /// 檔案下載功能的共用元件
    /// </summary>
    public abstract class FileDownloadCommon : PageCommon
    {
        public FileDownloadCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
        }

        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        public int qsWidth
        {
            get
            {
                string str = QueryStringToSafeStr("w");
                int nResult;

                if (str != null && int.TryParse(str, out nResult))
                {
                    int maxWidth = 1920;

                    if (nResult > maxWidth)
                        nResult = maxWidth;
                }
                else
                {
                    return -1;
                }

                return nResult;
            }
        }

        public int qsHeight
        {
            get
            {
                string str = QueryStringToSafeStr("h");
                int nResult;

                if (str != null && int.TryParse(str, out nResult))
                {
                    int maxHeight = 1440;
                    if (nResult > maxHeight)
                        nResult = maxHeight;
                }
                else
                {
                    return -1;
                }

                return nResult;
            }
        }

        /// <summary>
        /// 另存新檔
        /// </summary>
        public int qsSaveAs
        {
            get
            {
                string str = QueryStringToSafeStr("saveas");
                int nResult;

                if (str != null && int.TryParse(str, out nResult))
                {
                    if (nResult > 1)
                    {
                        nResult = 1;
                    }
                    else if (nResult < 0)
                    {
                        nResult = 0;
                    }
                }
                else
                {
                    return -1;
                }

                return nResult;
            }
        }

        /// <summary>
        /// 放大圖片
        /// </summary>
        public int qsPicStretchToSize
        {
            get
            {
                string str = QueryStringToSafeStr("stretch");
                int nResult;

                if (str != null && int.TryParse(str, out nResult))
                {
                    if (nResult > 1)
                    {
                        nResult = 1;
                    }
                    else if (nResult < 0)
                    {
                        nResult = 0;
                    }
                }
                else
                {
                    return -1;
                }

                return nResult;
            }
        }

        /// <summary>
        /// 圖片可以不固定長寬比例
        /// </summary>
        public int qsPicFlexable
        {
            get
            {
                string str = QueryStringToSafeStr("flexable");
                int nResult;

                if (str != null && int.TryParse(str, out nResult))
                {
                    if (nResult > 1)
                    {
                        nResult = 1;
                    }
                    else if (nResult < 0)
                    {
                        nResult = 0;
                    }
                }
                else
                {
                    return -1;
                }

                return nResult;
            }
        }

        #endregion

        #region 設定用屬性

        /// <summary>
        /// 檔案要另存
        /// </summary>
        public bool IsSaveAs
        {
            get { return isSaveAs; }
            set { isSaveAs = value; }
        }
        protected bool isSaveAs = true;

        /// <summary>
        /// 啟用圖片可變更至指定大小
        /// </summary>
        public bool EnabledPicResize
        {
            get { return enabledPicResize; }
            set { enabledPicResize = value; }
        }
        protected bool enabledPicResize = false;

        /// <summary>
        /// 圖片可以放大到指定大小
        /// </summary>
        public bool IsPicStretchToSize
        {
            get { return isPicStretchToSize; }
            set { isPicStretchToSize = value; }
        }
        protected bool isPicStretchToSize = false;

        /// <summary>
        /// 圖片不固定長寬比例
        /// </summary>
        public bool IsPicFlexable
        {
            get { return isPicFlexable; }
            set { isPicFlexable = value; }
        }
        protected bool isPicFlexable = false;

        /// <summary>
        /// 指定的圖片大小
        /// </summary>
        public Size PicFitSize
        {
            get { return picFitSize; }
            set { picFitSize = value; }
        }
        protected Size picFitSize;

        /// <summary>
        /// 在後台管理系統中(不使用圖片快取)
        /// </summary>
        public bool IsInBackend
        {
            get { return isInBackend; }
            set { isInBackend = value; }
        }
        protected bool isInBackend = false;

        #endregion

        /// <summary>
        /// 處理網頁要求
        /// </summary>
        public bool ProcessRequest()
        {
            //取得傳入代碼
            if (qsAttId == Guid.Empty)
            {
                return false;
            }

            //用代碼取得完整檔名
            string fileFullName = GetFileFullName(qsAttId);
            bool result = !string.IsNullOrEmpty(fileFullName);

            if (result)
            {
                FileInfo fi = new FileInfo(fileFullName);

                if (fi.Exists)
                {
                    //輸出檔案給用戶端
                    result = Export(fi.Name, fi.FullName);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 用代碼取得完整檔名(子類別需實作)
        /// </summary>
        protected abstract string GetFileFullName(Guid attId);


        /// <summary>
        /// 輸出檔案給用戶端
        /// </summary>
        private bool Export(string fileName, string fileFullName)
        {
            string fileExt = Path.GetExtension(fileName);
            string contentType = "application/octet-stream";
            ImageFormat contentImageFormat = ImageFormat.Jpeg;

            switch (fileExt.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    contentImageFormat = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    contentType = "image/bmp";
                    contentImageFormat = ImageFormat.Bmp;
                    break;
                case ".gif":
                    contentType = "image/gif";
                    contentImageFormat = ImageFormat.Gif;
                    break;
                case ".png":
                    contentType = "image/png";
                    contentImageFormat = ImageFormat.Png;
                    break;
                case ".tiff":
                case ".tif":
                    contentType = "image/tiff";
                    contentImageFormat = ImageFormat.Tiff;
                    break;
                case ".pdf":
                    contentType = "application/pdf";
                    break;
            }

            byte[] bytes;

            if (contentType.StartsWith("image") && enabledPicResize)
            {
                //檢查快取
                string cacheKey = string.Format("{0}:id{1}:w{2}:s{3}:f{4}",
                    Request.AppRelativeCurrentExecutionFilePath, qsAttId, picFitSize.Width,
                    isPicStretchToSize ? "1" : "0", isPicFlexable ? "1" : "0");

                //只對認識的圖片大小做快取
                //在後台管理系統中不使用快取
                bool useCacheFunction = false;
                List<int> cachedWidthList = new List<int>(new int[] { 120, 640, 800, 1024, 1280, 1920 });
                useCacheFunction = (!isInBackend && cachedWidthList.Contains(picFitSize.Width));

                object cachedImageBytes = null;

                if (useCacheFunction)
                {
                    cachedImageBytes = Cache.Get(cacheKey);
                }

                if (cachedImageBytes == null)
                {
                    //指定下載的圖片要變更大小
                    System.Drawing.Image img = System.Drawing.Image.FromFile(fileFullName);
                    MemoryStream ms = new MemoryStream();
                    Size destSize = picFitSize;
                    bool needsToShrinkOrExpand = false;

                    if (img.Width > picFitSize.Width || img.Height > picFitSize.Height)
                    {
                        //要縮圖

                        //等比例縮圖
                        if (!isPicFlexable)
                        {
                            float widthRate = img.Width / (float)picFitSize.Width;
                            float heightRate = img.Height / (float)picFitSize.Height;

                            if (widthRate > heightRate)
                            {
                                destSize.Width = Convert.ToInt32(img.Width / widthRate);
                                destSize.Height = Convert.ToInt32(img.Height / widthRate);
                            }
                            else
                            {
                                destSize.Width = Convert.ToInt32(img.Width / heightRate);
                                destSize.Height = Convert.ToInt32(img.Height / heightRate);
                            }
                        }

                        needsToShrinkOrExpand = true;
                    }
                    else if (isPicStretchToSize && (img.Width < picFitSize.Width || img.Height < picFitSize.Height))
                    {
                        //要放大

                        //等比例放大
                        if (!isPicFlexable)
                        {
                            float widthRate = picFitSize.Width / (float)img.Width;
                            float heightRate = picFitSize.Height / (float)img.Height;

                            if (widthRate < heightRate)
                            {
                                destSize.Width = Convert.ToInt32(img.Width * widthRate);
                                destSize.Height = Convert.ToInt32(img.Height * widthRate);
                            }
                            else
                            {
                                destSize.Width = Convert.ToInt32(img.Width * heightRate);
                                destSize.Height = Convert.ToInt32(img.Height * heightRate);
                            }
                        }

                        needsToShrinkOrExpand = true;
                    }
                    else
                    {
                        //不用縮圖或放大
                        img.Save(ms, contentImageFormat);
                    }

                    if (needsToShrinkOrExpand)
                    {
                        System.Drawing.Image imgFit;

                        if (destSize.Width * destSize.Height <= 120 * 120)
                        {
                            //低品質
                            imgFit = img.GetThumbnailImage(destSize.Width, destSize.Height,
                                new System.Drawing.Image.GetThumbnailImageAbort(() => false), IntPtr.Zero);
                        }
                        else
                        {
                            //高品質
                            imgFit = new Bitmap(img, new Size(destSize.Width, destSize.Height));
                        }

                        //還原旋轉方向
                        // reference: https://forums.asp.net/t/2016582.aspx?Resize+script+is+rotating+image+sometimes+
                        if (new List<int>(img.PropertyIdList).Contains(0x112))
                        {
                            PropertyItem prop = img.GetPropertyItem(0x112);

                            if (prop.Type == 3 && prop.Len == 2)
                            {
                                UInt16 orientationExif = BitConverter.ToUInt16(prop.Value, 0);

                                switch (orientationExif)
                                {
                                    case 8:
                                        imgFit.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                        break;
                                    case 3:
                                        imgFit.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                        break;
                                    case 6:
                                        imgFit.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                        break;
                                }
                            }
                        }

                        imgFit.Save(ms, contentImageFormat);
                        imgFit.Dispose();
                    }

                    img.Dispose();

                    //從頭開始讀
                    ms.Position = 0;
                    bytes = new byte[ms.Length];
                    ms.Read(bytes, 0, bytes.Length);
                    ms.Close();

                    //加入快取
                    if (useCacheFunction)
                    {
                        Cache.Insert(cacheKey, bytes, null,
                            DateTime.Now.AddMinutes(1), Cache.NoSlidingExpiration, CacheItemPriority.Normal,
                            null);
                    }
                }
                else
                {
                    bytes = (byte[])cachedImageBytes;
                }
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                }
                catch (Exception ex)
                {
                    logger.Error("", ex);

                    return false;
                }
            }

            Response.ContentType = contentType;
            Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");

            //設定存檔的檔名
            if (IsSaveAs)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            }
            else
            {
                Response.AddHeader("Content-Disposition", "filename=" + fileName);
            }

            if (bytes != null && bytes.Length > 0)
            {
                Response.BinaryWrite(bytes);
            }

            //Response.End();

            return true;
        }
    }

    /// <summary>
    /// 附件檔案下載功能的共用元件
    /// </summary>
    public class AttDownloadCommon : FileDownloadCommon
    {
        public AttDownloadCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
            if (qsSaveAs != -1)
            {
                isSaveAs = (qsSaveAs == 1);
            }
        }

        protected override string GetFileFullName(Guid attId)
        {
            string fileFullName = "";
            string attRootDir = Server.MapPath(string.Format("~/{0}", ConfigurationManager.AppSettings["AttRootDir"]));
            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            DataSet dsAttachFile = artPub.GetAttachFileDataForBackend(attId);

            if (dsAttachFile == null && dsAttachFile.Tables[0].Rows.Count == 0)
            {
                logger.ErrorFormat("can't get data of attId[{0}]", attId);
                return "";
            }

            DataRow drAtt = dsAttachFile.Tables[0].Rows[0];
            string filePath = drAtt.ToSafeStr("FilePath");
            string fileSavedName = drAtt.ToSafeStr("fileSavedName");

            // update readCount
            if (!isInBackend)
            {
                artPub.IncreaseAttachFileMultiLangReadCount(attId, qsCultureNameOfLangNo);
            }

            fileFullName = string.Format("{0}{1}/{2}", attRootDir, filePath, fileSavedName);

            return fileFullName;
        }
    }

    /// <summary>
    /// 附件檔案(以直接檢視的方式)下載功能的共用元件
    /// </summary>
    public class AttViewDownloadCommon : AttDownloadCommon
    {
        public AttViewDownloadCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
            isSaveAs = false;
            enabledPicResize = true;
            picFitSize.Width = 1920;
            picFitSize.Height = 1080;

            if (qsWidth > 0)
            {
                picFitSize.Width = qsWidth;
            }

            if (qsHeight > 0)
            {
                picFitSize.Height = qsHeight;
            }

            if (qsPicStretchToSize != -1)
            {
                isPicStretchToSize = (qsPicStretchToSize == 1);
            }

            if (qsPicFlexable != -1)
            {
                isPicFlexable = (qsPicFlexable == 1);
            }
        }

    }

    /// <summary>
    /// 網頁照片下載功能的共用元件
    /// </summary>
    public class ArtPicDownloadCommon : FileDownloadCommon
    {
        public ArtPicDownloadCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
            isSaveAs = false;
            enabledPicResize = true;
            picFitSize.Width = 1920;
            picFitSize.Height = 1080;

            if (qsSaveAs != -1)
            {
                isSaveAs = (qsSaveAs == 1);
            }

            if (qsWidth > 0)
            {
                picFitSize.Width = qsWidth;
            }

            if (qsHeight > 0)
            {
                picFitSize.Height = qsHeight;
            }

            if (qsPicStretchToSize != -1)
            {
                isPicStretchToSize = (qsPicStretchToSize == 1);
            }

            if (qsPicFlexable != -1)
            {
                isPicFlexable = (qsPicFlexable == 1);
            }
        }

        protected override string GetFileFullName(Guid attId)
        {
            string fileFullName = "";
            string attRootDir = Server.MapPath(string.Format("~/{0}", ConfigurationManager.AppSettings["AttRootDir"]));
            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            DataSet dsPic = artPub.GetArticlePictureDataForBackend(attId);

            if (dsPic == null && dsPic.Tables[0].Rows.Count == 0)
            {
                logger.ErrorFormat("can't get data of attId[{0}]", attId);
                return "";
            }

            DataRow drPic = dsPic.Tables[0].Rows[0];
            string filePath = "ArticlePictures";
            string articleId = drPic.ToSafeStr("ArticleId");
            string fileSavedName = drPic.ToSafeStr("fileSavedName");

            fileFullName = string.Format("{0}{1}/{2}/{3}", attRootDir, filePath, articleId, fileSavedName);

            return fileFullName;
        }
    }
}
