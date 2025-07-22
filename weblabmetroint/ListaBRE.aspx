<%@ Page Language="c#" CodeBehind="ListaBRE.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.ListaBRE"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">


</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarBREs %></legend>
        <table>
           
            <tr>
                <td colspan="5">
                    <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
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
                <td colspan="5">
                    <asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa"
                        AutoPostBack="true" OnSelectedIndexChanged="ddEmpresa_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>


            <tr>
                <td>
                 <%=Resources.Resource.SoBRECompletos %>
                </td>
                <td>
                   <asp:CheckBox
                            ID="cbCompletos" runat="server" AutoPostBack="True" Text="&nbsp;&nbsp;<%$ Resources:Resource, SoBRECompletos %>"></asp:CheckBox>
                </td>
               <td>
                   <%=Resources.Resource.ReferenciaRequisicao %>(BRE)
                </td>
                <td>
                    <asp:TextBox ID="txtRefReq" runat="server"></asp:TextBox>
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
                    <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, PesquisarBREs %>"
                        CausesValidation="true" CssClass="button" OnClick="btnPesquisa_Click"></asp:Button>
                </td>
            </tr>
        </table>
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
        DataKeyField="iDBRE" >
            <Columns>
                <asp:BoundColumn DataField="refBRE" SortExpression="refBRE" HeaderText="<%$ Resources:Resource, Ref %>"></asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:TextBox ID="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.nivelBloqueioLabmetro")),Convert.ToString(DataBinder.Eval (Container, "DataItem.codigoBloqueioSAP")))%>'
                            Width="10" Height="10" runat="server">
                        </asp:TextBox>&nbsp;
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Empresa" SortExpression="Empresa" HeaderText="<%$ Resources:Resource, Empresa %>">

                </asp:BoundColumn>
                  
                 <asp:TemplateColumn HeaderText="Acesso Certif. ">
                   
                        <ItemTemplate>
                        <%# strX(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bEmpBrePodeVerCertificados"))) %>
                    </ItemTemplate>
                    
                </asp:TemplateColumn>
               
                <asp:BoundColumn DataField="EmpresaContratante" SortExpression="EmpresaContratante"
                    HeaderText="<%$ Resources:Resource, EmpContrat %>"></asp:BoundColumn>
                
                <asp:BoundColumn DataField="dtBRE" SortExpression="dtBRE" HeaderText="<%$ Resources:Resource, Data %>" DataFormatString="{0:dd/MM/yyyy}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="expedicao" SortExpression="expedicao" HeaderText="<%$ Resources:Resource, Expedicao %>">
                </asp:BoundColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Detalhes %>" Text="<%$ Resources:Resource, Ver %>" DataNavigateUrlFormatString="FormBRE.aspx?btn=DOC&id={0}"
                    DataNavigateUrlField="idBRE" Target="_self">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
