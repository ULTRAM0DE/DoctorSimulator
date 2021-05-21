using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSimulator.BusinessLogic
{
    public struct Rank
	{
		public int Id;
		public string Name;

		public Rank(string name)
		{
			Name = name;

			switch (name)
			{
				case "Перваш": Id = 1; break;
				case "Второй курс": Id = 2; break;
				case "Третий курс": Id = 3; break;
				case "Четвертый курс": Id = 4; break;
				case "Интерн": Id = 5; break;
				case "Врач": Id = 6; break;
				default: throw new ArgumentException($"Нет ранга {name}");
			}
		}

		public static string GetRankNameById(int id)
		{
			switch (id)
			{
				case 1: return "Перваш";
				case 2: return "Второй курс";
				case 3: return "Третий курс";
				case 4: return "Четвертый курс";
				case 5: return "Интерн";
				case 6: return "Врач";
				default: throw new ArgumentException($"Нет ранга с id {id}");
			}
		}
	}
}
