<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelatoriosIgae.aspx.cs"
    Inherits="LabMetro.RelatoriosIgae" MasterPageFile="~/mp.Master" %>

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
        <legend>Relatórios de Actividade</legend>
        <asp:SqlDataSource ID="dsrc_concelhos" SelectCommand="select idConcelho, descricao from concelho order by 2"
            runat="server" ConnectionString="<%$ ConnectionStrings:connstr %>"></asp:SqlDataSource>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDtInicio" runat="server" Width="30" MaxLength="10">Data Início:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtInicio" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblDtFim" runat="server" Width="30" MaxLength="10">Data Fim:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtFim" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    Município:
                    <asp:DropDownList ID="ddConcelho" runat="server" DataValueField="idConcelho" DataTextField="descricao"
                        DataSourceID="dsrc_concelhos" AppendDataBoundItems="true">
                        <asp:ListItem Text="todos" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    Tipo de equipamento
                </td>
            </tr>
        </table>
        <ul>
            <li>Lista de equipamentos verificados, com municipio e resultado e data
                <asp:Button ID="button1" runat="server" OnClick="button1_Click" Text="ver" CssClass="button" /></li>
            <li>Lista de Verificações agrupados por município, tipo, estado em relação à verificação
                <asp:Button ID="button2" runat="server" OnClick="button2_Click" Text="ver"  CssClass="button" /></li>
            <li>Verificações aprovadas e reprovadas entre 2 datas por município
                <asp:Button ID="button3" runat="server" OnClick="button3_Click" Text="ver"  CssClass="button" /></li>
            <li>Plano de Verificações entre 2 datas, agrupado por muncípio
                <asp:Button ID="button4" runat="server" OnClick="button4_Click" Text="ver"  CssClass="button" /></li>
        </ul>
    </fieldset>
</asp:Content>
