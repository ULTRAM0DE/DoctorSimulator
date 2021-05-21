using DoctorSimulator.BusinessLogic;
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

namespace DoctorSimulator.GameWindows
{
	/// <summary>
	/// Логика взаимодействия для LITER.xaml
	/// </summary>
	public partial class LITER : Window
	{
		private Person person;
		public LITER(Person person)
		{
			InitializeComponent();

			this.person = person;
		}
		
	

        private void Book_Click(object sender, RoutedEventArgs e)
        {
			person.Books((sender as Button).Name);
		}

        private void Closewindow_Click(object sender, RoutedEventArgs e)
        {
			this.Close();
        }
    }
}
