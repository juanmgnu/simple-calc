using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Constructor
        public MainWindow()
        {
            InitializeComponent();
        }


        // Botones
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            string value = (string)button.Content;

            if (IsNumber(value))
            {
                HandleNumbers(value);
            }
            else if (IsOperator(value))
            {
                HandleOperators(value);
            }
            else if (value == ",")
            {
                HandleComma();
            }
            else if (value == "C" || value == "CE")
            {
                HandleClear(value);
            }
            else if (value == "=")
            {
                HandleEquals(Screen.Text);
            }
        }

        private void ButtonConvertClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Screen.Text) && IsNumber(Screen.Text))
            {
                Button button = (Button)sender;
                Int32.TryParse(Screen.Text, out int number);

                if (button.Name == "ButtonBin")
                {
                    Screen.Text = ConvertToBinary(number);
                }
                else
                {
                    Screen.Text = ConvertToHex(number);
                }
            }
        }


        // Menu
        private void SalirMenuItemClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =  MessageBox.Show("¿Seguro que quiere salir?", "Atención", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void ModoAvanzadoMenuItemChecked(object sender, RoutedEventArgs e)
        {
            EspacioModoAvanzado.Visibility = Visibility.Visible;
        }

        private void ModoAvanzadoMenuItemUnchecked(object sender, RoutedEventArgs e)
        {
            EspacioModoAvanzado.Visibility = Visibility.Collapsed;
        }


        // Métodos Auxiliares
        private bool IsNumber(string possibleNumber)
        {
            return double.TryParse(possibleNumber, out _);
        }

        private bool IsOperator(string possibleOperator)
        {
            return possibleOperator == "+" || possibleOperator == "-" || possibleOperator == "*" ||
                possibleOperator == "/" || possibleOperator == "^" || possibleOperator == "Sqrt" || 
                possibleOperator == "%" || possibleOperator == "!" || possibleOperator == "Log" ||
                possibleOperator == "Sen" || possibleOperator == "Cos" || possibleOperator == "Tan";
        }

        private bool ContainsOtherOperators(string screenContent)
        {
            return screenContent.Contains("+") || screenContent.Contains("-") || screenContent.Contains("*") || 
                screenContent.Contains("/") || screenContent.Contains("^") || screenContent.Contains("Sqrt") ||
                screenContent.Contains("%") || screenContent.Contains("!") || screenContent.Contains("Log") || 
                screenContent.Contains("Sen") || screenContent.Contains("Cos") || screenContent.Contains("Tan");
        }

        private string FindOperator(string screenContent)
        {
            if (screenContent.Contains("Sqrt") || screenContent.Contains("Log") || screenContent.Contains("Sen") || screenContent.Contains("Cos") || screenContent.Contains("Tan"))
            {
                return screenContent.Split(" ")[0]; // Retornamos el primer elemento del array de strings, que sería el operador buscado
            }
            else
            {
                foreach (char c in screenContent)
                {
                    if (IsOperator(c.ToString()))
                    {
                        return c.ToString();
                    }
                }
            }

            return String.Empty;
        }

        private void HandleNumbers(string value)
        {
            if (String.IsNullOrEmpty(Screen.Text))
            {
                Screen.Text = value;
            }
            else
            {
                Screen.Text += value;
            }
        }

        private void HandleComma()
        {
            int cantidadDeNumeros = Screen.Text.Split(new char[] { '+', '-', '*', '/' }).Length;
            int cantidadDeComas = Screen.Text.Where(c => c == ',').Count();

            if (!String.IsNullOrEmpty(Screen.Text))
            {
                if ((cantidadDeNumeros == 1 && cantidadDeComas == 0) || (cantidadDeNumeros == 2 && cantidadDeComas == 1))
                {
                    Screen.Text += ",";
                }
            }
        }

        private void HandleClear(string value)
        {
            if (value == "C")
            {
                if (Screen.Text.Length > 1)
                {
                    Screen.Text = Screen.Text.Remove(Screen.Text.Length - 1);
                }
                else
                {
                    Screen.Clear();
                }
            }
            else if (value == "CE")
            {
                Screen.Clear();
            }
        }

        private void HandleOperators(string value)
        {
            if (!ContainsOtherOperators(Screen.Text) && !String.IsNullOrEmpty(Screen.Text))
            {
                if (value == "Sqrt" || value == "Log" || value == "Sen" || value == "Cos" || value == "Tan")
                {
                    Screen.Text = value + " " + Screen.Text;
                }
                else
                {
                    Screen.Text += value;
                }
            }
        }

        private void HandleEquals(string screenContent)
        {
            string op = FindOperator(screenContent);

            // Arreglar bien el tema de los números negativos. Esto es temporal. 
            if (!String.IsNullOrEmpty(op))
            {
                switch (op)
                {
                    case "+":
                        Screen.Text = Sum();
                        break;
                    case "-":
                        Screen.Text = Sub();
                        break;
                    case "*":
                        Screen.Text = Mul();
                        break;
                    case "/":
                        Screen.Text = Div();
                        break;
                    case "^":
                        Screen.Text = Pow();
                        break;
                    case "Sqrt":
                        Screen.Text = Sqrt();
                        break;
                    case "%":
                        Screen.Text = Mod();
                        break;
                    case "!":
                        Screen.Text = Factorial();
                        break;
                    case "Log":
                        Screen.Text = Log();
                        break;
                    case "Sen":
                        Screen.Text = Sen();
                        break;
                    case "Cos":
                        Screen.Text = Cos();
                        break;
                    case "Tan":
                        Screen.Text = Tan();
                        break;
                }
            }
        }


        // Operaciones
        private string Sum()
        {
            string[] numbers = Screen.Text.Split("+");

            double.TryParse(numbers[0], out double n1);
            double.TryParse(numbers[1], out double n2);

            return Math.Round(n1 + n2, 12).ToString();
        }

        private string Sub()
        {
            string[] numbers = Screen.Text.Split("-");

            double.TryParse(numbers[0], out double n1);
            double.TryParse(numbers[1], out double n2);

            return Math.Round(n1 - n2, 12).ToString();
        }

        private string Mul()
        {
            string[] numbers = Screen.Text.Split("*");

            double.TryParse(numbers[0], out double n1);
            double.TryParse(numbers[1], out double n2);

            return Math.Round(n1 * n2, 12).ToString();
        }

        private string Div()
        {
            string[] numbers = Screen.Text.Split("/");

            double.TryParse(numbers[0], out double n1);
            double.TryParse(numbers[1], out double n2);

            return Math.Round(n1 / n2, 12).ToString();
        }

        private string Mod()
        {
            string[] numbers = Screen.Text.Split("%");

            double.TryParse(numbers[0], out double n1);
            double.TryParse(numbers[1], out double n2);

            return (n1 % n2).ToString();
        }

        private string Pow()
        {
            string[] numbers = Screen.Text.Split("^");

            double.TryParse(numbers[0], out double n1);
            double.TryParse(numbers[1], out double n2);

            return Math.Pow(n1, n2).ToString();
        }

        private string Sqrt()
        {
            string number = Screen.Text.Split(" ")[1];

            double.TryParse(number, out double n);

            return Math.Sqrt(n).ToString();
        }

        private string Factorial()
        {
            string number = Screen.Text.Split("!")[0];

            double.TryParse(number, out double n);

            // Iterativo usando while
            //double result = 1;

            //while (number != 1)
            //{
            //    result *= number;
            //    number--;
            //}

            //return result;

            // Iterativo usando for
            double result = 1;

            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }

            return result.ToString();

            // Recursivo (ignoramos los números negativos)
            //if (number < 1)
            //{
            //    return 1;
            //}
            //else if (number == 1)
            //{
            //    return 1;
            //}
            //else 
            //{
            //    return number * Factorial(number - 1);
            //}
        }

        private string Log()
        {
            string number = Screen.Text.Split(" ")[1];

            double.TryParse(number, out double n);

            return Math.Log10(n).ToString();
        }

        private string Sen()
        {
            string number = Screen.Text.Split(" ")[1];

            double.TryParse(number, out double n);

            return Math.Sin(n).ToString();
        }

        private string Cos()
        {
            string number = Screen.Text.Split(" ")[1];

            double.TryParse(number, out double n);

            return Math.Cos(n).ToString();
        }

        private string Tan()
        {
            string number = Screen.Text.Split(" ")[1];

            double.TryParse(number, out double n);

            return Math.Tan(n).ToString();
        }

        private string ConvertToBinary(int number)
        {
            // Si el número es con coma, se descarta la parte decimal
            return Convert.ToString(number, 2);

            //// Manual
            //int resto;
            //string resultado = String.Empty;

            //while (number > 0)
            //{
            //    resto = number % 2;
            //    number /= 2;
            //    resultado = resto.ToString() + resultado; // Agregamos los números en orden inverso.
            //}

            //return resultado;
        }

        private string ConvertToHex(int number)
        {
            return number.ToString("X");
        }

    }
}
