<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaEmpresasIgae.aspx.cs"
    Inherits="LabMetro.ListaEmpresasIgae" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>
           Empresas Clientes Metrologia Legal</legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="10%">
                    <label>
                        <%=Resources.Resource.Nome %>:</label><br />
                </td>
                <td width="30%">
                    <asp:TextBox ID="txtNomeEmpresa" runat="server" AutoPostBack="true" Width="150"></asp:TextBox>
                </td>
                <td width="25%">
                    <label>
                        <%=Resources.Resource.NIF %>:</label>
                </td>
                <td width="25%">
                    <asp:TextBox ID="txtNIF" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
              
                <td>
                    <label>
                        Município:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddConcelho" runat="server" DataValueField="idConcelho" DataTextField="descricao"
                        DataSourceID="dsrc_concelhos" AppendDataBoundItems="false"  OnDataBound="ddConcelhoDataBound" >
                    </asp:DropDownList>
                </td>
                  <td>
                    
                </td>
                <td>
                    
                </td>
            </tr>
           
            <tr>
                <!--so tem 2 td's visto haver um rowspan em cima-->
                <td align="center" colspan="4">
                    <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, ListarEmpresas %>">
                    </asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:SqlDataSource ID="dsrc_concelhos" SelectCommand="select idConcelho, descricao from concelho order by 2"
        runat="server" ConnectionString="<%$ ConnectionStrings:connstr %>">
      
    </asp:SqlDataSource>
  
    <asp:SqlDataSource ID="dsrc_actividades" SelectCommand="select idActividade, descricao from actividade order by 2"
        runat="server" ConnectionString="<%$ ConnectionStrings:connstr %>"></asp:SqlDataSource>
 
    <br />
    <br />
    <asp:DataGrid ID="DGEmpresas" runat="server" DataKeyField="idEmpresa" AutoGenerateColumns="false"
        PageSize="25" AllowSorting="True" AllowPaging="true" OnSortCommand="SortGrid"
        OnPageIndexChanged="DoPaging">
        <Columns>
           
            <asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="nome" />
            <asp:BoundColumn DataField="nif" SortExpression="nif" HeaderText="NIF" />
            <asp:BoundColumn DataField="morada" SortExpression="morada" HeaderText="Morada" />
            <asp:BoundColumn DataField="localidade" SortExpression="localidade" HeaderText="Localidade" />
            <asp:BoundColumn DataField="concelho" SortExpression="concelho" HeaderText="Municip" />
            <asp:BoundColumn DataField="actividade" SortExpression="actividade" HeaderText="Actividade" />
            <asp:BoundColumn DataField="telefone" SortExpression="telefone" HeaderText="Telefone" />
            <asp:BoundColumn DataField="fax" SortExpression="fax" HeaderText="Fax" ItemStyle-Wrap="False" />
            <asp:BoundColumn DataField="email" SortExpression="email" HeaderText="email" ItemStyle-Wrap="False">
            </asp:BoundColumn>
            
        </Columns>
    </asp:DataGrid>
</asp:Content>
