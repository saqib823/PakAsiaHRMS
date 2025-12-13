using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(email.Text) && !string.IsNullOrWhiteSpace(password.Text))
            {
                UserDAL dal = new UserDAL();
                LoggedInUser currentUser = dal.LoginUser(email.Text, password.Text);
                if (currentUser != null)
                {
                    Response.Redirect("~/Pages/dashboard.aspx");
                }
                else
                {
                    ShowAlert("Invalid Credentials", "danger");
                    return;
                }
            }
        }
        private void ShowAlert(string message, string css)
        {
            phAlert.Controls.Clear();

            phAlert.Controls.Add(new Literal
            {
                Text = $@"
        <div id='autoAlert' class='alert alert-{css} alert-dismissible fade show' role='alert'>
            {message}
        </div>

        <script>
            setTimeout(function () {{
                var alert = document.getElementById('autoAlert');
                if (alert) {{
                    alert.classList.remove('show');
                    alert.classList.add('hide');
                }}
            }}, 3000); // 3 seconds
        </script>"
            });
        }
    }
}