# WPFThreads
Sviluppo App WPF                           1° Quadrimestre
Inizio 13 ottobre 2022



20 ottobre 2022

Se clicchiamo su f4 comparirà una finestra con tutti gli attributi del programma, per esempio l’aspetto. 
La rotazione nella windows non è molto funzionale.
“Windows Stile none” fa comparire niente del programma lanciato, non compare nulla sullo schermo. 
Le Trasformazioni ti fanno fare più comandi di spostamento della finestra in esecuzione, e a tutti i componenti vpn.

2 febbraio 2023

Windows universale: è la versione più nuova anche se è gia stata sorpassata.
MAUI: funziona su linux, mac, windows.

Perchè usiamo WPF?
Non c’è per linux o mac, ha una caratteristica:
introduce il linguaggio XAML, una versione minimale, facile, vicina al linguaggio — , WPF ha dato la possibilità di essere utilizzato anche da Linux.



Creiamo un nuovo progetto con il nome

Scegliamo la versione di dotnet 6 perché ci dà meno problemi


Cliccare sul progetto, pulsante destro sul progetto, cliccare “Apri la cartella in Esplora File"
Grid = si chiama così e basta
Window = contiene tutto, le cose dentro sono gli attributi: Title, Height, Width,Background


Proprietà: fa vedere le proprietà dell’oggetto selezionato. 


Per aprirlo bisogna andare su “visualizza”, si aprirà un elenco andare dove bisogna andare in fondo e cliccare “Altre finestre”, dove si aprirà un'altra finestra dove bisogna cliccare “Gestione proprietà”. 

16 febbraio 2023:

CODICE:

Dobbiamo lanciare un thread separato e fargli girare la velocità che vuole in modo da lasciare il thread della grafica in pace. 

private void Button_Click(object sender, RoutedEventArgs e)
        {
            //IncrementCounterProva();

            Thread thread1 = new Thread(IncrementCounter1); //creazione di un nuovo thread
            thread1.Start();
        }

        private void IncrementCounter1()
        {
            for (int i = 0; i < 1000; i++)     //incrementa i giri a 1000
            {
                _counter++;
                Thread.Sleep(10);   //serve per fare "riposare" il programma

                Dispatcher.Invoke(   //zona critica, il “bigliettaio” che ti lascia fare le cose che 
                    () =>                     //normalmente non potresti

                    {
                        lblCounter1.Foreground = Brushes.LightBlue;
                        lblCounter1.Text = _counter.ToString();
                    }
                );
            }
        }
        private void IncrementCounterProva()   //Processo lento che dobbiamo lanciare
        {
            for (int i = 0; i < 200; i++)   
            {
                _counter++;

                lblCounter1.Foreground = Brushes.LightBlue;
                lblCounter1.Text = _counter.ToString();

                Thread.Sleep( 10 );
            }
        }
    }
}

Quando la zona è invalidata l'interfaccia grafica è costretta a refreshare. Prima faceva vedere solo l'ultimo risultato, cioè 1000. 

2° Quadrimestre

02 marzo 2023

i semafori sono dei costrutti messi a disposizione dal kernel, lock si utilizza per fare cose che sono per loro natura  disgiunta, fatte a pezzi, mentre facciamo qualcosa nessuno ci può essere interrotta, e sul lock si sono costruiti i semafori.

Il semaforo è un contatore di  interi che non può essere negativo 
ha due caratteristiche:

-signal = decrementa il contatore
-wait= aspetta che diventi 0

CountdownEvent semaforo = new CountdownEvent(2); // Inizializziamo un evento


una volta che il semaforo è stato caricato e ha finto e si è svuotato compare il messaggio che ha finito

Event Arg deve essere usato il meno possibile e qui non si sblocca più. Il semaforo non può essere usato così alla “leggera” in un Evento Arg. Quindi gli creiamo un’altro thread, il terzo, solo per il semaforo. Per creare questo 3 thread dobbiamo creare un metodo.
istruzione Attendi:


serve per non rallentare windows e farlo bloccare.  Facciamo un altro dispatcher.Invoke dentro ad attendi e lo mettiamo dentro il 3# Thread 

Dispatcher.Invoke : usato per aggiornamento dei contatori.
Questa funzione ha come parametro la lambda expression aspetta il suo turno e successivamente si attiva. La lambda rappresenta così: 
() =>
{ codice }
Se dovessimo rilanciare il programma si “romperebbe” tutto, perché il semaforo ad ogni click viene distrutto e reinizializzato. Quindi dobbiamo scrivere delle righe di codice dove diciamo che finchè non ha finito il suo compito non si può cliccare il pulsante, lo spegne momentaneamente, facendolo diventare grigio e non più cliccabile.. 


09 marzo 2023

*Svolgimento dell’ esercizio su classroom* 

16 marzo 2023 

CODICE COMPLETO:

-Finestra MainWindow.xaml

<Window x:Class="aurora.mingucci._4I.WPFTheads.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:aurora.mingucci._4I.WPFTheads"
        mc:Ignorable="d"
        Title="aurora.mingucci.4I.WPFThread 2023"     //Titolo progetto
        WindowStartupLocation="CenterScreen"
        Height="450" Width="800">
    <Grid ShowGridLines="True">
        <Grid.RowDefinitions>
            <RowDefinition Height="100"></RowDefinition>
            <RowDefinition></RowDefinition>
        </Grid.RowDefinitions>

        <StackPanel Grid.Row="0" Orientation="Horizontal">
            <Button x:Name="btnGo" Width="100" Click="Button_Click"></Button>
            <Button x:Name="btnClear" Width="100" Click="btnClear_Click"></Button>
        </StackPanel>

        <StackPanel Grid.Row="1">
            <TextBlock x:Name="lblCounter1" FontSize="50"></TextBlock>    //da in output a 
            <TextBlock x:Name="lblCounter2" FontSize="50"></TextBlock>    //livello grafico il 
            <TextBlock x:Name="lblCounter3" FontSize="50"></TextBlock>    //valore che hanno                                         
                                                                                                                    //i contatori
              //Inizializzazione della Progressbar
            <ProgressBar x:Name="prbarCounter1" Height="50" Maximum="0"></ProgressBar>
            <TextBlock x:Name="lblCounterTOT" FontSize="50">
            </TextBlock>
        </StackPanel>
    </Grid>
</Window>



-Finestra MainWindow.cs

using System;                                               | 
using System.Collections.Generic;               |
using System.Diagnostics.Metrics;               |
using System.Linq;                                       |
using System.Text;                                       |
using System.Threading;                             |
using System.Threading.Tasks;                    \
using System.Windows;                                 Librerie
using System.Windows.Controls;                 /
using System.Windows.Data;                      |
using System.Windows.Documents;           |
using System.Windows.Input;                     |
using System.Windows.Media;                   |
using System.Windows.Media.Imaging;     |
using System.Windows.Navigation;           |
using System.Windows.Shapes;                |

namespace aurora.mingucci._4I.WPFTheads
{
    public partial class MainWindow : Window
    {
        //Giri che compie ogni Thread (si usa le lettere Maiuscole per le costanti)
        const int GIRI1 = 5000;  
        const int GIRI2 = 500;   
        const int GIRI3 = 50;
        //Tempo in ms che ci impiega il tread a compiere un giro
        const int TEMPO1 = 1;
        const int TEMPO2 = 10;
        const int TEMPO3 = 100;
        //Contatori 
        int _counter1 = 0;
        int _counter2 = 0;
        int _counter3 = 0;
        static readonly object _locker = new object();

        CountdownEvent semaforo;     //Dichiarazione del semaforo

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Spegne momentaneamente il pulsante
            btnGo.IsEnabled = false;
            prbarCounter1.Maximum += (GIRI1 + GIRI2);

            Thread thread1 = new Thread(incrementa1);  //creazione del thread 1 e       
                                                                                     //farlo partire
            thread1.Start();

            Thread thread2 = new Thread(incrementa2);  //creazione del thread 2            
            thread2.Start();

            Thread thread3 = new Thread(incrementa3);  //creazione del thread 3            
             thread3.Start();

            semaforo = new CountdownEvent(3); //semaforo che finché non si sono        
                                                                        //eseguiti i 3 thread precedenti non 
                                                                        //funziona 

            Thread thread4 = new Thread(() =>   //aggiornare a livello grafico tutti  
                                                                       // i 3 contatori
            {
                semaforo.Wait();                            //semaforo si blocca
                Dispatcher.Invoke(() =>  //
                {
                    lblCounter1.Text = _counter1.ToString();  //contatore
                    lblCounter2.Text = _counter2.ToString();  //contatore
                    lblCounter3.Text = _counter3.ToString();  //contatore

                    //contatore totale di tutti i counter
                    prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                    btnGo.IsEnabled = true;  //si “risvegliano” i bottoni
                }
                );
            }
            );
            thread4.Start();   //parte ilo thread
        }
        // Metodo che serve per incrementare i contatori
        private void incrementa1()    //aumenta il contatore di 1
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
            prbarCounter1.Maximum = GIRI1 + GIRI2 + GIRI3;  //la progress bar deve 
                                                                                               //conteggiare tutti 3 i 
                                                                                               //GIRI
        }
    }
}
