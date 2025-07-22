<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BO_Marcas_Modelos.aspx.cs"
    Inherits="LabMetro.BO.BO_Marcas_Modelos" %>



<%@ Register TagPrefix="menu" TagName="inc_menu" Src="boMenu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>:: LabMetro - BO Marcas Modelo ::</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="..\Styles.css" type="text/css" rel="stylesheet">
    <style type="text/css">
        #footer
        {
            padding-right: 15px;
            border-top: #cccccc 1px solid;
            padding-left: 10px;
            font-size: 10px;
            color: #666666;
            line-height: 17px;
            padding-top: 5px;
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }
        #footer A
        {
            font-size: 11px;
            color: #666666;
            text-decoration: none;
        }
        #footer A:hover
        {
            text-decoration: underline;
        }
    </style>
</head>
<body onkeydown="CheckKey(event);" ms_positioning="GridLayout">

    <script type="text/javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisa").focus();
            }
        }

    </script>

    <form id="Form1" method="post" runat="server">
    <table id="tblMainBO">
        <tr>
            <td>
                <menu:inc_menu ID="inc_menu" runat="server"></menu:inc_menu>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td class="text_normal">
                <!-- body -->
                <table  style="font-size: 8pt; font-family: Arial" bordercolor="darkgray"
                    width="400" border="2">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" ForeColor="#ff0033" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="tblTituloCinzaClaroLetraBranca">
                        <td colspan="2">
                            <%=Resources.Resource.Limpar %>&nbsp;<%=Resources.Resource.Modelo %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.Marca %>:<asp:DropDownList ID="ddMarca" runat="server"
                                AutoPostBack="true" DataValueField="idMarca" DataTextField="descricao" 
                                Width="175px" onselectedindexchanged="ddMarca_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarca" runat="server" Width="175px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.Apagar %>&nbsp;<%=Resources.Resource.Modelo %>:<asp:DropDownList ID="ddModeloApagar" runat="server" DataValueField="ident"
                                DataTextField="descricao" Width="175px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <%=Resources.Resource.Manter %>&nbsp;<%=Resources.Resource.Modelo %>:
                            <asp:DropDownList ID="ddModeloManter" runat="server" DataValueField="ident" DataTextField="descricao"
                                Width="175px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnModelos" runat="server" onclick="btnModelos_Click" 
                                Text="<%$Resources:Resource, Limpar %>" />
                        </td>
                    </tr>
                </table>
                <br />
                <table  style="font-size: 8pt; font-family: Arial" bordercolor="darkgray"
                    width="400" border="2">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label1" ForeColor="#ff0033" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="tblTituloCinzaClaroLetraBranca">
                        <td colspan="2">
                           <%=Resources.Resource.Limpar %>&nbsp;<%=Resources.Resource.Marca %>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%=Resources.Resource.FraseAvisoBOMarcas%>.
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <%=Resources.Resource.Apagar %>&nbsp;<%=Resources.Resource.Marca %> :<asp:DropDownList ID="ddMarcaApagar" runat="server"
                                AutoPostBack="true" DataValueField="idMarca" DataTextField="descricao" 
                                Width="175px" onselectedindexchanged="ddMarca_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                          <%=Resources.Resource.Manter %>&nbsp;<%=Resources.Resource.Marca %>:<asp:DropDownList ID="ddMarcaManter" runat="server"
                                AutoPostBack="true" DataValueField="idMarca" DataTextField="descricao" 
                                Width="175px" onselectedindexchanged="ddMarca_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnMarcas" runat="server" onclick="btnMarcas_Click" 
                                Text="<%$Resources:Resource, Limpar %>" />
                        </td>
                    </tr>
                </table>

                <!-- FIM body -->
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
