using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Script.Serialization;

namespace hrms_PakAsia.Pages
{
    public partial class Recruitment : hrms_PakAsia.BasePage
    {
        // -------------------------
        // JobPosting paging
        // -------------------------
        private int JobPageSize => 10;
        private int JobCurrentPage
        {
            get { return ViewState["JobCurrentPage"] != null ? (int)ViewState["JobCurrentPage"] : 1; }
            set { ViewState["JobCurrentPage"] = value; }
        }
        private int JobTotalRecords
        {
            get { return ViewState["JobTotalRecords"] != null ? (int)ViewState["JobTotalRecords"] : 0; }
            set { ViewState["JobTotalRecords"] = value; }
        }
        private long? EditJobID
        {
            get { return ViewState["EditJobID"] as long?; }
            set { ViewState["EditJobID"] = value; }
        }

        // -------------------------
        // Candidate paging
        // -------------------------
        private int CandidatePageSize => 10;
        private int CandidateCurrentPage
        {
            get { return ViewState["CandidateCurrentPage"] != null ? (int)ViewState["CandidateCurrentPage"] : 1; }
            set { ViewState["CandidateCurrentPage"] = value; }
        }
        private int CandidateTotalRecords
        {
            get { return ViewState["CandidateTotalRecords"] != null ? (int)ViewState["CandidateTotalRecords"] : 0; }
            set { ViewState["CandidateTotalRecords"] = value; }
        }
        private long? EditCandidateID
        {
            get { return ViewState["EditCandidateID"] as long?; }
            set { ViewState["EditCandidateID"] = value; }
        }

        // -------------------------
        // Page Load
        // -------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindJobs();
                BindJobsDropdown();
                BindCandidates();
                // landing logged by BasePage.OnLoad
            }
        }

        // -------------------------
        // JOB POSTING METHODS
        // -------------------------
        private void BindJobs()
        {
            int total;
            DataTable dt = RecruitmentDAL.GetJobPostingsPaged(JobCurrentPage, JobPageSize, txtSearchJob.Text.Trim(), null, out total);
            JobTotalRecords = total;

            rptJobs.DataSource = dt;
            rptJobs.DataBind();
            BindJobPager();
        }

        protected void btnSaveJob_Click(object sender, EventArgs e)
        {
            string jobName = txtJobName.Text.Trim();
            if (string.IsNullOrEmpty(jobName)) return;

            if (EditJobID.HasValue)
            {
                var existing = RecruitmentDAL.GetJobPostingById(EditJobID.Value);
                string oldData = existing == null ? null : new JavaScriptSerializer().Serialize(new { JobID = EditJobID.Value, Name = existing["Name"] });
                RecruitmentDAL.SaveJobPosting(2, EditJobID.Value, jobName, 1, null, null);
                string newData = new JavaScriptSerializer().Serialize(new { JobID = EditJobID.Value, Name = jobName });
                LogAction("Update Job Posting", recordId: EditJobID.Value.ToString(), oldData: oldData, newData: newData, remarks: "Job posting updated");
                EditJobID = null;
            }
            else
            {
                RecruitmentDAL.SaveJobPosting(1, null, jobName, 1, DateTime.Now, 1);
                string newData = new JavaScriptSerializer().Serialize(new { Name = jobName });
                LogAction("Insert Job Posting", newData: newData, remarks: "Job posting created");
            }

            txtJobName.Text = "";
            BindJobs();
            BindJobsDropdown();
        }

        protected void btnClearJob_Click(object sender, EventArgs e)
        {
            txtJobName.Text = "";
            EditJobID = null;
        }

        protected void txtSearchJob_TextChanged(object sender, EventArgs e)
        {
            JobCurrentPage = 1;
            BindJobs();
            LogAction("Search Job Postings", remarks: $"Search='{txtSearchJob.Text?.Trim()}'");
        }

        protected void rptJobs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            long jobId = Convert.ToInt64(e.CommandArgument);

            if (e.CommandName == "EditJob")
            {
                DataRow dr = RecruitmentDAL.GetJobPostingById(jobId);
                if (dr != null)
                {
                    txtJobName.Text = dr["Name"].ToString();
                    EditJobID = jobId;
                }
            }
            else if (e.CommandName == "DeleteJob")
            {
                DataRow dr = RecruitmentDAL.GetJobPostingById(jobId);
                string oldData = dr == null ? null : new JavaScriptSerializer().Serialize(new { JobID = jobId, Name = dr["Name"] });
                RecruitmentDAL.DeleteJobPosting(jobId);
                LogAction("Delete Job Posting", recordId: jobId.ToString(), oldData: oldData, remarks: "Job posting deleted");
                BindJobs();
                BindJobsDropdown();
            }
        }

        private void BindJobsDropdown()
        {
            int totalRecords;
            ddlJob.DataSource = RecruitmentDAL.GetJobPostingsPaged(1, 100, null, null, out totalRecords);
            ddlJob.DataBind();
            ddlJob.Items.Insert(0, new ListItem("Select Job", "0"));
        }

        // -------------------------
        // CANDIDATE METHODS
        // -------------------------
        private void BindCandidates()
        {
            int total;
            DataTable dt = RecruitmentDAL.GetCandidatesPaged(
                CandidateCurrentPage,
                CandidatePageSize,
                txtSearchCandidate.Text.Trim(),
                ddlJob.SelectedValue != "0" ? Convert.ToInt64(ddlJob.SelectedValue) : (long?)null,
                out total
            );
            CandidateTotalRecords = total;

            rptCandidates.DataSource = dt;
            rptCandidates.DataBind();
            BindCandidatePager();
        }

        protected void btnSaveCandidate_Click(object sender, EventArgs e)
        {
            if (ddlJob.SelectedValue == "0" || string.IsNullOrEmpty(txtCandidateName.Text)) return;

            long jobId = Convert.ToInt64(ddlJob.SelectedValue);
            string name = txtCandidateName.Text.Trim();
            int status = Convert.ToInt32(ddlStatus.SelectedValue);
            string CVPath = null;

            try
            {
                CVPath = SaveCVFile(); // may return null if no file
            }
            catch (Exception ex)
            {
                // show message (optional)
                return;
            }
            if (EditCandidateID.HasValue)
            {
                var existing = RecruitmentDAL.GetCandidateById(EditCandidateID.Value);
                string oldData = existing == null ? null : new JavaScriptSerializer().Serialize(new
                {
                    CandidateID = EditCandidateID.Value,
                    JobID = existing["JobID"],
                    Name = existing["Name"],
                    Status = existing["Status"],
                    CV = existing["CVPath"]
                });
                RecruitmentDAL.SaveCandidate(2, EditCandidateID.Value, jobId, name, status, null, null, CVPath);
                string newData = new JavaScriptSerializer().Serialize(new { CandidateID = EditCandidateID.Value, JobID = jobId, Name = name, Status = status, CV = CVPath });
                LogAction("Update Candidate", recordId: EditCandidateID.Value.ToString(), oldData: oldData, newData: newData, remarks: "Candidate updated");
                EditCandidateID = null;
            }
            else
            {
                RecruitmentDAL.SaveCandidate(1, null, jobId, name, status, DateTime.Now, 1, CVPath);
                string newData = new JavaScriptSerializer().Serialize(new { JobID = jobId, Name = name, Status = status, CV = CVPath });
                LogAction("Insert Candidate", newData: newData, remarks: "Candidate created");
            }

            ClearCandidateForm();
            BindCandidates();
        }
        private string SaveCVFile()
        {
            if (!fuCV.HasFiles)
                return null;

            List<string> paths = new List<string>();

            string basePath = "~/Uploads/Candidates/";
            string serverPath = Server.MapPath(basePath);

            if (!Directory.Exists(serverPath))
                Directory.CreateDirectory(serverPath);

            foreach (HttpPostedFile file in fuCV.PostedFiles)
            {
                string ext = Path.GetExtension(file.FileName).ToLower();

                // optional validation
                if (!new[] { ".pdf", ".doc", ".docx" }.Contains(ext))
                    continue;

                string fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                string fullPath = basePath + fileName;

                file.SaveAs(Server.MapPath(fullPath));
                paths.Add(fullPath);
            }

            return paths.Count > 0 ? string.Join(",", paths) : null;
        }

        protected void btnClearCandidate_Click(object sender, EventArgs e)
        {
            ClearCandidateForm();
        }

        private void ClearCandidateForm()
        {
            txtCandidateName.Text = "";
            ddlJob.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            EditCandidateID = null;
        }

        protected void txtSearchCandidate_TextChanged(object sender, EventArgs e)
        {
            CandidateCurrentPage = 1;
            BindCandidates();
            LogAction("Search Candidates", remarks: $"Search='{txtSearchCandidate.Text?.Trim()}'");
        }
        protected void rptCandidates_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            long candidateId = Convert.ToInt64(e.CommandArgument.ToString().Split(',')[0]);

            if (e.CommandName == "EditCandidate")
            {
                DataRow dr = RecruitmentDAL.GetCandidateById(candidateId);
                if (dr != null)
                {
                    txtCandidateName.Text = dr["Name"].ToString();
                    ddlJob.SelectedValue = dr["JobID"].ToString();
                    ddlStatus.SelectedValue = dr["Status"].ToString();
                    EditCandidateID = candidateId;
                }
            }
            else if (e.CommandName == "DeleteCandidate")
            {
                DataRow dr = RecruitmentDAL.GetCandidateById(candidateId);
                string oldData = dr == null ? null : new JavaScriptSerializer().Serialize(new
                {
                    CandidateID = candidateId,
                    JobID = dr["JobID"],
                    Name = dr["Name"],
                    Status = dr["Status"],
                    CV = dr["CVPath"]
                });
                RecruitmentDAL.DeleteCandidate(candidateId);
                LogAction("Delete Candidate", recordId: candidateId.ToString(), oldData: oldData, remarks: "Candidate deleted");
                BindCandidates();
            }
            else if (e.CommandName == "ChangeStatus")
            {
                // Get StatusID from CommandArgument
                int statusId = Convert.ToInt32(e.CommandArgument.ToString().Split(',')[1]);
                // Update candidate status
                RecruitmentDAL.UpdateCandidateStatus(candidateId, statusId);
                LogAction("Update Candidate Status", recordId: candidateId.ToString(), newData: new JavaScriptSerializer().Serialize(new { CandidateID = candidateId, Status = statusId }), remarks: "Candidate status changed");
                // Rebind Repeater
                BindCandidates();
            }
        }

        // -------------------------
        // PAGINATION HELPERS
        // -------------------------
        private void BindJobPager()
        {
            List<object> pages = new List<object>();
            int totalPages = (int)Math.Ceiling((double)JobTotalRecords / JobPageSize);

            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(new { PageNumber = i, IsCurrent = i == JobCurrentPage });
            }

            rptJobPager.DataSource = pages;
            rptJobPager.DataBind();
            lblJobPageInfo.Text = $"Page {JobCurrentPage} of {totalPages}";
        }

        protected void rptJobPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                JobCurrentPage = Convert.ToInt32(e.CommandArgument);
                BindJobs();
            }
        }

        protected void btnJobPrev_Click(object sender, EventArgs e)
        {
            if (JobCurrentPage > 1)
            {
                JobCurrentPage--;
                BindJobs();
            }
        }

        protected void btnJobNext_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)JobTotalRecords / JobPageSize);
            if (JobCurrentPage < totalPages)
            {
                JobCurrentPage++;
                BindJobs();
            }
        }

        private void BindCandidatePager()
        {
            List<object> pages = new List<object>();
            int totalPages = (int)Math.Ceiling((double)CandidateTotalRecords / CandidatePageSize);

            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(new { PageNumber = i, IsCurrent = i == CandidateCurrentPage });
            }

            rptCandidatePager.DataSource = pages;
            rptCandidatePager.DataBind();
            lblCandidatePageInfo.Text = $"Page {CandidateCurrentPage} of {totalPages}";
        }

        protected void rptCandidatePager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                CandidateCurrentPage = Convert.ToInt32(e.CommandArgument);
                BindCandidates();
            }
        }

        protected void btnCandidatePrev_Click(object sender, EventArgs e)
        {
            if (CandidateCurrentPage > 1)
            {
                CandidateCurrentPage--;
                BindCandidates();
            }
        }

        protected void btnCandidateNext_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)CandidateTotalRecords / CandidatePageSize);
            if (CandidateCurrentPage < totalPages)
            {
                CandidateCurrentPage++;
                BindCandidates();
            }
        }

        protected void rptCandidates_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (drv != null)
                {
                    int status = Convert.ToInt32(drv["Status"]);

                    // Find the buttons
                    LinkButton btnPass = e.Item.FindControl("btnPass") as LinkButton;
                    LinkButton btnFailed = e.Item.FindControl("btnFailed") as LinkButton;
                    LinkButton btnHold = e.Item.FindControl("btnHold") as LinkButton;

                    // Show all buttons by default
                    btnPass.Visible = true;
                    btnFailed.Visible = true;
                    btnHold.Visible = true;

                    // If status is assigned, show only the assigned button
                    switch (status)
                    {
                        case 1: // Pass
                            btnPass.Visible = true;
                            btnFailed.Visible = false;
                            btnHold.Visible = false;
                            break;
                        case 2: // Failed
                            btnPass.Visible = false;
                            btnFailed.Visible = true;
                            btnHold.Visible = false;
                            break;
                        case 3: // Hold
                            btnPass.Visible = false;
                            btnFailed.Visible = false;
                            btnHold.Visible = true;
                            break;
                        default: // 0 = no status
                            btnPass.Visible = true;
                            btnFailed.Visible = true;
                            btnHold.Visible = true;
                            break;
                    }
                }
            }
        }

    }
}
