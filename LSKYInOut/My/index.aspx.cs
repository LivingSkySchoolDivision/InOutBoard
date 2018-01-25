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

        private void updateExistingStatuses(TrackedUser user)
        {
            txtUserID.Value = user.ID.ToString();
            tblUpdateControls.Visible = false;
            
            if (user != null)
            {
                tblUpdateControls.Visible = true;
                
                chkSatusList.Items.Clear();                    
                int statuscount = 0;
                foreach (UserStatus status in user.Statuses.OrderBy(x => x.Expires))
                {
                    statuscount++;

                    string line = "<b>" + status.Status.Name + "</b> until <b>" + status.Expires.ToShortDateString() + " at " + status.Expires.ToShortTimeString() + "</b>";
                    if (statuscount < user.Statuses.Count)
                    {
                        line += ", and then...";
                    }

                    chkSatusList.Items.Add(new ListItem() { Value = status.Thumbprint, Text = line });
                }

                lblSelectedUser.Text = user.DisplayName + " is currently: <b>" + user.ActiveStatus + "</b>";
                txtUserID.Value = user.ID.ToString();
                
            }
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
                        UserStatus userStatus = _userRepo.UpdateUserStatus(selectedUser, selectedStatus, selectedTimeSpan);

                        // Update the user status list displayed on the page
                        selectedUser.Statuses.Add(userStatus);                        
                        updateExistingStatuses(selectedUser);

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
            TrackedUserRepository _userRepo = new TrackedUserRepository();
            TrackedUser user = _userRepo.Get(Parsers.ToInt(drpUsers.SelectedValue));
            if (user != null)
            {
                updateExistingStatuses(user);
            }
        }

        protected void btnAddCustomStatus_Click(object sender, EventArgs e)
        {

        }

        protected void btnRemoveCheckedStatuses_Click(object sender, EventArgs e)
        {
            List<string> thumbprintsToRemove = new List<string>();
            foreach(ListItem item in chkSatusList.Items)
            {
                if (item.Selected)
                {
                    thumbprintsToRemove.Add(item.Value);
                }
            }

            if (thumbprintsToRemove.Count > 0)
            {
                UserStatusRepository _userStatusRepo = new UserStatusRepository();
                foreach (string thumb in thumbprintsToRemove)
                {
                    _userStatusRepo.RemoveStatus(thumb);
                }

                if (!string.IsNullOrEmpty(txtUserID.Value))
                {
                    // Remove them from the selected user object so we can update the display of them
                    TrackedUser user = _userRepo.Get(txtUserID.Value.ToInt());

                    // Update the display
                    updateExistingStatuses(user);
                }
            }
            
        }
    }
}