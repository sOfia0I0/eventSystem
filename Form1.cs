using System;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Guna.UI2.WinForms;
namespace eventSystem
{
    public partial class Form1 : Form
    {
        private string connectionString = "server=localhost;database=eventsystem;uid=root;pwd=cDta5hdh56yupo";
        private MySqlConnection connection;
        private MySqlCommand command;
        private Form2 form2;

        public Form1()
        {
            InitializeComponent();
            guna2TextBox6.PasswordChar = '*';
            guna2TextBox1.PasswordChar = '*';
            connection = new MySqlConnection(connectionString);
            guna2CheckBox1.CheckedChanged += guna2CheckBox1_CheckedChanged;
            guna2CheckBox2.CheckedChanged += guna2CheckBox2_CheckedChanged;
            guna2ComboBox1.Items.AddRange(new string[] { "Организатор", "Спикер", "Администратор" });
            guna2ComboBox2.Items.AddRange(new string[] { "Организатор", "Спикер" });
            form2 = new Form2();
        }
        
        private byte[] GenerateSalt(int length)
        {
            byte[] salt = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPassword = new byte[salt.Length + Encoding.UTF8.GetBytes(password).Length];
                Array.Copy(salt, saltedPassword, salt.Length);
                Array.Copy(Encoding.UTF8.GetBytes(password), 0, saltedPassword, salt.Length, Encoding.UTF8.GetBytes(password).Length);
                return sha256.ComputeHash(saltedPassword);
            }

        }
        private int GetTabIndexForRole(string role)
        {
            switch (role)
            {
                case "Организатор":
                    return 0;
                case "Спикер":
                    return 1;
                case "Администратор":
                    return 2;
                default:
                    return -1;
            }
        }
        private void guna2TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox4.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

        }
        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox3.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

        }
        private void guna2TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox5.Text.Length >= 12 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsDigit(e.KeyChar) && e.KeyChar != '+' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '+' && guna2TextBox5.Text.Length > 0)
            {
                e.Handled = true;
            }

        }
        private void guna2TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox6.Text.Length >= 8 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

        }
        private void guna2TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox7.Text.Length >= 32 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != '@')
            {
                e.Handled = true;
            }

        }
        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            guna2TextBox6.PasswordChar = guna2CheckBox1.Checked ? '\0' : '*';
        }
        private void guna2Button2_Click(object sender, EventArgs e) // регистрация
        {
            if (string.IsNullOrEmpty(guna2TextBox4.Text) || string.IsNullOrEmpty(guna2TextBox3.Text) || string.IsNullOrEmpty(guna2TextBox5.Text) ||
            string.IsNullOrEmpty(guna2TextBox7.Text) || string.IsNullOrEmpty(guna2TextBox6.Text) ||
            string.IsNullOrEmpty(guna2ComboBox2.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (guna2TextBox5.Text.Length < 11)
            {
                MessageBox.Show("Номер телефона должен содержать 11 цифр!");
                return;
            }
            if (guna2TextBox7.Text.Length < 3)
            {
                MessageBox.Show("Логин должен содержать от 3 до 32 символов!");
                return;
            }
            if (guna2TextBox6.Text.Length < 8)
            {
                MessageBox.Show("Пароль должен содержать не менее 8 символов!");
                return;
            }   

            try
            {
                connection.Open();
                string checkLoginQuery = "SELECT COUNT(*) FROM users WHERE login = @login";
                using (MySqlCommand checkLoginCommand = new MySqlCommand(checkLoginQuery, connection))
                {
                    checkLoginCommand.Parameters.AddWithValue("@login", guna2TextBox7.Text);
                    int loginCount = Convert.ToInt32(checkLoginCommand.ExecuteScalar());
                    if (loginCount > 0)
                    {
                        MessageBox.Show("Этот логин уже используется!");
                        return;
                    }

                }

                byte[] salt = GenerateSalt(16);
                byte[] passwordHash = HashPassword(guna2TextBox6.Text, salt);
                string insertQuery = "INSERT INTO users (lname, fname, phone, login, phash, psalt, urole) VALUES (@lname, @fname, @phone, @login, @phash, @psalt, @urole)";
                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@lname", guna2TextBox4.Text);
                    insertCommand.Parameters.AddWithValue("@fname", guna2TextBox3.Text);
                    insertCommand.Parameters.AddWithValue("@phone", guna2TextBox5.Text);
                    insertCommand.Parameters.AddWithValue("@login", guna2TextBox7.Text);
                    insertCommand.Parameters.AddWithValue("@phash", Convert.ToBase64String(passwordHash));
                    insertCommand.Parameters.AddWithValue("@psalt", Convert.ToBase64String(salt));
                    insertCommand.Parameters.AddWithValue("@urole", guna2ComboBox2.SelectedItem.ToString());
                    insertCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Регистрация успешна!");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации: " + ex.Message);
            }

            finally
            {
                connection.Close();
                guna2TextBox4.Text = string.Empty;
                guna2TextBox3.Text = string.Empty;
                guna2TextBox5.Text = string.Empty;
                guna2TextBox7.Text = string.Empty;
                guna2TextBox6.Text = string.Empty;
                guna2ComboBox2.SelectedIndex = -1;
            }

        }
        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox2.Text.Length >= 32 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '_' && e.KeyChar != '@')
            {
                e.Handled = true;
            }
        }
        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox1.Text.Length >= 8 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            guna2TextBox1.PasswordChar = guna2CheckBox2.Checked ? '\0' : '*';
        }
        private void guna2Button1_Click(object sender, EventArgs e) // вход
        {      
            if (string.IsNullOrEmpty(guna2TextBox1.Text) || string.IsNullOrEmpty(guna2TextBox2.Text) ||
         string.IsNullOrEmpty(guna2ComboBox1.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            if (guna2TextBox2.Text.Length < 3)
            {
                MessageBox.Show("Логин должен содержать от 3 до 32 символов!");
                return;
            }
            if (guna2TextBox1.Text.Length < 8)
            {
                MessageBox.Show("Пароль должен содержать не менее 8 символов!");
                return;
            }

            try
            {
                connection.Open();
                string query = "SELECT id, phash, psalt FROM users WHERE login = @login AND urole = @urole";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", guna2TextBox2.Text);
                    command.Parameters.AddWithValue("@urole", guna2ComboBox1.Text);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32("id");
                            string dbHash = reader.GetString("phash");
                            string dbSalt = reader.GetString("psalt");
                            byte[] salt = Convert.FromBase64String(dbSalt);
                            byte[] hash = HashPassword(guna2TextBox1.Text, salt);
                            string clientHash = Convert.ToBase64String(hash);

                            if (clientHash == dbHash)
                            {
                                form2.currentRole = guna2ComboBox1.SelectedItem.ToString();
                                form2.currentUserId = userId.ToString();
                                form2.guan2TabControl1.SelectedIndex = GetTabIndexForRole(guna2ComboBox1.SelectedItem.ToString());
                                form2.ShowDialog();
                                this.Show();
                            }
                            else
                            {
                                MessageBox.Show("Неверный пароль.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин, пароль или роль.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
            finally
            {
                connection.Close();
                guna2TextBox1.Text = string.Empty;
                guna2TextBox2.Text = string.Empty;
                guna2ComboBox1.SelectedIndex = -1;
            }
        }   
 
        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
