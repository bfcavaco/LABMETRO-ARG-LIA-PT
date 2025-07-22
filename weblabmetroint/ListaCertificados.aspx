<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web" %>

<%@ Page Language="c#" CodeBehind="ListaCertificados.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaCertificados" EnableEventValidation="false" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">


</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarCertificados %></legend>
        <!-- body -->
        <asp:Label ID="lblMessage" runat="server"></asp:Label><br />
        <br />
        <table >
            <tr>
                <td valign="top">
                    <table>
                        <tr>
                            <td>
                                <%=Resources.Resource.Empresa %>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchEmpresa" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <%=Resources.Resource.Grandeza %>:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddGrandeza" runat="server" AutoPostBack="True" DataTextField="descricao"
                                    DataValueField="idGrandeza">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <%=Resources.Resource.RefServico %>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchNServico" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <%=Resources.Resource.NumBRE%>:
                            </td>  <td>
                                <asp:TextBox ID="txtSearchBRE" runat="server"></asp:TextBox>
                               
                                </td>
                            <td> <%=Resources.Resource.Ano %>:</td>
                          
                              <td>
                                    <asp:DropDownList ID="ddAno" runat="server" DataTextField="ano" DataValueField="ano">
                                        <asp:ListItem Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="1">
                                    <asp:Button class="button" ID="btnSearch" runat="server" CssClass="button" Text="<%$ Resources:Resource, Pesquisar %>">
                                    </asp:Button>
                                </td>
                             </td>
                        </tr>
                    </table>
               
        <br />
     
        <asp:DataGrid 
        ID="dgCertificados" 
        runat="server"
        DataKeyField="nomeDocumento"
        AllowPaging="True" 
        PageSize="15" 
        AllowSorting="True" 
        OnPageIndexChanged="DoPaging"
        OnSortCommand="SortGrid"
        AutoGenerateColumns="False" 
        OnItemDataBound="dgCertificados_ItemDataBound"
        OnItemCommand="visualisarDocumento">
            <Columns>
                <asp:ButtonColumn Text="visualisarDocumento" ButtonType="LinkButton" DataTextField="nomeDocumento"
                    Visible="False" SortExpression="nomeDocumento" HeaderText="Visualisar Documento"
                    CommandName="Select">
                    <ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
                </asp:ButtonColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="Empresa">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="equipamento" SortExpression="equipamento" HeaderText="Equipamento">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="Ref.Calib.">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtCertificado" SortExpression="dtCertificado" HeaderText="Data de Emiss&#227;o"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:BoundColumn DataField="responsavelLaboratorio" SortExpression="responsavelLaboratorio"
                    HeaderText="Aprovado Por" ItemStyle-Width="20%"></asp:BoundColumn>
                <asp:BoundColumn DataField="nomeDocumento" SortExpression="nomeDocumento" HeaderText="Nome Documento"
                    Visible="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="numIdentificacao" SortExpression="numIdentificacao" HeaderText="Nº.Ident."
                    Visible="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="numSerie" SortExpression="numSerie" HeaderText="NºSérie"
                    Visible="True"></asp:BoundColumn>
            </Columns>
            <PagerStyle Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>

        <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Exportar p/Excel" />
        <asp:GridView ID="gvCertificados" runat="server" AutoGenerateColumns="false">
        <Columns>
        <asp:BoundField DataField="empresa" HeaderText="Empresa" />
       
       
        <asp:BoundField DataField="refServico" HeaderText="Serviço" />
        <asp:BoundField DataField="nomeDocumento" HeaderText="Certificado" />
        <asp:BoundField DataField="dtCertificado" HeaderText="dt.Emissao" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="responsavelLaboratorio" HeaderText="Aprovado Por"  />
        <asp:BoundField DataField="equipamento" HeaderText="Tipo" />
        <asp:BoundField DataField="numIdentificacao" HeaderText="NID" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="numSerie" HeaderText="NS" ItemStyle-HorizontalAlign="Left" />
        
        </Columns>
        </asp:GridView>

    </fieldset>
</asp:Content>
