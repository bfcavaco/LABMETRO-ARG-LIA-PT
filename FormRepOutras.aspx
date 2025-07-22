<%@ Page Language="c#" CodeBehind="FormRepOutras.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.FormRepOutras" MasterPageFile="~/mp.Master" EnableViewState="false"%>

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
        <legend>Outras Estatísticas</legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
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
                <td colspan="4" runat="server">
                    <asp:Label ID="lblGrandeza" runat="server">Grandeza/Laboratório:</asp:Label>&nbsp;<asp:DropDownList
                        ID="ddGrandeza" runat="server" DataValueField="idGrandeza" DataTextField="descricao">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblEmpresa" runat="server">Empresa:</asp:Label>&nbsp;<asp:TextBox
                        ID="txtEmpresa" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblValorMin" runat="server">Valor superior a (&euro;):</asp:Label><asp:Label
                        ID="lblBRE" runat="server">BRE:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtValorMin" runat="server"></asp:TextBox><asp:DropDownList ID="ddBRE"
                        runat="server" DataValueField="idBRE" DataTextField="refBRE" Width="100px">
                    </asp:DropDownList>
                </td>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblAnosIni" runat="server">Entre:</asp:Label><asp:Label ID="lblUltimoAno"
                        runat="server">Último ano:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddAnosIni" runat="server" DataValueField="ano" DataTextField="ano"
                        Width="100px">
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Label ID="lblAnosFim" runat="server">e:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddAnosFim" runat="server" DataValueField="ano" DataTextField="ano"
                        Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblPeriodo" runat="server">Período:</asp:Label>
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblPeriodo" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem>&nbsp;Ano</asp:ListItem>
                        <asp:ListItem>&nbsp;1º Semestre</asp:ListItem>
                        <asp:ListItem>&nbsp;2º Semestre</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblSuperior" runat="server">Superior a (&euro;):</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSuperior" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblVariacao" runat="server">&nbsp;&nbsp;&nbsp;Variação (%):</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVariacao" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="4">
                    <asp:DataList ID="dlTipoEquipamento" runat="server" CssClass="DG_cinzento" Width="100%"
                        RepeatDirection="Horizontal" DataKeyField="ident" GridLines="both" BorderColor="#FFFFFF"
                        BorderWidth="2" AlternatingItemStyle-BackColor="#d3d3d3" RepeatColumns="3">
                        <ItemStyle Font-Size="7"></ItemStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chb" runat="server"></asp:CheckBox><%# DataBinder.Eval(Container, "DataItem.descricao") %>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddFuncionario" runat="server" DataValueField="idFuncionario"
                        DataTextField="nome">
                    </asp:DropDownList>
                    <asp:DataList ID="DLFuncionario" runat="server" CssClass="DG_cinzento" Width="100%"
                        DataKeyField="idFuncionario" GridLines="horizontal" BorderColor="#FFFFFF" BorderWidth="2"
                        AlternatingItemStyle-BackColor="#d3d3d3" RepeatColumns="3" ShowFooter="false">
                        <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="checkboxFunc"></asp:CheckBox><%# DataBinder.Eval(Container, "DataItem.nome") %>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                   
                </td>
                <td colspan="4">
                    <asp:RadioButtonList ID="rblReports" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="rblReports_SelectedIndexChanged">
                        <asp:ListItem Value="0">&nbsp;Novas Empresas</asp:ListItem>
                        <asp:ListItem Value="1">&nbsp;Melhores Empresas</asp:ListItem>
                        <asp:ListItem Value="2">&nbsp;Trabalhos Ganhos / Perdidos</asp:ListItem>
                        <asp:ListItem Value="3">&nbsp;Orçamentos</asp:ListItem>
                        <asp:ListItem Value="4">&nbsp;Orçamentos em Atraso</asp:ListItem>
                        <asp:ListItem Value="5">&nbsp;Nº de Orçamentos por Mês</asp:ListItem>
                        <asp:ListItem Value="6">&nbsp;Tempo Médio de Espera dos Orçamentos</asp:ListItem>
                        <asp:ListItem Value="7">&nbsp;Equipamento Facturado com Preço Zero</asp:ListItem>
                        <asp:ListItem Value="8">&nbsp;BRE's Não Facturados</asp:ListItem>
                        <asp:ListItem Value="9">&nbsp;Equipamento Não Facturado Agrupado por BRE</asp:ListItem>
                        <asp:ListItem Value="11">&nbsp;Equipamento Não Facturado Agrupado por Empresa</asp:ListItem>
                        <asp:ListItem Value="10">&nbsp;Equipamento Não Facturado por BRE</asp:ListItem>
                        <asp:ListItem Value="17">&nbsp;Equipamento Não Facturado //PESQUISA POR EMPRESA(s)</asp:ListItem>
                        <asp:ListItem Value="12">&nbsp;Nº de Equipamentos Não Facturados</asp:ListItem>
                        <asp:ListItem Value="13">&nbsp;Nº de Calibrações por Funcionário</asp:ListItem>
                        <asp:ListItem Value="14">&nbsp;Nº de Calibrações por Tipo de Equipamento</asp:ListItem>
                        <asp:ListItem Value="15">&nbsp;Nº de Calibrações por Funcionário em Treino</asp:ListItem>
                        <asp:ListItem Value="16">&nbsp;BREs Sem Requisição Não Facturados</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="5">
                    <asp:Button class="button" ID="btnReport" CssClass="button" runat="server" Text="Ver Relatório"
                        OnClick="btnReport_Click"></asp:Button>
                </td>
            </tr>
        </table>
        <asp:DataGrid ID="dgTeste" runat="server" AutoGenerateColumns="True">
        </asp:DataGrid></fieldset>
</asp:Content>
