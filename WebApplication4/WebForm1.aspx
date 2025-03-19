<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication4.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="scriptManager" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>

        window.onload = function () {
            if (performance.navigation.type === 1) {
                // Questo si attiva SOLO se la pagina è stata aggiornata (refresh)
                window.location.href = window.location.href;
            }
        };

        function getPdfId() {
            var hardwareId = document.getElementById('<%= campo0.ClientID %>').value;
            if (hardwareId.trim() === "") {
                alert("Inserisci il nome del file!");
                return;
            }
            openPdfPopup(hardwareId);
           
        }
        // Apre il pdf in un pop up
        function openPdfPopup(pdfId) {
            
            var popupUrl = "PdfHandler.ashx?id=" + encodeURIComponent(pdfId);
            window.open(popupUrl, "PDF Viewer", "width=800,height=600");
        }

        //visualizza un panel in base all' id passato
        function showPanel(id) {
            if (id == "addStorage") {
                var btn = document.getElementById('<%= btnToggleVisibilityStorage.ClientID %>').click();

            }
            if (id == "addServerFisico") {
                document.getElementById('<%= btnToggleVisibility.ClientID %>').click();
            }
            if (id == "addCliente") {
                document.getElementById('<%= btnToggleVisibilityClient.ClientID %>').click();
            }
            if (id == "addHw") {
                document.getElementById('<%= btnToggleVisibilityHw.ClientID %>').click();
            }
            if (id == "addServerVirtuale"){
                document.getElementById('<%= btnToggleVisibilityServerVirtuale.ClientID %>').click();
            }
            if (id=="addStorageVirtuale") {
                document.getElementById('<%= btnToggleVisibilityStorageVirtuale.ClientID %>').click();
            }
        }   

        //confirm delete box
        function confirmDelete() {
            return confirm("Sei sicuro di voler eliminare questo cliente? potrebbe essere già collegato a qualche hardware o sever fisico");
        }
        
    </script>
    


    <!--<script type="text/javascript" src="Scripts/script.js"></script>-->

    <updatepanel id="ClientUpdatepanel" runat="server">
        <ContentTemplate>
            <table style="width: calc(100%);" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height:50px; background-color:blanchedalmond; border-bottom:solid 1px #C7C7C7">
                        <h2 style="margin-left:10px">CLIENTI<span style="float:right; margin:-5px 10px 0 0;" ><i id="addCliente" style="font-weight: bolder; background-color: darksalmon; font-size:32px; border: 2px solid darksalmon; border-radius: 30%" class="bi bi-plus" onClick="showPanel(this.id)"></i></span></h2>
                     </td>
                </tr>
            </table>

            <asp:GridView ID="gvClientInfo" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False" DataKeyNames="CLIENTE_Id" BackColor="#E9E9E9" 
BorderStyle="None" CellPadding="0" CellSpacing="0" ForeColor="Black" PageSize="100" BorderWidth="0px" PagerSettings-PageButtonCount="25" onrowcommand="ClientData_RowCommand" OnRowDataBound="gvClientInfo_RowDataBound"  EmptyDataText="NESSUN CLIENTE TROVATO">
                <Columns>
                    <asp:BoundField DataField="CLIENTE_Id" HeaderText="CLIENTE_Id">
                    <ItemStyle HorizontalAlign="Left" CssClass="hidden" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CLIENTE_Nome" HeaderText="CLIENTE_Nome" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Azioni">
                        <ItemTemplate>
                            <asp:Button ID="btnEditClient" runat="server" Text="Edit client" CommandName="EditClient" CommandArgument='<%# Container.DataItemIndex %>'  />
                            <asp:Button ID="btnShow" runat="server" Text="Visualizza Hardware e Server Fisico" CommandName="ShowHwServerF" CommandArgument='<%# Container.DataItemIndex %>'  />
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

            <div Style="min-height: 150px;" >
                <asp:Panel ID="ClientPnlForm" runat="server"  CssClass="form-container">
                    <div class="form-row">
                        <div>
                            <label for="clientNameField">Nome Cliente</label>
                            <asp:TextBox ID="clientNameField" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <asp:button ID="clientUpdate" runat="server" text="Save Client" OnClick="clientUpdate_Click" CssClass="updateClient-btn"/>
                        </div>
                        <div>
                            <asp:button ID="clientDelete" runat="server" text="Delete Client" OnClick="clientDelete_Click" OnClientClick="return confirmDelete();" CssClass="updateClient-btn"/>
                        </div>
                    </div>
                    <div CssClass="form-row" style=" visibility:hidden; height: none">
                        <asp:TextBox ID="clientCampo0" runat="server"></asp:TextBox>
                    </div>
                </asp:Panel>
            </div>

            <asp:Button ID="btnToggleVisibilityClient" runat="server" OnClick="btnToggleVisibilityClient_Click" style="display:none;" />

        </ContentTemplate>
    </updatepanel>

    <updatepanel ID="HwUpdatepanel" runat="server">
        <ContentTamplate>
             <table style="width:calc(100%);" cellpadding="0" cellspacing="0">
                 <tr>
                     <td style="height:50px; background-color:blanchedalmond; border-bottom:solid 1px #C7C7C7">
                         <h2 style="margin-left:10px">HARDWARE CLIENTI<span style="float:right; margin:-5px 10px 0 0;" ><i id="addHw" style="font-weight: bolder; background-color: darksalmon; font-size:32px; border: 2px solid darksalmon; border-radius: 30%" class="bi bi-plus" onClick="showPanel(this.id)"></i></span></h2>
                     </td>
                 </tr>
             </table>
            <asp:GridView ID="gvHardwareInfo" runat="server" AllowPaging="True" Width="100%" DataKeyNames="HARDWARE_Id" AutoGenerateColumns="False" BackColor="#E9E9E9" 
            BorderStyle="None" CellPadding="0" CellSpacing="0" ForeColor="Black" 
             PageSize="100" 
            BorderWidth="0px" PagerSettings-PageButtonCount="25"  onrowcommand="data_RowCommand"  EmptyDataText="NESSUN HARDWARE TROVATO">
                <Columns>
                    <asp:BoundField DataField="HARDWARE_Id" HeaderText="HARDWARE_Id" >
                    <ItemStyle HorizontalAlign="Left" CssClass="hidden" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="hidden" />
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
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="editt" CommandArgument='<%# Container.DataItemIndex %>'  />
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
            
            <div style=" min-height: 150px;">
                <asp:Panel ID="HwPnlForm" runat="server" CssClass="form-container">
                    <div class="form-row">
                        <div>
                            <label for="txtNome">Seriale:</label>
                            <asp:TextBox ID="campo1" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="txtSeriale">Fornitore:</label>
                            <asp:TextBox ID="campo4" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="txtDataScadenza">Data Istallazione:</label>
                            <asp:Textbox ID="campo2" runat="server" type="date"></asp:Textbox>
                        </div>
                        <div>
                            <label for="txtDataCreazione">Data Scadenza:</label>
                            <asp:Textbox ID="campo3" runat="server" type="date"></asp:Textbox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="txtTipo">Fattura Cliente:</label>
                            <asp:TextBox ID="campo5" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="txtCategoria">Fattura Fornitore:</label>
                            <asp:TextBox ID="campo6" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="txtTipo">HD:</label>
                            <asp:TextBox ID="campo7" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="txtCategoria">Nome File:</label>
                            <asp:TextBox ID="campo8" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="button-container">
                        <asp:Button ID="Button1" runat="server" Text="Update Hardware" CssClass="update-btn" OnClick="updateButton_Click"/>
                        <asp:Button ID="BtnDropRow" runat="server" Text="Delete Hardware" CssClass="delete-btn"  OnClick="BtnDropRow_Click"/>
                    </div>
                    <div class="button-container">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                        <asp:Button ID="BtnDisplayFile" runat="server" Text="Display File" OnClientClick="getPdfId(); return false;" CssClass="showFile-btn" />  
                        <asp:Button ID="BtnCancelFile" runat="server" Text="Delete File" CssClass="deleteFile-btn" OnClick="BtnCancelFile_Click"/>
                    </div>
                    <div style="visibility:hidden; height: none"> 
                        <asp:TextBox ID="campo0" runat="server"></asp:TextBox>
                    </div>
                </asp:Panel>
            </div>
                  
            <asp:Button ID="btnToggleVisibilityHw" runat="server" OnClick="btnToggleVisibilityHw_Click" style="display:none;" />

        </ContentTamplate>
    </updatepanel>

    <updatepanel ID ="ServerFisicoUpdatePanel" runat="server">
        <contentTemplate>
            <table style="width:calc(100%);" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height:50px; background-color:blanchedalmond; border-bottom:solid 1px #C7C7C7">
                        <h2 style="margin-left:10px">SERVER FISICI<span style="float:right; margin:-5px 10px 0 0;" ><i id="addServerFisico" style="font-weight: bolder; background-color: darksalmon; font-size:32px; border: 2px solid darksalmon; border-radius: 30%" class="bi bi-plus" onclick="showPanel(this.id)"></i></span></h2>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvServerFisico" runat="server" AllowPaging="True" Width="100%" DataKeyNames="SERVER_FISICO_Id" AutoGenerateColumns="False" BackColor="#E9E9E9" BorderStyle="None" CellPadding="0" CellSpacing="0" ForeColor="Black"  PageSize="100" 
BorderWidth="0px" PagerSettings-PageButtonCount="25"  onrowcommand="data_RowCommandServerFisico"  EmptyDataText="NESSUN SERVER FISICO TROVATO">
                <Columns>
                    <asp:BoundField DataField="SERVER_FISICO_Id" HeaderText="SERVER_FISICO_Id" >
                    <ItemStyle HorizontalAlign="Left" CssClass="hidden" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="hidden" />
                    </asp:BoundField>
                        <asp:BoundField DataField="SERVER_FISICO_Nome" HeaderText="SERVER_FISICO_Nome" >
                            <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                            <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                     <asp:TemplateField HeaderText="Azioni">
                        <ItemTemplate>
                            <asp:Button ID="btnShowInfoServerFisico" runat="server" Text="show details" CommandName="ShowInfo" CommandArgument='<%# Container.DataItemIndex %>'  />
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

            <div style=" min-height: 150px" id="ServerFisicoPanelForm">
                <asp:Panel ID="ServerFisicoPanel" runat="server" CssClass="form-container" >
                    <div class="form-row">
                        <div>
                            <label for="ServerFisicoNome">Nome server fisico:</label>
                            <asp:TextBox ID="ServerFisicoNome" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="ServerFisicoNote">Note:</label>
                            <asp:TextBox ID="ServerFisicoNote" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="ServerFisicoSeriale">Seriale:</label>
                            <asp:Textbox ID="ServerFisicoSeriale" runat="server" ></asp:Textbox>
                        </div>
                        <div>
                            <label for="ServerFisicoDataIstallazione">Data Istallazione:</label>
                            <asp:Textbox ID="ServerFisicoDataIstallazione" runat="server" type="date"></asp:Textbox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="ServerFisicoGaranziaScadenza">Data scadenza garanzia:</label>
                            <asp:TextBox ID="ServerFisicoGaranziaScadenza" runat="server" type="date"></asp:TextBox>
                        </div>
                        <div>
                            <label for="ServerFisicoIP">IP:</label>
                            <asp:TextBox ID="ServerFisicoIP" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="ServerFisicoHD">HD:</label>
                            <asp:TextBox ID="ServerFisicoHD" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="ServerFisicoFornitore">Fornitore:</label>
                            <asp:TextBox ID="ServerFisicoFornitore" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="ServerFisicoFatturaCliente">Fattura Cliente:</label>
                            <asp:TextBox ID="ServerFisicoFatturaCliente" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="ServerFisicoFatturaFornitore">Fattura fornitore:</label>
                            <asp:TextBox ID="ServerFisicoFatturaFornitore" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="ServerFisicoDominio">Dominio:</label>
                            <asp:TextBox ID="ServerFisicoDominio" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="ServerFisicoSubnet">Subnet:</label>
                            <asp:TextBox ID="ServerFisicoSubnet" runat="server"></asp:TextBox>
                        </div>
                    </div>

                     <div class="form-row">
                         <div>
                             <label for="ServerFisicoGateway">Gateway:</label>
                             <asp:TextBox ID="ServerFisicoGateway" runat="server"></asp:TextBox>
                         </div>
                         <div>
                             <label for="ServerFisicoHostNameMailServer">Host name mail server:</label>
                             <asp:TextBox ID="ServerFisicoHostNameMailServer" runat="server"></asp:TextBox>
                         </div>
                     </div>

                    <div class="form-row">
                        <div>
                            <asp:Button ID="SaveServerFisico" runat="server" Text="Save" OnClick="SaveServerFisico_Click" CssClass="saveServerFisico-btn" />
                        </div>
                        <div>
                            <asp:Button ID="deleteServerFisico" runat="server" Text="Delete" OnClick="deleteServerFisico_Click" CssClass="saveServerFisico-btn" />
                        </div>
                    </div>
                     <div style="visibility:hidden; height: none"> 
                         <asp:TextBox ID="ServerFisicoCampo0" runat="server"></asp:TextBox>
                     </div>

                </asp:Panel>
            </div>

            <asp:Button ID="btnToggleVisibility" runat="server" OnClick="btnToggleVisibility_Click" style="display:none;" />
             
        </contentTemplate>
    </updatepanel>

        

        <updatepanel ID ="StorageUpdatePanel" runat="server">
        <contentTemplate>
            <table style="width:calc(100%); " cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height:50px; background-color:blanchedalmond; border-bottom:solid 1px #C7C7C7">
                        <h2 style="margin-left:10px">STORAGE SERVER FISICI<span style="float:right; margin:-5px 10px 0 0;" ><i id="addStorage" style="font-weight: bolder; background-color: darksalmon; font-size:32px; border: 2px solid darksalmon; border-radius: 30%" class="bi bi-plus" onclick="showPanel(this.id)"></i></span></h2>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvStorage" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False" BackColor="#E9E9E9" BorderStyle="None" CellPadding="0" CellSpacing="0" ForeColor="Black"  PageSize="100" 
BorderWidth="0px" PagerSettings-PageButtonCount="25"  EmptyDataText="NESSUN STORAGE TROVATO" onrowcommand="data_RowCommandStorage" >
                <Columns>
                    <asp:BoundField DataField="STORAGE_Id" HeaderText="STORAGE_Id" >
                    <ItemStyle HorizontalAlign="Left" CssClass="hidden" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="hidden" />
                    </asp:BoundField>
                        <asp:BoundField DataField="STORAGE_Nome" HeaderText="STORAGE_Nome" >
                            <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                            <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STORAGE_Capacita" HeaderText="STORAGE_Capacita" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STORAGE_Note" HeaderText="STORAGE_Note" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                     <asp:TemplateField HeaderText="Azioni">
                        <ItemTemplate>
                            <asp:Button ID="btnShowInfoStorage" runat="server" Text="show details" CommandName="ShowInfoStorage" CommandArgument='<%# Container.DataItemIndex %>'  />
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
            
            <div style="height: 200px" id="StoragePanelForm" >
                <asp:Panel ID="StoragePanel" runat="server" CssClass="form-container">
                    <div class="form-row form-storageFisico">
                        <div>
                            <label for="StorageNome">Nome:</label>
                            <asp:TextBox ID="StorageNome" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="StorageCapacita">Capacita:</label>
                            <asp:TextBox ID="StorageCapacita" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="StorageNote">Note:</label>
                            <asp:Textbox ID="StorageNote" runat="server" ></asp:Textbox>
                        </div>
                    </div>

                    <asp:Literal ID="dynamicDivLiteral" runat="server" />


                    <div class="form-row">
                        <div>
                            <asp:button ID="SaveStorage" runat="server" text="Save" OnClick="SaveStorage_Click" CssClass="saveStorage-btn"/>
                        </div>
                        <div>
                            <asp:button ID="DeleteStorage" runat="server" text="Delete" OnClick="DeleteStorage_Click" CssClass="saveStorage-btn"/>
                        </div>
                    </div>
                    <div style="visibility:hidden; height: none"> 
                        <asp:TextBox ID="storageCampo0" runat="server"></asp:TextBox>
                    </div>

                </asp:Panel>
            </div>

            <asp:Button ID="btnToggleVisibilityStorage" runat="server" OnClick="btnToggleVisibilityStorage_Click" style="display:none;" />
           
        </contentTemplate>
    </updatepanel>

            <updatepanel ID ="ServerVirtualeUpdatePanel" runat="server">
        <contentTemplate>
            <table style="width:calc(100%);" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height:50px; background-color:blanchedalmond; border-bottom:solid 1px #C7C7C7">
                        <h2 style="margin-left:10px">SERVER VIRTUALE<span style="float:right; margin:-5px 10px 0 0;" ><i id="addServerVirtuale" style="font-weight: bolder; background-color: darksalmon; font-size:32px; border: 2px solid darksalmon; border-radius: 30%" class="bi bi-plus" onclick="showPanel(this.id)"></i></span></h2>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvServerVirtuale" runat="server" AllowPaging="True" Width="100%" DataKeyNames="SERVER_VIRTUALE_Id" AutoGenerateColumns="False" BackColor="#E9E9E9" BorderStyle="None" CellPadding="0" CellSpacing="0" ForeColor="Black"  PageSize="100" 
BorderWidth="0px" PagerSettings-PageButtonCount="25"  onrowcommand="data_RowCommandServerVirtuale"  EmptyDataText="NESSUN SERVER VIRTUALE TROVATO">
                <Columns>
                    <asp:BoundField DataField="SERVER_VIRTUALE_Id" HeaderText="SERVER_VIRTUALE_Id" >
                    <ItemStyle HorizontalAlign="Left" CssClass="hidden" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="hidden" />
                    </asp:BoundField>
                        <asp:BoundField DataField="SERVER_VIRTUALE_Nome" HeaderText="SERVER_VIRTUALE_Nome" >
                            <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                            <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                     <asp:TemplateField HeaderText="Azioni">
                        <ItemTemplate>
                            <asp:Button ID="btnShowInfoServerVirtuale" runat="server" Text="show details" CommandName="ShowInfoServerVirtuale" CommandArgument='<%# Container.DataItemIndex %>'  />
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

            <div style=" min-height: 150px" >
                <asp:Panel ID="ServerVirtualePanelForm" runat="server" CssClass="form-container" >
                    <div class="form-row">
                        <div>
                            <label for="ServerVirtualeNome">Nome server virtuale:</label>
                            <asp:TextBox ID="ServerVirtualeNome" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="ServerVirtualeRam">RAM:</label>
                            <asp:TextBox ID="ServerVirtualeRam" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <label for="ServerVirtualeCPU">CPU:</label>
                            <asp:Textbox ID="ServerVirtualeCPU" runat="server" ></asp:Textbox>
                        </div>
                        <div>
                            <label for="ServerVirtualeIP">IP:</label>
                            <asp:Textbox ID="ServerVirtualeIP" runat="server" ></asp:Textbox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="saveServerVirtuale">
                            <asp:button ID="SaveServerVirtuale" runat="server" text="Save" OnClick="SaveServerVirtuale_Click" CssClass="saveServerFisico-btn"/>
                        </div>
                        <div class="saveServerVirtuale">
                            <asp:button ID="deleteServerVirtuale" runat="server" text="Delete" OnClick="deleteServerVirtuale_Click" CssClass="saveServerFisico-btn"/>
                        </div>
                    </div>

                     <div style="visibility:hidden; height: none"> 
                         <asp:TextBox ID="serverVirtualeCampo0" runat="server"></asp:TextBox>
                     </div>

                </asp:Panel>
            </div>

            <asp:Button ID="btnToggleVisibilityServerVirtuale" runat="server" OnClick="btnToggleVisibilityServerVirtuale_Click" style="display:none;" />
             
        </contentTemplate>
    </updatepanel>

            <updatepanel ID ="StorageVirtualeUpdatepanel" runat="server">
        <contentTemplate>
            <table style="width:calc(100%); " cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height:50px; background-color:blanchedalmond; border-bottom:solid 1px #C7C7C7">
                        <h2 style="margin-left:10px">STORAGE SERVER VIRTUALE<span style="float:right; margin:-5px 10px 0 0;" ><i id="addStorageVirtuale" style="font-weight: bolder; background-color: darksalmon; font-size:32px; border: 2px solid darksalmon; border-radius: 30%" class="bi bi-plus" onclick="showPanel(this.id)"></i></span></h2>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvStorageVirtuale" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False" BackColor="#E9E9E9" BorderStyle="None" CellPadding="0" CellSpacing="0" ForeColor="Black"  PageSize="100" 
BorderWidth="0px" PagerSettings-PageButtonCount="25"  EmptyDataText="NESSUN STORAGE TROVATO" onrowcommand="data_RowCommandStorageVirtuale" >
                <Columns>
                    <asp:BoundField DataField="STORAGE_Id" HeaderText="STORAGE_Id" >
                    <ItemStyle HorizontalAlign="Left" CssClass="hidden" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="hidden" />
                    </asp:BoundField>
                        <asp:BoundField DataField="STORAGE_Nome" HeaderText="STORAGE_Nome" >
                            <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                            <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STORAGE_Capacita" HeaderText="STORAGE_Capacita" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STORAGE_Note" HeaderText="STORAGE_Note" >
                        <ItemStyle HorizontalAlign="Left" CssClass="TestoNero10px" />
                        <HeaderStyle HorizontalAlign="Left" CssClass="tagElenco" />
                    </asp:BoundField>
                     <asp:TemplateField HeaderText="Azioni">
                        <ItemTemplate>
                            <asp:Button ID="btnShowInfoStorageVirtuale" runat="server" Text="show details" CommandName="ShowInfoStorageVirtuale" CommandArgument='<%# Container.DataItemIndex %>'  />
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
            
            <div style="height: 200px"  >
                <asp:Panel ID="StorageVirtualePanel" runat="server" CssClass="form-container">
                    <div class="form-row">
                        <div>
                            <label for="StorageVirtualeNome">Nome Storage Virtuale:</label>
                            <asp:TextBox ID="StorageVirtualeNome" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="StorageVirtualeCapacita">Capacita Storage Virtuale:</label>
                            <asp:TextBox ID="StorageVirtualeCapacita" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label for="StorageVirtualeNote">Note Storage Virtuale:</label>
                            <asp:Textbox ID="StorageVirtualeNote" runat="server" ></asp:Textbox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div>
                            <asp:button ID="SaveStorageVirtuale" runat="server" text="Save" OnClick="SaveStorageVirtuale_Click" CssClass="saveStorage-btn"/>
                        </div>
                        <div>
                            <asp:button ID="DeleteStorageVirtuale" runat="server" text="Delete" OnClick="DeleteStorageVirtuale_Click" CssClass="saveStorage-btn"/>
                        </div>
                    </div>
                    <div style="visibility:hidden; height: none"> 
                        <asp:TextBox ID="StorageVirtualeCampo0" runat="server"></asp:TextBox>
                    </div>

                </asp:Panel>
            </div>

            <asp:Button ID="btnToggleVisibilityStorageVirtuale" runat="server" OnClick="btnToggleVisibilityStorageVirtuale_Click" style="display:none;" />
           
        </contentTemplate>
    </updatepanel>

</asp:Content>
