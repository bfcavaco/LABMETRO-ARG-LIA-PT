<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaContactosIgae.aspx.cs"
    Inherits="LabMetro.ListaContactosIgae" MasterPageFile="~/mp.Master" %>

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
        <legend>Contactos de Empresas Metrologia Legal por Empresa</legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                        DataTextField="nomeLoc">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button class="button" ID="btnSubmit" runat="server" CssClass="button" Text="<%$ Resources:Resource, pesquisar %>">
                    </asp:Button>
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
            AutoGenerateColumns="false"  ShowFooter="True">
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
                <asp:BoundColumn DataField="nome" HeaderText="Nome"></asp:BoundColumn>
                <asp:BoundColumn DataField="cargo" HeaderText="Cargo"></asp:BoundColumn>
                <asp:BoundColumn DataField="departamento" HeaderText="Departamento"></asp:BoundColumn>
                <asp:BoundColumn DataField="telefoneEmpresa" HeaderText="Telefone"></asp:BoundColumn>
                <asp:BoundColumn DataField="emailEmpresa" HeaderText="Email"></asp:BoundColumn>
                <asp:BoundColumn DataField="faxEmpresa" HeaderText="Fax"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
