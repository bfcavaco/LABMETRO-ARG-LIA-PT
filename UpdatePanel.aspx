<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdatePanel.aspx.cs" Inherits="LabMetro.UpdatePanel" MasterPageFile="~/mp.Master" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <div>
        <asp:TextBox ID="textOut" runat="server"></asp:TextBox>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="border-style: solid; background-color: gray;">
                    <asp:TextBox ID="txtIn" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Inside Button" onclick="Button1_Click" />
                </div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="Button2" runat="server" Text="Outside Button" onclick="Button2_Click" />
    </div>
  
</asp:Content>
