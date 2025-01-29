using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebApplication4
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        private SqlConnection l_myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebSiteConnectionString"].ConnectionString);
        //Variabili globali

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CaricaDati();
            }
        }

        private void CaricaDati()
        {
            try
            {
                l_myConnection.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT HARDWARE_ID, HARDWARE_Seriale, HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza, HARDWARE_Fornitore, HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore, HARDWARE_HD FROM HARDWARE", l_myConnection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvHardwareInfo.DataSource = dt;
                gvHardwareInfo.DataBind();
            }
            finally
            {
                l_myConnection.Close();
            }

        }


        protected void data_RowCommand(Object sender, GridViewCommandEventArgs e)
        {

            // If multiple ButtonField column fields are used, use the
            // CommandName property to determine which button was clicked.
            if (e.CommandName == "editt")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = gvHardwareInfo.Rows[index];
                string seriale = selectedRow.Cells[1].Text;
                string dataIstallazione = selectedRow.Cells[2].Text;
                string garanziaScadenza = selectedRow.Cells[3].Text;
                string fornitore = selectedRow.Cells[4].Text;
                string fatturaCliente = selectedRow.Cells[5].Text;
                string fatturaFornitore = selectedRow.Cells[6].Text;
                string HD = selectedRow.Cells[7].Text;


                // Gestione del campo data DateTime dataIstallazione;
                DateTime dataIstallazione1;
                DateTime garanziaScadenza1;

                bool isIstallationDateValid = DateTime.TryParse(dataIstallazione, out dataIstallazione1);
                if (!isIstallationDateValid)
                {
                    dataIstallazione1 = DateTime.Parse(dataIstallazione);
                }

                bool isScadenzaDateValid = DateTime.TryParse(garanziaScadenza, out garanziaScadenza1);
                if (!isScadenzaDateValid)
                {
                    garanziaScadenza1 = DateTime.Parse(garanziaScadenza);
                }

                campo0.Text = "" + index;
                campo1.Text = "" + seriale;
                campo2.SelectedDate = dataIstallazione1;
                campo3.SelectedDate = garanziaScadenza1;
                campo4.Text = "" + fornitore;
                campo5.Text = "" + fatturaCliente;
                campo6.Text = "" + fatturaFornitore;
                campo7.Text = "" + HD;
            }

        }


        protected void updateButton_Click(object sender, EventArgs e)
        {

            try
            {


                l_myConnection.Open();


                SqlCommand myCommand = new SqlCommand("UPDATE HARDWARE SET HARDWARE_Seriale=@HARDWARE_Seriale, HARDWARE_DataIstallazione=@HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza=@HARDWARE_GaranziaScadenza, HARDWARE_Fornitore=@HARDWARE_Fornitore, HARDWARE_FatturaCliente=@HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore=@HARDWARE_FatturaFornitore, HARDWARE_HD=@HARDWARE_HD  WHERE (HARDWARE_Id = @HARDWARE_Id)", l_myConnection);
                myCommand.Parameters.AddWithValue("@HARDWARE_Id", campo0.Text.ToString());



                myCommand.Parameters.AddWithValue("@HARDWARE_Seriale", campo1.Text.ToString());
                myCommand.Parameters.AddWithValue("@HARDWARE_DataIstallazione", campo2.ToString());
                myCommand.Parameters.AddWithValue("@HARDWARE_GaranziaScadenza", campo3.ToString());
                myCommand.Parameters.AddWithValue("@HARDWARE_Fornitore", campo4.Text.ToString());
                myCommand.Parameters.AddWithValue("@HARDWARE_FatturaCliente", campo5.Text.ToString());
                myCommand.Parameters.AddWithValue("@HARDWARE_FatturaFornitore", campo6.Text.ToString());
                myCommand.Parameters.AddWithValue("@HARDWARE_HD", campo7.Text.ToString());

                //ESEGUO LA QUERY
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                l_myConnection.Close();

                CaricaDati();
            }
        }

        /*protected void btnEdit_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow selectedRow = gvHardwareInfo.Rows[index];
            string seriale = selectedRow.Cells[1].Text;
            string dataIstallazione = selectedRow.Cells[2].Text;
            string garanziaScadenza = selectedRow.Cells[3].Text;
            string fornitore = selectedRow.Cells[4].Text;
            string fatturaCliente = selectedRow.Cells[5].Text;
            string fatturaFornitore = selectedRow.Cells[6].Text;
            string HD = selectedRow.Cells[7].Text;

            campo0.Text = "" + index;
            campo1.Text = "" + seriale;
            campo2.Text = "" + dataIstallazione;
            campo3.Text = "" + garanziaScadenza;
            campo4.Text = "" + fornitore;
            campo5.Text = "" + fatturaCliente;
            campo6.Text = "" + fatturaFornitore;
            campo7.Text = "" + HD;
        }

        /*protected void gvData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRow = Convert.ToInt32(gvHardwareInfo.SelectedRow.Cells[1].Text);

            SqlCommand myCommand = new SqlCommand("SELECT HARDWARE_Id, HARDWARE_Seriale, HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza, HARDWARE_Fornitore, HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore, HARDWARE_HD FROM HARDWARE where (HARDWARE_Id = @CodiceAzienda)", l_myConnection);
            myCommand.Parameters.AddWithValue("@CodiceAzienda", selectedRow);

            try
            {
                string sqlQuery = "UPDATE HARDWARE SET HARDWARE_Seriale=@HARDWARE_Seriale, HARDWARE_DataIstallazione=@HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza=@HARDWARE_GaranziaScadenza, HARDWARE_Fornitore=@HARDWARE_Fornitore, HARDWARE_FatturaCliente=@HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore=@HARDWARE_FatturaFornitore, HARDWARE_HD=@HARDWARE_HD  WHERE HARDWARE_Id = @HARDARE_Id";
            myCommandSave.Parameters.AddWithValue("@CodiceAzienda", strCodiceAzienda);



            myCommandSave.Parameters.AddWithValue("@RagioneSociale", tbRagioneSociale.Text.ToString());
            myCommandSave.Parameters.AddWithValue("@Indirizzo", tbIndirizzo.Text.ToString());
            myCommandSave.Parameters.AddWithValue("@Localita", tbLocalita.Text.ToString());
            myCommandSave.Parameters.AddWithValue("@Cap", tbCap.Text.ToString());
            myCommandSave.Parameters.AddWithValue("@Provincia", tbProvincia.Text.ToString());
            myCommandSave.Parameters.AddWithValue("@Email", tbEmail.Text.ToString());
            myCommandSave.Parameters.AddWithValue("@PartitaIva", tbPartitaIva.Text.ToString());
            myCommandSave.Parameters.AddWithValue("@CodiceFiscale", tbCodiceFiscale.Text.ToString());
            myCommandSave.Parameters.AddWithValue("@Note", tbNote.Text.ToString());

            LitAzienda.Text = tbRagioneSociale.Text;

            }
            finally
            {
                l_myConnection.Close();
            }
        }*/

    }

}