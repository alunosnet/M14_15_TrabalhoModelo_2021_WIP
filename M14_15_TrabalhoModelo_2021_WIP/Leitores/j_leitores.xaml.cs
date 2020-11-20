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
        public j_leitores(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
            AtualizaGrid();
        }

        private void AtualizaGrid()
        {
            DGLeitores.ItemsSource = Leitor.ListarTodos(bd);
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
        }

        private void LimparForm()
        {
            ImgFoto.Source = null;
            ImgFoto.Tag = "";
            tbNome.Text = "";
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
    }
}
