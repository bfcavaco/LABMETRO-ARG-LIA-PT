<%@ Page Language="c#" CodeBehind="FormEmpresa.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormEmpresa"
    MasterPageFile="~/mp.Master" Culture="en-US" MaintainScrollPositionOnPostback="true"  %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
<!-- culture en-us POR CAUSA DO PONTO NA LATITUDE E LONGITUDE-->
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
        <legend><%=Resources.Resource.Empresa %></legend>
        <table>
            <tr>
                <td colspan="4">
                 <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
        ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
                    <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr bgcolor="#cccccc">
                <td>
                    <%=Resources.Resource.PesquisarGrupo %>:
                </td>
                <td colspan="3">
                    <%=Resources.Resource.Nome %>:&nbsp;&nbsp;<asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true"
                        OnTextChanged="txtPesquisaEmpresa_TextChanged"></asp:TextBox>
                    <%=Resources.Resource.NIF %>:&nbsp;&nbsp;<asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;&nbsp;
                    <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, Pesquisar %>"
                        CausesValidation="false" OnClick="btnPesquisa_Click"></asp:Button>
                </td>
            </tr>
            <tr bgcolor="#cccccc">
                <td>
                    <%=Resources.Resource.Grupo %>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresaPai" runat="server" DataTextField="nome" DataValueField="idEmpresa">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NomeEmpresa %>:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtNomeEmpresa" runat="server" MaxLength="100" Width="80%"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNomeEmpresa">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NomeAbreviado %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeAbrev" runat="server" MaxLength="50" Width="95%"></asp:TextBox><asp:RequiredFieldValidator
                        ID="Requiredfieldvalidator4" runat="server" ControlToValidate="txtNomeAbrev">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <%=Resources.Resource.TipoEmpresa %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddTipoEmpresa" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNIF" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Estado %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddEstado" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumCliente %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumClienteSAP" runat="server"></asp:TextBox>
                </td>
                <td>Obra:
                </td>
                <td><asp:TextBox runat="server" ID="txtNumObra" MaxLength="50" Width="120px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Sede %>:
                </td>
                <td>
                    <asp:RadioButtonList ID="rblSede" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"> </asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"> </asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" ControlToValidate="rblSede">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <%=Resources.Resource.Actividade %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddActividade" runat="server" DataTextField="descricao" DataValueField="idActividade">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddActividade">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Morada %>:
                </td>
                <td>
                    <asp:TextBox ID="txtMorada" runat="server" Width="90%"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.CodigoPostal %>:
                </td>
                <td>
                    <asp:TextBox ID="txtCodigoPostal" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td>
                    <%=Resources.Resource.LocalidadePostal %>:
                </td>
                <td>
                    <asp:TextBox ID="txtLocalidadePostal" runat="server" Width="90%"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Localidade %>:
                </td>
                <td>
                     <asp:TextBox ID="txtLocalidade" runat="server" Width="90%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="validaLocalidade" runat="server" ControlToValidate="txtLocalidade">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                
                <td>
                    <%=Resources.Resource.Pais %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddPais" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.Concelho %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddConcelho" runat="server" DataTextField="descricao" DataValueField="idConcelho">
                    </asp:DropDownList>
                     <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" ControlToValidate="ddConcelho">*</asp:RequiredFieldValidator>
                </td>
            </tr>
             <tr>
                
                <td>
                    <%=Resources.Resource.Latitude %>:
                </td>
                <td>
                    <asp:TextBox ID="txtLatitude" runat="server" Width="50%"></asp:TextBox>
                   <asp:CompareValidator
                            ID="Comparevalidator32" runat="server" ControlToValidate="txtLatitude" Operator="DataTypeCheck"
                            Type="Double">!</asp:CompareValidator>
                   
                </td>
                <td>
                    <%=Resources.Resource.Longitude %>:
                </td>
                <td>
                    <asp:TextBox ID="txtLongitude" runat="server" Width="50%"></asp:TextBox>
                     <asp:CompareValidator
                            ID="Comparevalidator1" runat="server" ControlToValidate="txtLongitude" Operator="DataTypeCheck"
                            Type="Double">!</asp:CompareValidator>
                     
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Telefone1 %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTelefone1" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Telefone2 %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTelefone2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.EmailGeral %>:
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Fax %>:
                </td>
                <td>
                    <asp:TextBox ID="txtFax" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.RegiaoVendas %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddRegiaoVendas" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.CondicoesPagamento %>:
                </td>
                <td >
                    <asp:DropDownList ID="ddCondPagamentoFactura" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator5" runat="server" ControlToValidate="ddCondPagamentoFactura">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.TemContrato %>:
                </td>
                <td>
                    <asp:RadioButtonList ID="rblContrato" runat="server" AutoPostBack="False" RepeatLayout="Flow"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <%=Resources.Resource.DescontoEmPercentagem %>
                </td>
                <td >
                    <asp:TextBox ID="txtDesconto" runat="server"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator3" runat="server" ControlToValidate="txtDesconto" Type="Double"
                        Operator="DataTypeCheck"> ! </asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td bgcolor="palegoldenrod">
                    <%=Resources.Resource.PamentosAtraso %>:
                </td>
                <td bgcolor="palegoldenrod">
                    <asp:RadioButtonList ID="rblPagamentoAtraso" runat="server" AutoPostBack="False"
                        RepeatLayout="Flow" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblPagamentoAtraso_SelectedIndexChanged">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td bgcolor="palegoldenrod">
                    <%=Resources.Resource.NivelBloqueio %>:
                </td>
                <td  bgcolor="palegoldenrod">
                    <asp:DropDownList ID="ddNivelBloqueio" runat="server">
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Livre %>"></asp:ListItem>
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Amarelo %>"></asp:ListItem>
                        <asp:ListItem Value="2" Text="<%$ Resources:Resource, Laranja %>"></asp:ListItem>
                        <asp:ListItem Value="3" Text="<%$ Resources:Resource, Vermelho %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td bgcolor="palegoldenrod">
                   <%=Resources.Resource.RequisicoesAtraso %>:
                </td>
                <td bgcolor="palegoldenrod">
                    <asp:RadioButtonList ID="rblRequisicaoAtraso" runat="server" AutoPostBack="False"
                        RepeatLayout="Flow" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblRequisicaoAtraso_SelectedIndexChanged">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td bgcolor="palegoldenrod">
                    <%=Resources.Resource.BloqSistemaCentral %>:
                </td>
                <td bgcolor="palegoldenrod">
                    <asp:DropDownList ID="ddCodigoBloqueioSap" runat="server" DataTextField="descCodigoBloqueio"
                        DataValueField="CodigoBloqueio" Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" bgcolor="#eee8aa">
                    <%=Resources.Resource.AcessoCertififcadosSemRequisicao%>:
                    <asp:CheckBox ID="cbVerCertifsSemReq" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td bgcolor="#eee8aa" colspan="4">
                    <%=Resources.Resource.FacturarSemRequisicao %>:
                    <asp:CheckBox ID="cbNPrecisaReqPFacturar" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged">
                    </asp:CheckBox>
                </td>
            </tr>
            <tr>
                  <td colspan="2" bgcolor="#eee8aa">
                    <%=Resources.Resource.GestaoEquipamentos%>:
                    <asp:CheckBox ID="cbGestaoEquipamentos" runat="server"></asp:CheckBox>
                </td>
                <td  bgcolor="#eee8aa">
                        <%=Resources.Resource.FuncionarioGestaoEquipamentos%>:
                    </td>
                    <td bgcolor="#eee8aa">
                        <asp:DropDownList ID="ddFuncionario" runat="server" DataValueField="idFuncionario" DataTextField="nomeAbreviado">
                        </asp:DropDownList>
                    </td>
            </tr>
            
            <tr>
                <td>
                    <%=Resources.Resource.Observacoes %>:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="100%" MaxLength="500"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td>
                    <%=Resources.Resource.CriteriosAceitacao %>:
                </td>
                <td colspan="3">
                    
                    <asp:TextBox ID="txtCriteriosAceitacao" runat="server" Width="100%" Height="150" TextMode="MultiLine" ></asp:TextBox>
                </td>
            </tr>
              <tr>
                    <td><label>
                        <%=Resources.Resource.NomeFicheiro %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomeFicheiro" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                       <label>Ficheiro:</label> 
                    </td>
                    <td><input id="fileIn" type="file" size="59" name="fileIn" runat="server" />
                    </td>
                </tr>
                <tr>
                 <td id="tdFicheiroCriterios" runat="server"  colspan="2">
                     <asp:HyperLink runat="server" id="linkFicheiro" Target="new">ver ficheiro
                        </asp:HyperLink>
                </td>
                <td align="right"> 
                
                <asp:Button class="button" ID="btnRemove" 
                        CssClass="button" runat="server" 
                        Text="<%$ Resources:Resource, RemoverFicheiro %>"  OnClick="btnRemove_Click">
                        </asp:Button></td>
                    <td align="right">
                        <asp:Button class="button" ID="btnUpload" CssClass="button" runat="server" CausesValidation="false"
                            Text="Carregar Ficheiro" OnClick="btnUpload_Click"></asp:Button>
                       
                    </td>
                </tr>
            <tr>
                <td>Website:</td>
                <td> <asp:TextBox ID="txtWebsite" runat="server" Width="200px"></asp:TextBox></td>
                <td>Credito Max.</td>
                <td> <asp:TextBox ID="txtCreditoMax" runat="server" Width="200px"></asp:TextBox></td>
                <asp:CompareValidator
                        ID="Comparevalidator2" runat="server" ControlToValidate="txtCreditoMax" Type="Double"
                        Operator="DataTypeCheck"> ! </asp:CompareValidator>
            </tr>
            <tr>
                <td>Modo de Pagamento:</td>
                <td> <asp:TextBox ID="txtmodoPagamento" runat="server" Width="200px"></asp:TextBox></td>
                <td>Modo de Entrega</td>
                <td> <asp:TextBox ID="txtModoEntrega" runat="server" Width="200px"></asp:TextBox></td>
            </tr>

             <tr>
                 <td>Necessita de documentação de entrada:</td>
                <td>   <asp:CheckBox ID="cbRequerDocEntrada" runat="server"></asp:CheckBox></td>
                <td>Documentação de Entrada:</td>
                <td> <asp:TextBox ID="txtDocumentacaoEntrada" runat="server" Width="200px" MaxLength="250"></asp:TextBox></td>
                
            </tr>
        
                  
            <tr>
                <td colspan="4" align="center">
                    <p>
                        <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Gravar %>" OnClick="btnSubmit_Click" />
                    </p>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend><%=Resources.Resource.HistoricoEstados %></legend>
        <asp:DataGrid 
        ID="dgHistoricoEstadosEmpresa" 
        runat="server" 
        AutoGenerateColumns="True"
        OnSelectedIndexChanged="dgHistoricoEstadosEmpresa_SelectedIndexChanged">
        </asp:DataGrid>
    </fieldset>
</asp:Content>
