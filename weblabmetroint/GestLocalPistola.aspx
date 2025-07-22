<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestLocalPistola.aspx.cs" Inherits="LabMetro.GestLocalPistola"  MasterPageFile="~/mp.Master"%>
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
        <legend>Mudar Local Serviços (Pistola)</legend>
        <!-- body -->
        <asp:Label ID="lblMessage" runat="server"></asp:Label><br />
        <br />
        <table>
            <tr>
                <td valign="top">
                    <table>
                        
                        
                        <tr>
                            <td valign="top">
                                Actualizar Local do Equipamento para:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddLocalNovo" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                               
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                (Pistola)</td>
                            <td>
                                <asp:TextBox ID="txtIdServico" runat="server" 
                                    Font-Italic="True" ontextchanged="txtIdServico_TextChanged"></asp:TextBox>
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

