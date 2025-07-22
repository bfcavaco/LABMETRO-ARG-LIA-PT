<%@ Page Language="c#" CodeBehind="GestMarcas.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestMarcas"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoMarcas %></legend>
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label><br />
        <br />
        
        <%=Resources.Resource.Marca %>: <asp:TextBox ID="txtNome" runat="server"></asp:TextBox><asp:Button class="button" ID="btnSearch"
            CssClass="button" runat="server" Text="<%$ Resources:Resource, Pesquisar %>"></asp:Button><br />
        <br />
           <%=Resources.Resource.AlertaMarcas%>
           <br />
        <asp:DataGrid ID="DGMarcas" runat="server"  GridLines="horizontal"
            BorderColor="#E0E0E0" HeaderStyle-BackColor="#999999" HeaderStyle-Font-Bold="True"
            HeaderStyle-ForeColor="#FFFFFF" PagerStyle-Mode="NumericPages" OnSortCommand="SortGrid"
            OnPageIndexChanged="DoPaging" AllowSorting="True" PageSize="25" AllowPaging="true"
            ShowFooter="True" DataKeyField="idMarca" OnUpdateCommand="DGMarcas_UpdateGrid"
            OnCancelCommand="DGMarcas_CancelGrid" OnEditCommand="DGMarcas_Edit" 
            AutoGenerateColumns="false" >
            <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
            <FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
            <Columns>
                 <asp:TemplateColumn HeaderText="Apag." ItemStyle-BackColor="#DC0000" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="checkboxApagar" AutoPostBack="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Manter" ItemStyle-BackColor="Green"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="checkboxManter" AutoPostBack="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Marca %>" SortExpression="descricao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.descricao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescricao" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>'
                            runat="server" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtDescricaoFooter" runat="server" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, EmpManutCTA %>" SortExpression="idEmpresaManutCTA">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.empresaManutencao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEmpManutEdit" runat="server" DataTextField="empresaManutencao"
                            DataValueField="idEmpresa">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddEmpManutFooter" runat="server" DataTextField="empresaManutencao"
                            DataValueField="idEmpresa">
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                
                <asp:BoundColumn DataField="username" SortExpression="username" HeaderText="<%$ Resources:Resource, CriadoPor %>" ReadOnly="true" ></asp:BoundColumn>
                <asp:BoundColumn DataField="dtCriacao" SortExpression="dtCriacao" HeaderText="<%$ Resources:Resource, DtCriacao %>" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true"></asp:BoundColumn>
               
                <asp:EditCommandColumn EditText="<%$ Resources:Resource, Editar %>" UpdateText="<%$ Resources:Resource, Alterar %>" CancelText="<%$ Resources:Resource, Cancelar %>"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
                <asp:TemplateColumn HeaderText="">
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarMarca %>"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
                 <asp:ButtonColumn CommandName="verModelos" ButtonType="LinkButton" Text="<%$ Resources:Resource, Modelos %>"></asp:ButtonColumn>
               <asp:ButtonColumn CommandName="verEquipamentos" ButtonType="LinkButton" Text="<%$ Resources:Resource, Equips %>"></asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Button ID="Button1" runat="server" 
            Text="<%$ Resources:Resource, TrocarMarcas %>" onclick="Button1_Click" class="button" /><br />
            
            <asp:SqlDataSource runat="server" id="dsModelos"
            ConnectionString="<%$ ConnectionStrings:connstr %>" 
            SelectCommand="SELECT [idMarca], [descricao] FROM [Modelo] WHERE ([idMarca] = @idMarca) ORDER BY [descricao]">
                <SelectParameters>
                    <asp:Parameter Name="idMarca" Type="Int32" />
                </SelectParameters>
        </asp:SqlDataSource>
           <asp:SqlDataSource runat="server" id="dsEquips"
            ConnectionString="<%$ ConnectionStrings:connstr %>" 
            SelectCommand="select tipoEquipamento.descricao, equipamento.refUltimaCalibracao 
from equipamento 
inner join tipoEquipamento on equipamento.idTipoEquipamento = tipoEquipamento.idTipoEquipamento
inner join marca on equipamento.idMarca = marca.idMarca
 where equipamento.idMarca = @idMarca ">
                <SelectParameters>
                    <asp:Parameter Name="idMarca" Type="Int32" />
                </SelectParameters>
        </asp:SqlDataSource>
            <asp:GridView ID="gvGenerico" runat="server" AutoGenerateColumns="true"></asp:GridView>
         
    </fieldset>
   <%=Resources.Resource.LegendaGestaoMarcas %>
</asp:Content>
