using log4net;
using System;
using System.Net;
using System.Xml;

namespace Common.Utility
{
    public class XmlUtility
    {
        private static ILog logger = null;

        static XmlUtility()
        {
            logger = LogManager.GetLogger(typeof(XmlUtility));
        }

        protected XmlUtility()
        {
        }

        /// <summary>
        /// 取得網址回傳的XML資料
        /// </summary>
        public static XmlDocument GetXmlDocument(string address, int timeout)
        {
            WebRequest webReq;
            WebResponse webRes = null;
            XmlDocument xmlDoc = null;

            try
            {
                webReq = WebRequest.Create(address);
                ((HttpWebRequest)webReq).UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
                webReq.Timeout = timeout;
                webRes = webReq.GetResponse();
                xmlDoc = new XmlDocument();
                xmlDoc.Load(webRes.GetResponseStream());

                webRes.Close();
            }
            catch (Exception ex)
            {
                logger.Error("", ex);
                return null;
            }

            return xmlDoc;
        }

    }
}
