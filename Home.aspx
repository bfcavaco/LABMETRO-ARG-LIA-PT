<%@ Page Language="c#" CodeBehind="Home.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.Home"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <img src="IMAGES/ic_seta_red.gif" />
    <a href="IMAGES/Visio-sequenciaestados_Nova_ComSetas_PDF.pdf" target="_blank" style="font-size: 10pt">
        <%=Resources.Resource.DiagramaSequenciaEstados %></a>
    <br />
    <br />
    <img src="IMAGES/ic_seta_red.gif" />
    <a href="IMAGES/WORKFLOW1C_pdf.pdf" target="_blank" style="font-size: 10pt">
    <%=Resources.Resource.InstWorkFlowPrimeiroCertificado %></a>
    <br />
    <br />
    <%=Resources.Resource.Laboratorio %>:
    <asp:DropDownList ID="ddGrandeza" runat="server" AutoPostBack="True" DataTextField="descricao"
        DataValueField="idGrandeza" OnSelectedIndexChanged="ddGrandeza_SelectedIndexChanged" />
        <br /><br />
    <fieldset>
        <legend><%=Resources.Resource.EquipsPorCalibrarMais30Dias %></legend>
        <asp:SqlDataSource runat="server" ID="sqlDsEquipsCalibrar" SelectCommand="SELECT idServico,refServico,EstadoServico.descricao as EstadoServico, BRE.dtBre ,dtEstado,servico.idBRE, Datediff(day, servico.dtEstado, getDate())as Dias, servico.bdefinitivo , servico.observacoes FROM servico INNER JOIN Grandeza on Servico.idGrandeza = Grandeza.idGrandeza INNER JOIN BRE on Servico.idBRE = BRE.idBre INNER JOIN EstadoServico on servico.idEstadoServico = EstadoServico.idEstadoServico WHERE servico.idEstadoServico IN (1,2,3,4,5)AND servico.idGrandeza = @idGrandeza AND Datediff(day, servico.dtEstado, getDate()) > 30   ORDER BY DIAS DESC"
            ConnectionString="<%$ ConnectionStrings:connstr %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddGrandeza" Name="idGrandeza" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="gvEquipsCalibrar" runat="server" AllowPaging="true" AllowSorting="true"
            DataSourceID="sqlDsEquipsCalibrar" AutoGenerateColumns="false">
            <Columns>
                <asp:HyperLinkField HeaderText="<%$ Resources:Resource, RefCalib %>" DataNavigateUrlFormatString="FormPastaEnsaio.aspx?id={0}"
                    DataNavigateUrlFields="idServico" Target="_self" DataTextField="refServico" SortExpression="refServico">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkField>
                <asp:BoundField DataField="dtBRE" SortExpression="dtBRE" HeaderText="<%$ Resources:Resource, DataBRE %>" DataFormatString="{0:dd/MM/yyyy}"
                    ItemStyle-Wrap="False"></asp:BoundField>
                <asp:BoundField DataField="EstadoServico" HeaderText="<%$ Resources:Resource, Estado %>" SortExpression="EstadoServico">
                </asp:BoundField>
                <asp:BoundField DataField="dtEstado" SortExpression="dtEstado" HeaderText="<%$ Resources:Resource, DtEstado %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundField>
                <asp:BoundField DataField="observacoes" SortExpression="observacoes" HeaderText="<%$ Resources:Resource, ObsServ %>">
                </asp:BoundField>
                <asp:BoundField DataField="Dias" HeaderText="<%$ Resources:Resource, DiasSemMudanca %>" SortExpression="Dias">
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <br />
        <asp:Button class="button" ID="btnPrintCalib" runat="server" CssClass="button" Text="<%$ Resources:resource, ListaPDF %>"
            OnClick="btnPrintCalib_Click1"></asp:Button>
    </fieldset>
    <br />
    <br />
    <fieldset>
        <legend>
        <%=Resources.Resource.EquipamentosPorActualizarMais30dias %></legend>
        <asp:DataGrid 
        ID="dgEquipsActualizar" 
        runat="server" 
        AutoGenerateColumns="False"
        OnPageIndexChanged="PageEquipsActualizar" 
        OnSortCommand="SortEquipsActualizar"
        AllowSorting="True"
        AllowPaging="True" 
        PageSize="5" >
            <Columns>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, RefCalib %>" DataNavigateUrlFormatString="FormPastaEnsaio.aspx?id={0}"
                    DataNavigateUrlField="idServico" Target="_self" DataTextField="refServico" SortExpression="refServico">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="dtBRE" SortExpression="dtBRE" HeaderText="<%$ Resources:Resource, DataBRE %>" DataFormatString="{0:dd/MM/yyyy}"
                    ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="EstadoServico" HeaderText="<%$ Resources:Resource, Estado %>" SortExpression="idEstadoServico">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtEstado" SortExpression="dtEstado" HeaderText="<%$ Resources:Resource, DtEstado %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="observacoes" SortExpression="observacoes" HeaderText="<%$ Resources:Resource, ObsServ %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Dias" HeaderText="<%$ Resources:Resource, DiasSemMudanca %>" SortExpression="Dias">
                </asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <br />
        <asp:Button class="button" ID="btnPrintActual" runat="server" CssClass="button" Text="<%$ Resources:Resource, ListaPDF %>"
            OnClick="btnPrintActual_Click1"></asp:Button>
    </fieldset>
    <br />
    <br />
    <fieldset>
        <legend><%=Resources.Resource.LegServEstadoFinalizar %></legend>
        <asp:DataGrid 
        ID="dgFinalizar" 
        runat="server" 
        AutoGenerateColumns="False" 
        OnPageIndexChanged="PageEquipsFinalizar"
            OnSortCommand="SortEquipsFinalizar" 
            AllowSorting="True" 
            AllowPaging="True" 
            PageSize="5">
            <Columns>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, RefCalib %>" DataNavigateUrlFormatString="FormPastaEnsaio.aspx?id={0}"
                    DataNavigateUrlField="idServico" Target="_self" DataTextField="refServico" SortExpression="refServico">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="dtBRE" SortExpression="dtBRE" HeaderText="<%$ Resources:Resource, DataBRE %>" DataFormatString="{0:dd/MM/yyyy}"
                    ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="EstadoServico" HeaderText="<%$ Resources:Resource, Estado %>" SortExpression="EstadoServico">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtEstado" SortExpression="dtEstado" HeaderText="<%$ Resources:Resource, DtEstado %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="observacoes" SortExpression="observacoes" HeaderText="<%$ Resources:Resource, ObsServ %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Dias" HeaderText="<%$ Resources:Resource, DiasSemMudanca %>" SortExpression="Dias">
                </asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <br />
        <asp:Button class="button" ID="btnPrintFinal" runat="server" CssClass="button" Text="<%$ Resources:Resource, ListaPDF %>"
            OnClick="btnPrintFinal_Click1"></asp:Button>
    </fieldset>
    <br />
    <br />
    <fieldset>
        <legend><%=Resources.Resource.EmpresasVerificarNumSAP %></legend>
        <asp:DataGrid 
        ID="dgEmpresas" 
        runat="server" 
        AutoGenerateColumns="False" 
        PageSize="10" 
        AllowPaging="True" 
        AllowSorting="True"
        OnSortCommand="SortEmpresas" 
        OnPageIndexChanged="PageEmpresas" 
        PagerStyle-Mode="NumericPages">    
            <Columns>
                <asp:HyperLinkColumn HeaderText="" Text="<%$ Resources:Resource, Editar %>" DataNavigateUrlFormatString="FormEmpresa.aspx?btn=EMP&id={0}"
                    DataNavigateUrlField="idEmpresa" Target="_self"></asp:HyperLinkColumn>
                <asp:TemplateColumn SortExpression="codigoBloqueioSAP" HeaderText="<%$ Resources:Resource, Bloqueio %>" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToString(DataBinder.Eval (Container, "DataItem.codigoBloqueioSAP")))%>'
                            Width="15px" Height="15px" BorderStyle="None" BorderWidth="0" runat="server">
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="<%$ Resources:Resource, Empresa %>" ItemStyle-Wrap="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtCriacao" SortExpression="dtCriacao" HeaderText="<%$ Resources:Resource, DtCriacao %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="UtilCriacao" SortExpression="UtilCriacao" HeaderText="<%$ Resources:Resource, UltCriacao %>"
                    ItemStyle-Wrap="False"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <br />
            <asp:Button class="button" ID="btnEmpresas" runat="server" CssClass="button" Text="<%$ Resources:Resource, ListaPDF %>"
                OnClick="btnEmpresas_Click"></asp:Button>
    </fieldset>
</asp:Content>
