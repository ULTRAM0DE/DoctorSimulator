using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSimulator.BusinessLogic
{
	public class Person
	{
		// Делегат и событие для отправки сообщений во внешнюю среду
		public delegate void NotificationHandler(string message);
		public event NotificationHandler Notify;

		//Основные характеристики игрока
		public int Satiety { get; set; }
		public int Mood { get; set; }
		
		public int Intelect { get; set; }
		public int Money { get; set; }
		public Rank Rank { get; set; }

		public bool IsMoored { get; set; }
		public int WorkDay { get; set; }
		public TimeSpan CurrentTime { get; set; }



		private int eatingsCount;

		private static Random random;

		public Person()
		{
			eatingsCount = 0;
			random = new Random();
		}

		//Получить строку с текущим временем в формате: текущий день | часы:минуты
		public string GetTimeString()
		{
			// DayOfYear можно использовать для счета кол-ва возможных дней без учета кол-ва месяцев
			return $"{CurrentTime.Days} день | {CurrentTime.Hours}:{CurrentTime.Minutes}";
		}

		public void Eat()
		{
			if (SpendResourcesByHours(0.5))
			{
				string messageText = "поели=) \n+10 к настроению \n+50 к сытости \n+20 к запасу жизненных сил \nПрошло 30 минут";

				// Каждый третий прием пищи -250 денег
				if (eatingsCount % 3 == 0)
				{
					if (eatingsCount > 0)
					{
						if (SubstractGold(250))
							messageText += "\n-250 деняк";
					}
				}

				AddSatiety(50);
				AddMood(10);
				
				CurrentTime = CurrentTime.Add(TimeSpan.FromMinutes(30));

				eatingsCount++;

				Notify?.Invoke(messageText);
			}
		}
		public void Books(string name)
        {
			switch(name)
            {
				case "Chainick": SubstractGold(300); break;
				case "Sred": SubstractGold(1000); break;
				case "vis": SubstractGold(100000); break;
				case "BOG": SubstractGold(100000000); break;
			}

			SubstractMood(40);
			AddIntelect(100);
        }
		public void Sleep()
		{
			//if (SpendResourcesByHours(8))
			if (SubstractSatiety(3 * 8))
			{
			
				AddMood(10);
				CurrentTime = CurrentTime.Add(TimeSpan.FromHours(8));
				Notify?.Invoke("Вы поспали =) \n+10 к настроению" +
					"\nЗапас жизненных сил пополен до 100 \nПрошло 8 часов");
			}
		}

		

		public void DayWork()
		{
			if (SpendResourcesByHours(1))
			{
				int randomNumber = random.Next(0, 100);
				int loot;

				if (Rank.Name == "Перваш")
					loot = 3000;
				else if (Rank.Name == "Второй курс")
					loot = 5000;
				else if (Rank.Name == "Третий курс")
					loot = 7000;
				else if (Rank.Name == "Четвертый курс")
					loot = 9000;
				else if (Rank.Name == "Интерн")
                {

					if (Intelect <= 90)
					{
						loot = 0;
						Notify?.Invoke("Че тупой?Книги читать надо)))");
					}
					else
						loot = 9000;
				}
				else 
					loot = 0;

				Money += loot;

				WorkDay++;

				Notify?.Invoke($"Вы поработали \nЗаработок: {loot} \nПрошел 1 час");
			}
		}

		// Косвенные траты по-прохождению hours часов
		private bool SpendResourcesByHours(double hours)
		{
			return SubstractSatiety((int)(3 * hours));
		}

		// Методы добавления и вычитания некоторых характеристик игрока
		private void AddSatiety(int satiety)
		{
			if (Satiety + satiety >= 100)
				Satiety = 100;
			else
				Satiety += satiety;
		}
		private void AddIntelect(int intelect)
        {
			if (Intelect + intelect >= 1000)
				Intelect = 50;
			else
				Intelect += intelect;
			
        }

		public bool SubstractIntelect(int mood)
		{
			if (mood < 0)
				return false;

			if (Mood - mood < 0)
			{
				Notify?.Invoke("Вы слишком тупой");
				return false;
			}
			else
			{
				Mood -= mood;
				return true;
			}
		}

		private void AddMood(int mood)
		{
			if (Mood + mood >= 100)
				Mood = 100;
			else
				Mood += mood;
		}

		

		// public только из-за юнит-тестов
		public bool SubstractSatiety(int satiety)
		{
			if (satiety < 0)
				return false;

			if (Satiety - satiety < 0)
			{
				Notify?.Invoke("Вы слишком голодны");
				return false;
			}
			else
			{
				Satiety -= satiety;
				return true;
			}
		}

        public bool SubstractMood(int mood)
		{
			if (mood < 0)
				return false;

			if (Mood - mood < 0)
			{
				Notify?.Invoke("У вас слишком плохое настроение");
				return false;
			}
			else
			{
				Mood -= mood;
				return true;
			}
		}

		public bool SubstractGold(int gold)
		{
			if (gold < 0)
				return false;

			if (Money - gold < 0)
			{
				Notify?.Invoke("У вас слишком мало деняк");
				return false;
			}
			else
			{
				Money -= gold;
				return true;
			}
		}
	}
}
