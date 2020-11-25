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
            this.bd = bd;
            InitializeComponent();
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
            ImgCapa.Tag = null;
            tbNome.Text = "";
            tbAno.Text = "";
            tbPreco.Text = "";
            DGLivros.SelectedItem = null;
            btAtualizar.Visibility = Visibility.Hidden;
            btRemover.Visibility = Visibility.Hidden;
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
            LimparForm();
            AtualizaGrid();
        }

        private void DGLivros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Livro lv = (Livro)DGLivros.SelectedItem;
            if (lv == null) return;
            tbNome.Text = lv.nome;
            tbAno.Text = lv.ano.ToString();
            tbPreco.Text = lv.preco.ToString();
            DPData.SelectedDate = lv.data_aquisicao;
            if (System.IO.File.Exists(lv.capa))
            {
                //TODO:Corrigir o problema do lock ao ficheiro da capa
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                img.UriSource = new Uri(lv.capa);
                img.EndInit();

                ImgCapa.Source = img;

            }
            else
            {
                ImgCapa.Source = null;
            }
            //mostrar o botão atualizar
            btAtualizar.Visibility = Visibility.Visible;
            btRemover.Visibility = Visibility.Visible;
        }

        private void DGLivros_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if(col!=null && e.PropertyType == typeof(DateTime))
            {
                if (col.Header.ToString() == "data_aquisicao")
                    col.Binding = new Binding(e.PropertyName) { StringFormat = "dd-MM-yyyy" };
            }
        }
        //limpar form
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            LimparForm();
        }
        //atualizar livro
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Livro lv = (Livro)DGLivros.SelectedItem;
            if (lv == null) return;

            //atualizar dados
            lv.nome = tbNome.Text;
            lv.ano = int.Parse(tbAno.Text);
            lv.data_aquisicao = DPData.SelectedDate.Value;
            if (ImgCapa.Tag !=null)
            {
                //TODO:tentar apagar os ficheiros das capas que
                //já não são utilizados
                //ImgCapa.Source = null;
                //GC.Collect();
                //Guid guid = Guid.NewGuid();
                //string capa = Utils.pastaDoPrograma() + @"\" + guid.ToString();
                string ficheiro = ImgCapa.Tag.ToString();
                //lv.capa = capa;
                File.Copy(ficheiro, lv.capa, true);
            }
            lv.preco = Decimal.Parse(tbPreco.Text);

            lv.Atualizar(bd);
            LimparForm();
            AtualizaGrid();
        }

        private void tbPesqusiar_TextChanged(object sender, TextChangedEventArgs e)
        {
            DGLivros.ItemsSource = Livro.PesquisaPorNome(bd,tbPesquisar.Text);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Utils.printDG<Livro>(DGLivros, "Livros");
        }
    }
}
