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
    public partial class EmployeesListWindow : Window
    {
        private string connectionString = "Data Source=AORUS_PC;Initial Catalog=TaskScheduler;Integrated Security=SSPI";
        //храним сотрудника 
        private User selectedUser;

        public EmployeesListWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            List<User> employees = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT UserId, Username, Email FROM Users";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new User
                    {
                        UserId = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(2)
                    });
                }
            }

            EmployeesDataGrid.ItemsSource = employees;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, Email, PasswordHash) VALUES (@Username, @Email, @PasswordHash)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", "hashed_password");

                command.ExecuteNonQuery();
            }

            LoadEmployees();
            MessageBox.Show("Ваш сотрудник добавлен!");
            UsernameTextBox.Clear();
            EmailTextBox.Clear();
            UpdatePlaceholderVisibility();
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                //окно редакт. сотрудника
                UsernameTextBox.Text = selectedUser.Username;
                EmailTextBox.Text = selectedUser.Email;
                UsernamePlaceholder.Visibility = Visibility.Collapsed;
                EmailPlaceholder.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
            }
        }

        private void SaveEditedEmployee()
        {
            if (selectedUser != null)
            {
                string username = UsernameTextBox.Text;
                string email = EmailTextBox.Text;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Заполните все поля.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Users SET Username = @Username, Email = @Email WHERE UserId = @UserId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", selectedUser.UserId);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);

                    command.ExecuteNonQuery();
                }

                LoadEmployees();
                MessageBox.Show("Ваш сотрудник обновлен!");
                UsernameTextBox.Clear();
                EmailTextBox.Clear();
                UpdatePlaceholderVisibility();
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is User user)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Users WHERE UserId = @UserId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.ExecuteNonQuery();
                }
                LoadEmployees();
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для удаления.");
            }
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is User user)
            {
                // выбор сотрудника
                selectedUser = user;
            }
        }
        private void UpdatePlaceholderVisibility()
        {
            UsernamePlaceholder.Visibility = string.IsNullOrWhiteSpace(UsernameTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            EmailPlaceholder.Visibility = string.IsNullOrWhiteSpace(EmailTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UsernameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }
        private void EmailTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }
    }
}