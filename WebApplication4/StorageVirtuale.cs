using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4
{
    public class StorageVirtuale
    {
        // Proprietà della classe
        public string Nome { get; set; }
        public int Capacita { get; set; }
        public string Note { get; set; }


        // Costruttore
        public StorageVirtuale(string nome, int capacita, string note)
        {
            Nome = nome;
            Capacita = capacita;
            Note = note;
        }

        // Metodo della classe
        public void Saluta()
        {

        }
    }
}