<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BibliotecaDigital.Web.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        :root {
            --arctic: #F1F6F4;
            --forsythia: #FFC801;
            --nocturnal: #114C5A;
            --mint: #D9E8E2;
            --saffron: #FF9932;
            --noir: #172B36;
        }

        .dashboard-title {
            font-weight: 800;
            letter-spacing: -.04em;
        }

        .dashboard-subtitle {
            color: #6c757d;
        }

        .dash-card {
            border: 1px solid rgba(17,76,90,.08);
            border-radius: 22px;
            background: rgba(255,255,255,.92);
            box-shadow: 0 12px 28px rgba(23,43,54,.06);
        }

        .kpi-card {
            padding: 1.25rem;
            min-height: 135px;
            position: relative;
            overflow: hidden;
        }

        .kpi-card::after {
            content: "";
            position: absolute;
            right: -35px;
            top: -35px;
            width: 110px;
            height: 110px;
            border-radius: 50%;
            background: linear-gradient(135deg, rgba(255,200,1,.22), rgba(255,153,50,.16));
        }

        .kpi-label {
            color: #667085;
            font-size: .86rem;
            margin-bottom: .45rem;
        }

        .kpi-number {
            font-size: 2.25rem;
            font-weight: 850;
            line-height: 1;
            color: var(--noir);
        }

        .kpi-icon {
            width: 46px;
            height: 46px;
            border-radius: 15px;
            display: grid;
            place-items: center;
            background: linear-gradient(135deg, var(--mint), var(--arctic));
            color: var(--nocturnal);
            font-size: 1.3rem;
        }

        .section-title {
            font-weight: 800;
            font-size: 1rem;
            margin-bottom: 1rem;
            color: var(--noir);
        }

        .section-title i {
            color: var(--nocturnal);
        }

        .chart-wrap {
            height: 270px;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .category-row {
            display: grid;
            grid-template-columns: 1fr 70px 40px;
            align-items: center;
            gap: .75rem;
            padding: .72rem 0;
            border-bottom: 1px solid rgba(17,76,90,.08);
            color: #667085;
        }

        .category-row:last-child {
            border-bottom: none;
        }

        .category-name {
            display: flex;
            align-items: center;
            gap: .55rem;
        }

        .category-book-icon {
            color: var(--nocturnal);
            font-size: 1rem;
        }

        .category-percent {
            font-size: .85rem;
            color: #667085;
            text-align: right;
        }

        .category-total {
            color: var(--noir);
            text-align: right;
            font-weight: 800;
        }

        .tabla-dashboard {
            margin-bottom: 0;
            font-size: .9rem;
        }

        .tabla-dashboard th {
            background: var(--arctic);
            color: var(--noir);
            font-weight: 800;
            border-bottom: 1px solid rgba(17,76,90,.12);
        }

        .tabla-dashboard td {
            vertical-align: middle;
            color: #667085;
        }

        .user-cell {
            display: flex;
            align-items: center;
            gap: .5rem;
        }

        .user-mini-icon {
            width: 28px;
            height: 28px;
            border-radius: 10px;
            display: grid;
            place-items: center;
            background: linear-gradient(135deg, var(--mint), var(--arctic));
            color: var(--nocturnal);
            font-size: .85rem;
            flex-shrink: 0;
        }

        .access-chart-card {
            margin-top: 1rem;
            padding-top: 1rem;
            border-top: 1px solid rgba(17,76,90,.08);
        }

        .access-chart-wrap {
            height: 220px;
        }

        .empty-state {
            border-radius: 18px;
            border: 1px solid rgba(255,200,1,.25);
            background: rgba(255,200,1,.08);
            padding: 1rem;
        }

        [data-bs-theme="dark"] .dash-card {
            background: #1e1e1e;
            border-color: rgba(255,255,255,.08);
            box-shadow: 0 12px 28px rgba(0,0,0,.25);
        }

        [data-bs-theme="dark"] .dashboard-subtitle,
        [data-bs-theme="dark"] .kpi-label,
        [data-bs-theme="dark"] .tabla-dashboard td,
        [data-bs-theme="dark"] .category-row,
        [data-bs-theme="dark"] .category-percent {
            color: #adb5bd;
        }

        [data-bs-theme="dark"] .kpi-number,
        [data-bs-theme="dark"] .section-title,
        [data-bs-theme="dark"] .category-total {
            color: #f8f9fa;
        }

        [data-bs-theme="dark"] .tabla-dashboard th {
            background: rgba(255,255,255,.06);
            color: #f8f9fa;
        }
    </style>

    <div class="mb-4">
        <h2 class="dashboard-title mb-1">Dashboard</h2>
        <p class="dashboard-subtitle mb-0">
            Indicadores operativos y actividad del sistema Biblioteca Digital
        </p>
    </div>

    <asp:Panel ID="pnlDashboardCompleto" runat="server" Visible="false">

        <div class="row g-3 mb-4">

            <div class="col-md-3">
                <div class="dash-card kpi-card">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <div class="kpi-label">Libros digitales</div>
                            <div class="kpi-number">
                                <asp:Label ID="lblTotalLibros" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon"><i class="bi bi-book"></i></div>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="dash-card kpi-card">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <div class="kpi-label">Usuarios registrados</div>
                            <div class="kpi-number">
                                <asp:Label ID="lblTotalUsuarios" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon"><i class="bi bi-people"></i></div>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="dash-card kpi-card">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <div class="kpi-label">Categorías activas</div>
                            <div class="kpi-number">
                                <asp:Label ID="lblTotalCategorias" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon"><i class="bi bi-tags"></i></div>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="dash-card kpi-card">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <div class="kpi-label">Accesos registrados</div>
                            <div class="kpi-number">
                                <asp:Label ID="lblTotalAccesos" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon"><i class="bi bi-shield-check"></i></div>
                    </div>
                </div>
            </div>

        </div>

        <div class="row g-3">

            <div class="col-lg-5">
                <div class="dash-card p-3 h-100">
                    <h5 class="section-title">
                        <i class="bi bi-pie-chart me-2"></i>Libros por categoría
                    </h5>

                    <div class="chart-wrap">
                        <canvas id="chartCategoriasCompleto"></canvas>
                    </div>

                    <asp:Repeater ID="rptLibrosCategoriaCompleto" runat="server">
                        <ItemTemplate>
                            <div class="category-row categoria-data-completo"
                                 data-categoria='<%# Eval("Categoria") %>'
                                 data-total='<%# Eval("TotalLibros") %>'>
                                <span class="category-name">
                                    <i class="bi bi-book-fill category-book-icon"></i>
                                    <%# Eval("Categoria") %>
                                </span>
                                <span class="category-percent">0%</span>
                                <span class="category-total"><%# Eval("TotalLibros") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="col-lg-7">
                <div class="dash-card p-3 h-100">
                    <h5 class="section-title">
                        <i class="bi bi-clock-history me-2"></i>Últimos accesos
                    </h5>

                    <asp:GridView ID="gvAccesos" runat="server"
                        CssClass="table table-hover tabla-dashboard"
                        AutoGenerateColumns="false"
                        GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Usuario">
                                <ItemTemplate>
                                    <div class="user-cell">
                                        <span class="user-mini-icon">
                                            <i class="bi bi-person-fill"></i>
                                        </span>
                                        <span><%# Eval("Usuario") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
                            <asp:BoundField DataField="FechaAcceso" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="DireccionIP" HeaderText="IP" />
                            <asp:CheckBoxField DataField="Exitoso" HeaderText="OK" />
                        </Columns>
                    </asp:GridView>

                    <div class="access-chart-card">
                        <h6 class="section-title mb-3">
                            <i class="bi bi-bar-chart-line me-2"></i>Accesos por día
                        </h6>

                        <div class="access-chart-wrap">
                            <canvas id="chartAccesos"></canvas>
                        </div>
                    </div>

                </div>
            </div>

        </div>

        <div class="dash-card p-3 mt-3">
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

    <asp:Panel ID="pnlDashboardLibros" runat="server" Visible="false">

        <div class="row g-3 mb-4">
            <div class="col-md-4">
                <div class="dash-card kpi-card">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <div class="kpi-label">Libros registrados</div>
                            <div class="kpi-number">
                                <asp:Label ID="lblTotalLibrosRol3" runat="server" />
                            </div>
                        </div>
                        <div class="kpi-icon"><i class="bi bi-book"></i></div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row g-3">

            <div class="col-lg-5">
                <div class="dash-card p-3 h-100">
                    <h5 class="section-title">
                        <i class="bi bi-pie-chart me-2"></i>Libros por categoría
                    </h5>

                    <div class="chart-wrap">
                        <canvas id="chartCategoriasEjecutivo"></canvas>
                    </div>

                    <asp:Repeater ID="rptLibrosCategoriaRol3" runat="server">
                        <ItemTemplate>
                            <div class="category-row categoria-data-ejecutivo"
                                 data-categoria='<%# Eval("Categoria") %>'
                                 data-total='<%# Eval("TotalLibros") %>'>
                                <span class="category-name">
                                    <i class="bi bi-book-fill category-book-icon"></i>
                                    <%# Eval("Categoria") %>
                                </span>
                                <span class="category-percent">0%</span>
                                <span class="category-total"><%# Eval("TotalLibros") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="col-lg-7">
                <div class="dash-card p-3 h-100">
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

    <asp:Panel ID="pnlSinPermiso" runat="server" Visible="false" CssClass="empty-state">
        <i class="bi bi-exclamation-triangle me-2"></i>
        No tiene permisos para acceder al dashboard.
    </asp:Panel>

    <asp:Label ID="lblErrorDashboard" runat="server" CssClass="text-danger d-block mt-3" />

    <script>
        document.addEventListener("DOMContentLoaded", function () {

            const chartColors = [
                "#FFC801",
                "#FF9932",
                "#114C5A",
                "#172B36",
                "#D9E8E2",
                "#F1F6F4"
            ];

            function obtenerDatos(selector) {
                const items = document.querySelectorAll(selector);
                const labels = [];
                const values = [];

                items.forEach(function (item) {
                    labels.push(item.dataset.categoria);
                    values.push(parseInt(item.dataset.total || "0"));
                });

                return { labels, values };
            }

            function calcularTotal(values) {
                return values.reduce(function (a, b) {
                    return a + b;
                }, 0);
            }

            function actualizarPorcentajes(selector) {
                const items = document.querySelectorAll(selector);
                let total = 0;

                items.forEach(function (item) {
                    total += parseInt(item.dataset.total || "0");
                });

                items.forEach(function (item) {
                    const valor = parseInt(item.dataset.total || "0");
                    const porcentaje = total > 0 ? ((valor / total) * 100).toFixed(1) : "0.0";
                    const span = item.querySelector(".category-percent");

                    if (span) {
                        span.textContent = porcentaje + "%";
                    }
                });
            }

            function crearDonut(canvasId, selector) {
                const canvas = document.getElementById(canvasId);
                if (!canvas) return;

                const datos = obtenerDatos(selector);
                if (datos.labels.length === 0) return;

                new Chart(canvas, {
                    type: "doughnut",
                    data: {
                        labels: datos.labels,
                        datasets: [{
                            data: datos.values,
                            backgroundColor: chartColors,
                            borderWidth: 0,
                            hoverOffset: 8
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        cutout: "68%",
                        plugins: {
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        const total = calcularTotal(context.dataset.data);
                                        const value = context.raw;
                                        const percent = total > 0 ? ((value / total) * 100).toFixed(1) : "0.0";
                                        return context.label + ": " + value + " (" + percent + "%)";
                                    }
                                }
                            },
                            legend: {
                                position: "bottom",
                                labels: {
                                    usePointStyle: true,
                                    boxWidth: 8,
                                    padding: 16
                                }
                            }
                        }
                    }
                });

                actualizarPorcentajes(selector);
            }

            function crearChartAccesos() {
                const canvas = document.getElementById("chartAccesos");
                if (!canvas) return;

                const tabla = document.getElementById("<%= gvAccesos.ClientID %>");
                if (!tabla) return;

                const filas = tabla.querySelectorAll("tr");
                const conteo = {};

                filas.forEach(function (fila, index) {
                    if (index === 0) return;

                    const celdas = fila.querySelectorAll("td");
                    if (celdas.length < 3) return;

                    const fechaTexto = celdas[2].innerText.trim();
                    const fecha = fechaTexto.substring(0, 10);

                    if (!conteo[fecha]) {
                        conteo[fecha] = 0;
                    }

                    conteo[fecha]++;
                });

                const labels = Object.keys(conteo);
                const values = Object.values(conteo);

                if (labels.length === 0) return;

                new Chart(canvas, {
                    type: "bar",
                    data: {
                        labels: labels,
                        datasets: [{
                            label: "Accesos",
                            data: values,
                            backgroundColor: chartColors,
                            borderRadius: 10,
                            maxBarThickness: 42
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                display: false
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        return "Accesos: " + context.raw;
                                    }
                                }
                            }
                        },
                        scales: {
                            y: {
                                beginAtZero: true,
                                ticks: {
                                    precision: 0
                                }
                            }
                        }
                    }
                });
            }

            crearDonut("chartCategoriasCompleto", ".categoria-data-completo");
            crearDonut("chartCategoriasEjecutivo", ".categoria-data-ejecutivo");
            crearChartAccesos();
        });
    </script>

</asp:Content>