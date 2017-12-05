using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /*
     * 使用前，請在web.config中加入下列定義
     * 
        <configuration>
            <appSettings>
              <!-- 定義語言文化名稱(for db) -->
              <add key="CultureNameZHTW" value="zh-TW"/>
              <add key="CultureNameEN" value="en"/>

              <!-- 定義語言號碼(for pic dir, queryString) -->
              <add key="LangNoZHTW" value="1"/>
              <add key="LangNoEN" value="2"/>

              <!-- 設定可以在後台編輯的語言版本 -->
              <add key="EnableEditLangEN" value="true"/>
            </appSettings>
        </configuration>
     */

    /// <summary>
    /// 語系設定值管理
    /// </summary>
    public class LangManager
    {
        #region static memebers

        private static volatile LangManager instance = null;
        private static object syncRoot = new object();

        public static LangManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new LangManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// 語言文化名稱(中)
        /// </summary>
        public static string CultureNameZHTW { get { return ConfigurationManager.AppSettings["CultureNameZHTW"]; } }

        /// <summary>
        /// 語言文化名稱(eng)
        /// </summary>
        public static string CultureNameEN { get { return ConfigurationManager.AppSettings["CultureNameEN"]; } }

        /// <summary>
        /// 語言號碼(中)
        /// </summary>
        public static string LangNoZHTW { get { return ConfigurationManager.AppSettings["LangNoZHTW"]; } }

        /// <summary>
        /// 語言號碼(eng)
        /// </summary>
        public static string LangNoEN { get { return ConfigurationManager.AppSettings["LangNoEN"]; } }

        /// <summary>
        /// 是否有啟用編輯中文版
        /// </summary>
        public static bool IsEnableEditLangZHTW()
        {
            bool isEnabled = false;

            if (ConfigurationManager.AppSettings["EnableEditLangZHTW"] != null)
            {
                bool.TryParse(ConfigurationManager.AppSettings["EnableEditLangZHTW"], out isEnabled);
            }

            return isEnabled;
        }

        /// <summary>
        /// 是否有啟用編輯英文版
        /// </summary>
        public static bool IsEnableEditLangEN()
        {
            bool isEnabled = false;

            if (ConfigurationManager.AppSettings["EnableEditLangEN"] != null)
            {
                bool.TryParse(ConfigurationManager.AppSettings["EnableEditLangEN"], out isEnabled);
            }

            return isEnabled;
        }

        /// <summary>
        /// 是否有啟用編輯(中文版以外的)其他語言
        /// </summary>
        public static bool IsEnableEditOtherLanguages()
        {
            //是否啟用編輯英文版
            if (IsEnableEditLangEN())
                return true;

            return false;
        }

        #endregion

        private List<LangInfo> langInfos = null;

        public LangManager()
        {
            InitialLangInfos();
        }

        private void InitialLangInfos()
        {
            langInfos = new List<LangInfo>();

            langInfos.Add(new LangInfo()
            {
                CultureNameKey = "CultureNameZHTW",
                LangNoKey = "LangNoZHTW"
            });

            langInfos.Add(new LangInfo()
            {
                CultureNameKey = "CultureNameEN",
                LangNoKey = "LangNoEN"
            });
        }

        public void AddLangInfo(string cultureNameKey, string langNoKey)
        {
            langInfos.Add(new LangInfo()
            {
                CultureNameKey = cultureNameKey,
                LangNoKey = langNoKey
            });
        }

        /// <summary>
        /// 用語言號碼取得語言文化名稱
        /// </summary>
        public string GetCultureName(string langNo)
        {
            string cultureName = "";

            foreach (LangInfo info in langInfos)
            {
                if (ConfigurationManager.AppSettings[info.LangNoKey] == langNo)
                {
                    cultureName = ConfigurationManager.AppSettings[info.CultureNameKey];
                    break;
                }
            }

            //找不到時,預設第一項
            if (cultureName == "")
                cultureName = ConfigurationManager.AppSettings[langInfos[0].CultureNameKey];

            return cultureName;
        }

        /// <summary>
        /// 用語言文化名稱取得語言號碼
        /// </summary>
        public string GetLangNo(string cultureName)
        {
            string langNo = "";

            foreach (LangInfo info in langInfos)
            {
                if (ConfigurationManager.AppSettings[info.CultureNameKey] == cultureName)
                {
                    langNo = ConfigurationManager.AppSettings[info.LangNoKey];
                    break;
                }
            }

            //找不到時,預設第一項
            if (langNo == "")
                langNo = ConfigurationManager.AppSettings[langInfos[0].LangNoKey];

            return langNo;
        }

        #region inner class

        /// <summary>
        /// 語系設定值
        /// </summary>
        class LangInfo
        {
            public string CultureNameKey = "";
            public string LangNoKey = "";
        }

        #endregion
    }
}
