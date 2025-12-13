using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                InitialDataBindings();
            }
           
        }
        private void InitialDataBindings()
        {
            try
            {
                ddlDepartment.DataSource = CommonDAL.GetDepartments();
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("Select One", "0"));

                ddlRole.DataSource = CommonDAL.GetRoles();
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new ListItem("Select One", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
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
            byte[] fileBytes = null;
            string contentType = "";
            if (customFile != null && customFile.PostedFile != null && customFile.PostedFile.ContentLength > 0)
            {
                HttpPostedFile postedFile = customFile.PostedFile;

                
                using (var binaryReader = new BinaryReader(postedFile.InputStream))
                {
                    fileBytes = binaryReader.ReadBytes(postedFile.ContentLength);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                contentType = postedFile.ContentType;

                // fileBytes now contains the uploaded file as byte[]
            }



            // Other parameters required by SaveUserData
            string userID = "001";             // Empty for new user
            string createdBy = "Admin";     // You can replace with current logged-in user

            try
            {
                // Create instance of UserDAL
                UserDAL dal = new UserDAL();

                // Call the method
                string result = dal.SaveUserData(
                    username, password, firstName, lastName, email, cnic, phone, roleId, departmentId, createdBy, designation, fileBytes, contentType
                );


                // Optional: Show success message
                phAlert.Controls.Clear();

                phAlert.Controls.Add(new LiteralControl(@"
                    <div class='alert alert-subtle-success alert-dismissible fade show' role='alert'>
                        <strong>Success!</strong> User saved successfully.
                        <button class='btn-close' type='button' data-bs-dismiss='alert'></button>
                    </div>"));

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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}
