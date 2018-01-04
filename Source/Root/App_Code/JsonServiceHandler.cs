using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 取得附縮圖的網頁內容清單
/// </summary>
public class Article_GetListWithThumb : JsonServiceHandlerAbstract
{
    /// <summary>
    /// 取得附縮圖的網頁內容清單
    /// </summary>
    public Article_GetListWithThumb(HttpContext context)
        : base(context)
    {
    }

    public override ClientResult ProcessRequest()
    {
        ClientResult cr = null;

        return cr;
    }
}