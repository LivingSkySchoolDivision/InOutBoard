using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYInOut.My
{
    public partial class index1 : System.Web.UI.Page
    {
        TrackedUserRepository _userRepo = new TrackedUserRepository();
        StatusRepository _statusRepo = new StatusRepository();
        FriendlyTimeSpanRepository _timeRepo = new FriendlyTimeSpanRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load people
                drpUsers.Items.Clear();
                foreach (TrackedUser u in _userRepo.GetAll())
                {
                    drpUsers.Items.Add(new ListItem(u.DisplayName, u.ID.ToString()));
                }

                // Load statuses
                drpStatuses.Items.Clear();
                foreach (Status s in _statusRepo.GetAll())
                {
                    drpStatuses.Items.Add(new ListItem(s.Name, s.ID.ToString()));
                }

                // Load timespans
                drpExpiry.Items.Clear();
                foreach (FriendlyTimeSpan t in _timeRepo.GetAll())
                {
                    drpExpiry.Items.Add(new ListItem(t.Name, t.ID.ToString()));
                }
            }
        }

        private string errorText(string str)
        {
            return "<b style='color: red;'>" + str + "</b>";
        }

        private void updateExistingStatuses()
        {
            // Get the user ID from the hidden field
            TrackedUser selectedUser = _userRepo.Get(Parsers.ToInt(txtUserID.Value));
            tblCurrentStatuses.Rows.Clear();
            int statuscount = 0;
            foreach (UserStatus status in selectedUser.Statuses)
            {
                statuscount++;
                bool showThen = (statuscount < selectedUser.Statuses.Count) ? true : false;
                tblCurrentStatuses.Rows.Add(addStatusTableRow(status, showThen));
            }
        }
        
        private TableRow addStatusTableRow(UserStatus status, bool showThen)
        {
            TableRow row = new TableRow();
            row.CssClass = "status_table_row";

            string line = "<b>" + status.Status.Name + "</b> until <b>" + status.Expires.ToShortDateString() + " at " + status.Expires.ToShortTimeString() + "</b>";
            if (showThen)
            {
                line += ", and then...";
            }

            row.Cells.Add(new TableCell() { Text = "<div class=\"user_status\">" + line + "</div>", CssClass="status_table_row" });
            //row.Cells.Add(new TableCell() { Text = "<a class=\"status_controls\" href=\"#\">REMOVE THIS STATUS</a>", CssClass = "status_table_row" });
            return row;
        }

        protected void btnSetStatus_Click(object sender, EventArgs e)
        {
            // Parse selected values
            TrackedUser selectedUser = _userRepo.Get(Parsers.ToInt(txtUserID.Value));
            Status selectedStatus = _statusRepo.Get(Parsers.ToInt(drpStatuses.SelectedValue));
            FriendlyTimeSpan selectedTimeSpan = _timeRepo.Get(Parsers.ToInt(drpExpiry.SelectedValue));
                        
            if (selectedUser.ID > 0)
            {
                if (selectedStatus.ID > 0)
                {
                    if (selectedTimeSpan.ID > 0)
                    {
                        UserStatus userStatus = new UserStatus()
                        {
                            Status = selectedStatus,
                            Expires = DateTime.Now.Add(selectedTimeSpan.TimeSpan)
                        };
                        _userRepo.UpdateUserStatus(selectedUser, selectedStatus, selectedTimeSpan);
                        updateExistingStatuses();
                    }
                    else
                    {
                        litStatus.Text = errorText("Invalid timespan selected");
                    }
                }
                else
                {
                    litStatus.Text = errorText("Invalid status selected");
                }
            }
            else
            {
                litStatus.Text = errorText("Invalid user selected");
            }

        }

        protected void btnSelectUser_Click(object sender, EventArgs e)
        {
            // parse user
            TrackedUser selectedUser = _userRepo.Get(Parsers.ToInt(drpUsers.SelectedValue));

            if (selectedUser.ID > 0)
            {
                lblSelectedUser.Text = selectedUser.DisplayName + " is currently: <b>" + selectedUser.ActiveStatus + "</b>";
                txtUserID.Value = selectedUser.ID.ToString();
                tblUpdateControls.Visible = true;

                updateExistingStatuses();
            }

        }

        protected void btnAddCustomStatus_Click(object sender, EventArgs e)
        {

        }
    }
}