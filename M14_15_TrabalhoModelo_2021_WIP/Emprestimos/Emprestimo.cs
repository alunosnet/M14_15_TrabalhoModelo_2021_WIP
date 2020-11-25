using System;
using System.Collections.Generic;
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
        public void Adicionar(BaseDados bd)
        {
            string sql = "";
            //iniciar transação
            SqlTransaction transacao = bd.iniciarTransacao();
            //registar o empréstimo
            sql = $"INSERT INTO Emprestimos(nlivro,nleitor,data_emprestimo)" +
                $" VALUES (@nlivro,@nleitor,@data_emprestimo)";
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
            //TODO: null
            bd.executaSQL(sql,null,transacao);
            //terminar transação
            transacao.Commit();
        }
    }
}
