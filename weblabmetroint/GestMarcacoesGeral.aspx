<%@ Page Language="c#" CodeBehind="GestMarcacoesGeral.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestMarcacoesGeral" MasterPageFile="~/mp.Master" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
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

</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisa").focus();
            }
        }
        
        function confirm_delete()
		{
			if (confirm("Confirma eliminação da marcação?")==true)
				return true;
			else
				return false;
		}
		
		function confirm_fax()
		{
			if (confirm("Confirma envio de fax?")==true)
				return true;
			else
				return false;
		}
		
		function confirm_mail()
		{
			if (confirm("Confirma envio de mail?")==true)
				return true;
			else
				return false;
		}
		
		function CheckKey() 
		{
			if (event.keyCode == 13) 
			{
				document.getElementById("btnPesquisa").focus();
			}
		}


    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoMarcacoesGerais %></legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0033"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtEmpresa" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.RefMarcacao %>
                </td>
                <td>
                    <asp:TextBox ID="txtRefMarcacao" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Tecnico %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddTecnicoExterior" runat="server" DataTextField="nomeAbreviado"
                        DataValueField="idFuncionario" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.SemFax %>
                </td>
                <td>
                    <asp:CheckBox ID="cbSemFax" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.De %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDataInicio" runat="server" CssClass="date-input"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator6" runat="server" ControlToValidate="txtDataInicio" Type="Date"
                        Operator="DataTypeCheck"><%=Resources.Resource.FormatoErrado %></asp:CompareValidator>
                </td>
                <td>
                    <%=Resources.Resource.A %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="date-input"></asp:TextBox><asp:CompareValidator ID="Comparevalidator1"
                        runat="server" ControlToValidate="txtDataFim" Type="Date" Operator="DataTypeCheck"><%=Resources.Resource.FormatoErrado %></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <%=Resources.Resource.VerLinhasUnicas %>:<asp:CheckBox ID="cbLinhasUnicas" runat="server" Checked="True">
                    </asp:CheckBox>
                </td>
                <td colspan="2">
                    <asp:Button class="button" ID="btnPesquisa" runat="Server" CausesValidation="true"
                        Text="<%$ Resources:Resource, Ver %>" CssClass="button" Width="100"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <asp:DataGrid 
        ID="DataGrid1" 
        OnSortCommand="SortGrid" 
        AllowSorting="True" 
        AllowPaging="true"
        AutoGenerateColumns="false" 
        runat="server" 
        OnItemDataBound="dg_databound"
        DataKeyField="idMarcacao"
        OnEditCommand="editGrid" 
        OnUpdateCommand="updateGrid" 
        OnCancelCommand="cancelGrid"
        OnPageIndexChanged="doPaging" 
        PageSize="20"
        OnDeleteCommand="dg_DeleteCommand">
            <Columns>
               
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Dias %>" SortExpression="Weekday">
                    <ItemTemplate>
                        <%# diaSemana(DataBinder.Eval(Container, "DataItem.Weekday")) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
               
                
                 <asp:BoundColumn HeaderText="<%$ Resources:Resource, Data %>" DataField="date" SortExpression="date" DataFormatString="{0:dd/MM/yyyy}"
                    ReadOnly="True"></asp:BoundColumn>
                  
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="Cliente"
                    ReadOnly="True"></asp:BoundColumn>
                
                
                <asp:TemplateColumn HeaderText="Morada" SortExpression="moradaMarcacao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.moradaMarcacao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMoradaMarcacaoEdit" Text='<%# DataBinder.Eval(Container, "DataItem.moradaMarcacao") %>' runat="server" Width="75" />
                       
                    </EditItemTemplate>
                </asp:TemplateColumn>
                
                
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, DataMarcacao %>" SortExpression="dtMarcacao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.dtMarcacao","{0:dd/MM/yyyy}") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtStartEdit" Text='<%# DataBinder.Eval(Container, "DataItem.dtMarcacao","{0:dd/MM/yyyy}") %>'
                            runat="server" Width="75" />
                        <asp:CompareValidator ID="Comparevalidator2" runat="server" ControlToValidate="txtStartEdit"
                            Type="Date" Operator="DataTypeCheck">*</asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                
                
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, DataFim %>" SortExpression="dtFimMarcacao" ItemStyle-Wrap="False">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.dtFimMarcacao","{0:dd/MM/yyyy}") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEndEdit" Text='<%# DataBinder.Eval(Container, "DataItem.dtFimMarcacao","{0:dd/MM/yyyy}") %>'
                            runat="server" Width="75" />
                        <asp:CompareValidator ID="Comparevalidator3" runat="server" ControlToValidate="txtEndEdit"
                            Type="Date" Operator="DataTypeCheck">*</asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                
                
                <asp:TemplateColumn HeaderText="Periodo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.periodoMarcacao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPeriodoMarcacaoEdit" Text='<%# DataBinder.Eval(Container, "DataItem.periodoMarcacao") %>'
                            runat="server" Width="75" />
       
                    </EditItemTemplate>
                </asp:TemplateColumn>
       
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Tecnico %>" SortExpression="funcionario">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.funcionario") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddFuncionarioEdit" DataValueField="idFuncionario" DataTextField="nome"
                            runat="server">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                
                
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Tecnico %>" SortExpression="funcionario2">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.funcionario2") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddFuncionario2Edit" DataValueField="idFuncionario" DataTextField="nome"
                            runat="server">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                
                
                <asp:TemplateColumn HeaderText="DocEntr?" SortExpression="bRequerDocEntrada" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bRequerDocEntrada")))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
              
                <asp:BoundColumn DataField="documentacaoEntrada" SortExpression="documentacaoEntrada" HeaderText="Docum."
                    ReadOnly="True"></asp:BoundColumn>
                
                
                <asp:BoundColumn DataField="refMarcacao" SortExpression="idMarcacao" HeaderText="<%$ Resources:Resource, NumMarcacao %>"
                    ReadOnly="True"></asp:BoundColumn>
                
                
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, BRE %>" DataNavigateUrlField="idBRE" Target="_self"
                    DataTextField="BRE" SortExpression="idBRE"></asp:HyperLinkColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Req %>" DataNavigateUrlFormatString="FormRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_new" Text="<%$ Resources:Resource, Ver %>" SortExpression="refRequisicao">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, ReqPDF %>" SortExpression="nomeFicheiro">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddRequisicaoEdit" runat="server" DataValueField="idRequisicao"
                            DataTextField="referenciaCliente">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                            ID="Hyperlink1" Target="new">
											<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Orcamento %>" SortExpression="nomeFicheiro">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddOrcamentoEdit" runat="server" DataValueField="idOrcamento"
                            DataTextField="refOrcamento">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Button class="button" runat="server" CommandName="verOrcamentos" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "idOrcamento")%>'
                            Text="<%$ Resources:Resource, Ver %>" Enabled='<%# ConverteEstadoBotao(DataBinder.Eval(Container, "DataItem.idOrcamento")) %>'>
                        </asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Obs.Internas" SortExpression="obsInternas">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtObsInternasEdit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "obsInternas")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "obsInternas")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Obs.Cliente" SortExpression="obsCliente">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtObsClienteEdit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "obsCliente")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "obsCliente")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, BREDef %>" SortExpression="bdefinitivo" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bdefinitivo"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Enviar %>" SortExpression="bFaxEnviado" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bFaxEnviado"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="<%$ Resources:Resource, DataFax %>" DataField="dtUltimoFax" SortExpression="dtUltimoFax"
                    DataFormatString="{0:dd/MM/yyyy}" ReadOnly="True" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="<%$ Resources:Resource, Contacto %>" DataField="contacto" SortExpression="contacto"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="<%$ Resources:Resource, Tipo %>" DataField="Tipo" SortExpression="tipo" ReadOnly="True"
                    ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:ButtonColumn CommandName="enviarFax" Text="<%$ Resources:Resource, EnviarFax %>" ItemStyle-Font-Size="8"
                    ItemStyle-BackColor="#99cc33" ItemStyle-HorizontalAlign="Center"></asp:ButtonColumn>
                <asp:ButtonColumn CommandName="enviarMail" Text="<%$ Resources:Resource, EnviarMail %>" ItemStyle-Font-Size="8"
                    ItemStyle-BackColor="#99cc33" ItemStyle-HorizontalAlign="Center"></asp:ButtonColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="DarkGray">
                </asp:EditCommandColumn>
                <asp:ButtonColumn CommandName="Delete" Text="<%$ Resources:Resource, Apagar %>" ItemStyle-BackColor="#ff3333"
                    ItemStyle-ForeColor="#FFFFFF"></asp:ButtonColumn>
                <asp:BoundColumn HeaderText="" DataField="idGrandeza" SortExpression="" ReadOnly="True"
                    Visible="False"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <br />
        <asp:DataGrid ID="dgEmpresasManutencao" runat="server" AutoGenerateColumns="True">
        </asp:DataGrid>
    </fieldset>
</asp:Content>
