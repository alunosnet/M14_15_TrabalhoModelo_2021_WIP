using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace M14_15_TrabalhoModelo_2021_WIP
{
    class BaseDados
    {
        string BDName = "M14_15_TrabalhModelo_2021";
        string caminho;
        string strLigacao;
        SqlConnection ligacaoBD;

        public BaseDados()
        {
            //se a bd existe
            //definir o caminho para bd
            caminho = Utils.pastaDoPrograma() + @"\" + BDName + ".mdf";
            if (File.Exists(caminho) == false)
                criarBD();

            //atualizar a string ligação
            //abrir a ligação a bd

        }

        private void criarBD()
        {
            
        }

        ~BaseDados()
        {

        }
    }
}
