<%@ Page language="c#" Codebehind="ListaPrecario.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.ListaPrecario" %>
<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>:: LabMetro - Preçário ::</title>
        <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
        <meta name="CODE_LANGUAGE" Content="C#">
        <meta name="vs_defaultClientScript" content="JavaScript">
        <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
        <LINK href="Styles.css" type="text/css" rel="stylesheet">
    </HEAD>
    <body MS_POSITIONING="GridLayout">
        <form id="Form1" method="post" runat="server">
            <table id="tblMain">
                <tr>
                    <td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
                    <td vAlign="top"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
                </tr>
                <tr>
                    <td class="text_normal" valign="top"><!-- body -->
                        <table class="tblBody" borderColor="#b90000" cellSpacing="0" cellPadding="0">
                            <tr>
                                <td class="tblTituloVermelho" colSpan="2">Consultar Preçário</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DataGrid id="DataGrid1" CssClass="DG_branco" GridLines="horizontal" BorderColor="#E0E0E0"
                                        HeaderStyle-BackColor="#999999" HeaderStyle-Font-Bold="True" HeaderStyle-ForeColor="#FFFFFF"
                                        runat="server" AutoGenerateColumns="false" DataKeyField="ID">
                                        <Columns>
                                            <asp:ButtonColumn Text="+" ButtonType="PushButton"></asp:ButtonColumn>
                                            <asp:BoundColumn Visible="false" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="TIPOEQUIPAMENTO" HeaderText="Tipo Equip." ItemStyle-Width="200"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="DESC" HeaderText="Descrição" ItemStyle-Width="200">
                                              
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="false" DataField="IDPAI" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="PRECO" HeaderText="Preço"></asp:BoundColumn>
                                            
                                            <asp:BoundColumn DataField="PRECOMOVEL" HeaderText="Preço<br /> Móv."></asp:BoundColumn>
                                            <asp:BoundColumn DataField="PRECOEXTERIOR" HeaderText="Preço<br /> Ext."></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                        <asp:Button class="button" ID="btnExport" cssclass="button" Runat="server" Text="exportar para excel" onclick="btnExport_Click"></asp:Button>
                       
                    </td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
