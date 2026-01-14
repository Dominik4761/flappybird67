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
using System.Windows.Threading;

namespace flappybird67
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer timer = new DispatcherTimer();
		double sebesseg = 0;
		double gravitacio = 0.6;

		public MainWindow()
		{
			InitializeComponent();

			timer.Interval = TimeSpan.FromMilliseconds(20);
			timer.Tick += JatekLoop;
			timer.Start();

			KeyDown += Ablak_KeyDown;
		}

		void JatekLoop(object sender, EventArgs e)
		{
			sebesseg += gravitacio;
			Canvas.SetTop(pengo, Canvas.GetTop(pengo) + sebesseg);
		}

		void Ablak_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				sebesseg = -8;
			}
		}
	}
