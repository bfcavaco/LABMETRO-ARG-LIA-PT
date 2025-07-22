<%@ Page Language="c#" CodeBehind="ListaRequisicoes.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaRequisicoes" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">


</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.Pesquisar %> <%=Resources.Resource.Requisicoes %></legend>
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label><br />
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true" OnTextChanged="txtPesquisaEmpresa_TextChanged"></asp:TextBox>&nbsp;&nbsp;
                </td>
                <td>
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true" OnTextChanged="txtPesquisaNif_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button class="button" ID="btnPesquisaEmpresa" runat="server" Text="<%$ Resources:Resource, VerEmpresas %>"
                        CssClass="button" OnClick="btnPesquisaEmpresa_Click"></asp:Button>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa"
                        AutoPostBack="true" OnSelectedIndexChanged="ddEmpresa_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.RefInterna %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefReq" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Completa %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddCompletas" runat="server">
                        <asp:ListItem Value="" Text="<%$ Resources:Resource, Todas %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Incompletas %>"></asp:ListItem>
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Completa %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Contrato %>:
                </td>
                <td>
                    <asp:CheckBox ID="chbContrato" runat="server"></asp:CheckBox>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Validade %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddValidade" runat="server">
                        <asp:ListItem Value="" Text="<%$ Resources:Resource, Todas %>"></asp:ListItem>
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, ComDataValida %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, DatasAnteriores %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.SoComFicheiro %>:
                </td>
                <td>
                    <asp:CheckBox ID="cbFicheiro" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2" align="center">
                    <asp:Button class="button" ID="btnPesquisa" runat="server" CausesValidation="true"
                        Text="<%$ Resources:Resource, Pesquisar %>" CssClass="button" OnClick="btnPesquisa_Click"></asp:Button>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:DataGrid 
        ID="DGRequisicoes" 
        runat="server" 
        DataKeyField="idRequisicao" 
        OnPageIndexChanged="DoPaging" 
        OnSortCommand="SortGrid"
        AllowSorting="True" 
        PageSize="10" 
        AllowPaging="true" 
        AutoGenerateColumns="false"
        OnSelectedIndexChanged="DGRequisicoes_SelectedIndexChanged">
            <Columns>
                <asp:TemplateColumn SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                    <ItemTemplate>
                        <asp:TextBox ID="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.nivelBloqueioLabmetro")),Convert.ToString(DataBinder.Eval (Container, "DataItem.codigoBloqueioSAP")))%>'
                            Width="10" Height="10" runat="server">
                        </asp:TextBox>&nbsp;
                        <%# DataBinder.Eval(Container, "DataItem.empresa")%>
                        </asp:textbox>
                    </ItemTemplate>
                    <ItemStyle Width="100"></ItemStyle>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="referenciaCliente" SortExpression="referenciaCliente"
                    HeaderText="<%$Resources:Resource, RefReqCliente %>" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
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
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Renov %>" SortExpression="bRenovavel">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bRenovavel"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="dtCriacao" SortExpression="dtCriacao" HeaderText="<%$ Resources:Resource, DataIntroducao %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="obsReq" SortExpression="obsReq" HeaderText="<%$ Resources:Resource, ObsReq %>">
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Ficheiro %>" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                            ID="Hyperlink1" Target="new">
											<%=Resources.Resource.Ver %>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Editar %>" DataNavigateUrlFormatString="FormRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_self" DataTextField="refRequisicao"
                    SortExpression="refRequisicao">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:ButtonColumn CommandName="select" HeaderText="<%$ Resources:Resource, VerServicos %>" Text="<%$ Resources:Resource, Ver %>" ItemStyle-Font-Size="8">
                </asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
        <br />
        <br />
        <asp:Label ID="lblMessageServicos" runat="server" CssClass="lblMessage"></asp:Label>
        <asp:DataGrid 
        ID="dgServicos" 
        runat="server" 
        AllowSorting="False" 
        ShowFooter="false" 
        DataKeyField="idServico" 
        BorderWidth="2"
        AutoGenerateColumns="False" 
        OnItemCommand="desassociarRequisicao">
            <Columns>
                <asp:BoundColumn DataField="dtBre" SortExpression="dtBre" HeaderText="<%$ Resources:Resource, DataBRE %>" DataFormatString="{0:dd/MM/yyyy}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="refServico" HeaderText="<%$ Resources:Resource, RefServ %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="tipoEquipamento" HeaderText="<%$ Resources:Resource, TipoEquipamento %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="numIdentificacao" HeaderText="<%$ Resources:Resource, NumIdent %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="numSerie" HeaderText="<%$ Resources:Resource, NumSerie %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtCalibracao" SortExpression="dtCalibracao" HeaderText="<%$ Resources:Resource, DataCalibracao %>"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:ButtonColumn CommandName="desassociarReq" HeaderText="<%$ Resources:Resource, RemovReq %>" Text="<%$ Resources:Resource, Desassociar %>"
                    ItemStyle-Font-Size="8"></asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
