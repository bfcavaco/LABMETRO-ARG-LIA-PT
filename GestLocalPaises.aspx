<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - Localidades e Países ::</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form2" method="post" runat="server">
			<table id="tblMain">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
					<td vAlign="top" height="20"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top">
						<!-- body -->
						<asp:Label ID="lblMessage" Runat="server" CssClass="lblMessage"></asp:Label>
						<br>
						<br>
						Tabela:<asp:DropDownList ID="ddTabela" Runat="server" DataTextField="descricao" DataValueField="ident" AutoPostBack="true"></asp:DropDownList>
						
						<br>
						<br>
						Pesquisar:
						<asp:TextBox ID="txtNome" Runat="server"></asp:TextBox>
						<asp:Button class="button" CssClass="button" Runat="server" Text="Pesquisar" id="btnSearch"></asp:Button>
						<br>
						<br>
						<asp:DataGrid ID="DG" Runat="server" autogeneratecolumns="false" OnEditCommand="DG_Edit" OnCancelCommand="DG_CancelGrid"
							OnUpdateCommand="DG_UpdateGrid" DataKeyField="ident" ShowFooter="True" AllowPaging="true"
							PageSize="25" AllowSorting="True" OnPageIndexChanged="DoPaging" OnSortCommand="SortGrid" PagerStyle-Mode="NumericPages"
							HeaderStyle-ForeColor="#FFFFFF" HeaderStyle-Font-Bold="True" HeaderStyle-BackColor="#999999"
							BorderColor="#E0E0E0" GridLines="horizontal" CssClass="DG_branco" Width="50%">
							<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
							<FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Descrição" SortExpression="descricao">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.descricao") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox ID="txtDescricao" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>' Runat="server" />
									</EditItemTemplate>
									<FooterTemplate>
										<asp:TextBox ID="txtDescricaoFooter" Runat="server" />
									</FooterTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Estado" SortExpression="activo">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.activo"))) %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DropDownList ID="ddEstado" Runat="server">
											<asp:ListItem Value="1">activo</asp:ListItem>
											<asp:ListItem Value="0">inactivo</asp:ListItem>
										</asp:DropDownList>
									</EditItemTemplate>
									<FooterTemplate>
										<asp:DropDownList ID="ddEstadoFooter" Runat="server">
											<asp:ListItem Value="1">activo</asp:ListItem>
											<asp:ListItem Value="0">inactivo</asp:ListItem>
										</asp:DropDownList>
									</FooterTemplate>
								</asp:TemplateColumn>
								<asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar" ItemStyle-Width="100"></asp:EditCommandColumn>
								<asp:TemplateColumn HeaderText="">
									<FooterTemplate>
										<asp:LinkButton id="lnkButtonAdd" CommandName="Insert" runat="server" Text="Adicionar Registo"></asp:LinkButton>
									</FooterTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
						<asp:DataGrid ID="DGPais" Runat="server" autogeneratecolumns="false" OnEditCommand="DGPais_Edit"
							OnCancelCommand="DGPais_CancelGrid" OnUpdateCommand="DGPais_UpdateGrid" DataKeyField="ident"
							ShowFooter="True" AllowPaging="false" PageSize="25" HeaderStyle-ForeColor="#FFFFFF" HeaderStyle-Font-Bold="True"
							HeaderStyle-BackColor="#999999" BorderColor="#E0E0E0" GridLines="horizontal" CssClass="DG_branco"
							Width="50%">
							<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
							<FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="ID" SortExpression="ident">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.ident") %>
									</ItemTemplate>
									<FooterTemplate>
										<asp:TextBox ID="txtIdPaisFooter" Runat="server" />
									</FooterTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Descrição" SortExpression="descricao">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.descricao") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox ID="txtDescricaoPais" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>' Runat="server" />
									</EditItemTemplate>
									<FooterTemplate>
										<asp:TextBox ID="txtDescricaoPaisFooter" Runat="server" />
									</FooterTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Estado" SortExpression="activo">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.activo"))) %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DropDownList ID="ddEstadoPais" Runat="server">
											<asp:ListItem Value="1">activo</asp:ListItem>
											<asp:ListItem Value="0">inactivo</asp:ListItem>
										</asp:DropDownList>
									</EditItemTemplate>
									<FooterTemplate>
										<asp:DropDownList ID="ddEstadoPaisFooter" Runat="server">
											<asp:ListItem Value="1">activo</asp:ListItem>
											<asp:ListItem Value="0">inactivo</asp:ListItem>
										</asp:DropDownList>
									</FooterTemplate>
								</asp:TemplateColumn>
								<asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar" ItemStyle-Width="100"></asp:EditCommandColumn>
								<asp:TemplateColumn HeaderText="">
									<FooterTemplate>
										<asp:LinkButton id="Linkbutton1" CommandName="Insert" runat="server" Text="Adicionar Registo"></asp:LinkButton>
									</FooterTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
						<asp:DataGrid ID="DGTipoCertificado" Runat="server" autogeneratecolumns="false" OnEditCommand="DGTipoCertificado_Edit"
							OnCancelCommand="DGTipoCertificado_CancelGrid" OnUpdateCommand="DGTipoCertificado_UpdateGrid"
							DataKeyField="idTipoCertificado" ShowFooter="True" AllowPaging="false" PageSize="25" HeaderStyle-ForeColor="#FFFFFF"
							HeaderStyle-Font-Bold="True" HeaderStyle-BackColor="#999999" BorderColor="#E0E0E0" GridLines="horizontal"
							CssClass="DG_branco" Width="50%">
							<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
							<FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Descrição" SortExpression="descricao">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.descricao") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox ID="txtDescricaoTC" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>' Runat="server" />
									</EditItemTemplate>
									<FooterTemplate>
										<asp:TextBox ID="txtDescricaoTCFooter" Runat="server" />
									</FooterTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Sigla" SortExpression="sigla">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.sigla") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox ID="txtSigla" Text='<%# DataBinder.Eval(Container, "DataItem.sigla") %>' Runat="server" />
									</EditItemTemplate>
									<FooterTemplate>
										<asp:TextBox ID="txtSiglaFooter" Runat="server" />
									</FooterTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Estado" SortExpression="activo">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.activo"))) %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DropDownList ID="ddTCEstado" Runat="server">
											<asp:ListItem Value="1">activo</asp:ListItem>
											<asp:ListItem Value="0">inactivo</asp:ListItem>
										</asp:DropDownList>
									</EditItemTemplate>
									<FooterTemplate>
										<asp:DropDownList ID="ddTCEstadoFooter" Runat="server">
											<asp:ListItem Value="1">activo</asp:ListItem>
											<asp:ListItem Value="0">inactivo</asp:ListItem>
										</asp:DropDownList>
									</FooterTemplate>
								</asp:TemplateColumn>
								<asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar" ItemStyle-Width="100"></asp:EditCommandColumn>
								<asp:TemplateColumn HeaderText="">
									<FooterTemplate>
										<asp:LinkButton id="Linkbutton2" CommandName="Insert" runat="server" Text="Adicionar Registo"></asp:LinkButton>
									</FooterTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
						<!-- FIM body -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
