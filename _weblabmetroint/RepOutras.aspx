<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepOutras.aspx.cs" Inherits="LabMetro.RepOutras" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
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
        <legend>Outros reports</legend>
          <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
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
                <td id="Td1" colspan="4" runat="server">
                    <asp:Label ID="lblGrandeza" runat="server"><%=Resources.Resource.Grandeza %>:</asp:Label>&nbsp;<asp:DropDownList
                        ID="ddGrandeza" runat="server" DataValueField="idGrandeza" DataTextField="descricao">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblEmpresa" runat="server"><%=Resources.Resource.Empresa %>:</asp:Label>&nbsp;<asp:TextBox
                        ID="txtEmpresa" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblValorMin" runat="server"><%=Resources.Resource.Valor %>>:</asp:Label><asp:Label
                        ID="lblBRE" runat="server"><%=Resources.Resource.BRE %>::</asp:Label>
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
                        runat="server"><%=Resources.Resource.UltimoAno %>:</asp:Label>
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
                    <asp:Label ID="lblPeriodo" runat="server"><%=Resources.Resource.Periodo %>:</asp:Label>
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblPeriodo" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="<%$ Resources:Resource, Ano %>">&nbsp;Ano</asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, PrimTrimestre %>">&nbsp;1º Semestre</asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, SegTrimestre %>">&nbsp;2º Semestre</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblSuperior" runat="server"><%=Resources.Resource.SuperiorA %> (&euro;):</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSuperior" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblVariacao" runat="server">&nbsp;&nbsp;&nbsp;<%=Resources.Resource.Variacao %> (%):</asp:Label>
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
                <td colspan="4">
                    <asp:RadioButtonList ID="rblReports" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="rblReports_SelectedIndexChanged">
                        <asp:ListItem Value="rptNovasEmpresas" Text="<%$ Resources:Resource, NovasEmpresas %>"></asp:ListItem>
                        <asp:ListItem Value="rptMelhoresEmpresas" Text="<%$ Resources:Resource, MelhoresEmpresas %>"></asp:ListItem>
                        <asp:ListItem Value="rptTrabalhosGranhosPerdidos" Text="<%$ Resources:Resource, TrabalhosGanhosPerdidos %>"></asp:ListItem>
                        <asp:ListItem Value="rptOrcamentos" Text="<%$ Resources:Resource, Orcamentos %>"></asp:ListItem>


                        <asp:ListItem Value="rptOrcamentosAtraso" Text="<%$ Resources:Resource, OrcamentosEmAtraso %>"></asp:ListItem>
                        <asp:ListItem Value="rptOrcamentosMes" Text="<%$ Resources:Resource, NumOrcamentosMes %>"></asp:ListItem>
                        <asp:ListItem Value="rptTempoMedioEsperaOrcamentos" Text="<%$ Resources:Resource, TempoMedioEsperaOrcamentos %>"></asp:ListItem>
                        <asp:ListItem Value="rptEquipamentosPrecoZero" Text="<%$ Resources:Resource, EquipamentosPrecoZero %>"></asp:ListItem>
                        <asp:ListItem Value="rptBresNaoFacturados" Text="<%$ Resources:Resource, BresNaoFacturados %>"></asp:ListItem>
                        <asp:ListItem Value="rptEquipamentoNaoFacturadoAgrupadoBRE" Text="<%$ Resources:Resource, EquipamentoNaoFacturadoAgrupadodBRE %>"></asp:ListItem>
                        <asp:ListItem Value="rptEquipamentoNaoFacturadoAgrupadoEmpresa" Enabled="true" Text="<%$ Resources:Resource, EquipamentoNaoFacturadoAgrupadoEmpresa %>"></asp:ListItem>
                        <asp:ListItem Value="rptEquipamentoNaoFacturadoBRE" Text="<%$ Resources:Resource, EquipamentoNaoFacturadoPorBRE %>"></asp:ListItem>
                        <asp:ListItem Value="rptEquipamentoNaoFacturadoPesquisaEmpresa" Text="<%$ Resources:Resource, EquipamentoNaoFacturadoPesquisaEmpresa %>"></asp:ListItem>
                        <asp:ListItem Value="rptNumEquipamentoNaoFacturado" Text="<%$ Resources:Resource, NumEquipamentosNaoFacturados %>"></asp:ListItem>
                        <asp:ListItem Value="rptNumCalibracoesFuncionario" Text="<%$ Resources:Resource, CalibracoesPorFuncionario %>"></asp:ListItem>
                        <asp:ListItem Value="rptNumCalibracoesServicosFuncionario" Text="<%$ Resources:Resource, NumCalibracoesPorFuncionarioPesquisaServicos %>"></asp:ListItem>
                        <asp:ListItem Value="rptNumCalibracoesTipoEquipamento" Text="<%$ Resources:Resource, NumCalibracoesPorTipoEquipamento %>"></asp:ListItem>
                        <asp:ListItem Value="rptNumCalibracoesFuncionarioTreino" Text="<%$ Resources:Resource, NumCalibracoesPorFuncionarioEmTreino %>"></asp:ListItem>
                        <asp:ListItem Value="rptBresSemRequisicaoNaoFacturados" Text="<%$ Resources:Resource, BresSemRequisicaoNaoFacturados %>"></asp:ListItem>
                            <asp:ListItem Value="rptRepLisbtBRE" Text="Lista BRE p/ BSE"></asp:ListItem>
                         
                            <asp:ListItem Value="rptNovosClientesEntreDatas" Text="Novos Clientes entre datas"></asp:ListItem>
                          <asp:ListItem Value="rptClientesActivosEntreDatas" Text="Clientes activos entre datas"></asp:ListItem>
                         <asp:ListItem Value="rptTaxasServico" Text="Taxas Serviço"></asp:ListItem>
                           <asp:ListItem Value="rptServicosSupervisionados" Text="Serviços Supervisionados"></asp:ListItem>
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
        </asp:DataGrid>
        </fieldset>
</asp:Content>