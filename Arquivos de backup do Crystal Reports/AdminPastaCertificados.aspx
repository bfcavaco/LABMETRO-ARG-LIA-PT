<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPastaCertificados.aspx.cs"
    Inherits="LabMetro.AdminPastaCertificados" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Gestão de Certificados (Admin) - renomear e apagar</legend>
        <asp:Label ID="lblMessage" runat="server"></asp:Label><br />
<%--        <asp:FileUpload ID="FileUpload1" runat="server" /><br />
        <asp:Button ID="Button1" runat="server" Text="Carregar ficheiro" OnClick="Button1_Click" /><br />
        <br />--%>
        <asp:TextBox runat="server" id="txtSearch"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Pesquisar" />
        <asp:GridView ID="GridView1" runat="server" OnRowDeleting="GridView1_RowDeleting"
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
