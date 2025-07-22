<%@ Page language="c#" Codebehind="ListaServicosRequisicao.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.ListaServicosRequisicao" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Lista de Serviços por Requisição</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<br />
			<br />
			<asp:Label ID="lblMessage" CssClass="lblMessage" Runat="server"></asp:Label>
			<asp:DataGrid 
			ID="dgServicos"  
			Runat="server" 
			AutoGenerateColumns="false">
					
				<Columns>
					<asp:BoundColumn DataField="dtBre" SortExpression="dtBre" HeaderText="Data Bre" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
					<asp:BoundColumn DataField="refServico" HeaderText="Ref.Serviço"></asp:BoundColumn>
						<asp:BoundColumn DataField="dtCalibracao" SortExpression="dtCalibracao" HeaderText="Data Cal." DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
					<asp:BoundColumn DataField="tipoEquipamento" HeaderText="Tipo Equip." ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
					<asp:BoundColumn DataField="numIdentificacao" HeaderText="Núm. Ident." ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
					<asp:BoundColumn DataField="numSerie" HeaderText="Núm. Série." ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
					<asp:BoundColumn DataField="refFactura" HeaderText="Factura"></asp:BoundColumn>
					<asp:BoundColumn DataField="valor" HeaderText="valor" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
				
				</Columns>
			</asp:DataGrid>
		</form>
	</body>
</HTML>
