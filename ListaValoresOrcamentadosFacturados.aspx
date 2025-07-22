<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaValoresOrcamentadosFacturados.aspx.cs"
    Inherits="LabMetro.ListaValoresOrcamentadosFacturados" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Valores Orçamentados / Facturados por empresa</legend>
        <br />
        <br />
        <asp:SqlDataSource SelectCommandType="Text" SelectCommand="select * from [vValoresOrcamentadosFacturadosComDetalhe] order by ano, empresa"
            ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connstr %>" />
       
        <asp:GridView ID="gvResultados" runat="server"  DataSourceID="SqlDataSource1"
            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
            DataKeyNames="idEmpresa">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="ano" HeaderText="ano" SortExpression="ano" />
                <asp:BoundField DataField="idEmpresa" HeaderText="idEmpresa" 
                    InsertVisible="False" ReadOnly="True" SortExpression="idEmpresa" />
                <asp:BoundField DataField="empresa" HeaderText="empresa" 
                    SortExpression="empresa" />
                <asp:BoundField DataField="localidade" HeaderText="localidade" 
                    SortExpression="localidade" />
                <asp:BoundField DataField="morada" HeaderText="morada" 
                    SortExpression="morada" />
                <asp:BoundField DataField="f" HeaderText="f" SortExpression="f" />
                <asp:BoundField DataField="o" HeaderText="o" SortExpression="o" />
                <asp:BoundField DataField="actividade" HeaderText="actividade" 
                    SortExpression="actividade" />
                <asp:BoundField DataField="concelho" HeaderText="concelho" 
                    SortExpression="concelho" />
                <asp:BoundField DataField="distrito" HeaderText="distrito" 
                    SortExpression="distrito" />
                <asp:BoundField DataField="codigoBloqueio" HeaderText="codigoBloqueio" 
                    SortExpression="codigoBloqueio" />
                <asp:BoundField DataField="codigoBloqueioSAP" HeaderText="codigoBloqueioSAP" 
                    SortExpression="codigoBloqueioSAP" />
                <asp:BoundField DataField="descricao" HeaderText="descricao" 
                    SortExpression="descricao" />
                <asp:BoundField DataField="contacto" HeaderText="contacto" 
                    SortExpression="contacto" />
                <asp:BoundField DataField="telefoneEmpresa" HeaderText="telefoneEmpresa" 
                    SortExpression="telefoneEmpresa" />
                <asp:BoundField DataField="emailempresa" HeaderText="emailempresa" 
                    SortExpression="emailempresa" />
            </Columns>
            
            
        </asp:GridView>
        <br />
        <asp:Button ID="btnExportGrid" runat="server" Text="Exportar para Excel" OnClick="BtnExportGrid_Click" />
    </fieldset>
</asp:Content>

