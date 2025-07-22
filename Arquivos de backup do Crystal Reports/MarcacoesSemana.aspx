<%@ Page Language="c#" CodeBehind="MarcacoesSemana.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.MarcacoesSemana" MasterPageFile="~/mp.Master" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    <script type="text/javascript">

        $.datepicker.setDefaults({ showButtonPanel: true });

        $("#datepicker").datepicker($.datepicker.regional["pt"]);

        $(function() {

            /*$("#inputDtInicio").datepicker();*/

            /*resumto das tentativas frustradas de pôr isto a funcionar: enquando eu não usava masterpage, o codigo acim funcionava muito bem.
            quando juntei masterpage, por alguma razao o client id ficou alterado, o datepicker funciona no entanto com um input normal sem ser runat server. desta forma como está na linha a seguir, eu refiro.me ao control pela sua classe e não pelo seu id.... e isto funciona*/
            $('.date-input').datepicker();
        });

        /*$(function() {

            $("#txtDataFim").datepicker();
        });  */

    </script>

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
        <legend>Marcações da Semana</legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0033"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Técnico:
                </td>
                <td>
                    <asp:DropDownList ID="ddTecnicoExterior" runat="server" DataTextField="nomeAbreviado"
                        DataValueField="idFuncionario" AutoPostBack="True" OnSelectedIndexChanged="ddTecnicoExterior_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    De:
                </td>
                <td>
                    <asp:TextBox ID="txtStart" runat="server" CssClass="date-input"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStart" ID="REQ1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="Comparevalidator6" runat="server" ControlToValidate="txtStart"
                        Type="Date" Operator="DataTypeCheck">formato errado</asp:CompareValidator>
                </td>
                <td>
                    A:
                </td>
                <td>
                    <asp:TextBox ID="txtEnd" runat="server" CssClass="date-input"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEnd" ID="Requiredfieldvalidator1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="Comparevalidator1" runat="server" ControlToValidate="txtEnd"
                        Type="Date" Operator="DataTypeCheck">formato errado</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button class="button" ID="btnPesquisa" runat="Server" CausesValidation="true"
                        Text="Ver" CssClass="button" Width="100" OnClick="btnPesquisa_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button class="button" ID="btnReport" runat="server" Width="100" CssClass="button"
                        Text="imprimir" CausesValidation="true" Visible="False" OnClick="btnReport_Click">
                    </asp:Button>
                </td>
            </tr>
            </TBODY>
        </table>
        <br />
        <asp:DataGrid 
        ID="DataGrid1" 
        OnSortCommand="SortGrid" 
        AllowSorting="True" 
        AllowPaging="false"
        AutoGenerateColumns="false" 
        runat="server" 
        OnItemDataBound="dg_databound" 
        DataKeyField="idMarcacao">
            <Columns>
                <asp:TemplateColumn HeaderText="Dia(s)" SortExpression="Weekday">
                    <ItemTemplate>
                        <%# diaSemana(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.Weekday"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="Data" DataField="date" SortExpression="date" DataFormatString="{0:dd/MM/yyyy}"
                    ReadOnly="True" ItemStyle-Wrap="False"></asp:BoundColumn>
                      <asp:BoundColumn HeaderText="Período" DataField="periodoMarcacao" SortExpression="periodoMarcacao" 
                    ReadOnly="True" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="empresa"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="morada" SortExpression="morada" HeaderText="Morada<br />Empresa" ReadOnly="True">
                </asp:BoundColumn>
                
                <asp:BoundColumn DataField="localidade" SortExpression="localidade" HeaderText="Localidade<br/>Empresa"
                    ReadOnly="True"></asp:BoundColumn>
                    <asp:BoundColumn DataField="moradaMarcacao" SortExpression="moradaMarcacao" HeaderText="Morada<br />Marcação" ReadOnly="True">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="telefone" SortExpression="telefone" HeaderText="Telefone"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Técnico" SortExpression="funcionario">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.funcionario") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Técnico" SortExpression="funcionario2">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.funcionario2") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:HyperLinkColumn HeaderText="BRE" DataNavigateUrlField="idBRE" Target="_self"
                    DataTextField="BRE" SortExpression="bre"></asp:HyperLinkColumn>
                <asp:TemplateColumn HeaderText="BRE<br />def." SortExpression="bdefinitivo">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bdefinitivo"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Requisicao" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                            ID="Hyperlink1" Target="new">
											<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Orçamento" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:Button class="button" runat="server" CommandName="verOrcamentos" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "idOrcamento")%>'
                            Text="ver" Enabled='<%# ConverteEstadoBotao(DataBinder.Eval(Container, "DataItem.idOrcamento")) %>'
                            ID="Button1"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Camiao" SortExpression="bCamiao" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bCamiao"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Observacoes" SortExpression="obsInternas">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "obsInternas")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:ButtonColumn CommandName="verDadosEmpresaManutencao" HeaderText="Emp.<br />Mant."
                    Text="ver" ItemStyle-Font-Size="8"></asp:ButtonColumn>
                     <asp:BoundColumn HeaderText="<%$ Resources:Resource, Contacto %>" DataField="contacto" SortExpression="contacto"
                    ReadOnly="True"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid><br />
        <asp:DataGrid ID="dgEmpresasManutencao" runat="server" AutoGenerateColumns="True">
        </asp:DataGrid>
    </fieldset>
</asp:Content>
