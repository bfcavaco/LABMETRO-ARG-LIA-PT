<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZipCertificados.aspx.cs"
    Inherits="LabMetro.ZipCertificados" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    <script type="text/javascript">
     
        var allCheckBoxSelector = '#<%=gvCertificados.ClientID%> input[id*="chkAll"]:checkbox';
        var checkBoxSelector = '#<%=gvCertificados.ClientID%> input[id*="chkSelected"]:checkbox';

        function ToggleCheckUncheckAllOptionAsNeeded() {
            var totalCheckboxes = $(checkBoxSelector),
         checkedCheckboxes = totalCheckboxes.filter(":checked"),
         noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
         allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);

            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        }

        $(document).ready(function() {
            $(allCheckBoxSelector).live('click', function() {
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
        <legend>
            <%=Resources.Resource.PesquisarCertificados %></legend>
        <!-- body -->
        <asp:Label ID="lblMessage" runat="server"></asp:Label><br />
        <br />
        
        <table>
           
            <tr>
                <td colspan="5">
                    <asp:Label ID="Label1" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true" OnTextChanged="txtPesquisaEmpresa_TextChanged"></asp:TextBox>
                </td>
                
                                <td>
                    <asp:Button class="button" ID="btnPesquisaEmpresa" runat="server" Text="<%$ Resources:Resource, VerEmpresas %>"
                        CssClass="button" OnClick="btnPesquisaEmpresa_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa"
                        AutoPostBack="true" OnSelectedIndexChanged="ddEmpresa_SelectedIndexChanged">
                    </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr><td>Grandeza</td><td colspan="2"><asp:DropDownList ID="ddGrandeza" runat="server" DataValueField="idGrandeza" DataTextField="descricao">
            </asp:DropDownList></td></tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumBRE %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddBRE" runat="server" DataTextField="refBRE" DataValueField="idBre"></asp:DropDownList><asp:RequiredFieldValidator runat="server" ControlToValidate="ddBre">*</asp:RequiredFieldValidator>
                      <%--<asp:TextBox ID="txtSearchBRE" runat="server"></asp:TextBox>--%>
                </td>
                <td>
                    <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, Pesquisar %>"
                        CausesValidation="true" CssClass="button" OnClick="btnPesquisa_Click"></asp:Button>
                </td>
            </tr>
        </table>
       
        <br />
        <asp:GridView ID="gvCertificados" runat="server" CellPadding="4"
            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
            DataKeyNames="nomeDocumento">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        (Des)marcar todos<asp:CheckBox runat="server" ID="chkAll" />
                        <%--<asp:CheckBox runat="server" ID="headerSelectCheckbox" ClientIDMode="Static" /> --%>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkSelected" />
                       <%-- <asp:CheckBox runat="server" ID="rowSelectCheckbox" /> --%>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="nomeDocumento" HeaderText="Name" />
                <asp:BoundField DataField="dtCertificado" SortExpression="dtCertificado" HeaderText="Data de Emiss&#227;o"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
            </Columns>
        </asp:GridView><br />
                <asp:Button ID="btnDownloadNow" runat="server" Text="Gravar" 
            onclick="btnDownloadNow_Click" />
      
        <br />
      
        <asp:DataGrid ID="dgCertificados" runat="server" DataKeyField="nomeDocumento"                      AllowPaging="True"
            PageSize="15" AllowSorting="True" 
            AutoGenerateColumns="False" >
            <Columns>
                <asp:ButtonColumn Text="visualisarDocumento" ButtonType="LinkButton" DataTextField="nomeDocumento"
                    Visible="False" SortExpression="nomeDocumento" HeaderText="Visualisar Documento"
                    CommandName="Select">
                    <ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
                </asp:ButtonColumn></Columns></asp:DataGrid>
               
    </fieldset>
</asp:Content>
