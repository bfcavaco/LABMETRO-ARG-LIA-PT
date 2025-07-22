<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepEmpresas.aspx.cs" Inherits="LabMetro.RepEmpresas"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisa").focus();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Reports Empresas</legend>
        Aqui vão entrar reports imprimiváveis sobre empresas como:<br /><br />
        
            <img src="IMAGES/ic_seta_red.gif" /> empresas com código bloqueio errado...<br />
            <img src="IMAGES/ic_seta_red.gif" /> empresas por actividade e região...<br />
            <img src="IMAGES/ic_seta_red.gif" /> empresas sem concelho...<br />
            <img src="IMAGES/ic_seta_red.gif" /> empresas sem actividade...<br />
           
            <img src="IMAGES/ic_seta_red.gif" /> empresas sem número cliente...<br />
            <img src="IMAGES/ic_seta_red.gif" /> Serviços por empresa...<br />
            <img src="IMAGES/ic_seta_red.gif" /> Equipamentos por empresa...<br />
            <img src="IMAGES/ic_seta_red.gif" /> serviços/equipamentos por outros critérios, tipo grupos de empresas...<br />
            <img src="IMAGES/ic_seta_red.gif" /> Empresas por distrito/concelho...<br />
            <img src="IMAGES/ic_seta_red.gif" /> Empresas por tipo de actividade (lista comparativa)...<br />
            <img src="IMAGES/ic_seta_red.gif" /> empresas com contactos (para mailings etc) ...<br />
            <img src="IMAGES/ic_seta_red.gif" /> [outros]<br />
            idealmente reports podem ser exportados para pdf e excel
           
    </fieldset>
</asp:Content>
