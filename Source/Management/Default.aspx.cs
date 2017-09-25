using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common.DataAccess;
using Common.DataAccess.EmployeeAuthorityDataAccess;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAccessSource mainDB = new DataAccessSource();
        spEmployee_GetList cmd = new spEmployee_GetList(mainDB)
        {
            DeptId = 0,
            SearchName = "",
            ListMode = 2,
            BeginNum = 1,
            EndNum = 20,
            SortField = "",
            IsSortDesc = false
        };

        DataSet ds = cmd.ExecuteDataset();
        int rowCount = cmd.RowCount;
    }
}