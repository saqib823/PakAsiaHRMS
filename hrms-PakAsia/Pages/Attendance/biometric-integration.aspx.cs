using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Net.Sockets;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Attendance
{
    public partial class biometric_integration : System.Web.UI.Page
    {
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                LoadBranches();
                LoadAttendanceLogs();
            }
        }

        #endregion

        #region Device Configuration

        protected void btnTestConnection_Click(object sender, EventArgs e)
        {
            bool isConnected = TestDeviceConnection(
                txtIPAddress.Text.Trim(),
                txtPort.Text.Trim()
            );

            ShowAlert(
                isConnected ? "Device connection successful." : "Unable to connect to device.",
                isConnected ? "success" : "danger"
            );
        }

        protected void btnSaveDevice_Click(object sender, EventArgs e)
        {
            BiometricDeviceDAL.SaveDevice(
                txtDeviceName.Text.Trim(),
                ddlDeviceType.SelectedValue,
                txtIPAddress.Text.Trim(),
                Convert.ToInt32(txtPort.Text),
                Convert.ToInt32(ddlBranch.SelectedValue),
                ddlStatus.SelectedItem.Text == "Active",
                chkIn.Checked,
                chkOut.Checked,
                chkBreakIn.Checked,
                chkBreakOut.Checked,
                chkManualPunch.Checked
            );

            ShowAlert("Device configuration saved successfully.", "success");

            ClearDeviceForm();
        }

        #endregion

        #region Attendance Logs

        private void LoadAttendanceLogs()
        {
            rptAttendance.DataSource = AttendanceDAL.GetRealtimeLogs();
            rptAttendance.DataBind();
        }

        #endregion

        #region Helpers

        private void LoadBranches()
        {
            ddlBranch.DataSource = CommonDAL.GetBranches();
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();

            ddlBranch.Items.Insert(0, new ListItem("Select One", "0"));
        }

        private bool TestDeviceConnection(string ip, string port)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ip, Convert.ToInt32(port));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void ClearDeviceForm()
        {
            txtDeviceName.Text = "";
            txtIPAddress.Text = "";
            txtPort.Text = "";

            ddlDeviceType.SelectedIndex = 0;
            ddlBranch.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;

            chkIn.Checked = true;
            chkOut.Checked = true;
            chkBreakIn.Checked = false;
            chkBreakOut.Checked = false;
            chkManualPunch.Checked = false;
        }

        private void ShowAlert(string message, string css)
        {
            phAlert.Controls.Clear();

            phAlert.Controls.Add(new Literal
            {
                Text = $@"
                <div id='autoAlert' class='alert alert-{css} alert-dismissible fade show'>
                    {message}
                </div>
                <script>
                    setTimeout(() => document.getElementById('autoAlert')?.remove(), 3000);
                </script>"
            });
        }

        #endregion
    }
}
