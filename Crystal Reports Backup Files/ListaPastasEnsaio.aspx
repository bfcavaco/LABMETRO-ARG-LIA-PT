<%@ Page Language="c#" CodeBehind="ListaPastasEnsaio.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaPastasEnsaio" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">


</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarServicos %></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeEmpresa" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NumBRE %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumBRE" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.RefCalib %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefServico" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Estado %>
                </td>
                <td>
                    <asp:DropDownList ID="ddEstado" runat="server" DataValueField="ident" DataTextField="descricao">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.NumIdent %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumIdentificacao" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NumSerie %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumSerie" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.GrandezaLaboratorio %>.:
                </td>
                <td>
                    <asp:DropDownList ID="ddGrandeza" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.LocalEquip %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddLocalEquipamento" runat="server" DataValueField="ident" DataTextField="descricao">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.CalibExterna %>:
                </td>
                <td>
                    <asp:RadioButtonList ID="rblCalibracaoExterna" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTipoEquipamento" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.IDEquipamentoBD %>.
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtIdEquipamento" runat="server"></asp:TextBox>
                    <asp:CompareValidator ID="compId" runat="server" ControlToValidate="txtIdEquipamento"
                        Type="Integer" Operator="DataTypeCheck"> ! </asp:CompareValidator>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Marca %>:
                </td>
                <td>
                    <asp:TextBox ID="txtMarca" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Modelo %>:
                </td>
                <td>
                    <asp:TextBox ID="txtModelo" runat="server"></asp:TextBox>
                </td>
                <td>Campo(s) Extra(s): <asp:TextBox ID="txtCampoExtra" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                  <td >
                    <%=Resources.Resource.BREsPosteriores %>:
                </td>
                <td>  <asp:TextBox ID="txtDtBRE" runat="server" Width="100px">01-02-2007</asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator1" runat="server" ControlToValidate="txtDtBRE" Operator="DataTypeCheck"
                        Type="Date"><%=Resources.Resource.DataInvalida %></asp:CompareValidator></td>
                <td>
                  Núm. Etiqueta IPQ:
                </td>
                <td>
                   <asp:TextBox ID="txtNumEtiquetaIPQ" runat="server"></asp:TextBox>
                </td>
              
                
              
            </tr>
              <tr>
                <td>
                    <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CssClass="button" CausesValidation="True"></asp:Button>
                </td>
                <td>
                    <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                </td>
                
                <td colspan="3">
                   
                </td>
                <td>
                  
                </td>
            </tr>

        </table>
        <br />
        <br />
        <asp:DataGrid 
        ID="DGpastaEnsaio"
        DataKeyField="idServico"  
        runat="server" 
        AutoGenerateColumns="false"
        AllowPaging="true" 
        AllowSorting="True" 
        PageSize="15" 
        OnSortCommand="SortGrid"
        OnPageIndexChanged="DoPaging" 
        OnItemCommand="dgPE_itemCommand"
        >
            <Columns>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, ReferenciaServico %>" DataNavigateUrlFormatString="FormPastaEnsaio.aspx?btn=LAB&id={0}"
                    DataNavigateUrlField="idServico" DataTextField="refServico" SortExpression="refServico"
                    Target="_self">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Left"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:TemplateColumn HeaderText="M.S.*" SortExpression="bVariasGrandezas"><ItemTemplate> <asp:CheckBox id="cbVariasGrandezas" runat="server" Checked='<%#DataBinder.Eval(Container, "DataItem.bVariasGrandezas")%>' Enabled="false" /></ItemTemplate></asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:TextBox ID="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.nivelBloqueioLabmetro")),Convert.ToString(DataBinder.Eval (Container, "DataItem.codigoBloqueioSAP")))%>'
                            Width="10" Height="10" runat="server">
                        </asp:TextBox>&nbsp;
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Empresa" SortExpression="Empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="EmpresaContratante" SortExpression="EmpresaContratante"
                    HeaderText="<%$ Resources:Resource, EmpContrat %>"></asp:BoundColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, NumBRE %>" DataNavigateUrlFormatString="FormBRE.aspx?btn=DOC&id={0}"
                    DataNavigateUrlField="idBRE" DataTextField="refBRE" SortExpression="refBRE" Target="_self">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Left"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="Observacoes" SortExpression="Observacoes" HeaderText="Observações">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtBRE" SortExpression="dtBRE" HeaderText="<%$ Resources:Resource, DataBRE %>" DataFormatString="{0:dd/MM/yyyy}">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="codTipoEquipamento" SortExpression="codTipoEquipamento"
                    HeaderText="<%$ Resources:Resource, Equipamento %>"></asp:BoundColumn>
                      <asp:BoundColumn DataField="acessorios" SortExpression="acessorios"
                    HeaderText="Acessórios"></asp:BoundColumn>
                <asp:BoundColumn DataField="numIdentificacao" SortExpression="numIdentificacao" HeaderText="<%$ Resources:Resource, NumIdent %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="numSerie" SortExpression="numSerie" HeaderText="<%$ Resources:Resource, NumSerie %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="marca" SortExpression="marca" HeaderText="<%$ Resources:Resource, Marca %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="modelo" SortExpression="modelo" HeaderText="<%$ Resources:Resource, Modelo %>">
                </asp:BoundColumn>
                   <asp:BoundColumn DataField="alcance" SortExpression="alcance" HeaderText="Gama">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="estado" SortExpression="estado" HeaderText="<%$ Resources:Resource, Estado %>" ItemStyle-Width="60">
                </asp:BoundColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, NumBSE %>" DataNavigateUrlFormatString="FormBSE.aspx?btn=DOC&id={0}"
                    DataNavigateUrlField="idBSE" DataTextField="refBSE" SortExpression="refBSE" Target="_self">
                    <ItemStyle Font-Bold="True" HorizontalAlign="Left"></ItemStyle></asp:HyperLinkColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Fact %>" DataNavigateUrlFormatString="FormFactura.aspx?btn=FAC&id={0}"
                        DataNavigateUrlField="idFactura" DataTextField="refFactura" SortExpression="refFactura" Target="_self">
                        <itemstyle font-bold="True" horizontalalign="Left"></itemstyle>
                </asp:HyperLinkColumn>
             
<%--                <asp:BoundColumn DataField="LocalEntrada" SortExpression="idLocalEntrada" HeaderText="<%$ Resources:Resource, LocalEntradaEquipamento %>">
                </asp:BoundColumn>
                  <asp:BoundColumn DataField="LocalDestino" SortExpression="idLocalDestino" HeaderText="<%$ Resources:Resource, LocalDestino %>">
                </asp:BoundColumn>
                   <asp:BoundColumn DataField="LocalCalibracao" SortExpression="idLocalCalibracao" HeaderText="<%$ Resources:Resource, LocalCalibracao %>">
                </asp:BoundColumn>--%>
              

            </Columns>
        </asp:DataGrid>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Exportar p/Excel" />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
        <Columns>
        <asp:BoundField DataField="empresa" HeaderText="Empresa" />
        <asp:BoundField DataField="refBre" HeaderText="BRE" />
        <asp:BoundField DataField="dtBre" HeaderText="DtBRE" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="refServico" HeaderText="Serviço" />
        <asp:BoundField DataField="estado" HeaderText="Estado" />
        <asp:BoundField DataField="dtBse" HeaderText="DtBSE" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="tipoEquipamento" HeaderText="Tipo" />
        <asp:BoundField DataField="numIdentificacao" HeaderText="NID" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="numSerie" HeaderText="NS" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="marca" HeaderText="Marca" />
        <asp:BoundField DataField="modelo" HeaderText="Modelo" />
        <asp:BoundField DataField="refBSE" HeaderText="BSE" />
  <asp:BoundField DataField="alcance" HeaderText="Gama" />        <asp:BoundField DataField="observacoes" HeaderText="observacoes" />
            <asp:BoundField DataField="codigoIPQ" HeaderText="codigoIPQ" />
             <asp:BoundField DataField="dtEstado" HeaderText="dtEstado" 
                SortExpression="dtEstado" DataFormatString="{0:dd/MM/yyyy}" />
              <asp:BoundField DataField="refServicoCertificado" HeaderText="refServicoCertificado" />
            <asp:BoundField DataField="subtipo" HeaderText="SubTipo" />
             <asp:BoundField DataField="classe" HeaderText="classe" />
            <asp:BoundField DataField="campo1" HeaderText="campo1" />
            <asp:BoundField DataField="funcionarioTreino" HeaderText="funcionarioTreino" />
                      <asp:BoundField DataField="dtUltimaCalibracao" HeaderText="dtUltimaCalibracao" />
            <asp:BoundField DataField="refUltimaCalibracao" HeaderText="refUltCal" />
              <asp:BoundField DataField="calibracaoExterna" SortExpression="calibracaoExterna" HeaderText="Calib.Ext.">
                </asp:BoundField>
        </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
