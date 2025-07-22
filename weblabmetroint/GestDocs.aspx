<%@ Page Language="c#" CodeBehind="GestDocs.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestDocs"
    EnableViewState="True" EnableEventValidation="false" MasterPageFile="~/mp.Master" %>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.ValDeCertificados %></legend>
        <asp:Label ID="lblAlertaAprovacao" runat="server" CssClass="lblMessage" /><br />
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchEmpresa" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Grandeza %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddGrandeza" runat="server" AutoPostBack="True" DataTextField="descricao"
                        DataValueField="idGrandeza">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.RefServico %>:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchNServico" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button class="button" ID="btnSearch" runat="server" CssClass="button"  Text="<%$ Resources:Resource, Pesquisar %>">
                    </asp:Button>&nbsp;
                    <asp:Button class="button" ID="btnLimparCampos" runat="server" CssClass="button"
                        Text="<%$ Resources:Resource, LimparCampos %>"></asp:Button>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <br />
       Só meus: <asp:CheckBox ID="cbCertifsUser" runat="server" AutoPostBack="True" />
        <asp:DataGrid ID="dgDocumentos" runat="server" AutoGenerateColumns="False" OnItemDataBound="dgDocumentos_ItemDataBound"
            OnSortCommand="SortGrid" AllowSorting="False" OnItemCommand="visualisarDocumento"
            AllowPaging="False" DataKeyField="idServico">
            <Columns>
                <asp:ButtonColumn Text="visualisarDocumento" ButtonType="LinkButton" DataTextField="nomeDocumento"
                    Visible="false" SortExpression="nomeDocumento" HeaderText="<%$ Resources:Resource, VerDocumento %>"
                    CommandName="Select">
                    <ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
                </asp:ButtonColumn>
                <asp:BoundColumn DataField="idServico" SortExpression="idServico" HeaderText="<%$ Resources:Resource, IdServico %>"
                    Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="equipamento" SortExpression="equipamento" HeaderText="<%$ Resources:Resource, Equipamento %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtEstadoServico" SortExpression="dtEstadoServico" HeaderText="<%$ Resources:Resource, DtEstado %>"
                    DataFormatString="{0:dd/MM/yyyy}" Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="nomeFichCalibracao" SortExpression="nomeFichCalibracao"
                    HeaderText="<%$ Resources:Resource, FicheiroCalibracao %>" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="<%$ Resources:Resource, RefCalib %>"
                    Visible="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="tecnicoLaboratorio" SortExpression="tecnicoLaboratorio"
                    HeaderText="<%$ Resources:Resource, TecnicoCalibValid %>" Visible="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="username" SortExpression="username" HeaderText="<%$ Resources:Resource, UserName %>"
                    Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="idEstadoCertificado" SortExpression="idEstadoCertificado"
                    HeaderText="<%$ Resources:Resource, IdEstadoCertificado %>" Visible="false"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Aprovar %>">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbAprovar" runat="server" Checked='<%#DataBinder.Eval(Container, "DataItem.cbAprovar")%>'>
                        </asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Rejeitar %>">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbRejeitar" runat="server" Checked='<%#DataBinder.Eval(Container, "DataItem.cbRejeitar")%>'>
                        </asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Observacoes %>">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    <ItemTemplate>
                        <asp:TextBox ID="txtObservacoes" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.obsWorkflowCertificado")%>' Wrap="true">
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="belongsToUser" SortExpression="belongsToUser" HeaderText="<%$ Resources:Resource, PertenceUtilizador %>"
                    Visible="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Acreditado %>">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbTemplate" runat="server" AutoPostBack="True" OnCheckedChanged="cbTemplate_checked"
                            Checked='<%#DataBinder.Eval(Container, "DataItem.cbTemplate")%>'></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="acreditado" HeaderText="<%$ Resources:Resource, Acreditado %>" Visible="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="simbolo" HeaderText="<%$ Resources:Resource, SimboloAcreditacao %>" ItemStyle-BackColor="Gainsboro">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="estadoServico" HeaderText="<%$ Resources:Resource, EstadoServico %>" ItemStyle-BackColor="Gainsboro">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtEstadoServico" DataFormatString="{0:dd/MM/yyyy}" HeaderText="<%$ Resources:Resource, DtEstado %>"
                    ItemStyle-BackColor="Gainsboro" Visible="true"></asp:BoundColumn>
                <asp:BoundColumn DataField="estadoCertificado" HeaderText="<%$ Resources:Resource, EstadoCertificado %>" ItemStyle-BackColor="Gainsboro">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="nomeFichCalibracao" HeaderText="<%$ Resources:Resource, Ficheiro %>" ItemStyle-BackColor="Gainsboro">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="nomeDocumento" HeaderText="" ItemStyle-BackColor="Gainsboro"
                    Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="tipoCertificado" HeaderText="<%$ Resources:Resource, Tipo %>" ItemStyle-BackColor="Gainsboro"
                    Visible="true"></asp:BoundColumn>
                <asp:BoundColumn DataField="refServicoPai" HeaderText="<%$ Resources:Resource, RefServicoPai %>" ItemStyle-BackColor="Gainsboro"
                    Visible="true"></asp:BoundColumn>
                    
                    
                     <asp:TemplateColumn HeaderText="Cert.Concl.">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bCertConclusivo"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="linguaCertificado" HeaderText="Lingua" ItemStyle-BackColor="Gainsboro"></asp:BoundColumn>
                    
            </Columns>
            <PagerStyle Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
        <br />
        <div class="lblMessage">
            <%=Resources.Resource.FraseAvisoClique %></div>
        <br />
        <asp:Button class="button" ID="btnAprovarAll" runat="server" CssClass="button" Text="<%$ Resources:Resource, AprovarTodos %>">
        </asp:Button>&nbsp;
        <asp:Button class="button" ID="btnRejeitarAll" runat="server" CssClass="button" Text="<%$ Resources:Resource, RejeitarTodos %>">
        </asp:Button>&nbsp;
        <asp:Button class="button" ID="btnDeselectAll" runat="server" CssClass="button" Text="<%$ Resources:Resource, LimparTodos %>">
        </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Confirmar %>">
        </asp:Button>
        <br />
        <br />
        <asp:CheckBox ID="cbTodos" runat="server" AutoPostBack="True" Text="<%$ Resources:Resource, VerTodos %>" Font-Bold="True">
        </asp:CheckBox>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="Textbox1" runat="server" Width="15" BackColor="#999999" Enabled="False"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                    <%=Resources.Resource.FraseCalibUtilizadorLog %>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="CorLegendaDoUser" runat="server" Width="15" BackColor="#FFE5BF"
                        Enabled="False"></asp:TextBox>&nbsp;&nbsp;&nbsp; <%=Resources.Resource.FraseCalibOutroUser %>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="CorLegendaAnulado" runat="server" Width="15" BackColor="#FFA380"
                        Enabled="False"></asp:TextBox>&nbsp;&nbsp;&nbsp; <%=Resources.Resource.FraseAvisoDocFicheiroFalta %>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.UploadCertificado %>:
                    <asp:Label ID="ficheiro" runat="server"></asp:Label>
                   <%-- <input id="fileIn" type="file"
                        size="59" name="fileIn" runat="server" />
                    <asp:Button class="button" ID="btnUpload" runat="server" Text="<%$ Resources:Resource, CarregarFicheiro %>"
                        CausesValidation="false"></asp:Button>--%>

                      
        <input type="file" id="FileUpload1" multiple="multiple" runat="server"/>
        <asp:Button runat="server" ID="Button1" Text="UPLOAD FILES"
            onclick="btnupload_Click"/>&nbsp;<br/>
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:DataGrid ID="dgteste" AutoGenerateColumns="true" runat="server"></asp:DataGrid>
</asp:Content>
