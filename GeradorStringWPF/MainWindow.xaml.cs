using GeradorStringWPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GeradorStringWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<DadosAleatorios> listDados = new List<DadosAleatorios>();
        public MainWindow()
        {
            InitializeComponent();
            lblProcessamento.Content = 0 + " ms";
            lblLinhas.Content = "";
            pgbDados.Visibility = Visibility.Collapsed;
            
        }

        private async void acaoGerar(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            sw.Start();
            sw2.Start();

            string descricao = "";
            string localizacao = "";
            int qtde;
            double valor;
            DateTime data = new DateTime();
            Color cores = new Color();
            bool check = new bool();
            listDados.Clear();
            pgbDados.Value = 0;
            pgbDados.Maximum = Convert.ToInt32(ttbNrLinhas.Text);
            lblLinhas.Content = ttbNrLinhas.Text;

            for (int i = 0; i < Convert.ToInt32(ttbNrLinhas.Text); i++)
            {
                descricao = stringAleatoria(Convert.ToInt32(ttbMin.Text), Convert.ToInt32(ttbMax.Text));
                localizacao = stringAleatoria(Convert.ToInt32(ttbMin.Text), Convert.ToInt32(ttbMax.Text));
                qtde = qtdeAleatoria();
                valor = valorAleatorio();
                data = dataAleatoria();
                cores = corAleatoria();
                check = checkAleatorio();
                listDados.Add(new DadosAleatorios() { Id = i, Descricao = descricao, Localizacao = localizacao, Qtde = qtde, Valor = valor, Data = data, Cores = cores, Check = check });
                sw2.Start();
                lblProcessamento.Content = sw2.ElapsedMilliseconds + "," + qtdeAleatoria() + " ms";
            }
            dgvDados.ItemsSource = listDados;

            if (cbxVarrer.IsChecked == true)
            {
                lblProcessamento.Visibility = Visibility.Collapsed;
                txtProcessamento.Visibility = Visibility.Collapsed;
                txtGerandoLinhas.Visibility = Visibility.Visible;
                for (int i = 0; i < dgvDados.Items.Count - 1; i++)
                {
                    pgbDados.Visibility = Visibility.Visible;
                    dgvDados.SelectedIndex = i;
                    pgbDados.Value += 1;
                    dgvDados.ScrollIntoView(dgvDados.SelectedItem);
                    await Task.Delay(10);
                    lblCPU.Content = GetCpuUsage();
                }
                sw.Stop();
                lblProcessamento.Content = sw.ElapsedMilliseconds + "," + qtdeAleatoria() + " ms";
            }
            txtGerandoLinhas.Visibility = Visibility.Collapsed;
            lblProcessamento.Visibility = Visibility.Visible;
            txtProcessamento.Visibility = Visibility.Visible;
            pgbDados.Visibility = Visibility.Collapsed;
            
        }

        public static string stringAleatoria(int min, int max)
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            int tamanho = random.Next(min, max);
            var result = new string(
                Enumerable.Repeat(chars, tamanho)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public static int qtdeAleatoria()
        {
            Random random = new Random();
            int result = random.Next(9999);
            return result;
        }

        public static double valorAleatorio()
        {
            Random random = new Random();
            double result = random.NextDouble();
            double resultado = qtdeAleatoria() + result;
            return resultado;
        }

        public static DateTime dataAleatoria()
        {
            DateTime data = new DateTime();
            Random random = new Random();

            int y = random.Next(1900, 2500);
            int m = random.Next(01, 13);
            int d;
            if (m == 2)
                d = random.Next(1, 28);
            else if (m == 4 || m == 6 || m == 9 || m == 11)
                d = random.Next(1, 31);
            else
                d = random.Next(1, 32);

            data = new DateTime(y, m, d);
            return data.Date;
        }

        public static Color corAleatoria()
        {
            Random random = new Random();
            byte r = Convert.ToByte(random.Next(00, 256));
            byte g = Convert.ToByte(random.Next(00, 256));
            byte b = Convert.ToByte(random.Next(00, 256));
            Color result = Color.FromRgb(r, g, b);

            return result;
        }

        public static bool checkAleatorio()
        {
            Random random = new Random();
            int result = random.Next(0, 3);
            switch (result)
            {
                case 1:
                    return true;
                case 2:
                    return false;
                default:
                    return false;
            }
        }

        public int GetCpuUsage()
        {
            Process[] runningNow = Process.GetProcesses();

            foreach (Process process in runningNow)
            {
                using (PerformanceCounter cpuUsage = new PerformanceCounter("Process", "% Processor Time"))
                using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", process.ProcessName))
                using (PerformanceCounter memProcess = new PerformanceCounter("Memory", "Available MBytes"))
                {
                    pcProcess.NextValue();
                    cpuUsage.NextValue();
                    Thread.Sleep(6000);

                    double cpuUse = Math.Round(pcProcess.NextValue() / cpuUsage.NextValue() * 100, 2);

                    // Check for Not-A-Number (Division by Zero)
                    if (Double.IsNaN(cpuUse))
                        cpuUse = 0;
                    return Convert.ToInt32(cpuUse);
                }
            }
            return 0;

                
            /*System.Diagnostics.PerformanceCounter cpuCounter;
            cpuCounter = new System.Diagnostics.PerformanceCounter("Process", "% Processor Time", System.Diagnostics.Process.GetProcessesByName("wait").First().ProcessName);
            return (int)cpuCounter.NextValue();*/
        }
    }
}
