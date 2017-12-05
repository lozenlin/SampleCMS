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

        public Guid ContextId
        {
            get { return contextId; }
            set { contextId = value; }
        }
        protected Guid contextId;

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
        protected Guid nullArticleId;
        protected string errMsg = "";

        /// <summary>
        /// 附件檔案管理
        /// </summary>
        public AttachFileManagerLogic(HttpContext context)
        {
            this.context = context;
            logger = LogManager.GetLogger(this.GetType());
            nullArticleId = new Guid("683F6132-CF89-413D-9503-FB2F3E47E4EF");
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

            return LoadData();
        }

        public Guid GetNullArticleId()
        {
            return nullArticleId;
        }

        public string GetErrMsg()
        {
            return errMsg;
        }

        /// <summary>
        /// 依照文章代碼初使化預設值
        /// </summary>
        public virtual void InitialDefaultValue(Guid? contextId)
        {
            //fileExtLimitations = new List<string>();
            //fileMimeLimitations = new List<string>();
        }

        /// <summary>
        /// 檢查副檔名是否允許
        /// </summary>
        public bool IsFileExtValid(string ext)
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

        #endregion

        protected virtual bool LoadData()
        {
            bool result = false;

            return result;
        }

    }
}
