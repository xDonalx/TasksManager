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
    public partial class CompletedTasksWindow : Window
    {
        private string connectionString = "Data Source=AORUS_PC;Initial Catalog=TaskScheduler;Integrated Security=SSPI";

        public CompletedTasksWindow()
        {
            InitializeComponent();
            LoadCompletedTasks();
        }

        private void LoadCompletedTasks()
        {
            List<Task> completedTasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TaskId, Title, Status, DueDate, Priority FROM Tasks WHERE Status = 'Выполнено'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    completedTasks.Add(new Task
                    {
                        TaskId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Status = reader.GetString(2),
                        DueDate = reader.GetDateTime(3),
                        Priority = reader.GetInt32(4)
                    });
                }
            }

            CompletedTasksDataGrid.ItemsSource = completedTasks;
        }
    }
}
