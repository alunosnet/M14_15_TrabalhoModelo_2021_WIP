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
            DataTable dados = bd.devolveSQL("SELECT * FROM Leitores");
            DGConsultas.DataContext = dados.DefaultView;

        }
        /*
         Top Livros - Francisco
        Numero de livros comprados por mes - Duarte
        Mes com mais emprestimos - Firmino
        Media da duracao dos emprestimos em dias - Faure
        Media da duracao dos emprestimos em dias por leitor incluir todos os leitores- Carlos
        Lista dos livros nunca emprestados - Gonçalo
        Lista dos livros cujo preco é superior a media dos precos - Guilherme Leite
        Lista dos livros emprestados aos leitores que nasceram na decada de 1970 - Eduardo
        Nome dos livros emprestados aos leitores cujo nome comeca por J - Filipe

 

        Numero de livros registados e emprestados 
        Nome do livro emprestado a mais tempo
        Lista dos livros emprestados fora do prazo incluir nome do leitor e numero de dias fora de prazo
        Top de leitores 
        Lista dos leitores com livros por devolver
         */
    }
}
