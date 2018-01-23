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
            Response.Write("\"Statuses\" :[");

            int counter = 0;
            foreach (TrackedUser u in users.OrderBy(x => x.DisplayName))
            {
                counter++;
                Response.Write("{");

                Response.Write("\"ID\": " + u.ID + ",");
                Response.Write("\"Name\": \"" + u.DisplayName + "\",");
                Response.Write("\"Status\": \"" + u.Status.Name + "\",");
                Response.Write("\"IsAtWork\": \""+u.Status.IsAtWork+"\",");
                Response.Write("\"StatusColor\": \"" + u.Status.Color + "\",");
                Response.Write("\"InOrOut\": \"" + (u.Status.ID != 0 ? (u.Status.IsInOffice ? "IN" : "OUT") : "") + "\"");
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
    }
}