<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BibliotecaDigital.Web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container vh-100 d-flex justify-content-center align-items-center">
        <div class="card shadow p-4" style="width: 400px;">

            <h3 class="text-center mb-4">Biblioteca Digital</h3>

            <div class="mb-3">
                <label class="form-label">Correo</label>
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label class="form-label">Contraseña</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            </div>

            <asp:Button ID="btnLogin" runat="server"
                Text="Iniciar Sesión"
                CssClass="btn btn-primary w-100"
                OnClick="btnLogin_Click" />

            <div class="text-center mt-3">
                <a href="Registro.aspx" class="text-decoration-none">Regístrate</a>
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger mt-3 d-block"></asp:Label>

        </div>
    </div>

</asp:Content>