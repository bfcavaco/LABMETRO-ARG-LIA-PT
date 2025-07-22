<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaVDs.aspx.cs" Inherits="LabMetro.ListaVDs"
    MasterPageFile="~/mp.Master" %>


<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">   <script type="text/javascript">

        $.datepicker.setDefaults({ showButtonPanel: true });

        $("#datepicker").datepicker($.datepicker.regional["pt"]);

        $(function() {

        /*$("#inputDtInicio").datepicker();*/

        /*resumto das tentativas frustradas de pôr isto a funcionar: enquando eu não usava masterpage, o codigo acim funcionava muito bem.
        quando juntei masterpage, por alguma razao o client id ficou alterado, o datepicker funciona no entanto com um input normal sem ser runat server. desta forma como está na linha a seguir, eu refiro.me ao control pela sua classe e não pelo seu id.... e isto funciona*/
        $('.date-input').datepicker(); 
        });

        /*$(function() {

            $("#txtDataFim").datepicker();
        });  */

    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Listas VDs</legend>Número funcionário:
        <asp:TextBox ID="txtNumFuncionario" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" ControlToValidate="txtNumFuncionario">*</asp:RequiredFieldValidator><br />
        Data Inicio:
        <asp:TextBox ID="txtDataInicio" runat="server" CssClass="date-input"></asp:TextBox>


        
       
        <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" ControlToValidate="txtDataInicio">*</asp:RequiredFieldValidator><br />
        Data Fim:
        <asp:TextBox ID="txtDataFim" runat="server"  CssClass="date-input"></asp:TextBox>
        <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" ControlToValidate="txtDataFim">*</asp:RequiredFieldValidator><br />
        <asp:RadioButtonList runat="server" ID="rblTipoLista">
            <asp:ListItem Value="1" Selected="True">Lista de Duplicados</asp:ListItem>
            <asp:ListItem Value="2">Listagem de VDs efectuadas entre - e </asp:ListItem>
        </asp:RadioButtonList>
        <asp:Button class="button" ID="btnImprimir" CssClass="button" runat="server" Text="Imprimir Lista"
            Width="150" OnClick="btnImprimir_Click" CausesValidation="true" />
    </fieldset>
</asp:Content>
