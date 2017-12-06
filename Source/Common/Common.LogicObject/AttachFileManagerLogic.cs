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
using System.Linq;
using System.Text;
using System.Web;

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

            //octet-stream
            fileExtLimitations.Add("csv");
            fileExtLimitations.Add("rar");
            fileMimeLimitations.Add("application/octet-stream");

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
                    mdfDate = Convert.ToDateTime("MdfDate");
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

                    attSubjectZhTw = drZhTw.ToSafeStr("ArticleSubject");
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

                    attSubjectEn = drEn.ToSafeStr("ArticleSubject");
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

    }
}
