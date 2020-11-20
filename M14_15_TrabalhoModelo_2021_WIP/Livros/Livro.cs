using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M14_15_TrabalhoModelo_2021_WIP.Livros
{
    class Livro
    {
        public int nlivro { get; set; }
        public string nome { get; set; }
        public int ano { get; set; }
        public DateTime data_aquisicao { get; set; }
        public decimal preco { get; set; }
        public string capa { get; set; }
        public bool estado { get; set; }

        public Livro(int nlivro, string nome, int ano, DateTime data_aquisicao, decimal preco, string capa, bool estado)
        {
            this.nlivro = nlivro;
            this.nome = nome;
            this.ano = ano;
            this.data_aquisicao = data_aquisicao;
            this.preco = preco;
            this.capa = capa;
            this.estado = estado;
        }

        //Adicionar
        public void Adicionar(BaseDados bd)
        {
            string sql = $@"INSERT INTO Livros(nome,ano,
                    data_aquisicao,preco,capa,estado) VALUES
                (@nome,@ano,@data_aquisicao,@preco,@capa,@estado)";
            //parametros
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
                    ParameterName="@ano",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.ano
                },
                new SqlParameter()
                {
                    ParameterName="@data_aquisicao",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_aquisicao
                },
                new SqlParameter()
                {
                    ParameterName="@preco",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.preco
                },
                new SqlParameter()
                {
                    ParameterName="@capa",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.capa
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=this.estado
                },
            };
            //executar
            bd.executaSQL(sql, parametros);
        }
        //remover
        public static void Remover(BaseDados bd,int id)
        {
            string sql = "DELETE FROM Livros WHERE nlivro=" + id;
            bd.executaSQL(sql);
        }
        //TODO:atualizar

        //TODO:get

        //listar todos
        public static List<Livro> ListaTodosLivros(BaseDados bd,bool ordenar=false)
        {
            string sql = "SELECT * FROM Livros";
            if (ordenar)
                sql += " ORDER BY nome";

            DataTable dados=bd.devolveSQL(sql);
            List<Livro> lista = new List<Livro>();
            foreach(DataRow linha in dados.Rows)
            {
                int nlivro = int.Parse(linha["nlivro"].ToString());
                string nome = linha["nome"].ToString();
                int ano = int.Parse(linha["ano"].ToString());
                DateTime data = DateTime.Parse(linha["data_aquisicao"].ToString());
                decimal preco = Decimal.Parse(linha["preco"].ToString());
                string capa = linha["capa"].ToString();
                bool estado = bool.Parse(linha["estado"].ToString());
                Livro novo = new Livro(nlivro,nome,ano,data,preco,capa,estado);
                lista.Add(novo);
            }
            return lista;
        }
        //TODO:listar disponíveis

        //TODO:pesquisar por nome

        //TODO:tostring

    }
}
