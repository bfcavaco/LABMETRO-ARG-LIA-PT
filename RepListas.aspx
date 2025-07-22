<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepListas.aspx.cs" Inherits="LabMetro.RepListas"
    MasterPageFile="~/mp.Master" %>

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
    <fieldset>
        <legend><%=Resources.Resource.ReportsListas %></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDtInicio" runat="server"><%=Resources.Resource.DataInicio %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtInicio" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblDtFim" runat="server"><%=Resources.Resource.DataFim %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtFim" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblValorMin" runat="server"><%=Resources.Resource.Valor %> > (&euro;):</asp:Label>
                    <asp:Label ID="lblEstado" runat="server"><%=Resources.Resource.Estado %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtValorMin" runat="server"> </asp:TextBox>
                    <asp:DropDownList ID="ddEstado" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblGrandeza" runat="server"><%=Resources.Resource.Grandeza %></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddGrandeza" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPesquisaEmpresa" runat="server">
									<%=Resources.Resource.PesquisarEmpresa %></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtEmpresa" runat="server" AutoPostBack="True" OnTextChanged="txtEmpresa_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEmpresa" runat="server"><%=Resources.Resource.Empresa %>:</asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataList RepeatColumns="3" runat="server" ID="dlTipoEquipamento" Width="100%"
                        BackColor="#d3d3d3" BorderWidth="2" BorderColor="#FFFFFF" GridLines="both" DataKeyField="ident"
                        RepeatDirection="Horizontal">
                        <ItemStyle Font-Size="7"></ItemStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chb" runat="server"></asp:CheckBox><%# DataBinder.Eval(Container, "DataItem.descricao") %>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:RadioButtonList ID="rblReports" runat="server" AutoPostBack="True" OnSelectedIndexChanged="OnSelectedIndexChangedMethod">
                        <asp:ListItem Value="2" Text="<%$ Resources:Resource, NumDiasEspera %>"></asp:ListItem>
                        <asp:ListItem Value="3" Text="<%$ Resources:Resource, NumEntradasTempoMedioEspera %>"></asp:ListItem>
                        <asp:ListItem Value="4"  Text="<%$ Resources:Resource, EquipamentoCalibrado %>"></asp:ListItem>
                        <asp:ListItem Value="5" Text="<%$ Resources:Resource, EquipamentoNaoCalibraco %>"></asp:ListItem>
                        <asp:ListItem Value="6" Text="<%$ Resources:Resource, EstadoEquipamento %>"></asp:ListItem>
                        <asp:ListItem Value="7" Text="<%$ Resources:Resource, EquipamentoPorEstadoConGrandeza %>"></asp:ListItem>
                        <asp:ListItem Value="8" Text="<%$ Resources:Resource, PlanoCalibracao %>"></asp:ListItem>
                        <asp:ListItem Value="9" Text="<%$ Resources:Resource, EquipamantoACalibrarPlanoCal %>"></asp:ListItem>
                        <asp:ListItem Value="10" Text="<%$ Resources:Resource, CalibracoesEmAtrasoPlanoCal %>"></asp:ListItem>
                        <asp:ListItem Value="11" Text="<%$ Resources:Resource, EmpresasComBloqueio %>"></asp:ListItem>
                        <asp:ListItem Value="12" Text="<%$ Resources:Resource, CodigosSAP %>"></asp:ListItem>
                        <asp:ListItem Value="13" Text="<%$ Resources:Resource, EquipamentoPorEstadoFacturarAngola %>"></asp:ListItem>
                        
                        
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center">
                    <asp:Button class="button" ID="btnReport" runat="server" Text="<%$ Resources:Resource, Ver %>" OnClick="btnReport_Click">
                    </asp:Button><br />
                    <asp:Button ID="btnExcel" class="button" runat="server" OnClick="Button1_Click" Text="Plano Calibracao em excel" />
                    <asp:GridView ID="gv" runat="server" AutoGenerateColumns="true" />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
