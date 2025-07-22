<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestMarcasModelos.aspx.cs"
    Inherits="LabMetro.GestMarcasModelos" MasterPageFile="~/mp.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="System.Web.Services" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <!--ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" TargetControlID="ddMarca"
        Category="Marca" PromptText="Marca..." ServicePath="~/Webservices/wsMarcaModelo.asmx"
        ServiceMethod="GetMarcas" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown2" runat="server" TargetControlID="ddModelo"
        ParentControlID="ddMarca" PromptText="Modelo..." ServiceMethod="GetModelosByMarca"
        ServicePath="~/Webservices/wsMarcaModelo.asmx" Category="Marca" /-->
    <br />
    <br />
    Marca:
    <asp:DropDownList ID="ddMarca" runat="server" />
    <br />
    Modelo:
    <asp:DropDownList ID="ddModelo" runat="server" />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Submit" />
    <fieldset>
        <legend>Limpar Marcas/Modelos</legend>
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td>
                    Marca (a manter):<asp:DropDownList ID="ddMarcaManter" runat="server" AutoPostBack="true"
                        DataValueField="idMarca" DataTextField="descricao" OnSelectedIndexChanged="ddMarca_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td> Marca a apagar:<asp:DropDownList ID="ddMarcaApagar" runat="server" AutoPostBack="true"
                        DataValueField="idMarca" DataTextField="descricao" Width="175px" OnSelectedIndexChanged="ddMarca_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
             <tr>
                <td colspan="2">
                    ao apagar uma marca, todos os modelos vão passar dessa marca para a marca a manter,
                    devendo ser limpos de seguida.
                </td>
            </tr>
             <tr>
                <td colspan="2">
                    <asp:Button ID="btnMarcas" runat="server" OnClick="btnMarcas_Click" Text="limpar marca" />
                </td>
            </tr>
            <tr>
                <td>
                    Modelo a manter:
                    <asp:DropDownList ID="ddModeloManter" runat="server" DataValueField="ident" DataTextField="descricao"
                        Width="175px">
                    </asp:DropDownList>
                </td>
                <td>
                    Modelo a apagar:<asp:DropDownList ID="ddModeloApagar" runat="server" DataValueField="ident"
                        DataTextField="descricao" Width="175px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnModelos" runat="server" OnClick="btnModelos_Click" Text="limpar modelo" />
                </td>
            </tr>
           
        </table>
    </fieldset>
</asp:Content>
