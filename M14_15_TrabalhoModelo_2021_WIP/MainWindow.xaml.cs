using M14_15_TrabalhoModelo_2021_WIP.Emprestimos;
using M14_15_TrabalhoModelo_2021_WIP.Leitores;
using M14_15_TrabalhoModelo_2021_WIP.Livros;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M14_15_TrabalhoModelo_2021_WIP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BaseDados bd = new BaseDados();
        public MainWindow()
        {
            InitializeComponent();


        }

        private void Menu_Sair_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Menu_Livros_Click(object sender, RoutedEventArgs e)
        {
            j_livros j = new j_livros(bd);
            j.Show();
        }

        private void Menu_Leitores_Click(object sender, RoutedEventArgs e)
        {
            j_leitores j = new j_leitores(bd);
            j.Show();
        }

        private void Menu_Emprestimos_Click(object sender, RoutedEventArgs e)
        {
            j_emprestimos j = new j_emprestimos(bd);
            j.Show();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sql = "";
            switch (cbConsultas.SelectedIndex)
            {
                case 0:
                    sql = $@"SELECT TOP 3 livros.nome, count(nemprestimo) as [Nr de empréstimos]
                            FROM Livros LEFT JOIN Emprestimos
                            ON Livros.nlivro=Emprestimos.nlivro
                            GROUP BY Livros.nlivro,Livros.nome
                            ORDER BY count(nemprestimo)"; break;
                case 1:
                    sql = $@"SELECT count(*) as [Nr Livros],month(data_aquisicao) as Mês
                            FROM Livros
                            GROUP BY month(data_aquisicao)"; break;
                case 2:
                    sql = $@"SELECT TOP 1 count(*) as [Nr Empréstimos],month(data_emprestimo) as Mês
                            FROM Emprestimos
                            GROUP BY month(data_emprestimo)
                            ORDER BY [Nr Empréstimos] DESC"; break;
                case 3:
                    sql = $@"SELECT AVG(datediff(day,data_devolve,data_emprestimo)) as [Média de duração dos empréstimos]
                            FROM Emprestimos"; break;
                case 4:
                    sql = $@"SELECT nome, AVG(datediff(day,data_devolve,data_emprestimo)) as [Média de duração dos empréstimos]
                            FROM Leitores LEFT JOIN Emprestimos
                            ON emprestimos.nleitor=leitores.nleitor
                            GROUP BY leitores.nleitor,nome"; break;
                case 5:
                    /*sql = $@"SELECT Nome,livros.nlivro,nemprestimo
                            FROM Livros LEFT JOIN Emprestimos
                            ON livros.nlivro=emprestimos.nlivro
                            WHERE nemprestimo is null"; break;*/
                    sql = $@"SELECT * FROM LIVROS 
                            WHERE nlivro not in (SELECT DISTINCT nlivro 
                            FROM emprestimos)"; break;
                case 6:
                    sql = $@"SELECT nome,preco
                            FROM Livros WHERE preco>(select avg(preco) from livros)"; break;
                case 7:
                    sql = $@"SELECT livros.nome,data_nasc
                            FROM Livros INNER JOIN Emprestimos
                            ON Livros.nlivro=emprestimos.nlivro
                            INNER JOIN Leitores
                            ON emprestimos.nleitor=leitores.nleitor
                            WHERE livros.estado=0 AND year(data_nasc)>=1970 AND 
                            year(data_nasc)<=1979"; break;
                case 8:
                    sql = $@"SELECT livros.nome,leitores.nome
                            FROM Livros INNER JOIN Emprestimos
                            ON Livros.nlivro=emprestimos.nlivro
                            INNER JOIN Leitores
                            ON emprestimos.nleitor=leitores.nleitor
                            WHERE leitores.nome like 'j%' AND livros.estado=0"; 
                    break;
                case 9:
                    sql = $@"SELECT count(*)
                            FROM Livros 
                            WHERE estado=0"; 
                    break;
                case 10:
                    sql = $@"SELECT TOP 1 nome,datediff(day,data_emprestimo,getdate()) as [Duração]
                            FROM Livros INNER JOIN EMPRESTIMOS
                            ON livros.nlivro=emprestimos.nlivro
                            WHERE livros.estado=0
                            ORDER BY [Duração] DESC"; 
                    break;
                case 11:
                    sql = $@"SELECT livros.nome,leitores.nome,datediff(day,data_emprestimo,getdate())
                            FROM Livros inner join emprestimos
                            on livros.nlivro=emprestimos.nlivro
                            inner join leitores
                            on leitores.nleitor=emprestimos.nleitor
                            WHERE livros.estado=0 and
                            datediff(day,data_emprestimo,getdate())>7"; 
                        break;
                case 12:
                    sql = $@"SELECT TOP 3 leitores.nome, count(nemprestimo) as [Nr de empréstimos]
                            FROM Leitores INNER JOIN Emprestimos
                            ON Leitores.nleitor=Emprestimos.nleitor
                            GROUP BY leitores.nleitor,leitores.nome
                            ORDER BY count(nemprestimo) DESC"; break;
                case 13:
                    sql = $@"SELECT nome
                               FROM Leitores INNER JOIN Emprestimos
                            on Emprestimos.nleitor=leitores.nleitor
                        WHERE Emprestimos.estado=1 Or Data_devolve is null"; break;

            }
            DataTable dados = bd.devolveSQL(sql);
            DGConsultas.DataContext = dados.DefaultView;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }
        
    }
}
