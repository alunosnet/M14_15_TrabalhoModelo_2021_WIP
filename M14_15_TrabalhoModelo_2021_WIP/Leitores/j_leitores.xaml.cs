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

namespace M14_15_TrabalhoModelo_2021_WIP.Leitores
{
    /// <summary>
    /// Interaction logic for j_leitores.xaml
    /// </summary>
    public partial class j_leitores : Window
    {
        BaseDados bd;
        int LeitoresPorPagina = 5;
        public j_leitores(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
            btAtualizar.Visibility = Visibility.Hidden;
            btRemover.Visibility = Visibility.Hidden;
            ContarPaginas();
            cbPaginacao.SelectedIndex =0;
            AtualizaGrid();
        }
        void ContarPaginas()
        {
            decimal npaginas=Math.Ceiling((decimal)Leitor.NrLeitores(bd)/LeitoresPorPagina);
            cbPaginacao.Items.Clear();
            for (int i = 1; i <= npaginas; i++)
            {
                cbPaginacao.Items.Add(i);
            }

        }
        private void AtualizaGrid()
        {
            if(cbPaginacao.SelectedItem==null)
                DGLeitores.ItemsSource = Leitor.ListarTodos(bd);
            else
            {
                int p = int.Parse(cbPaginacao.SelectedItem.ToString());
                int primeiro = (p - 1) * LeitoresPorPagina;
                DGLeitores.ItemsSource = Leitor.ListarTodos(bd, primeiro + 1, primeiro + LeitoresPorPagina);

            }

            
        }

        //adicionar
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nome = tbNome.Text;
            DateTime data = DPData.SelectedDate.Value;
            var foto = Utils.ImagemParaVetor(ImgFoto.Tag.ToString());

            Leitor novo = new Leitor(0, nome, data, foto, true);
            novo.Adicionar(bd);

            LimparForm();
            AtualizaGrid();
            ContarPaginas();
        }

        private void LimparForm()
        {
            ImgFoto.Source = null;
            DGLeitores.SelectedItem = null;
            ImgFoto.Tag = "";
            tbNome.Text = "";
            btAtualizar.Visibility = Visibility.Hidden;
            btRemover.Visibility = Visibility.Hidden;
        }

        //imagem
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
                    ImgFoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImgFoto.Tag = openFileDialog.FileName;
                }
            }
        }

        private void cbPaginacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtualizaGrid();
        }
        
        private void DGLeitores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Leitor lt = (Leitor)DGLeitores.SelectedItem;
            if (lt == null) return;
            tbNome.Text = lt.nome;
            DPData.SelectedDate = lt.data_nascimento;
            string ficheiro=Utils.pastaDoPrograma()+@"\temp.jpg";
            Utils.VetorParaImagem(lt.fotografia, ficheiro);
            //Corrigir o problema do lock ao ficheiro
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            img.UriSource = new Uri(ficheiro);
            img.EndInit();
            ImgFoto.Source = img;

            //apagar o ficheiro temp.jpg
            File.Delete(ficheiro);
            btRemover.Visibility = Visibility.Visible;
            btAtualizar.Visibility = Visibility.Visible;
        }
        //limpar form
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LimparForm();
        }
        //atualizar
        private void btAtualizar_Click(object sender, RoutedEventArgs e)
        {
            Leitor lt = (Leitor)DGLeitores.SelectedItem;
            if (lt == null) return;
            lt.nome = tbNome.Text;
            lt.data_nascimento = DPData.SelectedDate.Value;
            if(ImgFoto.Tag!=null)
                lt.fotografia= Utils.ImagemParaVetor(ImgFoto.Tag.ToString());
            lt.Atualizar(bd);
            LimparForm();
            AtualizaGrid();
        }
        //remover
        private void btRemover_Click(object sender, RoutedEventArgs e)
        {
            Leitor lt = (Leitor)DGLeitores.SelectedItem;
            if (lt == null) return;
            Leitor.Remover(bd,lt.nleitor);
            LimparForm();
            AtualizaGrid();
            ContarPaginas();
        }
    }
}
