<%@ Page Language="c#" CodeBehind="GestCertUtilizadores.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestCertUtilizadores" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoUtilizadores %> / Workflow</legend>
   <br /><br />
         <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <asp:DataGrid ID="DGGrandezas" runat="server" AutoGenerateColumns="False"
            OnEditCommand="DGGrandezas_Edit" OnCancelCommand="DGGrandezas_CancelGrid" OnUpdateCommand="DGGrandezas_UpdateGrid"
            DataKeyField="idResponsaveis" ShowFooter="True" OnSortCommand="SortGridG" AllowSorting="True"
            HeaderStyle-ForeColor="#FFFFFF" HeaderStyle-Font-Bold="True" HeaderStyle-BackColor="#999999"
            BorderColor="#FFFFFF" GridLines="Both" BackColor="Gainsboro" AlternatingItemStyle-BackColor="LightGrey">
            <FooterStyle Font-Bold="True" ForeColor="White" BackColor="#999999"></FooterStyle>
            <Columns>
                <asp:BoundColumn Visible="False" DataField="idFuncionario" SortExpression="idFuncionario"
                    HeaderText="idFuncionario"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idGrau" SortExpression="idGrau" HeaderText="idGrau">
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idPerfil" SortExpression="idPerfil" HeaderText="idPerfil">
                </asp:BoundColumn>
                <asp:TemplateColumn SortExpression="funcionario" HeaderText="Funcion&#225;rio">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.funcionario") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddFuncionarioGFooter" runat="server" DataTextField="funcionario"
                            DataValueField="idFuncionario" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn SortExpression="grandeza" HeaderText="Grandeza">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.grandeza") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddGrandezaGFooter" runat="server" DataTextField="grandeza"
                            DataValueField="idGrandeza">
                        </asp:DropDownList>
                    </FooterTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddGrandezaGEdit" runat="server" DataTextField="grandeza" DataValueField="idGrandeza">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkButtonGAdd" CommandName="Insert" runat="server" Text="Adicionar Registo"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Alterar" CancelText="Cancelar"
                    EditText="Editar">
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:EditCommandColumn>
                <asp:ButtonColumn Text="Apagar" ButtonType="LinkButton" CommandName="Apagar"></asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
       
       <%=Resources.Resource.FraseListaFuncionariosCert %>
        <br />
        <br />
        <asp:DataGrid 
        ID="DGFuncionarios" 
        runat="server" 
        AutoGenerateColumns="false"
        OnEditCommand="DGFuncionarios_Edit" 
        OnCancelCommand="DGFuncionarios_CancelGrid"
        OnUpdateCommand="DGFuncionarios_UpdateGrid" 
        DataKeyField="idFuncionario" 
        ShowFooter="True"
        OnSortCommand="SortGridR" 
        AllowSorting="True"
        AllowPaging="false" 
        Visible="True"
        OnSelectedIndexChanged="DGFuncionarios_SelectedIndexChanged">
            <Columns>
                <asp:ButtonColumn Text="»»»" ButtonType="LinkButton" Visible="True" HeaderText="<%$ Resources:Resource, VerGrandezasAssociadas %>"
                    CommandName="Select" ItemStyle-HorizontalAlign="Center"></asp:ButtonColumn>
                <asp:BoundColumn DataField="idGrau" SortExpression="idGrau" HeaderText="idGrau" Visible="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="idPerfil" SortExpression="idPerfil" HeaderText="idPerfil"
                    Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="funcionario" SortExpression="funcionario" HeaderText="<%$ Resources:Resource, Funcionario %>"
                    ReadOnly="True" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="laboratorio" SortExpression="laboratorio" HeaderText="<%$ Resources:Resource, Laboratorio %>"
                    ReadOnly="True" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:TemplateColumn SortExpression="perfil" HeaderText="<%$ Resources:Resource, Perfil %>" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="False">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.perfil") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddPerfilREdit" runat="server" DataTextField="descricao" DataValueField="ident">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn SortExpression="grau" HeaderText="<%$ Resources:Resource, Grau %>" ItemStyle-Wrap="False"
                    Visible="true">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.grau") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddGrauEdit" runat="server" DataTextField="grau" DataValueField="idGrau">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Alterar" CancelText="Cancelar"
                    EditText="Editar"></asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
        <hr>
        <asp:Label ID="Label1" runat="server" Width="700"></asp:Label>
        <!--fim body -->
    </fieldset>
</asp:Content>
