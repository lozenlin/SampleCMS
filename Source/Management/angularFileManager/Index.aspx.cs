using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class angularFileManager_Index : System.Web.UI.Page
{
    protected BackendPageCommon c;
    protected string afmLang = "en";

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new BackendPageCommon(this.Context, this.ViewState);

        afmLang = c.seCultureNameOfBackend;

        if (string.Compare(afmLang, "zh-TW", true) == 0)
        {
            afmLang = "zh_tw";
        }
        else if (afmLang != "en")
        {
            afmLang = "en";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}