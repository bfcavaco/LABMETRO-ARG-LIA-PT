<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_ExternalUserMenu.ascx"%>
<%@ Page language="c#" Codebehind="PedidoOrcamento.aspx.cs" AutoEventWireup="false" Inherits="LabMetro.PedidoOrcamento" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>:: Pedido de Orçamento - Utilizadores não registados ::</title>
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
                    <td><!-- body --><asp:label id="lblMessage" CssClass="lblMessage" Runat="server"></asp:label>
                        <asp:validationsummary id="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
                            ShowSummary="true" CssClass="errorMessage" DisplayMode="SingleParagraph"></asp:validationsummary>
                        <table class="tblBody">
                            <tr class="tblTituloVermelho">
                                <td colSpan="4">Pedido de Orçamento para utilizadores não registados</td>
                            </tr>
                            <tr>
                                <td class="tblTituloCinzaClaro" colSpan="4">Dados da Empresa:</td>
                            </tr>
                            <tr>
                                <td>Nome completo da Empresa:</td>
                                <td><asp:textbox id="txtNomeEmpresa" Runat="server" Width="200" MaxLength="100"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" Runat="server" ControlToValidate="txtNomeEmpresa">*</asp:requiredfieldvalidator></td>
                                <td>Nome abreviado:</td>
                                <td><asp:textbox id="txtNomeAbrev" Runat="server" Width="200px" MaxLength="30"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator4" Runat="server" ControlToValidate="txtNomeAbrev">*</asp:requiredfieldvalidator></td>
                            </tr>
                            <tr>
                                <td>Morada:</td>
                                <td><asp:textbox id="txtMorada" Runat="server" Width="200px"></asp:textbox></td>
                                <td>Código Postal:</td>
                                <td><asp:textbox id="txtCodigoPostal1" runat="server" Width="40px" MaxLength="4"></asp:textbox>-
                                    <asp:textbox id="txtCodigoPostal2" runat="server" Width="33px" MaxLength="3"></asp:textbox></td>
                            </tr>
                            <TR>
                                <TD>País</TD>
                                <TD><asp:dropdownlist id="ddPais" Runat="server" DataValueField="ident" DataTextField="descricao"></asp:dropdownlist></TD>
                                <TD>Localidade:</TD>
                                <TD><asp:dropdownlist id="ddLocalidade" Runat="server" DataValueField="ident" DataTextField="descricao"></asp:dropdownlist></TD>
                            </TR>
                            <tr>
                                <td>Telefone1:</td>
                                <td><asp:textbox id="txtTelefone1" Runat="server"></asp:textbox></td>
                                <td>Telefone2:</td>
                                <td><asp:textbox id="txtTelefone2" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td>Email geral:</td>
                                <td><asp:textbox id="txtEmail" Runat="server" Width="200px"></asp:textbox></td>
                                <td>Fax:</td>
                                <td><asp:textbox id="txtFax" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td class="tblTituloCinzaClaro" colSpan="4">Dados da pessoa de contacto:</td>
                            </tr>
                            <tr>
                                <td>Nome Contacto:</td>
                                <td><asp:textbox id="txtNome" Runat="server" Width="200px" MaxLength="100"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator2" Runat="server" ControlToValidate="txtNome">*</asp:requiredfieldvalidator></td>
                                <td>Titulo:</td>
                                <td><asp:dropdownlist id="ddtitulo" Runat="server" DataValueField="ident" DataTextField="descricao"></asp:dropdownlist></td>
                            </tr>
                            <tr>
                                <td>Departamento:</td>
                                <td><asp:textbox id="txtDepartamento" Runat="server" Width="200px"></asp:textbox></td>
                                <td>Cargo:</td>
                                <td><asp:textbox id="txtCargo" runat="server" Width="200px"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td>Extensão:</td>
                                <td><asp:textbox id="txtExtensao" Runat="server"></asp:textbox></td>
                                <td>Telefone directo:</td>
                                <td><asp:textbox id="txtTelefoneDirecto" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td>Email directo:</td>
                                <td><asp:textbox id="txtEmailDirecto" Runat="server" Width="200px"></asp:textbox></td>
                                <td>Fax directo:</td>
                                <td><asp:textbox id="txtFaxDirecto" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td class="tblTituloCinzaClaro" colSpan="4">Dados do pedido de orçamento:</td>
                            </tr>
                            <TR>
                                <TD>Referência do pedido:</TD>
                                <TD><asp:textbox id="txtRefCliente" runat="server" Width="175px"></asp:textbox></TD>
                                <td>Tipo de Serviço:</td>
                                <td><asp:DropDownList ID="ddTipoServico" Runat="server" DataTextField="descricao" DataValueField="ident"></asp:DropDownList></td>
                            </TR>
                            <TR>
                                <TD>Trabalho a Efectuar:&nbsp;
                                </TD>
                                <TD colspan="3"><asp:dropdownlist id="ddLocalExecucao" runat="server" DataValueField="ident" DataTextField="descricao"></asp:dropdownlist>***</TD>
                            </TR>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="3" class="errorMessage">***: nossas = Instalações ISQ; vossas = 
                                    Instalações cliente.</td>
                            </tr>
                            <tr>
                                <td>Tipo de Equipamento:</td>
                                <td colspan="3"><asp:DropDownList ID="ddTipoEquipamento" Runat="server" DataTextField="descricao" DataValueField="ident"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>Quantidade:</td>
                                <td><asp:textbox id="txtQuantidade" runat="server"></asp:textbox></td>
                                <TD>Localidade Calibr.:</TD>
                                <TD><asp:textbox id="txtLocalidadeCalib" runat="server" Width="125px"></asp:textbox></TD>
                            </tr>
                            <tr>
                                <td>Marca</td>
                                <td><asp:textbox id="txtMarca" Runat="server"></asp:textbox></td>
                                <td>Modelo</td>
                                <td><asp:textbox id="txtModelo" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td>Gama(alcance):</td>
                                <td><asp:textbox id="txtAlcance" Runat="server"></asp:textbox></td>
                                <td>Classe:</td>
                                <td><asp:textbox id="txtClasse" Runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td>Nº Pontos de calibração:</td>
                                <td><asp:textbox id="txtNumPontos" Runat="server"></asp:textbox></td>
                                <td>Detalhes dos pontos:</td>
                                <td><asp:textbox id="txtDescPontos" runat="server"></asp:textbox></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4"><asp:button id="btnSubmit" runat="server" CssClass="button" Text="enviar pedido"></asp:button></td>
                            </tr>
                        </table>
                        <!-- FIM body --></td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
