<%@ Page language="c#" Codebehind="PagErro.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.PagErro" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>:: LabMetro - Erro ::</title>
        <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
        <meta name="CODE_LANGUAGE" Content="C#">
        <meta name="vs_defaultClientScript" content="JavaScript">
        <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
      <LINK href="Styles.css" type="text/css" rel="stylesheet">
    </HEAD>
    <body>
        <h3>Página de Erro</h3>
        <span class="text_normal">Ocorreu um erro na página:<%=Request.QueryString["aspxerrorpath"] %><br />
           Por favor comuniquem o erro por email para <a href="mailto:dmelamed@isq.pt">dmelamed@isq.pt</a><br />
        
            <br /> <br />
        </span><button value="Voltar" onclick="javascript:history.back();" class="button" type="button">
            Voltar</button>
    </body>
</HTML>
