using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYInOut.JSON
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TrackedUserRepository _userRepo = new TrackedUserRepository();

            List<TrackedUser> users = _userRepo.GetAll().Where(u => !u.IsHidden).ToList();
            Response.Clear();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/json; charset=utf-8";

            int filteredGroupID = Request.QueryString?["groupid"]?.ToString().ToInt() ?? 0;

            if (filteredGroupID > 0)
            {
                users = users.Where(u => u.GroupIDs.Contains(filteredGroupID)).ToList();
            }

            Response.Write("{ ");            
            Response.Write("\"Users\" :[");

            int counter = 0;
            foreach (TrackedUser u in users.OrderBy(x => x.DisplayName))
            {
                counter++;
                Response.Write("{");

                Response.Write("\"ID\": " + u.ID + ",");
                Response.Write("\"Name\": \"" + u.DisplayName + "\",");
                Response.Write("\"ActiveStatus\": " + StatusJSON(u.ActiveStatus) + ",");
                Response.Write("\"AllStatuses\": [");

                int statusCounter = 0;
                foreach(UserStatus status in u.Statuses)
                {
                    statusCounter++;
                    Response.Write(StatusJSON(status));
                    if (statusCounter < u.Statuses.Count)
                    {
                        Response.Write(",");
                    }
                }
                Response.Write("] ");

                Response.Write("}");

                if (counter < users.Count)
                {
                    Response.Write(",");
                }            
            }
            Response.Write("] ");

            Response.Write("} ");
            Response.End();
        }

        private string StatusJSON(UserStatus status)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("{ ");            
            returnMe.Append(" \"Name\": \"" + status.Status.Name + "\",");
            returnMe.Append(" \"InOrOut\": \"" + (status.Status.ID != 0 ? (status.Status.IsInOffice ? "IN" : "OUT") : "?") + "\",");
            returnMe.Append(" \"Color\": \"" + status.Status.Color + "\",");
            returnMe.Append(" \"IsInOffice\": \"" + status.Status.IsInOffice + "\",");
            returnMe.Append(" \"IsAtWork\": \"" + status.Status.IsAtWork + "\",");
            returnMe.Append(" \"IsBusy\": \"" + status.Status.IsBusy + "\",");
            returnMe.Append(" \"Expires\": \"" + status.Expires + "\" ");
            returnMe.Append("} ");

            return returnMe.ToString();
        }
    }
}