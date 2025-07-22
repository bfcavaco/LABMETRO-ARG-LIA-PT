<%@ Page Language="c#" CodeBehind="GestMarcacoes.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestMarcacoes" MasterPageFile="~/mp.Master" %>
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
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Gerir Marcações CTA/AUT</legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0033"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Empresa:
                </td>
                <td>
                    <asp:TextBox ID="txtEmpresa" runat="server"></asp:TextBox>
                </td>
                <td>
                    RefMarcacao:
                </td>
                <td>
                    <asp:TextBox ID="txtRefMarcacao" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Técnico:
                </td>
                <td>
                    <asp:DropDownList ID="ddTecnicoExterior" runat="server" DataTextField="nomeAbreviado"
                        DataValueField="idFuncionario" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    Sem fax:
                </td>
                <td>
                    <asp:CheckBox ID="cbSemFax" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    De:
                </td>
                <td>
                    <asp:TextBox ID="txtDataInicio" runat="server" CssClass="date-input"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator6" runat="server" ControlToValidate="txtDataInicio" Type="Date"
                        Operator="DataTypeCheck">formato errado</asp:CompareValidator>
                </td>
                <td>
                    A:
                </td>
                <td>
                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="date-input"></asp:TextBox><asp:CompareValidator ID="Comparevalidator1"
                        runat="server" ControlToValidate="txtDataFim" Type="Date" Operator="DataTypeCheck">formato errado</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Ver linhas únicas:<asp:CheckBox ID="cbLinhasUnicas" runat="server" Checked="True">
                    </asp:CheckBox>
                </td>
                <td colspan="2">
                    <asp:Button class="button" ID="btnPesquisa" runat="Server" CausesValidation="true"
                        Text="Ver" CssClass="button" Width="100"></asp:Button>
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
                <asp:TemplateColumn HeaderText="Dia(s)" SortExpression="Weekday">
                    <ItemTemplate>
                        <%# diaSemana(DataBinder.Eval(Container, "DataItem.Weekday")) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="Data" DataField="date" SortExpression="date" DataFormatString="{0:dd/MM/yyyy}"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="empresa"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Dt.Marcação" SortExpression="dtMarcacao">
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
                <asp:TemplateColumn HeaderText="Dt.Fim." SortExpression="dtFimMarcacao" ItemStyle-Wrap="False">
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
                <asp:TemplateColumn HeaderText="Técnico" SortExpression="funcionario">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.funcionario") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddFuncionarioEdit" DataValueField="idFuncionario" DataTextField="nome"
                            runat="server">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="refMarcacao" SortExpression="idMarcacao" HeaderText="NºMarc."
                    ReadOnly="True"></asp:BoundColumn>
                <asp:HyperLinkColumn HeaderText="BRE" DataNavigateUrlField="idBRE" Target="_self"
                    DataTextField="BRE" SortExpression="idBRE"></asp:HyperLinkColumn>
                <asp:HyperLinkColumn HeaderText="Requ." DataNavigateUrlFormatString="FormRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_new" Text="ver" SortExpression="refRequisicao">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:TemplateColumn HeaderText="Req.Pdf" SortExpression="nomeFicheiro">
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
                <asp:TemplateColumn HeaderText="Camiao" SortExpression="bCamiao" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bCamiao"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="BRE<br />def." SortExpression="bdefinitivo" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bdefinitivo"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Grand." SortExpression="idGrandeza">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.idGrandeza".ToUpper().TrimStart().TrimEnd()) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddGrandezaEdit" runat="server">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="AUT">AUT</asp:ListItem>
                            <asp:ListItem Value="CTA">CTA</asp:ListItem>
                            <asp:ListItem Value="AGE">AGE</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Enviado" SortExpression="bFaxEnviado" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bFaxEnviado"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="Data<br />Envio" DataField="dtUltimoFax" SortExpression="dtUltimoFax"
                    DataFormatString="{0:dd/MM/yyyy}" ReadOnly="True" ItemStyle-Wrap="False"></asp:BoundColumn>
            <asp:BoundColumn HeaderText="Tipo" DataField="tipo" SortExpression="tipo" ReadOnly="True" ItemStyle-Wrap="False"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Observacoes" SortExpression="obsInternas">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtObsInternasEdit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "obsInternas")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "obsInternas")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:ButtonColumn CommandName="verDadosEmpresaManutencao" HeaderText="Emp.<br />Mant."
                    Text="ver" ItemStyle-Font-Size="8"></asp:ButtonColumn>
                <asp:ButtonColumn CommandName="verFax" HeaderText="Fax" Text="ver" ItemStyle-Font-Size="8"
                    ItemStyle-HorizontalAlign="Center"></asp:ButtonColumn>
                <asp:ButtonColumn CommandName="enviarFax" HeaderText="Fax" Text="enviar" ItemStyle-Font-Size="8"
                    ItemStyle-BackColor="#99cc33" ItemStyle-HorizontalAlign="Center"></asp:ButtonColumn>
                <asp:ButtonColumn CommandName="enviarMail" HeaderText="Mail" Text="enviar" ItemStyle-Font-Size="8"
                    ItemStyle-BackColor="#99cc33" ItemStyle-HorizontalAlign="Center"></asp:ButtonColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="DarkGray">
                </asp:EditCommandColumn>
                <asp:ButtonColumn CommandName="Delete" Text="Apagar" ItemStyle-BackColor="#ff3333"
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
