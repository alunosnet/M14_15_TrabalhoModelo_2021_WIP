using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M14_15_TrabalhoModelo_2021_WIP.Leitores
{
    class Leitor
    {
        public int nleitor { get; set; }
        public string nome { get; set; }
        public DateTime data_nascimento { get; set; }
        public byte[] fotografia { get; set; }
        public bool estado { get; set; }

        public Leitor(int nleitor, string nome, DateTime data_nascimento, byte[] fotografia, bool estado)
        {
            this.nleitor = nleitor;
            this.nome = nome;
            this.data_nascimento = data_nascimento;
            this.fotografia = fotografia;
            this.estado = estado;
        }
        public void Adicionar(BaseDados bd)
        {
            string sql = $@"INSERT INTO Leitores(nome,data_nasc,fotografia,estado)
                        VALUES (@nome,@data_nasc,@fotografia,@estado)";

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nome",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.nome
                },
                new SqlParameter()
                {
                    ParameterName="@data_nasc",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_nascimento
                },
                new SqlParameter()
                {
                    ParameterName="@fotografia",
                    SqlDbType=System.Data.SqlDbType.VarBinary,
                    Value=this.fotografia
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=true
                },
            };
            bd.executaSQL(sql, parametros);
        }

        public static List<Leitor> ListarTodos(BaseDados bd)
        {
            var dados = bd.devolveSQL("SELECT * FROM Leitores");
            List<Leitor> lista = new List<Leitor>();
            foreach(DataRow linha in dados.Rows)
            {
                int nleitor = int.Parse(linha["nleitor"].ToString());
                string nome = linha["nome"].ToString();
                DateTime data = DateTime.Parse(linha["data_nasc"].ToString());
                byte[] fotografia = (byte[])linha["fotografia"];
                bool estado = bool.Parse(linha["estado"].ToString());
                Leitor novo = new Leitor(nleitor, nome, data, fotografia, estado);
                lista.Add(novo);
            }
            return lista;
        }
        public static List<Leitor> ListarTodos(BaseDados bd,int primeiro,
            int ultimo)
        {
            string sql = $@"SELECT nleitor,nome,data_nasc,fotografia,estado
                            FROM (
                                SELECT row_number() over (order by nleitor) as num,
                                nleitor,nome,data_nasc,fotografia,estado FROM Leitores) as p
                            WHERE num>={primeiro} and num<={ultimo}";

            var dados = bd.devolveSQL(sql);

            List<Leitor> lista = new List<Leitor>();
            foreach (DataRow linha in dados.Rows)
            {
                int nleitor = int.Parse(linha["nleitor"].ToString());
                string nome = linha["nome"].ToString();
                DateTime data = DateTime.Parse(linha["data_nasc"].ToString());
                byte[] fotografia = (byte[])linha["fotografia"];
                bool estado = bool.Parse(linha["estado"].ToString());
                Leitor novo = new Leitor(nleitor, nome, data, fotografia, estado);
                lista.Add(novo);
            }
            return lista;
        }
        public static int NrLeitores(BaseDados bd)
        {
            DataTable dados = bd.devolveSQL("select count(*) from leitores");
            int nr=int.Parse(dados.Rows[0][0].ToString());
            return nr;
        }
        public static void Remover(BaseDados bd,int id)
        {
            string sql = "DELETE FROM Leitores WHERE nleitor=" + id;
            bd.executaSQL(sql);
        }
        public void Atualizar(BaseDados bd)
        {
            string sql = $@"UPDATE Leitores
                        SET nome=@nome,data_nasc=@data_nasc,
                        fotografia=@fotografia
                        WHERE nleitor=@nleitor";

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nome",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.nome
                },
                new SqlParameter()
                {
                    ParameterName="@data_nasc",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_nascimento
                },
                new SqlParameter()
                {
                    ParameterName="@fotografia",
                    SqlDbType=System.Data.SqlDbType.VarBinary,
                    Value=this.fotografia
                },
                new SqlParameter()
                {
                    ParameterName="@nleitor",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=nleitor
                },
            };
            bd.executaSQL(sql, parametros);
        }

        public override string ToString()
        {
            return nome;
        }
    }
}
