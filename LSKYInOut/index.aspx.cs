using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYInOut
{
    public partial class index : System.Web.UI.Page
    {
        private TableRow addUserStatusRow(TrackedUser user)
        {
            TableRow row = new TableRow();

            row.Cells.Add(new TableCell()
            {
                Text = user.DisplayName
            });

            row.Cells.Add(new TableCell()
            {
                Text = user.ActiveStatus.Status.Name
            });
            
            return row;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string htmlContent = "";

            htmlContent += "<p><a class=\"button\" href=\"/My\">Change your status</a></p>";
            htmlContent += "<p><a class=\"button\" href=\"/My/ipad.aspx\">Change your status (ipad version)</a></p>";
            htmlContent += "<p><a class=\"button\" href=\"/JSON\">JSON file</a></p>";
            htmlContent += "<p><a class=\"button\" href=\"/Manage\">Manage Users and Statuses</a></p>";
            litLinks.Text = htmlContent;

            tblAllUsersStatus.Rows.Clear();
            TrackedUserRepository _userRepo = new TrackedUserRepository();

            foreach(TrackedUser user in _userRepo.GetAll())
            {
                tblAllUsersStatus.Rows.Add(addUserStatusRow(user));
            }
                       
        }
    }
}