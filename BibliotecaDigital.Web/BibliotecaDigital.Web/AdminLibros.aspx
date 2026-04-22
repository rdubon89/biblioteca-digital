<%@ Page Title="Administrar Libros" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminLibros.aspx.cs" Inherits="BibliotecaDigital.Web.AdminLibros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Iconos Bootstrap -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    <style>
        .panel-card {
            border: none;
            border-radius: 18px;
            box-shadow: 0 12px 28px rgba(0,0,0,0.10);
            overflow: hidden;
        }

        .panel-titulo {
            font-weight: 700;
            margin-bottom: .25rem;
        }

        .panel-subtitulo {
            color: #6c757d;
            margin-bottom: 0;
        }

        .form-label {
            font-weight: 600;
        }

        .btn-accion {
            min-width: 140px;
        }

        .seccion-icono {
            font-size: 1.2rem;
        }

        .tabla-libros th {
            background-color: rgba(13, 110, 253, 0.08);
            font-weight: 600;
            vertical-align: middle;
        }

        [data-bs-theme="dark"] .panel-card {
            background-color: #1e1e1e;
            color: #f1f1f1;
            box-shadow: 0 12px 28px rgba(0,0,0,0.35);
        }

        [data-bs-theme="dark"] .panel-subtitulo,
        [data-bs-theme="dark"] .text-muted {
            color: #cfcfcf !important;
        }

        [data-bs-theme="dark"] .table {
            color: #f1f1f1;
        }

        [data-bs-theme="dark"] .tabla-libros th {
            background-color: rgba(77, 163, 255, 0.15);
            color: #ffffff;
        }

        [data-bs-theme="dark"] .table-striped > tbody > tr:nth-of-type(odd) > * {
            color: #f1f1f1;
            background-color: rgba(255,255,255,0.03);
        }
    </style>

    <!-- =====================================================
         CABECERA
         ===================================================== -->
    <div class="d-flex justify-content-between align-items-center mb-4 flex-wrap gap-2">
        <div>
            <h2 class="mb-1">Administración de Libros</h2>
            <p class="panel-subtitulo">
                Gestión de libros, categorías y archivos del sistema.
            </p>
        </div>

        <div class="d-flex gap-2 flex-wrap">
            <a href="Home.aspx" class="btn btn-outline-secondary">
                <i class="bi bi-house-door me-1"></i>Inicio
            </a>

            <a href="Dashboard.aspx" class="btn btn-outline-primary">
                <i class="bi bi-bar-chart-line me-1"></i>Dashboard
            </a>
        </div>
    </div>

    <!-- =====================================================
         FORMULARIO
         ===================================================== -->
    <div class="card panel-card p-4 mb-4">
        <div class="d-flex align-items-center gap-2 mb-3">
            <i class="bi bi-book-half seccion-icono text-primary"></i>
            <div>
                <h5 class="panel-titulo">Registro y edición de libros</h5>
                <p class="panel-subtitulo">Permite registrar nuevos libros o editar libros existentes.</p>
            </div>
        </div>

        <asp:HiddenField ID="hfIdLibro" runat="server" />

        <div class="row g-3">
            <div class="col-md-4">
                <label class="form-label">Título</label>
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" placeholder="Ingrese el título"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">Autor</label>
                <asp:TextBox ID="txtAutor" runat="server" CssClass="form-control" placeholder="Ingrese el autor"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">ISBN</label>
                <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control" placeholder="Ingrese el ISBN"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">Fecha de publicación</label>
                <asp:TextBox ID="txtFechaPublicacion" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">Categoría</label>
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" onchange="toggleNuevaCategoria()"></asp:DropDownList>
            </div>

            <div class="col-md-4">
                <label class="form-label">Archivo</label>
                <asp:FileUpload ID="fuLibro" runat="server" CssClass="form-control" />
            </div>

            <!-- NUEVA CATEGORÍA -->
            <div class="col-md-4" id="bloqueNuevaCategoria" style="display:none;">
                <label class="form-label">Nueva categoría</label>
                <asp:TextBox ID="txtNuevaCategoria" runat="server" CssClass="form-control"
                    placeholder="Ingrese la nueva categoría"></asp:TextBox>
            </div>

            <div class="col-md-8" id="bloqueNuevaRuta" style="display:none;">
                <label class="form-label">Ruta física de la categoría</label>
                <asp:TextBox ID="txtNuevaRutaFisica" runat="server" CssClass="form-control"
                    placeholder="Ejemplo: C:\rutas\nuevacategoria"></asp:TextBox>
            </div>

            <div class="col-md-12 d-flex gap-2 flex-wrap mt-2">
                <asp:Button ID="btnSubir" runat="server"
                    Text="Guardar libro"
                    CssClass="btn btn-success btn-accion"
                    OnClick="btnSubir_Click" />

                <asp:Button ID="btnActualizar" runat="server"
                    Text="Actualizar libro"
                    CssClass="btn btn-warning btn-accion"
                    Visible="false"
                    OnClick="btnActualizar_Click" />

                <asp:Button ID="btnCancelar" runat="server"
                    Text="Cancelar"
                    CssClass="btn btn-secondary btn-accion"
                    Visible="false"
                    OnClick="btnCancelar_Click" />
            </div>

            <div class="col-md-12">
                <asp:Label ID="lblMensaje" runat="server" CssClass="d-block mt-2"></asp:Label>
            </div>
        </div>
    </div>

    <!-- =====================================================
         TABLA DE LIBROS
         ===================================================== -->
    <div class="card panel-card p-4">
        <div class="d-flex align-items-center gap-2 mb-3">
            <i class="bi bi-journal-richtext seccion-icono text-success"></i>
            <div>
                <h5 class="panel-titulo">Libros registrados</h5>
                <p class="panel-subtitulo">Listado general de libros disponibles en el sistema.</p>
            </div>
        </div>

        <asp:GridView ID="gvLibros" runat="server"
            CssClass="table table-striped table-bordered tabla-libros"
            AutoGenerateColumns="false"
            GridLines="None"
            DataKeyNames="IdLibro"
            OnRowCommand="gvLibros_RowCommand">
            <Columns>

                <asp:BoundField DataField="Titulo" HeaderText="Título" />
                <asp:BoundField DataField="Autor" HeaderText="Autor" />
                <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                <asp:BoundField DataField="FechaPublicacion" HeaderText="Fecha Publicación" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />

                <asp:TemplateField HeaderText="Acción">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server"
                            CssClass="btn btn-sm btn-warning"
                            CommandName="EditarLibro"
                            CommandArgument='<%# Eval("IdLibro") %>'>
                            <i class="bi bi-pencil-square me-1"></i>Editar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnEliminar" runat="server"
                            CssClass="btn btn-sm btn-danger ms-1"
                            CommandName="EliminarLibro"
                            CommandArgument='<%# Eval("IdLibro") %>'
                            OnClientClick="return confirm('¿Está seguro de eliminar este libro?');">
                            <i class="bi bi-trash me-1"></i>Eliminar
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>

    <!-- =====================================================
         SCRIPT PARA NUEVA CATEGORÍA
         ===================================================== -->
    <script>
        function toggleNuevaCategoria() {
            const ddl = document.getElementById('<%= ddlCategoria.ClientID %>');
            const bloqueCategoria = document.getElementById('bloqueNuevaCategoria');
            const bloqueRuta = document.getElementById('bloqueNuevaRuta');

            if (ddl && ddl.value === "-1") {
                bloqueCategoria.style.display = "";
                bloqueRuta.style.display = "";
            } else {
                bloqueCategoria.style.display = "none";
                bloqueRuta.style.display = "none";
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            toggleNuevaCategoria();
        });
    </script>

</asp:Content>