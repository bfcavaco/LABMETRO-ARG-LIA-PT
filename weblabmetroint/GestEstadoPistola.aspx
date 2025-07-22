<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestEstadoPistola.aspx.cs" Inherits="LabMetro.GestEstadoPistola" MasterPageFile="~/mp.Master"%>
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
        <legend>Mudar Estados Serviços (Pistola)</legend>
        <!-- body -->
        <asp:Label ID="lblMessage" runat="server"></asp:Label><br />
        <br />
        <table>
            <tr>
                <td valign="top">
                    <table>
                        
                        <tr>
                            <td>
                                Estado
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEstadoOrigem" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Mudar para:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEstadoDestino" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                Nota: Equipamentos só podem ser suspensos ou anulados na pasta de Ensaio.
                            </td>
                        </tr>
                       
                        <tr>
                            <td valign="top">
                                (Pistola)</td>
                            <td>
                                <asp:TextBox ID="txtIdServico" runat="server" 
                                    Font-Italic="True" ontextchanged="txtIdServico_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                         
                    </table>
                    <br />
                   
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
