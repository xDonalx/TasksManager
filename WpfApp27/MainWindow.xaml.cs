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

namespace WpfApp27
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddEditTaskWindow window = new AddEditTaskWindow();
            window.ShowDialog();
        }

        private void ShowTasksList_Click(object sender, RoutedEventArgs e)
        {
            TasksListWindow window = new TasksListWindow();
            window.ShowDialog();
        }

        private void ShowEmployeesList_Click(object sender, RoutedEventArgs e)
        {
            EmployeesListWindow window = new EmployeesListWindow();
            window.ShowDialog();
        }

        private void ShowCompletedTasks_Click(object sender, RoutedEventArgs e)
        {
            CompletedTasksWindow window = new CompletedTasksWindow();
            window.ShowDialog();
        }
    }
}