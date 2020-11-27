using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M14_15_TrabalhoModelo_2021_WIP.Emprestimos
{
    class Emprestimo
    {
        public int nemprestimo { get; set; }
        public int nleitor { get; set; }
        public int nlivro { get; set; }
        public DateTime data_emprestimo { get; set; }
        public DateTime data_devolve { get; set; }
        public bool estado { get; set; }

        public Emprestimo(int nleitor, int nlivro)
        {
            this.nleitor = nleitor;
            this.nlivro = nlivro;
            this.estado = true;
            this.data_emprestimo = DateTime.Now;
        }

        public Emprestimo(int nemprestimo,int nleitor, int nlivro, DateTime data) : this(nleitor, nlivro)
        {
            this.data_devolve = data;
            this.nemprestimo = nemprestimo;

        }

        public void Adicionar(BaseDados bd)
        {
            string sql = "";
            //iniciar transação
            SqlTransaction transacao = bd.iniciarTransacao();
            //registar o empréstimo
            sql = $"INSERT INTO Emprestimos(nlivro,nleitor,data_emprestimo,estado)" +
                $" VALUES (@nlivro,@nleitor,@data_emprestimo,1)";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nlivro",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.nlivro
                },
                new SqlParameter()
                {
                    ParameterName="@nleitor",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.nleitor
                },
                new SqlParameter()
                {
                    ParameterName="@data_emprestimo",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_emprestimo
                }
            };
            bd.executaSQL(sql, parametros,transacao);
            //atualizar o estado do livro
            sql = "UPDATE Livros SET estado=0 WHERE nlivro=" + this.nlivro;
       
            bd.executaSQL(sql,null,transacao);
            //terminar transação
            transacao.Commit();
        }

        internal static IEnumerable ListaEmprestimosPorConcluir(BaseDados bd)
        {
            string sql = "SELECT * FROM Emprestimos where estado=1 or data_devolve is null";
            DataTable dados = bd.devolveSQL(sql);
            List<Emprestimo> lista = new List<Emprestimo>();
            foreach(DataRow linha in dados.Rows)
            {
                int nemprestimo = int.Parse(linha["nemprestimo"].ToString());
                int nleitor = int.Parse(linha["nleitor"].ToString());
                int nlivro = int.Parse(linha["nlivro"].ToString());
                DateTime data = DateTime.Parse(linha["data_emprestimo"].ToString());
                Emprestimo emp = new Emprestimo(nemprestimo, nleitor, nlivro, data);
                lista.Add(emp);
            }
            return lista;
        }

        public void Receber(BaseDados bd)
        {
            string sql = "";
            
            //iniciar transação
            SqlTransaction transacao = bd.iniciarTransacao();
            //atualizar o estado do empréstimo
            sql = $"UPDATE Emprestimos SET estado=0, data_devolve=@data" +
                $" WHERE nemprestimo=@nemprestimo";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@data",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=DateTime.Now
                },
                new SqlParameter()
                {
                    ParameterName="@nemprestimo",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.nemprestimo
                }
                
            };
            bd.executaSQL(sql, parametros, transacao);
            //atualizar o estado do livro
            sql = "UPDATE Livros SET estado=1 WHERE nlivro=" + this.nlivro;

            bd.executaSQL(sql, null, transacao);
            //terminar transação
            transacao.Commit();
        }
    }
}
