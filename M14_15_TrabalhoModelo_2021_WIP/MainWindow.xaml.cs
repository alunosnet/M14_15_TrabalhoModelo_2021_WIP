using M14_15_TrabalhoModelo_2021_WIP.Leitores;
using M14_15_TrabalhoModelo_2021_WIP.Livros;
using System;
using System.Collections.Generic;
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
    }
}
