
using LAM.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;

namespace LAM.View
{
    // Declaration
    public delegate void UpdateBalcao(int balcao);
    
    /// <summary>
    /// Interaction logic for ctrlVoos.xaml
    /// </summary>
    public partial class ctrlVoos : UserControl
    {
        public event UpdateBalcao updateBalcao;
        Companhia tempComp;

        public int balcao { get; set; }
        static LAMEntities context = new LAMEntities();

        public ctrlVoos()
        {
            InitializeComponent();
            cblCompania.ItemsSource = context.Companhias.ToList<Companhia>(); ;

            this.updateBalcao += new UpdateBalcao(updateBalca);
        }

        private void updateBalca(int balcao)
        {
            if (this.balcao == balcao)
                this.lerTv(balcao);
        }

        protected virtual void UpdateBalcao(int balcao)
        {
            if (updateBalcao != null)
            {
                updateBalcao(balcao);//Raise the event
            }
        }


        public void lerTv(int balcao)
        {
            this.balcao = balcao;
            try
            {
                var balcoes = context.Balcaos.Where(p => p.Balcao1.Value == balcao).First();

                byte[] data = balcoes.Companhia1.Icon; // Load that from your database

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = new MemoryStream(data);
                img.EndInit();

                imgCompania.Source = img;

                lblClasse.Content = balcoes.Classe;
                lblVoo.Content = balcoes.Voo;
                lblDestino.Content = balcoes.Cidade;
                tempComp = balcoes.Companhia1;

            }
            catch (Exception e) { }
        }

        public void loadComponetes(string caminhologo, string voo, string destino, string classe)
        {
            lblClasse.Content = classe;
            lblVoo.Content = voo;
            lblDestino.Content = destino;

            //Image finalImage = new Image();
            //finalImage.Width = 80;
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"/LAM;component/Resources/Logotipos/" + caminhologo, UriKind.RelativeOrAbsolute);
            logo.EndInit();

            imgCompania.Source = logo;
        }

        public void loadComponetes(ImageSource logo, string voo, string destino, string classe)
        {
            lblClasse.Content = classe;
            lblVoo.Content = voo;
            lblDestino.Content = destino;

            //Image finalImage = new Image();
            //finalImage.Width = 80;
            //BitmapImage logo = new BitmapImage();
            //logo.BeginInit();
            //logo.UriSource = new Uri(@"/LAM;component/Resources/Logotipos/" + caminhologo, UriKind.RelativeOrAbsolute);
            //logo.EndInit();

            imgCompania.Source = logo;
        }

        #region accoes
        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            txtVoo.Visibility = Visibility.Visible;
        }

        public void editar()
        {
            txtVoo.Visibility = Visibility.Visible;
            txtClasse.Visibility = Visibility.Visible;
            txtDestino.Visibility = Visibility.Visible;
            cblCompania.Visibility = Visibility.Visible;
        }
        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            txtClasse.Visibility = Visibility.Visible;
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            txtDestino.Visibility = Visibility.Visible;
        }

        private void Image_MouseDown_img(object sender, MouseButtonEventArgs e)
        {
            cblCompania.Visibility = Visibility.Visible;
        }

        private void txtVoo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lblVoo.Content = txtVoo.Text;
                txtVoo.Visibility = Visibility.Hidden;
            }
        }

        private void txtDestino_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lblDestino.Content = txtDestino.Text;
                txtDestino.Visibility = Visibility.Hidden;
            }
        }

        private void txtClasse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lblClasse.Content = txtClasse.Text;
                txtClasse.Visibility = Visibility.Hidden;
            }
        }

        //cblCompania_KeyDown
        private void cblCompania_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Companhia comp = (Companhia)cblCompania.SelectedItem;

                    byte[] data = comp.Icon; // Load that from your database

                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.StreamSource = new MemoryStream(data);
                    img.EndInit();

                    imgCompania.Source = img;
                    cblCompania.Visibility = Visibility.Hidden;
                    tempComp = comp;
                }
            }
            catch { }

        }


        internal void gravar()
        {
            var balcoes = context.Balcaos.Where(p => p.Balcao1.Value == balcao).First();
            balcoes.Cidade = lblDestino.Content == null ? "" : lblDestino.Content.ToString();
            balcoes.Classe = lblClasse.Content == null ? "" : lblClasse.Content.ToString();
            balcoes.Voo = lblVoo.Content == null ? "" : lblVoo.Content.ToString();
            balcoes.Companhia1 = tempComp == null ? null : tempComp;

            context.SaveChanges();

        }

        public void Process(UpdateBalcao logHandler)
        {
            if (logHandler != null)
            {
                logHandler(balcao);
            }
        }

        internal int getTvNumber()
        {
            switch (balcao)
            {
                case 1: return 3;
                case 2: return 4;
                case 3: return 5;
                case 4: return 6;
                case 5: return 7;
            }
            return 0;
        }

        public void actualiza()
        {
            gravar();
        }
    }
        #endregion
}