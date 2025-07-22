<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepExcel.aspx.cs" Inherits="LabMetro.RepExcel"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnSubmit").focus();
            }
        }

    </script>

</asp:Content>


  <asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="mainContent">
  <fieldset>
  
  <legend></legend>
     </fieldset>
    <fieldset>
        <legend>Serviços com tempos</legend>
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
                 
                </td>
                <td>
                    <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                </td>
                
                <td colspan="3">
                    <%=Resources.Resource.BREsPosteriores %>:  <asp:TextBox ID="txtDtBRE" runat="server" Width="100px">01-02-2007</asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator1" runat="server" ControlToValidate="txtDtBRE" Operator="DataTypeCheck"
                        Type="Date"><%=Resources.Resource.DataInvalida %></asp:CompareValidator>
                </td>
                <td>
                  
                </td>
            </tr>
        </table>
        <br />
        <br />
         
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Resultados em Excel" />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
        <Columns>
        <asp:BoundField DataField="empresa" HeaderText="Empresa" />
        <asp:BoundField DataField="refBre" HeaderText="BRE" />
        <asp:BoundField DataField="dtBre" HeaderText="DtBRE" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="dtPrevisao" HeaderText="dtPrevisao" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="dtCalibracao" HeaderText="dtCalibracao" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="dtCertificado" HeaderText="dtCertificado" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="refServico" HeaderText="Serviço" />
        <asp:BoundField DataField="estado" HeaderText="Estado" />
        <asp:BoundField DataField="tipoEquipamento" HeaderText="Tipo" />
        <asp:BoundField DataField="numIdentificacao" HeaderText="NID" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="numSerie" HeaderText="NS" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="observacoes" HeaderText="observacoes" />
        <asp:BoundField DataField="localcalibracao" HeaderText="localcalibracao" />
       </Columns>
        </asp:GridView>
    </fieldset>
    <fieldset><legend></legend></fieldset>
</asp:Content>
