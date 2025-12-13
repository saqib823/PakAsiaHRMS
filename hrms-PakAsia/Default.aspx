<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="hrms_PakAsia._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main class="main" id="top">
        <div class="container">
            <div class="row flex-center min-vh-100 py-5">
                <div class="col-sm-10 col-md-8 col-lg-5 col-xl-5 col-xxl-3">
                    <a class="d-flex flex-center text-decoration-none mb-4" href="../../../index.html">
                        <div class="d-flex align-items-center fw-bolder fs-3 d-inline-block">
                            <img src="../../../assets/img/icons/logo.png" alt="phoenix" width="100" /></div>
                    </a>
                    <div class="text-center mb-7">
                        <h3 class="text-body-highlight">Sign In</h3>
                        <p class="text-body-tertiary">Get access to your account</p>
                    </div>


                    <div class="mb-3 text-start">
                        <label class="form-label" for="email">Email address</label>
                        <div class="form-icon-container">
                            <asp:TextBox runat="server" CssClass="form-control form-icon-input" ID="email" TextMode="Email" placeholder="name@example.com" /><span class="fas fa-user text-body fs-9 form-icon"></span></div>
                    </div>
                    <div class="mb-3 text-start">
                        <label class="form-label" for="password">Password</label>
                        <div class="form-icon-container" data-password="data-password">
                            <asp:TextBox CssClass="form-control form-icon-input pe-6" runat="server" ID="password" TextMode="Password" placeholder="Password" data-password-input="data-password-input" />
                            <button type="button"  class="btn px-3 py-0 h-100 position-absolute top-0 end-0 fs-7 text-body-tertiary" data-password-toggle="data-password-toggle"><span class="uil uil-eye show"></span><span class="uil uil-eye-slash hide"></span></button>
                            <span class="fas fa-key text-body fs-9 form-icon"></span>
                        </div>
                    </div>
                    <div class="row flex-between-center mb-7">
                        <div class="col-auto">
                            <div class="form-check mb-0">
                                <input class="form-check-input" id="basic-checkbox" type="checkbox" checked="checked" /><label class="form-check-label mb-0" for="basic-checkbox">Remember me</label></div>
                        </div>
                        <div class="col-auto"><a class="fs-9 fw-semibold" href="forgot-password.html">Forgot Password?</a></div>
                    </div>
                    <asp:Button runat="server" ID="btnSignIn" OnClick="btnSignIn_Click" CssClass="btn btn-primary w-100 mb-3" Text="Sign In"/>
                </div>
                                        <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>

            </div>
        </div>
        
    </main>
  

</asp:Content>
