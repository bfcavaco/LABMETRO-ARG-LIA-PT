<%@ Page Language="c#" CodeBehind="ListaErrosOrdensVenda.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaErrosOrdensVenda" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
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
        <legend><%=Resources.Resource.LogsErrosVendasSAP %></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label><br />
        <br />
        <asp:DropDownList ID="ddFicheiros" AutoPostBack="True" DataTextField="name" DataValueField="name"
            runat="server">
        </asp:DropDownList>
        <asp:DataGrid 
        ID="DG" 
        runat="server" 
        OnSortCommand="sortGrid" 
        AllowSorting="True" 
        AllowPaging="False"
        ShowHeader="True" 
        AutoGenerateColumns="False">
            <Columns>
                <asp:BoundColumn DataField="nomeFicheiro" HeaderText="<%$ Resources:Resource, OrdemVenda %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="descricaoErro" HeaderText="<%$ Resources:Resource, Erro %>"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Factura %>" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%# ConverteID(System.Convert.ToString(DataBinder.Eval(Container,"DataItem.nomeFicheiro")))%>'
                            ID="Hyperlink1" Target="new"> <%=Resources.Resource.Ver %>
				
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
