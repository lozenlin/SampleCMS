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
        DataAccessSource db = new DataAccessSource();
        spEmployee_GetDataToLogin sp = new spEmployee_GetDataToLogin(db)
        {
            EmpAccount = "EmpAccountValue",
            newId = 123
        };

        DataSet ds = sp.ExecuteDataset();
    }
}