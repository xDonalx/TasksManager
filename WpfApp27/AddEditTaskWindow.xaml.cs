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
    public partial class AddEditTaskWindow : Window
    {
        private string connectionString = "Data Source=AORUS_PC;Initial Catalog=TaskScheduler;Integrated Security=SSPI";
        private bool isEditMode;
        private Task taskToEdit;

        public AddEditTaskWindow()
        {
            InitializeComponent();
            isEditMode = false;
        }

        public AddEditTaskWindow(Task task) : this()
        {
            taskToEdit = task;
            isEditMode = true;
            //заполнение полей
            TaskTitleTextBox.Text = task.Title;
            TaskDescriptionTextBox.Text = task.Description;
            TaskDueDatePicker.SelectedDate = task.DueDate;
            TaskPriorityComboBox.SelectedIndex = task.Priority - 1;
            TaskCompletedCheckBox.IsChecked = task.Status == "Выполнено!";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new Task
            {
                TaskId = isEditMode ? taskToEdit.TaskId : 0,
                Title = TaskTitleTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                DueDate = TaskDueDatePicker.SelectedDate ?? DateTime.Now,
                Priority = TaskPriorityComboBox.SelectedIndex + 1,
                Status = TaskCompletedCheckBox.IsChecked == true ? "Выполнено!" : "Запланировано!"
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = isEditMode
                    ? "UPDATE Tasks SET Title = @Title, Description = @Description, DueDate = @DueDate, Priority = @Priority, Status = @Status WHERE TaskId = @TaskId"
                    : "INSERT INTO Tasks (UserId, Title, Description, DueDate, Priority, Status) VALUES (@UserId, @Title, @Description, @DueDate, @Priority, @Status)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (isEditMode)
                        command.Parameters.AddWithValue("@TaskId", newTask.TaskId);
                    else
                        command.Parameters.AddWithValue("@UserId", 1);

                    command.Parameters.AddWithValue("@Title", newTask.Title);
                    command.Parameters.AddWithValue("@Description", newTask.Description);
                    command.Parameters.AddWithValue("@DueDate", newTask.DueDate);
                    command.Parameters.AddWithValue("@Priority", newTask.Priority);
                    command.Parameters.AddWithValue("@Status", newTask.Status);
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show(isEditMode ? "Задача была обновлена!" : "Задача была добавлена!");
            Close();
        }

        private void TaskCompletedCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}