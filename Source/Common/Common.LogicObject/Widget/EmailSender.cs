// ===============================================================================
// EmailSender of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// EmailSender.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// Email 寄送工具
    /// </summary>
    public class EmailSender
    {
        protected ILog logger = null;
        protected string errMsg = "";

        public EmailSender()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        #region 設定用屬性

        /// <summary>
        /// 寄信時用的SMTP伺服器位置, default 127.0.0.1
        /// </summary>
        public string SmtpServer = "127.0.0.1";

        /// <summary>
        /// 寄信時用的SMTP Port, default 25
        /// </summary>
        public int SmtpPort = 25;

        /// <summary>
        /// 寄送失敗時,重試的次數, default 1
        /// </summary>
        public int RetryCount = 1;

        /// <summary>
        /// 寄送逾時時間(ms), default 60000
        /// </summary>
        public int TimeOutMs = 60000;

        #endregion

        #region 傳送純文字格式信件

        /// <summary>
        /// 使用匿名方式傳送純文字格式信件
        /// </summary>
        public bool Send(string From,
            string To,
            string Subject, string Body,
            params string[] AttFileNames)
        {
            return Send(From,
                To,
                Subject, Body,
                TimeOutMs, 
                AttFileNames);
        }

        /// <summary>
        /// 使用匿名方式傳送純文字格式信件
        /// </summary>
        public bool Send(string From,
            string To,
            string Subject, string Body,
            int timeout, 
            params string[] AttFileNames)
        {
            string Cc = "", Bcc = "";

            return Send(From,
                To, Cc, Bcc,
                Subject, Body,
                timeout, 
                AttFileNames);
        }

        /// <summary>
        /// 使用匿名方式傳送純文字格式信件
        /// </summary>
        public bool Send(string From,
            string To, string Cc, string Bcc,
            string Subject, string Body,
            params string[] AttFileNames)
        {
            return Send(From,
                To, Cc, Bcc,
                Subject, Body,
                TimeOutMs, 
                AttFileNames);
        }

        /// <summary>
        /// 使用匿名方式傳送純文字格式信件
        /// </summary>
        public bool Send(string From,
            string To, string Cc, string Bcc,
            string Subject, string Body,
            int timeout, 
            params string[] AttFileNames)
        {
            string Account = "", Password = "";

            return Send(Account, Password, From,
                To, Cc, Bcc,
                Subject, Body,
                timeout, 
                AttFileNames);
        }

        /// <summary>
        /// 使用驗證方式傳送純文字格式信件
        /// </summary>
        public bool Send(string Account, string Password, string From,
            string To, string Cc, string Bcc,
            string Subject, string Body,
            params string[] AttFileNames)
        {
            return Send(Account, Password, From,
                To, Cc, Bcc,
                Subject, Body,
                TimeOutMs, 
                AttFileNames);
        }

        /// <summary>
        /// 使用驗證方式傳送純文字格式信件
        /// </summary>
        public bool Send(string Account, string Password, string From,
            string To, string Cc, string Bcc,
            string Subject, string Body,
            int timeout, 
            params string[] AttFileNames)
        {
            bool IsBodyHtml = false;

            return SendBase(Account, Password, From,
                To, Cc, Bcc,
                Subject, Body, IsBodyHtml,
                timeout, 
                AttFileNames);
        }

        #endregion

        #region 傳送HTML格式信件

        /// <summary>
        /// 使用匿名方式傳送HTML格式信件
        /// </summary>
        public bool SendHtml(string From,
            string To, string Cc, string Bcc,
            string Subject, string Body,
            params string[] AttFileNames)
        {
            return SendHtml(From,
                To, Cc, Bcc,
                Subject, Body,
                TimeOutMs, 
                AttFileNames);
        }

        /// <summary>
        /// 使用匿名方式傳送HTML格式信件
        /// </summary>
        public bool SendHtml(string From,
            string To, string Cc, string Bcc,
            string Subject, string Body,
            int timeout, 
            params string[] AttFileNames)
        {
            string Account = "", Password = "";

            return SendHtml(Account, Password, From,
                To, Cc, Bcc,
                Subject, Body,
                timeout, 
                AttFileNames);
        }

        /// <summary>
        /// 使用驗證方式傳送HTML格式信件
        /// </summary>
        public bool SendHtml(string Account, string Password, string From,
            string To, string Cc, string Bcc,
            string Subject, string Body,
            params string[] AttFileNames)
        {
            return SendHtml(Account, Password, From,
                To, Cc, Bcc,
                Subject, Body,
                TimeOutMs, 
                AttFileNames);
        }

        /// <summary>
        /// 使用驗證方式傳送HTML格式信件
        /// </summary>
        public bool SendHtml(string Account, string Password, string From,
            string To, string Cc, string Bcc,
            string Subject, string Body,
            int timeout, 
            params string[] AttFileNames)
        {
            bool IsBodyHtml = true;

            return SendBase(Account, Password, From,
                To, Cc, Bcc,
                Subject, Body, IsBodyHtml,
                timeout, 
                AttFileNames);
        }

        #endregion

        /// <summary>
        /// 使用驗證方式傳送指定格式信件
        /// </summary>
        private bool SendBase(string Account, string Password, string From,
            string To, string Cc, string Bcc,
            string Subject, string Body, bool IsBodyHtml,
            int timeout, 
            params string[] AttFileNames)
        {
            //建立寄信元件
            SmtpClient MailSender = GetMailSender(Account, Password, timeout);

            //建立信件
            MailMessage MailMsg = GetMailMsg(
                From, To, Cc,
                Bcc, Subject, Body,
                IsBodyHtml, AttFileNames);

            //寄送郵件
            bool bResult = SendMailMsg(MailSender, MailMsg);

            MailMsg.Dispose();

            return bResult;
        }

        /// <summary>
        /// 取得預設建立的 SmtpClient 寄信元件
        /// </summary>
        public SmtpClient GetMailSender(string Account, string Password, int timeout)
        {
            //建立寄信元件
            SmtpClient MailSender = new SmtpClient(SmtpServer, SmtpPort);
            MailSender.Timeout = timeout;

            if (Account == "" && Password == "")
            {
                //使用匿名傳送
                MailSender.UseDefaultCredentials = true;
            }
            else
            {
                //使用帳號密碼
                MailSender.UseDefaultCredentials = false;
                MailSender.Credentials = new System.Net.NetworkCredential(Account, Password);
            }

            return MailSender;
        }

        /// <summary>
        /// 取得預設建立的信件元件
        /// </summary>
        public MailMessage GetMailMsg(string From, string To, string Cc,
            string Bcc, string Subject, string Body,
            bool IsBodyHtml, params string[] AttFileNames)
        {
            //建立信件
            MailMessage MailMsg = new MailMessage(From, To, Subject, Body);

            if (Cc != "")
                MailMsg.CC.Add(Cc);

            if (Bcc != "")
                MailMsg.Bcc.Add(Bcc);

            if (IsBodyHtml)
            {
                //使用Html格式
                MailMsg.IsBodyHtml = true;
                MailMsg.BodyEncoding = Encoding.UTF8;
            }

            //附件
            if (AttFileNames.Length > 0)
                foreach (string FileName in AttFileNames)
                    MailMsg.Attachments.Add(new Attachment(FileName));

            return MailMsg;
        }

        /// <summary>
        /// 寄送郵件
        /// </summary>
        public bool SendMailMsg(SmtpClient MailSender, MailMessage MailMsg)
        {
            errMsg = "";

            for (int i = 1; i <= RetryCount; i++)
            {
                try
                {
                    MailSender.Send(MailMsg);
                }
                catch (SmtpException smtpEx)
                {
                    logger.Error("", smtpEx);

                    //只記錄最後一次的錯誤
                    if (i == RetryCount)
                    {
                        errMsg = string.Format("an error occurred!\n reason: {0}({1})\n",
                            smtpEx.Message, smtpEx.StatusCode.ToString());
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    logger.Error("", ex);

                    //只記錄最後一次的錯誤
                    if (i == RetryCount)
                    {
                        errMsg = string.Format("an error occurred!\n reason: {0}\n",
                            ex.Message);
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 取得最後錯誤訊息
        /// </summary>
        public string GetErrMsg()
        {
            return errMsg;
        }
    }
}
