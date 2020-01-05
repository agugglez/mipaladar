using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace MiPaladar.Stuff
{
    public class DataGridContextHelper
    {
        static DataGridContextHelper()
        {

            DependencyProperty dp = FrameworkElement.DataContextProperty.AddOwner(typeof(DataGridColumn));
            FrameworkElement.DataContextProperty.OverrideMetadata(typeof(DataGrid),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnDataContextChanged)));

        }


        public static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGrid grid = d as DataGrid;
            if (grid != null)
            {
                foreach (DataGridColumn col in grid.Columns)
                {
                    col.SetValue(FrameworkElement.DataContextProperty, e.NewValue);
                }
            }
        }

    } 

}
