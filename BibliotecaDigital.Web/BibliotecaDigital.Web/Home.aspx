<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BibliotecaDigital.Web.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    <style>
        .home-header {
            margin-bottom: 1.5rem;
        }

        .home-title {
            font-weight: 800;
            letter-spacing: -0.04em;
        }

        .home-subtitle {
            color: #6c757d;
            font-size: .95rem;
        }

        .flat-card {
            border: 1px solid rgba(0,0,0,.06);
            border-radius: 18px;
            background: #ffffff;
            box-shadow: 0 8px 22px rgba(0,0,0,.04);
        }

        .quick-card {
            text-decoration: none;
            color: inherit;
            padding: 1.25rem;
            transition: transform .2s ease, box-shadow .2s ease;
            display: block;
            height: 100%;
        }

        .quick-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 14px 28px rgba(0,0,0,.08);
            color: inherit;
        }

        .quick-icon {
            width: 46px;
            height: 46px;
            border-radius: 14px;
            display: grid;
            place-items: center;
            font-size: 1.35rem;
            background: rgba(13,110,253,.08);
            color: #0d6efd;
            margin-bottom: .9rem;
        }

        .quick-title {
            font-weight: 700;
            margin-bottom: .25rem;
        }

        .quick-text {
            color: #6c757d;
            font-size: .9rem;
            margin-bottom: 0;
        }

        .filter-card {
            padding: 1rem;
            margin-bottom: 1.5rem;
        }

        .catalog-title {
            font-weight: 700;
            letter-spacing: -0.02em;
        }

        .book-card {
            border: 1px solid rgba(0,0,0,.06);
            border-radius: 18px;
            background: #ffffff;
            box-shadow: 0 8px 20px rgba(0,0,0,.035);
            transition: transform .2s ease, box-shadow .2s ease, border-color .2s ease;
        }

        .book-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 14px 30px rgba(0,0,0,.075);
            border-color: rgba(13,110,253,.35);
        }

        .book-card-body {
            padding: 1rem;
            display: flex;
            flex-direction: column;
            height: 100%;
        }

        .book-badge {
            width: 42px;
            height: 42px;
            border-radius: 14px;
            display: grid;
            place-items: center;
            background: rgba(13,110,253,.08);
            color: #0d6efd;
            flex-shrink: 0;
        }

        .book-title {
            font-size: 1.05rem;
            font-weight: 800;
            line-height: 1.2;
            letter-spacing: -0.02em;
            color: #111827;
        }

        .book-author {
            font-size: .9rem;
            color: #6c757d;
        }

        .book-info {
            display: grid;
            gap: .45rem;
            margin-bottom: 1rem;
            color: #6c757d;
            font-size: .9rem;
        }

        .book-info div {
            display: flex;
            align-items: center;
            gap: .5rem;
        }

        .book-info i {
            color: #0d6efd;
            font-size: .95rem;
        }

        .book-actions {
            margin-top: auto;
            display: flex;
            gap: .5rem;
            flex-wrap: wrap;
        }

        .book-actions .btn {
            border-radius: 999px;
            padding: .35rem .8rem;
            font-size: .85rem;
        }

        .empty-state {
            border-radius: 18px;
            border: 1px solid rgba(255,193,7,.25);
            background: rgba(255,193,7,.08);
            padding: 1rem;
        }

        [data-bs-theme="dark"] .flat-card,
        [data-bs-theme="dark"] .book-card {
            background: #1e1e1e;
            border-color: rgba(255,255,255,.08);
            box-shadow: 0 8px 22px rgba(0,0,0,.25);
        }

        [data-bs-theme="dark"] .home-subtitle,
        [data-bs-theme="dark"] .quick-text,
        [data-bs-theme="dark"] .book-author,
        [data-bs-theme="dark"] .book-info,
        [data-bs-theme="dark"] .text-muted {
            color: #adb5bd !important;
        }

        [data-bs-theme="dark"] .book-title {
            color: #f8f9fa;
        }

        [data-bs-theme="dark"] .quick-card {
            color: #f8f9fa;
        }

        [data-bs-theme="dark"] .quick-card:hover {
            color: #ffffff;
        }
    </style>

    <div class="home-header">
        <h2 class="home-title mb-1">Biblioteca Digital</h2>
        <p class="home-subtitle mb-0">
            Bienvenido, <%: Session["Nombre"] %> · Rol: <%: Session["Rol"] %>
        </p>
    </div>

    <div class="row g-3 mb-4">

        <asp:PlaceHolder ID="phDashboard" runat="server" Visible="false">
            <div class="col-md-4">
                <a href="Dashboard.aspx" class="flat-card quick-card">
                    <div class="quick-icon">
                        <i class="bi bi-bar-chart-line"></i>
                    </div>
                    <div class="quick-title">Dashboard</div>
                    <p class="quick-text">Consulta indicadores y actividad reciente del sistema.</p>
                </a>
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="phAdminLibros" runat="server" Visible="false">
            <div class="col-md-4">
                <a href="AdminLibros.aspx" class="flat-card quick-card">
                    <div class="quick-icon">
                        <i class="bi bi-cloud-upload"></i>
                    </div>
                    <div class="quick-title">Administrar libros</div>
                    <p class="quick-text">Carga, edita y organiza documentos digitales.</p>
                </a>
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="phAdminUsuarios" runat="server" Visible="false">
            <div class="col-md-4">
                <a href="AdminUsuarios.aspx" class="flat-card quick-card">
                    <div class="quick-icon">
                        <i class="bi bi-people"></i>
                    </div>
                    <div class="quick-title">Administrar usuarios</div>
                    <p class="quick-text">Gestiona usuarios, roles y permisos del sistema.</p>
                </a>
            </div>
        </asp:PlaceHolder>

    </div>

    <div class="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
        <div>
            <h4 class="catalog-title mb-1">Catálogo de libros</h4>
            <p class="text-muted mb-0 small">Consulta, abre o descarga archivos disponibles.</p>
        </div>
    </div>

    <div class="flat-card filter-card">
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
            <div class="row g-3" id="contenedorLibros">
        </HeaderTemplate>

        <ItemTemplate>
            <div class="col-md-4 libro-item"
                 data-titulo='<%# Server.HtmlEncode(Convert.ToString(Eval("Titulo"))) %>'
                 data-autor='<%# Server.HtmlEncode(Convert.ToString(Eval("Autor"))) %>'
                 data-categoria='<%# Server.HtmlEncode(Convert.ToString(Eval("Categoria"))) %>'
                 data-isbn='<%# Server.HtmlEncode(Convert.ToString(Eval("ISBN"))) %>'>

                <div class="book-card h-100">
                    <div class="book-card-body">

                        <div class="d-flex align-items-start gap-3 mb-3">
                            <div class="book-badge">
                                <i class="bi bi-book"></i>
                            </div>

                            <div class="flex-grow-1">
                                <h5 class="book-title mb-1"><%# Eval("Titulo") %></h5>
                                <p class="book-author mb-0"><%# Eval("Autor") %></p>
                            </div>
                        </div>

                        <div class="book-info">
                            <div>
                                <i class="bi bi-bookmark"></i>
                                <span>Categoría: <%# Eval("Categoria") %></span>
                            </div>

                            <div>
                                <i class="bi bi-upc-scan"></i>
                                <span>ISBN: <%# Eval("ISBN") %></span>
                            </div>

                            <div>
                                <i class="bi bi-calendar-event"></i>
                                <span>Fecha: <%# Eval("FechaPublicacion", "{0:dd/MM/yyyy}") %></span>
                            </div>
                        </div>

                        <div class="book-actions">
                            <a href='<%# ApiBaseUrl + "api/libros/abrir/" + Eval("IdLibro") %>'
                               class="btn btn-outline-primary btn-sm"
                               target="_blank">
                                <i class="bi bi-eye me-1"></i>Abrir
                            </a>

                            <a href='<%# ApiBaseUrl + "api/libros/descargar/" + Eval("IdLibro") %>'
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

    <div id="sinResultados" class="empty-state mt-3" style="display:none;">
        <i class="bi bi-search me-2"></i>
        No se encontraron libros con esos criterios.
    </div>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const txtBuscar = document.getElementById("txtBuscar");
            const ddlCategoriaFiltro = document.getElementById("ddlCategoriaFiltro");
            const libros = document.querySelectorAll(".libro-item");
            const sinResultados = document.getElementById("sinResultados");

            function cargarCategorias() {
                const categorias = new Set();

                libros.forEach(function (libro) {
                    const categoria = (libro.dataset.categoria || "").trim();

                    if (categoria) {
                        categorias.add(categoria);
                    }
                });

                Array.from(categorias).sort().forEach(function (categoria) {
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

                libros.forEach(function (libro) {
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