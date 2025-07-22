<%@ Page Language="c#" CodeBehind="FormRepListas.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.FormRepListas" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    <!-- %@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %  isto estava no topo -->
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnReport").focus();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Estatísticas Listas</legend>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMessage"
            runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDtInicio" runat="server">Data Início:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtInicio" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblDtFim" runat="server">Data Fim:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtFim" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblValorMin" runat="server">Valor superior a (&euro;):</asp:Label>
                    <asp:Label ID="lblEstado" runat="server">Estado:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtValorMin" runat="server"></asp:TextBox>
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
                    <asp:Label ID="lblGrandeza" runat="server">Laboratório / Grandeza:</asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddGrandeza" runat="server" DataTextField="descricao" DataValueField="ident"
                        Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPesquisaEmpresa" runat="server">
									Pesquisar Empresa:</asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtEmpresa" runat="server" AutoPostBack="True" OnTextChanged="txtEmpresa_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEmpresa" runat="server">Empresa:</asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataList RepeatColumns="3" runat="server" ID="dlTipoEquipamento" Width="100%" BackColor="#d3d3d3" BorderWidth="2" BorderColor="#FFFFFF"
                        GridLines="both" DataKeyField="ident" RepeatDirection="Horizontal">
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
                        <asp:ListItem Value="2">&nbsp;Nº de Dias de Espera</asp:ListItem>
                        <asp:ListItem Value="3">&nbsp;Nº de Entradas e Tempo Médio de Espera</asp:ListItem>
                        <asp:ListItem Value="4">&nbsp;Equipamento Calibrado</asp:ListItem>
                        <asp:ListItem Value="5">&nbsp;Equipamento Não Calibrado</asp:ListItem>
                        <asp:ListItem Value="6">&nbsp;Estados dos Equipamentos</asp:ListItem>
                        <asp:ListItem Value="7">&nbsp;Equipamentos por Estado (c/ pesquisa p/Grandeza)</asp:ListItem>
                        <asp:ListItem Value="8">&nbsp;Plano de Calibração</asp:ListItem>
                        <asp:ListItem Value="9">&nbsp;Equipamentos a Calibrar (plano cal.)</asp:ListItem>
                        <asp:ListItem Value="10">&nbsp;Calibrações em Atraso (plano cal.)</asp:ListItem>
                        <asp:ListItem Value="11">&nbsp;Empresas com bloqueio(falidas, em contencioso, c/ pag.atraso ou requis. atraso)</asp:ListItem>
                        <asp:ListItem Value="12">&nbsp;Códigos Laboratórios (CC, PEP, Reg.Vendas)</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            
            <tr>
                <td colspan="5" align="center">
                    <asp:Button class="button" ID="btnReport" runat="server" Text="Ver Report" CssClass="button"
                        OnClick="btnReport_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
