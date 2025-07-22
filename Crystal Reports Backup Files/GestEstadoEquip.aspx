<%@ Page Language="c#" CodeBehind="GestEstadoEquip.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestEstadoEquip" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">

</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisa").focus();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.MudarEstadosServico %></legend>
        <!-- body -->
        <asp:Label ID="lblMessage" runat="server"></asp:Label><br />
        <br />
        <table>
            <tr>
                <td valign="top">
                    <table>
                        <tr>
                            <td>
                                <%=Resources.Resource.Empresa %>:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;<asp:Button
                                    ID="btnEmpresas" runat="server" Text="<%$ Resources:Resource, verEmpresas %>" CssClass="button" CausesValidation="false">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr id="trEmpresa" runat="server">
                            <td style="height: 4px">
                                <%=Resources.Resource.Empresa %>:
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataTextField="nome"
                                    DataValueField="idEmpresa" OnSelectedIndexChanged="ddEmpresa_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 12px" colspan="4">
                                <asp:Label ID="lblEmpresaDevedora" runat="server" CssClass="lblMessage"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=Resources.Resource.Equipamento %>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchEquipamento" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <%=Resources.Resource.NumBRE %> <%=Resources.Resource.Completo %>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchRefBRE" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=Resources.Resource.Estado %>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEstadoOrigem" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button class="button" ID="btnPesquisa" runat="server" CssClass="button" Text="<%$ Resources:Resource, Pesquisar %>"></asp:Button>
                            </td>
                            <td>
                                <asp:Button class="button" ID="btnLimparCampos" runat="server" CssClass="button" Text="<%$ Resources:Resource, LimparPesquisa %>">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <%=Resources.Resource.MudarPara %>:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEstadoDestino" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <%=Resources.Resource.NotaAnulacaoEquipamentos %>.
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:DataGrid 
                    ID="dgEstadosServico" 
                    runat="server"
                    DataKeyField="idServico" 
                    AllowPaging="False" 
                    AllowSorting="True"
                    OnSortCommand="SortGrid" 
                    AutoGenerateColumns="True">
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="checkbox" AutoPostBack="false"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <br />
                    <br />
                    <asp:Button class="button" ID="btnSelectAll" runat="server" CssClass="button" Text="<%$ Resources:Resource, SeleccionarTodos %>">
                    </asp:Button>&nbsp;&nbsp;&nbsp;
                    <asp:Button class="button" ID="btnDeselectAll" runat="server" CssClass="button"
                        Text="<%$ Resources:Resource, LimparTodos %>"></asp:Button>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button class="button_red" ID="btnImprimirEtiquetasCalibracao" 
                        runat="server" Text="<%$ Resources:Resource, ImprimirEtiquetasCalibracao %>" 
                        onclick="btnImprimirEtiquetasCalibracao_Click"></asp:Button>&nbsp;&nbsp;&nbsp;
                    <asp:Button class="button_red" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, MudarEstadosServicosSeleccionados %>"></asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
