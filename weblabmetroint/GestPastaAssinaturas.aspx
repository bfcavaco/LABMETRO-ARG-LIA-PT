<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestPastaAssinaturas.aspx.cs"
    Inherits="LabMetro.GestPastaAssinaturas" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Gestão de Assinaturas</legend>
        <asp:Label ID="labelStatus" runat="server"></asp:Label><br />
        <asp:FileUpload ID="FileUpload1" runat="server" /><br />
        <asp:Button ID="Button1" runat="server" Text="Carregar ficheiro" OnClick="Button1_Click" /><br />
        <br />
        <asp:GridView ID="GridView1" runat="server" DataSource="<%# GetUploadList() %>" OnRowDeleting="GridView1_RowDeleting"
            AutoGenerateColumns="False" Width="400">
            <Columns>
                <asp:TemplateField HeaderText="Ficheiros">
                    <ItemStyle HorizontalAlign="Center" Width="70%" Font-Bold="true" Font-Size="12" />
                    <ItemTemplate>
                        <asp:HyperLink ID="FileLink" NavigateUrl='<%# "uploads/certificados/assinaturas/" + Container.DataItem.ToString() %>'
                            Text='<%# Container.DataItem.ToString() %>' runat="server" Target="_blank" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Apagar?">
                    <ItemStyle HorizontalAlign="Center" Width="30%" Font-Bold="true" Font-Size="12"  />
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                            Text="Apagar?" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table cellpadding="5" cellspacing="1">
            <tr>
                <td width="100" align="right">
                    Alterar nome de ficheiro:
                </td>
                <td align="center" bgcolor="#FFFFFF">
                    Nome actual:<asp:TextBox ID="txtOldFile" runat="server"></asp:TextBox>
                    <br />
                    Novo nome::<asp:TextBox ID="txtNewFile" runat="server" Width="126px"></asp:TextBox><br />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submeter" OnClick="btnSubmit_Click" /><br />
                    &nbsp;
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
