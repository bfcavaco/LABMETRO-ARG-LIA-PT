<%@ Page Language="c#" CodeBehind="GestOrcamentos.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestOrcamentos" MasterPageFile="~/mp.Master" %>

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
        <legend>
            <%=Resources.Resource.ControlOrcamentos %></legend>
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label><br />
        <br />
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtEmpresa" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.TipoEmpresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTipoEquipamento" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.ReferenciaOrcamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefOrcamento" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.EstadoOrcamento %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddEstadoOrcamento" runat="server" DataValueField="ident" DataTextField="descricao">
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDtInicio" runat="server"><%=Resources.Resource.DataInicio %>:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDtInicio" runat="server"></asp:TextBox><asp:CompareValidator
                            ID="Comparevalidator1" runat="server" ControlToValidate="txtDtInicio" Type="Date"
                            Operator="DataTypeCheck"><%= Resources.Resource.FormatoErrado %></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:Label ID="lblDtFim" runat="server"><%=Resources.Resource.DataFim %>:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDtFim" runat="server"></asp:TextBox><asp:CompareValidator ID="Comparevalidator2"
                            runat="server" ControlToValidate="txtDtFim" Type="Date" Operator="DataTypeCheck"><%= Resources.Resource.FormatoErrado %></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblValorMin" runat="server"><%=Resources.Resource.ValorSuperior %> (&euro;):</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtValorMinimo" runat="server"></asp:TextBox><asp:CompareValidator
                            ID="Comparevalidator3" runat="server" ControlToValidate="txtValorMinimo" Type="Double"
                            Operator="DataTypeCheck"><%= Resources.Resource.FormatoErrado %></asp:CompareValidator>
                    </td>
                    <td>Calibracao Externa:
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cbCalibracaoExterna" />
                    </td>
                </tr>

                <tr>
                     <td>
                   Razao Cliente
                </td>
                <td>
                    <asp:DropDownList ID="ddRazaoCliente" runat="server" DataValueField="idRazaoCliente" DataTextField="razaoCliente">
                    </asp:DropDownList>
                </td>
                    <td>FollowUP:
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cbFollowup" />
                    </td>
                </tr>
                <tr>
                    <td>Funcionario</td>
                    <td>
                        <asp:DropDownList ID="ddFuncionario" runat="server" DataTextField="descricao" DataValueField="idFuncionario">
                        </asp:DropDownList></td>

                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, Pesquisar %>"
                            CausesValidation="true" CssClass="button" OnClick="btnPesquisa_Click"></asp:Button>
                    </td>
                </tr>
        </table>
        <br />
        <asp:DataGrid BorderColor="#FFFFFF" GridLines="Both" BorderWidth="2" ID="DG" runat="server"
            AutoGenerateColumns="false" AllowPaging="true" PageSize="25" AllowSorting="True"
            OnSortCommand="SortGrid" OnPageIndexChanged="DoPaging" PagerStyle-Mode="NumericPages"
            DataKeyField="idOrcamento" BackColor="Gainsboro" AlternatingItemStyle-BackColor="LightGrey"
            OnItemCommand="dg_ItemCommand" OnUpdateCommand="updateGrid" OnCancelCommand="cancelGrid"
            OnEditCommand="editGrid" OnItemDataBound="DG_ItemDataBound">
            <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
            <FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
            <Columns>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>"
                    ReadOnly="True"></asp:BoundColumn>
                 <asp:BoundColumn DataField="codigoPostal" SortExpression="codigoPostal" HeaderText="<%$ Resources:Resource, CodigoPostal %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="contacto" SortExpression="contacto" HeaderText="<%$ Resources:Resource, Contacto %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="telefone" SortExpression="telefone" HeaderText="<%$ Resources:Resource, Telefone %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="refOrcamento" SortExpression="refOrcamento" HeaderText="<%$ Resources:Resource, RefOrc %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="versao" SortExpression="versao" HeaderText="<%$ Resources:Resource, Versao %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtOrcamento" SortExpression="dtOrcamento" HeaderText="<%$ Resources:Resource, DataOrc  %>"
                    DataFormatString="{0:dd/MM/yyyy}" ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="funcionario" SortExpression="funcionario" HeaderText="<%$ Resources:Resource, Funcionario %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, ValorTotal %>" SortExpression="valorTotal"
                    ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToCurrencyString(DataBinder.Eval(Container, "DataItem.valorTotal")) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Estado %>" SortExpression="estado">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.estado") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEstado" runat="server" DataValueField="ident" DataTextField="descricao"
                            Font-Size="8">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="FollowUP" SortExpression="bFollowup">
                    <EditItemTemplate>
                        <asp:Checkbox ID="checkFollowup" runat="server"/>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Checkbox ID="checkFollowupItem" runat="server" Enabled="false"  Checked='<%# Bind("bFollowup") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="RazaoCliente" SortExpression="razaoCliente">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.razaoCliente") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddRazaoCliente" runat="server" DataValueField="idRazaoCliente" DataTextField="razaoCliente"
                            Font-Size="8">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                  
                <asp:TemplateColumn HeaderText="ObsFollowup" SortExpression="observacoes">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtObsFollowup" runat="server" Width="100%" MaxLength="150"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "obsFollowup")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
      
                <asp:TemplateColumn HeaderText="Obs.Orc." SortExpression="observacoes">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtObs" runat="server" Width="100%" MaxLength="150"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "observacoes")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:ButtonColumn CommandName="verServicos" HeaderText="<%$ Resources:Resource, VerServicos %>"
                    Text="<%$ Resources:Resource, Servicos %>" ItemStyle-Font-Size="8"></asp:ButtonColumn>
                <asp:ButtonColumn CommandName="verRequisicoes" HeaderText="<%$ Resources:Resource, VerRequisicoes %>"
                    Text="<%$ Resources:Resource, Requisicoes %>" ItemStyle-Font-Size="8"></asp:ButtonColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Detalhes %>" Text="ver"
                    DataNavigateUrlFormatString="FormOrcam.aspx?btn=ORC&id={0}" DataNavigateUrlField="idOrcamento"
                    Target="_blank"></asp:HyperLinkColumn>
                <asp:EditCommandColumn EditText="<%$ Resources:Resource, Editar %>" UpdateText="<%$ Resources:Resource, Alterar %>" CancelText="<%$ Resources:Resource, Cancelar %>"></asp:EditCommandColumn>
                <asp:HyperLinkColumn DataNavigateUrlField="emailContacto" Text="mail" DataNavigateUrlFormatString="mailto:{0};"></asp:HyperLinkColumn>
                    
            </Columns>
        </asp:DataGrid><br />
        <br />
        <asp:DataGrid ID="DGServicos" runat="server" AutoGenerateColumns="True">
        </asp:DataGrid>
        <asp:DataGrid ID="DGRequisicoes" runat="server" AutoGenerateColumns="True">
            <Columns>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Ficheiro %>" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                            ID="Hyperlink1" Target="new">
											<%=Resources.Resource.Ver %>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Button ID="btnExcel" Text="Exportar para Excel" runat="server" OnClick="btnExcel_Click" />
        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="empresa" HeaderText="<%$ Resources:Resource, Empresa %>" />
                <asp:BoundField DataField="contacto" HeaderText="<%$ Resources:Resource, Contacto %>" />
                <asp:BoundField DataField="telefone" HeaderText="<%$ Resources:Resource, Telefone %>" />
                <asp:BoundField DataField="refOrcamento" HeaderText="<%$ Resources:Resource, Ref %>" />
                <asp:BoundField DataField="versao" HeaderText="<%$ Resources:Resource, Versao %>" />
                <asp:BoundField DataField="dtOrcamento" HeaderText="<%$ Resources:Resource, Data %>" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="funcionario" HeaderText="<%$ Resources:Resource, Funcionario %>" />
                <asp:BoundField DataField="valortotal" HeaderText="<%$ Resources:Resource, ValorTotal %>" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="estado" HeaderText="<%$ Resources:Resource, Estado %>" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="observacoes" HeaderText="<%$ Resources:Resource, Obs %>" />
                <asp:BoundField DataField="calibracaoExterna" HeaderText="Cal.Ext." />
                 <asp:BoundField DataField="bFollowUp" HeaderText="Followup" />
                <asp:BoundField DataField="razaoCliente" HeaderText="Razao" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="obsFollowup" HeaderText="Obs.Followup" ItemStyle-HorizontalAlign="Left" />

            </Columns>
        </asp:GridView>


        <asp:Button ID="btnNumOrcamentosPorEstadoEMes" Text="Num.Orcamentos por estado para Excel" runat="server" OnClick="btnNumOrcamentosPorEstadoEMes_Click" />
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="true">
            <Columns>
            </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
