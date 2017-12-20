using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYInOut.My
{
    public partial class index : System.Web.UI.Page
    {
        TrackedUserRepository _userRepo = new TrackedUserRepository();
        StatusRepository _statusRepo = new StatusRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            // If you come back to this and wonder wtf the question marks are, they are null propagation operators.
            // If the groupid variable is null, it resolves to the 0 at the end.

            int staffID = Request.QueryString?["staffid"]?.ToString().ToInt() ?? 0;
            int statusID = Request.QueryString?["statusid"]?.ToString().ToInt() ?? 0;
            int filteredGroupID = Request.QueryString?["groupid"]?.ToString().ToInt() ?? 0;

            // If there is no staff Id, display staff picker
            // If there is no status ID, display status picker
            // If there IS a staff and status ID, display expiration picker

            // Show all 3 sections on the page, but if there are no IDs then they should be empty

            TrackedUser user = _userRepo.Get(staffID);
            Status status = _statusRepo.Get(statusID);

            if (user.ID == 0)
            {
                Response.Write("<!--Staff-->");
                litStaffList.Text = StaffPicker(filteredGroupID);
            } else if (status.ID == 0)
            {
                Response.Write("<!--Status-->");
                litStatusList.Text = StatusPicker(filteredGroupID, staffID);
            } else
            {
                Response.Write("<!--Expiration-->");
                litExpiration.Text = ExpiryPicker(filteredGroupID, staffID, statusID);
            }
        }

        private string StaffPicker(int groupID)
        {
            StringBuilder returnMe = new StringBuilder();
            
            returnMe.Append("<div id=\"staff_picker_container\">");

            List<TrackedUser> displayedUsers = _userRepo.GetAll().Where(u => !u.IsHidden).ToList();
            if (groupID > 0)
            {
                displayedUsers = displayedUsers.Where(u => u.GroupIDs.Contains(groupID)).ToList();
            }

            foreach (TrackedUser u in displayedUsers)
            {
                returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=" + u.ID + "\" class=\"staff_picker_person\">" + u.DisplayNameLastNameFirst + "</a>");
            }

            returnMe.Append("</div>");

            return returnMe.ToString();
        }

        private string StatusPicker(int groupID, int staffID)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<div id=\"status_picker_container\">");
            returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=0\" class=\"fa fa-arrow-circle-left return_button\"></a>");


            List<Status> statuses = _statusRepo.GetAll();

            foreach (Status s in statuses)
            {
                returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=" + staffID + "&statusid=" + s.ID + "\" class=\"status_picker_status\">" + s.Name + "</a>");
            }

            returnMe.Append("</div>");

            return returnMe.ToString();
        }

        private string ExpiryPicker(int groupID, int staffID, int statusID)
        {

            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<div id=\"expiration_picker_container\">");
            returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=" + staffID + "\" class=\"fa fa-arrow-circle-left return_button\"></a>");

                        

            returnMe.Append("</div>");

            return returnMe.ToString();
        }

    }
}