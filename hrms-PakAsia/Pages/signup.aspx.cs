using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace hrms_PakAsia.Pages
{
    public partial class signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDepartments();
            }
            if (fuCustomFile != null && fuCustomFile.HasFile)
            {
                string fileName = fuCustomFile.FileName;
                string filePath = Server.MapPath("~/Uploads/") + fileName;
                fuCustomFile.SaveAs(filePath);
            }
        }
        private void BindDepartments()
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.GetDepartments();

            ddlDepartment.DataSource = dt;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentID";
            ddlDepartment.DataBind();

            // Add default item
            ddlDepartment.Items.Insert(0, new ListItem("Select Department", ""));
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Get values from TextBoxes
            string username = UserName.Text.Trim();
            string email = EmailAddress.Text.Trim();
            string firstName = FirstName.Text.Trim();
            string lastName = LastName.Text.Trim();
            string designation = Designation.Text.Trim();
            string cnic = Cnic.Text.Trim();
            string phone = PhoneNumber.Text.Trim();
            string password = Password.Text; // if needed, otherwise ignore

            // Get selected values from DropDownLists
            string departmentId = ddlDepartment.SelectedValue;
            string roleId = ddlRole.SelectedValue;
          

            // Other parameters required by SaveUserData
            string userID = "001";             // Empty for new user
            string createdBy = "Admin";     // You can replace with current logged-in user

            try
            {
                // Create instance of UserDAL
                UserDAL dal = new UserDAL();

                // Call the method
                string result = dal.SaveUserData(
                    username, firstName, lastName, email, cnic, phone, roleId, departmentId, createdBy, designation
                );


                // Optional: Show success message
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('User saved successfully. Result: {result}');", true);

                // Clear form
                ClearForm();
            }
            catch (Exception ex)
            {
                // Handle errors
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
        }

        // Optional helper method to clear the form
        private void ClearForm()
        {
            UserName.Text = "";
            EmailAddress.Text = "";
            FirstName.Text = "";
            LastName.Text = "";
            Designation.Text = "";
            Cnic.Text = "";
            PhoneNumber.Text = "";
            Password.Text = "";
            ddlDepartment.SelectedIndex = 0;
            ddlRole.SelectedIndex = 0;
        }

    }
}