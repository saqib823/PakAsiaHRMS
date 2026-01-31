<%@ Page Title="Verify User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerifyUser.aspx.cs" Inherits="hrms_PakAsia.Pages.VerifyUser" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main class="main" id="top">
        <div class="container">
            <div class="row flex-center min-vh-100 py-5">
                <div class="col-sm-10 col-md-8 col-lg-5 col-xl-5 col-xxl-3">
                    <a class="d-flex flex-center text-decoration-none mb-4" href="../../../index.html">
                        <div class="d-flex align-items-center fw-bolder fs-3 d-inline-block">
                            <img runat="server" src="~/assets/img/icons/logo.png" alt="phoenix" width="100" /></div>
                    </a>
                    <div class="text-center mb-7">
                        <h3 class="text-body-highlight">Verify Account</h3>
                        <p class="text-body-tertiary">2 factor authentication to get access to your account</p>
                    </div>


                    <div class="mb-3 text-start">
                        <label class="form-label" for="email">Enter OTP:</label>
                        <div class="form-icon-container">
                            <asp:TextBox runat="server" CssClass="form-control form-icon-input" ID="otp"  placeholder="Enter OTP" /><span class="fas fa-user text-body fs-9 form-icon"></span></div>
                    </div>
                  
                    <asp:Button runat="server" ID="btnVerify" OnClick="btnVerify_Click" CssClass="btn btn-primary w-100 mb-3" Text="Verify"/>
                </div>
                                        <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>

            </div>
        </div>
        
    </main>
  

</asp:Content>

