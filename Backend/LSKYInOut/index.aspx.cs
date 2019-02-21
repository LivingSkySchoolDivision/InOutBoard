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
            TrackedUserRepository _userRepo = new TrackedUserRepository();
            List<TrackedUser> allUsers = _userRepo.GetAll();

            Dictionary<int, Group> foundGroupIDs = new Dictionary<int, Group>();

            tblAllUsersStatus.Rows.Clear();
            foreach (TrackedUser user in allUsers)
            {
                tblAllUsersStatus.Rows.Add(addUserStatusRow(user));
                foreach(Group group in user.Groups)
                {
                    if (!group.IsHidden)
                    {
                        if (!foundGroupIDs.ContainsKey(group.ID))
                        {
                            foundGroupIDs.Add(group.ID, group);
                        }
                    }
                }
            }

            htmlContent += "<p><a class=\"button\" href=\"/My\">Change your status</a></p>";
            htmlContent += "<p><a class=\"button\" href=\"/iPad/\">Change your status (ipad version)</a></p>";
            foreach (Group group in foundGroupIDs.Values.OrderBy(x => x.Name)) {
                htmlContent += "<p><a class=\"button\" href=\"/iPad/?groupid=" + group.ID + "\">Ipad for: " + group.Name + "</a></p>";
            }
            htmlContent += "<p><a class=\"button\" href=\"/JSON\">JSON file</a></p>";
            htmlContent += "<p><a class=\"button\" href=\"/Manage\">Manage Users and Statuses</a></p>";
            litLinks.Text = htmlContent;

            
                       
        }
    }
}