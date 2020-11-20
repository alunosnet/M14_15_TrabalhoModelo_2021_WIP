using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace M14_15_TrabalhoModelo_2021_WIP.Livros
{
    /// <summary>
    /// Interaction logic for j_livros.xaml
    /// </summary>
    public partial class j_livros : Window
    {
        BaseDados bd;
        public j_livros(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
            AtualizaGrid();
        }
        //adicionar
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //validar dados do form
            string nome = tbNome.Text;
            if (nome.Trim().Length == 0)
            {
                MessageBox.Show("O nome é obrigatório");
                return;
            }
            int ano = int.Parse(tbAno.Text);
            if (ano < 0 || ano > DateTime.Now.Year)
            {
                MessageBox.Show("O ano não está correto");
                return;
            }
            decimal preco = Decimal.Parse(tbPreco.Text);
            if (preco < 0)
            {
                MessageBox.Show("O preço não pode ser negativo");
                return;
            }
            Guid guid = Guid.NewGuid();
            string capa = Utils.pastaDoPrograma() + @"\" + guid.ToString();
            //criar objeto
            Livro lv = new Livro(0, nome, ano, DPData.SelectedDate.Value, preco, capa, true);
            //guardar na bd
            lv.Adicionar(bd);
            //guardar imagem
            string ficheiro = ImgCapa.Tag.ToString();
            if (ficheiro != string.Empty)
            {
                if (File.Exists(ficheiro))
                    File.Copy(ficheiro, capa);
            }
            //limpar form
            LimparForm();
            //atualizar grid
            AtualizaGrid();
        }

        private void AtualizaGrid()
        {
            DGLivros.ItemsSource = Livro.ListaTodosLivros(bd);
        }

        private void LimparForm()
        {
            ImgCapa.Source = null;
            ImgCapa.Tag = "";
            tbNome.Text = "";
            tbAno.Text = "";
            tbPreco.Text = "";
            
        }

        //escolher imagem
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Imagens |*.jpg;*.png | Todos os ficheiros |*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName != string.Empty && File.Exists(openFileDialog.FileName))
                {
                    ImgCapa.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImgCapa.Tag = openFileDialog.FileName;
                }
            }
        }
        //remover
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Livro livro = (Livro)DGLivros.SelectedItem;
            if (livro == null)
            {
                MessageBox.Show("Selecione o livro a remover.");
                return;
            }
            Livro.Remover(bd, livro.nlivro);
            AtualizaGrid();
        }
        //TODO: mostrar os dados livro selecionado com o click na grid
    }
}
