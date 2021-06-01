<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userForm.aspx.cs" Inherits="Aquarelle.userForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Form</title>
    <!-- CSS only -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous" />
    <!-- JavaScript Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-gtEjrD/SeCtmISkJkNUaaKMoLD0//ElJ19smozuHV6z3Iehds+3Ulb9Bn9Plx0x4" crossorigin="anonymous"></script>

</head>
<body>
    <form id="form1" runat="server">
        <!--Timer to check every 5s, if new file has been added-->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:Timer ID="TimerVerifyNewFile" OnTick="TimerVerifyNewFile_Tick" runat="server" Interval="10000" />

        <div>
            <!-- Navbar -->
            <nav class="navbar navbar-light" style="background-color: rebeccapurple;">
                <div class="container-fluid">
                    <label id="WelcomeLabel" runat="server" class="navbar-brand mb-0 h1" href="#" style="color: floralwhite;"></label>
                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                            <li class="nav-item">
                                <asp:Button
                                    type="button"
                                    class="btn btn-danger"
                                    ID="btnLogout"
                                    runat="server"
                                    OnClick="btnLogout_Click"
                                    Text="Logout" />
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>

            <!-- Alert -->
            <div runat="server" id="Alert" visible="false" class="alert alert-danger alert-dismissible fade show d-flex align-items-center" role="alert">
                <svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Danger:">
                    <use xlink:href="#exclamation-triangle-fill" />
                </svg>
                <div>
                    <label runat="server" id="AlertLabel" />
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>

            <!-- Content -->
            <div class="container-fluid" style="background-color: #e3f2fd;">
                <asp:GridView ID="GrdFilesView"
                    class="table table-striped"
                    runat="server"
                    AutoGenerateColumns="false"
                    OnRowCommand="GrdFilesView_RowCommand"
                    OnRowDataBound="OnRowDataBound">
                    <Columns>
                        <asp:BoundField DataField="FileName" HeaderText="File Name" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <div class="d-grid gap-2 d-md-block">
                                    <asp:Button class="btn btn-primary"
                                        type="button"
                                        ID="btnView"
                                        runat="server"
                                        CommandName="view"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        Text="View"
                                        CausesValidation="false" />

                                    <asp:Button class="btn btn-secondary"
                                        type="button"
                                        ID="btnPrint"
                                        runat="server"
                                        CommandName="print"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        Text="Print" />

                                    <asp:Button class="btn btn-success"
                                        type="button"
                                        ID="btnMarkAsRead"
                                        runat="server"
                                        CommandName="markAsRead"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        Text="Mark As Read" />

                                    <asp:Button class="btn btn-warning"
                                        type="button"
                                        ID="btnExtractInfo"
                                        runat="server"
                                        CommandName="extractInfo"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        Text="Extract Information" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="IsFileRead" />
                    </Columns>
                </asp:GridView>
            </div>

            <!-- No data -->
            <div class="container-fluid" id="divNoFile" runat="server" visible="false" style="background-color: #C0392B; height: auto; text-align: center; margin-top: 90px;">
                <label style="font-family: fantasy; font-size: xxx-large; color: white; line-height: 400px;">
                    No Files Available !
                </label>
            </div>

        </div>

        <svg xmlns="http://www.w3.org/2000/svg" style="display: none;">
            <symbol id="check-circle-fill" fill="currentColor" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z" />
            </symbol>
            <symbol id="info-fill" fill="currentColor" viewBox="0 0 16 16">
                <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z" />
            </symbol>
            <symbol id="exclamation-triangle-fill" fill="currentColor" viewBox="0 0 16 16">
                <path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z" />
            </symbol>
        </svg>



        <%-- <asp:Button ID="testBtn" runat="server" OnClick="testBtn_Click" />--%>
    </form>
</body>
</html>

