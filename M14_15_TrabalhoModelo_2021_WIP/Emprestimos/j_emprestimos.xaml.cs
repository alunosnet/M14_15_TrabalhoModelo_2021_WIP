using M14_15_TrabalhoModelo_2021_WIP.Leitores;
using M14_15_TrabalhoModelo_2021_WIP.Livros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace M14_15_TrabalhoModelo_2021_WIP.Emprestimos
{
    /// <summary>
    /// Interaction logic for j_emprestimos.xaml
    /// </summary>
    public partial class j_emprestimos : Window
    {
        BaseDados bd;
        public j_emprestimos(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
            AtualizaCBLeitores();
            AtualizaCBLivros();
        }

        void AtualizaCBLeitores()
        {
            var lista=Leitor.ListarTodos(bd);
            cbLeitores.ItemsSource = lista;
        }
        void AtualizaCBLivros()
        {
            var lista = Livro.ListaTodosLivrosComEstado(bd,1,true);
            cbLivros.ItemsSource = lista;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Leitor lt = (Leitor)cbLeitores.SelectedItem;
            Livro lv = (Livro)cbLivros.SelectedItem;
            if (lv == null || lt == null) return;
            Emprestimo emp = new Emprestimo(lt.nleitor, lv.nlivro);
            emp.Adicionar(bd);
            AtualizaCBLivros();
        }
    }
}
