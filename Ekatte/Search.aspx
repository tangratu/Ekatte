<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Ekatte.Search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="auto-style1">
            <asp:Button ID="Bt_import" runat="server" OnClick="Button1_Click" Text="Import data" />
            <br />
            <asp:TextBox ID="Tb_search" runat="server" Visible="False" Width="403px"></asp:TextBox>
            <br />
            <asp:Button ID="Bt_search" runat="server" Text="Search" Visible="False" OnClick="Bt_search_Click" />
            <asp:GridView ID="GV_ek" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center">
                <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" />
                <asp:BoundField DataField="name" HeaderText="Name" />
                <asp:BoundField DataField="ekatte" HeaderText="Ekatte" />
                <asp:BoundField DataField="cat" HeaderText="Category" />
                <asp:BoundField DataField="idkm" HeaderText="Township ID" />
                    </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:Label ID="Lb_obl" runat="server"></asp:Label>
&nbsp;&nbsp;
            <asp:Label ID="Lb_obsh" runat="server"></asp:Label>
&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Lb_km" runat="server"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Lb_ek" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
