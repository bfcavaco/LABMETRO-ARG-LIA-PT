

<%@ Page Language="c#" CodeBehind="ListaFacturasES.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaFacturasES" MasterPageFile="~/mp.Master" %>




<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">

    <script type="text/javascript">

        var allCheckBoxSelector = '#<%=DGFacturas.ClientID%> input[id*="chkAll"]:checkbox';
        var checkBoxSelector = '#<%=DGFacturas.ClientID%> input[id*="DGFacturas"]:checkbox';

        function ToggleCheckUncheckAllOptionAsNeeded() {
            var totalCheckboxes = $(checkBoxSelector),
         checkedCheckboxes = totalCheckboxes.filter(":checked"),
         noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
         allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);

            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        }

        $(document).ready(function () {
            $(allCheckBoxSelector).live('click', function () {
                $(checkBoxSelector).attr('checked', $(this).is(':checked'));

                ToggleCheckUncheckAllOptionAsNeeded();
            });

            $(checkBoxSelector).live('click', ToggleCheckUncheckAllOptionAsNeeded);

            ToggleCheckUncheckAllOptionAsNeeded();
        });

    </script>

</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">


</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarFacturas %></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeEmpresa" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTipoEquipamento" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumFactura %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumFactura" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NumBRE%>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumBRE" runat="server"></asp:TextBox>
                </td>
                    <tr>
                        <td>
                            <%=Resources.Resource.CriadaPor %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNomeFuncionarioAlteracao" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <%=Resources.Resource.RefServ %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefServico" runat="server"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="3">
                                <asp:Button class="button" ID="btnPesquisa" CssClass="button" runat="server" Text="<%$ Resources:Resource, Pesquisar %>"
                                    OnClick="btnPesquisa_Click"></asp:Button>
                            </td>
                            <td>
                            </td>
                        </tr>
        </table>
        <br />
         <asp:Button id="btnFicheiroAxapta" Text="EXPORTAR (AXAPTA)" runat="server" OnClick="btnFicheiroAxapta_Click" />


        <asp:Button id="btnAbrirFicheiro" Text="FICHERO (AXAPTA)" runat="server" OnClick="btnAbrirFicheiro_Click" />
        <br />
        <asp:DataGrid 
        ID="DGFacturas" 
        runat="server" 
        AutoGenerateColumns="false" 
        AllowPaging="true"
        PageSize="100" 
        AllowSorting="True" 
        OnSortCommand="SortGrid" 
        OnPageIndexChanged="DoPaging"
        DataKeyField="idFactura">
            <Columns>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource,RefVer %>" DataNavigateUrlFormatString="FormFactura.aspx?btn=FAC&id={0}"
                    DataNavigateUrlField="idFactura" DataTextField="refFactura" SortExpression="refFactura"
                    Target="_self">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Left"></ItemStyle>
                </asp:HyperLinkColumn>
               
                <asp:BoundColumn DataField="dtFactura" SortExpression="dtFactura" HeaderText="<%$ Resources:Resource, Data %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="FuncionarioCriacao" SortExpression="FuncionarioCriacao"
                    HeaderText="<%$ Resources:Resource, CriadoPor %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="empContrato" SortExpression="empContrato" HeaderText="<%$ Resources:Resource, EmpFact %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="valorTotalFactura" SortExpression="valorTotalFactura"
                    HeaderText="<%$ Resources:Resource, ValorTotal %>" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dataficheiro" SortExpression="dataficheiro" HeaderText="<%$ Resources:Resource, Ficheiro %>">
                </asp:BoundColumn>
                
                     <asp:TemplateColumn>
                        <HeaderTemplate>
                            (Des)marcar todos<asp:CheckBox runat="server" ID="chkAll" />
                            <%--<asp:CheckBox runat="server" ID="headerSelectCheckbox" ClientIDMode="Static" /> --%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="checkbox"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                
            </Columns>
        </asp:DataGrid>

       


       
               
    </fieldset>
</asp:Content>
