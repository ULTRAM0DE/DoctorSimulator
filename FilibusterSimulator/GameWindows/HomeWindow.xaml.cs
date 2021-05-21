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
using DoctorSimulator.BusinessLogic;

namespace DoctorSimulator.GameWindows
{
	/// <summary>
	/// Логика взаимодействия для HomeWindow.xaml
	/// </summary>
	public partial class HomeWindow : Window
	{
		public Game Game;
		public Person Person;

		// Нужно для последующего закрытия при выходе в главное меню
		public MainWindow MainWindow;

		// Вывести сообщение на экран
		void DisplayMessage(string message) => tblDisplay.Text = message;

		// Стандартный конструктор
		public HomeWindow(MainWindow mainWindow)
		{
			InitializeComponent();
			MainWindow = mainWindow;

			InitGame();
			InitPerson();
			InitImages();
			InitButtons();
		}

		// Конструктор, использующийся для загрузки сохранений
		public HomeWindow(Game newGame, Person newPerson, MainWindow mainWindow)
		{
			InitializeComponent();
			MainWindow = mainWindow;

			InitGame(newGame);
			InitPerson(newPerson);
			InitImages();
			InitButtons();
		}

		// Инициализация игры
		public void InitGame(Game game = null)
		{
			if (game == null)
			{
				
				Game = new Game
				{

					GameHoursPerOneRealMinute = 4,
					DisplayText = "Добро пожаловать в Симулятор Врача!" +
					"\n\nВ правом верхнем углу вы видите такие характеристики, как: сытость, настроение, запас жизненных сил, Деньги и ранг." +
					"\n\nСлева от дисплея находятся действия, которые влияют на персонажа." +
					"\n\nСмысл игры - дойти до ранга `Врач`." +
					"\nВсего рангов 6: Перваш, Второй курс, третий курс, Четвертый и пятый курс, Интерн, Врач." +
					"\n\nДо Третьего курса ранг поднимается каждые два дня. Потом у вас откроется возможность работать." +
					"\n\nЧетвертый курс - 10 Дней работы, Интерн - 25 Дней работы, Врач - 50 Дней работы."
				};
			}
			else
			{
				Game = game;
			}

			DisplayMessage(Game.DisplayText);

			
		}

		// Инициализация игрока
		public void InitPerson(Person person = null)
		{
			if (person == null)
			{
				Person = new Person
				{
					Satiety = 100,
					Mood = 100,
					Intelect = 100,
				
					Money = 1000,
					Rank = new Rank("Перваш"),

					IsMoored = false,
					WorkDay = 0,
					CurrentTime = new TimeSpan(0, 0, 0, 0)
				};
			}
			else
			{
				Person = person;
			}

			if (Person.WorkDay > 0)
				tblDayWork.Text = "Заработано: " + Person.WorkDay.ToString();
			else
				tblDayWork.Visibility = Visibility.Hidden;




			// Person.Notify -> DisplayMessage
			Person.Notify += DisplayMessage;

			// Методы класса Game, вызов которых возможен только после инициализация игрока
			Game.SetPersonAndHomeWindow(this, Person);
			Game.RefreshCharacteristics();
			Game.RefreshSomeUIElements();
			Game.ProcessTime();
		}

		// Кнопка "Пауза"
		void BtnPauseGame_Click(object sender, RoutedEventArgs e)
		{
			PauseWindow pauseWindow = new PauseWindow(this);

			WPF_Misc.FocusWindow(pauseWindow);
			WPF_Misc.OpenPauseWindow(this, pauseWindow);
		}

		// Метод, отвечающий за выполнение других методов при нажатии на кнопки
		void BtnActions_Click(object sender, RoutedEventArgs e)
		{
			switch ((sender as Button).Name)
			{
				case "btnEat": Person.Eat(); break;
				case "btnSleep": Person.Sleep(); break;
				
				
				case "btnDayWork":
					Person.DayWork();
					tblDayWork.Text = "Выполнено работ: " + Person.WorkDay.ToString();
					break;
			}

			Game.RefreshCharacteristics();
		}

		// Инициализация иконок характеристик
		void InitImages()
		{
			imgSatiety.Source = WPF_Misc.ImageSourceFromBitmap(DoctorSimulator.Properties.Resources.Satiety);
			imgMood.Source = WPF_Misc.ImageSourceFromBitmap(WPF_Misc.GetMoodImage(Person.Mood));
			imgIntelect.Source = WPF_Misc.ImageSourceFromBitmap(DoctorSimulator.Properties.Resources.Intelect);
			imgGold.Source = WPF_Misc.ImageSourceFromBitmap(DoctorSimulator.Properties.Resources.Gold);
			imgRank.Source = WPF_Misc.ImageSourceFromBitmap(DoctorSimulator.Properties.Resources.Rank);
		}

		// Инициализация кнопок
		void InitButtons()
		{
			btnPauseGame.Click += BtnPauseGame_Click;
			btnEat.Click += BtnActions_Click;
			btnSleep.Click += BtnActions_Click;
			
			

			btnDayWork.Click += BtnActions_Click;
		}

		public void btnBooks_Click(object sender, RoutedEventArgs e)
		{
			WPF_Misc.OpenPauseWindow(this, new LITER(Person));
		}

	}
}
