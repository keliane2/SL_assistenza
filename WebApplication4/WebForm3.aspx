<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="WebApplication4.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>


// campi obligatori
<!--html-->
<asp:TextBox ID="txtNome" runat="server" />
<asp:TextBox ID="txtEmail" runat="server" />

<asp:CustomValidator 
    ID="cvMultiValidator" 
    runat="server" 
    ControlToValidate="txtNome" 
    ClientValidationFunction="validazioneClient"
    OnServerValidate="validazioneServer"
    ErrorMessage="Tutti i campi sono obbligatori!" 
    ForeColor="Red" />

<asp:Button ID="btnInvia" runat="server" Text="Invia" />




.selected-row {
    background-color: darksalmon !important;
}


document.addEventListener("DOMContentLoaded", function () {
    var righe = document.querySelectorAll("#<%= gvClientInfo.ClientID %> tr"); // Seleziona tutte le righe della GridView

    righe.forEach(function (riga) {
        riga.addEventListener("click", function () {
            // Rimuove la classe 'selected-row' da tutte le righe
            righe.forEach(r => r.classList.remove("selected-row"));

            // Aggiunge la classe alla riga cliccata
            this.classList.add("selected-row");
        });
    });
});



$(document).ready(function () {
    // Gestisci il click sulle righe della GridView
    $("#<%= gvStorage.ClientID %> tr").click(function () {

        document.getElementById('<%= btnToggleVisibilityStorage.ClientID %>').click();
        // Rimuovi il colore di sfondo da tutte le righe
        $("#<%= gvStorage.ClientID %> tr").css("background-color", "");
        $("#<%= gvStorage.ClientID %> tr:first-child").css("background-color", "black");

        // Cambia il colore della riga selezionata
        $(this).css("background-color", "darksalmon");  // Sostituisci con il colore che desideri


    });
});




using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;

public partial class YourPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod]
    public static List<string> GetDropdownItems()
    {
        List<string> items = new List<string>();
        string connStr = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string query = "SELECT ColumnName FROM YourTable";  // Modify this query based on your database
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                items.Add(reader["ColumnName"].ToString());
            }
        }

        return items;
    }
}


//front end
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<script>
    function loadDropdownItems() {
        $.ajax({
            type: "POST",
            url: "YourPage.aspx/GetDropdownItems",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var ddl = $("#<%= ddlItems.ClientID %>");
                ddl.empty(); // Clear existing items

                // Add new items
                $.each(response.d, function (index, item) {
                    ddl.append($("<option></option>").val(item).text(item));
                });
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    }
</script>

<asp:DropDownList ID="ddlItems" runat="server" CssClass="form-control">
</asp:DropDownList>

<asp:Button ID="btnLoadItems" runat="server" CssClass="btn btn-primary" Text="Load Items" OnClientClick="loadDropdownItems(); return false;" />
<asp:GridView ID="gvDati" runat="server" AutoGenerateColumns="False" CssClass="grid">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" />
        <asp:BoundField DataField="Nome" HeaderText="Nome" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
    </Columns>
</asp:GridView>