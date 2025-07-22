<%@ Page Title="" Language="C#" MasterPageFile="~/mp.Master" AutoEventWireup="true" CodeBehind="RepIndicadoresGestao.aspx.cs" Inherits="LabMetro.RepIndicadoresGestao" %>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>



<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    <%-- <script type="text/javascript" src="crystalreportviewers13/js/crviewer/crv.js"></script> --%>
  <%--  <script src='<%=ResolveUrl("~/crystalreportviewers13/js/crviewer/crv.js")%>' type="text/javascript"></script>--%>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
 
    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnReport").focus();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Indicadores Gestão Labmetro</legend>
          <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
       
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblDtInicio" runat="server"><%=Resources.Resource.DataInicio %>:</asp:Label>
                   
                </td>
                <td>
                    <asp:TextBox ID="txtDtInicio" runat="server"></asp:TextBox> <asp:CompareValidator
                            ID="Comparevalidator1" runat="server" ControlToValidate="txtDtInicio"
                            Operator="DataTypeCheck" Type="Date"><%=Resources.Resource.DataInvalida %></asp:CompareValidator>
                </td>
                <td>
                    <asp:Label ID="lblDtFim" runat="server"><%=Resources.Resource.DataFim %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtFim" runat="server"></asp:TextBox> <asp:CompareValidator
                            ID="Comparevalidator2" runat="server" ControlToValidate="txtDtFim"
                            Operator="DataTypeCheck" Type="Date"><%=Resources.Resource.DataInvalida %></asp:CompareValidator>
                </td>
            </tr>
            
            <tr>
                <td colspan="5">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="5">
                    <asp:Button class="button" ID="btnReport" CssClass="button" runat="server" Text="Ver Relatório"
                        OnClick="btnReport_Click"></asp:Button>

                     <asp:Button class="button" ID="btnExcel" CssClass="button" runat="server" Text="Exportar p/ Excel"
                        OnClick="btnExportExcel_Click"></asp:Button>
                    <br />
                   
                    <br />
                </td>
            </tr>
        </table>
        <asp:GridView runat="server" AutoGenerateColumns="true" ID="gvReport"></asp:GridView>
         &nbsp;<asp:DataGrid ID="dgReport" runat="server" AutoGenerateColumns="True">
        </asp:DataGrid>
         <br />
        <p>NOTA EXPLICATIVA:

</p>
        <p>todos os reports aqui apresentados baseiam-se numa View &quot;vServicosRealizados&quot; que exclui todos os serviços anulado por erro (Estado -1)</p>
        <p>Núm entradas entre --- e ----: pela Data do BRE, todos os serviços que entraram, inclusivé os posteriormente anulados</p>
        <p>&nbsp;</p>
        <p>Núm Serviços realizados entre --- e ----&nbsp; pela Data de Realização, - nao relacionado com data de entrada atenção que sao apenas contabilizados alguns estados finais, ATENÇÃO há serviços que param ANTES (ex nunca sao &quot;entregues&quot;)

</p>
        <p>Núm Serviços anulado entre --- e ----&nbsp; São relacionados com as entradas do período (data BRE)

</p>
        <p>Núm Serviços em backlog --- e ---- sem Data, são todos os que se encontram no estado Aguarda Calibração ou equivalento.

</p>
        <p>Média de dias, relacionados com as datas do BRE (data de Entrada)</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>



       
        </fieldset>
</asp:Content>