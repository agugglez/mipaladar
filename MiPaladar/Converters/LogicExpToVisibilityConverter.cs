using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Globalization;

namespace MiPaladar.Converters
{
    public class LogicExpToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "!v1a!v2")
                Console.WriteLine();

            bool[] boolValues = new bool[values.Length];            

            //copy original values, set to false when UnsetValue
            for (int i = 0; i < values.Length; i++)
            {
                if (!(values[i] is bool)) { boolValues[i] = false; continue; }

                boolValues[i] = (bool)values[i];
            }

            string expression = (string)parameter;

            for (int i = 0; i < boolValues.Length; i++)
            {
                bool valuei = (bool)boolValues[i];
                expression = expression.Replace("v" + (i + 1), valuei ? "1" : "0");
            }

            return EvaluatePostFix(ToPostfix(expression)) ? 
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        string ToPostfix(string a)
        {
            int N = a.Length;
            Stack<char> s = new Stack<char>(N);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                if (a[i] == ')')
                    sb.Append(s.Pop() + " ");
                if ((a[i] == '!') || (a[i] == 'o') || (a[i] == 'a'))
                    s.Push(a[i]);
                if ((a[i] == '0') || (a[i] == '1'))
                    sb.Append(a[i] + " ");
            }

            while (s.Count > 0) sb.Append(s.Pop() + " ");

            return sb.ToString();
        }

        bool EvaluatePostFix(string a)
        {
            int N = a.Length;
            Stack<bool> s = new Stack<bool>(N);

            for (int i = 0; i < N; i++)
            {
                if (a[i] == '!')
                    s.Push(!s.Pop());
                if (a[i] == 'o')
                    s.Push(s.Pop() || s.Pop());
                if (a[i] == 'a')
                    s.Push(s.Pop() && s.Pop());
                if ((a[i] == '0'))
                    s.Push(false);
                if ((a[i] == '1'))
                    s.Push(true);
            }
            return s.Pop();
        }
    }

}
