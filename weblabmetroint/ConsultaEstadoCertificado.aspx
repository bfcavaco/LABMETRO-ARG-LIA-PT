<%@ Page Language="c#" CodeBehind="ConsultaEstadoCertificado.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ConsultaEstadoCertificado" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
        ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
    <fieldset>
        <legend><%=Resources.Resource.ConsultarEstadoCertifcado %></legend><%=Resources.Resource.RefServ %>:
        <asp:TextBox ID="txtRefServico" runat="server"></asp:TextBox>
        <asp:Button class="button" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Procurar %>" OnClick="btnSubmit_Click">
        </asp:Button>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <asp:DataGrid 
        ID="dgResult" 
        runat="server" 
        DataKeyField="refServico"
        OnSortCommand="SortGrid" 
        OnPageIndexChanged="DoPaging" 
        AllowSorting="True" 
        PageSize="50"
        AllowPaging="true" 
        AutoGenerateColumns="true" 
        OnSelectedIndexChanged="dgResult_SelectedIndexChanged">
            <Columns>
                <asp:ButtonColumn CommandName="select" HeaderText="Detalhes" Text="ver" ItemStyle-Font-Size="8"
                    ItemStyle-HorizontalAlign="Center"></asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
        <br />
        <br />
        <div class="errorMessage">
        </div>
        <br />
        <br />
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <br />
        <br />
        <%=Resources.Resource.Legenda %>:
        <br />
        <br />
        <table style="font-size: 8pt" width="600" border="1" bordercolor="lightgrey" cellpadding="2"
            cellspacing="0">
            <tr>
                <td colspan="4">
                    <br />
                    <%=Resources.Resource.FraseAvisoConsultaCert %>
                </td>
            </tr>
            <tr bgcolor="gainsboro">
                <td>
                    <%=Resources.Resource.EstadoCertificado %>
                </td>
                <td>
                    <%=Resources.Resource.Descricao %>
                </td>
                <td>
                    <%=Resources.Resource.Accao %>
                </td>
            </tr>
            <tr>
                <td>
                   <%=Resources.Resource.NaoExistente %>
                </td>
                <td>
                    <%=Resources.Resource.FraseNegaCriacaoPDF %>
                </td>
                <td>
                    <%=Resources.Resource.FraseAccaoPassar %>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.CalibSemValidacao %>
                </td>
                <td>
                    <%=Resources.Resource.FraseDescCalibSemValid %>
                </td>
                <td>
                    <%=Resources.Resource.FraseAccaoCalibSemValid %>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.CalibradoComValidacao %>
                </td>
                <td>
                    <%=Resources.Resource.FraseDescCalibComValid %>
                </td>
                <td>
                    <%=Resources.Resource.FraseAccaoCalibComValid %>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Certificado %>
                </td>
                <td>
                    <%=Resources.Resource.FraseDescCertificado %>
                </td>
                <td>
                    <%=Resources.Resource.FraseAccaoCertificado %>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.RejeitadoPeloTecnico %>
                </td>
                <td>
                    <%=Resources.Resource.FraseDescRejTecnico %>
                </td>
                <td rowspan="2">
                    <%=Resources.Resource.FraseAccaoRejTecnico %>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.RejeitadoPeloResponsavel %>
                </td>
                <td>
                    <%=Resources.Resource.FraseDescRejResponsavel %>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
