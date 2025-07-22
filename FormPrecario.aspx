<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@ Page language="c#" Codebehind="FormPrecario.aspx.cs" AutoEventWireup="false" Inherits="LabMetro.FormPrecario" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>:: LabMetro - Preçário :: </title>
        <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
        <meta content="C#" name="CODE_LANGUAGE">
        <meta content="JavaScript" name="vs_defaultClientScript">
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <LINK href="Styles.css" type="text/css" rel="stylesheet">
    </HEAD>
    <body MS_POSITIONING="GridLayout">
        <form id="Form1" method="post" runat="server">
            <table id="tblMain" width="800">
                <tr>
                    <td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
                    <td vAlign="top"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
                </tr>
                <tr>
                    <td class="text_normal" vAlign="top"><!-- body -->
                        <table class="tblBody" borderColor="#b90000" cellSpacing="0" cellPadding="0">
                            <tr>
                                <td class="tblTituloVermelho" colSpan="2">Inserir Preço</td>
                            </tr>
                            <tr>
                                <td colSpan="2"><asp:label id="lblMessage" Runat="server"></asp:label></td>
                            </tr>
                            <tr>
                                <td>Tipo se Serviço:</td>
                                <td><asp:dropdownlist id="ddTipoServico" DataTextField="descricao" DataValueField="ident" Runat="server"
                                        AutoPostBack="true"></asp:dropdownlist></td>
                            </tr>
                            <tr>
                                <td style="HEIGHT: 23px">Tipo de Equipamento:</td>
                                <td style="HEIGHT: 23px"><asp:dropdownlist id="ddTipoEquipamento" DataTextField="descricao" DataValueField="ident" Runat="server"
                                        AutoPostBack="true"></asp:dropdownlist></td>
                            </tr>
                            <tr>
                                <td width="30%">Pai:</td>
                                <td width="70%"><asp:dropdownlist id="ddPai" DataTextField="descricao" DataValueField="ident" Runat="server"></asp:dropdownlist></td>
                            </tr>
                            <tr>
                                <td>Descrição:</td>
                                <td><asp:textbox id="txtDescricao" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td>Preço</td>
                                <td><asp:textbox id="txtPreco" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td>Preço Móvel:</td>
                                <td><asp:textbox id="txtPrecoMovel" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td>Preço exterior:</td>
                                <td><asp:textbox id="txtPrecoExterior" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td align="center" colSpan="2"><asp:button id="btnInsert" CssClass="button" Runat="server" Text="Inserir"></asp:button><br>
                                    <br>
                                </td>
                            </tr>
                            <tr>
                                <td class="tblTituloVermelho" colSpan="2">Alterar Preçário</td>
                            </tr>
                        </table>
                        <asp:DataGrid id="DataGrid1" CssClass="DG_branco" GridLines="horizontal" BorderColor="#E0E0E0"
                            HeaderStyle-BackColor="#999999" HeaderStyle-Font-Bold="True" HeaderStyle-ForeColor="#FFFFFF"
                            runat="server" AutoGenerateColumns="false" DataKeyField="ID" OnCancelCommand="cancelGrid"
                            OnEditCommand="editGrid" OnUpdateCommand="updateGrid" ShowFooter="false">
                            <Columns>
                                <asp:TemplateColumn HeaderText="Tipo Equipamento">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.TIPOEQUIPAMENTO")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Descrição">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.DESC")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescricaoEdit" Runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Preço" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.PRECO")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPrecoEdit" Runat="server" Width="50"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Preço Móvel" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.PRECOMOVEL")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPrecoMovelEdit" Runat="server" Width="50"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Preço Ext." HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.PRECOEXTERIOR")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPrecoExteriorEdit" Runat="server" Width="50"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:EditCommandColumn CancelText="cancelar" EditText="editar" UpdateText="alterar"></asp:EditCommandColumn>
                                <asp:ButtonColumn CommandName="Delete" Text="Remover"></asp:ButtonColumn>
                                <asp:TemplateColumn HeaderText="">
                                    <FooterTemplate>
                                        <asp:LinkButton id="Linkbutton1" CommandName="Insert" runat="server" Text="Adicionar Registo"></asp:LinkButton>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid></td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
