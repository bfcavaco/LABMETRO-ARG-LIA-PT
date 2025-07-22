<%@ Page language="c#" Codebehind="AdminSAPFiles.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.AdminSAPFiles" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AdminSAPFiles</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:DataGrid runat="server" id="dgFiles" Font-Name="Verdana" AutoGenerateColumns="True" AlternatingItemStyle-BackColor="#eeeeee"
				HeaderStyle-BackColor="Gainsboro" HeaderStyle-ForeColor="White" HeaderStyle-Font-Size="13pt"
				HeaderStyle-Font-Bold="True" CssClass="DG_branco" Width="60%" OnItemCommand="dgFiles_ItemCommand"
				DataKeyField="Name">
				<Columns>
					<asp:TemplateColumn HeaderText="Ver Ficheiro" SortExpression="nomeFicheiro">
						<ItemTemplate>
							<asp:HyperLink Runat=server NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.Name"))%>' ID="Hyperlink1" Target=new>
								<%# DataBinder.Eval(Container.DataItem, "Name")%>
							</asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:ButtonColumn CommandName="fillData" Text="Ver dados"></asp:ButtonColumn>
					<asp:BoundColumn DataField="LastWriteTime" HeaderText="Dt. Modif." ItemStyle-HorizontalAlign="Center"
						DataFormatString="{0:d}" />
					<asp:BoundColumn DataField="Length" HeaderText="Tamanho" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,### bytes}" />
				</Columns>
			</asp:DataGrid>
			<asp:DataGrid ID="DG" Runat="server" AutoGenerateColumns="True" AlternatingItemStyle-BackColor=#cccccc></asp:DataGrid>
			<asp:Button class="button" ID="btnSubmit" CssClass="button" Runat="server" Text="Importar Dados" onclick="btnSubmit_Click"></asp:Button>
		</form>
	</body>
</HTML>
