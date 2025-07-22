
<%@ Page Language="c#" CodeBehind="ListaEmpresas.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.Empresas" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

  

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.Empresas %></legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="10%">
                    <label>
                    <%=Resources.Resource.NomeEmpresa %>:</label><br />
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
                    <%=Resources.Resource.Estado %>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddEstado" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                    <%=Resources.Resource.TipoEmpresa%>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddTipoEmpresa" runat="server" DataValueField="ident" DataTextField="descricao">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                    <%=Resources.Resource.BloqSistemaCentral%>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddCodigoBloqueioSap" runat="server" AutoPostBack="True" DataTextField="descCodigoBloqueio"
                        DataValueField="codigoBloqueio">
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                    <%=Resources.Resource.PamentosAtraso %>:</label>
                </td>
                <td>
                    <asp:CheckBox ID="cbPagamentoAtraso" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                    <%=Resources.Resource.BloqueioInterno %>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddNivelBloqueio" runat="server">
                        <asp:ListItem Value="">...</asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Livre %>"></asp:ListItem>
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Amarelo %>"></asp:ListItem>
                        <asp:ListItem Value="2" Text="<%$ Resources:Resource, Laranja %>"></asp:ListItem>
                        <asp:ListItem Value="3" Text="<%$ Resources:Resource, Vermelho %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                    <%=Resources.Resource.RequisicoesAtraso %>:</label>
                </td>
                <td>
                    <asp:CheckBox ID="chRequisicaoAtraso" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                    <%=Resources.Resource.CondicoesPagamento %>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddCondPagamento" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                    <%=Resources.Resource.NumClienteSAP %>:</label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumClienteSAP" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                    <%=Resources.Resource.Sede %>:</label>
                </td>
                <td>
                    <asp:CheckBox ID="cbSede" runat="server"></asp:CheckBox>
                </td>
                <td>
                    <label>
                    <%=Resources.Resource.CertificadosemRequisicao %>:</label>
                </td>
                <td>
                    <asp:CheckBox ID="cbCertifsSemReq" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                    <%=Resources.Resource.FacturarSemRequisicao %>:</label>
                </td>
                <td>
                    <asp:CheckBox ID="cbPodeFacturarSemRequisicao" runat="server"></asp:CheckBox>
                </td>
                <td><label>
                    <%=Resources.Resource.Actividade %>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddActividade" runat="server" DataValueField="idActividade"
                        DataTextField="descricao" DataSourceID="dsrc_actividades" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                    <%=Resources.Resource.GestaoEquipamentos %>:</label>
                </td>
                <td>
                    <asp:CheckBox ID="cbGestaoEquipamentos" runat="server"></asp:CheckBox>
                </td>
                <td>
                </td>
                <td>
                 
                </td>
            </tr>
            <tr>
                <td>
                    <label><%=Resources.Resource.Distrito %>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddDistrito" runat="server" DataValueField="idDistrito" DataTextField="descricao"
                        DataSourceID="dsrc_distritos" AutoPostBack="true" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <label><%=Resources.Resource.Concelho %>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddConcelho" runat="server" DataValueField="idConcelho" DataTextField="descricao"
                        DataSourceID="dsrc_concelhos" AppendDataBoundItems="false"  OnDataBound="ddConcelhoDataBound" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label><%=Resources.Resource.Pais %>: </label>
                </td>
                <td>
                    <asp:DropDownList ID="ddPais" runat="server" DataValueField="idPais" DataTextField="descricao"
                        DataSourceID="dsrc_paises" AutoPostBack="true" AppendDataBoundItems="true">
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
                   
                   <br /><br />
                 <%--   <asp:Button class="button" ID="btnExcel" runat="server" 
                        Text="Lista Empresas com Contactos em excel" onclick="btnExcel_Click">
                    </asp:Button>--%>
                </td>
            </tr>
        </table>
         <asp:GridView ID="gv" runat="server" AutoGenerateColumns="true" />
    </fieldset>
    <asp:SqlDataSource ID="dsrc_concelhos" SelectCommand="select idConcelho, descricao from concelho where idDistrito = @idDistrito order by 2"
        runat="server" ConnectionString="<%$ ConnectionStrings:connstr %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddDistrito" PropertyName="SelectedValue" Name="idDistrito" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsrc_distritos" SelectCommand="select idDistrito, descricao from distrito order by 2"
        runat="server" ConnectionString="<%$ ConnectionStrings:connstr %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="dsrc_actividades" SelectCommand="select idActividade, descricao from actividade order by 2"
        runat="server" ConnectionString="<%$ ConnectionStrings:connstr %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="dsrc_paises" SelectCommand="select idPais, descricao from pais order by 2"
        runat="server" ConnectionString="<%$ ConnectionStrings:connstr %>"></asp:SqlDataSource>
    <br />
    <br />
    <asp:DataGrid 
    ID="DGEmpresas" 
    runat="server" 
    DataKeyField="idEmpresa"
    AutoGenerateColumns="false"
    PageSize="15" 
    AllowSorting="True" 
    AllowPaging="true" 
    OnSortCommand="SortGrid" 
    OnPageIndexChanged="DoPaging"
    >
      
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:TextBox ID="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.nivelBloqueioLabmetro")),Convert.ToString(DataBinder.Eval (Container, "DataItem.codigoBloqueioSAP")))%>'
                        Width="10" Height="10" runat="server" BorderStyle="None" BorderWidth="0">
                    </asp:TextBox>
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, verEmpresas %>" DataNavigateUrlFormatString="FormEmpresa.aspx?btn=EMP&id={0}"
                DataNavigateUrlField="idEmpresa" DataTextField="nomeComprido" SortExpression="nomeComprido"
                Target="_self">
                <ItemStyle Font-Bold="True" HorizontalAlign="Left"></ItemStyle>
            </asp:HyperLinkColumn>
            <asp:BoundColumn DataField="numClienteSAP" SortExpression="numClienteSAP" HeaderText="<%$ Resources:Resource, NumClienteSAP %>">
            </asp:BoundColumn>
           
            <asp:BoundColumn DataField="actividade" SortExpression="actividade" HeaderText="<%$ Resources:Resource, Actividade %>">
            </asp:BoundColumn>
            
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Sede %>" SortExpression="sede">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# strX(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.sede"))) %>
                    </ItemTemplate>
                   
                </asp:TemplateColumn>
            <asp:BoundColumn DataField="localidadeEmpresa" SortExpression="localidadeEmpresa"
                HeaderText="<%$ Resources:Resource, Localidade %>"></asp:BoundColumn>
            <asp:BoundColumn DataField="estadoEmpresa" SortExpression="estadoEmpresa" HeaderText="<%$ Resources:Resource, Estado %>">
            </asp:BoundColumn>
             <asp:BoundColumn DataField="morada" SortExpression="morada" HeaderText="<%$ Resources:Resource, morada %>"
                ItemStyle-Wrap="False"></asp:BoundColumn>
             <asp:BoundColumn DataField="codigoPostal" SortExpression="codigoPostal" HeaderText="<%$ Resources:Resource, codigoPostal %>"
                ItemStyle-Wrap="False"></asp:BoundColumn>
            <asp:BoundColumn DataField="telefone" SortExpression="telefone" HeaderText="<%$ Resources:Resource, Telefone %>"
                ItemStyle-Wrap="False"></asp:BoundColumn>
            <asp:BoundColumn DataField="nif" SortExpression="nif" HeaderText="<%$ Resources:Resource, NIF %>">
            </asp:BoundColumn>
               <asp:BoundColumn DataField="idEmpresaCrm" SortExpression="idEmpresaCrm" HeaderText="idEmpresaCrm">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="dtEstado" SortExpression="dtEstado" HeaderText="<%$ Resources:Resource, DtEstado %>"
                DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
            <asp:BoundColumn DataField="CondicoesPagamento" SortExpression="CondicoesPagamento"
                HeaderText="<%$ Resources:Resource, CondPagam %>" ItemStyle-Width="35"></asp:BoundColumn>
            <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Contacto %>" Text="<%$ Resources:Resource, Ver %>" DataNavigateUrlFormatString="ListaContactos.aspx?btn=EMP&id={0}"
                DataNavigateUrlField="idEmpresa" Target="_self">
                <ItemStyle HorizontalAlign="center"></ItemStyle>
            </asp:HyperLinkColumn>
        </Columns>
    </asp:DataGrid>
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:TextBox ID="Textbox2" runat="server" Enabled="False" BackColor="White" Width="15px" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                <%=Resources.Resource.FraseBloqBranco %>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtLegendaVerde" runat="server" Enabled="False" BackColor="#b0e0e6"
                    Width="15px" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>&nbsp;&nbsp;&nbsp;  <%=Resources.Resource.FraseBloqAzul %>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtLegendaAmarela" runat="server" Enabled="False" BackColor="Gold"
                    Width="15px" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>&nbsp;&nbsp;&nbsp; <%=Resources.Resource.FraseBloqAmarelo %>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtLegendaLaranja" runat="server" Enabled="False" BackColor="DarkOrange"
                    Width="15px" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>&nbsp;&nbsp;&nbsp;<%=Resources.Resource.FraseBloqLaranja %>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="Textbox1" runat="server" Enabled="False" BackColor="Crimson" Width="15px" BorderStyle="Solid" BorderWidth="1">
                </asp:TextBox>&nbsp;&nbsp;&nbsp;<%=Resources.Resource.FraseBloqVermelho %>
            </td>
        </tr>
    </table>
</asp:Content>
