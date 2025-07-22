<%@ Page language="c#" Codebehind="BO_Equipamentos.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.BO.BO_Equipamentos" %>
<%@Register TagPrefix=menu TagName=inc_menu src="boMenu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - BO Equipamentos ::</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="..\Styles.css" type="text/css" rel="stylesheet">
		<style type="text/css">#footer { PADDING-RIGHT: 15px; BORDER-TOP: #cccccc 1px solid; PADDING-LEFT: 10px; FONT-SIZE: 10px; COLOR: #666666; LINE-HEIGHT: 17px; PADDING-TOP: 5px; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }
	#footer A { FONT-SIZE: 11px; COLOR: #666666; TEXT-DECORATION: none }
	#footer A:hover { TEXT-DECORATION: underline }
		</style>
	</HEAD>
	<body onkeydown="CheckKey(event);" MS_POSITIONING="GridLayout">
		<script type="text/javascript">
	function CheckKey() 
	{
		if (event.keyCode == 13) 
		{
			document.getElementById("btnPesquisa").focus();
		}
	}

		</script>
		<form id="Form1" method="post" runat="server">
			<table id="tblMainBO">
				<tr>
					<td><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu><br />
						<br />
					</td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body -->
						<table id="tblPesquisa" style="FONT-SIZE: 8pt; FONT-FAMILY: Arial" borderColor="darkgray"
							width="400" border="2">
							<tr>
								<td colSpan="4"><asp:label id="lblMessage" ForeColor="#ff0033" Runat="server"></asp:label></td>
							</tr>
							<tr class="tblTituloCinzaClaroLetraBranca">
								<td colSpan="4"><%=Resources.Resource.PesquisarEquipamentos %>:</td>
							</tr>
							<tr>
								<td><%=Resources.Resource.Empresa %>:</td>
								<td><asp:textbox id="txtPesquisaEmpresa" Runat="server"></asp:textbox></td>
								<td colSpan="2"><asp:Button class="button" id="btnPesquisaEmpresa" Runat="server" Text="<%$ Resources:Resource, verEmpresas %>" CssClass="button"></asp:button></td>
							</tr>
							<tr>
								<td colSpan="4"><asp:dropdownlist id="ddEmpresa" runat="server" DataTextField="Nome_" DataValueField="idEmpresa" AutoPostBack="true"></asp:dropdownlist></td>
							</tr>
							<tr class="tblTituloCinzaClaroLetraBranca">
								<td colSpan="4"><%=Resources.Resource.Equipamentos %>:</td>
							</tr>
							<tr>
								<td colSpan="2"><%=Resources.Resource.Grandeza %>:<asp:textbox id="txtGrandeza" Runat="server" MaxLength="3" Width="30"></asp:textbox>&nbsp;<%=Resources.Resource.Familia %>:<asp:textbox id="txtFamilia" Runat="server"></asp:textbox></td>
								<td colSpan="2"><%=Resources.Resource.IDEquipamentoBD %><asp:textbox id="txtIdEquipamento" Runat="server"></asp:textbox></td>
							</tr>
							<tr>
								<td><%=Resources.Resource.TipoEquipamento %>:</td>
								<td colSpan="3"><asp:textbox id="txtTipoEquipamento" runat="server"></asp:textbox></td>
							</tr>
							<TR>
								<td><%=Resources.Resource.NumIdent %>:<br />
									<asp:textbox id="txtNumIdentificacao" runat="server"></asp:textbox><br />
									<%=Resources.Resource.NumSerie %>:<br />
									<asp:textbox id="txtNumSerie" runat="server"></asp:textbox></td>
								<TD><%=Resources.Resource.CriteriosPesquisa %>:</TD>
								<TD style="HEIGHT: 133px" colSpan="2" rowSpan="2"><asp:radiobuttonlist id="rbCriteriosPesquisa" runat="server" Font-Size="10px">
										<asp:ListItem Value="ignorarEspacos" Text="<%$ Resources:Resource, IgnorarEspacos %>"></asp:ListItem>
										<asp:ListItem Value="ignorarCaracteres" Text="<%$ Resources:Resource, IgnorarCaracteres %>"></asp:ListItem>
										<asp:ListItem Value="ignorarAmbos" Text="<%$ Resources:Resource, IgnorarAmbos %>"></asp:ListItem>
										<asp:ListItem Value="nada" Selected="True" Text="<%$ Resources:Resource, ManterTudoNormal %>"></asp:ListItem>
									</asp:radiobuttonlist></TD>
							</TR>
							<TR>
								<td style="HEIGHT: 51px"><%=Resources.Resource.FiltrarResultados %><br />
									<asp:checkbox id="cbFilterResultados" Runat="server"></asp:checkbox></td>
								<TD style="HEIGHT: 51px"><%=Resources.Resource.Filtro %></TD>
							</TR>
							<tr>
								<td><%=Resources.Resource.ProcurarPor %>&nbsp;<%=Resources.Resource.RefServ%>:</td>
								<td colSpan="3"><asp:textbox id="txtRefUltEntrada" runat="server"></asp:textbox>
								</td>
							</tr>
							<tr>
								<td colSpan="4"><asp:Button class="button" id="btnPesquisa" Runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CssClass="button"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
									<%=Resources.Resource.FraseCaractConsiderados %>:&nbsp; " * "&nbsp; " - " " _ " " . "</td>
							</tr>
						</table>
						<br />
						<br />
						<div class="lblVermelhaBO"><%=Resources.Resource.FraseSelectEquipEditar %></div>
						<asp:datagrid id="DGEquipamentos" Runat="server"  Font-Size="10" OnDeleteCommand ="deleteEquipamento"
							OnUpdateCommand="updateEquipamento" OnCancelCommand="cancelGrid" OnEditCommand="editEquipamento"
							Font-Name="Arial" AutoGenerateColumns="false" AllowPaging="true" PageSize="50" AllowSorting="True"
							OnSortCommand="SortGrid" OnPageIndexChanged="doPaging" PagerStyle-Mode="NumericPages" BorderColor="#FFFFFF"
							GridLines="Both" BorderWidth="2" DataKeyField="idEquipamento" BackColor="Gainsboro" AlternatingItemStyle-BackColor="LightGrey"
							SelectedItemStyle-BackColor="#99cc99" EditItemStyle-Width="100px" EditItemStyle-Height="10px">
							<Columns>
								<asp:ButtonColumn CommandName="Select" Text=">>>" ItemStyle-Width="30"></asp:ButtonColumn>
								<asp:TemplateColumn SortExpression="Estado" HeaderText="Activo.">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:textbox id="txtColor" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.Estado")))%>' Width="10" Height="10" Runat="server">
										</asp:textbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="idEquipamento" SortExpression="idEquipamento" HeaderText="idEquipamento"
									ReadOnly="true"></asp:BoundColumn>
								<asp:BoundColumn DataField="idGrandeza" SortExpression="idGrandeza" HeaderText="Grandeza" ReadOnly="true"></asp:BoundColumn>
								<asp:BoundColumn DataField="familia" SortExpression="familia" HeaderText="Família" ReadOnly="true"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Tipo" SortExpression="tipoEquipamento">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:dropdownlist id="ddTipoEquipamento" Runat="server" DataValueField="idTipoEquipamento" DataTextField="descricao"
											Font-Size="8"></asp:dropdownlist>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="NºID" SortExpression="numIdentificacao" ItemStyle-Font-Size="Large">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="txtNID" Runat="server" Width="80" Font-Size="8"></asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="NºSérie" SortExpression="numSerie"  ItemStyle-Font-Size="Large">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.numSerie") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="txtNS" Runat="server" Width="80" Font-Size="8"></asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="utilCriacao" SortExpression="utilCriacao" HeaderText="Criado por" ReadOnly="true"></asp:BoundColumn>
								<asp:BoundColumn DataField="dtCriacao" SortExpression="dtCriacao" HeaderText="Dt.Criação" DataFormatString="{0:dd/MM/yyyy}"
									ReadOnly="true"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Period.Cal." SortExpression="periodicidadeCalibracao">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.periodicidadeCalibracao") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="txtPC" Runat="server" Width="50" Font-Size="8"></asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Ref.últ.cal." SortExpression="refUltimaCalibracao">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.refUltimaCalibracao") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="txtRUC" Runat="server" Width="80" Font-Size="8"></asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Dt.últ.Cal." SortExpression="dtUltimaCalibracao">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.dtUltimaCalibracao","{0:dd/MM/yyyy}") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="txtDUC" Runat="server" Width="80" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.dtUltimaCalibracao","{0:dd/MM/yyyy}") %>'>
										</asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="Últ.Ref.Serv.Real"
									ItemStyle-BackColor="#99cccc" ReadOnly="true"></asp:BoundColumn>
								<asp:BoundColumn DataField="dtEntradaServico" SortExpression="dtEntradaServico" HeaderText="Dt.últ.Entr."
									ItemStyle-BackColor="#99cccc" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
								<asp:BoundColumn DataField="numServ" SortExpression="numServ" HeaderText="#Serv." ItemStyle-BackColor="#99cccc"
									ItemStyle-HorizontalAlign="Center" ReadOnly="true"></asp:BoundColumn>
								<asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"></asp:EditCommandColumn>
								<asp:ButtonColumn CommandName="Delete" Text="Apagar"></asp:ButtonColumn>
                                <asp:BoundColumn DataField="numIdentificacao" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="numSerie" Visible="false"></asp:BoundColumn>
							</Columns>
						</asp:datagrid><br />
						<br />
						<table id="footer" cellSpacing="0" cellPadding="0" width="584" align="left" border="0"
							style="WIDTH: 584px; HEIGHT: 40px">
							<tr>
								<td><asp:linkbutton id="Linkbutton1" Runat="server" CssClass="LinkVermelhoSemUnderline" CausesValidation="False"><%=Resources.Resource.Detalhes %></asp:linkbutton>&nbsp; 
									| &nbsp;
									<asp:linkbutton id="Linkbutton2" Runat="server" CssClass="LinkVermelhoSemUnderline" CausesValidation="False"><%=Resources.Resource.Historico %></asp:linkbutton>&nbsp; 
									| &nbsp;
									<asp:linkbutton id="Linkbutton3" Runat="server" CssClass="LinkVermelhoSemUnderline" CausesValidation="False"><%=Resources.Resource.Servicos %></asp:linkbutton>&nbsp; 
									| &nbsp;
									<asp:linkbutton id="Linkbutton4" Runat="server" CssClass="LinkVermelhoSemUnderline" CausesValidation="False"><%=Resources.Resource.Certificados %></asp:linkbutton>&nbsp;&nbsp;&nbsp;</td>
							</tr>
						</table>
						<!--DataFormatString="{0:dd/MM/yyyy}"--><br />
						<br />
						<asp:label id="lblMessage2" style="FONT-SIZE: 8pt; FONT-FAMILY: Arial" ForeColor="#ff0033"
							Runat="server"></asp:label><br />
						<br />
						<div class="lblVermelhaBO"><%=Resources.Resource.FraseEquipSelectSubst %></div>
						<%=Resources.Resource.FraseInfoSubstEquipo %>.
						<br />
						<br />
						<asp:dropdownlist id="ddEquipamentoAManter" Runat="server"></asp:dropdownlist>&nbsp;&nbsp;<asp:Button class="button" id="btnTrocar" CssClass="button" Runat="server" Text="<%$ Resources:Resource, TrocarEquipamentos %>"></asp:button><br />
						<br />
						<asp:datagrid id="dgGenerico" Runat="server" CssClass="DG_branco" Font-Size="8" Font-Name="Arial"
							AutoGenerateColumns="True" BorderColor="#FFFFFF" GridLines="Both" BorderWidth="2" BackColor="Gainsboro"
							AlternatingItemStyle-BackColor="LightGrey"></asp:datagrid><br />
						<br />
						<asp:datagrid id="dgCertificados" Runat="server" CssClass="DG_branco" Font-Size="8" Font-Name="Arial"
							AutoGenerateColumns="False" BorderColor="#FFFFFF" GridLines="Both" BorderWidth="2" DataKeyField="nomeDocumento"
							BackColor="Gainsboro" AlternatingItemStyle-BackColor="LightGrey" OnItemCommand="visualisarDocumento"
							OnItemDataBound="dgCertificados_ItemDataBound">
							<Columns>
								<asp:ButtonColumn Text="visualisarDocumento" ButtonType="LinkButton" DataTextField="nomeDocumento"
									Visible="False" SortExpression="nomeDocumento" HeaderText="Visualisar Documento" CommandName="Select">
									<ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
								</asp:ButtonColumn>
								<asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="Ref.Calib."></asp:BoundColumn>
								<asp:BoundColumn DataField="dtCertificado" SortExpression="dtCertificado" HeaderText="Data de Emiss&#227;o"
									DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
								<asp:BoundColumn DataField="responsavelLaboratorio" SortExpression="responsavelLaboratorio" HeaderText="Aprovado Por"
									ItemStyle-Width="20%"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Ficheiro" SortExpression="nomeFicheiro">
									<ItemTemplate>
										<asp:HyperLink Runat=server NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeDocumento"))%>' ID="Hyperlink1" Target=new>
											<%# DataBinder.Eval(Container.DataItem, "nomeDocumento")%>
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
						<!-- FIM body --></td>
				</tr>
				<TR>
					<TD class="text_normal" vAlign="top"></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
