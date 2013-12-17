using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace LAM.View
{
    /// <summary>
    /// Interaction logic for TelaGestaoPartidas.xaml
    /// </summary>
    public partial class TelaGestaoBalcao : MetroWindow
    {
        public TelaGestaoBalcao()
        {
            InitializeComponent();
        }

        internal void iniciarComponentes(int balcao)
        {
            voos.lerTv(balcao);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var allScreens = System.Windows.Forms.Screen.AllScreens.ToList();

                var locationScreen = allScreens.SingleOrDefault(s => this.Left >= s.WorkingArea.Left && this.Left < s.WorkingArea.Right);

                var tela3 = allScreens.ElementAt(voos.getTvNumber());

                Balcao form3 = new Balcao();

                form3.voos.loadComponetes(voos.imgCompania.Source, voos.lblVoo.Content as string, voos.lblDestino.Content as string, voos.lblClasse.Content as string);

                form3.Left = tela3.WorkingArea.Left;
                form3.Top = tela3.WorkingArea.Top;
                form3.Width = tela3.WorkingArea.Width;
                form3.Height = tela3.WorkingArea.Height;
                form3.WindowState = WindowState.Normal;

                form3.WindowStartupLocation = WindowStartupLocation.Manual;
                form3.Owner = this;
                form3.ShowInTaskbar = false;

                form3.Show();
                new ctrlVoos().actualiza();
            }
            catch 
            {
                MessageBox.Show("A tela " + voos.getTvNumber() +" do balcao " + voos.balcao + " não se encontra disponivel - por favor, contacte o administrador do sistema");
            }
            
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            voos.editar();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
