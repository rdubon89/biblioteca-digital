<%@ Page Title="Administrar Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminUsuarios.aspx.cs" Inherits="BibliotecaDigital.Web.AdminUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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

        .tabla-usuarios th {
            background-color: rgba(13, 110, 253, 0.08);
            font-weight: 600;
            vertical-align: middle;
        }

        .badge-rol {
            font-size: .82rem;
            padding: .45rem .7rem;
            border-radius: 999px;
        }

        .bloque-filtro {
            border: 1px solid rgba(0,0,0,.08);
            border-radius: 12px;
            padding: 14px;
            background: rgba(255,255,255,.65);
            backdrop-filter: blur(4px);
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

        [data-bs-theme="dark"] .tabla-usuarios th {
            background-color: rgba(77, 163, 255, 0.15);
            color: #ffffff;
        }

        [data-bs-theme="dark"] .table-striped > tbody > tr:nth-of-type(odd) > * {
            color: #f1f1f1;
            background-color: rgba(255,255,255,0.03);
        }

        [data-bs-theme="dark"] .bloque-filtro {
            background: rgba(30,30,30,.8);
            border-color: rgba(255,255,255,.08);
        }
    </style>

    <div class="d-flex justify-content-between align-items-center mb-4 flex-wrap gap-2">
        <div>
            <h2 class="mb-1">Administración de Usuarios</h2>
            <p class="panel-subtitulo">
                Gestión visual y control de cuentas del sistema.
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
         BÚSQUEDA Y FILTROS (ARRIBA)
         ===================================================== -->
    <div class="card panel-card p-4 mb-4">
        <div class="d-flex align-items-center gap-2 mb-3">
            <i class="bi bi-search seccion-icono text-info"></i>
            <div>
                <h5 class="panel-titulo">Búsqueda y filtros</h5>
                <p class="panel-subtitulo">Busque usuarios por nombre, correo o rol.</p>
            </div>
        </div>

        <div class="bloque-filtro">
            <div class="row g-3">
                <div class="col-md-6">
                    <label class="form-label">Buscar por nombre o correo</label>
                    <input type="text" id="txtBuscarUsuario" class="form-control"
                           placeholder="Escriba nombre o correo..." />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Filtrar por rol</label>
                    <select id="ddlFiltroRol" class="form-select">
                        <option value="">Todos los roles</option>
                        <option value="administrador">Administrador</option>
                        <option value="bibliotecario">Bibliotecario</option>
                        <option value="ejecutivo">Ejecutivo</option>
                        <option value="user">User</option>
                    </select>
                </div>

                <div class="col-md-2 d-flex align-items-end">
                    <button type="button" id="btnLimpiarFiltroUsuarios" class="btn btn-secondary w-100">
                        Limpiar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- =====================================================
         NUEVO USUARIO
         ===================================================== -->
    <asp:Panel ID="pnlNuevoUsuario" runat="server" Visible="false" CssClass="card panel-card p-4 mb-4">
        <div class="d-flex align-items-center gap-2 mb-3">
            <i class="bi bi-person-plus seccion-icono text-success"></i>
            <div>
                <h5 class="panel-titulo">Nuevo usuario</h5>
                <p class="panel-subtitulo">Registro rápido de nuevas cuentas.</p>
            </div>
        </div>

        <div class="row g-3">
            <div class="col-md-4">
                <label class="form-label">Nombre completo</label>
                <asp:TextBox ID="txtNuevoNombre" runat="server" CssClass="form-control" placeholder="Ingrese el nombre"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">Correo electrónico</label>
                <asp:TextBox ID="txtNuevoCorreo" runat="server" CssClass="form-control" placeholder="Ingrese el correo"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">Contraseña</label>
                <asp:TextBox ID="txtNuevoPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">Rol del usuario</label>
                <asp:DropDownList ID="ddlNuevoRol" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>

            <div class="col-md-12 d-flex gap-2 flex-wrap mt-2">
                <asp:Button ID="btnAgregarUsuario" runat="server"
                    Text="Agregar usuario"
                    CssClass="btn btn-success btn-accion"
                    OnClick="btnAgregarUsuario_Click" />
            </div>

            <div class="col-md-12">
                <asp:Label ID="lblMensajeNuevoUsuario" runat="server" CssClass="d-block mt-2"></asp:Label>
            </div>
        </div>
    </asp:Panel>

    <!-- =====================================================
         EDICIÓN DE USUARIO
         ===================================================== -->
    <div class="card panel-card p-4 mb-4">
        <div class="d-flex align-items-center gap-2 mb-3">
            <i class="bi bi-person-gear seccion-icono text-primary"></i>
            <div>
                <h5 class="panel-titulo">Edición de usuario</h5>
                <p class="panel-subtitulo">Seleccione un usuario desde la tabla para cargar sus datos.</p>
            </div>
        </div>

        <asp:HiddenField ID="hfIdUsuario" runat="server" />

        <div class="row g-3">
            <div class="col-md-4">
                <label class="form-label">Nombre completo</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ingrese el nombre"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">Correo electrónico</label>
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Ingrese el correo"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <label class="form-label">Rol del usuario</label>
                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>

            <div class="col-md-12 d-flex gap-2 flex-wrap mt-2">
                <asp:Button ID="btnGuardarCambios" runat="server"
                    Text="Guardar cambios"
                    CssClass="btn btn-warning btn-accion"
                    Visible="false"
                    OnClick="btnGuardarCambios_Click" />

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
         TABLA DE USUARIOS
         ===================================================== -->
    <div class="card panel-card p-4">
        <div class="d-flex align-items-center gap-2 mb-3">
            <i class="bi bi-people-fill seccion-icono text-success"></i>
            <div>
                <h5 class="panel-titulo">Usuarios registrados</h5>
                <p class="panel-subtitulo">Listado general de cuentas creadas en el sistema.</p>
            </div>
        </div>

        <asp:GridView ID="gvUsuarios" runat="server"
            CssClass="table table-striped table-bordered tabla-usuarios"
            AutoGenerateColumns="false"
            GridLines="None"
            DataKeyNames="IdUsuario"
            OnRowCommand="gvUsuarios_RowCommand"
            ClientIDMode="Static">
            <Columns>

                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>
                        <span class="dato-nombre"><%# Eval("Nombre") %></span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Correo electrónico">
                    <ItemTemplate>
                        <span class="dato-correo"><%# Eval("Correo") %></span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="FechaCreacion" HeaderText="Fecha de creación" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="UltimoAcceso" HeaderText="Último acceso" DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                <asp:TemplateField HeaderText="Rol">
                    <ItemTemplate>
                        <span class="badge bg-primary badge-rol dato-rol">
                            <%# Eval("Rol") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Acción">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server"
                            CssClass="btn btn-sm btn-warning"
                            CommandName="EditarUsuario"
                            CommandArgument='<%# Eval("IdUsuario") %>'>
                            <i class="bi bi-pencil-square me-1"></i>Editar
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnEliminar" runat="server"
                            CssClass="btn btn-sm btn-danger ms-1"
                            CommandName="EliminarUsuario"
                            CommandArgument='<%# Eval("IdUsuario") %>'
                            OnClientClick="return confirm('¿Está seguro de eliminar este usuario?');">
                            <i class="bi bi-trash me-1"></i>Eliminar
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const txtBuscar = document.getElementById("txtBuscarUsuario");
            const ddlRol = document.getElementById("ddlFiltroRol");
            const btnLimpiar = document.getElementById("btnLimpiarFiltroUsuarios");
            const tabla = document.getElementById("gvUsuarios");

            if (!tabla) return;

            function obtenerFilasTabla() {
                let filas = tabla.querySelectorAll("tbody tr");

                if (!filas || filas.length === 0) {
                    filas = tabla.querySelectorAll("tr");
                }

                return Array.from(filas).filter(fila => {
                    return fila.querySelector(".dato-nombre") !== null;
                });
            }

            function filtrarUsuarios() {
                const texto = (txtBuscar.value || "").toLowerCase().trim();
                const rolSeleccionado = (ddlRol.value || "").toLowerCase().trim();

                const filas = obtenerFilasTabla();

                filas.forEach(fila => {
                    const nombre = (fila.querySelector(".dato-nombre")?.textContent || "").toLowerCase().trim();
                    const correo = (fila.querySelector(".dato-correo")?.textContent || "").toLowerCase().trim();
                    const rol = (fila.querySelector(".dato-rol")?.textContent || "").toLowerCase().trim();

                    const coincideTexto =
                        texto === "" ||
                        nombre.includes(texto) ||
                        correo.includes(texto);

                    const coincideRol =
                        rolSeleccionado === "" ||
                        rol.includes(rolSeleccionado);

                    fila.style.display = (coincideTexto && coincideRol) ? "" : "none";
                });
            }

            if (txtBuscar) txtBuscar.addEventListener("input", filtrarUsuarios);
            if (ddlRol) ddlRol.addEventListener("change", filtrarUsuarios);

            if (btnLimpiar) {
                btnLimpiar.addEventListener("click", function () {
                    txtBuscar.value = "";
                    ddlRol.selectedIndex = 0;
                    filtrarUsuarios();
                });
            }
        });
    </script>

</asp:Content>