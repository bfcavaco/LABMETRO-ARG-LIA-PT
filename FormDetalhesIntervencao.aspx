<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormDetalhesIntervencao.aspx.cs"
    Inherits="LabMetro.FormDetalhesIntervencao" MasterPageFile="~/mp.Master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .formLabel
        {
            width: 100em;
            text-align: left;
            margin-left: 0.5em;
            font-weight: bold;
        }
        .submit input
        {
            margin-left: 1.5em;
        }
        .fvForm label
        {
            width: 15em;
            float: left;
            text-align: right;
            margin-right: 0.5em;
            display: block;
        }
        .fvForm input, select, textarea
        {
            margin-left: 1.5em;
        }
        .fvForm textarea
        {
            width: 250px;
            height: 150px;
        }
        .fvForm .boxes
        {
            width: 1em;
        }
        br
        {
            clear: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnSubmit").focus();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Detalhes Intervenção</legend>
     <input type="button" value="Voltar" onclick="javascript:history.back();" class="button" />
    </fieldset>
</asp:Content>
