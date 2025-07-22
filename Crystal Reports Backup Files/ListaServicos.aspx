<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaServicos.aspx.cs"
    Inherits="LabMetro.ListaServicos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Listar Serviços</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     
        <table>
            <tr>
                <td colspan="6">
                    Listar Serviços
                </td>
            </tr>
            <tr>
                <td>
                    Empresa:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeEmpresa" runat="server" Width="288px"></asp:TextBox>
                </td>
                <td>
                    Nº BRE:
                </td>
                <td>
                    <asp:TextBox ID="txtRefBRE" runat="server"></asp:TextBox>
                </td>
                <td>
                    Ref. Cal.:
                </td>
                <td>
                    <asp:TextBox ID="txtRefServico" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Estado
                </td>
                <td>
                    <asp:DropDownList ID="ddEstado" runat="server" DataValueField="ident" DataTextField="descricao">
            
                    </asp:DropDownList>
                </td>
                <td>
                    Num.Ident.:
                </td>
                <td>
                    <asp:TextBox ID="txtNumIdentificacao" runat="server"></asp:TextBox>
                </td>
                <td>
                    Núm. Série:
                </td>
                <td>
                    <asp:TextBox ID="txtNumSerie" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Grandeza/Lab.:
                </td>
                <td>
                    <asp:DropDownList ID="ddGrandeza" runat="server" DataTextField="descricao" DataValueField="ident" Width="300px">
            
                    </asp:DropDownList>
                </td>
                <td>
                    Local Equip:
                </td>
                <td>
                    <asp:DropDownList ID="ddLocalEquipamento" runat="server" DataValueField="ident" DataTextField="descricao">
                    
                    </asp:DropDownList>
                </td>
                <td>
                    Calibração Externa:
                </td>
                <td>
                    <asp:RadioButtonList ID="rblCalibracaoExterna" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Sim</asp:ListItem>
                        <asp:ListItem Value="0">Não</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Tipo Equip.:
                </td>
                <td>
                    <asp:TextBox ID="txtTipoEquipamento" runat="server" Width="288px"></asp:TextBox>
                </td>
                <td>
                    ID(BD)Equip.
                </td>
                <td>
                    <asp:TextBox ID="txtIdEquipamento" runat="server"></asp:TextBox>
                    <asp:CompareValidator ID="compId" runat="server" ControlToValidate="txtIdEquipamento"
                        Type="Integer" Operator="DataTypeCheck"> ! </asp:CompareValidator>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Marca:
                </td>
                <td>
                    <asp:TextBox ID="txtMarca" runat="server" Width="288px"></asp:TextBox>
                </td>
                <td>
                    Modelo:
                </td>
                <td>
                    <asp:TextBox ID="txtModelo" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    
                </td>
                <td>
                </td>
                <td colspan="2">
                    BRE's posteriores a:
                </td>
                <td>
                    <asp:TextBox ID="txtDtBRE" runat="server">01-02-2007</asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator1" runat="server" ControlToValidate="txtDtBRE" Operator="DataTypeCheck"
                        Type="Date">data inválida!</asp:CompareValidator>
                </td>
            </tr>
        </table>
       
    </div>
  
  
   
    <asp:ObjectDataSource ID="objDSServicos" runat="server" 
        SelectMethod="GetData" 
        
        TypeName="LabMetro.DataAccessLayer.dlServicoTableAdapters.ListaServicosTableAdapter" 
        OnSelected="selected" OnSelecting="selecting" EnablePaging="True" 
        MaximumRowsParameterName="NumRows" 
        OldValuesParameterFormatString="original_{0}" SelectCountMethod="CountServicos" 
        StartRowIndexParameterName="StartRowIndex" >
        <SelectParameters>
            <asp:ControlParameter ControlID="txtNomeEmpresa" Name="empresa"  PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtRefServico" Name="refServico" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtRefBRE" Name="refBre" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ddEstado" Name="idEstado" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="txtIdEquipamento" Name="idEquipamento" Type="Int32" />
            <asp:ControlParameter ControlID="txtTipoEquipamento" Name="tipoEquipamento" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtNumIdentificacao" Name="numIdentificacao" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtNumSerie" Name="numSerie" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ddLocalEquipamento" Name="local" Type="Int32" />
            <asp:ControlParameter ControlID="ddGrandeza" Name="idGrandeza"   PropertyName="SelectedValue" Type="String" />
            <asp:ControlParameter ControlID="txtDtBRE" Name="dtBRE" Type="DateTime"/>
            <asp:ControlParameter ControlID="rblCalibracaoExterna" Name="bCalibracaoExterna" PropertyName="SelectedValue" Type="String"  />
            <asp:Parameter Name="StartRowIndex" Type="Int32"/>
            <asp:Parameter Name="NumRows" Type="Int32" DefaultValue=""/>
                    </SelectParameters>
    </asp:ObjectDataSource>
  
  
   
    <asp:GridView ID="gvServicos" runat="server" AllowPaging="True" 
        PageSize="15" AutoGenerateColumns="False" 
        DataKeyNames="idServico" DataSourceID="objDSServicos" 
        onselectedindexchanged="gvServicos_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="idServico" HeaderText="idServico" 
                InsertVisible="False" ReadOnly="True" SortExpression="idServico" />
            <asp:BoundField DataField="refServico" HeaderText="refServico" 
                SortExpression="refServico" />
            <asp:BoundField DataField="idEquipamento" HeaderText="idEquipamento" 
                SortExpression="idEquipamento" />
            <asp:BoundField DataField="idGrandeza" HeaderText="idGrandeza" 
                SortExpression="idGrandeza" />
            <asp:BoundField DataField="tipoEquipamento" HeaderText="tipoEquipamento" 
                SortExpression="tipoEquipamento" />
            <asp:BoundField DataField="numIdentificacao" HeaderText="numIdentificacao" 
                SortExpression="numIdentificacao" />
            <asp:BoundField DataField="numSerie" HeaderText="numSerie" 
                SortExpression="numSerie" />
            <asp:BoundField DataField="empresa" HeaderText="empresa" ReadOnly="True" 
                SortExpression="empresa" />
            <asp:BoundField DataField="empresaContratante" HeaderText="empresaContratante" 
                SortExpression="empresaContratante" />
            <asp:BoundField DataField="nivelBloqueioLabmetro" 
                HeaderText="nivelBloqueioLabmetro" ReadOnly="True" 
                SortExpression="nivelBloqueioLabmetro" />
            <asp:BoundField DataField="codigoBloqueioSAP" HeaderText="codigoBloqueioSAP" 
                ReadOnly="True" SortExpression="codigoBloqueioSAP" />
            <asp:BoundField DataField="marca" HeaderText="marca" ReadOnly="True" 
                SortExpression="marca" />
            <asp:BoundField DataField="modelo" HeaderText="modelo" ReadOnly="True" 
                SortExpression="modelo" />
            <asp:BoundField DataField="refBRE" HeaderText="refBRE" ReadOnly="True" 
                SortExpression="refBRE" />
            <asp:BoundField DataField="dtBRE" HeaderText="dtBRE" SortExpression="dtBRE" />
            <asp:BoundField DataField="refBSE" HeaderText="refBSE" ReadOnly="True" 
                SortExpression="refBSE" />
            <asp:BoundField DataField="dtBSE" HeaderText="dtBSE" SortExpression="dtBSE" />
            <asp:BoundField DataField="codTipoEquipamento" HeaderText="codTipoEquipamento" 
                SortExpression="codTipoEquipamento" />
            <asp:BoundField DataField="equipamento" HeaderText="equipamento" 
                SortExpression="equipamento" />
            <asp:BoundField DataField="estado" HeaderText="estado" 
                SortExpression="estado" />
            <asp:BoundField DataField="ano" HeaderText="ano" SortExpression="ano" />
            <asp:BoundField DataField="numBRE" HeaderText="numBRE" 
                SortExpression="numBRE" />
            <asp:BoundField DataField="idBRE" HeaderText="idBRE" SortExpression="idBRE" />
            <asp:BoundField DataField="idBSE" HeaderText="idBSE" SortExpression="idBSE" />
            <asp:BoundField DataField="idLocalCalibracao" HeaderText="idLocalCalibracao" 
                SortExpression="idLocalCalibracao" />
            <asp:BoundField DataField="LocalCalibracao" HeaderText="LocalCalibracao" 
                SortExpression="LocalCalibracao" />
            <asp:BoundField DataField="idFactura" HeaderText="idFactura" 
                SortExpression="idFactura" />
            
        </Columns>
    </asp:GridView>
  
    <asp:Button class="button" ID="btnSubmit" runat="server" Text="Pesquisar" 
        onclick="btnSubmit_Click" style="width: 88px" />
   
   
 </form>
</body>
</html>
