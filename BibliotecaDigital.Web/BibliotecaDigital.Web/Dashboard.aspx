<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BibliotecaDigital.Web.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    <style>
        .dashboard-header {
            margin-bottom: 1.75rem;
        }

        .dashboard-title {
            font-weight: 700;
            letter-spacing: -0.03em;
        }

        .dashboard-subtitle {
            color: #6c757d;
            font-size: .95rem;
        }

        .flat-card {
            border: 1px solid rgba(0,0,0,.06);
            border-radius: 18px;
            background: #ffffff;
            box-shadow: 0 8px 22px rgba(0,0,0,.04);
        }

        .kpi-card {
            padding: 1.25rem;
            transition: transform .2s ease, box-shadow .2s ease;
        }

        .kpi-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 12px 28px rgba(0,0,0,.07);
        }

        .kpi-label {
            color: #6c757d;
            font-size: .86rem;
            margin-bottom: .35rem;
        }

        .kpi-number {
            font-size: 2rem;
            font-weight: 800;
            line-height: 1;
            letter-spacing: -0.04em;
        }

        .kpi-icon {
            width: 44px;
            height: 44px;
            border-radius: 14px;
            display: grid;
            place-items: center;
            font-size: 1.35rem;
            background: rgba(13,110,253,.08);
            color: #0d6efd;
        }

        .section-title {
            font-weight: 700;
            font-size: 1rem;
            margin-bottom: 1rem;
        }

        .section-title i {
            color: #0d6efd;
        }

        .tabla-dashboard {
            margin-bottom: 0;
            font-size: .92rem;
        }

        .tabla-dashboard th {
            background: #f8f9fa;
            color: #495057;
            font-weight: 700;
            border-bottom: 1px solid #e9ecef;
        }

        .tabla-dashboard td {
            vertical-align: middle;
        }

        .categoria-item {
            margin-bottom: 1rem;
        }

        .categoria-label {
            font-size: .9rem;
        }

        .barra-categoria {
            height: 8px;
            border-radius: 999px;
            background: #edf2f7;
        }

        .progress-bar {
            border-radius: 999px;
        }

        .empty-state {
            border-radius: 18px;
            border: 1px solid rgba(255,193,7,.25);
            background: rgba(255,193,7,.08);
            padding: 1rem;
        }

        [data-bs-theme="dark"] .flat-card {
            background: #1e1e1e;
            border-color: rgba(255,255,255,.08);
            box-shadow: 0 8px 22px rgba(0,0,0,.25);
        }

        [data-bs-theme="dark"] .dashboard-subtitle,
        [data-bs-theme="dark"] .kpi-label {
            color: #adb5bd;
        }

        [data-bs-theme="dark"] .tabla-dashboard th {
            background: rgba(255,255,255,.06);
            color: #f8f9fa;
        }

        [data-bs-theme="dark"] .table {
            color: #f8f9fa;
        }

        [data-bs-theme="dark"] .barra-categoria {
            background: rgba(255,255,255,.08);
        }
    </style>

    <!-- CABECERA -->
    <div class="dashboard-header">
        <h2 class="dashboard-title mb-1">Dashboard</h2>
        <p class="dashboard-subtitle mb-0">
            Resumen operativo de Biblioteca Digital
        </p>
    </div>

    <!-- DASHBOARD COMPLETO -->
    <asp:Panel ID="pnlDashboardCompleto" runat="server" Visible="false">

        <div class="row g-3 mb-4">

            <div class="col-md-3">
                <div class="flat-card kpi-card">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="kpi-label">Libros</div>
                            <div class="kpi-number text-primary">
                                <asp:Label ID="lblTotalLibros" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon">
                            <i class="bi bi-book"></i>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="flat-card kpi-card">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="kpi-label">Usuarios</div>
                            <div class="kpi-number text-success">
                                <asp:Label ID="lblTotalUsuarios" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon">
                            <i class="bi bi-people"></i>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="flat-card kpi-card">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="kpi-label">Categorías</div>
                            <div class="kpi-number text-warning">
                                <asp:Label ID="lblTotalCategorias" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon">
                            <i class="bi bi-tags"></i>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="flat-card kpi-card">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="kpi-label">Accesos</div>
                            <div class="kpi-number text-danger">
                                <asp:Label ID="lblTotalAccesos" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon">
                            <i class="bi bi-shield-check"></i>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="row g-3">

            <div class="col-lg-7">
                <div class="flat-card p-3 h-100">
                    <h5 class="section-title">
                        <i class="bi bi-clock-history me-2"></i>Últimos accesos
                    </h5>

                    <asp:GridView ID="gvAccesos" runat="server"
                        CssClass="table table-hover tabla-dashboard"
                        AutoGenerateColumns="false"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
                            <asp:BoundField DataField="FechaAcceso" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="DireccionIP" HeaderText="IP" />
                            <asp:CheckBoxField DataField="Exitoso" HeaderText="OK" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="col-lg-5">
                <div class="flat-card p-3 h-100">
                    <h5 class="section-title">
                        <i class="bi bi-bar-chart me-2"></i>Libros por categoría
                    </h5>

                    <asp:Repeater ID="rptLibrosCategoriaCompleto" runat="server">
                        <ItemTemplate>
                            <div class="categoria-item">
                                <div class="d-flex justify-content-between categoria-label mb-1">
                                    <span><%# Eval("Categoria") %></span>
                                    <strong><%# Eval("TotalLibros") %></strong>
                                </div>
                                <div class="progress barra-categoria">
                                    <div class="progress-bar" role="progressbar" style='width: <%# Eval("TotalLibros") %>0%;'></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </div>

        <div class="flat-card p-3 mt-3">
            <h5 class="section-title">
                <i class="bi bi-journal-text me-2"></i>Últimos libros registrados
            </h5>

            <asp:GridView ID="gvUltimosLibrosCompleto" runat="server"
                CssClass="table table-hover tabla-dashboard"
                AutoGenerateColumns="false"
                GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Titulo" HeaderText="Título" />
                    <asp:BoundField DataField="Autor" HeaderText="Autor" />
                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="FechaPublicacion" HeaderText="Publicación" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </asp:GridView>
        </div>

    </asp:Panel>

    <!-- DASHBOARD EJECUTIVO -->
    <asp:Panel ID="pnlDashboardLibros" runat="server" Visible="false">

        <div class="row g-3 mb-4">
            <div class="col-md-4">
                <div class="flat-card kpi-card">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="kpi-label">Libros registrados</div>
                            <div class="kpi-number text-primary">
                                <asp:Label ID="lblTotalLibrosRol3" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon">
                            <i class="bi bi-book"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row g-3">

            <div class="col-lg-5">
                <div class="flat-card p-3 h-100">
                    <h5 class="section-title">
                        <i class="bi bi-bar-chart me-2"></i>Libros por categoría
                    </h5>

                    <asp:Repeater ID="rptLibrosCategoriaRol3" runat="server">
                        <ItemTemplate>
                            <div class="categoria-item">
                                <div class="d-flex justify-content-between categoria-label mb-1">
                                    <span><%# Eval("Categoria") %></span>
                                    <strong><%# Eval("TotalLibros") %></strong>
                                </div>
                                <div class="progress barra-categoria">
                                    <div class="progress-bar bg-success" role="progressbar" style='width: <%# Eval("TotalLibros") %>0%;'></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="col-lg-7">
                <div class="flat-card p-3 h-100">
                    <h5 class="section-title">
                        <i class="bi bi-journal-text me-2"></i>Últimos libros registrados
                    </h5>

                    <asp:GridView ID="gvUltimosLibrosRol3" runat="server"
                        CssClass="table table-hover tabla-dashboard"
                        AutoGenerateColumns="false"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Titulo" HeaderText="Título" />
                            <asp:BoundField DataField="Autor" HeaderText="Autor" />
                            <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                            <asp:BoundField DataField="FechaPublicacion" HeaderText="Publicación" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>

    </asp:Panel>

    <!-- SIN PERMISO / ERROR -->
    <asp:Panel ID="pnlSinPermiso" runat="server" Visible="false" CssClass="empty-state">
        <i class="bi bi-exclamation-triangle me-2"></i>
        No tiene permisos para acceder al dashboard.
    </asp:Panel>

    <asp:Label ID="lblErrorDashboard" runat="server" CssClass="text-danger d-block mt-3" />

</asp:Content>