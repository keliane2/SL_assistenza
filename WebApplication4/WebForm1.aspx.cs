using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebApplication4
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //Variabili globali
        private SqlConnection l_myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebSiteConnectionString"].ConnectionString);
        private SqlDataAdapter da, da2;
        private List<StorageFisico> StorageFisicoList;
        private List<StorageVirtuale> StorageVirtualeList;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StorageFisicoList"] != null)
            {
                StorageFisicoList = (List<StorageFisico>)Session["StorageFisicoList"];
            }
            else
            {
                StorageFisicoList = new List<StorageFisico>();
            }
            if (Session["StorageVirtualeList"] != null)
            {
                StorageVirtualeList = (List<StorageVirtuale>)Session["StorageVirtualeList"];
            }
            else
            {
                StorageVirtualeList = new List<StorageVirtuale>();
            }
            if (!Page.IsPostBack)
            {
                hideAll();
                CaricaDatiClienti();

            }
        }

        private void CaricaDati(SqlDataAdapter da)
        {
            try
            {
                l_myConnection.Open();

                DataTable dt = new DataTable();
                da.Fill(dt);
                gvHardwareInfo.DataSource = dt;
                gvHardwareInfo.DataBind();
                // gvHardwareInfo.Columns[0].Visible = false;
            }
            finally
            {
                l_myConnection.Close();
            }

        }

        private void hideAll()
        {
            ServerFisicoPanel.Visible = false;
            StorageUpdatePanel.Visible = false;
            HwUpdatepanel.Visible = false;
            ServerFisicoUpdatePanel.Visible = false;
            StoragePanel.Visible = false;
            ClientPnlForm.Visible = false;
            HwPnlForm.Visible = false;
            ServerVirtualePanelForm.Visible = false;
            ServerVirtualeUpdatePanel.Visible = false;
            StorageVirtualePanel.Visible = false;
            StorageVirtualeUpdatepanel.Visible = false;
            SSF.Visible = false;
            ServerFisicoActionPanel.Visible = false;
            SSV.Visible = false;
            ServerVirtualeActionPanel.Visible = false;
        }

        private void CaricaDatiServerFisico(SqlDataAdapter da)
        {
            try
            {
                l_myConnection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvServerFisico.DataSource = dt;
                gvServerFisico.DataBind();
                //gvClientInfo.Columns[0].Visible = false;
            }
            finally
            {
                l_myConnection.Close();
            }

        }

        private void CaricaDatiServerVirtuale(string SERVER_VIRTUALE_IdServerFisico)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT SERVER_VIRTUALE_Id, SERVER_VIRTUALE_Nome FROM SERVER_VIRTUALI  WHERE SERVER_VIRTUALE_IdServerFisico=" + SERVER_VIRTUALE_IdServerFisico, l_myConnection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvServerVirtuale.DataSource = dt;
                gvServerVirtuale.DataBind();
                //gvClientInfo.Columns[0].Visible = false;
            }
            finally
            {
                l_myConnection.Close();
            }

        }

        private void CaricaDatiStorage(string STORAGE_ServerFisicoId)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT STORAGE_Id, STORAGE_Nome, STORAGE_Capacita, STORAGE_Note FROM STORAGE WHERE STORAGE_ServerFisicoId=" + STORAGE_ServerFisicoId, l_myConnection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvStorage.DataSource = dt;
                gvStorage.DataBind();
                //gvClientInfo.Columns[0].Visible = false;
            }
            finally
            {
                l_myConnection.Close();
            }

        }


        protected void data_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editt")
            {
                HwPnlForm.Visible = true;
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = gvHardwareInfo.Rows[index];
                string seriale = selectedRow.Cells[1].Text;
                string dataIstallazione = selectedRow.Cells[2].Text;
                string garanziaScadenza = selectedRow.Cells[3].Text;
                string fornitore = selectedRow.Cells[4].Text;
                string fatturaCliente = selectedRow.Cells[5].Text;
                string fatturaFornitore = selectedRow.Cells[6].Text;
                string HD = selectedRow.Cells[7].Text;


                campo0.Text = "" + selectedRow.Cells[0].Text;
                campo1.Text = "" + seriale;
                campo2.Text = DateTime.Parse(dataIstallazione).ToString("yyyy-MM-dd");
                campo3.Text = DateTime.Parse(garanziaScadenza).ToString("yyyy-MM-dd");
                campo4.Text = "" + fornitore;
                campo5.Text = "" + fatturaCliente;
                campo6.Text = "" + fatturaFornitore;
                campo7.Text = "" + HD;

                //I check if there is a linked file
                try
                {
                    l_myConnection.Open();
                    SqlCommand myCommand = new SqlCommand("SELECT FILE_Nome FROM FILES WHERE (FILE_IdHardware=@FILE_IdHardware)", l_myConnection);
                    myCommand.Parameters.AddWithValue("@FILE_IdHardware", campo0.Text.ToString());

                    SqlDataReader myDataReader = myCommand.ExecuteReader();

                    if (myDataReader.Read())
                    {
                        if (myDataReader["FILE_Nome"] != DBNull.Value)
                            campo8.Text = myDataReader["FILE_Nome"].ToString();
                    }
                    else
                    {
                        campo8.Text = "";
                    }
                }
                finally
                {
                    l_myConnection.Close();
                }

            }
            else if (e.CommandName == "deletee")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                SqlCommand cmd = new SqlCommand("DELETE from HARDWARE WHERE (HARDWARE_Id = @HARDWARE_Id)", l_myConnection);
                cmd.Parameters.AddWithValue("@HARDWARE_Id", index);

                try
                {
                    l_myConnection.Open();

                    cmd.ExecuteNonQuery();

                }
                finally
                {
                    da = new SqlDataAdapter("SELECT HARDWARE_ID, HARDWARE_Seriale, HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza, HARDWARE_Fornitore, HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore, HARDWARE_HD FROM HARDWARE", l_myConnection);
                    l_myConnection.Close();

                    CaricaDati(da);
                }
            }

        }

        protected void data_RowCommandServerFisico(Object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow selectedRow = gvServerFisico.Rows[index];

            ServerFisicoCampo0.Text = "" + selectedRow.Cells[0].Text;

            try
            {
                l_myConnection.Open();

                if (e.CommandName == "ShowInfo")
                {
                    ServerFisicoPanel.Visible = true;
                    StorageUpdatePanel.Visible = true;
                    ServerFisicoActionPanel.Visible = true;
                    StoragePanel.Visible = false;
                    ServerVirtualeUpdatePanel.Visible = true;
                    StorageVirtualeUpdatepanel.Visible = false;
                    ServerVirtualePanelForm.Visible = false;
                    SSF.Visible = false;
                    //SaveServerFisico.Enabled = false;
                    deleteServerFisico.Enabled = true;
                    SqlCommand myCommand = new SqlCommand("SELECT SERVER_FISICO_Nome, SERVER_FISICO_Note, SERVER_FISICO_Seriale, SERVER_FISICO_DataIstallazione, SERVER_FISICO_GaranziaScadenza, SERVER_FISICO_IP, SERVER_FISICO_HD, SERVER_FISICO_Fornitore, SERVER_FISICO_FatturaCliente, SERVER_FISICO_FatturaFornitore, SERVER_FISICO_Dominio, SERVER_FISICO_Subnet, SERVER_FISICO_Gateway, SERVER_FISICO_HostNameMailServer FROM SERVER_FISICI where SERVER_FISICO_Id = @SERVER_FISICO_Id", l_myConnection);
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_Id", ServerFisicoCampo0.Text.ToString());

                    SqlDataReader myDataReader = myCommand.ExecuteReader();

                    if (myDataReader.Read())
                    {
                        if (myDataReader["SERVER_FISICO_Nome"] != DBNull.Value)
                            ServerFisicoNome.Text = myDataReader["SERVER_FISICO_Nome"].ToString();

                        if (myDataReader["SERVER_FISICO_Note"] != DBNull.Value)
                            ServerFisicoNote.Text = myDataReader["SERVER_FISICO_Note"].ToString();

                        if (myDataReader["SERVER_FISICO_Seriale"] != DBNull.Value)
                            ServerFisicoSeriale.Text = myDataReader["SERVER_FISICO_Seriale"].ToString();

                        if (myDataReader["SERVER_FISICO_DataIstallazione"] != DBNull.Value)
                            ServerFisicoDataIstallazione.Text = DateTime.Parse(myDataReader["SERVER_FISICO_DataIstallazione"].ToString()).ToString("yyyy-MM-dd");

                        if (myDataReader["SERVER_FISICO_GaranziaScadenza"] != DBNull.Value)
                            ServerFisicoGaranziaScadenza.Text = DateTime.Parse(myDataReader["SERVER_FISICO_GaranziaScadenza"].ToString()).ToString("yyyy-MM-dd");

                        if (myDataReader["SERVER_FISICO_IP"] != DBNull.Value)
                            ServerFisicoIP.Text = myDataReader["SERVER_FISICO_IP"].ToString();

                        if (myDataReader["SERVER_FISICO_HD"] != DBNull.Value)
                            ServerFisicoHD.Text = myDataReader["SERVER_FISICO_HD"].ToString();

                        if (myDataReader["SERVER_FISICO_Fornitore"] != DBNull.Value)
                            ServerFisicoFornitore.Text = myDataReader["SERVER_FISICO_Fornitore"].ToString();

                        if (myDataReader["SERVER_FISICO_FatturaCliente"] != DBNull.Value)
                            ServerFisicoFatturaCliente.Text = myDataReader["SERVER_FISICO_FatturaCliente"].ToString();

                        if (myDataReader["SERVER_FISICO_FatturaFornitore"] != DBNull.Value)
                            ServerFisicoFatturaFornitore.Text = myDataReader["SERVER_FISICO_Fornitore"].ToString();

                        if (myDataReader["SERVER_FISICO_Dominio"] != DBNull.Value)
                            ServerFisicoDominio.Text = myDataReader["SERVER_FISICO_Dominio"].ToString();

                        if (myDataReader["SERVER_FISICO_Subnet"] != DBNull.Value)
                            ServerFisicoSubnet.Text = myDataReader["SERVER_FISICO_Subnet"].ToString();

                        if (myDataReader["SERVER_FISICO_Gateway"] != DBNull.Value)
                            ServerFisicoGateway.Text = myDataReader["SERVER_FISICO_Gateway"].ToString();

                        if (myDataReader["SERVER_FISICO_HostNameMailServer"] != DBNull.Value)
                            ServerFisicoHostNameMailServer.Text = myDataReader["SERVER_FISICO_HostNameMailServer"].ToString();


                        //carico i dati nei gridview degli storage e dei server virtuali
                        CaricaDatiStorage(ServerFisicoCampo0.Text.ToString());
                        CaricaDatiServerVirtuale(ServerFisicoCampo0.Text.ToString());

                    }

                }

            }
            finally
            {
                l_myConnection.Close();
            }
        }

        protected void data_RowCommandServerVirtuale(Object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow selectedRow = gvServerVirtuale.Rows[index];

            serverVirtualeCampo0.Text = "" + selectedRow.Cells[0].Text;
            ServerVirtualePanelForm.Visible = true;

            try
            {
                l_myConnection.Open();
                if (e.CommandName == "ShowInfoServerVirtuale")
                {
                    //ServerFisicoPanel.Visible = true;
                    StorageVirtualeUpdatepanel.Visible = true;
                    StorageVirtualePanel.Visible = false;
                    SSV.Visible = false;
                    ServerVirtualeActionPanel.Visible = true;
                    deleteServerVirtuale.Enabled = true;
                    SqlCommand myCommand = new SqlCommand("SELECT SERVER_VIRTUALE_Nome, SERVER_VIRTUALE_RAM, SERVER_VIRTUALE_CPU, SERVER_VIRTUALE_IP FROM SERVER_VIRTUALI where SERVER_VIRTUALE_Id = @SERVER_VIRTUALE_Id", l_myConnection);
                    myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_Id", serverVirtualeCampo0.Text.ToString());

                    SqlDataReader myDataReader = myCommand.ExecuteReader();

                    if (myDataReader.Read())
                    {
                        if (myDataReader["SERVER_VIRTUALE_Nome"] != DBNull.Value)
                            ServerVirtualeNome.Text = myDataReader["SERVER_VIRTUALE_Nome"].ToString();

                        if (myDataReader["SERVER_VIRTUALE_RAM"] != DBNull.Value)
                            ServerVirtualeRam.Text = myDataReader["SERVER_VIRTUALE_RAM"].ToString();

                        if (myDataReader["SERVER_VIRTUALE_CPU"] != DBNull.Value)
                            ServerVirtualeCPU.Text = myDataReader["SERVER_VIRTUALE_CPU"].ToString();

                        if (myDataReader["SERVER_VIRTUALE_IP"] != DBNull.Value)
                            ServerVirtualeIP.Text = myDataReader["SERVER_VIRTUALE_IP"].ToString();

                        CaricaDatiStorageVirtuale(serverVirtualeCampo0.Text);

                    }

                }

            }
            finally
            {
                l_myConnection.Close();
            }
        }


        protected void updateButton_Click(object sender, EventArgs e)
        {

            if (campo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    SqlCommand myCommand, cmd;
                    myCommand = new SqlCommand("UPDATE HARDWARE SET HARDWARE_Seriale=@HARDWARE_Seriale, HARDWARE_DataIstallazione=@HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza=@HARDWARE_GaranziaScadenza, HARDWARE_Fornitore=@HARDWARE_Fornitore, HARDWARE_FatturaCliente=@HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore=@HARDWARE_FatturaFornitore, HARDWARE_HD=@HARDWARE_HD  WHERE (HARDWARE_Id = @HARDWARE_Id)", l_myConnection);
                    myCommand.Parameters.AddWithValue("@HARDWARE_Id", campo0.Text.ToString());

                    myCommand.Parameters.AddWithValue("@HARDWARE_Seriale", campo1.Text.ToString());
                    myCommand.Parameters.AddWithValue("@HARDWARE_DataIstallazione", campo2.Text);
                    myCommand.Parameters.AddWithValue("@HARDWARE_GaranziaScadenza", campo3.Text);
                    myCommand.Parameters.AddWithValue("@HARDWARE_Fornitore", campo4.Text.ToString());
                    myCommand.Parameters.AddWithValue("@HARDWARE_FatturaCliente", campo5.Text.ToString());
                    myCommand.Parameters.AddWithValue("@HARDWARE_FatturaFornitore", campo6.Text.ToString());
                    myCommand.Parameters.AddWithValue("@HARDWARE_HD", campo7.Text.ToString());

                    //ESEGUO LA QUERY
                    myCommand.ExecuteNonQuery();

                    if (FileUpload1.HasFile)
                    {

                        if (campo8.Text != "")
                        {

                            cmd = new SqlCommand("UPDATE FILES SET FILE_Nome=@FILE_Nome, FILE_B64=@FILE_B64, FILE_IdServizio=@FILE_IdServizio, FILE_IdLAN=@FILE_IdLAN, FILE_IdSoftware=@FILE_IdSoftware,FILE_IdServerFisico=@FILE_IdServerFisico,FILE_IdHardware=@FILE_IdHardware WHERE (FILE_IdHardware=@FILE_IdHardware)", l_myConnection);
                            cmd.Parameters.AddWithValue("@FILE_IdHardware", campo0.Text.ToString());
                            cmd.Parameters.AddWithValue("@FILE_Nome", FileUpload1.FileName);
                            cmd.Parameters.AddWithValue("@FILE_B64", FileUpload1.FileBytes);
                            cmd.Parameters.AddWithValue("@FILE_IdServizio", DBNull.Value);
                            cmd.Parameters.AddWithValue("@FILE_IdLAN", DBNull.Value);
                            cmd.Parameters.AddWithValue("@FILE_IdSoftware", DBNull.Value);
                            cmd.Parameters.AddWithValue("@FILE_IdServerFisico", DBNull.Value);

                            cmd.ExecuteNonQuery();

                        }
                        else if (campo8.Text == "")
                        {
                            cmd = new SqlCommand("INSERT INTO FILES VALUES (@FILE_Nome, @FILE_b64, @FILE_IdServizio, @FILE_IdLAN,@FILE_IdSoftware,@FILE_IdServerFisico,@FILE_IdHardware)", l_myConnection);

                            cmd.Parameters.AddWithValue("@FILE_Nome", FileUpload1.FileName);
                            cmd.Parameters.AddWithValue("@FILE_B64", FileUpload1.FileBytes);
                            cmd.Parameters.AddWithValue("@FILE_IdServizio", DBNull.Value);
                            cmd.Parameters.AddWithValue("@FILE_IdLAN", DBNull.Value);
                            cmd.Parameters.AddWithValue("@FILE_IdSoftware", DBNull.Value);
                            cmd.Parameters.AddWithValue("@FILE_IdServerFisico", DBNull.Value);
                            cmd.Parameters.AddWithValue("@FILE_IdHardware", campo0.Text.ToString());

                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                finally
                {
                    l_myConnection.Close();

                    da = new SqlDataAdapter("SELECT HARDWARE_ID, HARDWARE_Seriale, HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza, HARDWARE_Fornitore, HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore, HARDWARE_HD FROM HARDWARE WHERE HARDWARE_IdCliente=" + Convert.ToInt32(clientCampo0.Text), l_myConnection);
                    CaricaDati(da);
                }

            }
            else
            {
                try
                {
                    l_myConnection.Open();

                    SqlCommand myCommand;

                    if (FileUpload1.HasFile)
                    {
                        myCommand = new SqlCommand(@"
                                    DECLARE @NuovoId INT;
    
                                    INSERT INTO HARDWARE 
                                    VALUES (@HARDWARE_Seriale, @HARDWARE_DataIstallazione, @HARDWARE_GaranziaScadenza, 
                                            @HARDWARE_Fornitore, @HARDWARE_FatturaCliente, @HARDWARE_FatturaFornitore, 
                                            @HARDWARE_HD, @HARDWARE_IdTipologia, @HARDWARE_IdRivenditore, @HARDWARE_IdCliente);

                                    SET @NuovoId = SCOPE_IDENTITY();
    
                                    INSERT INTO FILES 
                                    VALUES (@FILE_Nome, @FILE_b64, @FILE_IdServizio, @FILE_IdLAN, @FILE_IdSoftware, 
                                            @FILE_IdServerFisico, @NuovoId);
                                ", l_myConnection);
                        myCommand.Parameters.AddWithValue("@HARDWARE_IdTipologia", 1);
                        myCommand.Parameters.AddWithValue("@HARDWARE_IdRivenditore", 1);
                        myCommand.Parameters.AddWithValue("@HARDWARE_IdCliente", clientCampo0.Text.ToString());

                        myCommand.Parameters.AddWithValue("@HARDWARE_Seriale", campo1.Text.ToString());
                        myCommand.Parameters.AddWithValue("@HARDWARE_DataIstallazione", campo2.Text);
                        myCommand.Parameters.AddWithValue("@HARDWARE_GaranziaScadenza", campo3.Text);
                        myCommand.Parameters.AddWithValue("@HARDWARE_Fornitore", campo4.Text.ToString());
                        myCommand.Parameters.AddWithValue("@HARDWARE_FatturaCliente", campo5.Text.ToString());
                        myCommand.Parameters.AddWithValue("@HARDWARE_FatturaFornitore", campo6.Text.ToString());
                        myCommand.Parameters.AddWithValue("@HARDWARE_HD", campo7.Text.ToString());

                        myCommand.Parameters.AddWithValue("@FILE_Nome", FileUpload1.FileName);
                        myCommand.Parameters.AddWithValue("@FILE_B64", FileUpload1.FileBytes);
                        myCommand.Parameters.AddWithValue("@FILE_IdServizio", DBNull.Value);
                        myCommand.Parameters.AddWithValue("@FILE_IdLAN", DBNull.Value);
                        myCommand.Parameters.AddWithValue("@FILE_IdSoftware", DBNull.Value);
                        myCommand.Parameters.AddWithValue("@FILE_IdServerFisico", DBNull.Value);

                        myCommand.ExecuteNonQuery();

                    }
                    else
                    {
                        myCommand = new SqlCommand("INSERT INTO HARDWARE VALUES(@HARDWARE_Seriale,@HARDWARE_DataIstallazione,@HARDWARE_GaranziaScadenza,@HARDWARE_Fornitore,@HARDWARE_FatturaCliente,@HARDWARE_FatturaFornitore,@HARDWARE_HD,@HARDWARE_IdTipologia,@HARDWARE_IdRivenditore,@HARDWARE_IdCliente);SELECT SCOPE_IDENTITY() AS NuovoId;", l_myConnection);
                        myCommand.Parameters.AddWithValue("@HARDWARE_IdTipologia", 1);
                        myCommand.Parameters.AddWithValue("@HARDWARE_IdRivenditore", 1);
                        myCommand.Parameters.AddWithValue("@HARDWARE_IdCliente", clientCampo0.Text.ToString());

                        myCommand.Parameters.AddWithValue("@HARDWARE_Seriale", campo1.Text.ToString());
                        myCommand.Parameters.AddWithValue("@HARDWARE_DataIstallazione", campo2.Text);
                        myCommand.Parameters.AddWithValue("@HARDWARE_GaranziaScadenza", campo3.Text);
                        myCommand.Parameters.AddWithValue("@HARDWARE_Fornitore", campo4.Text.ToString());
                        myCommand.Parameters.AddWithValue("@HARDWARE_FatturaCliente", campo5.Text.ToString());
                        myCommand.Parameters.AddWithValue("@HARDWARE_FatturaFornitore", campo6.Text.ToString());
                        myCommand.Parameters.AddWithValue("@HARDWARE_HD", campo7.Text.ToString());

                        myCommand.ExecuteNonQuery();
                    }
                }
                finally
                {
                    l_myConnection.Close();

                    da = new SqlDataAdapter("SELECT HARDWARE_ID, HARDWARE_Seriale, HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza, HARDWARE_Fornitore, HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore, HARDWARE_HD FROM HARDWARE WHERE HARDWARE_IdCliente=" + Convert.ToInt32(clientCampo0.Text), l_myConnection);
                    CaricaDati(da);
                }
            }
            pulisci_HW();
            HwPnlForm.Visible = false;
        }


        protected void pulisci_HW()
        {
            campo0.Text = "";
            campo1.Text = "";
            campo2.Text = "";
            campo3.Text = "";
            campo4.Text = "";
            campo5.Text = "";
            campo6.Text = "";
            campo7.Text = "";
            campo8.Text = "";
        }

        protected void pulisci_ServerFisico()
        {
            ServerFisicoNome.Text = "";
            ServerFisicoNote.Text = "";
            ServerFisicoSeriale.Text = "";
            ServerFisicoDataIstallazione.Text = "";
            ServerFisicoGaranziaScadenza.Text = "";
            ServerFisicoIP.Text = "";
            ServerFisicoHD.Text = "";
            ServerFisicoFornitore.Text = "";
            ServerFisicoFatturaCliente.Text = "";
            ServerFisicoFatturaFornitore.Text = "";
            ServerFisicoDominio.Text = "";
            ServerFisicoSubnet.Text = "";
            ServerFisicoGateway.Text = "";
            ServerFisicoHostNameMailServer.Text = "";
            ServerFisicoCampo0.Text = "";
        }

        protected void pulisci_ServerVirtuale()
        {
            serverVirtualeCampo0.Text = "";
            ServerVirtualeNome.Text = "";
            ServerVirtualeRam.Text = "";
            ServerVirtualeCPU.Text = "";
            ServerVirtualeIP.Text = "";
        }

        protected void pulisci_Storage()
        {
            storageCampo0.Text = "";
            StorageNome.Text = "";
            StorageCapacita.Text = "";
            StorageNote.Text = "";
        }


        protected void BtnCancelFile_Click(object sender, EventArgs e)
        {
            SqlCommand cmd;


            if (campo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    cmd = new SqlCommand("DELETE FROM FILES WHERE (FILE_IdHardware=@FILE_IdHardware)", l_myConnection);
                    cmd.Parameters.AddWithValue("@FILE_IdHardware", campo0.Text.ToString());
                    cmd.ExecuteNonQuery();

                }
                finally
                {
                    l_myConnection.Close();
                    da = new SqlDataAdapter("SELECT HARDWARE_ID, HARDWARE_Seriale, HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza, HARDWARE_Fornitore, HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore, HARDWARE_HD FROM HARDWARE WHERE HARDWARE_IdCliente=" + Convert.ToInt32(clientCampo0.Text), l_myConnection);

                    CaricaDati(da);
                }
            }
            else
            {
                l_myConnection.Close();
            }
            pulisci_HW();
            HwPnlForm.Visible = false;
        }
        protected void BtnDropRow_Click(object sender, EventArgs e)
        {


            if (campo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    delete_hardware(campo0.Text);
                }
                finally
                {
                    l_myConnection.Close();
                    da = new SqlDataAdapter("SELECT HARDWARE_ID, HARDWARE_Seriale, HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza, HARDWARE_Fornitore, HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore, HARDWARE_HD FROM HARDWARE WHERE HARDWARE_IdCliente=" + Convert.ToInt32(clientCampo0.Text), l_myConnection);

                    CaricaDati(da);
                }
            }
            else
            {
                l_myConnection.Close();
            }
            pulisci_HW();
            HwPnlForm.Visible = false;
        }

        private void delete_hardware(string hardware_Id)
        {

            SqlCommand myCommand, cmd;

            myCommand = new SqlCommand("SELECT FILE_Id FROM FILES WHERE FILE_IdHardware=@FILE_IdHardware", l_myConnection);
            myCommand.Parameters.AddWithValue("@FILE_IdHardware", hardware_Id);

            using (SqlDataReader myDataReader = myCommand.ExecuteReader())
            {
                if (myDataReader.Read() && myDataReader["FILE_Id"] != DBNull.Value)
                {
                    cmd = new SqlCommand("DELETE FROM FILES WHERE (FILE_IdHardware=@FILE_IdHardware)", l_myConnection);
                    cmd.Parameters.AddWithValue("@FILE_IdHardware", hardware_Id);
                    cmd.ExecuteNonQuery();
                }
            }


            myCommand = new SqlCommand("DELETE FROM HARDWARE WHERE HARDWARE_Id=@HARDWARE_Id", l_myConnection);
            myCommand.Parameters.AddWithValue("@HARDWARE_Id", hardware_Id);

            myCommand.ExecuteNonQuery();
        }

        protected void data_RowCommandStorage(Object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow selectedRow = gvStorage.Rows[index];


            if (e.CommandName == "ShowInfoStorage")
            {
                StorageNome.Text = "" + selectedRow.Cells[1].Text;
                StorageCapacita.Text = "" + selectedRow.Cells[2].Text;
                StorageNote.Text = "" + selectedRow.Cells[3].Text;
                storageCampo0.Text = "" + selectedRow.Cells[0].Text;
            }

            StoragePanel.Visible = true;
            DeleteStorage.Enabled = true;
            SaveStorage.Enabled = true;
        }

        protected void SaveStorage_Click(object sender, EventArgs e)
        {
            if (StorageCapacita.Text != "" && StorageNome.Text != "" && StorageNote.Text != "" && clientCampo0.Text != "" && ServerFisicoCampo0.Text != "")
            {
                int capacita;
                if (int.TryParse(StorageCapacita.Text, out capacita))  // Assicurati di fare il parsing corretto
                {
                    // Aggiungi il nuovo StorageFisico alla lista
                    this.StorageFisicoList.Add(new StorageFisico(StorageNome.Text, capacita, StorageNote.Text));
                    Session["StorageFisicoList"] = StorageFisicoList;
                }

                try
                {
                    SaveStorage.Enabled = true;
                    l_myConnection.Open();

                    SqlCommand myCommand;

                    foreach (StorageFisico storageFisico in this.StorageFisicoList)
                    {
                        if (storageCampo0.Text == "")
                        {
                            myCommand = new SqlCommand("INSERT INTO STORAGE VALUES(@STORAGE_Nome,@STORAGE_Capacita,@STORAGE_Note,@STORAGE_ServerFisicoId, @STORAGE_ServerVirtualeId)", l_myConnection);
                        }
                        else
                        {
                            myCommand = new SqlCommand("UPDATE STORAGE SET STORAGE_Nome=@STORAGE_Nome, STORAGE_Capacita=@STORAGE_Capacita, STORAGE_Note=@STORAGE_Note, STORAGE_ServerFisicoId=@STORAGE_ServerFisicoId, STORAGE_ServerVirtualeId=@STORAGE_ServerVirtualeId WHERE STORAGE_Id=@STORAGE_Id", l_myConnection);
                            myCommand.Parameters.AddWithValue("@STORAGE_Id", storageCampo0.Text.ToString());
                        }
                        myCommand.Parameters.AddWithValue("@STORAGE_Nome", storageFisico.Nome.ToString());
                        myCommand.Parameters.AddWithValue("@STORAGE_Capacita", storageFisico.Capacita.ToString());
                        myCommand.Parameters.AddWithValue("@STORAGE_Note", storageFisico.Note.ToString());
                        myCommand.Parameters.AddWithValue("@STORAGE_ServerFisicoId", ServerFisicoCampo0.Text.ToString());
                        myCommand.Parameters.AddWithValue("@STORAGE_ServerVirtualeId", DBNull.Value);


                        myCommand.ExecuteNonQuery();
                    }

                }
                finally
                {
                    l_myConnection.Close();
                    ///devo caricare i dati dello storage in modo adeguato
                    CaricaDatiStorage(ServerFisicoCampo0.Text.ToString());
                    StoragePanel.Visible = false;
                    Session["StorageFisicoList"] = null;
                }
                pulisci_Storage();
            }
        }

        protected void SaveServerFisico_Click(object sender, EventArgs e)
        {
            if (clientCampo0.Text != "")
            {
                
                try
                {
                    l_myConnection.Open();

                    SqlCommand myCommand;

                    if (ServerFisicoCampo0.Text == "")
                    {
                        myCommand = new SqlCommand(@"
                                    DECLARE @NuovoId INT ;
                                    INSERT INTO SERVER_FISICI 
                                    VALUES(@SERVER_FISICO_Nome, @SERVER_FISICO_Note, @SERVER_FISICO_Seriale, @SERVER_FISICO_DataIstallazione, 
                                           @SERVER_FISICO_GaranziaScadenza, @SERVER_FISICO_IP, @SERVER_FISICO_HD, @SERVER_FISICO_Fornitore,
                                           @SERVER_FISICO_FatturaCliente, @SERVER_FISICO_FatturaFornitore, @SERVER_FISICO_Dominio, @SERVER_FISICO_Subnet, 
                                           @SERVER_FISICO_Gateway, @SERVER_FISICO_HostnameMailServer,  @SERVER_FISICO_IdCliente, 
                                           @SERVER_FISICO_IdRivenditore, @SERVER_FISICO_IdTipologia);

                                    SET @NuovoId = SCOPE_IDENTITY();
    
                                    SELECT SERVER_FISICO_Id
                                    FROM SERVER_FISICI
                                    WHERE SERVER_FISICO_Id=@NuovoId;
                                ", l_myConnection
                        );
                    }
                    else
                    {
                        myCommand = new SqlCommand("UPDATE SERVER_FISICI SET SERVER_FISICO_Nome=@SERVER_FISICO_Nome, SERVER_FISICO_Note=@SERVER_FISICO_Note, SERVER_FISICO_Seriale=@SERVER_FISICO_Seriale, SERVER_FISICO_DataIstallazione=@SERVER_FISICO_DataIstallazione, SERVER_FISICO_GaranziaScadenza=@SERVER_FISICO_GaranziaScadenza, SERVER_FISICO_IP=@SERVER_FISICO_IP, SERVER_FISICO_HD=@SERVER_FISICO_HD, SERVER_FISICO_Fornitore=@SERVER_FISICO_Fornitore, SERVER_FISICO_FatturaCliente=@SERVER_FISICO_FatturaCliente, SERVER_FISICO_FatturaFornitore=@SERVER_FISICO_FatturaFornitore, SERVER_FISICO_Dominio=@SERVER_FISICO_Dominio, SERVER_FISICO_Subnet=@SERVER_FISICO_Subnet, SERVER_FISICO_Gateway=@SERVER_FISICO_Gateway, SERVER_FISICO_HostnameMailServer=@SERVER_FISICO_HostnameMailServer,  SERVER_FISICO_IdCliente=@SERVER_FISICO_IdCliente, SERVER_FISICO_IdRivenditore=@SERVER_FISICO_IdRivenditore, SERVER_FISICO_IdTipologia=@SERVER_FISICO_IdTipologia where SERVER_FISICO_Id=@SERVER_FISICO_Id", l_myConnection);
                        myCommand.Parameters.AddWithValue("@SERVER_FISICO_Id", ServerFisicoCampo0.Text.ToString());
                    }
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_Nome", ServerFisicoNome.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_Note", ServerFisicoNote.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_Seriale", ServerFisicoSeriale.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_DataIstallazione", ServerFisicoDataIstallazione.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_GaranziaScadenza", ServerFisicoGaranziaScadenza.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_IP", ServerFisicoIP.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_HD", ServerFisicoHD.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_Fornitore", ServerFisicoFornitore.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_FatturaCliente", ServerFisicoFatturaCliente.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_FatturaFornitore", ServerFisicoFatturaFornitore.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_Dominio", ServerFisicoDominio.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_Subnet", ServerFisicoSubnet.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_Gateway", ServerFisicoGateway.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_HostnameMailServer", ServerFisicoHostNameMailServer.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_IdCliente", clientCampo0.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_IdRivenditore", 2);
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_IdTipologia", 7);


                    if (ServerFisicoCampo0.Text == "")
                    {
                        using (SqlDataReader myDataReader = myCommand.ExecuteReader())
                        {
                            if (myDataReader.Read() && myDataReader["SERVER_FISICO_Id"] != DBNull.Value)
                            {
                                // Imposta il valore di ServerFisicoNome al valore dell'ID appena inserito
                                ServerFisicoCampo0.Text = myDataReader["SERVER_FISICO_Id"].ToString();

                                if (this.StorageFisicoList != null)
                                {
                                    for (int i = 0; i <= SSFRowCount; i++)
                                    {
                                        string nomeTextBoxId = "SSF_Nome" + i;
                                        string capacitaTextBoxId = "SSF_Capacita" + i;
                                        string noteTextBoxId = "SSF_Note" + i;

                                        TextBox txtNome = (TextBox)SSFph.FindControl(nomeTextBoxId);
                                        TextBox txtCapacita = (TextBox)SSFph.FindControl(capacitaTextBoxId);
                                        TextBox txtNote = (TextBox)SSFph.FindControl(noteTextBoxId);

                                        myCommand = new SqlCommand("INSERT INTO STORAGE VALUES(@STORAGE_Nome,@STORAGE_Capacita,@STORAGE_Note,@STORAGE_ServerFisicoId, @STORAGE_ServerVirtualeId)", l_myConnection);
                                        myCommand.Parameters.AddWithValue("@STORAGE_Nome", txtNome.Text.ToString());
                                        myCommand.Parameters.AddWithValue("@STORAGE_Capacita", txtCapacita.Text);
                                        myCommand.Parameters.AddWithValue("@STORAGE_Note", txtNote.Text.ToString());
                                        myCommand.Parameters.AddWithValue("@STORAGE_ServerFisicoId", ServerFisicoCampo0.Text.ToString());
                                        myCommand.Parameters.AddWithValue("@STORAGE_ServerVirtualeId", DBNull.Value);

                                        myCommand.ExecuteNonQuery();

                                    }
                                    Session["SSFRowCount"] = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        myCommand.ExecuteNonQuery();

                    }


                    da2 = new SqlDataAdapter("SELECT SERVER_FISICO_Id, SERVER_FISICO_Nome FROM SERVER_FISICI where SERVER_FISICO_IdCliente = " + clientCampo0.Text.ToString(), l_myConnection);

                }
                finally
                {
                    l_myConnection.Close();
                    ///devo caricare i dati dello storage in modo adeguato
                    CaricaDatiServerFisico(da2);
                    CaricaDatiStorage(ServerFisicoCampo0.Text);
                    CaricaDatiServerVirtuale(ServerFisicoCampo0.Text);

                    hideAll();
                    ServerFisicoUpdatePanel.Visible = true;
                    HwUpdatepanel.Visible = true;
                    StorageFisicoList = null;
                }
                //pulisci_ServerFisico();
            }
        }


        protected void btnToggleVisibility_Click(object sender, EventArgs e)
        {
            if (clientCampo0.Text != "")
            {
                pulisci_ServerFisico();
                SSF_Nome0.Text = "";
                SSF_Capacita0.Text = "";
                SSF_Note0.Text = "";
                ServerFisicoPanel.Visible = true;
                SaveServerFisico.Enabled = true;
                deleteServerFisico.Enabled = false;
                ServerFisicoCampo0.Text = "";
                ServerVirtualePanelForm.Visible = false;
                StoragePanel.Visible = false;
                SSF.Visible = true;
                ServerFisicoActionPanel.Visible = true;
                Session["StorageFisicoList"] = null;

            }
        }

        protected void btnToggleVisibilityStorage_Click(object sender, EventArgs e)
        {
            if (ServerFisicoCampo0.Text != "")
            {
                StoragePanel.Visible = true;
                DeleteStorage.Enabled = false;
                if (StorageNome.Text != "" && StorageCapacita.Text != "" && StorageNote.Text != "")
                {
                    int capacita;
                    if (int.TryParse(StorageCapacita.Text, out capacita))  // Assicurati di fare il parsing corretto
                    {
                        // Aggiungi il nuovo StorageFisico alla lista
                        this.StorageFisicoList.Add(new StorageFisico(StorageNome.Text, capacita, StorageNote.Text));
                        Session["StorageFisicoList"] = StorageFisicoList;
                    }
                    StorageNome.Text = "";
                    StorageNote.Text = "";
                    StorageCapacita.Text = "";
                }

            }
        }



        // Funzione che recupera i dati dal database
        private void GetDropdownItems()
        {
            List<string> items = new List<string>();

            try
            {

                // Usa i parametri SQL per evitare SQL Injection
                string query = "SELECT STORAGE_Nome FROM STORAGE WHERE STORAGE_ServerFisicoId = @ServerFisicoId";
                SqlCommand myCommand = new SqlCommand(query, l_myConnection);
                myCommand.Parameters.AddWithValue("@ServerFisicoId", ServerFisicoCampo0.Text);

                using (SqlDataReader myDataReader = myCommand.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        if (myDataReader["STORAGE_Nome"] != DBNull.Value)
                        {
                            // Crea un nuovo ListItem con il testo e il valore
                            items.Add(myDataReader["STORAGE_Nome"].ToString());

                            // Aggiungi l'item al DropDownList
                            //ServerVirtualeStorage.Items.Add(item);

                            // Incrementa l'indice
                        }
                    }
                }
            }
            finally
            {
                //.Items.Clear();

                foreach (var item in items)
                {
                    //ServerVirtualeStorage.Items.Add(new ListItem(item));
                }
            }

        }

        protected void SaveServerVirtuale_Click(object sender, EventArgs e)
        {
            if (clientCampo0.Text != "" && ServerFisicoCampo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    SqlCommand myCommand;


                    if (serverVirtualeCampo0.Text == "")
                    {
                        myCommand = new SqlCommand(@"
                                    DECLARE @NuovoId INT ;
                                    INSERT INTO SERVER_VIRTUALI 
                                    VALUES(@SERVER_VIRTUALE_Nome, @SERVER_VIRTUALE_RAM, @SERVER_VIRTUALE_CPU, @SERVER_VIRTUALE_IP, @SERVER_VIRTUALE_IdServerFisico);

                                    SET @NuovoId = SCOPE_IDENTITY();
    
                                    SELECT SERVER_VIRTUALE_Id
                                    FROM SERVER_VIRTUALI
                                    WHERE SERVER_VIRTUALE_Id=@NuovoId;
                                ", l_myConnection
                        );
                    }
                    else
                    {
                        myCommand = new SqlCommand("UPDATE SERVER_VIRTUALI SET SERVER_VIRTUALE_Nome=@SERVER_VIRTUALE_Nome, SERVER_VIRTUALE_RAM=@SERVER_VIRTUALE_RAM,  SERVER_VIRTUALE_CPU=@SERVER_VIRTUALE_CPU, SERVER_VIRTUALE_IP=@SERVER_VIRTUALE_IP, SERVER_VIRTUALE_IdServerFisico=@SERVER_VIRTUALE_IdServerFisico WHERE SERVER_VIRTUALE_Id=@SERVER_VIRTUALE_Id", l_myConnection);
                        myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_Id", serverVirtualeCampo0.Text.ToString());
                    }
                    myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_Nome", ServerVirtualeNome.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_RAM", ServerVirtualeRam.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_CPU", ServerVirtualeCPU.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_IP", ServerVirtualeIP.Text.ToString());
                    myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_IdServerFisico", ServerFisicoCampo0.Text.ToString());

                    if (serverVirtualeCampo0.Text == "")
                    {
                        using (SqlDataReader myDataReader = myCommand.ExecuteReader())
                        {
                            if (myDataReader.Read() && myDataReader["SERVER_VIRTUALE_Id"] != DBNull.Value)
                            {
                                // Imposta il valore di ServerFisicoNome al valore dell'ID appena inserito
                                serverVirtualeCampo0.Text = myDataReader["SERVER_VIRTUALE_Id"].ToString();

                                if (this.StorageVirtualeList != null)
                                {
                                    for (int i = 0; i <= SSVRowCount; i++)
                                    {
                                        string nomeTextBoxId = "SSV_Nome" + i;
                                        string capacitaTextBoxId = "SSV_Capacita" + i;
                                        string noteTextBoxId = "SSV_Note" + i;

                                        TextBox txtNome = (TextBox)SSVph.FindControl(nomeTextBoxId);
                                        TextBox txtCapacita = (TextBox)SSVph.FindControl(capacitaTextBoxId);
                                        TextBox txtNote = (TextBox)SSVph.FindControl(noteTextBoxId);

                                        myCommand = new SqlCommand("INSERT INTO STORAGE VALUES(@STORAGE_Nome,@STORAGE_Capacita,@STORAGE_Note,@STORAGE_ServerFisicoId, @STORAGE_ServerVirtualeId)", l_myConnection);
                                        myCommand.Parameters.AddWithValue("@STORAGE_Nome", txtNome.Text.ToString());
                                        myCommand.Parameters.AddWithValue("@STORAGE_Capacita", txtCapacita.Text);
                                        myCommand.Parameters.AddWithValue("@STORAGE_Note", txtNote.Text.ToString());
                                        myCommand.Parameters.AddWithValue("@STORAGE_ServerFisicoId", DBNull.Value);
                                        myCommand.Parameters.AddWithValue("@STORAGE_ServerVirtualeId", serverVirtualeCampo0.Text.ToString());
                                        myCommand.ExecuteNonQuery(); 

                                    }

                                    Session["SSVRowCount"] = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        myCommand.ExecuteNonQuery();

                    }

                }
                finally
                {
                    l_myConnection.Close();
                    ///devo caricare i dati dello storage in modo adeguato
                    CaricaDatiServerVirtuale(ServerFisicoCampo0.Text.ToString());
                    //CaricaDatiStorage(ServerFisicoCampo0.Text);

                    ServerVirtualePanelForm.Visible = false;
                    SSV.Visible = false;
                    ServerVirtualeActionPanel.Visible = false;
                    StorageVirtualeUpdatepanel.Visible = false;
                    StorageVirtualeList = null;
                }
                pulisci_ServerVirtuale();
            }
        }


        protected void deleteServerVirtuale_Click(object sender, EventArgs e)
        {
            if (serverVirtualeCampo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    delete_serverVirtuale(serverVirtualeCampo0.Text);

                }
                finally
                {
                    l_myConnection.Close();

                    CaricaDatiServerVirtuale(ServerFisicoCampo0.Text.ToString());
                    CaricaDatiStorageVirtuale(serverVirtualeCampo0.Text);
                }
            }
            pulisci_ServerVirtuale();
            ServerVirtualePanelForm.Visible = false;
            SSV.Visible = false;
            ServerVirtualeActionPanel.Visible = false;
            StorageVirtualeUpdatepanel.Visible = false;
        }


        private void delete_serverVirtuale(string serverVirtuale_Id)
        {

            //se ci sono dei storage virtuali, li sopprimo
            SqlCommand myCommand = new SqlCommand("SELECT STORAGE_Id FROM STORAGE where STORAGE_ServerVirtualeId=@STORAGE_ServerVirtualeId", l_myConnection);
            myCommand.Parameters.AddWithValue("@STORAGE_ServerVirtualeId", serverVirtuale_Id);

            //DA FARE
            SqlDataReader myDataReader2 = myCommand.ExecuteReader();

            while (myDataReader2.Read())
            {
                if (myDataReader2["STORAGE_Id"] != DBNull.Value)
                {
                    myCommand = new SqlCommand("DELETE FROM STORAGE WHERE STORAGE_Id=@STORAGE_Id", l_myConnection);
                    myCommand.Parameters.AddWithValue("@STORAGE_Id", myDataReader2["STORAGE_Id"]);
                    myCommand.ExecuteNonQuery();
                }

            }

            myCommand = new SqlCommand("DELETE FROM SERVER_VIRTUALI WHERE SERVER_VIRTUALE_Id=@SERVER_VIRTUALE_Id", l_myConnection);
            myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_Id", serverVirtuale_Id);

            myCommand.ExecuteNonQuery();
        }


        protected void btnToggleVisibilityServerVirtuale_Click(object sender, EventArgs e)
        {
            if (clientCampo0.Text != "" && ServerFisicoCampo0.Text != "")
            {
                pulisci_ServerVirtuale();
                SSV_Nome0.Text = "";
                SSV_Capacita0.Text = "";
                SSV_Note0.Text = "";
                ServerVirtualePanelForm.Visible = true;
                SaveServerVirtuale.Enabled = true;
                deleteServerVirtuale.Enabled = false;
                ServerVirtualeActionPanel.Visible = true;
                StorageVirtualeUpdatepanel.Visible = false;
                SSV.Visible = true;
            }
        }


        protected void deleteServerFisico_Click(object sender, EventArgs e)
        {
            if (ServerFisicoCampo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    delete_serverFisco(ServerFisicoCampo0.Text);

                    da2 = new SqlDataAdapter("SELECT SERVER_FISICO_Id, SERVER_FISICO_Nome FROM SERVER_FISICI where SERVER_FISICO_IdCliente = " + clientCampo0.Text.ToString(), l_myConnection);
                }
                finally
                {
                    l_myConnection.Close();

                }
            }
            CaricaDatiServerFisico(da2);
            CaricaDatiStorage(ServerFisicoCampo0.Text);
            CaricaDatiServerVirtuale(ServerFisicoCampo0.Text);
            pulisci_ServerFisico();
            hideAll();
            ServerFisicoUpdatePanel.Visible = true;
            HwUpdatepanel.Visible = true;
        }

        private void delete_serverFisco(string serverFisico_Id)
        {

            //se ci sono dei server virtuali, li sopprimo
            SqlCommand myCommand = new SqlCommand("SELECT SERVER_VIRTUALE_Id FROM SERVER_VIRTUALI where SERVER_VIRTUALE_IdServerFisico=@SERVER_VIRTUALE_IdServerFisico", l_myConnection);
            myCommand.Parameters.AddWithValue("@SERVER_VIRTUALE_IdServerFisico", serverFisico_Id);

            SqlDataReader myDataReader2 = myCommand.ExecuteReader();

            while (myDataReader2.Read())
            {
                if (myDataReader2["SERVER_VIRTUALE_Id"] != DBNull.Value)
                {
                    delete_serverVirtuale(myDataReader2["SERVER_VIRTUALE_Id"].ToString());
                }

            }

            // Se ci sono dei storage legati, li sopprimo
            myCommand = new SqlCommand("SELECT STORAGE_Id FROM STORAGE where STORAGE_ServerFisicoId=@STORAGE_ServerFisicoId", l_myConnection);
            myCommand.Parameters.AddWithValue("@STORAGE_ServerFisicoId", serverFisico_Id);

            SqlDataReader myDataReader = myCommand.ExecuteReader();

            while (myDataReader.Read())
            {
                if (myDataReader["STORAGE_Id"] != DBNull.Value)
                {
                    myCommand = new SqlCommand("DELETE FROM STORAGE WHERE STORAGE_Id=@STORAGE_Id", l_myConnection);
                    myCommand.Parameters.AddWithValue("@STORAGE_Id", myDataReader["STORAGE_Id"]);
                    myCommand.ExecuteNonQuery();
                }

            }

            myDataReader.Close();

            myCommand = new SqlCommand("DELETE FROM SERVER_FISICI WHERE SERVER_FISICO_Id=@SERVER_FISICO_Id", l_myConnection);
            myCommand.Parameters.AddWithValue("@SERVER_FISICO_Id", serverFisico_Id);

            myCommand.ExecuteNonQuery();
        }

        protected void DeleteStorage_Click(object sender, EventArgs e)
        {
            if (storageCampo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    SqlCommand myCommand = new SqlCommand("DELETE FROM STORAGE WHERE STORAGE_Id=@STORAGE_Id", l_myConnection);
                    myCommand.Parameters.AddWithValue("@STORAGE_Id", storageCampo0.Text);

                    myCommand.ExecuteNonQuery();
                }
                finally
                {
                    l_myConnection.Close();

                    CaricaDatiStorage(ServerFisicoCampo0.Text);
                    pulisci_Storage();
                }
            }
            StoragePanel.Visible = false;
        }

        protected void btnToggleVisibilityHw_Click(object sender, EventArgs e)
        {
            if (clientCampo0.Text != "")
            {
                pulisci_HW();
                HwPnlForm.Visible = true;
            }
        }


        /// Gestione Clienti
        private void CaricaDatiClienti()
        {
            try
            {
                l_myConnection.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT CLIENTE_Id, CLIENTE_Nome FROM CLIENTE", l_myConnection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvClientInfo.DataSource = dt;
                gvClientInfo.DataBind();
                //gvClientInfo.Columns[0].Visible = false;
            }
            finally
            {
                l_myConnection.Close();
            }

        }

        protected void btnToggleVisibilityClient_Click(object sender, EventArgs e)
        {
            hideAll();
            ClientPnlForm.Visible = true;
            clientCampo0.Text = "";
            clientNameField.Text = "";
            clientDelete.Enabled = false;
        }

        protected void gvClientInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Aggiungi un identificativo unico alla riga se necessario
                e.Row.Attributes["onclick"] = "this.style.backgroundColor='#FFFF00';";
            }*/
        }


        protected void ClientData_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow selectedRow = gvClientInfo.Rows[index];

            clientCampo0.Text = "" + selectedRow.Cells[0].Text;
            clientNameField.Text = "" + selectedRow.Cells[1].Text;
            HwUpdatepanel.Visible = true;
            ServerFisicoUpdatePanel.Visible = true;

            ServerVirtualeUpdatePanel.Visible = false;
            StorageUpdatePanel.Visible = false;
            StorageVirtualeUpdatepanel.Visible = false;
            StorageVirtualePanel.Visible = false;

            try
            {

                l_myConnection.Open();

            }
            finally
            {

                if (e.CommandName == "ShowHwServerF")
                {
                    pulisci_ServerFisico();
                    ServerFisicoPanel.Visible = false;
                    //StoragePanel.Visible = false;
                    pulisci_HW();
                    string query1 = "SELECT HARDWARE_ID, HARDWARE_Seriale, HARDWARE_DataIstallazione, HARDWARE_GaranziaScadenza, HARDWARE_Fornitore, HARDWARE_FatturaCliente, HARDWARE_FatturaFornitore, HARDWARE_HD FROM HARDWARE where HARDWARE_IdCliente = " + clientCampo0.Text;
                    da = new SqlDataAdapter(query1, l_myConnection);

                    string query2 = "SELECT SERVER_FISICO_Id, SERVER_FISICO_Nome FROM SERVER_FISICI  where SERVER_FISICO_IdCliente =" + clientCampo0.Text;
                    da2 = new SqlDataAdapter(query2, l_myConnection);
                    l_myConnection.Close();

                    CaricaDati(da);

                    CaricaDatiServerFisico(da2);
                }
                else if (e.CommandName == "EditClient")
                {
                    pulisci_ServerFisico();
                    pulisci_HW();
                    hideAll();

                    clientDelete.Enabled = true;
                    ClientPnlForm.Visible = true;
                }
            }
        }

        protected void clientUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                l_myConnection.Open();

                SqlCommand myCommand;


                if (clientCampo0.Text == "")
                {
                    myCommand = new SqlCommand("INSERT INTO CLIENTE VALUES(@CLIENTE_Nome)", l_myConnection);
                }
                else
                {
                    myCommand = new SqlCommand("UPDATE CLIENTE SET CLIENTE_Nome=@CLIENTE_Nome  WHERE (CLIENTE_Id = @CLIENTE_Id)", l_myConnection);
                    myCommand.Parameters.AddWithValue("@CLIENTE_Id", clientCampo0.Text.ToString());

                }

                myCommand.Parameters.AddWithValue("@CLIENTE_Nome", clientNameField.Text.ToString());

                //ESEGUO LA QUERY
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                l_myConnection.Close();

                ClientPnlForm.Visible = false;

                CaricaDatiClienti();

            }
            clientNameField.Text = "";
            clientCampo0.Text = "";
        }

        protected void clientDelete_Click(object sender, EventArgs e)
        {
            if (clientCampo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    SqlCommand myCommand = new SqlCommand("SELECT SERVER_FISICO_Id FROM SERVER_FISICI WHERE SERVER_FISICO_IdCliente= @SERVER_FISICO_IdCliente", l_myConnection);
                    myCommand.Parameters.AddWithValue("@SERVER_FISICO_IdCliente", clientCampo0.Text);

                    using (SqlDataReader myDataReader = myCommand.ExecuteReader())
                    {
                        while (myDataReader.Read())
                        {
                            if (myDataReader["SERVER_FISICO_Id"] != DBNull.Value)
                            {
                                delete_serverFisco(myDataReader["SERVER_FISICO_Id"].ToString());
                            }

                        }
                    }

                    myCommand = new SqlCommand("SELECT HARDWARE_Id FROM HARDWARE WHERE HARDWARE_IdCliente= @HARDWARE_IdCliente", l_myConnection);
                    myCommand.Parameters.AddWithValue("@HARDWARE_IdCliente", clientCampo0.Text);

                    SqlDataReader myDataReader2 = myCommand.ExecuteReader();

                    while (myDataReader2.Read())
                    {
                        if (myDataReader2["HARDWARE_Id"] != DBNull.Value)
                        {
                            delete_hardware(myDataReader2["HARDWARE_Id"].ToString());
                        }

                    }

                    myCommand = new SqlCommand("DELETE FROM CLIENTE WHERE CLIENTE_Id=@CLIENTE_Id", l_myConnection);
                    myCommand.Parameters.AddWithValue("@CLIENTE_Id", clientCampo0.Text);

                    myCommand.ExecuteNonQuery();
                }
                finally
                {
                    l_myConnection.Close();

                    CaricaDatiClienti();
                }
            }
            clientNameField.Text = "";
            clientCampo0.Text = "";
            ClientPnlForm.Visible = false;
        }


        private void CaricaDatiStorageVirtuale(string STORAGE_ServerVirtualeId)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT STORAGE_Id, STORAGE_Nome, STORAGE_Capacita, STORAGE_Note FROM STORAGE WHERE STORAGE_ServerVirtualeId=" + STORAGE_ServerVirtualeId, l_myConnection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvStorageVirtuale.DataSource = dt;
                gvStorageVirtuale.DataBind();
                //gvClientInfo.Columns[0].Visible = false;
            }
            finally
            {
                l_myConnection.Close();
            }

        }

        protected void data_RowCommandStorageVirtuale(Object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow selectedRow = gvStorageVirtuale.Rows[index];


            if (e.CommandName == "ShowInfoStorageVirtuale")
            {
                StorageVirtualeNome.Text = "" + selectedRow.Cells[1].Text;
                StorageVirtualeCapacita.Text = "" + selectedRow.Cells[2].Text;
                StorageVirtualeNote.Text = "" + selectedRow.Cells[3].Text;
                StorageVirtualeCampo0.Text = "" + selectedRow.Cells[0].Text;
            }

            StorageVirtualePanel.Visible = true;
            DeleteStorageVirtuale.Enabled = true;
            SaveStorageVirtuale.Enabled = true;
        }

        protected void SaveStorageVirtuale_Click(object sender, EventArgs e)
        {
            if (StorageVirtualeCapacita.Text != "" && StorageVirtualeNome.Text != "" && StorageVirtualeNote.Text != "" && clientCampo0.Text != "" && serverVirtualeCampo0.Text != "")
            {
                int capacita;
                if (int.TryParse(StorageVirtualeCapacita.Text, out capacita))  // Assicurati di fare il parsing corretto
                {
                    // Aggiungi il nuovo StorageFisico alla lista
                    this.StorageVirtualeList.Add(new StorageVirtuale(StorageVirtualeNome.Text, capacita, StorageVirtualeNote.Text));
                    Session["StorageVirtualeList"] = StorageVirtualeList;
                }
                try
                {
                    SaveStorageVirtuale.Enabled = true;
                    l_myConnection.Open();

                    SqlCommand myCommand;

                    foreach (StorageVirtuale storageVirtuale in this.StorageVirtualeList)
                    {
                        if (StorageVirtualeCampo0.Text == "")
                        {
                            myCommand = new SqlCommand("INSERT INTO STORAGE VALUES(@STORAGE_Nome,@STORAGE_Capacita,@STORAGE_Note,@STORAGE_ServerFisicoId, @STORAGE_ServerVirtualeId)", l_myConnection);
                        }
                        else
                        {
                            myCommand = new SqlCommand("UPDATE STORAGE SET STORAGE_Nome=@STORAGE_Nome, STORAGE_Capacita=@STORAGE_Capacita, STORAGE_Note=@STORAGE_Note, STORAGE_ServerFisicoId=@STORAGE_ServerFisicoId, STORAGE_ServerVirtualeId=@STORAGE_ServerVirtualeId WHERE STORAGE_Id=@STORAGE_Id", l_myConnection);
                            myCommand.Parameters.AddWithValue("@STORAGE_Id", StorageVirtualeCampo0.Text.ToString());
                        }

                        myCommand.Parameters.AddWithValue("@STORAGE_Nome", storageVirtuale.Nome.ToString());
                        myCommand.Parameters.AddWithValue("@STORAGE_Capacita", storageVirtuale.Capacita.ToString());
                        myCommand.Parameters.AddWithValue("@STORAGE_Note", storageVirtuale.Note.ToString());
                        myCommand.Parameters.AddWithValue("@STORAGE_ServerFisicoId", DBNull.Value);
                        myCommand.Parameters.AddWithValue("@STORAGE_ServerVirtualeId", serverVirtualeCampo0.Text.ToString());

                        myCommand.ExecuteNonQuery();
                    }

                }
                finally
                {
                    l_myConnection.Close();
                    ///devo caricare i dati dello storage in modo adeguato
                    CaricaDatiStorageVirtuale(serverVirtualeCampo0.Text.ToString());
                    StorageVirtualePanel.Visible = false;
                    Session["StorageVirtualeList"] = null;
                }
                pulisci_StorageVirtuale();
            }
        }

        protected void DeleteStorageVirtuale_Click(object sender, EventArgs e)
        {
            if (StorageVirtualeCampo0.Text != "")
            {
                try
                {
                    l_myConnection.Open();

                    SqlCommand myCommand = new SqlCommand("DELETE FROM STORAGE WHERE STORAGE_Id=@STORAGE_Id", l_myConnection);
                    myCommand.Parameters.AddWithValue("@STORAGE_Id", StorageVirtualeCampo0.Text);

                    myCommand.ExecuteNonQuery();
                }
                finally
                {
                    l_myConnection.Close();

                    CaricaDatiStorageVirtuale(serverVirtualeCampo0.Text);
                    pulisci_StorageVirtuale();
                }
            }
            StorageVirtualePanel.Visible = false;
        }

        protected void pulisci_StorageVirtuale()
        {
            StorageVirtualeCampo0.Text = "";
            StorageVirtualeNome.Text = "";
            StorageVirtualeCapacita.Text = "";
            StorageVirtualeNote.Text = "";
        }

        protected void btnToggleVisibilityStorageVirtuale_Click(object sender, EventArgs e)
        {
            if (serverVirtualeCampo0.Text != "")
            {
                StorageVirtualePanel.Visible = true;
                DeleteStorageVirtuale.Enabled = false;
                if (StorageVirtualeNome.Text != "" && StorageVirtualeCapacita.Text != "" && StorageVirtualeNote.Text != "")
                {
                    int capacita;
                    if (int.TryParse(StorageVirtualeCapacita.Text, out capacita))  // Assicurati di fare il parsing corretto
                    {
                        // Aggiungi il nuovo StorageFisico alla lista
                        this.StorageVirtualeList.Add(new StorageVirtuale(StorageVirtualeNome.Text, capacita, StorageVirtualeNote.Text));
                        Session["StorageVirtualeList"] = StorageVirtualeList;
                    }
                    StorageVirtualeNome.Text = "";
                    StorageVirtualeNote.Text = "";
                    StorageVirtualeCapacita.Text = "";
                }
            }
        }


        // Proprietà per gestire il conteggio dei controlli tramite Session
        protected int SSFRowCount
        {
            get
            {
                if (Session["SSFRowCount"] == null)
                    Session["SSFRowCount"] = 0;
                return (int)Session["SSFRowCount"];
            }
            set
            {
                Session["SSFRowCount"] = value;
            }
        }
        protected int SSVRowCount
        {
            get
            {
                if (Session["SSVRowCount"] == null)
                    Session["SSVRowCount"] = 0;
                return (int)Session["SSVRowCount"];
            }
            set
            {
                Session["SSVRowCount"] = value;
            }
        }

        protected int SVSFRowCount
        {
            get
            {
                if (Session["SVSFRowCount"] == null)
                    Session["SVSFRowCount"] = 0;
                return (int)Session["SVSFRowCount"];
            }
            set
            {
                Session["SVSFRowCount"] = value;
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            // Ricrea i controlli dinamici ad ogni postback utilizzando il valore memorizzato in Session
            for (int i = 1; i <= SSFRowCount; i++)
            {
                AddStorageRow(i, "SSF");
            }
            for (int i = 1; i <= SSVRowCount; i++)
            {
                AddStorageRow(i, "SSV");
            }
            for (int i = 1; i <= SVSFRowCount; i++)
            {
                AddStorageRow(i, "SVSF");
            }
        }

        protected void AddSSF_Button_Click(object sender, EventArgs e)
        {
            // Incrementa il contatore salvato nella Sessione
            SSFRowCount++;
            AddStorageRow(SSFRowCount, "SSF");
        }

        protected void AddSSV_Button_Click(object sender, EventArgs e)
        {
            // Incrementa il contatore salvato nella Sessione
            SSVRowCount++;
            AddStorageRow(SSVRowCount, "SSV");
        }

        protected void AddSVSF_Button_Click(object sender, EventArgs e)
        {
            // Incrementa il contatore salvato nella Sessione
            SSVRowCount++;
            AddServerRow(SSVRowCount, "SVSF");
        }

        private void AddStorageRow(int index, string ID_base)
        {

            ColorConverter colorConverter = new ColorConverter();
            Color red = (Color)colorConverter.ConvertFromString("Red");

            // Crea un Panel per rappresentare una riga dinamica (equivalente a un div con classe "form-row")
            Panel pnlRow = new Panel();
            pnlRow.CssClass = "form-row";

            // Blocco per "Nome Storage"
            Panel pnlNome = new Panel();
            //pnlNome.Controls.Add(new LiteralControl("<div>"));
            pnlNome.Controls.Add(new LiteralControl($"<label for='{ID_base}_Nome{index}'>Nome Storage: </label>"));
            TextBox txtNome = new TextBox();
            txtNome.ID = ID_base + "_Nome" + index;
            pnlNome.Controls.Add(txtNome);
            RequiredFieldValidator validatorNome = new RequiredFieldValidator();
            validatorNome.ID = txtNome.ID + "Validator";
            validatorNome.ControlToValidate = txtNome.ID;
            validatorNome.ErrorMessage = "Inserisci il nome dello storage prima";
            validatorNome.ForeColor = red;
            validatorNome.Display = ValidatorDisplay.Dynamic;
            validatorNome.ValidationGroup = "Group2";
            pnlNome.Controls.Add(validatorNome);
            //pnlNome.Controls.Add(new LiteralControl("</div>"));


            // Blocco per "Capacità"
            Panel pnlCapacita = new Panel();
            pnlCapacita.Controls.Add(new LiteralControl($"<label for='{ID_base}_Capacita{index}'>Capacità: </label>"));
            TextBox txtCapacita = new TextBox();
            txtCapacita.ID = ID_base + "_Capacita" + index;
            pnlCapacita.Controls.Add(txtCapacita);
            RequiredFieldValidator validatorCapacita = new RequiredFieldValidator();
            validatorCapacita.ID = txtCapacita.ID + "Validator";
            validatorCapacita.ControlToValidate = txtCapacita.ID;
            validatorCapacita.ErrorMessage = "Inserisci la capacita dello storage";
            validatorCapacita.ForeColor = red;
            validatorCapacita.Display = ValidatorDisplay.Dynamic;
            validatorCapacita.ValidationGroup = "Group2";
            pnlCapacita.Controls.Add(validatorCapacita);

            // Blocco per "Note"
            Panel pnlNote = new Panel();
            pnlNote.Controls.Add(new LiteralControl($"<label for='{ID_base}_Note{index}'>Note: </label>"));
            TextBox txtNote = new TextBox();
            txtNote.ID = ID_base + "_Note" + index;
            pnlNote.Controls.Add(txtNote);
            RequiredFieldValidator validatorNote = new RequiredFieldValidator();
            validatorNote.ID = txtNote.ID + "Validator";
            validatorNote.ControlToValidate = txtNote.ID;
            validatorNote.ErrorMessage = "Inserisci le note";
            validatorNote.ForeColor = red;
            validatorNote.Display = ValidatorDisplay.Dynamic;
            validatorNote.ValidationGroup = "Group2";
            pnlNote.Controls.Add(validatorNote);

            // Aggiunge i tre blocchi al pannello della riga
            pnlRow.Controls.Add(pnlNome);
            pnlRow.Controls.Add(pnlCapacita);
            pnlRow.Controls.Add(pnlNote);

            // Aggiunge la riga al PlaceHolder
            SSFph.Controls.Add(pnlRow);
        }

        private void AddServerRow(int index, string ID_base)
        {

            ColorConverter colorConverter = new ColorConverter();
            Color red = (Color)colorConverter.ConvertFromString("Red");

            // Crea un Panel per rappresentare una riga dinamica (equivalente a un div con classe "form-row")
            Panel pnlRow = new Panel();
            pnlRow.CssClass = "form-row";

            // Blocco per "Nome Server"
            Panel pnlNome = new Panel();
            pnlNome.Controls.Add(new LiteralControl($"<label for='{ID_base}_Nome{index}'>Nome Storage: </label>"));
            TextBox txtNome = new TextBox();
            txtNome.ID = ID_base + "_Nome" + index;
            pnlNome.Controls.Add(txtNome);
            RequiredFieldValidator validatorNome = new RequiredFieldValidator();
            validatorNome.ID = txtNome.ID + "Validator";
            validatorNome.ControlToValidate = txtNome.ID;
            validatorNome.ErrorMessage = "Inserisci il nome del server virtuale";
            validatorNome.ForeColor = red;
            validatorNome.Display = ValidatorDisplay.Dynamic;
            validatorNome.ValidationGroup = "Group2";
            pnlNome.Controls.Add(validatorNome);


            // Blocco per "RAM"
            Panel pnlRAM = new Panel();
            pnlRAM.Controls.Add(new LiteralControl($"<label for='{ID_base}_RAM{index}'>RAM: </label>"));
            TextBox txtRAM = new TextBox();
            txtRAM.ID = ID_base + "RAM" + index;
            pnlRAM.Controls.Add(txtRAM);
            RequiredFieldValidator validatorRAM = new RequiredFieldValidator();
            validatorRAM.ID = txtRAM.ID + "Validator";
            validatorRAM.ControlToValidate = txtRAM.ID;
            validatorRAM.ErrorMessage = "Inserisci la RAM del server virtuale";
            validatorRAM.ForeColor = red;
            validatorRAM.Display = ValidatorDisplay.Dynamic;
            validatorRAM.ValidationGroup = "Group2";
            pnlRAM.Controls.Add(validatorRAM);

            // Blocco per "CPU"
            Panel pnlCPU = new Panel();
            pnlCPU.Controls.Add(new LiteralControl($"<label for='{ID_base}_CPU{index}'>CPU: </label>"));
            TextBox txtCPU = new TextBox();
            txtCPU.ID = ID_base + "_CPU" + index;
            pnlCPU.Controls.Add(txtCPU);
            RequiredFieldValidator validatorCPU = new RequiredFieldValidator();
            validatorCPU.ID = txtCPU.ID + "Validator";
            validatorCPU.ControlToValidate = txtCPU.ID;
            validatorCPU.ErrorMessage = "Inserisci la CPU";
            validatorCPU.ForeColor = red;
            validatorCPU.Display = ValidatorDisplay.Dynamic;
            validatorCPU.ValidationGroup = "Group2";
            pnlCPU.Controls.Add(validatorCPU);

            // Blocco per "IP"
            Panel pnlIP = new Panel();
            pnlIP.Controls.Add(new LiteralControl($"<label for='{ID_base}_IP{index}'>IP: </label>"));
            TextBox txtIP = new TextBox();
            txtIP.ID = ID_base + "_IP" + index;
            pnlCPU.Controls.Add(txtCPU);
            RequiredFieldValidator validatorIP = new RequiredFieldValidator();
            validatorIP.ID = txtCPU.ID + "Validator";
            validatorIP.ControlToValidate = txtCPU.ID;
            validatorIP.ErrorMessage = "Inserisci l'IP del server";
            validatorIP.ForeColor = red;
            validatorIP.Display = ValidatorDisplay.Dynamic;
            validatorIP.ValidationGroup = "Group2";
            pnlIP.Controls.Add(validatorIP); 

            // Aggiunge i tre blocchi al pannello della riga
            pnlRow.Controls.Add(pnlNome);
            pnlRow.Controls.Add(pnlRAM);
            pnlRow.Controls.Add(pnlCPU);
            PN

            // Aggiunge la riga al PlaceHolder
            SSFph.Controls.Add(pnlRow);
        }

    }
}