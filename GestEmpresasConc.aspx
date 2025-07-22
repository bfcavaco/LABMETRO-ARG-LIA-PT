<%@ Page Language="c#" CodeBehind="GestEmpresasConc.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestEmpresasConc" MasterPageFile="~/mp.Master" %>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript">

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


    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisa").focus();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
        ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
    <fieldset>
        <legend>Gestão de Marcações CTA/AUT</legend>
        <fieldset>Pesquisar Empresas (Activas/Clientes)
        <table >
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    Empresa:&nbsp;<asp:TextBox ID="txtNomeEmpresa" runat="server" />
                    Nif:&nbsp;<asp:TextBox ID="txtNIF" runat="server"></asp:TextBox>
                    NºCliente:&nbsp;<asp:TextBox ID="txtNumClienteSAP" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    Concessionário:&nbsp;<asp:CheckBox ID="cbConcessionario" runat="server"></asp:CheckBox>
                    CTA:&nbsp;<asp:CheckBox ID="cbCTA" runat="server"></asp:CheckBox>
                    AGE:<asp:CheckBox ID="cbAGE" runat="server"></asp:CheckBox>
                    S.P.:&nbsp;<asp:CheckBox ID="cbSistemaPesagem" runat="server"></asp:CheckBox>
                    Centro B:<asp:CheckBox ID="cbCentroB" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    C/equips a<br />
                    calibrar entre:&nbsp;<asp:TextBox ID="txtDtInicioCalib" runat="server" Width="80"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator6" runat="server" ControlToValidate="txtDtInicioCalib" Type="Date"
                        Operator="DataTypeCheck">!</asp:CompareValidator>
                    &nbsp;e&nbsp;
                    <asp:TextBox ID="txtDtFimCalib" runat="server" Width="80"> </asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator1" runat="server" ControlToValidate="txtDtFimCalib" Type="Date"
                        Operator="DataTypeCheck">!</asp:CompareValidator>
                    C/&nbsp;visitas a marcar entre&nbsp;&nbsp;<asp:TextBox ID="txtDtInicioVisita" runat="server"
                        Width="80" ></asp:TextBox><asp:CompareValidator ID="Comparevalidator2" runat="server"
                            ControlToValidate="txtDtInicioVisita" Type="Date" Operator="DataTypeCheck">!</asp:CompareValidator>
                    &nbsp;e:&nbsp;
                    <asp:TextBox ID="txtDtFimVisita" runat="server" Width="80"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator3" runat="server" ControlToValidate="txtDtFimVisita" Type="Date"
                        Operator="DataTypeCheck">!</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Nº Série:&nbsp;<asp:TextBox ID="txtNumSerie" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                </td>
                
            </tr>
            <tr>
                <!--so tem 2 td's visto haver um rowspan em cima-->
                <td align="center" colspan="4">
                    <asp:Button class="button" ID="btnPesquisa" runat="Server" CausesValidation="False"
                        Text="Listar Empresas" CssClass="button"></asp:Button>&nbsp;&nbsp;&nbsp;
                    <asp:Button class="button" ID="btnReport" runat="server" Text="Imp. Lista empresas atraso"
                        CssClass="button" CausesValidation="False"></asp:Button>
                    <br />
                    Filtros para impressão: <asp:CheckBoxList ID="cbGrandeza" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                        <asp:ListItem Value="AGE">AGE</asp:ListItem>
                        <asp:ListItem Value="OPC">OPC</asp:ListItem>
                        <asp:ListItem Value="AUT">AUT</asp:ListItem>
                        <asp:ListItem Value="CTA">CTA</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblAlerta" runat="server" CssClass="lblMessage">Label</asp:Label><br />
        <!-- datagrid empresas -->
        <asp:DataGrid 
        ID="DGEmpresas" 
        runat="server" 
        DataKeyField="idEmpresa"
        OnSortCommand="SortGridEmpresas" 
        OnPageIndexChanged="DoPagingEmpresas" 
        AllowSorting="True"
        PageSize="5" 
        AllowPaging="true" 
        AutoGenerateColumns="false" 
        OnUpdateCommand="updateEmpresa"
        OnCancelCommand="cancelEmpresa" 
        OnEditCommand="editEmpresa" 
        OnItemDataBound="DGEmpresas_DataBound">
            <Columns>
                <asp:ButtonColumn CommandName="Select" Text=">>>" ItemStyle-Width="30"></asp:ButtonColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:TextBox ID="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.nivelBloqueioLabmetro")),Convert.ToString(DataBinder.Eval (Container, "DataItem.codigoBloqueioSAP")))%>'
                            Width="20" Height="20" runat="server">
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="Nome " ReadOnly="True"
                    ItemStyle-Width="300"></asp:BoundColumn>
                <asp:BoundColumn DataField="localidadeEmpresa" SortExpression="localidadeEmpresa"
                    HeaderText="Localidade" ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="morada" SortExpression="morada" HeaderText="morada" ReadOnly="True">
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Telefone" SortExpression="telefone" ItemStyle-Wrap="False">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.telefone") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditTelefone" runat="server" Width="75" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.telefone") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Fax" SortExpression="fax" ItemStyle-Wrap="False">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.fax") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditFax" runat="server" Width="75" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.fax") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Email" SortExpression="email" ItemStyle-Wrap="False">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.email") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditEmail" runat="server" Width="120" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.email") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Dt.visita." SortExpression="dtUltimaVisita">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.dtUltimaVisita","{0:dd/MM/yyyy}") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditDtUltimaVisita" runat="server" Width="80" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.dtUltimaVisita","{0:dd/MM/yyyy}") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="P." SortExpression="intTipoContrato">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.intTipoContrato") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditTipoContrato" runat="server" Width="50" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.intTipoContrato") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Con." SortExpression="bConcessionario">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbItemConcessionario" Checked='<%# DataBinder.Eval(Container, "DataItem.bConcessionario") %>'
                            runat="server" Enabled="false"></asp:CheckBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditConcessionario" Checked='<%# DataBinder.Eval(Container, "DataItem.bConcessionario") %>'
                            runat="server" Enabled="true"></asp:CheckBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CTA" SortExpression="bCTA">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbItemCTA" Checked='<%# DataBinder.Eval(Container, "DataItem.bCTA") %>'
                            runat="server" Enabled="false"></asp:CheckBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditCTA" Checked='<%# DataBinder.Eval(Container, "DataItem.bCTA") %>'
                            runat="server" Enabled="true"></asp:CheckBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="B" SortExpression="bCentroB">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbItemCentroB" Checked='<%# DataBinder.Eval(Container, "DataItem.bCentroB") %>'
                            runat="server" Enabled="false"></asp:CheckBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditCentroB" Checked='<%# DataBinder.Eval(Container, "DataItem.bCentroB") %>'
                            runat="server" Enabled="true"></asp:CheckBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="S.P." SortExpression="bSistemaPesagem">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbItemSistemaPesagem" Checked='<%# DataBinder.Eval(Container, "DataItem.bSistemaPesagem") %>'
                            runat="server" Enabled="false"></asp:CheckBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditSistemaPesagem" Checked='<%# DataBinder.Eval(Container, "DataItem.bSistemaPesagem") %>'
                            runat="server" Enabled="true"></asp:CheckBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="AGE" SortExpression="bAGE">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbItemAGE" Checked='<%# DataBinder.Eval(Container, "DataItem.bAGE") %>'
                            runat="server" Enabled="false"></asp:CheckBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditAGE" Checked='<%# DataBinder.Eval(Container, "DataItem.bAGE") %>'
                            runat="server" Enabled="true"></asp:CheckBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar">
                </asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
        </fieldset>
        <fieldset>
        <legend></legend>
        <!-- fim datagrid empresas -->
        <table cellspacing="0" cellpadding="0" align="left" border="0">
            <tr>
                <td style="width: 765px" width="765">
                    <asp:LinkButton ID="btnContactos" runat="server" CausesValidation="False">Contactos</asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="btnEquipamentos" runat="server" CausesValidation="False">Equips.(activos)</asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="btnMarcacoes" runat="server" CausesValidation="False">Marcações</asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="btnRequisicao" runat="server" CausesValidation="False">Requisições</asp:LinkButton>|
                </td>
            </tr>
        </table>
        <br />
        <br />
        &nbsp;Fazer Marcação:
        <table>
            <tr>
                <td>
                    Técnico:
                </td>
                <td>
                    <asp:DropDownList ID="ddTecnicoExterior" runat="server" DataTextField="nomeAbreviado"
                        DataValueField="idFuncionario">
                    </asp:DropDownList>
                </td>
                <td>
                    Local Calib.:
                </td>
                <td>
                    <asp:DropDownList ID="ddLocalCalibracao" runat="server" DataTextField="descricao"
                        DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Data da visita:
                </td>
                <td>
                    <asp:TextBox ID="txtDataProximaVisita" runat="server" MaxLength="10"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator7" runat="server" ControlToValidate="txtDataProximaVisita"
                        Type="Date" Operator="DataTypeCheck">dd-mm-aaaa</asp:CompareValidator><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDataProximaVisita"> ! </asp:RequiredFieldValidator>&nbsp;
                </td>
                <td>
                    Data últ.dia:
                </td>
                <td>
                    <asp:TextBox ID="txtDataUltDiaMarcacao" runat="server" MaxLength="10"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator4" runat="server" ControlToValidate="txtDataUltDiaMarcacao"
                        Type="Date" Operator="DataTypeCheck">dd-mm-aaaa</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Requisição:
                </td>
                <td>
                    <asp:DropDownList ID="ddRequisicao" runat="server" DataTextField="referenciaCliente"
                        DataValueField="idRequisicao">
                    </asp:DropDownList>
                </td>
                <td>
                    Observações(internas)
                </td>
                <td>
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Grandeza (p/fax):
                </td>
                <td>
                    <asp:DropDownList ID="ddGrandezaFax" runat="server" Font-Size="9px">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>AUT</asp:ListItem>
                        <asp:ListItem>CTA</asp:ListItem>
                        <asp:ListItem>AGE</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button class="button_confirm" ID="btnFazerMarcacao" runat="server" Text="Marcar!" />
                    
                </td>
                <td>
                </td>
            </tr>
        </table>
        </fieldset>
        
        <fieldset>
        <legend></legend>
            Filtrar(CONC):<asp:DropDownList ID="ddGrandezas" runat="server">
                <asp:ListItem Value="">Sem filtro</asp:ListItem>
                <asp:ListItem Value="CTA">CTA</asp:ListItem>
                <asp:ListItem Value="AUT">AUT</asp:ListItem>
                <asp:ListItem Value="AGE">AGE</asp:ListItem>
                <asp:ListItem Value="OPC">OPC</asp:ListItem>
            </asp:DropDownList>
            <br />
            Filtrar(CTA):&nbsp;&nbsp;<asp:DropDownList ID="ddFamilias" runat="server" DataTextField="descricao"
                DataValueField="idFamilia">
            </asp:DropDownList>
            <asp:Button class="button" ID="btnFiltrarEquips" runat="server" Text="Filtrar" CausesValidation="False"
                OnClick="btnFiltrarEquips_Click"></asp:Button><br />
        </p>
        <asp:Button class="button" ID="btnA" runat="server" Text="Todos A" CausesValidation="False">
        </asp:Button>
        <asp:Button class="button" ID="bntC" runat="server" Text="Todos C" CausesValidation="False">
        </asp:Button>
        <asp:Button class="button" ID="bntE" runat="server" Text="Todos E" CausesValidation="False">
        </asp:Button>
        <asp:Button class="button" ID="btnV" runat="server" Text="Todos V" CausesValidation="False"
            OnClick="btnV_Click"></asp:Button>
        <asp:DataGrid 
        ID="DGEquipamentos" 
        runat="server" 
        DataKeyField="idEquipamento" 
        OnSortCommand="SortGridEquipamentos"
        AllowSorting="True" 
        PageSize="15" 
        AllowPaging="false" 
        AutoGenerateColumns="false"
        OnUpdateCommand="updateEquipamento"
        OnCancelCommand="cancelGridEquipamento" 
        OnEditCommand="editEquipamento" 
        EditItemStyle-Width="100px"
        OnItemDataBound="DGEquipamentos_DataBound">
            <Columns>
                <asp:TemplateColumn HeaderText="A" ItemStyle-BackColor="#ff3333">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="checkbox" AutoPostBack="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="C" ItemStyle-BackColor="#99cc00">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="cbCalibracao" AutoPostBack="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="E" ItemStyle-BackColor="#ffcc00">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="cbEnsaio" AutoPostBack="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="V" ItemStyle-BackColor="#cccc99">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="cbVerificacao" AutoPostBack="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="idGrandeza" SortExpression="idGrandeza" HeaderText="GR."
                    ReadOnly="true"></asp:BoundColumn>
                <asp:BoundColumn DataField="familia" SortExpression="familia" HeaderText="Família"
                    ReadOnly="true" Visible="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Tipo" SortExpression="tipoEquipamento">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="NºID" SortExpression="numIdentificacao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="NºSérie" SortExpression="numSerie">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.numSerie") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.numSerie") %>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Per." SortExpression="periodicidadeCalibracao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.periodicidadeCalibracao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPC" runat="server" Width="50" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.periodicidadeCalibracao") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ref.últ.cal." SortExpression="refUltimaCalibracao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.refUltimaCalibracao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRUC" runat="server" Width="80" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.refUltimaCalibracao") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Dt.últ.Cal." SortExpression="dtUltimaCalibracao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.dtUltimaCalibracao","{0:dd/MM/yyyy}") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDUC" runat="server" Width="80" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.dtUltimaCalibracao","{0:dd/MM/yyyy}") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Obs" SortExpression="observacoes">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.observacoes") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtObs" runat="server" Width="80" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.observacoes") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar">
                </asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Label runat="server" ID="lblLegenda">
						Legenda: <br />A: serve para actualizar os equipamentos marcados com a data e periodicidade de calib. empresa (dt.útl.visita + tipo de contrato)<br />
						C + E: serve para incluir equipamentos marcados numa marcação, C para fazer uma 
							Calibração e E para fazer um Ensaio.<br />
							<br />
        </asp:Label>
        <!-- fim datagrid Equipamentos -->
        <!-- datagrid Contactos -->
        <asp:DataGrid 
        ID="dgContactos" 
        runat="server"
        DataKeyField="idContacto"
        OnSortCommand="SortGridContactos"
        OnPageIndexChanged="DoPagingContactos" 
        AllowSorting="True" 
        PageSize="25" 
        AllowPaging="true"
        AutoGenerateColumns="false"
        OnUpdateCommand="updateContacto"
        OnCancelCommand="cancelContacto" 
        OnEditCommand="editContacto" 
        Width="60%">
            <Columns>
                <asp:ButtonColumn CommandName="Select" Text=">>>" ItemStyle-Width="30"></asp:ButtonColumn>
                <asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="Nome " ReadOnly="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="cargo" SortExpression="cargo" HeaderText="cargo" ReadOnly="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="departamento" SortExpression="departamento" HeaderText="Departamento."
                    ReadOnly="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="telefoneEmpresa" SortExpression="telefoneEmpresa" HeaderText="telefone"
                    ReadOnly="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="faxEmpresa" SortExpression="faxEmpresa" HeaderText="Fax"
                    ReadOnly="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="emailEmpresa" SortExpression="emailEmpresa" HeaderText="Email"
                    ReadOnly="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Cont.Princ." SortExpression="contactoPrincipal">
                    <ItemTemplate>
                        <asp:CheckBox ID="Checkbox3" Checked='<%# DataBinder.Eval(Container, "DataItem.contactoPrincipal") %>'
                            runat="server" Enabled="false"></asp:CheckBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="Checkbox4" Checked='<%# DataBinder.Eval(Container, "DataItem.contactoPrincipal") %>'
                            runat="server" Enabled="true"></asp:CheckBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Activo" SortExpression="activo">
                    <ItemTemplate>
                        <asp:CheckBox ID="Checkbox5" Checked='<%# DataBinder.Eval(Container, "DataItem.activo") %>'
                            runat="server" Enabled="false"></asp:CheckBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="Checkbox6" Checked='<%# DataBinder.Eval(Container, "DataItem.activo") %>'
                            runat="server" Enabled="true"></asp:CheckBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="userId" SortExpression="userId" HeaderText="Id.Util.<br />LabmOnline"
                    ReadOnly="True"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <!-- fim datagrid contactos -->
        <asp:DataGrid 
        ID="dgGenerico" 
        runat="server" 
        AutoGenerateColumns="True">
        </asp:DataGrid>
        <!-- Datagrid Marcações -->
        <asp:DataGrid 
        ID="dgMarcacoes" 
        runat="server"
        AutoGenerateColumns="False">
            <Columns>
                <asp:BoundColumn DataField="dtMarcacao" SortExpression="dtMarcacao" HeaderText="Data"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="Empresa"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="funcionario" SortExpression="funcionario" HeaderText="Técnico"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="refmarcacao" SortExpression="refmarcacao" HeaderText="Ref.Marcação"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="refBRE" SortExpression="refBRE" HeaderText="Ref.BRE"
                    ReadOnly="True"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid><!-- Fim Datagrid Marcações -->
        <!-- Datagrid Requisicoes -->
        <asp:DataGrid 
        ID="dgRequisicoes" 
        runat="server" 
        DataKeyField="idRequisicao" 
        OnSortCommand="SortGrid"
        OnPageIndexChanged="DoPaging" 
        AllowSorting="True" 
        PageSize="10" 
        AllowPaging="true"
        AutoGenerateColumns="false">
            <Columns>
                <asp:BoundColumn DataField="referenciaCliente" SortExpression="referenciaCliente"
                    HeaderText="Ref.Req. Cliente"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Empresa<br />tem<br />contrato?" SortExpression="eContrato">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.eContrato"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="dtRequisicao" SortExpression="dtRequisicao" HeaderText="Data Req."
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtValidade" SortExpression="dtValidade" HeaderText="Data Val."
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="compl." SortExpression="completa">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.completa"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Renovável" SortExpression="bRenovavel">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bRenovavel"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ficheiro" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                            ID="Hyperlink1" Target="new">
											<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:HyperLinkColumn HeaderText="(Editar)<br />Requisição" DataNavigateUrlFormatString="FormRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_new" DataTextField="refRequisicao"
                    SortExpression="refRequisicao">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:HyperLinkColumn HeaderText="Ver<br />Serviços" DataNavigateUrlFormatString="ListaServicosRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_new" Text="ver">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:TemplateColumn HeaderText="Marcar<br />completa">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chbCompleta" AutoPostBack="True" Checked='<%#DataBinder.Eval(Container.DataItem, "completa") %>'
                            OnCheckedChanged="cb_SetComplete"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <!-- FIM dgrequisicoes -->
        <asp:Button class="button" ID="btnActualizarDadosEmpresaEquip" runat="server" Text="Actualizar Equips. Com data Ultima Visita Empresa" CausesValidation="False"></asp:Button><!--fim body -->
            </fieldset>
    </fieldset>
</asp:Content>
