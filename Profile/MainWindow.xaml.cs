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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Profile
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/*
		 * Реализовать экран профиля.
		 * На экране должна быть возможность загрузить фото, заполнить имя и адрес.
		 * Все данные должны куда-то сохраниться и при переоткрытии программы отобразиться.
		*/
		private Guid ProfileId = Guid.Parse("49ae8f3e-09bb-4f70-baf8-b002e4eef8d3");

		public MainWindow()
		{
			InitializeComponent();


			var profile = new ProfileClass
			{
				Id = ProfileId
			};

			try // Первое открытие - профиля нет
			{
				using (var context = new Context("Server=BorisHOME\\Boris;Database=UserProfile;Trusted_Connection=True;"))
				{
					context.Profiles.Add(profile);
					context.SaveChanges();
				}
			}
			catch // Если профиль уже существует
			{
				using (var context = new Context("Server=BorisHOME\\Boris;Database=UserProfile;Trusted_Connection=True;"))
				{
					var thisProfile = context.Profiles.Where(p => p.Id.ToString() == "49ae8f3e-09bb-4f70-baf8-b002e4eef8d3").FirstOrDefault<ProfileClass>();

					string userImageProfile = thisProfile.ImagePath;
					if (userImageProfile == null)
					{
						userImageProfile = "https://st.depositphotos.com/1779253/5140/v/950/depositphotos_51405259-stock-illustration-male-avatar-profile-picture-use.jpg";
					}

					userFirstName.Text = thisProfile.FirstName;
					userLastName.Text = thisProfile.LastName;
					userAddress.Text = thisProfile.Address;
					userImage.Source = new BitmapImage(new Uri(userImageProfile, UriKind.Absolute));
				}
			}
		}

		private void SaveChangesClicked(object sender, RoutedEventArgs e)
		{
			if (userFirstName.Text == "" || userLastName.Text == "" || userAddress.Text == "")
			{
				MessageBox.Show("Необходимо заполнить все поля");
				return;
			}


			using (var context = new Context("Server=BorisHOME\\Boris;Database=UserProfile;Trusted_Connection=True;"))
			{
				var profile = context.Profiles.Where(p => p.Id.ToString() == "49ae8f3e-09bb-4f70-baf8-b002e4eef8d3").FirstOrDefault<ProfileClass>();

				profile.FirstName = userFirstName.Text;
				profile.LastName = userLastName.Text;
				profile.Address = userAddress.Text;

				context.SaveChanges();
			}
			MessageBox.Show("Успешно сохранено");
		}

		private void DownloadImageClicked(object sender, RoutedEventArgs e)
		{
			ImageUrlWindow imageUrlWindow = new ImageUrlWindow();
			imageUrlWindow.Show();
		}
	}
}
