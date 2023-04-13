using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace aurora.mingucci._4I.WPFTheads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int GIRI1 = 5000;  //si usa le lettere Maiuscole
        const int GIRI2 = 500;   //per le costanti
        const int GIRI3 = 50;

        const int TEMPO1 = 1;
        const int TEMPO2 = 10;
        const int TEMPO3 = 100;

        int _counter1 = 0;
        int _counter2 = 0;
        int _counter3 = 0;
        static readonly object _locker = new object();

        CountdownEvent semaforo;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Spegne momentaneamente il pulsante
            btnGo.IsEnabled = false;
            prbarCounter1.Maximum += (GIRI1 + GIRI2);

            Thread thread1 = new Thread(incrementa1);  //crazione del tread 1 e farlo partire
            thread1.Start();

            Thread thread2 = new Thread(incrementa2);  //crazione del tread 1 e farlo partire
            thread2.Start();

            Thread thread3 = new Thread(incrementa3);  //crazione del tread 1 e farlo partire
            thread3.Start();

            semaforo = new CountdownEvent(3); //semaforo 3 eventi

            Thread thread4 = new Thread(() =>
            {
                semaforo.Wait();
                Dispatcher.Invoke(() =>
                {
                    lblCounter1.Text = _counter1.ToString();  //contatore
                    lblCounter2.Text = _counter2.ToString();  //contatore
                    lblCounter3.Text = _counter3.ToString();  //contatore

                    prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                    btnGo.IsEnabled = true;
                }
                );
            }
            );

            thread4.Start();

        }

        // Processo lento che dobbiamo lanciare...
        private void incrementa1()    //aumenta i contatore di 1
        {
            for (int x = 0; x < GIRI1; x++)
            {
                lock (_locker)
                {
                    _counter1++;
                }

                Dispatcher.Invoke(

                    () =>

                    {
                        lblCounter1.Text = _counter1.ToString();
                        prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                        lblCounterTOT.Text = (_counter1 + _counter2 + _counter3).ToString();    
                    }

                );

                Thread.Sleep(TEMPO1);
            }
            semaforo.Signal();  //SEGNALAZIONE AL SEMAFORO
        }

        private void incrementa2()
        {
            for (int x = 0; x < GIRI2; x++)
            {
                lock (_locker)
                {
                    _counter2++;
                }

                Dispatcher.Invoke(

                    () =>

                    {
                        lblCounter2.Text = _counter2.ToString();
                        prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                        lblCounterTOT.Text = (_counter1 + _counter2 + _counter3).ToString();
                    }

                );

                Thread.Sleep(TEMPO2);
            }
            semaforo.Signal();
        }

        private void incrementa3()
        {
            for (int x = 0; x < GIRI3; x++)
            {
                lock (_locker)
                {
                    _counter3++;
                }

                Dispatcher.Invoke(

                    () =>

                    {
                        lblCounter3.Text = _counter3.ToString();
                        prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                        lblCounterTOT.Text = (_counter1 + _counter2 + _counter3).ToString();
                    }

                );

                Thread.Sleep(TEMPO3);
            }
            semaforo.Signal();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e) //pulsante clear
        {
             _counter1 = 0;
             _counter2 = 0;
             _counter3 = 0;
            lblCounter1.Text = _counter1.ToString();
            lblCounter2.Text = _counter2.ToString();
            lblCounter3.Text = _counter3.ToString();

            prbarCounter1.Value = 0;
            prbarCounter1.Maximum = GIRI1 + GIRI2 + GIRI3;  //la progress bar deve conteggiare tutti 3 i GIRI

        }
    }
}
