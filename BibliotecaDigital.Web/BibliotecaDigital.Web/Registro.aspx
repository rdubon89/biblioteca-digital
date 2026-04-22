<%@ Page Title="Registro de Usuario" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="BibliotecaDigital.Web.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Contenedor centrado vertical y horizontalmente -->
    <div class="container vh-100 d-flex justify-content-center align-items-center">

        <!-- Tarjeta de registro -->
        <div class="card shadow p-4" style="width:450px;">

            <h3 class="text-center mb-4">Registro de Usuario</h3>

            <!-- Campo Nombre -->
            <div class="mb-3">
                <label class="form-label">Nombre</label>
                <asp:TextBox 
                    ID="txtNombre" 
                    runat="server" 
                    CssClass="form-control">
                </asp:TextBox>
            </div>

            <!-- Campo Correo -->
            <div class="mb-3">
                <label class="form-label">Correo electrónico</label>
                <asp:TextBox 
                    ID="txtCorreo" 
                    runat="server" 
                    CssClass="form-control">
                </asp:TextBox>
            </div>

            <!-- Campo Contraseña -->
            <div class="mb-3">
                <label class="form-label">Contraseña</label>
                <asp:TextBox 
                    ID="txtPassword" 
                    runat="server" 
                    TextMode="Password" 
                    CssClass="form-control">
                </asp:TextBox>
            </div>

            <!-- Botón Registrar -->
            <asp:Button 
                ID="btnRegistrar" 
                runat="server"
                Text="Registrarme"
                CssClass="btn btn-success w-100"
                OnClick="btnRegistrar_Click" />

            <!-- Enlace para volver al login -->
            <div class="text-center mt-3">
                <a href="Login.aspx" class="text-decoration-none">
                    Volver al login
                </a>
            </div>

            <!-- Mensajes del sistema -->
            <asp:Label 
                ID="lblMensaje" 
                runat="server" 
                CssClass="text-danger mt-3 d-block">
            </asp:Label>

        </div>
    </div>

</asp:Content>