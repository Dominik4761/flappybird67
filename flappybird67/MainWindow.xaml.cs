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

		bool vege = false;

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
			if (vege) return;

			sebesseg += gravitacio;
			Canvas.SetTop(pengo, Canvas.GetTop(pengo) + sebesseg);

			MozgatCsopar(alsocso, felsocso, alsocso2, ref pontozva1);
			MozgatCsopar(alsocso2, felsocso2, alsocso, ref pontozva2);

			tbPont.Text = "Pontszám: " + pont;

			EllenorizUtkozes();
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

		Rect Hitbox(Rectangle r, double bal, double fel, double jobb, double le)
		{
			double x = Canvas.GetLeft(r) + bal;
			double y = Canvas.GetTop(r) + fel;
			double w = Math.Max(1, r.Width - bal - jobb);
			double h = Math.Max(1, r.Height - fel - le);
			return new Rect(x, y, w, h);
		}

		void EllenorizUtkozes()
		{
			var pRect = Hitbox(pengo, 18, 15, 18, 15);

			var a1 = Hitbox(alsocso, 2, 1, 2, 1);
			var f1 = Hitbox(felsocso, 2, 1, 2, 1);
			var a2 = Hitbox(alsocso2, 2, 1, 2, 1);
			var f2 = Hitbox(felsocso2, 2, 1, 2, 1);

			if (pRect.IntersectsWith(a1) || pRect.IntersectsWith(f1) || pRect.IntersectsWith(a2) || pRect.IntersectsWith(f2))
			{
				JatekVege();
				return;
			}

			double py = Canvas.GetTop(pengo);
			double h = jatekCanvas.Height;

			if (py < 0 || py + pengo.Height > h)
			{
				JatekVege();
			}
		}

		void JatekVege()
		{
			vege = true;
			timer.Stop();
			tbVege.Visibility = Visibility.Visible;
		}

		void Ablak_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space && !vege)
			{
				sebesseg = -8;
			}
		}
	}
}
