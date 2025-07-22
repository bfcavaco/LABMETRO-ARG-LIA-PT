<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepQualidade.aspx.cs" Inherits="LabMetro.RepQualidade" MasterPageFile="~/mp.Master" %>
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
        <legend>Estatísticas Listas</legend>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMessage"
            runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td colspan="5">
                    <div>
                        Percentagem de calibrações c/ certificado calibrados até X dias úteis a partir da
                        data de entrada (= dt.BRE)
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDtInicio" runat="server">Entre:</asp:Label><asp:TextBox ID="txtDtInicio"
                        runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblDtFim" runat="server">e:</asp:Label><asp:TextBox ID="txtDtFim"
                        runat="server"></asp:TextBox>
                </td>
                <td>
                    Dias:
                    <asp:TextBox ID="txNumDias" runat="server" Width="20px"></asp:TextBox>
                </td>
                <td>
                    <asp:CompareValidator ID="Comparevalidator1" runat="server" Operator="DataTypeCheck"
                        Type="Integer" ControlToValidate="txNumDias">*</asp:CompareValidator><asp:Button
                            class="button" ID="btnRecalcular" runat="server" CssClass="button" Text="Recalcular Totais">
                        </asp:Button>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:DataGrid ID="dgTotais" runat="server" CellPadding="3" CellSpacing="2" AutoGenerateColumns="False"
                        ShowFooter="false" OnItemDataBound="dgTotais_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn HeaderText="Grandeza/Laboratório" DataField="descGrandeza" HeaderStyle-HorizontalAlign="Left"
                                SortExpression="descGrandeza" ItemStyle-BackColor="#666666" ItemStyle-ForeColor="#FFFFFF">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="totalServicosInternos" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-BackColor="#cccccc"></asp:BoundColumn>
                            <asp:BoundColumn DataField="parcialDiasInternos" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-BackColor="#cccccc"></asp:BoundColumn>
                            <asp:BoundColumn DataField="percentagemInterna" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-BackColor="#cccccc" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="totalServicosExternos" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-BackColor="#ffcc33"></asp:BoundColumn>
                            <asp:BoundColumn DataField="parcialDiasExternos" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-BackColor="#ffcc33"></asp:BoundColumn>
                            <asp:BoundColumn DataField="percentagemExterna" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-BackColor="#ffcc33" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="totalServicos" ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#ff9933">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="parcialDias" ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#ff9933">
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Renovável" SortExpression="bRenovavel" ItemStyle-BackColor="#ff9933"
                                ItemStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Calculapercentagem(Convert.ToDouble(DataBinder.Eval(Container, "DataItem.totalServicos")), Convert.ToDouble(DataBinder.Eval(Container, "DataItem.parcialDias"))) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="idGrandeza" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#ff9933">
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Button class="button" ID="btnReport" runat="server" CssClass="button" Text="Ver Gráfico" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <div>
                        Resultados detalhados<br />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    Duração máxima<br />
                    a visualizar:
                </td>
                <td>
                    <asp:DropDownList ID="ddDias" runat="server" DataValueField="numDiasUteis" DataTextField="numDiasUteis">
                    </asp:DropDownList>
                </td>
                <td>
                    Filtrar por<br />
                    grandeza:
                </td>
                <td>
                    <asp:DropDownList ID="ddGrandeza" runat="server" DataValueField="idGrandeza" DataTextField="descricao">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="5">
                    <asp:Button class="button" ID="btnServicos" runat="server" CssClass="button" Text="Detalhes Serviços">
                    </asp:Button><asp:Button class="button" ID="btnDias" runat="server" CssClass="button"
                        Text="Detalhes Dias" Visible="False"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:DataGrid 
                    ID="dgDias" 
                    runat="server"
                    AutoGenerateColumns="False" 
                    Width="568px"
                    AllowSorting="False" 
                    ShowFooter="false" >
                        <Columns>
                            <asp:BoundColumn DataField="Grandeza" HeaderText="Grandeza/Laboratório" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                            <asp:BoundColumn DataField="numServicos" HeaderText="Nº Serviços"></asp:BoundColumn>
                            <asp:BoundColumn DataField="numDiasUteis" HeaderText="Nº Dias Úteis"></asp:BoundColumn>
                            <asp:BoundColumn></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:DataGrid 
                    ID="dgServicos" 
                    runat="server"
                    AutoGenerateColumns="False" 
                    Width="568px"
                    AllowSorting="False" 
                    ShowFooter="false" 
                    PageSize="25"
                    OnPageIndexChanged="doPagingServicos"
                    AllowPaging="False" 
                    OnSelectedIndexChanged="dgServicos_SelectedIndexChanged">
                        <Columns>
                            <asp:HyperLinkColumn HeaderText="Ref.Cal.<br />" DataNavigateUrlFormatString="FormPastaEnsaio.aspx?id={0}"
                                DataNavigateUrlField="idServico" Target="_new" DataTextField="refServico" SortExpression="refServico"
                                HeaderStyle-HorizontalAlign="Left"></asp:HyperLinkColumn>
                            <asp:BoundColumn DataField="dtBRE" HeaderText="dt. BRE" HeaderStyle-HorizontalAlign="Left"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                         
                            <asp:BoundColumn DataField="dtCalibracao" HeaderText="dt Calib." HeaderStyle-HorizontalAlign="Left"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Calib.<br />Ext." SortExpression="calibracaoExterna">
                                <ItemTemplate>
                                    <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.calibracaoExterna"))) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="numDiasUteis" HeaderText="Nº Dias Úteis" HeaderStyle-HorizontalAlign="Left">
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>