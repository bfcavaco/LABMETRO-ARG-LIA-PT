<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="LabMetro.Marcas" MasterPageFile="~/mp.Master"%>

 <%@ Register
Assembly="AjaxControlToolkit"
Namespace="AjaxControlToolkit"
TagPrefix="ajaxToolkit" %>



<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    marca <asp:DropDownList ID="ddMarca" runat="server"></asp:DropDownList>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" TargetControlID="ddMarca" PromptText="Marca..." PromptValue="???" Category="Marca" ServicePath="~/wsMarca.asmx" ServiceMethod="GetMarcas">
    </ajaxToolkit:CascadingDropDown>
    
        </asp:Content>
 
 