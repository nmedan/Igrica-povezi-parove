using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGameExample.Commands;
using System.Windows.Input;
using System.IO;
using System.Windows.Controls;
using MemoryGameExample.Model;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
namespace MemoryGameExample.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {
        MainWindow mainWindow;
        bool twoFieldsOpened;
        public DispatcherTimer dispatcherTimer;
        public int time;
        public int moves;
        public BitmapImage[] sources;
        public BitmapImage[] sourcesRandomized;
        public Button[] buttons;
        public Image[] images;
        string[] paths;

        #region constructor
        public MainWindowViewModel(MainWindow MainWindow)
        {
            mainWindow = MainWindow;
            this.StartGame();
        }
        #endregion

        #region methods
        public void Timer_Tick(object sender, EventArgs e)
        {
            mainWindow.Time.Text = time.ToString();
            time++;
        }

        public bool End()
        {

            if (!buttons.Any(x => x.Visibility == System.Windows.Visibility.Visible))
            {
                dispatcherTimer.Stop();
                return true;
            }
            return false;
        }

        public void StartGame()
        {
            time = 0;
            moves = 0;
            mainWindow.Time.Text = time.ToString();
            mainWindow.Moves.Text = moves.ToString();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();
            sources = new BitmapImage[16];
            sourcesRandomized = new BitmapImage[16];
            paths = new string[] {@"\SlikeMemory\Picture1.png", @"\SlikeMemory\Picture2.png",
            @"\SlikeMemory\Picture3.png", @"\SlikeMemory\Picture4.png", @"\SlikeMemory\Picture5.png",
            @"\SlikeMemory\Picture6.png", @"\SlikeMemory\Picture7.png", @"\SlikeMemory\Picture8.png"};
            images = new Image[] {mainWindow.Button1_Image, mainWindow.Button2_Image, mainWindow.Button3_Image, mainWindow.Button4_Image,
            mainWindow.Button5_Image, mainWindow.Button6_Image, mainWindow.Button7_Image, mainWindow.Button8_Image, mainWindow.Button9_Image,
            mainWindow.Button10_Image, mainWindow.Button11_Image, mainWindow.Button12_Image, mainWindow.Button13_Image, mainWindow.Button14_Image,
            mainWindow.Button15_Image, mainWindow.Button16_Image};
            buttons = new Button[] {mainWindow.Button1, mainWindow.Button2, mainWindow.Button3, mainWindow.Button4, mainWindow.Button5, mainWindow.Button6, mainWindow.Button7,
                mainWindow.Button8, mainWindow.Button9, mainWindow.Button10, mainWindow.Button11, mainWindow.Button12, mainWindow.Button13, mainWindow.Button14, mainWindow.Button15, mainWindow.Button16};
            for (int i = 0; i < 8; i++)
            {
                sources[i] = new BitmapImage();
                sources[i].BeginInit();
                sources[i].UriSource = new Uri(paths[i], UriKind.Relative);
                sources[i].EndInit();

            }
            for (int i = 8; i < 16; i++)
            {
                sources[i] = new BitmapImage();
                sources[i].BeginInit();
                sources[i].UriSource = new Uri(paths[i - 8], UriKind.Relative);
                sources[i].EndInit();

            }
            Random rnd = new Random();
            sourcesRandomized = sources.OrderBy(x => rnd.Next()).ToArray();
            for (int i = 0; i < sourcesRandomized.Length; i++)
            {
                images[i].Source = sourcesRandomized[i];
                images[i].Visibility = System.Windows.Visibility.Collapsed;
                buttons[i].Visibility = System.Windows.Visibility.Visible;
            }
        }

        public async void PlayMove(object p)
        {
            if (!twoFieldsOpened)
            {
                images[(int)p - 1].Visibility = System.Windows.Visibility.Visible;

                for (int i = 0; i < 16; i++)
                {
                    if (images[i].Visibility == System.Windows.Visibility.Visible && i != ((int)p - 1))
                    {
                        twoFieldsOpened = true;
                        Task wait = Task.Delay(1000);
                        await wait;

                        if (!images[i].Source.ToString().Equals(images[(int)p - 1].Source.ToString()))
                        {
                            images[i].Visibility = System.Windows.Visibility.Collapsed;
                            images[(int)p - 1].Visibility = System.Windows.Visibility.Collapsed;

                            twoFieldsOpened = false;

                        }
                        else
                        {
                            buttons[i].Visibility = System.Windows.Visibility.Collapsed;
                            buttons[(int)p - 1].Visibility = System.Windows.Visibility.Collapsed;
                            images[i].Visibility = System.Windows.Visibility.Collapsed;
                            images[(int)p - 1].Visibility = System.Windows.Visibility.Collapsed;

                            twoFieldsOpened = false;
                            if (End())
                            {
                                Highscores highscore = new Model.Highscores();
                                highscore.Name = "Player";
                                highscore.Time = int.Parse(mainWindow.Time.Text);
                                highscore.Moves = int.Parse(mainWindow.Moves.Text);
                                using (HighscoresContext db = new HighscoresContext())
                                {
                                    var highscores = db.Highscores.ToList();
                                    highscores.Add(highscore);
                                    var highscoresOrdered = highscores.OrderBy(x => x.Time).ThenBy(x => x.Moves).ToList();
                                    if (highscoresOrdered.IndexOf(highscore) <= 2)
                                    {
                                        EnterHighscore eh = new EnterHighscore(highscore);
                                        eh.ShowDialog();
                                    }
                                }

                                dispatcherTimer.Stop();
                                dispatcherTimer.IsEnabled = false;




                            }
                        }
                        moves++;
                        mainWindow.Moves.Text = moves.ToString();

                    }
                }
            }
        }
        #endregion

        #region commmands
        private ICommand move;
        public ICommand Move
        {
            get
            {
                if (move == null)
                {
                    move = new RelayCommand(param => MoveExecute(param), param => CanMoveExecute());
                }
                return move;
            }
        }
        private void MoveExecute(object parameter)
        {
            if (!End())
            {
                switch (parameter)
                {
                    case "1":
                        PlayMove(1);
                        break;
                    case "2":
                        PlayMove(2);
                        break;
                    case "3":
                        PlayMove(3);
                        break;
                    case "4":
                        PlayMove(4);
                        break;
                    case "5":
                        PlayMove(5);
                        break;
                    case "6":
                        PlayMove(6);
                        break;
                    case "7":
                        PlayMove(7);
                        break;
                    case "8":
                        PlayMove(8);
                        break;
                    case "9":
                        PlayMove(9);
                        break;
                    case "10":
                        PlayMove(10);
                        break;
                    case "11":
                        PlayMove(11);
                        break;
                    case "12":
                        PlayMove(12);
                        break;
                    case "13":
                        PlayMove(13);
                        break;
                    case "14":
                        PlayMove(14);
                        break;
                    case "15":
                        PlayMove(15);
                        break;
                    case "16":
                        PlayMove(16);
                        break;
                }

            }


        }
        private bool CanMoveExecute()
        {
            return true;
        }

        private ICommand newGame;
        public ICommand NewGame
        {
            get
            {
                if (newGame == null)
                {
                    newGame = new RelayCommand(param => NewGameExecute(), param => CanNewGameExecute());
                }
                return newGame;
            }
        }

        public void NewGameExecute()
        {
            dispatcherTimer.Stop();
            StartGame();
        }

        public bool CanNewGameExecute()
        {
            return true;
        }

        private ICommand highscores;
        public ICommand Highscores
        {
            get
            {
                if (highscores == null)
                {
                    highscores = new RelayCommand(param => HighscoresExecute(), param => CanHighscoresExecute());
                }
                return highscores;
            }
        }

        public void HighscoresExecute()
        {
            HighscoreDialog hd = new HighscoreDialog();
            hd.ShowDialog();
        }

        public bool CanHighscoresExecute()
        {
            return true;
        }

        private ICommand exit;
        public ICommand Exit
        {
            get
            {
                if (exit == null)
                {
                    exit = new RelayCommand(param => ExitExecute(), param => CanExitExecute());
                }
                return exit;
            }
        }

        public void ExitExecute()
        {
            mainWindow.Close();
        }

        public bool CanExitExecute()
        {
            return true;
        }

        private ICommand viewHelp;
        public ICommand ViewHelp
        {
            get
            {
                if (viewHelp == null)
                {
                    viewHelp = new RelayCommand(param => ViewHelpExecute(), param => CanViewHelpExecute());
                }
                return viewHelp;
            }
        }

        public void ViewHelpExecute()
        {

            var appPath = Environment.CurrentDirectory;
            appPath = appPath.Replace("\\bin\\Debug", "");
            System.Diagnostics.Process.Start(appPath + @"\Docs\MemoryBlocks.docx");

        }

        public bool CanViewHelpExecute()
        {
            return true;
        }
        
    }
}
#endregion
