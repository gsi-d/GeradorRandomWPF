using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeradorStringWPF.Model
{
    public class DadosAleatorios : DependencyObject
    {

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Localizacao { get; set; }
        public int Qtde { get; set; }
        public double Valor { get; set; }
        public DateTime Data { get ; set; }
        public Color Cores { get; set; }
        public bool Check { get; set; }
        public int Linhas { get; set; }
    }
}
