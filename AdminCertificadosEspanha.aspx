<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminCertificadosEspanha.aspx.cs" Inherits="LabMetro.AdminCertificadosEspanha" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:Label ID="lblMessage" Runat="server" ></asp:Label> <div>Importar certificados (UPLOAD)
        <input type="file" id="FileUpload1" multiple="multiple" runat="server"/>
        <asp:Button runat="server" ID="btnupload" Text="Cargar"
            onclick="btnupload_Click"/>&nbsp;<br/>
       

    </div> 


        
        	<asp:DataGrid ID="dgFiles" Runat="server" AutoGenerateColumns="True" 
          
           OnItemCommand="ItemsGrid_Command">
                <Columns><asp:ButtonColumn 
                 HeaderText="Delete" 
                 ButtonType="LinkButton" 
                 Text="Delete" 
                 CommandName="Delete"/>  </Columns>
        	</asp:DataGrid>

			<asp:Button class="button" ID="btnSubmit" Runat="server" Text="Importar Ficheros / Actualizar BD" onclick="btnSubmit_Click"></asp:Button><br />
			<br />
			
    </form>
</body>
</html>
