using CommunityToolkit.Mvvm.Messaging;
using Sudoku.Messages;
using Sudoku.ViewModels;
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

namespace Sudoku.Components
{
    /// <summary>
    /// Interaction logic for SingleGridCell.xaml
    /// </summary>
    public partial class SingleGridCell : UserControl
    {
        public SingleGridCell()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (GridCell.IsReadOnly)
            {
                e.Handled = true;
                return;
            }
            bool isValidNumber = int.TryParse(e.Text, out int number) && number >= 1 && number <= 9;
            if (isValidNumber && DataContext is SingleGridCellViewModel vm)
                vm.AddCellValue(number);

            e.Handled = true;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && DataContext is SingleGridCellViewModel vm)
            {
                vm.DeleteCellValue();
            }
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox && DataContext is SingleGridCellViewModel vm)
            {
                vm.SendCellSelectedMessage();
            }
        }
    }
}
