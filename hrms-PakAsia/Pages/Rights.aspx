<%@ Page Title="Assign Rights"
    Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="Rights.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Rights" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main class="main mt-10">

      <asp:TreeView
        ID="tvFolders"
        runat="server"
        ShowCheckBoxes="All"
        EnableClientScript="true"
        ExpandDepth="1"
        CssClass="treeview">
    </asp:TreeView>


        <asp:Button
            ID="btnSave"
            runat="server"
            Text="Save Rights"
            CssClass="btn btn-secondary mt-3"
            OnClick="btnSave_Click" />

    </main>
    <script type="text/javascript">
    document.addEventListener("change", function (e) {

        if (e.target.type !== "checkbox") return;

        let checkbox = e.target;
        let li = checkbox.closest("li");

        // 1️⃣ Apply to children
        let childCheckboxes = li.querySelectorAll("input[type='checkbox']");
        childCheckboxes.forEach(cb => cb.checked = checkbox.checked);

        // 2️⃣ Update parents
        updateParents(li);
    });

    function updateParents(li) {
        let parentLi = li.parentElement.closest("li");
        if (!parentLi) return;

        let childCbs = parentLi.querySelectorAll(":scope > ul > li input[type='checkbox']");
        let parentCb = parentLi.querySelector("input[type='checkbox']");

        parentCb.checked = [...childCbs].some(cb => cb.checked);

        updateParents(parentLi);
    }
</script>

</asp:Content>
