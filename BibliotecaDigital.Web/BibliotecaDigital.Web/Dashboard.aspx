<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BibliotecaDigital.Web.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Iconos Bootstrap -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    <style>
        /* =====================================================
           ESTILO GENERAL DEL DASHBOARD
           ===================================================== */

        .kpi-card {
            border: none;
            border-radius: 16px;
            box-shadow: 0 10px 24px rgba(0,0,0,0.08);
            transition: all .25s ease;
            overflow: hidden;
        }

        .kpi-card:hover {
            transform: translateY(-4px);
        }

        .kpi-icono {
            font-size: 1.8rem;
            opacity: 0.9;
        }

        .kpi-numero {
            font-size: 2rem;
            font-weight: 700;
            line-height: 1.1;
        }

        .panel-card {
            border: none;
            border-radius: 16px;
            box-shadow: 0 10px 24px rgba(0,0,0,0.08);
        }

        .panel-titulo {
            font-weight: 700;
            margin-bottom: 1rem;
        }

        .tabla-dashboard th {
            background-color: rgba(13, 110, 253, 0.08);
            font-weight: 600;
        }

        .barra-categoria {
            height: 14px;
            border-radius: 8px;
        }

        .subtitulo-dashboard {
            color: #6c757d;
        }

        /* =====================================================
           MODO OSCURO
           ===================================================== */

        [data-bs-theme="dark"] .kpi-card,
        [data-bs-theme="dark"] .panel-card {
            background-color: #1e1e1e;
            color: #f1f1f1;
            box-shadow: 0 10px 24px rgba(0,0,0,0.35);
        }

        [data-bs-theme="dark"] .subtitulo-dashboard,
        [data-bs-theme="dark"] .text-muted {
            color: #cfcfcf !important;
        }

        [data-bs-theme="dark"] .table {
            color: #f1f1f1;
        }

        [data-bs-theme="dark"] .tabla-dashboard th {
            background-color: rgba(77, 163, 255, 0.15);
            color: #ffffff;
        }

        [data-bs-theme="dark"] .table-striped > tbody > tr:nth-of-type(odd) > * {
            color: #f1f1f1;
            background-color: rgba(255,255,255,0.03);
        }

        [data-bs-theme="dark"] .progress {
            background-color: rgba(255,255,255,0.08);
        }
    </style>

    <!-- =====================================================
         CABECERA DEL DASHBOARD
         ===================================================== -->
    <div class="d-flex justify-content-between align-items-center mb-4 flex-wrap gap-2">
        <div>
            <h2 class="mb-1">Dashboard</h2>
            <p class="subtitulo-dashboard mb-0">
                Indicadores y monitoreo del sistema Biblioteca Digital
            </p>
        </div>

        <div class="d-flex gap-2">
            <a href="Home.aspx" class="btn btn-outline-secondary">
                <i class="bi bi-house-door me-1"></i>Inicio
            </a>
        </div>
    </div>

    <!-- =====================================================
         DASHBOARD COMPLETO PARA ROLES 1 Y 2
         ===================================================== -->
    <asp:Panel ID="pnlDashboardCompleto" runat="server" Visible="false">

        <!-- KPIs -->
        <div class="row g-4 mb-4">

            <div class="col-md-3">
                <div class="card kpi-card p-3 border-start border-4 border-primary">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="text-muted">Total de libros</div>
                            <div class="kpi-numero text-primary">
                                <asp:Label ID="lblTotalLibros" runat="server"></asp:Label>
                            </div>
                        </div>
                        <i class="bi bi-book-half kpi-icono text-primary"></i>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="card kpi-card p-3 border-start border-4 border-success">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="text-muted">Total de usuarios</div>
                            <div class="kpi-numero text-success">
                                <asp:Label ID="lblTotalUsuarios" runat="server"></asp:Label>
                            </div>
                        </div>
                        <i class="bi bi-people-fill kpi-icono text-success"></i>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="card kpi-card p-3 border-start border-4 border-warning">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="text-muted">Total de categorías</div>
                            <div class="kpi-numero text-warning">
                                <asp:Label ID="lblTotalCategorias" runat="server"></asp:Label>
                            </div>
                        </div>
                        <i class="bi bi-tags-fill kpi-icono text-warning"></i>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="card kpi-card p-3 border-start border-4 border-danger">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="text-muted">Total de accesos</div>
                            <div class="kpi-numero text-danger">
                                <asp:Label ID="lblTotalAccesos" runat="server"></asp:Label>
                            </div>
                        </div>
                        <i class="bi bi-shield-lock-fill kpi-icono text-danger"></i>
                    </div>
                </div>
            </div>

        </div>

        <div class="row g-4">

            <!-- Últimos accesos -->
            <div class="col-lg-7">
                <div class="card panel-card p-3 h-100">
                    <h5 class="panel-titulo">
                        <i class="bi bi-clock-history me-2"></i>Últimos accesos
                    </h5>

                    <asp:GridView ID="gvAccesos" runat="server"
                        CssClass="table table-striped table-bordered tabla-dashboard"
                        AutoGenerateColumns="false"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
                            <asp:BoundField DataField="FechaAcceso" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="DireccionIP" HeaderText="IP" />
                            <asp:CheckBoxField DataField="Exitoso" HeaderText="Exitoso" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- Libros por categoría -->
            <div class="col-lg-5">
                <div class="card panel-card p-3 h-100">
                    <h5 class="panel-titulo">
                        <i class="bi bi-bar-chart-fill me-2"></i>Libros por categoría
                    </h5>

                    <asp:Repeater ID="rptLibrosCategoriaCompleto" runat="server">
                        <ItemTemplate>
                            <div class="mb-3">
                                <div class="d-flex justify-content-between">
                                    <span><%# Eval("Categoria") %></span>
                                    <strong><%# Eval("TotalLibros") %></strong>
                                </div>
                                <div class="progress barra-categoria">
                                    <div class="progress-bar bg-primary"
                                         role="progressbar"
                                         style='width: <%# Eval("TotalLibros") %>0%;'>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </div>

        <!-- Últimos libros -->
        <div class="card panel-card p-3 mt-4">
            <h5 class="panel-titulo">
                <i class="bi bi-journal-richtext me-2"></i>Últimos libros registrados
            </h5>

            <asp:GridView ID="gvUltimosLibrosCompleto" runat="server"
                CssClass="table table-striped table-bordered tabla-dashboard"
                AutoGenerateColumns="false"
                GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Titulo" HeaderText="Título" />
                    <asp:BoundField DataField="Autor" HeaderText="Autor" />
                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="FechaPublicacion" HeaderText="Fecha Publicación" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </asp:GridView>
        </div>

    </asp:Panel>

    <!-- =====================================================
         DASHBOARD SOLO DE LIBROS PARA ROL 3
         ===================================================== -->
    <asp:Panel ID="pnlDashboardLibros" runat="server" Visible="false">

        <!-- KPI de libros -->
        <div class="row g-4 mb-4">
            <div class="col-md-4">
                <div class="card kpi-card p-3 border-start border-4 border-primary">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="text-muted">Libros registrados</div>
                            <div class="kpi-numero text-primary">
                                <asp:Label ID="lblTotalLibrosRol3" runat="server"></asp:Label>
                            </div>
                        </div>
                        <i class="bi bi-book-half kpi-icono text-primary"></i>
                    </div>
                </div>
            </div>
        </div>

        <div class="row g-4">

            <!-- Libros por categoría -->
            <div class="col-lg-6">
                <div class="card panel-card p-3 h-100">
                    <h5 class="panel-titulo">
                        <i class="bi bi-bar-chart-fill me-2"></i>Libros por categoría
                    </h5>

                    <asp:Repeater ID="rptLibrosCategoriaRol3" runat="server">
                        <ItemTemplate>
                            <div class="mb-3">
                                <div class="d-flex justify-content-between">
                                    <span><%# Eval("Categoria") %></span>
                                    <strong><%# Eval("TotalLibros") %></strong>
                                </div>
                                <div class="progress barra-categoria">
                                    <div class="progress-bar bg-success"
                                         role="progressbar"
                                         style='width: <%# Eval("TotalLibros") %>0%;'>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <!-- Últimos libros -->
            <div class="col-lg-6">
                <div class="card panel-card p-3 h-100">
                    <h5 class="panel-titulo">
                        <i class="bi bi-journal-richtext me-2"></i>Últimos libros registrados
                    </h5>

                    <asp:GridView ID="gvUltimosLibrosRol3" runat="server"
                        CssClass="table table-striped table-bordered tabla-dashboard"
                        AutoGenerateColumns="false"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Titulo" HeaderText="Título" />
                            <asp:BoundField DataField="Autor" HeaderText="Autor" />
                            <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                            <asp:BoundField DataField="FechaPublicacion" HeaderText="Fecha Publicación" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>

    </asp:Panel>

    <!-- =====================================================
         MENSAJE DE SIN PERMISO
         ===================================================== -->
    
    <asp:Panel ID="pnlSinPermiso" runat="server" Visible="false" CssClass="alert alert-warning">
        No tiene permisos para acceder al dashboard.
    </asp:Panel>
    <asp:Label ID="lblErrorDashboard" runat="server" CssClass="text-danger d-block mt-3">
    </asp:Label>
</asp:Content>