﻿


using LAM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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







namespace LAM.View
{
    /// <summary>
    /// Interaction logic for ctrlPartidas.xaml
    /// </summary>
    public partial class ctrlPartidas : UserControl
    {
        //ObservableCollection<chekin> list = new ObservableCollection<chekin>();
        List<KeyValuePair<string, string>> imageslist;
        ObservableCollection<Companhia> companhias;

        int counter = 0;

        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        ObservableCollection<Partida> list;
        static LAMEntities context = new LAMEntities();
        ComboBox tempcombo = new ComboBox();
        Partida chegada = new Partida();

        public static List<Partida> cheg
        {
            get
            {
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
                    return new List<Partida>();

                return context.Partida.ToList();
            }
        }

        public ctrlPartidas()
        {
           
            
            InitializeComponent();
            
           // g.ItemsSource = list;
            BindData();
            
            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void BindData()
        {
            //var dataSource = new ObservableCollection<ImapHost>(context.Car);
            list = new ObservableCollection<Partida>(context.Partida.ToList());
            companhias = new ObservableCollection<Companhia>(context.Companhia.ToList());
            list.CollectionChanged += CollectionChanged;
            list.Take(10); 
            g.DataContext = list;
            //g.DataContext = list;

        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (Partida cheg in e.NewItems)
                    context.Partida.Add(cheg);

            else if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (Partida cheg in e.OldItems)
                    context.Partida.Remove(cheg);
            //BindData();
        }

        public void inicializarcomponentes()
        {

        }



        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d;
            d = DateTime.Now;
            label2.Content = d.ToString("dd/MM/yyyy ") + d.Hour + " : " + d.Minute + " : " + d.Second;
            if (counter == 30)
            {
                
                list.Take(10);
                g.DataContext = getRandomList();
                return;
            }
            counter++;
        }

        private void g_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void clInternalComp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                tempcombo = sender as ComboBox;
                chegada = g.SelectedItem as Partida;

                if (chegada == null)
                    chegada = new Partida();
                var combo = (ComboBox)sender;
                var companhia = (Companhia)combo.SelectedItem;

                if (companhia == null)
                    return;
                chegada.Companhia1 = companhia;
                chegada.Companhia = companhia.Id;
                g.SelectedItem = chegada;
                // 

            }

            catch (NullReferenceException nex)
            {

                // MessageBox.Show(nex.Message);

            }
        }

        private void cblInternalComp_Initialized(object sender, EventArgs e)
        {
              tempcombo = (FrameworkElement)sender as ComboBox;
            tempcombo.Items.Clear();
            tempcombo.ItemsSource = companhias;
        }

        private  void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            try
            {
                if (e.EditAction == DataGridEditAction.Cancel)
                {
                    e.Cancel = false;
                    return;

                }

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    DataGridRow dgr = e.Row;
                    //XmlElement xe = Chegada;
                }
                SaveChanges();
            }
            catch(Exception ex){
            
            }
        }

        public async void SaveChanges()
        {
            if (g.SelectedItem == null)
                return;

            if (g.SelectedItem == chegada)
                context.SaveChanges();
            else
            {
                chegada = new Partida();
                chegada = g.SelectedItem as Partida;
                var Companhia =  tempcombo.SelectedItem==null ? null: tempcombo.SelectedItem as Companhia;

                if (Companhia == null)
                    return;

                chegada.Companhia =  Companhia.Id;
               await context.SaveChangesAsync();

            }
            chegada = null;

        }

        private List<object> getRandomList() {
            var _list = new List<object>();
            Random random = new Random();
            for (int i = 0; i < 10; i++) {
                int position = random.Next(list.Count);
                if(position<list.Count && position>0)
                _list.Add(list.ElementAt(position));
            }
                return _list;
        
        }
    }


}
