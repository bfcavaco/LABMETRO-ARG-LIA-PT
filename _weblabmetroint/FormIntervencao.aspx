<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormIntervencao.aspx.cs"
    Inherits="LabMetro.FormIntervencao" MasterPageFile="~/mp.Master" %>

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
            width: 300px;
            
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
        <legend>Intervencao</legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label><br />
        <asp:LinkButton ID="link" runat="server" CausesValidation="false" Font-Bold="true"
            OnClick="linkInsert_Click">Inserir Nova Intervenção</asp:LinkButton>&nbsp;<br />
        <br />
        <table width="90%">
            <tr>
                <td>
                    <asp:ObjectDataSource ID="OBJDS_ListaIntervencoesByIdEquipamento" 
                        runat="server" 
                        SelectMethod="BLLGetIntervencoesByIdEquipamento" 
                        TypeName="LabMetro.BusinessLogicLayer.IntervencaoBLL" 
                        OldValuesParameterFormatString="original_{0}">
                       
                        <SelectParameters>
                            <asp:QueryStringParameter Name="idEquipamento" QueryStringField="id" 
                                Type="Int32" />
                        </SelectParameters>
                      
                      
                    </asp:ObjectDataSource>
                    <asp:GridView ID="gvIntervencoes" runat="server" DataSourceID="OBJDS_ListaIntervencoesByIdEquipamento"
                        AutoGenerateColumns="False" DataKeyNames="idIntervencao,idEquipamento"
                        PagerStyle-BackColor="White" PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Right"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="0.9em">
                        <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                        <Columns>
                            <asp:ButtonField CommandName="Select" Text="<%$ Resources:Resource, Ver %>" />
                        
                            <asp:BoundField DataField="intervencao" HeaderText="Intervenção" SortExpression="intervencao" />
                            <asp:BoundField DataField="dataIntervencao" HeaderText="Data" 
                                SortExpression="dataIntervencao"  DataFormatString="{0:dd/MM/yyyy}" 
                                ItemStyle-Wrap="false" >
<ItemStyle Wrap="False"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="entidadeIntervencao" HeaderText="Entidade"
                                SortExpression="entidadeIntervencao" />
                            <asp:BoundField DataField="numDocumento" HeaderText="Documento" SortExpression="numDocumento" />
                            <asp:BoundField DataField="conclusao" HeaderText="Conclusão" SortExpression="conclusao" />
                            <asp:BoundField DataField="observacoes" HeaderText="Observações" SortExpression="observacoes" />
                            <asp:BoundField DataField="aprovadoPor" HeaderText="Aprovado por" SortExpression="aprovadoPor" />
                            <asp:BoundField DataField="dtAprovacao" HeaderText="Dt. aprov." 
                                SortExpression="dtAprovacao"  DataFormatString="{0:dd/MM/yyyy}"  
                                ItemStyle-Wrap="false" >
<ItemStyle Wrap="False"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="dtCriacaoCliente" HeaderText="dtCriacaoCliente" SortExpression="dtCriacaoCliente"  DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="dtAlteracaoCliente" HeaderText="dtAlteracaoCliente" SortExpression="dtAlteracaoCliente"  DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="idUtilCriacaoCliente" HeaderText="idUtilCriacaoCliente"
                                SortExpression="idUtilCriacaoCliente" />
                            <asp:BoundField DataField="idUtilAlteracaoCliente" HeaderText="idUtilAlteracaoCliente"
                                SortExpression="idUtilAlteracaoCliente" />
                        </Columns>

<PagerStyle HorizontalAlign="Right" BackColor="White" Font-Bold="True" Font-Size="0.9em"></PagerStyle>
                    </asp:GridView>
                    <asp:FormView ID="fvIntervencao" runat="server" 
                        DataSourceID="OBJDS_fvIntervencao"  CssClass="fvForm" AllowPaging="True" OnPageIndexChanging="fv_PageIndexChanging"
            OnItemInserted="fv_Inserted" OnItemInserting="fv_Inserting" OnItemUpdated="fv_Updated"
            OnItemUpdating="fv_Updating" OnItemCommand="fv_ItemCommand" PagerSettings-Mode="NumericFirstLast"
            OnDataBound="fv_DataBound">
            <PagerSettings Mode="NumericFirstLast"></PagerSettings>

                        <EditItemTemplate>
                            <label>Intervenção:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="intervencaoTextBox" runat="server" Text='<%# Bind("intervencao") %>' />
                            <asp:RequiredFieldValidator ID="reqvalIntervencao" Display="Static" ErrorMessage="*"
                                ControlToValidate="intervencaoTextBox" runat="server" />
                            <br />
                            <label>Dt. Intervenção:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="dataIntervencaoTextBox" runat="server" Text='<%# Bind("dataIntervencao","{0:dd/MM/yyyy}") %>' />
                            <asp:RequiredFieldValidator ID="reqvaldtIntervencao" Display="Static" ErrorMessage="*"
                                ControlToValidate="dataIntervencaoTextBox" runat="server" />
                            <asp:CompareValidator ID="compvaldtIntervencao" runat="server" ControlToValidate="dataIntervencaoTextBox"
                                Operator="DataTypeCheck" Type="Date" Text="data!" />
                            <br />
                            <label>Entidade Intervenção:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="entidadeIntervencaoTextBox" runat="server" Text='<%# Bind("entidadeIntervencao") %>'
                                MaxLength="150" />
                            <asp:RequiredFieldValidator ID="reqvalEntidadeIntervencao" Display="Static" ErrorMessage="*"
                                ControlToValidate="entidadeIntervencaoTextBox" runat="server" />
                            <br />
                            <label>Núm./Ref. Documento:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="numDocumentoTextBox" runat="server" Text='<%# Bind("numDocumento") %>'
                                MaxLength="50" />
                            <br />
                            <label>Conclusão:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="conclusaoTextBox" runat="server" Text='<%# Bind("conclusao") %>'
                                MaxLength="250" Width="400" />
                            <br />
                            <label>Observações:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="observacoesTextBox" runat="server" Text='<%# Bind("observacoes") %>'
                                MaxLength="250" Width="400" />
                            <br />
                            <label>Aprovado por:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="aprovadoPorTextBox" runat="server" Text='<%# Bind("aprovadoPor") %>'
                                MaxLength="150" />
                            <br />
                            <label>Aprovado em:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="dtAprovacaoTextBox" runat="server" Text='<%# Bind("dtAprovacao","{0:dd/MM/yyyy}") %>' />
                            <asp:CompareValidator ID="comvaldtAprovacao" runat="server" ControlToValidate="dtAprovacaoTextBox"
                                Operator="DataTypeCheck" Type="Date" Text="data!" />
                            <br />
                            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                Text="Update" />
                            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False"
                                CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <label>Intervenção:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="intervencaoTextBox" runat="server" Text='<%# Bind("intervencao") %>' />
                            <asp:RequiredFieldValidator ID="reqvalIntervencao" Display="Static" ErrorMessage="*"
                                ControlToValidate="intervencaoTextBox" runat="server" />
                            <br />
                            <label>Dt. Intervenção:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="dataIntervencaoTextBox" runat="server" Text='<%# Bind("dataIntervencao") %>' />
                            <asp:RequiredFieldValidator ID="reqvaldtIntervencao" Display="Static" ErrorMessage="*"
                                ControlToValidate="dataIntervencaoTextBox" runat="server" />
                            <asp:CompareValidator ID="compvaldtIntervencao" runat="server" ControlToValidate="dataIntervencaoTextBox"
                                Operator="DataTypeCheck" Type="Date" Text="data!" />
                            <br />
                            <label>Entidade Intervenção:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="entidadeIntervencaoTextBox" runat="server" Text='<%# Bind("entidadeIntervencao") %>'
                                MaxLength="150" />
                            <asp:RequiredFieldValidator ID="reqvalEntidadeIntervencao" Display="Static" ErrorMessage="*"
                                ControlToValidate="entidadeIntervencaoTextBox" runat="server" />
                            <br />
                            <label>Núm./Ref. Documento:</label>
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="numDocumentoTextBox" runat="server" Text='<%# Bind("numDocumento") %>'
                                MaxLength="50" />
                            <br />
                            <label>Conclusão:</label>
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="conclusaoTextBox" runat="server" Text='<%# Bind("conclusao") %>'
                                MaxLength="250" Width="400" />
                            <br />
                            <label>Observações:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="observacoesTextBox" runat="server" Text='<%# Bind("observacoes") %>'
                                MaxLength="250"  Width="400" />
                            <br />
                            <label>Aprovado por:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="aprovadoPorTextBox" runat="server" Text='<%# Bind("aprovadoPor") %>'
                                MaxLength="150" />
                            <br />
                            <label>Aprovado em:</label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="dtAprovacaoTextBox" runat="server" Text='<%# Bind("dtAprovacao","{0:dd/MM/yyyy}") %>' />
                            <asp:CompareValidator ID="comvaldtAprovacao" runat="server" ControlToValidate="dtAprovacaoTextBox"
                                Operator="DataTypeCheck" Type="Date" Text="data!" />
                            <br />
                            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                Text="Insert" />
                            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False"
                                CommandName="Cancel" Text="Cancel" />
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <label></label>
                            Intervenção:
                            <asp:Label ID="intervencaoLabel" runat="server" Text='<%# Bind("intervencao") %>' />
                            <br />
                            <label></label>
                            Dt. Intervenção:
                            <asp:Label ID="dataIntervencaoLabel" runat="server" Text='<%# Bind("dataIntervencao") %>' />
                            <br />
                            <label></label>
                            Entidade Intervenção:
                            <asp:Label ID="entidadeIntervencaoLabel" runat="server" Text='<%# Bind("entidadeIntervencao") %>' />
                            <br />
                            <label></label>
                            Núm./Ref. Documento:
                            <asp:Label ID="numDocumentoLabel" runat="server" Text='<%# Bind("numDocumento") %>' />
                            <br />
                            <label></label>
                            Conclusão:
                            <asp:Label ID="conclusaoLabel" runat="server" Text='<%# Bind("conclusao") %>' />
                            <br />
                            <label></label>
                            Observações:
                            <asp:Label ID="observacoesLabel" runat="server" Text='<%# Bind("observacoes") %>' />
                            <br />
                            <label></label>
                            Aprovado por:
                            <asp:Label ID="aprovadoPorLabel" runat="server" Text='<%# Bind("aprovadoPor") %>' />
                            <br />
                            <label></label>
                            Dt. aprovação:
                            <asp:Label ID="dtAprovacaoLabel" runat="server" Text='<%# Bind("dtAprovacao") %>' />
                            <br />
                            <label></label>
                            dtCriacaoCliente:
                            <asp:Label ID="dtCriacaoClienteLabel" runat="server" Text='<%# Bind("dtCriacaoCliente") %>' />
                            <br />
                            dtAlteracaoCliente:
                            <asp:Label ID="dtAlteracaoClienteLabel" runat="server" Text='<%# Bind("dtAlteracaoCliente") %>' />
                            <br />
                            idUtilCriacaoCliente:
                            <asp:Label ID="idUtilCriacaoClienteLabel" runat="server" Text='<%# Bind("idUtilCriacaoCliente") %>' />
                            <br />
                            idUtilAlteracaoCliente:
                            <asp:Label ID="idUtilAlteracaoClienteLabel" runat="server" Text='<%# Bind("idUtilAlteracaoCliente") %>' />
                            <br />
                            <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit"
                                Text="Edit" />
                            &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New"
                                Text="New" />
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:ObjectDataSource ID="OBJDS_fvIntervencao" runat="server" InsertMethod="BLLInsertIntervencao"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="BLLGetIntervencaoById"
                        TypeName="LabMetro.BusinessLogicLayer.IntervencaoBLL" UpdateMethod="BLLUpdateIntervencao"
                         OnInserted="OBJDS_Inserted" OnUpdated="OBJDS_Updated">
                        <UpdateParameters>
                            <asp:Parameter Name="intervencao" Type="String" />
                            <asp:Parameter Name="dataIntervencao" Type="DateTime" />
                            <asp:Parameter Name="entidadeIntervencao" Type="String" />
                            <asp:Parameter Name="numDocumento" Type="String" />
                            <asp:Parameter Name="conclusao" Type="String" />
                            <asp:Parameter Name="observacoes" Type="String" />
                            <asp:Parameter Name="aprovadoPor" Type="String" />
                            <asp:Parameter Name="dtAprovacao" Type="DateTime" />
                            <asp:ControlParameter ControlID="gvIntervencoes" Name="idIntervencao" PropertyName="SelectedDataKey.Values[idIntervencao]"
                                Type="Int32" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="gvIntervencoes" Name="idIntervencao" PropertyName="SelectedDataKey.Values[idIntervencao]"
                                Type="Int32" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:QueryStringParameter Name="idEquipamento" QueryStringField="id" Type="Int32" />
                            <asp:Parameter Name="intervencao" Type="String" />
                            <asp:Parameter Name="dataIntervencao" Type="DateTime" />
                            <asp:Parameter Name="entidadeIntervencao" Type="String" />
                            <asp:Parameter Name="numDocumento" Type="String" />
                            <asp:Parameter Name="conclusao" Type="String" />
                            <asp:Parameter Name="observacoes" Type="String" />
                            <asp:Parameter Name="aprovadoPor" Type="String" />
                            <asp:Parameter Name="dtAprovacao" Type="DateTime" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
