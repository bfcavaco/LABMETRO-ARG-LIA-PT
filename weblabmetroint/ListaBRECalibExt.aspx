<%@ Page Language="c#" CodeBehind="ListaBRECalibExt.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaBRECaliExt" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">



</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarBREsExternos%></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true" OnTextChanged="txtPesquisaEmpresa_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true" OnTextChanged="txtPesquisaNif_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <asp:Button class="button" ID="btnPesquisaEmpresa" runat="server" Text="<%$ Resources:Resource, VerEmpresas %>"
                        CssClass="button" OnClick="btnPesquisaEmpresa_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa"
                        AutoPostBack="true" OnSelectedIndexChanged="ddEmpresa_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumBRE %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumBre" runat="server"></asp:TextBox>
                </td>
                <td colspan="3">
                    <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CausesValidation="true"
                        CssClass="button" OnClick="btnPesquisa_Click"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:DataGrid 
        ID="DGBRE" 
        runat="server" 
        AutoGenerateColumns="false" 
        AllowPaging="true"
        PageSize="25" 
        AllowSorting="True" 
        OnSortCommand="SortGrid" 
        OnPageIndexChanged="DoPaging"
        PagerStyle-Mode="NumericPages" 
        DataKeyField="iDBRE" >
            <Columns>
                <asp:BoundColumn DataField="refBRE" SortExpression="refBRE" HeaderText="<%$ Resources:Resource, Ref %>"></asp:BoundColumn>
                <asp:TemplateColumn SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                    <ItemTemplate>
                        <asp:TextBox ID="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.nivelBloqueioLabmetro")))%>'
                            Width="10" Height="10" runat="server">
                        </asp:TextBox>&nbsp;
                        <%# DataBinder.Eval(Container, "DataItem.empresa")%>
                        </asp:textbox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="dtBRE" SortExpression="dtBRE" HeaderText="<%$ Resources:Resource, Data %>" DataFormatString="{0:dd/MM/yyyy}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="entreguePor" SortExpression="entreguePor" HeaderText="<%$ Resources:Resource, PedidoPor %>">
                </asp:BoundColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Detalhes %>" Text="<%$ Resources:Resource, Ver %>" DataNavigateUrlFormatString="FormBRECalibExt.aspx?btn=DOC&id={0}"
                    DataNavigateUrlField="idBRE" Target="_self">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
