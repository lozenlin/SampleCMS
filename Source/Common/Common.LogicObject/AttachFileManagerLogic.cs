// ===============================================================================
// AttachFileManagerLogic of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// AttachFileManagerLogic.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using log4net;
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
using System.Web.UI.WebControls;

namespace Common.LogicObject
{
    /// <summary>
    /// 附件檔案管理
    /// </summary>
    public class AttachFileManagerLogic
    {
        #region Public properties

        public Guid AttId
        {
            get { return attId; }
            set { attId = value; }
        }
        protected Guid attId;

        public Guid? ContextId
        {
            get { return contextId; }
            set { contextId = value; }
        }
        protected Guid? contextId;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        protected string filePath;

        public string FileSavedName
        {
            get { return fileSavedName; }
            set { fileSavedName = value; }
        }
        protected string fileSavedName;

        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
        protected int fileSize;

        public int SortNo
        {
            get { return sortNo; }
            set { sortNo = value; }
        }
        protected int sortNo;

        public string FileMIME
        {
            get { return fileMIME; }
            set { fileMIME = value; }
        }
        protected string fileMIME;

        public bool DontDelete
        {
            get { return dontDelete; }
            set { dontDelete = value; }
        }
        protected bool dontDelete = false;

        public string AttSubjectZhTw
        {
            get { return attSubjectZhTw; }
            set { attSubjectZhTw = value; }
        }
        protected string attSubjectZhTw;

        public string AttSubjectEn
        {
            get { return attSubjectEn; }
            set { attSubjectEn = value; }
        }
        protected string attSubjectEn;

        public bool IsShowInLangZhTw
        {
            get { return isShowInLangZhTw; }
            set { isShowInLangZhTw = value; }
        }
        protected bool isShowInLangZhTw = false;

        public bool IsShowInLangEn
        {
            get { return isShowInLangEn; }
            set { isShowInLangEn = value; }
        }
        protected bool isShowInLangEn = false;

        #endregion

        #region Read-only properties

        public int ReadCountZhTw
        {
            get { return readCountZhTw; }
        }
        protected int readCountZhTw;

        public int ReadCountEn
        {
            get { return readCountEn; }
        }
        protected int readCountEn;

        /// <summary>
        /// 副檔名限制名單
        /// </summary>
        public List<string> FileExtLimitations
        {
            get { return fileExtLimitations; }
        }
        protected List<string> fileExtLimitations = null;

        /// <summary>
        /// MIME限制名單
        /// </summary>
        public List<string> FileMimeLimitations
        {
            get { return fileMimeLimitations; }
        }
        protected List<string> fileMimeLimitations = null;

        public string FileFullName
        {
            get { return fileFullName; }
        }
        protected string fileFullName;

        public string PostAccount
        {
            get { return postAccount; }
        }
        protected string postAccount;

        public DateTime? PostDate
        {
            get { return postDate; }
        }
        protected DateTime? postDate;

        public string MdfAccount
        {
            get { return mdfAccount; }
        }
        protected string mdfAccount;

        public DateTime? MdfDate
        {
            get { return mdfDate; }
        }
        protected DateTime? mdfDate;

        #endregion

        protected HttpContext context;
        protected ILog logger = null;
        protected AttFileErrState errState = AttFileErrState.None;

        /// <summary>
        /// 附件檔案管理
        /// </summary>
        public AttachFileManagerLogic(HttpContext context)
        {
            this.context = context;
            logger = LogManager.GetLogger(this.GetType());
        }

        #region 工具屬性

        protected HttpServerUtility Server
        {
            get { return context.Server; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 初始化資料
        /// </summary>
        public virtual bool Initialize(Guid attId, Guid contextId)
        {
            //設定代碼
            this.attId = attId;
            this.contextId = contextId;

            bool result = LoadData();

            if (result)
            {
                InitialDefaultValue(contextId);
            }

            return result;
        }

        public AttFileErrState GetErrState()
        {
            return errState;
        }

        /// <summary>
        /// 依照文章代碼初使化預設值
        /// </summary>
        public virtual void InitialDefaultValue(Guid? specificId)
        {
            fileExtLimitations = new List<string>();
            fileMimeLimitations = new List<string>();

            //doc
            fileExtLimitations.Add("doc");
            fileMimeLimitations.Add("application/msword");
            fileExtLimitations.Add("docx");
            fileMimeLimitations.Add("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            fileExtLimitations.Add("xls");
            fileMimeLimitations.Add("application/vnd.ms-excel");
            fileExtLimitations.Add("xlsx");
            fileMimeLimitations.Add("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            fileExtLimitations.Add("ppt");
            fileExtLimitations.Add("pps");
            fileMimeLimitations.Add("application/vnd.ms-powerpoint");
            fileExtLimitations.Add("pptx");
            fileMimeLimitations.Add("application/vnd.openxmlformats-officedocument.presentationml.presentation");
            fileExtLimitations.Add("ppsx");
            fileMimeLimitations.Add("application/vnd.openxmlformats-officedocument.presentationml.slideshow");

            fileExtLimitations.Add("odt");
            fileMimeLimitations.Add("application/vnd.oasis.opendocument.text");

            fileExtLimitations.Add("ods");
            fileMimeLimitations.Add("application/vnd.oasis.opendocument.spreadsheet");

            fileExtLimitations.Add("odp");
            fileMimeLimitations.Add("application/vnd.oasis.opendocument.presentation");

            fileExtLimitations.Add("pdf");
            fileMimeLimitations.Add("application/pdf");

            fileExtLimitations.Add("txt");
            fileMimeLimitations.Add("text/plain");

            fileExtLimitations.Add("csv");
            fileMimeLimitations.Add("text/csv");

            //graphic
            fileExtLimitations.Add("jpg");
            fileMimeLimitations.Add("image/jpeg");

            fileExtLimitations.Add("gif");
            fileMimeLimitations.Add("image/gif");

            fileExtLimitations.Add("png");
            fileMimeLimitations.Add("image/png");

            fileExtLimitations.Add("bmp");
            fileMimeLimitations.Add("image/bmp");

            //compression
            fileExtLimitations.Add("zip");
            fileMimeLimitations.Add("application/x-zip-compressed");

            fileExtLimitations.Add("rar");
            fileMimeLimitations.Add("application/x-rar-compressed");

            //audio
            fileExtLimitations.Add("wav");
            fileMimeLimitations.Add("audio/wav");

            fileExtLimitations.Add("mp3");
            fileMimeLimitations.Add("audio/mpeg");

            fileExtLimitations.Add("wma");
            fileMimeLimitations.Add("audio/x-ms-wma");

            //video: avi, mov, mp4, wmv
            fileExtLimitations.Add("avi");
            fileMimeLimitations.Add("video/avi");

            fileExtLimitations.Add("mov");
            fileMimeLimitations.Add("video/quicktime");

            fileExtLimitations.Add("mp4");
            fileMimeLimitations.Add("video/mp4");

            fileExtLimitations.Add("wmv");
            fileMimeLimitations.Add("video/x-ms-wmv");

        }

        public bool SaveData(FileUpload fu, string mdfAccount)
        {
            return SaveData(fu.PostedFile, mdfAccount);
        }

        public virtual bool SaveData(HttpPostedFile postedFile, string mdfAccount)
        {
            bool hasFile = (postedFile != null && postedFile.ContentLength > 0);

            filePath = GetDirectoryName();
            string pathDirFullName = GetAttRootDirectoryFullName() + filePath;
            DirectoryInfo diPathDir = new DirectoryInfo(pathDirFullName);

            if (!diPathDir.Exists)
            {
                diPathDir.Create();
            }

            if (attId != Guid.Empty)
            {
                // update
                if (hasFile)
                {
                    // delete old file
                    if (!DeletePhysicalFileData())
                    {
                        return false;
                    }

                    if (!CheckExtAndExtractFileInfo(postedFile))
                    {
                        return false;
                    }

                    // save to disk
                    //儲存實體檔案(視條件縮圖)
                    if (!SavePhysicalFileDataAndShrinkImage(postedFile))
                    {
                        return false;
                    }
                }

                // save to db
                if (!UpdateData(mdfAccount))
                {
                    return false;
                }
            }
            else if (contextId.HasValue)
            {
                // add
                if (!hasFile)
                {
                    errState = AttFileErrState.AttachFileIsRequired;
                    return false;
                }

                if (!CheckExtAndExtractFileInfo(postedFile))
                {
                    return false;
                }

                // save to disk
                //儲存實體檔案(視條件縮圖)
                if (!SavePhysicalFileDataAndShrinkImage(postedFile))
                {
                    return false;
                }

                // save to db
                if (!InsertData(mdfAccount))
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool DeleteData()
        {
            if (attId == Guid.Empty)
            {
                errState = AttFileErrState.AttIdIsRequired;
                return false;
            }

            if (fileFullName == "")
            {
                errState = AttFileErrState.NoInitialize;
                return false;
            }

            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            bool result = artPub.DeleteAttachFileData(attId);

            if (result)
            {
                if (!DeletePhysicalFileData())
                {
                    logger.InfoFormat("can't delete physical file [{0}]", fileFullName);
                }
            }
            else
            {
                errState = AttFileErrState.DeleteDataFailed;
            }

            return result;
        }

        #endregion

        protected virtual bool LoadData()
        {
            bool result = false;

            if (attId != Guid.Empty)
            {
                ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
                DataSet dsAtt = artPub.GetAttachFileDataForBackend(attId);

                if (dsAtt == null || dsAtt.Tables[0].Rows.Count == 0)
                {
                    errState = AttFileErrState.LoadDataFailed;
                    return false;
                }

                DataRow drFirst = dsAtt.Tables[0].Rows[0];

                contextId = (Guid)drFirst["ArticleId"];
                filePath = drFirst.ToSafeStr("FilePath");
                fileSavedName = drFirst.ToSafeStr("FileSavedName");
                fileSize = Convert.ToInt32(drFirst["FileSize"]);
                sortNo = Convert.ToInt32(drFirst["SortNo"]);
                fileMIME = drFirst.ToSafeStr("FileMIME");
                dontDelete = Convert.ToBoolean(drFirst["DontDelete"]);
                fileFullName = string.Format("{0}{1}/{2}", GetAttRootDirectoryFullName(), filePath, fileSavedName);
                postAccount = drFirst.ToSafeStr("PostAccount");
                postDate = Convert.ToDateTime(drFirst["PostDate"]);

                if (!Convert.IsDBNull(drFirst["MdfDate"]))
                {
                    mdfAccount = drFirst.ToSafeStr("MdfAccount");
                    mdfDate = Convert.ToDateTime(drFirst["MdfDate"]);
                }
                
                //zh-TW
                if (LangManager.IsEnableEditLangZHTW())
                {
                    DataSet dsAttZhTw = artPub.GetAttachFileMultiLangDataForBackend(attId, LangManager.CultureNameZHTW);

                    if (dsAttZhTw == null || dsAttZhTw.Tables[0].Rows.Count == 0)
                    {
                        errState = AttFileErrState.LoadMultiLangDataFailed;
                        return false;
                    }

                    DataRow drZhTw = dsAttZhTw.Tables[0].Rows[0];

                    attSubjectZhTw = drZhTw.ToSafeStr("AttSubject");
                    isShowInLangZhTw = Convert.ToBoolean(drZhTw["IsShowInLang"]);
                    readCountZhTw = Convert.ToInt32(drZhTw["ReadCount"]);

                    if (!Convert.IsDBNull(drZhTw["MdfDate"]))
                    {
                        DateTime mdfDateZhTw = Convert.ToDateTime(drZhTw["MdfDate"]);

                        if (!mdfDate.HasValue || mdfDateZhTw > mdfDate.Value)
                        {
                            mdfAccount = drZhTw.ToSafeStr("MdfAccount");
                            mdfDate = mdfDateZhTw;
                        }
                    }
                }

                //en
                if (LangManager.IsEnableEditLangEN())
                {
                    DataSet dsAttEn = artPub.GetAttachFileMultiLangDataForBackend(attId, LangManager.CultureNameEN);

                    if (dsAttEn == null || dsAttEn.Tables[0].Rows.Count == 0)
                    {
                        errState = AttFileErrState.LoadMultiLangDataFailed;
                        return false;
                    }

                    DataRow drEn = dsAttEn.Tables[0].Rows[0];

                    attSubjectEn = drEn.ToSafeStr("AttSubject");
                    isShowInLangEn = Convert.ToBoolean(drEn["IsShowInLang"]);
                    readCountEn = Convert.ToInt32(drEn["ReadCount"]);

                    if (!Convert.IsDBNull(drEn["MdfDate"]))
                    {
                        DateTime mdfDateEn = Convert.ToDateTime(drEn["MdfDate"]);

                        if (!mdfDate.HasValue || mdfDateEn > mdfDate.Value)
                        {
                            mdfAccount = drEn.ToSafeStr("MdfAccount");
                            mdfDate = mdfDateEn;
                        }
                    }
                }

                result = true;
            }
            else if (contextId != null)
            {
                // new one
                sortNo = GetNextSortNo();

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 取得附件根目錄的完整路徑
        /// </summary>
        protected virtual string GetAttRootDirectoryFullName()
        {
            return Server.MapPath(string.Format("~/{0}", ConfigurationManager.AppSettings["AttRootDir"]));
        }

        /// <summary>
        /// 取得下一個排序編號
        /// </summary>
        protected virtual int GetNextSortNo()
        {
            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            int newSortNo = artPub.GetAttachFileMaxSortNo(contextId) + 10;

            return newSortNo;
        }

        /// <summary>
        /// 檢查副檔名是否允許
        /// </summary>
        protected bool IsFileExtValid(string ext)
        {
            if (fileExtLimitations == null)
                return true;

            bool isValid = false;

            foreach (string validFileExt in fileExtLimitations)
            {
                if (string.Compare(ext, validFileExt, true) == 0)
                {
                    isValid = true;
                    break;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 取得目錄名稱
        /// </summary>
        protected virtual string GetDirectoryName()
        {
            return string.Format("{0:yyyyMM}", DateTime.Today);
        }

        protected virtual bool CheckExtAndExtractFileInfo(HttpPostedFile postedFile)
        {
            string pathDirFullName = GetAttRootDirectoryFullName() + filePath;
            string uploadedFileName = Path.GetFileName(postedFile.FileName);
            string ext = Path.GetExtension(postedFile.FileName);

            if (ext.StartsWith("."))
            {
                ext = ext.Substring(1);
            }

            if (!IsFileExtValid(ext))
            {
                errState = AttFileErrState.InvalidFileExt;
                return false;
            }

            fileSavedName = string.Format("{0:yyyyMMdd_HHmmssfff}.{1}", DateTime.Now, ext);
            fileSize = postedFile.ContentLength;

            if (fileSize > 0)
            {
                fileSize = (int)Math.Round(fileSize / 1024f, MidpointRounding.AwayFromZero);
            }

            fileMIME = postedFile.ContentType;
            fileFullName = pathDirFullName + "\\" + fileSavedName;

            return true;
        }

        /// <summary>
        /// 儲存實體檔案(視條件縮圖)
        /// </summary>
        protected virtual bool SavePhysicalFileDataAndShrinkImage(HttpPostedFile postedFile)
        {
            try
            {
                string fileExt = Path.GetExtension(fileSavedName);
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
                }

                if (contentType == "application/octet-stream")
                {
                    // save normal file
                    postedFile.SaveAs(fileFullName);
                    return true;
                }

                //載入圖片,檢查是否需要縮圖
                System.Drawing.Image img = System.Drawing.Image.FromStream(postedFile.InputStream);

                int maxWidth = 1920;
                int maxHeight = 1440;
                bool isHeightMoreThanWidth = false;
                bool needsToShrink = false;

                if (img.Height > img.Width)
                {
                    isHeightMoreThanWidth = true;
                }

                if (img.Width * img.Height > maxWidth * maxHeight)
                {
                    needsToShrink = true;
                }

                if (needsToShrink)
                {
                    //等比例縮圖
                    float widthRate, heightRate;

                    if (isHeightMoreThanWidth)
                    {
                        widthRate = img.Width / (float)maxHeight;
                        heightRate = img.Height / (float)maxWidth;
                    }
                    else
                    {
                        widthRate = img.Width / (float)maxWidth;
                        heightRate = img.Height / (float)maxHeight;
                    }

                    int fitWidth, fitHeight;

                    if (widthRate > heightRate)
                    {
                        fitWidth = Convert.ToInt32(img.Width / widthRate);
                        fitHeight = Convert.ToInt32(img.Height / widthRate);
                    }
                    else
                    {
                        fitWidth = Convert.ToInt32(img.Width / heightRate);
                        fitHeight = Convert.ToInt32(img.Height / heightRate);
                    }

                    System.Drawing.Image imgFit = new Bitmap(img, new Size(fitWidth, fitHeight));

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

                    imgFit.Save(fileFullName, contentImageFormat);
                    imgFit.Dispose();
                }
                else
                {
                    //不用縮圖,直接存檔
                    postedFile.SaveAs(fileFullName);
                }
            }
            catch (Exception ex)
            {
                logger.Error("", ex);
                errState = AttFileErrState.SavePhysicalFileFailed;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 刪除實體檔案
        /// </summary>
        protected virtual bool DeletePhysicalFileData()
        {
            if (fileFullName == "")
            {
                errState = AttFileErrState.NoInitialize;
                return false;
            }

            try
            {
                FileInfo fi = new FileInfo(fileFullName);

                if (fi.Exists)
                    fi.Delete();
            }
            catch (Exception ex)
            {
                logger.Error("", ex);
                errState = AttFileErrState.DeletePhysicalFileFailed;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增附件資料
        /// </summary>
        protected virtual bool InsertData(string postAccount)
        {
            Guid newAttId = Guid.NewGuid();

            AttachFileParams param = new AttachFileParams()
            {
                AttId = newAttId,
                ArticleId = contextId.Value,
                FilePath = filePath,
                FileSavedName = fileSavedName,
                FileSize = fileSize,
                SortNo = sortNo,
                FileMIME = fileMIME,
                DontDelete = dontDelete,
                PostAccount = postAccount
            };

            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            bool result = artPub.InsertAttachFileData(param);

            if (result)
            {
                attId = newAttId;

                //zh-TW
                if (result && LangManager.IsEnableEditLangZHTW())
                {
                    AttachFileMultiLangParams paramZhTw = new AttachFileMultiLangParams()
                    {
                        AttId = attId,
                        CultureName = LangManager.CultureNameZHTW,
                        AttSubject = attSubjectZhTw,
                        IsShowInLang = isShowInLangZhTw,
                        PostAccount = postAccount
                    };

                    result = artPub.InsertAttachFileMultiLangData(paramZhTw);
                }

                //en
                if (result && LangManager.IsEnableEditLangEN())
                {
                    AttachFileMultiLangParams paramEn = new AttachFileMultiLangParams()
                    {
                        AttId = attId,
                        CultureName = LangManager.CultureNameEN,
                        AttSubject = attSubjectEn,
                        IsShowInLang = isShowInLangEn,
                        PostAccount = postAccount
                    };

                    result = artPub.InsertAttachFileMultiLangData(paramEn);
                }

                if (!result)
                {
                    errState = AttFileErrState.InsertMultiLangDataFailed;
                }
            }
            else
            {
                errState = AttFileErrState.InsertDataFailed;
            }

            return result;
        }

        /// <summary>
        /// 更新附件資料
        /// </summary>
        protected virtual bool UpdateData(string mdfAccount)
        {
            AttachFileParams param = new AttachFileParams()
            {
                AttId = attId,
                FilePath = filePath,
                FileSavedName = fileSavedName,
                FileSize = fileSize,
                SortNo = sortNo,
                FileMIME = fileMIME,
                DontDelete = dontDelete,
                PostAccount = postAccount
            };

            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            bool result = artPub.UpdateAttachFileData(param);

            if (result)
            {
                //zh-TW
                if (result && LangManager.IsEnableEditLangZHTW())
                {
                    AttachFileMultiLangParams paramZhTw = new AttachFileMultiLangParams()
                    {
                        AttId = attId,
                        CultureName = LangManager.CultureNameZHTW,
                        AttSubject = attSubjectZhTw,
                        IsShowInLang = isShowInLangZhTw,
                        PostAccount = postAccount
                    };

                    result = artPub.UpdateAttachFileMultiLangData(paramZhTw);
                }

                //en
                if (result && LangManager.IsEnableEditLangEN())
                {
                    AttachFileMultiLangParams paramEn = new AttachFileMultiLangParams()
                    {
                        AttId = attId,
                        CultureName = LangManager.CultureNameEN,
                        AttSubject = attSubjectEn,
                        IsShowInLang = isShowInLangEn,
                        PostAccount = postAccount
                    };

                    result = artPub.UpdateAttachFileMultiLangData(paramEn);
                }

                if (!result)
                {
                    errState = AttFileErrState.UpdateMultiLangDataFailed;
                }
            }
            else
            {
                errState = AttFileErrState.UpdateDataFailed;
            }

            return result;
        }
    }

    /// <summary>
    /// 網頁照片管理
    /// </summary>
    public class ArticlePictureManagerLogic : AttachFileManagerLogic
    {
        public ArticlePictureManagerLogic(HttpContext context)
            : base(context)
        {
        }

        #region Public methods

        /// <summary>
        /// 依照文章代碼初使化預設值
        /// </summary>
        public override void InitialDefaultValue(Guid? specificId)
        {
            fileExtLimitations = new List<string>();
            fileMimeLimitations = new List<string>();

            //graphic
            fileExtLimitations.Add("jpg");
            fileMimeLimitations.Add("image/jpeg");

            fileExtLimitations.Add("gif");
            fileMimeLimitations.Add("image/gif");

            fileExtLimitations.Add("png");
            fileMimeLimitations.Add("image/png");

            fileExtLimitations.Add("bmp");
            fileMimeLimitations.Add("image/bmp");
        }

        public override bool DeleteData()
        {
            if (attId == Guid.Empty)
            {
                errState = AttFileErrState.AttIdIsRequired;
                return false;
            }

            if (fileFullName == "")
            {
                errState = AttFileErrState.NoInitialize;
                return false;
            }

            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            bool result = artPub.DeleteArticlePictureData(attId);

            if (result)
            {
                if (!DeletePhysicalFileData())
                {
                    logger.InfoFormat("can't delete physical file [{0}]", fileFullName);
                }
            }
            else
            {
                errState = AttFileErrState.DeleteDataFailed;
            }

            return result;
        }

        #endregion

        protected override bool LoadData()
        {
            bool result = false;

            if (attId != Guid.Empty)
            {
                ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
                DataSet dsPic = artPub.GetArticlePictureDataForBackend(attId);

                if (dsPic == null || dsPic.Tables[0].Rows.Count == 0)
                {
                    errState = AttFileErrState.LoadDataFailed;
                    return false;
                }

                DataRow drFirst = dsPic.Tables[0].Rows[0];

                contextId = (Guid)drFirst["ArticleId"];
                filePath = GetDirectoryName();
                fileSavedName = drFirst.ToSafeStr("FileSavedName");
                fileSize = Convert.ToInt32(drFirst["FileSize"]);
                sortNo = Convert.ToInt32(drFirst["SortNo"]);
                fileMIME = drFirst.ToSafeStr("FileMIME");
                fileFullName = string.Format("{0}{1}/{2}", GetAttRootDirectoryFullName(), filePath, fileSavedName);
                postAccount = drFirst.ToSafeStr("PostAccount");
                postDate = Convert.ToDateTime(drFirst["PostDate"]);

                if (!Convert.IsDBNull(drFirst["MdfDate"]))
                {
                    mdfAccount = drFirst.ToSafeStr("MdfAccount");
                    mdfDate = Convert.ToDateTime(drFirst["MdfDate"]);
                }

                //zh-TW
                if (LangManager.IsEnableEditLangZHTW())
                {
                    DataSet dsPicZhTw = artPub.GetArticlePictureMultiLangDataForBackend(attId, LangManager.CultureNameZHTW);

                    if (dsPicZhTw == null || dsPicZhTw.Tables[0].Rows.Count == 0)
                    {
                        errState = AttFileErrState.LoadMultiLangDataFailed;
                        return false;
                    }

                    DataRow drZhTw = dsPicZhTw.Tables[0].Rows[0];

                    attSubjectZhTw = drZhTw.ToSafeStr("PicSubject");
                    isShowInLangZhTw = Convert.ToBoolean(drZhTw["IsShowInLang"]);

                    if (!Convert.IsDBNull(drZhTw["MdfDate"]))
                    {
                        DateTime mdfDateZhTw = Convert.ToDateTime(drZhTw["MdfDate"]);

                        if (!mdfDate.HasValue || mdfDateZhTw > mdfDate.Value)
                        {
                            mdfAccount = drZhTw.ToSafeStr("MdfAccount");
                            mdfDate = mdfDateZhTw;
                        }
                    }
                }

                //en
                if (LangManager.IsEnableEditLangEN())
                {
                    DataSet dsPicEn = artPub.GetArticlePictureMultiLangDataForBackend(attId, LangManager.CultureNameEN);

                    if (dsPicEn == null || dsPicEn.Tables[0].Rows.Count == 0)
                    {
                        errState = AttFileErrState.LoadMultiLangDataFailed;
                        return false;
                    }

                    DataRow drEn = dsPicEn.Tables[0].Rows[0];

                    attSubjectEn = drEn.ToSafeStr("PicSubject");
                    isShowInLangEn = Convert.ToBoolean(drEn["IsShowInLang"]);

                    if (!Convert.IsDBNull(drEn["MdfDate"]))
                    {
                        DateTime mdfDateEn = Convert.ToDateTime(drEn["MdfDate"]);

                        if (!mdfDate.HasValue || mdfDateEn > mdfDate.Value)
                        {
                            mdfAccount = drEn.ToSafeStr("MdfAccount");
                            mdfDate = mdfDateEn;
                        }
                    }
                }

                result = true;
            }
            else if (contextId != null)
            {
                // new one
                sortNo = GetNextSortNo();

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 取得下一個排序編號
        /// </summary>
        protected override int GetNextSortNo()
        {
            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            int newSortNo = artPub.GetArticlePictureMaxSortNo(contextId) + 10;

            return newSortNo;
        }

        /// <summary>
        /// 取得目錄名稱
        /// </summary>
        protected override string GetDirectoryName()
        {
            return string.Format("ArticlePictures/{0}", contextId);
        }

        /// <summary>
        /// 新增附件資料
        /// </summary>
        protected override bool InsertData(string postAccount)
        {
            Guid newPicId = Guid.NewGuid();

            ArticlePictureParams param = new ArticlePictureParams()
            {
                PicId = newPicId,
                ArticleId = contextId.Value,
                FileSavedName = fileSavedName,
                FileSize = fileSize,
                SortNo = sortNo,
                FileMIME = fileMIME,
                PostAccount = postAccount
            };

            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            bool result = artPub.InsertArticlePictureData(param);

            if (result)
            {
                attId = newPicId;

                //zh-TW
                if (result && LangManager.IsEnableEditLangZHTW())
                {
                    ArticlePictureMultiLangParams paramZhTw = new ArticlePictureMultiLangParams()
                    {
                        PicId = attId,
                        CultureName = LangManager.CultureNameZHTW,
                        PicSubject = attSubjectZhTw,
                        IsShowInLang = isShowInLangZhTw,
                        PostAccount = postAccount
                    };

                    result = artPub.InsertArticlePictureMultiLangData(paramZhTw);
                }

                //en
                if (result && LangManager.IsEnableEditLangEN())
                {
                    ArticlePictureMultiLangParams paramEn = new ArticlePictureMultiLangParams()
                    {
                        PicId = attId,
                        CultureName = LangManager.CultureNameEN,
                        PicSubject = attSubjectEn,
                        IsShowInLang = isShowInLangEn,
                        PostAccount = postAccount
                    };

                    result = artPub.InsertArticlePictureMultiLangData(paramEn);
                }

                if (!result)
                {
                    errState = AttFileErrState.InsertMultiLangDataFailed;
                }
            }
            else
            {
                errState = AttFileErrState.InsertDataFailed;
            }

            return result;
        }

        /// <summary>
        /// 更新附件資料
        /// </summary>
        protected override bool UpdateData(string mdfAccount)
        {
            ArticlePictureParams param = new ArticlePictureParams()
            {
                PicId = attId,
                FileSavedName = fileSavedName,
                FileSize = fileSize,
                SortNo = sortNo,
                FileMIME = fileMIME,
                PostAccount = postAccount
            };

            ArticlePublisherLogic artPub = new ArticlePublisherLogic(null);
            bool result = artPub.UpdateArticlePictureData(param);

            if (result)
            {
                //zh-TW
                if (result && LangManager.IsEnableEditLangZHTW())
                {
                    ArticlePictureMultiLangParams paramZhTw = new ArticlePictureMultiLangParams()
                    {
                        PicId = attId,
                        CultureName = LangManager.CultureNameZHTW,
                        PicSubject = attSubjectZhTw,
                        IsShowInLang = isShowInLangZhTw,
                        PostAccount = postAccount
                    };

                    result = artPub.UpdateArticlePictureMultiLangData(paramZhTw);
                }

                //en
                if (result && LangManager.IsEnableEditLangEN())
                {
                    ArticlePictureMultiLangParams paramEn = new ArticlePictureMultiLangParams()
                    {
                        PicId = attId,
                        CultureName = LangManager.CultureNameEN,
                        PicSubject = attSubjectEn,
                        IsShowInLang = isShowInLangEn,
                        PostAccount = postAccount
                    };

                    result = artPub.UpdateArticlePictureMultiLangData(paramEn);
                }

                if (!result)
                {
                    errState = AttFileErrState.UpdateMultiLangDataFailed;
                }
            }
            else
            {
                errState = AttFileErrState.UpdateDataFailed;
            }

            return result;
        }

    }
}
