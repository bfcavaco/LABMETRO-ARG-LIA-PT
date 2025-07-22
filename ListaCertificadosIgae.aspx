<%@ Page Language="C#" CodeBehind="ListaCertificadosIgae.aspx.cs"
    Inherits="LabMetro.ListaCertificadosIgae" AutoEventWireup="True"
EnableEventValidation="false" MasterPageFile="~/mp.Master"  %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Certificados de Verificação</legend>
        <!-- body -->
        <asp:Label ID="lblMessage" runat="server"></asp:Label><br />
        <br />
        <table>
            <tr>
                <td valign="top">
                    <table>
                        <tr>
                            <td>
                                <%=Resources.Resource.Empresa %>:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                                    DataTextField="nomeLoc">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Equipamento:
                                    
                                </td>
                                <td><asp:TextBox ID="txtSearchEquipamento" runat="server"></asp:TextBox>
                                <%=Resources.Resource.Ano %>:
                                    <asp:DropDownList ID="ddAno" runat="server" DataTextField="ano" DataValueField="ano">
                                        <asp:ListItem Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button class="button" ID="btnSearch" runat="server" CssClass="button" Text="<%$ Resources:Resource, Pesquisar %>">
                                    </asp:Button>
                                </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <asp:DataGrid ID="dgCertificados" runat="server" DataKeyField="nomeDocumento" AllowPaging="True"
            PageSize="15" AllowSorting="True" OnPageIndexChanged="DoPaging" OnSortCommand="SortGrid"
            AutoGenerateColumns="False" OnItemDataBound="dgCertificados_ItemDataBound" OnItemCommand="visualisarDocumento">
            <Columns>
                <asp:ButtonColumn Text="visualisarDocumento" ButtonType="LinkButton" DataTextField="nomeDocumento"
                    Visible="False" SortExpression="nomeDocumento" HeaderText="Visualisar Documento"
                    CommandName="Select">
                    <ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
                </asp:ButtonColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="Empresa">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="Ref.Serv.">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtCertificado" SortExpression="dtCertificado" HeaderText="Dt.Certif."
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:BoundColumn DataField="nomeDocumento" SortExpression="nomeDocumento" HeaderText="Nome Documento"
                    Visible="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="equipamento" SortExpression="equipamento" HeaderText="Equipamento">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="numIdentificacao" SortExpression="numIdentificacao" HeaderText="Nº.Ident."
                    Visible="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="numSerie" SortExpression="numSerie" HeaderText="NºSérie"
                    Visible="True"></asp:BoundColumn>
                    <asp:BoundColumn DataField="conformidade" SortExpression="conformidade" HeaderText="Conformidade"
                    Visible="True"></asp:BoundColumn>
            </Columns>
            <PagerStyle Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
