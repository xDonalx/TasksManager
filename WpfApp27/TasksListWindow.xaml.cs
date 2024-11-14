using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace WpfApp27
{
    public partial class TasksListWindow : Window
    {
        private string connectionString = "Data Source=AORUS_PC;Initial Catalog=TaskScheduler;Integrated Security=SSPI";

        public TasksListWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void LoadTasks()
        {
            List<Task> tasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TaskId, Title, Status, DueDate, Priority FROM Tasks";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new Task
                    {
                        TaskId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Status = reader.GetString(2),
                        DueDate = reader.GetDateTime(3),
                        Priority = reader.GetInt32(4)
                    });
                }
            }

            TasksDataGrid.ItemsSource = tasks;
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is Task selectedTask)
            {
                AddEditTaskWindow window = new AddEditTaskWindow(selectedTask);
                window.ShowDialog();
                LoadTasks();
            }
            else
            {
                MessageBox.Show("Выберите задачу для редактирования.");
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is Task selectedTask)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TaskId", selectedTask.TaskId);
                    command.ExecuteNonQuery();
                }
                LoadTasks();
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления.");
            }
        }
    }
}
