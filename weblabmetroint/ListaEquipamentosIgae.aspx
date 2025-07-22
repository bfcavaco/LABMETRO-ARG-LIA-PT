<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaEquipamentosIgae.aspx.cs" Inherits="LabMetro.ListaEquipamentosIgae" MasterPageFile="~/mp.Master" %>

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
        <legend>Equipamentos sujeitos a Metrologia Legal por Empresa</legend>
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
           <asp:DataGrid 
        ID="DGEquipamentos" 
        runat="server" 
        AutoGenerateColumns="true" 
        AllowPaging="true" 
        PageSize="25" 
        AllowSorting="True" 
        OnPageIndexChanged="DoPaging" 
        OnSortCommand="SortGrid">
            <Columns>
              
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>

