using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public interface IMasterArticleSettings
{
    /// <summary>
    /// 為首頁
    /// </summary>
    bool IsHomePage { get; set; }
}