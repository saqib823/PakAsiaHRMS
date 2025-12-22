using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Shifts
{
    public partial class SplitShift : System.Web.UI.Page
    {
        private const int PageSize = 10;

        private int CurrentPage
        {
            get => ViewState["PageIndex"] == null ? 0 : (int)ViewState["PageIndex"];
            set => ViewState["PageIndex"] = value;
        }

        private int EditingShiftID
        {
            get => ViewState["EditingShiftID"] == null ? 0 : (int)ViewState["EditingShiftID"];
            set => ViewState["EditingShiftID"] = value;
        }

        private int TotalPages { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSplitShifts();
                LoadSplitShiftTable();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int shiftId = Convert.ToInt32(ddlShift.SelectedValue);

            // Nullable TimeSpan
            TimeSpan? p1Start = string.IsNullOrWhiteSpace(txtPart1Start.Text) ? (TimeSpan?)null : TimeSpan.Parse(txtPart1Start.Text);
            TimeSpan? p1End = string.IsNullOrWhiteSpace(txtPart1End.Text) ? (TimeSpan?)null : TimeSpan.Parse(txtPart1End.Text);
            TimeSpan? p2Start = string.IsNullOrWhiteSpace(txtPart2Start.Text) ? (TimeSpan?)null : TimeSpan.Parse(txtPart2Start.Text);
            TimeSpan? p2End = string.IsNullOrWhiteSpace(txtPart2End.Text) ? (TimeSpan?)null : TimeSpan.Parse(txtPart2End.Text);

            if (EditingShiftID > 0)
            {
                // Update existing split shift parts
                ShiftDAL.UpdateSplitShift(EditingShiftID, p1Start, p1End, 1);
                ShiftDAL.UpdateSplitShift(EditingShiftID, p2Start, p2End, 2);

                EditingShiftID = 0;
                btnSave.Text = "Save Split Shift";
            }
            else
            {
                // Insert new split shift
                ShiftDAL.InsertSplitShift(shiftId, p1Start, p1End, p2Start, p2End);
            }

            ClearForm();
            CurrentPage = 0;
            LoadSplitShiftTable();
        }

        protected void rptSplitShifts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                DataTable dt = ShiftDAL.GetSplitShiftById(id); // DataTable, not DataRow
                if (dt.Rows.Count > 0)
                {
                    var part1 = dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("PartNo") == 1);
                    var part2 = dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("PartNo") == 2);

                    ddlShift.SelectedValue = dt.Rows[0]["ShiftID"].ToString();
                    if (part1 != null)
                    {
                        var start1 = part1.Field<TimeSpan?>("StartTime");
                        var end1 = part1.Field<TimeSpan?>("EndTime");

                        txtPart1Start.Text = start1?.ToString(@"hh\:mm") ?? "";
                        txtPart1End.Text = end1?.ToString(@"hh\:mm") ?? "";
                    }

                    if (part2 != null)
                    {
                        var start2 = part2.Field<TimeSpan?>("StartTime");
                        var end2 = part2.Field<TimeSpan?>("EndTime");

                        txtPart2Start.Text = start2?.ToString(@"hh\:mm") ?? "";
                        txtPart2End.Text = end2?.ToString(@"hh\:mm") ?? "";
                    }


                    EditingShiftID = id;
                    btnSave.Text = "Update";
                }
            }
            else if (e.CommandName == "DeleteRow")
            {
                ShiftDAL.DeleteSplitShift(id);
                LoadSplitShiftTable();
            }
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            CurrentPage = Convert.ToInt32(e.CommandArgument);
            LoadSplitShiftTable();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0) CurrentPage--;
            LoadSplitShiftTable();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages - 1) CurrentPage++;
            LoadSplitShiftTable();
        }

        private void LoadSplitShiftTable()
        {
            int totalRecords;
            DataTable dt = ShiftDAL.GetSplitShiftDetailsPaged(CurrentPage, PageSize, out totalRecords);

            rptSplitShifts.DataSource = dt;
            rptSplitShifts.DataBind();

            TotalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
            BuildPager(totalRecords);

            lblPageInfo.Text = $"Page {CurrentPage + 1} of {TotalPages}";
            btnPrev.Enabled = CurrentPage > 0;
            btnNext.Enabled = CurrentPage < TotalPages - 1;
        }

        private void BuildPager(int totalRecords)
        {
            int pageCount = (int)Math.Ceiling((double)totalRecords / PageSize);
            DataTable dtPager = new DataTable();
            dtPager.Columns.Add("Text");
            dtPager.Columns.Add("PageIndex", typeof(int));
            dtPager.Columns.Add("IsCurrent", typeof(bool));

            for (int i = 0; i < pageCount; i++)
            {
                DataRow dr = dtPager.NewRow();
                dr["Text"] = (i + 1).ToString();
                dr["PageIndex"] = i;
                dr["IsCurrent"] = i == CurrentPage;
                dtPager.Rows.Add(dr);
            }

            rptPager.DataSource = dtPager;
            rptPager.DataBind();
        }

        private void ClearForm()
        {
            txtPart1Start.Text = "";
            txtPart1End.Text = "";
            txtPart2Start.Text = "";
            txtPart2End.Text = "";
            ddlShift.SelectedIndex = 0;
            EditingShiftID = 0;
            btnSave.Text = "Save Split Shift";
        }

        private void LoadSplitShifts()
        {
            DataTable dt = ShiftDAL.GetSplitTypeShifts();
            ddlShift.DataSource = dt;
            ddlShift.DataTextField = "ShiftName";
            ddlShift.DataValueField = "ShiftID";
            ddlShift.DataBind();
            ddlShift.Items.Insert(0, new ListItem("-- Select Shift --", "0"));
        }
    }
}
