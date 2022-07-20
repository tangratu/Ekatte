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
                <asp:BoundField DataField="name" HeaderText="Име" />
                <asp:BoundField DataField="ekatte" HeaderText="Екате" />
                <asp:BoundField DataField="kmetstvo" HeaderText="Кметство" />
                <asp:BoundField DataField="obshtina" HeaderText="Община" />
                <asp:BoundField DataField="oblast" HeaderText="Област" />
                    </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:Label ID="Lb_obl" runat="server">Области: </asp:Label>
&nbsp;&nbsp;
            <asp:Label ID="Lb_obsh" runat="server">Общини: </asp:Label>
&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Lb_km" runat="server">Кметства: </asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Lb_ek" runat="server">ЕКАТЕ: </asp:Label>
        </div>
    </form>
</body>
</html>
