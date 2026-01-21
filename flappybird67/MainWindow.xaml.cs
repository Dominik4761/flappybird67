using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace flappybird67
{
	public partial class MainWindow : Window
	{
		DispatcherTimer timer = new DispatcherTimer();
		double sebesseg = 0;
		double gravitacio = 0.6;

		double oszlopSebesseg = 3.0;
		const double res = 140;
		const double tavolsag = 260;

		int pont = 0;

		bool pontozva1 = false;
		bool pontozva2 = false;

		Random rnd = new Random();

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

			MozgatCsopar(alsocso, felsocso, alsocso2, ref pontozva1);
			MozgatCsopar(alsocso2, felsocso2, alsocso, ref pontozva2);

			tbPont.Text = "Pontszám: " + pont;
		}

		void MozgatCsopar(Rectangle also, Rectangle felso, Rectangle masikAlso, ref bool pontozva)
		{
			double x = Canvas.GetLeft(also) - oszlopSebesseg;

			Canvas.SetLeft(also, x);
			Canvas.SetLeft(felso, x);

			double pengoX = Canvas.GetLeft(pengo) + pengo.Width;

			if (!pontozva && pengoX > x + also.Width)
			{
				pont++;
				pontozva = true;
			}

			if (x < -also.Width)
			{
				double masikX = Canvas.GetLeft(masikAlso);
				double ujX = masikX + tavolsag;

				Canvas.SetLeft(also, ujX);
				Canvas.SetLeft(felso, ujX);

				double alsoTop = rnd.Next(260, 400);
				Canvas.SetTop(also, alsoTop);
				Canvas.SetTop(felso, alsoTop - res - felso.Height);

				pontozva = false;
			}
		}

		void Ablak_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				sebesseg = -8;
			}
		}
	}
}
