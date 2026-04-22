<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BibliotecaDigital.Web.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    <style>
        /* =====================================================
           CABECERA Y BOTONES RÁPIDOS
           ===================================================== */

        .btn-acceso {
            min-width: 220px;
            min-height: 56px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 10px;
            border-radius: 14px;
            font-weight: 500;
            font-size: 1rem;
            text-decoration: none;
            transition: all 0.25s ease;
            box-shadow: 0 6px 16px rgba(0, 0, 0, 0.08);
        }

        .btn-acceso i {
            font-size: 1.2rem;
        }

        .btn-acceso:hover {
            transform: translateY(-3px);
            box-shadow: 0 12px 22px rgba(0, 0, 0, 0.14);
        }

        .btn-acceso:focus {
            box-shadow: 0 0 0 .2rem rgba(13, 110, 253, 0.2);
        }

        /* =====================================================
           TARJETAS DE LIBROS
           ===================================================== */

        .card-libro {
            border-radius: 14px;
            border: 1px solid #e6e6e6;
            transition: all 0.35s ease;
            overflow: hidden;
            background: #ffffff;
            animation: fadeInUp 0.6s ease;
        }

        .card-libro:hover {
            transform: translateY(-10px) scale(1.02);
            box-shadow: 0 18px 35px rgba(0, 0, 0, 0.18);
            border-color: #0d6efd;
        }

        .card-libro .card-title {
            font-weight: 700;
            font-size: 1.2rem;
            transition: color 0.3s ease;
        }

        .card-libro:hover .card-title {
            color: #0d6efd;
        }

        .card-libro .card-text {
            font-size: 0.95rem;
        }

        .bloque-filtro {
            border: 1px solid rgba(0,0,0,.08);
            border-radius: 12px;
            padding: 14px;
            background: rgba(255,255,255,.65);
            backdrop-filter: blur(4px);
        }

        .acciones-libro .btn {
            min-width: 110px;
            transition: all 0.25s ease;
        }

        .acciones-libro .btn:hover {
            transform: translateY(-2px);
        }

        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(15px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        /* =====================================================
           MODO OSCURO
           ===================================================== */

        [data-bs-theme="dark"] .card-libro {
            background-color: #1e1e1e;
            border-color: #333;
        }

        [data-bs-theme="dark"] .card-libro .card-title {
            color: #ffffff;
        }

        [data-bs-theme="dark"] .card-libro .card-text,
        [data-bs-theme="dark"] .text-muted {
            color: #cfcfcf !important;
        }

        [data-bs-theme="dark"] .card-libro:hover {
            border-color: #4da3ff;
            box-shadow: 0 18px 35px rgba(0, 0, 0, 0.55);
        }

        [data-bs-theme="dark"] .bloque-filtro {
            background: rgba(30,30,30,.8);
            border-color: rgba(255,255,255,.08);
        }

        [data-bs-theme="dark"] .btn-acceso {
            color: #ffffff !important;
        }

        [data-bs-theme="dark"] .btn-acceso i {
            color: #ffffff !important;
        }

        [data-bs-theme="dark"] .btn-success.btn-acceso {
            background-color: #198754 !important;
            border-color: #198754 !important;
        }

        [data-bs-theme="dark"] .btn-outline-primary.btn-acceso,
        [data-bs-theme="dark"] .btn-dark.btn-acceso {
            color: #ffffff !important;
        }

        [data-bs-theme="dark"] .btn-outline-primary.btn-acceso {
            border-color: #4d6bfe !important;
            background-color: rgba(77, 107, 254, 0.05) !important;
        }

        [data-bs-theme="dark"] .btn-outline-primary.btn-acceso:hover {
            background-color: rgba(77, 107, 254, 0.18) !important;
            border-color: #6a84ff !important;
        }

        [data-bs-theme="dark"] .btn-dark.btn-acceso {
            background-color: #23272b !important;
            border-color: #343a40 !important;
        }

        [data-bs-theme="dark"] .btn-dark.btn-acceso:hover {
            background-color: #2c3136 !important;
            border-color: #495057 !important;
        }
    </style>

    <div class="card shadow-sm p-4">

        <div class="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-3">
            <div>
                <h2 class="mb-1">Biblioteca Digital</h2>
                <p class="text-muted mb-0">
                    Bienvenido: <%: Session["Nombre"] %> |
                    Rol: <%: Session["Rol"] %>
                </p>
            </div>

            <asp:Button ID="btnLogout" runat="server"
                Text="Cerrar sesión"
                CssClass="btn btn-outline-danger"
                OnClick="btnLogout_Click" />
        </div>

        <hr />

        <asp:Panel ID="pnlAdminLibros" runat="server" Visible="false" CssClass="mb-3 d-flex gap-3 flex-wrap">

            <a href="AdminLibros.aspx" class="btn btn-success btn-acceso">
                <i class="bi bi-upload me-1"></i>Administrar libros
            </a>

            <a href="Dashboard.aspx" class="btn btn-outline-primary btn-acceso">
                <i class="bi bi-bar-chart-line me-1"></i>Dashboard
            </a>

        </asp:Panel>

        <asp:Panel ID="pnlAdminUsuarios" runat="server" Visible="false" CssClass="mb-4 d-flex gap-3 flex-wrap">
            <a href="AdminUsuarios.aspx" class="btn btn-dark btn-acceso">
                <i class="bi bi-people me-1"></i>Administrar usuarios
            </a>
        </asp:Panel>

        <div class="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
            <h4 class="mb-0">Catálogo de libros</h4>
            <span class="text-muted small">Consulta, abre o descarga archivos</span>
        </div>

        <div class="bloque-filtro mb-4">
            <div class="row g-3">

                <div class="col-md-8">
                    <label for="txtBuscar" class="form-label">
                        <i class="bi bi-search me-1"></i>Buscar
                    </label>
                    <input type="text"
                           id="txtBuscar"
                           class="form-control"
                           placeholder="Buscar por libro, autor, ISBN o categoría..." />
                </div>

                <div class="col-md-4">
                    <label for="ddlCategoriaFiltro" class="form-label">
                        <i class="bi bi-funnel me-1"></i>Categoría
                    </label>
                    <select id="ddlCategoriaFiltro" class="form-select">
                        <option value="">Todas las categorías</option>
                    </select>
                </div>

            </div>
        </div>

        <asp:Repeater ID="rptLibros" runat="server">

            <HeaderTemplate>
                <div class="row g-4" id="contenedorLibros">
            </HeaderTemplate>

            <ItemTemplate>
                <div class="col-md-4 libro-item"
                    data-titulo='<%# Eval("Titulo") %>'
                    data-autor='<%# Eval("Autor") %>'
                    data-categoria='<%# Eval("Categoria") %>'
                    data-isbn='<%# Eval("ISBN") %>'>

                    <div class="card card-libro h-100 shadow-sm">
                        <div class="card-body d-flex flex-column">

                            <h5 class="card-title mb-3">
                                <%# Eval("Titulo") %>
                            </h5>

                            <p class="card-text mb-2">
                                <strong>Autor:</strong>
                                <%# Eval("Autor") %>
                            </p>

                            <p class="card-text mb-2">
                                <small class="text-muted">
                                    <i class="bi bi-bookmark me-1"></i>
                                    Categoría: <%# Eval("Categoria") %>
                                </small>
                            </p>

                            <p class="card-text mb-2">
                                <small class="text-muted">
                                    <i class="bi bi-upc-scan me-1"></i>
                                    ISBN: <%# Eval("ISBN") %>
                                </small>
                            </p>

                            <p class="card-text mb-3">
                                <small class="text-muted">
                                    <i class="bi bi-calendar-event me-1"></i>
                                    Fecha: <%# Eval("FechaPublicacion", "{0:dd/MM/yyyy}") %>
                                </small>
                            </p>

                            <div class="acciones-libro mt-auto d-flex gap-2 flex-wrap">
                                <a href='https://localhost:44341/api/libros/abrir/<%# Eval("IdLibro") %>'
                                   class="btn btn-outline-primary btn-sm"
                                   target="_blank">
                                    <i class="bi bi-eye me-1"></i>Abrir
                                </a>

                                <a href='https://localhost:44341/api/libros/descargar/<%# Eval("IdLibro") %>'
                                   class="btn btn-primary btn-sm">
                                    <i class="bi bi-download me-1"></i>Descargar
                                </a>
                            </div>

                        </div>
                    </div>
                </div>
            </ItemTemplate>

            <FooterTemplate>
                </div>
            </FooterTemplate>

        </asp:Repeater>

        <asp:Label ID="lblSinLibros" runat="server" CssClass="text-muted d-block mt-3"></asp:Label>

        <div id="sinResultados" class="text-muted mt-3" style="display:none;">
            No se encontraron libros con esos criterios.
        </div>

    </div>

    <script>
        document.addEventListener("DOMContentLoaded", function () {

            const txtBuscar = document.getElementById("txtBuscar");
            const ddlCategoriaFiltro = document.getElementById("ddlCategoriaFiltro");
            const libros = document.querySelectorAll(".libro-item");
            const sinResultados = document.getElementById("sinResultados");

            function cargarCategorias() {
                const categorias = new Set();

                libros.forEach(libro => {
                    const categoria = (libro.dataset.categoria || "").trim();
                    if (categoria) {
                        categorias.add(categoria);
                    }
                });

                const categoriasOrdenadas = Array.from(categorias).sort();

                categoriasOrdenadas.forEach(categoria => {
                    const option = document.createElement("option");
                    option.value = categoria.toLowerCase();
                    option.textContent = categoria;
                    ddlCategoriaFiltro.appendChild(option);
                });
            }

            function filtrarLibros() {
                const texto = txtBuscar.value.toLowerCase().trim();
                const categoriaSeleccionada = ddlCategoriaFiltro.value.toLowerCase().trim();

                let visibles = 0;

                libros.forEach(libro => {
                    const titulo = (libro.dataset.titulo || "").toLowerCase();
                    const autor = (libro.dataset.autor || "").toLowerCase();
                    const categoria = (libro.dataset.categoria || "").toLowerCase();
                    const isbn = (libro.dataset.isbn || "").toLowerCase();

                    const coincideTexto =
                        titulo.includes(texto) ||
                        autor.includes(texto) ||
                        categoria.includes(texto) ||
                        isbn.includes(texto);

                    const coincideCategoria =
                        categoriaSeleccionada === "" || categoria === categoriaSeleccionada;

                    if (coincideTexto && coincideCategoria) {
                        libro.style.display = "";
                        visibles++;
                    } else {
                        libro.style.display = "none";
                    }
                });

                sinResultados.style.display = visibles === 0 ? "block" : "none";
            }

            cargarCategorias();
            txtBuscar.addEventListener("input", filtrarLibros);
            ddlCategoriaFiltro.addEventListener("change", filtrarLibros);
        });
    </script>

</asp:Content>