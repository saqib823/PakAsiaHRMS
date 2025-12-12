<%@ Page Title="Sign up" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="hrms_PakAsia.Pages.signup" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main class="main" id="top">
        <div class="container">

            <div class="row py-5">

                <!-- Page Heading -->
                <div class="text-center mb-4">
                    <h3 class="mt-3">Create Account</h3>
                    <p class="text-body-tertiary">Fill the required information</p>
                </div>

                <!-- Row 1 — 4 Fields -->
                <div class="row">

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Username</label>
                        <asp:TextBox CssClass="form-control" ID="UserName" runat="server" placeholder="Username"></asp:TextBox>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Email</label>
                        <asp:TextBox CssClass="form-control" ID="EmailAddress" runat="server" TextMode="Email" placeholder="email@example.com"></asp:TextBox>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">First Name</label>
                        <asp:TextBox CssClass="form-control" ID="FirstName" runat="server" placeholder="First name"></asp:TextBox>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Last Name</label>
                        <asp:TextBox CssClass="form-control" ID="LastName" runat="server" placeholder="Last name"></asp:TextBox>
                    </div>
                    
                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Designation</label>
                        <asp:TextBox CssClass="form-control" ID="Designation" runat="server" placeholder="Designation"></asp:TextBox>
                    </div>


              
                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Department</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlDepartment" runat="server">
                            <asp:ListItem Text="Select Department" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                   <div class="col-sm-3 mt-3">
                        <label class="form-label">Role</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlRole" runat="server">
                            <asp:ListItem Text="Select Role" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">CNIC</label>
                        <asp:TextBox CssClass="form-control" ID="Cnic" runat="server" MaxLength="15"  placeholder="12345-1234567-1"></asp:TextBox>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Phone</label>
                        <asp:TextBox CssClass="form-control" ID="PhoneNumber" runat="server" MaxLength="10" placeholder="03XX-XXXXXXX"></asp:TextBox>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Password</label>
                        <asp:TextBox CssClass="form-control" ID="Password" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 mt-3">
                      <label class="form-label" for="customFile">Attach Image</label>
    <asp:FileUpload ID="fuCustomFile" CssClass="form-control" runat="server" />
                    </div>
                
                   


                <!-- Buttons -->
                <div class="text-end mt-4">

                    <asp:Button ID="btnClear" runat="server"
                        Text="Clear" CssClass="btn btn-secondary me-2 mb-3" />

                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server"
                        Text="Save" CssClass="btn btn-primary mb-3" />

                </div>
                                    </div>

                <!-- TABLE BELOW THE FORM -->
                
                      <div id="tableExample3" data-list="{&quot;valueNames&quot;:[&quot;name&quot;,&quot;email&quot;,&quot;age&quot;],&quot;page&quot;:5,&quot;pagination&quot;:true}">
                        <div class="search-box mb-3 mx-auto">
                          <form class="position-relative"><input class="form-control search-input search form-control-sm" type="search" placeholder="Search" aria-label="Search">
                            <svg class="svg-inline--fa fa-magnifying-glass search-box-icon" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="magnifying-glass" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M416 208c0 45.9-14.9 88.3-40 122.7L502.6 457.4c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L330.7 376c-34.4 25.2-76.8 40-122.7 40C93.1 416 0 322.9 0 208S93.1 0 208 0S416 93.1 416 208zM208 352a144 144 0 1 0 0-288 144 144 0 1 0 0 288z"></path></svg><!-- <span class="fas fa-search search-box-icon"></span> Font Awesome fontawesome.com -->
                          </form>
                        </div>
                        <div class="table-responsive">
                          <table class="table table-striped table-sm fs-9 mb-0">
                            <thead>
                              <tr>
                                <th class="sort border-top border-translucent ps-3" data-sort="name">Name</th>
                                <th class="sort border-top" data-sort="email">Email</th>
                                <th class="sort border-top" data-sort="age">Age</th>
                                <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                              </tr>
                            </thead>
                            <tbody class="list"><tr>
                                <td class="align-middle ps-3 name">Anna</td>
                                <td class="align-middle email">anna@example.com</td>
                                <td class="align-middle age">18</td>
                                <td class="align-middle white-space-nowrap text-end pe-0">
                                  <div class="btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs-10" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><svg class="svg-inline--fa fa-ellipsis fs-10" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z"></path></svg><!-- <span class="fas fa-ellipsis-h fs-10"></span> Font Awesome fontawesome.com --></button>
                                    <div class="dropdown-menu dropdown-menu-end py-2"><a class="dropdown-item" href="#!">View</a><a class="dropdown-item" href="#!">Export</a>
                                      <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Remove</a>
                                    </div>
                                  </div>
                                </td>
                              </tr><tr>
                                <td class="align-middle ps-3 name">Homer</td>
                                <td class="align-middle email">homer@example.com</td>
                                <td class="align-middle age">35</td>
                                <td class="align-middle white-space-nowrap text-end pe-0">
                                  <div class="btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs-10" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><svg class="svg-inline--fa fa-ellipsis fs-10" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z"></path></svg><!-- <span class="fas fa-ellipsis-h fs-10"></span> Font Awesome fontawesome.com --></button>
                                    <div class="dropdown-menu dropdown-menu-end py-2"><a class="dropdown-item" href="#!">View</a><a class="dropdown-item" href="#!">Export</a>
                                      <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Remove</a>
                                    </div>
                                  </div>
                                </td>
                              </tr><tr>
                                <td class="align-middle ps-3 name">Oscar</td>
                                <td class="align-middle email">oscar@example.com</td>
                                <td class="align-middle age">52</td>
                                <td class="align-middle white-space-nowrap text-end pe-0">
                                  <div class="btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs-10" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><svg class="svg-inline--fa fa-ellipsis fs-10" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z"></path></svg><!-- <span class="fas fa-ellipsis-h fs-10"></span> Font Awesome fontawesome.com --></button>
                                    <div class="dropdown-menu dropdown-menu-end py-2"><a class="dropdown-item" href="#!">View</a><a class="dropdown-item" href="#!">Export</a>
                                      <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Remove</a>
                                    </div>
                                  </div>
                                </td>
                              </tr><tr>
                                <td class="align-middle ps-3 name">Emily</td>
                                <td class="align-middle email">emily@example.com</td>
                                <td class="align-middle age">30</td>
                                <td class="align-middle white-space-nowrap text-end pe-0">
                                  <div class="btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs-10" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><svg class="svg-inline--fa fa-ellipsis fs-10" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z"></path></svg><!-- <span class="fas fa-ellipsis-h fs-10"></span> Font Awesome fontawesome.com --></button>
                                    <div class="dropdown-menu dropdown-menu-end py-2"><a class="dropdown-item" href="#!">View</a><a class="dropdown-item" href="#!">Export</a>
                                      <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Remove</a>
                                    </div>
                                  </div>
                                </td>
                              </tr><tr>
                                <td class="align-middle ps-3 name">Jara</td>
                                <td class="align-middle email">jara@example.com</td>
                                <td class="align-middle age">25</td>
                                <td class="align-middle white-space-nowrap text-end pe-0">
                                  <div class="btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs-10" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><svg class="svg-inline--fa fa-ellipsis fs-10" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z"></path></svg><!-- <span class="fas fa-ellipsis-h fs-10"></span> Font Awesome fontawesome.com --></button>
                                    <div class="dropdown-menu dropdown-menu-end py-2"><a class="dropdown-item" href="#!">View</a><a class="dropdown-item" href="#!">Export</a>
                                      <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Remove</a>
                                    </div>
                                  </div>
                                </td>
                              </tr></tbody>
                          </table>
                        </div>
                        <div class="d-flex justify-content-between mt-3"><span class="d-none d-sm-inline-block" data-list-info="data-list-info">1 to 5 <span class="text-body-tertiary"> Items of </span>43</span>
                          <div class="d-flex"><button class="page-link disabled" data-list-pagination="prev" disabled=""><svg class="svg-inline--fa fa-chevron-left" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="chevron-left" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512" data-fa-i2svg=""><path fill="currentColor" d="M9.4 233.4c-12.5 12.5-12.5 32.8 0 45.3l192 192c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L77.3 256 246.6 86.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0l-192 192z"></path></svg><!-- <span class="fas fa-chevron-left"></span> Font Awesome fontawesome.com --></button>
                            <ul class="mb-0 pagination"><li class="active"><button class="page" type="button" data-i="1" data-page="5">1</button></li><li><button class="page" type="button" data-i="2" data-page="5">2</button></li><li><button class="page" type="button" data-i="3" data-page="5">3</button></li><li class="disabled"><button class="page" type="button">...</button></li></ul><button class="page-link pe-0" data-list-pagination="next"><svg class="svg-inline--fa fa-chevron-right" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="chevron-right" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512" data-fa-i2svg=""><path fill="currentColor" d="M310.6 233.4c12.5 12.5 12.5 32.8 0 45.3l-192 192c-12.5 12.5-32.8 12.5-45.3 0s-12.5-32.8 0-45.3L242.7 256 73.4 86.6c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0l192 192z"></path></svg><!-- <span class="fas fa-chevron-right"></span> Font Awesome fontawesome.com --></button>
                          </div>
                        </div>
                      </div>
                    

            </div>

        </div>
    </main>

</asp:Content>
