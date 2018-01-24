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
        FriendlyTimeSpanRepository _timeRepo = new FriendlyTimeSpanRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            // If you come back to this and wonder wtf the question marks are, they are null propagation operators.
            // If the groupid variable is null, it resolves to the 0 at the end.

            int staffID = Request.QueryString?["staffid"]?.ToString().ToInt() ?? 0;
            int statusID = Request.QueryString?["statusid"]?.ToString().ToInt() ?? 0;
            int filteredGroupID = Request.QueryString?["groupid"]?.ToString().ToInt() ?? 0;
            Double expireHours = Request.QueryString?["expires"]?.ToString().ToDouble() ?? 0;
            int clear_status = Request.QueryString?["clearstatus"]?.ToString().ToInt() ?? 0;

            // If there is no staff Id, display staff picker
            // If there is no status ID, display status picker
            // If there IS a staff and status ID, display expiration picker

            // Show all 3 sections on the page, but if there are no IDs then they should be empty

            TrackedUser user = _userRepo.Get(staffID);
            Status status = _statusRepo.Get(statusID);

            if (expireHours < 0)
            {
                expireHours = 0;
            }
            if (expireHours > 9000)
            {
                expireHours = 9000;
            }

            if (clear_status == 1)
            {
                _userRepo.ClearUserStatus(user);

                // Reset the variables so we go back to the main menu
                staffID = 0;
                statusID = 0;
                expireHours = 0;
                user = _userRepo.Get(0);
                status = _statusRepo.Get(0);
            }

            if ((user.ID > 0) && (status.ID > 0) && (expireHours > 0))
            {
                // Set the user's status
                // Calculate a timespan based on the hours given                 
                DateTime expiryDate = DateTime.Now.AddHours(expireHours);

                Response.Write("<!--Receipt-->");
                litReceipt.Text = Receipt(filteredGroupID, user, status, expiryDate);

                _userRepo.UpdateUserStatus(user, status, expiryDate);

                // Reset the variables so we go back to the main menu
                /*
                staffID = 0;
                statusID = 0;
                expireHours = 0;
                user = _userRepo.Get(0);
                status = _statusRepo.Get(0);*/
            } 

            if (user.ID == 0)
            {
                Response.Write("<!--Staff-->");
                litStaffList.Text = StaffPicker(filteredGroupID);
            } else if (status.ID == 0)
            {
                Response.Write("<!--Status-->");
                litStatusList.Text = StatusPicker(filteredGroupID, user);
            } else if (expireHours == 0)
            {
                Response.Write("<!--Expiration-->");
                litExpiration.Text = ExpiryPicker(filteredGroupID, user, status);
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

            foreach (TrackedUser u in displayedUsers.OrderBy(u => u.DisplayName))
            {
                returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=" + u.ID + "\" class=\"button\">" + u.DisplayName + "</a>");
            }

            returnMe.Append("</div>");

            return returnMe.ToString();
        }

        private string StatusPicker(int groupID, TrackedUser staff)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<div id=\"status_picker_container\">");

            returnMe.Append("<div id=\"top_bar\">");
            returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=0\" class=\"fa fa-arrow-circle-left return_button\"></a>");
            returnMe.Append("<div id=\"top_bar_info\">");
            returnMe.Append("<b>" + staff.DisplayName + "</b> is:");
            returnMe.Append("</div>");
            returnMe.Append("</div>");


            List<Status> statuses = _statusRepo.GetAll();

            if (staff.Statuses.Count > 0)
            {
                returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=" + staff.ID + "&clearstatus=1\" class=\"button clear_button\">Clear my status(es)</a>");
            }

            foreach (Status s in statuses)
            {
                returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=" + staff.ID + "&statusid=" + s.ID + "\" class=\"button\">" + s.Name + "</a>");
            }
            
            returnMe.Append("</div>");

            return returnMe.ToString();
        }

        private string ExpiryPicker(int groupID, TrackedUser staff, Status status)
        {

            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<div id=\"expiration_picker_container\">");

            returnMe.Append("<div id=\"top_bar\">");
            returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=" + staff.ID + "\" class=\"fa fa-arrow-circle-left return_button\"></a>");
            returnMe.Append("<div id=\"top_bar_info\">");
            returnMe.Append("<b>" + staff.DisplayName + "</b> is <b>" + status.Name + "</b> until:");
            returnMe.Append("</div>");
            returnMe.Append("</div>");

            List<FriendlyTimeSpan> timeSpans = _timeRepo.GetAll();

            foreach(FriendlyTimeSpan t in timeSpans)
            {
                double TotalHours = t.TimeSpan.TotalHours;
                if (TotalHours < 0.6)
                {
                    TotalHours = 0.6;
                }
                returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=" + staff.ID + "&statusid=" + status.ID + "&expires=" + TotalHours + "\" class=\"button\">" + t.Name + "</a>");
            }

                        

            returnMe.Append("</div>");

            return returnMe.ToString();
        }

        private string Receipt(int groupID, TrackedUser user, Status status, DateTime expirydate)            
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<div id=\"receipt_container\">");
            
            returnMe.Append("<div class=\"receipt_box\">");
            returnMe.Append("<div class=\"receipt_text\">" + user.DisplayName + "</div>");
            returnMe.Append("<div class=\"receipt_interlude\">is now</div>");
            returnMe.Append("<div class=\"receipt_text\">" + status.Name + "</div>");
            returnMe.Append("<div class=\"receipt_interlude\">until</div>");
            returnMe.Append("<div class=\"receipt_text\">" + expirydate.ToLongDateString() + "</div>");
            returnMe.Append("<div class=\"receipt_interlude\">at</div>");
            returnMe.Append("<div class=\"receipt_text\">" + expirydate.ToShortTimeString() + "</div>");
            returnMe.Append("<a href=\"?groupid=" + groupID + "&staffID=0&statusid=0\" class=\"button\" style=\"width: 80%;margin-left: auto; margin-right: auto;\">OK</a>");
            returnMe.Append("</div>");

            returnMe.Append("</p>");

            returnMe.Append("</div>");

            return returnMe.ToString();
        }
    }
}