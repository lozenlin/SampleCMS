using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common.DataAccess;
using Common.DataAccess.EmployeeAuthority;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);

        spEmployee_GetList cmdInfo = new spEmployee_GetList()
        {
            DeptId = 0,
            SearchName = "",
            ListMode = 2,
            BeginNum = 1,
            EndNum = 20,
            SortField = "",
            IsSortDesc = false
        };
        
        DataSet ds = cmd.ExecuteDataset(cmdInfo);
        int rowCount = cmdInfo.RowCount;

        if (ds != null)
            Response.Write(string.Format("table rows count:{0} <br>", ds.Tables[0].Rows.Count));

        Response.Write(string.Format("rowCount:{0}", rowCount));
    }
}