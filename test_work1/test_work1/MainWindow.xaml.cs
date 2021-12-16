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
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Drawing;
using Image = System.Windows.Controls.Image;

namespace test_work1
{
    public partial class MainWindow : Window
    {
        List<string> urlList = new List<string>();
        private int i, j, k;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            string p1 = textBox1.Text;
            string p2 = textBox2.Text;
            string p3 = textBox3.Text;
            BitmapImage bi1 = new BitmapImage();
            BitmapImage bi2 = new BitmapImage();
            BitmapImage bi3 = new BitmapImage();
            string str = (string)((Button)e.OriginalSource).Name;
            NumCheck(ref i, ref j, ref k);
            if (pbStat.Value % 100 == 0)
            {
                pbStat.Value = 0;
                pbStat.Maximum = 0;
            }
            if (str == startAll.Name)
            {
                BitmapCheck(bi1, p1, picture1, start1, stop1);
                BitmapCheck(bi2, p2, picture2, start2, stop2);
                BitmapCheck(bi3, p3, picture3, start3, stop3);
            }
            else if (str == start1.Name)
            {
                BitmapCheck(bi1, p1, picture1, start1, stop1);
            }
            else if (str == start2.Name)
            {
                BitmapCheck(bi2, p2, picture2, start2, stop2);
            }
            else if (str == start3.Name)
            {
                BitmapCheck(bi3, p3, picture3, start3, stop3);
            }
        }

        public void DownloadProg(object sender, DownloadProgressEventArgs e)
        {
            if (picture1.Source != null && sender.ToString() == picture1.Source.ToString())
            {
                if (i != -1)
                {
                    pbStat.Value += (e.Progress - i);
                    i = e.Progress;
                }
                else
                {
                    picture1.Source = null;
                }
            }
            if (picture2.Source != null && sender.ToString() == picture2.Source.ToString())
            {
                if (j != -1)
                {
                    pbStat.Value += (e.Progress - j);
                    j = e.Progress;
                }
                else
                {
                    picture2.Source = null;
                }
            }
            if (picture3.Source != null && sender.ToString() == picture3.Source.ToString())
            {
                if (k != -1)
                {
                    pbStat.Value += (e.Progress - k);
                    k = e.Progress;
                }
                else
                {
                    picture3.Source = null;
                }
            }
        }

        private void DownloadComp(object sender, EventArgs e)
        {
            if (picture1.Source != null && sender.ToString() == picture1.Source.ToString())
            {
                i = 0;
                ButSwitcher(start1, stop1);
            }
            if (picture2.Source != null && sender.ToString() == picture2.Source.ToString())
            {
                j = 0;
                ButSwitcher(start2, stop2);
            }
            if (picture3.Source != null && sender.ToString() == picture3.Source.ToString())
            {
                k = 0;
                ButSwitcher(start3, stop3);
            }
            urlList.Add(sender.ToString());
            StartAllEnable(start1, start2, start3);
        }

        private void DownloadFail(object sender, ExceptionEventArgs e)
        {
            if (picture1.Source != null && sender.ToString() == picture1.Source.ToString())
            {
                ButSwitcher(start1, stop1);
            }
            if (picture2.Source != null && sender.ToString() == picture2.Source.ToString())
            {
                ButSwitcher(start2, stop2);
            }
            if (picture3.Source != null && sender.ToString() == picture3.Source.ToString())
            {
                ButSwitcher(start3, stop3);
            }
            StartAllEnable(start1, start2, start3);
            pbStat.Maximum -= 100;
        }

        void BitmapCheck(BitmapImage bi, string str, Image pic, Button butStart, Button butStop)
        {
            try
            {
                if (str.Contains(".jpg") || str.Contains(".png") || str.Contains(".jpeg"))
                {
                    if (!urlList.Contains(str))
                    {
                        startAll.IsEnabled = false;
                        ButSwitcher(butStop, butStart);
                        pbStat.Maximum += 100;
                    }
                    bi.BeginInit();
                    bi.UriSource = new Uri(str);
                    bi.DownloadFailed += DownloadFail;
                    bi.DownloadProgress += DownloadProg;
                    bi.DownloadCompleted += DownloadComp;
                    bi.EndInit();
                    pic.Source = bi;
                }
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        void ButSwitcher (Button b1, Button b2)
        {
            b1.IsEnabled = true;
            b2.IsEnabled = false;
        }
        void StartAllEnable(Button b1, Button b2, Button b3)
        {
            if (b1.IsEnabled == true && b2.IsEnabled == true && b3.IsEnabled == true)
                startAll.IsEnabled = true;
            else startAll.IsEnabled = false;
        }
        void NumCheck (ref int i, ref int j, ref int k)
        {
            if (i == -1)
                i = 0;
            if (j == -1)
                j = 0;
            if (k == -1)
                k = 0;
        }
        int StopFunc(int num, Button b1, Button b2)
        {
            pbStat.Value -= num;
            pbStat.Maximum -= 100;
            num = -1;
            ButSwitcher(b1, b2);
            return num;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            string str = (string)((Button)e.OriginalSource).Name;
            if (str == stop1.Name)
                i = StopFunc(i, start1, stop1);
            if (str == stop2.Name)
                j = StopFunc(j, start2, stop2);
            if (str == stop3.Name)
                k = StopFunc(k, start3, stop3);
            StartAllEnable(start1, start2, start3);
        }
    }
}
