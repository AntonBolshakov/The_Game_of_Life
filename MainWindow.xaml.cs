using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace The_Game_of_Life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private const int WidthField = 44, HeightField = 44;
        private bool stopFlag = true;
        private bool[,] CellArray = new bool[HeightField, WidthField];
        private bool[,] TCellArray = new bool[HeightField, WidthField];
        private Button[,] ArrayField = new Button[HeightField, WidthField];
        private int generation = 0;
        private int sg = 0;
        private Label GenerationOutput = new Label();
        private Grid GameGrid = new Grid();
        private Button ButtonGo = new Button();
        private Button ButtonStop = new Button();
        private Button ButtonNewGame = new Button();
        private Button ButtonExit = new Button();
        private Button ButtonSave = new Button();
        private Button ButtonLoading = new Button();
        private TextBox NameFileSaveGame = new TextBox();
        private ListBox Autor = new ListBox();
        private string ree = "Save/";
        private Label DelInfo = new Label();
        private Grid LoadGrid = new Grid();
        private Label LoadFon = new Label();
        private Label LoadFon1 = new Label();
        private ListBox LoadInfoTime = new ListBox();
        private Button Loadloading = new Button();
        private Button LoadDelet = new Button();
        private Button LoadExit = new Button();


        public Menu()
        {
            InitializeComponent();
            MenuGame();
        }

        private void PlayGame_Click(object sender, RoutedEventArgs e)
        { PlayGame(); }

        private void Loading_Click(object sender, RoutedEventArgs e)
        { LoadingGame(); }

        private void Program_Click(object sender, RoutedEventArgs e)
        { ProgramGame(); }

        private void Autor_Click(object sender, RoutedEventArgs e)
        { AutorGame(); }

        private void Exit_Click(object sender, RoutedEventArgs e)
        { ExitGame(); }

        private void Yes_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

        private void Menu_Click(object sender, RoutedEventArgs e)
        { MenuGame(); }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button ButtonCell = (Button)sender;
            Coords co = (Coords)ButtonCell.Tag;
            ButtonCell.Background = new SolidColorBrush(Colors.Gold);
            CellArray[co.I, co.J] = true;
        }

        private void Go_Click(object sender, EventArgs e)
        {
            ButtonNewGame.IsEnabled = false;
            ButtonExit.IsEnabled = false;
            ButtonGo.IsEnabled = false;
            ButtonSave.IsEnabled = false;
            ButtonLoading.IsEnabled = false;
            ButtonStop.IsEnabled = true;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            stopFlag = false;
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Start();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            ButtonNewGame.IsEnabled = true;
            ButtonExit.IsEnabled = true;
            ButtonGo.IsEnabled = true;
            ButtonSave.IsEnabled = true;
            ButtonLoading.IsEnabled = true;
            ButtonStop.IsEnabled = false;
            stopFlag = true;
            return;
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void MenuGame_Click(object sender, EventArgs e)
        {
            GameGrid.Children.Remove(GenerationOutput);
            NewGame();
            MenuGame();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveGame();
        }

        private void Delet_Click(object sender, RoutedEventArgs e)
        {
            if (Autor.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                Loadloading.IsEnabled = false;
                LoadDelet.IsEnabled = false;
                LoadExit.IsEnabled = false;

                Label exit = new Label();
                exit.Style = (Style)this.Resources["MyStyleT"];
                exit.Content = "Вы действительно хотите удалить игру?";
                exit.MinWidth = 520;
                exit.MinHeight = 80;
                exit.FontSize = 20;
                exit.Foreground = new SolidColorBrush(Colors.Black);
                exit.Margin = new Thickness(76, 300, 76, 255);
                LoadGrid.Children.Add(exit);

                Button yes = new Button();
                yes.Style = (Style)this.Resources["MyStyleB"];
                yes.MinWidth = 200;
                yes.Height = 45;
                yes.Content = "Да";
                yes.Margin = new Thickness(0, 320, 300, 12);
                LoadGrid.Children.Add(yes);
                yes.Click += new RoutedEventHandler(YesDel_Click);

                Button no = new Button();
                no.Style = (Style)this.Resources["MyStyleB"];
                no.MinWidth = 200;
                no.Height = 45;
                no.Content = "Нет";
                no.Margin = new Thickness(300, 320, 0, 12);
                LoadGrid.Children.Add(no);
                no.Click += new RoutedEventHandler(Loading_Click);
            }
        }

        private void YesDel_Click(object sender, RoutedEventArgs e)
        {
            string Ch = " ";
            Ch = Autor.Items[Autor.SelectedIndex].ToString();
            string LoadingSaveGame = " ";
            LoadingSaveGame = ree + Ch + ".txt";
            FileInfo fi = new FileInfo(LoadingSaveGame);
            fi.Delete();
            LoadingGame();
        }

        private void LoadingSaveGame_Click(object sender, RoutedEventArgs e)
        {
            if (Autor.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                string Ch = " ";
                Ch = Autor.Items[Autor.SelectedIndex].ToString();
                string LoadingSaveGame = ree + Ch + ".txt";
                StreamReader fp = new StreamReader(LoadingSaveGame);
                int n;
                string str;
                for (int i = 0; i < HeightField; i++)
                {
                    for (int j = 0; j < WidthField; j++)
                    {
                        CellArray[i, j] = false;
                        TCellArray[i, j] = false;
                        str = fp.ReadLine();
                        n = Convert.ToInt32(str);

                        if (n == 1)
                            CellArray[i, j] = true;
                        else
                        {
                            CellArray[i, j] = false;
                        }
                    }
                }
                str = fp.ReadLine();
                n = Convert.ToInt32(str);
                generation = n;
                fp.Close();
                PlayGame();
            }
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            if (NameFileSaveGame.Text == string.Empty)
            {
                return;
            }
            else
            {
                if (NameFileSaveGame.Text.Length >= 25)
                {
                    NameFileSaveGame.Text = "Без названия";
                }
                else
                {
                
                string SaveGames = ree + NameFileSaveGame.Text + ".txt";
                int n = 0;
                StreamWriter fout;
                fout = new StreamWriter(SaveGames);
                for (int i = 0; i < HeightField; i++)
                {
                    for (int j = 0; j < WidthField; j++)
                    {
                        if (CellArray[i, j] == true)
                        {
                            n = 1;
                            fout.WriteLine(n);
                        }
                        else
                        {
                            n = 0;
                            fout.WriteLine(n);
                        }
                    }
                }
                fout.WriteLine(generation);
                fout.Close();
                NameFileSaveGame.Text = " ";
                PlayGame();
                }
            }
        }


//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
//                                               Функции                                                   //
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   MenuGame   +++ )

        private void MenuGame()
        {
            Grid MainGrid = new Grid();
            this.Content = MainGrid;
            MainGrid.Margin = new Thickness(0);
            MainGrid.Background = new SolidColorBrush(Colors.Black);

            Label Fon;
            Fon = new Label();
            Fon.Style = (Style)this.Resources["Fon"];
            MainGrid.Children.Add(Fon);

            Label head;
            head = new Label();
            head.Style = (Style)this.Resources["Head"];
            head.Content = "Игра  B  Жизнь";
            head.Width = 430;
            head.Height = 100;
            head.FontSize = 50;
            head.Margin = new Thickness(20, 50, 0, 0);
            MainGrid.Children.Add(head);

            Button play;
            play = new Button();
            play.Style = (Style)this.Resources["MyStyleB"];
            play.MinWidth = 200;
            play.Content = "Играть";
            play.Margin = new Thickness(80, 0, 74, 280);
            MainGrid.Children.Add(play);
            play.Click += new RoutedEventHandler(PlayGame_Click);

            Button loading;
            loading = new Button();
            loading.Style = (Style)this.Resources["MyStyleB"];
            loading.MinWidth = 210;
            loading.Content = "Загрузить";
            loading.Margin = new Thickness(75, 0, 69, 155);
            MainGrid.Children.Add(loading);
            loading.Click += new RoutedEventHandler(Loading_Click);

            Button program;
            program = new Button();
            program.Style = (Style)this.Resources["MyStyleB"];
            program.MinWidth = 215;
            program.Content = "О программе";
            program.Margin = new Thickness(72, 0, 66, 30);
            MainGrid.Children.Add(program);
            program.Click += new RoutedEventHandler(Program_Click);

            Button autor;
            autor = new Button();
            autor.Style = (Style)this.Resources["MyStyleB"];
            autor.MinWidth = 210;
            autor.Content = "Автор";
            autor.Margin = new Thickness(75, 0, 69, -95);
            MainGrid.Children.Add(autor);
            autor.Click += new RoutedEventHandler(Autor_Click);

            Button exit;
            exit = new Button();
            exit.Style = (Style)this.Resources["MyStyleB"];
            exit.MinWidth = 200;
            exit.Content = "Выход";
            exit.Margin = new Thickness(80, 0, 74, -220);
            MainGrid.Children.Add(exit);
            exit.Click += new RoutedEventHandler(Exit_Click);
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   LoadingGame   +++ )

        private void LoadingGame()
        {
            Loadloading.IsEnabled = true;
            LoadDelet.IsEnabled = true;
            LoadExit.IsEnabled = true;

            Grid MainGrid = new Grid();
            LoadGrid = MainGrid;
            this.Content = LoadGrid;
            LoadGrid.Margin = new Thickness(0);
            LoadGrid.Background = new SolidColorBrush(Colors.Black);

            Label Fon = new Label();
            LoadFon = Fon;
            LoadFon = new Label();
            LoadFon.Style = (Style)this.Resources["Fon"];
            MainGrid.Children.Add(LoadFon);

            Label Fon1 = new Label();
            LoadFon1 = Fon1;
            LoadFon1 = new Label();
            LoadFon1.Style = (Style)this.Resources["LoadingFon"];
            LoadFon1.Width = 850;
            LoadFon1.Height = 640;
            LoadFon1.Margin = new Thickness(0, -50, 0, 50);
            LoadFon1.Foreground = new SolidColorBrush(Colors.White);
            LoadFon1.Content = "Загрузка Игры";
            LoadFon1.FontSize = 50;
            MainGrid.Children.Add(LoadFon1);

            ListBox InfoTime = new ListBox();
            LoadInfoTime = InfoTime;
            LoadInfoTime.Style = (Style)this.Resources["StyleListBoxInfo"];
            LoadInfoTime.Width = 750;
            LoadInfoTime.Height = 530;
            LoadInfoTime.FontSize = 25;
            LoadInfoTime.Foreground = new SolidColorBrush(Colors.White);
            LoadInfoTime.Margin = new Thickness(0, -10, 0, 10);
            MainGrid.Children.Add(LoadInfoTime);

            ListBox AutorR = new ListBox();
            Autor = AutorR;
            Autor.Style = (Style)this.Resources["StyleListBox"];
            Autor.Width = 750;
            Autor.Height = 530;
            Autor.FontSize = 25;
            Autor.Foreground = new SolidColorBrush(Colors.White);
            Autor.Margin = new Thickness(0, -10, 0, 10);
            MainGrid.Children.Add(Autor);

            DirectoryInfo dir = new DirectoryInfo(ree);
            FileInfo[] txtfiles = dir.GetFiles("*.txt");
            Console.WriteLine("1000", txtfiles.Length);
            foreach (FileInfo f in txtfiles)
            {
                Console.WriteLine("Name is : {0}", f.Name);
                Console.WriteLine("Creation time is : {0}", f.CreationTime);
                string SavedGames = f.Name.Replace(".txt", "");
                Autor.Items.Add(SavedGames);
                InfoTime.Items.Add(f.CreationTime + " ");
            }

            Button loading = new Button();
            Loadloading = loading;
            Loadloading.Style = (Style)this.Resources["MyStyleB"];
            Loadloading.MinWidth = 210;
            Loadloading.Content = "Загрузить";
            Loadloading.Margin = new Thickness(-270, 350, 270, -270);
            MainGrid.Children.Add(Loadloading);
            Loadloading.Click += new RoutedEventHandler(LoadingSaveGame_Click);

            Button Delet = new Button();
            LoadDelet = Delet;
            LoadDelet.Style = (Style)this.Resources["MyStyleB"];
            LoadDelet.MinWidth = 210;
            LoadDelet.Content = "Удалить";
            LoadDelet.Margin = new Thickness(-30, 350, 0, -270);
            MainGrid.Children.Add(LoadDelet);
            LoadDelet.Click += new RoutedEventHandler(Delet_Click);

            Button exit = new Button();
            LoadExit = exit;
            LoadExit.Style = (Style)this.Resources["MyStyleB"];
            LoadExit.MinWidth = 200;
            LoadExit.Content = "Отмена";
            LoadExit.Margin = new Thickness(275, 350, -275, -270);
            MainGrid.Children.Add(LoadExit);
            if (generation == 0)
                LoadExit.Click += new RoutedEventHandler(Menu_Click);
            else
                LoadExit.Click += new RoutedEventHandler(PlayGame_Click);

        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   ExitGame   +++ )

        private void ExitGame()
        {
            Grid ExitMenu = new Grid();
            this.Content = ExitMenu;
            ExitMenu.Margin = new Thickness(0);
            ExitMenu.Background = new SolidColorBrush(Colors.Black);

            Label Fon;
            Fon = new Label();
            Fon.Style = (Style)this.Resources["ExFon"];
            ExitMenu.Children.Add(Fon);

            Label exit;
            exit = new Label();
            exit.Style = (Style)this.Resources["MyStyleT"];
            exit.Content = "Вы действительно хотите выйти?";
            exit.MinWidth = 420;
            exit.MinHeight = 80;
            exit.FontSize = 20;
            exit.Foreground = new SolidColorBrush(Colors.Black);
            exit.Margin = new Thickness(76, 40, 76, 255);
            ExitMenu.Children.Add(exit);

            Button yes;
            yes = new Button();
            yes.Style = (Style)this.Resources["MyStyleB"];
            yes.MinWidth = 200;
            yes.Height = 45;
            yes.Content = "Да";
            yes.Margin = new Thickness(0, 320, 350, 12);
            ExitMenu.Children.Add(yes);
            yes.Click += new RoutedEventHandler(Yes_Click);

            Button no;
            no = new Button();
            no.Style = (Style)this.Resources["MyStyleB"];
            no.MinWidth = 200;
            no.Height = 45;
            no.Content = "Нет";
            no.Margin = new Thickness(350, 320, 0, 12);
            ExitMenu.Children.Add(no);
            no.Click += new RoutedEventHandler(Menu_Click);
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   AutorGame   +++ )

        private void AutorGame()
        {
            Grid AutorGrid = new Grid();
            this.Content = AutorGrid;
            AutorGrid.Margin = new Thickness(0);
            AutorGrid.Background = new SolidColorBrush(Colors.Black);

            Label Fon;
            Fon = new Label();
            Fon.Style = (Style)this.Resources["FonInfo"];
            AutorGrid.Children.Add(Fon);

            Label head;
            head = new Label();
            head.Style = (Style)this.Resources["Head"];
            head.Content = "Автор";
            head.Width = 430;
            head.Height = 100;
            head.FontSize = 45;
            head.Margin = new Thickness(20, 50, 0, 0);
            AutorGrid.Children.Add(head);

            Label Autor;
            Autor = new Label();
            Autor.Style = (Style)this.Resources["MyStyleTextLeft"];
            String AI = "Студент 2 курса \n" +
                        "Факультета Математики, Физики и Информатики \n\n" +
                        "Большаков Антон Александрович \n\n" +
                        "Группы 121131 \n\n" +
                        "Направление/ Специальность: \n" +
                        "Фундаментальная информатика и информационные технологии \n\n" +
                        "Профиль/ Специализация: \n" +
                        "Открытые информационные системы";
            Autor.Content = AI;
            Autor.Width = 950;
            Autor.Height = 700;
            Autor.FontSize = 25;
            Autor.Foreground = new SolidColorBrush(Colors.White);
            Autor.Margin = new Thickness(150, 80, 0, -180);
            AutorGrid.Children.Add(Autor);

            Button menu;
            menu = new Button();
            menu.Style = (Style)this.Resources["MyStyleL"];
            menu.MinWidth = 200;
            menu.Height = 45;
            menu.Content = "В меню";
            menu.Margin = new Thickness(0, 0, 20, 20);
            AutorGrid.Children.Add(menu);
            menu.Click += new RoutedEventHandler(Menu_Click);
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   ProgramGame   +++ )

        private void ProgramGame()
        {
            Grid ProgramGrid = new Grid();
            this.Content = ProgramGrid;
            ProgramGrid.Margin = new Thickness(0);
            ProgramGrid.Background = new SolidColorBrush(Colors.Black);

            Label Fon;
            Fon = new Label();
            Fon.Style = (Style)this.Resources["FonInfo"];
            ProgramGrid.Children.Add(Fon);

            TextBox TextInfo = new TextBox();
            TextInfo.IsReadOnly = true;
            TextInfo.Style = (Style)this.Resources["ProgramInfo"];
            String AI = "\n Данное приложение было написано на языке C#\n с целью освоения клеточных автоматов.\n\n" +
                        " Клеточный автомат представляет собой совокупность пространства,\n" +
                        " поделенного на клетки, в каждой из которых содержится некое значение,\n" +
                        " и правил, задающих закон преобразования при совершении шага.\n\n\n\n" +
                        " О игре.\n\n" +
                        " Каждая клетка может находиться в двух состояниях: \"Живом\" и \"Мертвом\".\n" +
                        " Соседями клетки являются все восемь клеток,\n" +
                        " имеющих либо общую сторону, либо общую вершину.\n\n" +
                        " Правила \"Рождения\" и \"Вымирания\" для клеток:\n" +
                        " 1. Клетка рождается, если количество соседей равно 3.\n" +
                        " 2. Клетка умирает, если количество соседей либо больше 3 (перенаселение),\n" +
                        " либо меньше 2 (одиночество).\n\n\n" +
                        " В процессе игры популяция непрестанно претерпевает необычные,\n" +
                        " нередко очень красивые и всегда неожиданные изменения.\n\n" +
                        " Иногда первоначальная колония организмов вымирает, но,\n" +
                        " в большинстве своем, исходные конфигурации либо переходят в устойчивые\n" +
                        " и перестают изменяться, либо переходят в колебательный режим.\n\n" +
                        " Если все клетки перейдут в мертвое состояние, то игра закончится.\n\n\n" +
                        " Вы можете сохранить или загрузить игру, а также удалить уже созданные записи игр." +
                        "\n\n\n\n\n\n\n\n\n";
            TextInfo.Text = AI;
            TextInfo.Width = 978;
            TextInfo.Height = 495;
            TextInfo.FontSize = 24;
            TextInfo.Foreground = new SolidColorBrush(Colors.White);
            TextInfo.Margin = new Thickness(3, 14 , -3, -14);
            ProgramGrid.Children.Add(TextInfo);

            Label head;
            head = new Label();
            head.Style = (Style)this.Resources["Head"];
            head.Content = "O программе";
            head.Width = 430;
            head.Height = 100;
            head.FontSize = 43;
            head.Margin = new Thickness(20, 50, 0, 0);
            ProgramGrid.Children.Add(head);

            Button menu;
            menu = new Button();
            menu.Style = (Style)this.Resources["MyStyleL"];
            menu.MinWidth = 200;
            menu.Height = 45;
            menu.Content = "В меню";
            menu.Margin = new Thickness(0, 0, 20, 20);
            ProgramGrid.Children.Add(menu);
            menu.Click += new RoutedEventHandler(Menu_Click);
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   NewGame   +++ )

        private void NewGame()
        {
            stopFlag = true;
            for (int i = 0; i < HeightField; i++)
            {
                for (int j = 0; j < WidthField; j++)
                {
                    CellArray[i, j] = false;
                    TCellArray[i, j] = false;
                    ArrayField[i, j].Background = new SolidColorBrush(Colors.Transparent);
                }
            }

            generation = 0;
            GenerationOutput.Content = "Поколение: " + generation;
            return;
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   TheEnd   +++ )

        private void TheEnd()
        {
            Grid MainGrid = new Grid();
            this.Content = MainGrid;
            MainGrid.Margin = new Thickness(0);
            MainGrid.Background = new SolidColorBrush(Colors.Black);

            Label Fon;
            Fon = new Label();
            Fon.Style = (Style)this.Resources["TheEnd"];
            MainGrid.Children.Add(Fon);

            Label info;
            info = new Label();
            info.Style = (Style)this.Resources["HeadText2"];
            info.Content = "Количество поколений:  " + generation;
            info.Width = 650;
            info.Height = 80;
            info.FontSize = 23;
            info.Foreground = new SolidColorBrush(Colors.White);
            info.Margin = new Thickness(0, 490, 0, -490);
            MainGrid.Children.Add(info);

            Label HeadInfo;
            HeadInfo = new Label();
            HeadInfo.Style = (Style)this.Resources["HeadText"];
            HeadInfo.Content = "Все клетки мертвы!";
            HeadInfo.Width = 420;
            HeadInfo.Height = 80;
            HeadInfo.FontSize = 27;
            HeadInfo.Foreground = new SolidColorBrush(Colors.White);
            HeadInfo.Margin = new Thickness(0, 430, 0, -430);
            MainGrid.Children.Add(HeadInfo);

            Button menu;
            menu = new Button();
            menu.Style = (Style)this.Resources["MyStyleB"];
            menu.MinWidth = 200;
            menu.Height = 45;
            menu.Content = "В меню";
            menu.Margin = new Thickness(0, 0, 20, -500);
            MainGrid.Children.Add(menu);
            menu.Click += new RoutedEventHandler(MenuGame_Click);
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   SaveGame   +++ )

        private void SaveGame()
        {
            Grid MainGrid = new Grid();
            this.Content = MainGrid;
            MainGrid.Margin = new Thickness(0);
            MainGrid.Background = new SolidColorBrush(Colors.Black);

            Label Fon;
            Fon = new Label();
            Fon.Style = (Style)this.Resources["Fon"];
            MainGrid.Children.Add(Fon);

            Label Fon1;
            Fon1 = new Label();
            Fon1.Style = (Style)this.Resources["LoadingFon"];
            Fon1.Width = 700;
            Fon1.Height = 250;
            Fon1.Margin = new Thickness(0, -14, 0, 14);
            Fon1.Foreground = new SolidColorBrush(Colors.White);
            Fon1.Content = "Сохранение Игры";
            Fon1.FontSize = 50;
            MainGrid.Children.Add(Fon1);

            TextBox NFSG = new TextBox();
            NameFileSaveGame = NFSG;
            NameFileSaveGame.Style = (Style)this.Resources["NameFileSaveGame"];
            NameFileSaveGame.Width = 400;
            NameFileSaveGame.Height = 60;
            NameFileSaveGame.FontSize = 35;
            NameFileSaveGame.Foreground = new SolidColorBrush(Colors.White);
            MainGrid.Children.Add(NameFileSaveGame);

            Button loading;
            loading = new Button();
            loading.Style = (Style)this.Resources["MyStyleB"];
            loading.MinWidth = 250;
            loading.Content = "Сохранить";
            loading.Margin = new Thickness(-130, 170, 300, -270);
            MainGrid.Children.Add(loading);
            loading.Click += new RoutedEventHandler(SaveGame_Click);

            Button exit;
            exit = new Button();
            exit.Style = (Style)this.Resources["MyStyleB"];
            exit.MinWidth = 250;
            exit.Content = "Отмена";
            exit.Margin = new Thickness(130, 170, -300, -270);
            MainGrid.Children.Add(exit);
            if (generation == 0)
                exit.Click += new RoutedEventHandler(Menu_Click);
            else
                exit.Click += new RoutedEventHandler(PlayGame_Click);
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   PlayGame   +++ ) !!!

        private void PlayGame()
        {
            Grid GameGridTemp = new Grid();
            GameGrid = GameGridTemp;
            this.Content = GameGrid;
            GameGrid.Margin = new Thickness(0);
            GameGrid.Background = new SolidColorBrush(Colors.Black);

            Label Fon;
            Fon = new Label();
            Fon.Style = (Style)this.Resources["Fon"];
            GameGrid.Children.Add(Fon);

            Grid LeftGrid = new Grid();
            LeftGrid.Style = (Style)this.Resources["LeftGrid"];
            LeftGrid.Width = 330;
            LeftGrid.Height = 900;
            LeftGrid.Margin = new Thickness(0);
            GameGrid.Children.Add(LeftGrid);

            Grid RightGrid = new Grid();
            RightGrid.Style = (Style)this.Resources["RightGrid"];
            RightGrid.Width = 330;
            RightGrid.Height = 900;
            RightGrid.Margin = new Thickness(0);
            GameGrid.Children.Add(RightGrid);

            Label GenerationOutputTemp = new Label();
            GenerationOutput = GenerationOutputTemp;
            GenerationOutput.Style = (Style)this.Resources["HeadText2"];
            GenerationOutput.Foreground = new SolidColorBrush(Colors.White);
            GenerationOutput.Content = "Поколение: " + generation;
            GenerationOutput.Width = 430;
            GenerationOutput.Height = 100;
            GenerationOutput.FontSize = 25;
            GenerationOutput.Margin = new Thickness(0, 700, 0, -700);
            LeftGrid.Children.Add(GenerationOutput);

            Button ButtonGoTemp = new Button();
            ButtonGo = ButtonGoTemp;
            ButtonGo.Style = (Style)this.Resources["MyStyleB"];
            ButtonGo.Width = 180;
            ButtonGo.Height = 40;
            ButtonGo.Content = "Начать";
            ButtonGo.Margin = new Thickness(0, -250, 0, 250);
            RightGrid.Children.Add(ButtonGo);
            ButtonGo.Click += new RoutedEventHandler(Go_Click);

            Button ButtonStopTemp = new Button();
            ButtonStop = ButtonStopTemp;
            ButtonStop.Style = (Style)this.Resources["MyStyleB"];
            ButtonStop.Width = 180;
            ButtonStop.Height = 40;
            ButtonStop.Content = "Стоп";
            ButtonStop.Margin = new Thickness(0, -190, 0, 190);
            RightGrid.Children.Add(ButtonStop);
            ButtonStop.Click += new RoutedEventHandler(Stop_Click);

            Button ButtonNewGameTemp = new Button();
            ButtonNewGame = ButtonNewGameTemp;
            ButtonNewGame.Style = (Style)this.Resources["MyStyleB"];
            ButtonNewGame.Width = 180;
            ButtonNewGame.Height = 40;
            ButtonNewGame.Content = "Новая Игра";
            ButtonNewGame.Margin = new Thickness(0, -250, 0, 250);
            LeftGrid.Children.Add(ButtonNewGame);
            ButtonNewGame.Click += new RoutedEventHandler(NewGame_Click);

            Button ButtonExitTemp = new Button();
            ButtonExit = ButtonExitTemp;
            ButtonExit.Style = (Style)this.Resources["MyStyleB"];
            ButtonExit.Width = 180;
            ButtonExit.Height = 40;
            ButtonExit.Content = "В меню";
            ButtonExit.Margin = new Thickness(0, 300, 0, -300);
            RightGrid.Children.Add(ButtonExit);
            ButtonExit.Click += new RoutedEventHandler(MenuGame_Click);

            Button ButtonSaveTemp = new Button();
            ButtonSave = ButtonSaveTemp;
            ButtonSave.Style = (Style)this.Resources["MyStyleB"];
            ButtonSave.Width = 180;
            ButtonSave.Height = 40;
            ButtonSave.Content = "Сохранить";
            ButtonSave.Margin = new Thickness(0, -190, 0, 190);
            LeftGrid.Children.Add(ButtonSave);
            ButtonSave.Click += new RoutedEventHandler(Save_Click);

            Button loading = new Button();
            ButtonLoading = loading;
            ButtonLoading.Style = (Style)this.Resources["MyStyleB"];
            ButtonLoading.Width = 180;
            ButtonLoading.Height = 40;
            ButtonLoading.Content = "Загрузить";
            ButtonLoading.Margin = new Thickness(0, -130, 0, 130);
            LeftGrid.Children.Add(ButtonLoading);
            ButtonLoading.Click += new RoutedEventHandler(Loading_Click);

            WrapPanel PlayingField = new WrapPanel();
            PlayingField.Style = (Style)this.Resources["Field"];
            PlayingField.Width = 660;
            PlayingField.Height = 660;
            PlayingField.Margin = new Thickness(0);
            GameGrid.Children.Add(PlayingField);

            for (int i = 0; i < HeightField; i++)
            {
                for (int j = 0; j < WidthField; j++)
                {
                    ArrayField[i, j] = new Button();
                    ArrayField[i, j].Width = 15;
                    ArrayField[i, j].Height = 15;
                    if (CellArray[i, j])
                    {
                        ArrayField[i, j].Background = new SolidColorBrush(Colors.Gold);
                    }
                    else
                    {
                        ArrayField[i, j].Background = new SolidColorBrush(Colors.Transparent);
                    }
                    ArrayField[i, j].Margin = new Thickness(0);
                    ArrayField[i, j].Click += new RoutedEventHandler(Button_Click);
                    ArrayField[i, j].Tag = new Coords(i, j);
                    PlayingField.Children.Add(ArrayField[i, j]);
                }
            }
        }


//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   timerTick   +++ )

        private void timerTick(object sender, EventArgs e)
        {
            if (stopFlag == true)
            { return; }
            else
            {
                generation += 1;
                GenerationOutput.Content = "Поколение: ";
                GenerationOutput.Content = "Поколение: " + generation;

                for (int i = 0; i < HeightField; i++)
                {
                    for (int j = 0; j < WidthField; j++)
                    {
                        if (CellArray[i, j] == true)
                        {
                            ArrayField[i, j].Background = new SolidColorBrush(Colors.Gold);
                        }
                        else
                        {
                            ArrayField[i, j].Background = new SolidColorBrush(Colors.Transparent);
                            sg += 1;
                        }
                        ArrayField[i, j].Tag = new Coords(i, j);
                    }
                }
                if ((HeightField * WidthField) == sg)
                {
                    sg = 0;
                    stopFlag = true;
                    TheEnd();
                }
                sg = 0;
                NextGeneration();
            }
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   CellCount   +++ ) 

        private int CellCount(int y, int x)
        {
            int LifeCount = 0;
            int limit = WidthField - 1;

            if (((x - 1) >= 0) && ((y - 1) >= 0))
            {
                if (CellArray[y - 1, x - 1] == true)
                    LifeCount += 1;
            }
            if ((y - 1) >= 0)
            {
                if (CellArray[y - 1, x] == true)
                    LifeCount += 1;
            }
            if (((x + 1) <= limit) && ((y - 1) >= 0))
            {
                if (CellArray[y - 1, x + 1] == true)
                    LifeCount += 1;
            }
            if ((x - 1) >= 0)
            {
                if (CellArray[y, x - 1] == true)
                    LifeCount += 1;
            }
            if ((x + 1) <= limit)
            {
                if (CellArray[y, x + 1] == true)
                    LifeCount += 1;
            }
            if (((x - 1) >= 0) && ((y + 1) <= limit))
            {
                if (CellArray[y + 1, x - 1] == true)
                    LifeCount += 1;
            }
            if ((y + 1) <= limit)
            {
                if (CellArray[y + 1, x] == true)
                    LifeCount += 1;
            }
            if (((x + 1) <= limit) && ((y + 1) <= limit))
            {
                if (CellArray[y + 1, x + 1] == true)
                    LifeCount += 1;
            }
            return LifeCount;
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       ( +++   NextGeneration   +++ ) 

        public void NextGeneration()
        {
            for (int i = 0; i < HeightField; i++)
            {
                for (int j = 0; j < WidthField; j++)
                {
                    if ((CellCount(i, j) == 3) && (!CellArray[i, j])) { TCellArray[i, j] = true; }
                    if (((CellCount(i, j) == 3) || (CellCount(i, j) == 2)) && (CellArray[i, j])) { TCellArray[i, j] = true; }
                    if ((CellCount(i, j) < 2) && (CellArray[i, j])) { TCellArray[i, j] = false; }
                    if ((CellCount(i, j) > 3) && (CellArray[i, j])) { TCellArray[i, j] = false; }
                }
            }
            for (int i = 0; i < HeightField; i++)
            {
                for (int j = 0; j < WidthField; j++)
                {
                    CellArray[i, j] = TCellArray[i, j];
                    if (CellArray[i, j] == true)
                    {
                        ArrayField[i, j].Background = new SolidColorBrush(Colors.Gold);
                    }
                    else
                    {
                        ArrayField[i, j].Background = new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }


        public class Coords
        {
            private int i;
            private int j;
            public int I
            {
                get { return i; }
                set { i = I; }
            }
            public int J
            {
                get { return j; }
                set { j = J; }
            }
            public Coords(int a, int b)
            {
                i = a; j = b;
            }
        }
    }
}
