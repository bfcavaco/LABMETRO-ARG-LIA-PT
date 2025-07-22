<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestServicosSemReq2.aspx.cs" Inherits="LabMetro.GestServicosSemReq2" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.ServicosSemRequisicao %></legend>
        <p>Instruções de preenchimento</p>
        <ul>

           <li>Para associar requisicoes, é preciso seleccionar primeiro uma empresa, idealmente tambem um BRE (mas o BRE nao é obrigatório).</li>
            <li>Ao seleccionar a empresa, aparecem apenas os serviços da empresa e no fundo da tabela aparece uma dropdown com as Requisicoes.</li>
            <li>Marcar os serviços a associar, escolher a Requisicao da Dropdown no final da tabela, e carregar em "Actualizar serviços com requisição".</li>
        </ul>

. 

</p>
        
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label><br />
        <br />
        <table>
          
           <%-- <tr>
                <td>
                    <%=Resources.Resource.BRE %>:
                </td>
                <td>
                    <asp:TextBox ID="txtBRE" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Ano %>:
                </td>
                <td>
                    <asp:TextBox ID="txtAno" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtEmpresa" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.RefCalib %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefServico" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td colspan="2">
                    <%=Resources.Resource.SoServicosFacturados %>:
                    <asp:CheckBox ID="chckFactura" runat="server" AutoPostBack="true"></asp:CheckBox>
                </td>
                <td colspan="2">
                    <%=Resources.Resource.SoServicosFacturadosComReq %>:
                    <asp:CheckBox ID="chckSemFactura" runat="server" AutoPostBack="true" Checked="true">
                    </asp:CheckBox>
                </td>
            </tr>
            <tr><td colspan="4">
          Empresa: <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa" DataTextField="empresa" onselectedindexchanged="ddEmpresa_SelectedIndexChanged"></asp:DropDownList>
          BRE:   <asp:DropDownList ID="ddBre" runat="server" DataValueField="idBre" DataTextField="refbre" OnSelectedIndexChanged="ddBre_SelectedIndexChanged"></asp:DropDownList>
          
                </td></tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button class="button" ID="btnSubmit" runat="server" CssClass="button" Text="<%$ Resources:Resource, Pesquisar %>" />
                    
                </td>
                <td>
                </td>
                <td align="right">
                    <asp:Button class="button" ID="btnList" runat="server" CssClass="button" Text=" <%$Resources:Resource, ListaBRESemReqNFact %>">
                    </asp:Button>
                </td>
            </tr>
        </table>
        
        <asp:DataGrid 
        ID="DG" 
        runat="server" 
        OnUpdateCommand="DG_UpdateGrid" 
        OnCancelCommand="DG_CancelGrid"
        OnEditCommand="DG_EditGrid" 
        PagerStyle-Mode="NumericPages" 
        OnSortCommand="SortGrid"
        OnPageIndexChanged="DoPaging" 
        AllowSorting="true" 
        ShowFooter="false" 
        AllowPaging="true"
        PageSize="50"
        AutoGenerateColumns="false" 
        DataKeyField="idEmpresa" 
         >
            
            <Columns>
                <asp:ButtonColumn CommandName="select" DataTextField="Empresa"  HeaderText="<%$ Resources:Resource, EmpresaClienteVerReq %>">
                </asp:ButtonColumn>
                <asp:HyperLinkColumn HeaderText="<%$Resources:Resource, BRE %>" DataNavigateUrlFormatString="FormBRE.aspx?id={0}"
                    DataNavigateUrlField="idBRE" Target="_new" DataTextField="refBRE" SortExpression="refBRE">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="dtBre" SortExpression="dtBre" HeaderText="<%$ Resources:Resource, DataBRE %>" ReadOnly="true"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, PastaEnsaio %>" DataNavigateUrlFormatString="FormPastaEnsaio.aspx?id={0}"
                    DataNavigateUrlField="idServico" Target="_new" DataTextField="refServico" SortExpression="refServico">
                </asp:HyperLinkColumn>
                <asp:BoundColumn HeaderText="<%$ Resources:Resource, IdServico %>" DataField="idServico" Visible="false" ReadOnly="True">
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="<%$ Resources:Resource, EmpresaContratou %>" DataField="EmpContrat" Visible="true"
                    ReadOnly="True" SortExpression="EmpContrat"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="<%$ Resources:Resource, Equipamento %>" DataField="equipamento" Visible="true"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="estado" SortExpression="estado" HeaderText="<%$ Resources:Resource, Estado %>" ReadOnly="true">
                </asp:BoundColumn>
                   <asp:TemplateColumn>
                        <HeaderTemplate>
                           Associar
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="checkbox"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                <%--<asp:TemplateColumn HeaderText="<%$ Resources:Resource, Req %>">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddRequisicao" runat="server" DataValueField="idRequisicao"
                            DataTextField="referenciaCliente">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Adicionar<br /> Req." UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100"></asp:EditCommandColumn>--%>
            </Columns>
        </asp:DataGrid>
        <br />
        <br />
         Requisicao: <asp:DropDownList ID="ddRequisao" runat="server" DataValueField="idRequisicao" DataTextField="refrequisicao"></asp:DropDownList>
        <asp:Button ID="btnUpdate" runat="server" text="Actualizar Serviços com Requisicao" OnClick="btnUpdate_Click"/>
        <asp:DataGrid 
        ID="DGRequisicoes" 
        runat="server"
        DataKeyField="idRequisicao"
        PagerStyle-Mode="NumericPages" 
        OnPageIndexChanged="DoPaging" 
        OnSortCommand="SortGrid"
        AllowSorting="True" 
        PageSize="25" 
        AllowPaging="true" 
        AutoGenerateColumns="false">
            <Columns>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, EditarRequisicao %>" DataNavigateUrlFormatString="FormRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_self" DataTextField="refRequisicao"
                    SortExpression="refRequisicao">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="referenciaCliente" SortExpression="referenciaCliente"
                    HeaderText="<%$ Resources:Resource, RefReqCliente %>" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                <asp:BoundColumn DataField="observacoes" SortExpression="observacoes" HeaderText="<%$ Resources:Resource, ObservacoesEmpresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtRequisicao" SortExpression="dtRequisicao" HeaderText="<%$ Resources:Resource, DtReq %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtValidade" SortExpression="dtValidade" HeaderText="<%$ Resources:Resource, DtVal %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Completa %>" SortExpression="completa">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.completa"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Contrato %>" SortExpression="bContrato">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bContrato"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Renovavel %>" SortExpression="bRenovavel">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bRenovavel"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="dtCriacao" SortExpression="dtCriacao" HeaderText="<%$ Resources:Resource, DataIntroducao %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Ficheiro %>" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                            ID="Hyperlink1" Target="new">
											<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:ButtonColumn CommandName="select" HeaderText="<%$ Resources:Resource, ServicosAssociados %>" Text="ver"
                    ItemStyle-Font-Size="8"></asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
        <p>
        <%=Resources.Resource.FraseInfoReqEnviada %>
        </p>
        <p>
       <%=Resources.Resource.FraseInfoProcInterno %>
        </p>
        <asp:Label ID="lblMessage2" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <asp:DataGrid ID="dgServicos" runat="server" BackColor="Gainsboro"
            CellPadding="2" CellSpacing="0" GridLines="Horizontal" BorderColor="#FFFFFF"
            AllowSorting="False" ShowFooter="false" BorderWidth="2" AutoGenerateColumns="False">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ></HeaderStyle>
            <Columns>
                <asp:BoundColumn DataField="dtBre" SortExpression="dtBre" HeaderText="Data Bre" DataFormatString="{0:dd/MM/yyyy}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="refServico" HeaderText="<%$ Resources:Resource, RefServ %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="tipoEquipamento" HeaderText="<%$ Resources:Resource, TipoEquipamento %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="numIdentificacao" HeaderText="<%$ Resources:Resource, NumIdent %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="numSerie" HeaderText="<%$ Resources:Resource, NumSerie %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtCalibracao" SortExpression="dtCalibracao" HeaderText="<%$ Resources:Resource, DataCalibracao %>"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <a name="bottom"></a>


         <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Exportar p/Excel" />
        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="True">
        <Columns>
       <%-- <asp:BoundField DataField="empresa" HeaderText="Empresa" />
        <asp:BoundField DataField="refBre" HeaderText="BRE" />
        <asp:BoundField DataField="dtBre" HeaderText="DtBRE" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="refServico" HeaderText="Serviço" />
        <asp:BoundField DataField="estado" HeaderText="Estado" />
        <asp:BoundField DataField="dtBse" HeaderText="DtBSE" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="tipoEquipamento" HeaderText="Tipo" />
        <asp:BoundField DataField="numIdentificacao" HeaderText="NID" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="numSerie" HeaderText="NS" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="marca" HeaderText="Marca" />
        <asp:BoundField DataField="modelo" HeaderText="Modelo" />
        <asp:BoundField DataField="refBSE" HeaderText="BSE" />
        <asp:BoundField DataField="observacoes" HeaderText="observacoes" />
            <asp:BoundField DataField="codigoIPQ" HeaderText="codigoIPQ" />
             <asp:BoundField DataField="dtEstado" HeaderText="dtEstado" 
                SortExpression="dtEstado" DataFormatString="{0:dd/MM/yyyy}" />
              <asp:BoundField DataField="refServicoCertificado" HeaderText="refServicoCertificado" />
            <asp:BoundField DataField="subtipo" HeaderText="SubTipo" />
             <asp:BoundField DataField="classe" HeaderText="classe" />
            <asp:BoundField DataField="campo1" HeaderText="campo1" />
            <asp:BoundField DataField="funcionarioTreino" HeaderText="funcionarioTreino" />
                      <asp:BoundField DataField="dtUltimaCalibracao" HeaderText="dtUltimaCalibracao" />
            <asp:BoundField DataField="refUltimaCalibracao" HeaderText="refUltCal" />--%>
        </Columns>
        </asp:GridView>


    </fieldset>
</asp:Content>
