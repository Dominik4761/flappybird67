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
		int rekord = 0;

		bool pontozva1 = false;
		bool pontozva2 = false;

		bool vege = false;
		bool jatekElindult = false;

		Random rnd = new Random();

		public MainWindow()
		{
			InitializeComponent();

			timer.Interval = TimeSpan.FromMilliseconds(20);
			timer.Tick += JatekLoop;

			KeyDown += Ablak_KeyDown;

			jatekCanvas.Visibility = Visibility.Hidden;
			menuCanvas.Visibility = Visibility.Visible;
		}

		private void Konnyu_Click(object sender, RoutedEventArgs e)
		{
			oszlopSebesseg = 3.0;
			InditJatek();
		}

		private void Kozepes_Click(object sender, RoutedEventArgs e)
		{
			oszlopSebesseg = 6.0;
			InditJatek();
		}

		private void Nehez_Click(object sender, RoutedEventArgs e)
		{
			oszlopSebesseg = 7.5;
			InditJatek();
		}

		void InditJatek()
		{
			menuCanvas.Visibility = Visibility.Hidden;
			jatekCanvas.Visibility = Visibility.Visible;

			ResetJatek();
			timer.Start();
		}

		void ResetJatek()
		{
			vege = false;
			jatekElindult = false;

			pont = 0;
			tbPont.Text = "Pontszám: 0";

			tbVege.Visibility = Visibility.Collapsed;
			tbRekord.Visibility = Visibility.Collapsed;
			restartBtn.Visibility = Visibility.Collapsed;
			backBtn.Visibility = Visibility.Visible;

			sebesseg = 0;
			Canvas.SetTop(pengo, 206);
			Canvas.SetLeft(pengo, 28);

			Canvas.SetLeft(alsocso, 421);
			Canvas.SetTop(alsocso, 333);

			Canvas.SetLeft(felsocso, 421);
			Canvas.SetTop(felsocso, 17);

			Canvas.SetLeft(alsocso2, 234);
			Canvas.SetTop(alsocso2, 274);

			Canvas.SetLeft(felsocso2, 234);
			Canvas.SetTop(felsocso2, -42);

			pontozva1 = false;
			pontozva2 = false;
		}

		void JatekLoop(object sender, EventArgs e)
		{
			if (vege || !jatekElindult)
				return;

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
			var pRect = Hitbox(pengo, 0, 0, 0, 0);

			var a1 = Hitbox(alsocso, 0, 0, 0, 0);
			var f1 = Hitbox(felsocso, 0, 0, 0, 0);
			var a2 = Hitbox(alsocso2, 0, 0, 0, 0);
			var f2 = Hitbox(felsocso2, 0, 0, 0, 0);

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
			jatekElindult = false;

			timer.Stop();
			tbVege.Visibility = Visibility.Visible;

			if (pont > rekord)
			{
				rekord = pont;
			}

			tbRekord.Text = "Rekord: " + rekord;
			tbRekord.Visibility = Visibility.Visible;

			restartBtn.Visibility = Visibility.Visible;
			backBtn.Visibility = Visibility.Visible;
		}

		void Ablak_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				if (!vege && !jatekElindult)
				{
					jatekElindult = true;
					backBtn.Visibility = Visibility.Collapsed;
					sebesseg = -8;
				}
				else if (!vege && jatekElindult)
				{
					sebesseg = -8;
				}
				else if (vege)
				{
					ResetJatek();                 
					timer.Start();
				}
			}
		}

		private void restartBtn_Click(object sender, RoutedEventArgs e)
		{
			ResetJatek();
			timer.Start();
		}

		private void backBtn_Click(object sender, RoutedEventArgs e)
		{
			timer.Stop();

			jatekCanvas.Visibility = Visibility.Hidden;
			menuCanvas.Visibility = Visibility.Visible;

			vege = false;
			jatekElindult = false;
		}
	}
}
