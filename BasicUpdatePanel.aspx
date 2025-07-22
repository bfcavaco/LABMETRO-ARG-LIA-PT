<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server" language="C#" >
    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);
        string theTime = DateTime.Now.ToLongTimeString();
        for (int i = 0; i < 3; i++) {
            theTime += "<br />" + theTime;
        }
        time1.Text = theTime;
        time2.Text = theTime;
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Basic Update Panel</title>
</head>
<body>
<form id="form1" runat="server">
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Label runat="server" ID="time1"></asp:Label><br /><br />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate>
       <div style="border-style:solid;background-color:gray;">
         <asp:Label runat="server" ID="time2"></asp:Label><br />
         <asp:Button ID="Button1" runat="server" Text="Inside Button" />
         </div><br />
   </ContentTemplate>
</asp:UpdatePanel>
    <asp:Button ID="Button2" runat="server" Text="Outside Button" />
</div>
</form>
</body>
</html>
