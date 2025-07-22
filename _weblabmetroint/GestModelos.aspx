<%@ Page Language="c#" CodeBehind="GestModelos.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestModelos" 
    MasterPageFile="~/mp.Master" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
 <%--   <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js"
type = "text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
type = "text/javascript"></script>
<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
rel = "Stylesheet" type="text/css" />   --%> <!--https://www.aspsnippets.com/Articles/Using-jQuery-AutoComplete-Plugin-in-ASP.Net.aspx-->
<%--<script type="text/javascript">



    $(document).ready(function () {
        $("#<%=txtSearchMarca.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("Webservices/wsMarca.asmx/GetMarcas") %>',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('-')[0],
                                val: item.split('-')[1]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                $("#<%=hfidMarca.ClientID %>").val(i.item.val);
            },
            minLength: 1
        });
    });

  
</script>--%>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">

    <fieldset>
        <legend>
            <%=Resources.Resource.GestaoModelos %>
        </legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />  <br />  <br />
       
        <%=Resources.Resource.Marca %>:
        <asp:DropDownList ID="ddMarca" runat="server" DataTextField="descricao"
            DataValueField="idMarca" 
            onselectedindexchanged="ddMarca_SelectedIndexChanged" AutoPostBack="true">
        </asp:DropDownList>
   
  Marca:
   <%--  <asp:TextBox ID="txtSearchMarca" runat="server"></asp:TextBox>
    <asp:HiddenField ID="hfidMarca" runat="server" />--%>
        &nbsp;&nbsp;&nbsp;<%=Resources.Resource.Modelo %>:<asp:TextBox ID="txtModelo" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Button
            ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" OnClick="btnSubmit_Click">
        </asp:Button><br /> <%=Resources.Resource.AlertaModelos%>
           <br />
      
        <asp:DataGrid ID="DGModelos" runat="server" AutoGenerateColumns="false" OnEditCommand="DGModelos_Edit"
            OnCancelCommand="DGModelos_CancelGrid" OnUpdateCommand="DGModelos_UpdateGrid"
            DataKeyField="idModelo" ShowFooter="True" AllowPaging="true" PageSize="25" AllowSorting="True"
            OnPageIndexChanged="DoPaging" OnSortCommand="SortGrid">
            <Columns>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Apagar %>" ItemStyle-BackColor="#DC0000" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="checkboxApagar" AutoPostBack="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Manter" ItemStyle-BackColor="Green"  ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="checkboxManter" AutoPostBack="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Marca %>" SortExpression="marca">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.marca") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddMarcaFooter" runat="server" DataTextField="descricao" DataValueField="idMarca">
                        </asp:DropDownList>
                       <%-- <asp:TextBox ID="txtMarcaFooter" runat="server"></asp:TextBox>
    <asp:HiddenField ID="hfidMarcaFooter" runat="server" />--%>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Modelo %>" SortExpression="descricao">
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
              
                 <asp:BoundColumn DataField="username" SortExpression="username" HeaderText="<%$ Resources:Resource, CriadoPor %>" ReadOnly="true"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtCriacao" SortExpression="dtCriacao" HeaderText="<%$ Resources:Resource, DtCriacao %>" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" ></asp:BoundColumn>
                <asp:EditCommandColumn EditText="<%$ Resources:Resource, Editar %>" UpdateText="<%$ Resources:Resource, Alterar %>" CancelText="<%$ Resources:Resource, Cancelar %>"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
                <asp:TemplateColumn HeaderText="">
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarModelo %>"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Button ID="Button1" runat="server" 
            Text="<%$ Resources:Resource, TrocarModelos %>" onclick="Button1_Click" class="button" />
        <br />
  
    </fieldset> 
        <%=Resources.Resource.InstrucoesGestaoModelos %>
    
    
</asp:Content>
