<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication4.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


        
            <asp:GridView ID="gvHardwareInfo" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False" BackColor="#E9E9E9" 
            BorderStyle="None" CellPadding="0" CellSpacing="0" ForeColor="Black" 
             PageSize="100" 
            BorderWidth="0px" PagerSettings-PageButtonCount="25"  onrowcommand="data_RowCommand" >
                <Columns>
                    <asp:BoundField DataField="HARDWARE_Id" HeaderText="HARDWARE_Id" ReadOnly="True" InsertVisible="False" Visible="False" >
                    <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                        <asp:BoundField DataField="HARDWARE_Seriale" HeaderText="HARDWARE_Seriale" >
                            <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                            <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HARDWARE_DataIstallazione" HeaderText="HARDWARE_DataIstallazione" >
                            <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                            <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                        </asp:BoundField>
                    <asp:BoundField DataField="HARDWARE_GaranziaScadenza" HeaderText="HARDWARE_GaranziaScadenza" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HARDWARE_Fornitore" HeaderText="HARDWARE_Fornitore" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HARDWARE_FatturaFornitore" HeaderText="HARDWARE_FatturaFornitore" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HARDWARE_FatturaCliente" HeaderText="HARDWARE_FatturaCliente" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HARDWARE_HD" HeaderText="HARDWARE_HD" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                     <asp:TemplateField HeaderText="Azioni">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="editt" CommandArgument='<%# Eval("HARDWARE_Id") %>'  />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle CssClass="GridRowSelectedItem" Font-Bold="False" ForeColor="#181818" />
                <PagerStyle BackColor="#3E3E3E" HorizontalAlign="Center" CssClass="pagerGridView" Font-Bold="True" Font-Names="Arial" ForeColor="White" Height="36px" />
                <HeaderStyle Font-Bold="True" BackColor="#3E3E3E" ForeColor="White" Height="25px" />
                <RowStyle CssClass="GridRowItem" />
                <AlternatingRowStyle CssClass="GridRowAlternateItem" />
                <EmptyDataRowStyle CssClass="TestoGrigio" Height="50px" HorizontalAlign="Center" />
            </asp:GridView>
        

            <div style="display:flex; flex-direction:column; width:100vw;height:500px;">
                <div style="display:flex; flex-direction:row; width:100vw; justify-content:space-around ">
                    <div style="margin:50px">
                        <asp:TextBox ID="campo1" runat="server"></asp:TextBox>
                    </div>
                    <div style="margin:50px">
                        <asp:Calendar ID="campo2" runat="server"></asp:Calendar>
                    </div>
                </div>
                <div style="display:flex; flex-direction:row; width:100vw; justify-content:space-around">
                    <div style="margin:50px">
                        <asp:Calendar ID="campo3" runat="server"></asp:Calendar>
                    </div>
                    <div style="margin:50px">
                        <asp:TextBox ID="campo4" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div style="display:flex; flex-direction:row; width:100vw; justify-content:space-around">
                    <div style="margin:50px">
                        <asp:TextBox ID="campo5" runat="server"></asp:TextBox>
                    </div>
                    <div style="margin:50px">
                        <asp:TextBox ID="campo6" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div style="display:flex; flex-direction:row; width:100vw; justify-content:space-around">
                    <div style="margin:50px">
                        <asp:TextBox ID="campo7" runat="server"></asp:TextBox>
                    </div>
                    <div style="margin:50px"> 
                        <asp:Button ID="Button1" runat="server" Text="Modify" OnClick="updateButton_Click" />
                    </div>
                </div>
                <div style=" display:flex; flex-direction:row; width:100vw; justify-content:space-around">
                    <div style="margin:50px; visibility:hidden"> 
                        <asp:TextBox ID="campo0" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>

</asp:Content>
