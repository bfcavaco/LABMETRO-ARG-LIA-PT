<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="True" Inherits="LabMetro._Default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
    <head>
        <title>:: LabMetro ::</title>
        <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
        <meta content="C#" name="CODE_LANGUAGE" />
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchemaa" />
        <link href="labmetro.css" type="text/css" rel="stylesheet" />  	
        <style type="text/css">
		body {background-color: white; }
		</style>
    </head>
  	<body>


        <form id="Form1" method="post" runat="server">
            <table id="tblLogin" cellspacing="0" cellpadding="0" width="300" border="0" runat="server">
              
                <tr class="LoginHeader">
                    <td><%=Resources.Resource.UtilizadoresRegistados %></td>
                </tr>
                <tr>
                    <td><asp:label id="lblMessage" runat="server"></asp:label></td>
                </tr>
                <tr>
                    <td><asp:label id="lblUsername" runat="server">Username:</asp:label><br />
                        <asp:textbox id="txtUsername" runat="server"></asp:textbox></td>
                </tr>
                <tr>
                    <td><asp:label id="lblPassword" runat="server">Password:</asp:label><br />
                        <INPUT id="txtPassword" style="WIDTH: 145px" type="password" size="18" name="Password1"
                            runat="server" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center"><asp:Button cssClass="button" id="btnSubmit" runat="server" Text="<%$ Resources:Resource, IniciarSessao %>" onclick="btnSubmit_Click"></asp:button></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Button class="button" id="btnPassword" runat="server" CssClass="button" Text="<%$ Resources:Resource, MudarPassword %>" CausesValidation="false" onclick="btnPassword_Click"></asp:button></td>
                </tr>
            </table>
            <table id="tblPwd" cellSpacing="0" cellPadding="0" width="300" border="0" runat="server">
                <tr class="LoginHeader">
                    <td><%=Resources.Resource.AlterarPassword %></td>
                </tr>
                <tr>
                    <td><asp:label id="Label1" runat="server"></asp:label></td>
                </tr>
                <tr>
                    <td><asp:label id="Label2" runat="server">Username:</asp:label><br />
                        <asp:textbox id="txtUserNamePwd" runat="server"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" Display="Static" ErrorMessage="*" ControlToValidate="txtUserNamePwd"
                            Runat="server"></asp:requiredfieldvalidator></td>
                </tr>
                <tr>
                    <td><asp:label id="Label3" runat="server"><%=Resources.Resource.PasswordActual %>:</asp:label><br />
                        <asp:textbox id="passwordold" Runat="server" TextMode="Password"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator2" Display="Static" ErrorMessage="*" ControlToValidate="passwordold"
                            Runat="server"></asp:requiredfieldvalidator></td>
                </tr>
                <tr>
                    <td><asp:label id="Label4" runat="server"><%=Resources.Resource.PasswordNova %>:</asp:label><br />
                        <asp:textbox id="passwordnew1" Runat="server" TextMode="Password"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator3" Display="Static" ErrorMessage="*" ControlToValidate="passwordnew1"
                            Runat="server"></asp:requiredfieldvalidator><asp:comparevalidator id="Comparevalidator3" runat="server" ErrorMessage="Password nova igual à antiga."
                            ControlToValidate="passwordnew1" Operator="NotEqual" ControlToCompare="passwordold"></asp:comparevalidator></td>
                </tr>
                <tr>
                    <td><asp:label id="Label5" runat="server"><%=Resources.Resource.Confirmar %>&nbsp;<%=Resources.Resource.PasswordNova %>:</asp:label><br />
                        <asp:textbox id="passwordnew2" Runat="server" TextMode="Password"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator4" Display="Static" ErrorMessage="*" ControlToValidate="passwordnew2"
                            Runat="server"></asp:requiredfieldvalidator><asp:comparevalidator id="Comparevalidator2" runat="server" ErrorMessage="Passwords não são iguais." ControlToValidate="passwordnew2"
                            ControlToCompare="passwordnew1"></asp:comparevalidator></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center"><asp:Button class="button" id="btnChangePwd" runat="server" CssClass="button" Text="<%$ Resources:Resource, Submeter %>" CausesValidation="true" onclick="btnChangePwd_Click"></asp:Button></td>
                </tr>
                <tr>
                    <td align="right">&nbsp; <input type="button" value="Voltar" onclick="javascript:history.back();" class="button"></td>
                </tr>
            </table>
        </form>
    </body>
</html>
