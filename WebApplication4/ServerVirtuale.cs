using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4
{
    public class ServerVirtuale
    {
        // Proprietà della classe
        public string Nome { get; set; }
        public string RAM { get; set; }
        public string CPU { get; set; }
        public string IP { get; set; }


        // Costruttore
        public ServerVirtuale(string nome, string ram, string cpu, string ip)
        {
            Nome = nome;
            RAM = ram;
            CPU = cpu;
            IP = ip;
        }

        // Metodo della classe
        public void Saluta()
        {

        }
    }
}