<%@ Page Language="c#" CodeBehind="ListaContactos.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaContactos" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnSubmit").focus();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
        ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
    <fieldset>
        <legend>
            <%=Resources.Resource.PesquisarContactos %></legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true"></asp:TextBox><br />
                </td>
                <td>
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <%=Resources.Resource.SoSedes %>:<asp:CheckBox ID="cbSede" runat="server"></asp:CheckBox>
                    <asp:Button class="button" ID="btnPesquisaEmpresa" runat="server" CssClass="button"
                        Text="<%$Resources:Resource, verEmpresas %>"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                        DataTextField="nome">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NomeContacto %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeContacto" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Email %>:
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <%=Resources.Resource.SoContactosActivos %>:<asp:CheckBox ID="chbActivo" runat="server"
                        Checked="True"></asp:CheckBox><asp:Button class="button" ID="btnSubmit" runat="server"
                            CssClass="button" Text="<%$ Resources:Resource, pesquisar %>"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="Textbox1" Enabled="False" BackColor="white" Width="10" Height="10"
                        runat="server" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>&nbsp;<%=Resources.Resource.ContactosActivos%><br />
                    <asp:TextBox ID="Textbox2" Enabled="False" BackColor="red" Width="10" Height="10"
                        runat="server" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>&nbsp;<%=Resources.Resource.ContactosInactivos%><br />
                </td>
            </tr>
        </table>
        <asp:DataGrid ID="DGContactos" runat="server" AlternatingItemStyle-BackColor="LightGrey"
            BackColor="Gainsboro" DataKeyField="idContacto" BorderWidth="2" GridLines="Both"
            BorderColor="#FFFFFF" PagerStyle-Mode="NumericPages" OnPageIndexChanged="DoPaging"
            OnSortCommand="SortGrid" AllowSorting="True" PageSize="25" AllowPaging="true"
            AutoGenerateColumns="false" OnEditCommand="dgContactos_edit " OnCancelCommand="dgContactos_cancel"
            OnUpdateCommand="dgContactos_update" ShowFooter="True">
            <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
            <FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
            <Columns>
                <asp:TemplateColumn SortExpression="activo" HeaderText="<%$ Resources:Resource, Activo %>">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColor" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.activo")))%>'
                            Width="10" Height="10" BorderStyle="None" BorderWidth="0" runat="server">
                        </asp:TextBox>&nbsp;
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEstado" runat="server"></asp:CheckBox></EditItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="<%$ Resources:Resource, Nome %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>"
                    ReadOnly="True"></asp:BoundColumn>
                 <asp:BoundColumn DataField="telefone" SortExpression="telefone" HeaderText="Telef.Emp."
                    ReadOnly="True"></asp:BoundColumn>
              <%--  <asp:BoundColumn DataField="estadoEmpresa" SortExpression="estadoEmpresa" HeaderText="<%$ Resources:Resource, EstadoEmpresa %>"
                    ReadOnly="True"></asp:BoundColumn>--%>
                <asp:BoundColumn DataField="departamento" SortExpression="departamento" HeaderText="<%$ Resources:Resource, Departamento %>"
                    ItemStyle-Width="50"></asp:BoundColumn>
                <asp:BoundColumn DataField="cargo" SortExpression="cargo" HeaderText="<%$ Resources:Resource, Funcao %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="telefoneEmpresa" SortExpression="telefoneEmpresa" HeaderText="Telef.Contacto" ItemStyle-Wrap="false">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="faxEmpresa" SortExpression="faxEmpresa" HeaderText="<%$ Resources:Resource, Fax %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="emailEmpresa" SortExpression="emailEmpresa" HeaderText="<%$ Resources:Resource, Email %>"
                    ItemStyle-Wrap="True" ItemStyle-Width="80"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Fact %>" SortExpression="bFacturacao">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# strX(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bFacturacao"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chbFacturacao" runat="server"></asp:CheckBox></EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Orc %>" SortExpression="bOrcamento">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# strX(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bOrcamento"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chbOrcamento" runat="server"></asp:CheckBox></EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Qual %>" SortExpression="bQualidade">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# strX(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bQualidade"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chbQualidade" runat="server"></asp:CheckBox></EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Manut %>" SortExpression="bManutencao">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# strX(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bManutencao"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chbManutencao" runat="server"></asp:CheckBox></EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Req %>" SortExpression="bRequisicoes">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# strX(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bRequisicoes"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chbRequisicoes" runat="server"></asp:CheckBox></EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Labm.Online" SortExpression="bCertificados">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# strX(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bCertificados"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chbCertificados" runat="server"></asp:CheckBox></EditItemTemplate>
                </asp:TemplateColumn>
              
                <asp:HyperLinkColumn DataNavigateUrlFormatString="FormContacto.aspx?btn=EMP&id={0}"
                    DataNavigateUrlField="idContacto" Target="_self" ItemStyle-Width="40" Text="<%$ Resources:Resource, Ver %>"
                    HeaderText="<%$ Resources:Resource, Detalhes %>">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                </asp:HyperLinkColumn>
               <%-- <asp:EditCommandColumn EditText="<%$ Resources:Resource, Editar %>" UpdateText="<%$ Resources:Resource, Alterar %>"
                    CancelText="<%$ Resources:Resource, Cancelar %>" ItemStyle-Width="40"></asp:EditCommandColumn>--%>
            </Columns>
        </asp:DataGrid>
        <br />
        <span>
            <%= Resources.Resource.Legenda %>:<br />
            <%= Resources.Resource.Fact %>-<%= Resources.Resource.Facturacao %>
            <br />
            <%= Resources.Resource.Orc %>-<%= Resources.Resource.Orcamento %>
            <br />
            <%= Resources.Resource.Qual %>-<%= Resources.Resource.Qualidade %>
            <br />
            <%= Resources.Resource.Manut %>-<%= Resources.Resource.Manutencao %>
            <br />
            <%= Resources.Resource.Req %>-<%= Resources.Resource.Requisicoes %>
            <br />
            <%= Resources.Resource.Certif %>-<%= Resources.Resource.AcessoCertificados %>
            <br />
        </span>
    </fieldset>
</asp:Content>
