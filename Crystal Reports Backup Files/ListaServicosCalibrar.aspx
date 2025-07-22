<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaServicosCalibrar.aspx.cs" Inherits="LabMetro.ListaServicosCalibrar" MasterPageFile="~/mp.Master"%>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
  <fieldset>
        <legend></legend>
    <%=Resources.Resource.GrandezaLaboratorio %>.:
            
                    <asp:DropDownList ID="ddGrandeza" runat="server" 
        DataTextField="descricao" DataValueField="ident" 
        onselectedindexchanged="ddGrandeza_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>&nbsp;  <%=Resources.Resource.LocalEquip %>:
               
                    <asp:DropDownList ID="ddLocalCalibracao" runat="server" 
        DataValueField="ident" DataTextField="descricao" 
        onselectedindexchanged="ddLocalCalibracao_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="-1" Text=""></asp:ListItem>
                    </asp:DropDownList><br />
                    <br />
                    
                    <asp:Label runat="server" ID="lblCountServicos"></asp:Label><br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:connstr %>" 
        
        SelectCommand="SELECT * FROM [vAguardaCalibNew] WHERE ([idGrandeza] = @idGrandeza) and (idLocalCalibracao = @idLocalCalibracao or @idLocalCalibracao = -1) order by idNivelPrioridade desc, dtprevisao asc">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddGrandeza" Name="idGrandeza" 
                PropertyName="SelectedValue" Type="String" />
            <asp:ControlParameter ControlID="ddLocalCalibracao" Name="idLocalCalibracao"  DefaultValue="-1"
                PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
<asp:GridView id="gvServicos" runat="server" AllowSorting="True" 
        DataSourceID="SqlDataSource1" AutoGenerateColumns="False" 
        DataKeyNames="idServico">
    <Columns>
       
            <asp:HyperLinkField HeaderText="Ref.Serviço" DataNavigateUrlFields="idServico" DataTextField="refServico" DataNavigateUrlFormatString="FormPastaEnsaio.aspx?btn=LAB&id={0}" Target="_self" SortExpression="refServico" />
           
            <asp:TemplateField SortExpression="bVariasGrandezas" HeaderText="M.S.">
            <ItemTemplate>
             <%# LabMetro.GERAL.clsGeral.ConverteBoolSimNao(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bVariasGrandezas")))%>
            </ItemTemplate>
            </asp:TemplateField>
            
            <asp:BoundField DataField="Prioridade" HeaderText="<%$ Resources:Resource, Prioridade %>" 
            SortExpression="Prioridade" />
                  <asp:BoundField DataField="dtPrevisao" HeaderText="<%$ Resources:Resource, DataPrevisao %>" 
            SortExpression="dtPrevisao" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="observacoes" HeaderText="<%$ Resources:Resource, Obs %>" 
            SortExpression="observacoes" />
        <asp:BoundField DataField="nomeAbreviado" HeaderText="<%$ Resources:Resource, Cliente %>" 
            SortExpression="nomeAbreviado" />
        <asp:BoundField DataField="descricao" HeaderText="<%$ Resources:Resource, Equipamento %>" 
            SortExpression="descricao" />
        <asp:BoundField DataField="refBRE" 
            HeaderText="<%$ Resources:Resource, BRE %>" ReadOnly="True" 
            SortExpression="refBRE" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="dtBRE" HeaderText="<%$ Resources:Resource, DataBRE %>" 
            SortExpression="dtBRE" />
        <asp:BoundField DataField="dtAguarda" HeaderText="<%$ Resources:Resource, DataAguarda %>" 
            ReadOnly="True" SortExpression="dtAguarda" />
        <asp:BoundField DataField="localEquipamento" HeaderText="<%$ Resources:Resource, Local %>" 
            SortExpression="localEquipamento" />
        <asp:BoundField DataField="diasDesdeBRE" HeaderText="<%$ Resources:Resource, DiasDesdeBRE %>" 
            ReadOnly="True" SortExpression="diasDesdeBRE" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="diasDesdeAguardaCalib" 
            HeaderText="<%$ Resources:Resource, DiasDesdeAguardaCalib %>" ReadOnly="True" 
            SortExpression="diasDesdeAguardaCalib" ItemStyle-HorizontalAlign="Center" />
        
        <asp:TemplateField HeaderText="<%$ Resources:Resource, Requisicao %>" SortExpression="nomeFicheiro">
          
            <ItemTemplate>
                <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                            ID="Hyperlink1" Target="new" Text='<%# Bind("nomeFicheiro") %>'>
											
                        </asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    </asp:GridView><br />
     <asp:Button 
                ID="btnExportGrid" runat="server" 
                Text="Exportar Excel" OnClick="BtnExportGrid_Click" />
    </fieldset>
</asp:Content>

