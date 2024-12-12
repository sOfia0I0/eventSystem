using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace eventSystem
{
    public partial class Form2 : Form
    {
        private string connectionString = "server=localhost;database=eventsystem;uid=root;pwd=cDta5hdh56yupo";
        private MySqlConnection connection;
        public string currentRole;
        public string currentUserId;
        public event EventHandler ClearDataGridView;
        private MySqlCommand command;
        public Form2()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
            guan2TabControl1.Selecting += guan2TabControl1_Selecting;
            guna2ComboBox3.Items.AddRange(new string[] { "Название", "Тип мероприятия", "Место проведения" });
            guna2ComboBox7.Items.AddRange(new string[] { "Название", "Рейтинг", "Описание" });
            LoadRatings();
            LoadLocations();
            LoadEventTypes();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            switch (currentRole)
            {
                case "Организатор":
                    guan2TabControl1.SelectedIndex = 0;
                    LoadOrganizatorData();
                    break;
                case "Спикер":
                    guan2TabControl1.SelectedIndex = 1;
                    LoadSpikerData();
                    break;
                case "Администратор":
                    guan2TabControl1.SelectedIndex = 2;
                    break;
            }
        }
        private void guan2TabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage.Name != currentRole)
            {
                e.Cancel = true;
                MessageBox.Show("У вас нет доступа к этой вкладке.");
            }
        }
        private void LoadRatings() // загрузка рейтингов
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT ratingname FROM ratings";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    guna2ComboBox4.Items.Add(reader["ratingname"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки рейтингов: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private int GetRatingId(string ratingName) // получение ID рейтинга
        {
            int ratingId = -1;
            if (string.IsNullOrEmpty(ratingName))
            {
                return ratingId;
            }
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT id FROM ratings WHERE ratingname = @ratingName";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ratingName", ratingName);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    ratingId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка получения ID рейтинга: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return ratingId;
        }






        private void LoadLocations() // загрузка локаций
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT locationame FROM eventlocations";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    guna2ComboBox1.Items.Add(reader["locationame"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки локаций: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private int GetLocationId(string locationName) // получение ID локации
        {
            int locationId = -1;
            if (string.IsNullOrEmpty(locationName))
            {
                return locationId;
            }
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT id FROM eventlocations WHERE locationame = @locationName";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@locationName", locationName);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    locationId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка получения ID локации: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return locationId;
        }
        private void LoadEventTypes() // загрузка типов событий
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT typename FROM eventtypes";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    guna2ComboBox2.Items.Add(reader["typename"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки типов событий: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private int GetEventTypeId(string eventTypeName) // получение ID типа события
        {
            int eventTypeId = -1;
            if (string.IsNullOrEmpty(eventTypeName))
            {
                return eventTypeId;
            }
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT id FROM eventtypes WHERE typename = @eventTypeName";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@eventTypeName", eventTypeName);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    eventTypeId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка получения ID типа события: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return eventTypeId;
        }

        // организатор
        private void LoadOrganizatorData()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT id, lname AS 'Фамилия', fname AS 'Имя', phone AS 'Телефон', login AS 'Логин' " + "FROM users WHERE id = @currentUserId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@currentUserId", currentUserId);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                guna2DataGridView1.DataSource = dataTable;
                guna2DataGridView1.Columns["id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void UpdateOrganizatorData(int selectedId)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string lname = guna2DataGridView1.CurrentRow.Cells["Фамилия"].Value.ToString();
                string fname = guna2DataGridView1.CurrentRow.Cells["Имя"].Value.ToString();
                string phone = guna2DataGridView1.CurrentRow.Cells["Телефон"].Value.ToString();
                string login = guna2DataGridView1.CurrentRow.Cells["Логин"].Value.ToString();
                string query = "UPDATE users SET lname = @lname, fname = @fname, phone = @phone, login = @login WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lname", lname);
                    command.Parameters.AddWithValue("@fname", fname);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@id", selectedId);
                    command.ExecuteNonQuery();
                    LoadOrganizatorData();
                    MessageBox.Show("Данные успешно изменены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись для обновления.");
                return;
            }
            int selectedId = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["id"].Value);
            UpdateOrganizatorData(selectedId);
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text.Length >= 2000 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void guna2TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox4.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox4.Text) ||
    string.IsNullOrEmpty(textBox1.Text) ||
    string.IsNullOrEmpty(guna2ComboBox1.Text) ||
    string.IsNullOrEmpty(guna2ComboBox2.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            int locationId = GetLocationId(guna2ComboBox1.SelectedItem.ToString());
            if (locationId == -1)
            {
                MessageBox.Show("Ошибка: место проведения не найдено.");
                return;
            }

            int eventTypeId = GetEventTypeId(guna2ComboBox2.SelectedItem.ToString());
            if (eventTypeId == -1)
            {
                MessageBox.Show("Ошибка: тип не найден.");
                return;
            }

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string checkQuery = "SELECT COUNT(*) FROM eventss WHERE eventname = @eventname AND eventdate = @eventdate";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@eventname", guna2TextBox4.Text);
                checkCommand.Parameters.AddWithValue("@eventdate", guna2DateTimePicker1.Value.Date);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Мероприятие с таким же названием и датой уже существует.");
                    return;
                }

                string query = "INSERT INTO eventss (eventname, location_id, eventtype_id, eventdate, descriptionn, user_id) " +
                               "VALUES (@eventname, @location_id, @eventtype_id, @eventdate, @descriptionn, @user_id)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@eventname", guna2TextBox4.Text);
                command.Parameters.AddWithValue("@location_id", locationId);
                command.Parameters.AddWithValue("@eventtype_id", eventTypeId);
                command.Parameters.AddWithValue("@eventdate", guna2DateTimePicker1.Value);
                command.Parameters.AddWithValue("@descriptionn", textBox1.Text);
                command.Parameters.AddWithValue("@user_id", currentUserId);
                command.ExecuteNonQuery();

                MessageBox.Show("Мероприятие успешно добавлено!");

                guna2TextBox4.Text = string.Empty;
                textBox1.Text = string.Empty;
                guna2ComboBox1.SelectedIndex = -1;
                guna2ComboBox2.SelectedIndex = -1;
                guna2DateTimePicker1.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления мероприятия: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox2.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox8.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string searchColumn = guna2ComboBox3.SelectedItem?.ToString();
                string query = "";

                if (searchColumn == "Название")
                {
                    query = @"SELECT e.id, e.eventname AS 'Название мероприятия', el.locationame AS 'Место проведения мероприятия', et.typename AS 'Тип мероприятия', e.eventdate AS 'Дата мероприятия', e.descriptionn AS 'Описание мероприятия'
                      FROM eventss e
                      JOIN eventtypes et ON e.eventtype_id = et.id
                      JOIN eventlocations el ON e.location_id = el.id
                      WHERE e.eventname LIKE @search AND e.user_id = @user_id";
                }
                else if (searchColumn == "Тип мероприятия")
                {
                    query = @"SELECT e.id, e.eventname AS 'Название мероприятия', el.locationame AS 'Место проведения мероприятия', et.typename AS 'Тип мероприятия', e.eventdate AS 'Дата мероприятия', e.descriptionn AS 'Описание мероприятия'              
                FROM eventss e
              JOIN eventtypes et ON e.eventtype_id = et.id
              JOIN eventlocations el ON e.location_id = el.id
              WHERE et.typename LIKE @search AND e.user_id = @user_id";
                }
                else if (searchColumn == "Место проведения")
                {
                    query = @"SELECT e.id, e.eventname AS 'Название мероприятия', el.locationame AS 'Место проведения мероприятия', et.typename AS 'Тип мероприятия', e.eventdate AS 'Дата мероприятия', e.descriptionn AS 'Описание мероприятия'              
                FROM eventss e
              JOIN eventtypes et ON e.eventtype_id = et.id
              JOIN eventlocations el ON e.location_id = el.id
              WHERE el.locationame LIKE @search AND e.user_id = @user_id";
                }
                else
                {
                    MessageBox.Show("Выберите столбец для поиска.");
                    return;
                }

                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@search", "%" + guna2TextBox2.Text + "%");
                command.Parameters.AddWithValue("@user_id", currentUserId);

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView4.DataSource = null;
                    guna2DataGridView4.Rows.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        guna2DataGridView4.DataSource = dt;
                        guna2DataGridView4.Columns["id"].Visible = false;
                        guna2DataGridView4.Columns["Дата мероприятия"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    }
                    else
                    {
                        MessageBox.Show("Совпадений не найдено.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string searchColumn = guna2ComboBox7.SelectedItem?.ToString();
                string query = "";

                if (searchColumn == "Название")
                {
                    query = @"SELECT p.id, p.topic AS 'Тема выступления', p.descriptionn AS 'Описание выступления', p.duration AS 'Длительность выступления', r.ratingname AS 'Рейтинг выступления' 
                      FROM performances p
                      JOIN ratings r ON p.rating_id = r.id
                      WHERE p.topic LIKE @search";
                }
                else if (searchColumn == "Описание")
                {
                    query = @"SELECT p.id, p.topic AS 'Тема выступления', p.descriptionn AS 'Описание выступления', p.duration AS 'Длительность выступления', r.ratingname AS 'Рейтинг выступления' 
                      FROM performances p
                      JOIN ratings r ON p.rating_id = r.id
                      WHERE p.descriptionn LIKE @search";
                }
                else if (searchColumn == "Рейтинг")
                {
                    query = @"SELECT p.id, p.topic AS 'Тема выступления', p.descriptionn AS 'Описание выступления', p.duration AS 'Длительность выступления', r.ratingname AS 'Рейтинг выступления' 
                      FROM performances p
                      JOIN ratings r ON p.rating_id = r.id
                      WHERE r.ratingname LIKE @search";
                }
                else
                {
                    MessageBox.Show("Выберите допустимый столбец для поиска.");
                    return;
                }

                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@search", "%" + guna2TextBox8.Text + "%");
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView7.DataSource = null;
                    guna2DataGridView7.Rows.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        guna2DataGridView7.DataSource = dt;
                        guna2DataGridView7.Columns["id"].Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Совпадений не найдено.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void guna2DataGridView4_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string eventName = guna2DataGridView4.Rows[e.RowIndex].Cells["Название мероприятия"].Value.ToString();
                label12.Text = eventName;
            }
        }

        private void guna2DataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string performanceName = guna2DataGridView7.Rows[e.RowIndex].Cells["Тема выступления"].Value.ToString();
                label24.Text = performanceName;
            }
        }
        private void guna2Button16_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView4.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите мероприятие.");
                return;
            }

            int eventId = Convert.ToInt32(guna2DataGridView4.CurrentRow.Cells["id"].Value);
            if (guna2DataGridView7.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите выступление.");
                return;
            }

            int performanceId = Convert.ToInt32(guna2DataGridView7.CurrentRow.Cells["id"].Value);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // Проверка на существование записи
                string checkQuery = "SELECT COUNT(*) FROM speakerevents WHERE event_id = @eventId AND performance_id = @performanceId";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@eventId", eventId);
                checkCommand.Parameters.AddWithValue("@performanceId", performanceId);

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Это выступление уже добавлено в данное мероприятие.");
                    return;
                }

                // Вставка новой записи
                string query = "INSERT INTO speakerevents (event_id, performance_id) VALUES (@eventId, @performanceId)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@eventId", eventId);
                command.Parameters.AddWithValue("@performanceId", performanceId);
                command.ExecuteNonQuery();

                MessageBox.Show("В мероприятие успешно добавлено выступление!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления выступления: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void guna2Button25_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, открыто ли соединение
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // SQL запрос
                string query = @"
        SELECT 
    se.id,
    e.eventname AS 'Название меропрятия',
    l.locationame AS 'Место проведения меропрятия',
    t.typename AS 'Тип мероприятия',
    e.eventdate AS 'Дата меропрятия',
    e.descriptionn AS 'Описание меропрятия',
    p.topic AS 'Тема выступления',
    p.descriptionn AS 'Описание выступления',
    p.duration AS 'Длительность выступления',
    r.ratingname AS 'Рейтинг выступления',
        u.lname AS 'Фамилия спикера',
        u.fname AS 'Имя спикера',
        u.phone AS 'Телефон спикера'
FROM 
    speakerevents se
JOIN 
    eventss e ON se.event_id = e.id
JOIN 
    performances p ON se.performance_id = p.id
JOIN 
    eventlocations l ON e.location_id = l.id
JOIN 
    eventtypes t ON e.eventtype_id = t.id
JOIN 
    ratings r ON p.rating_id = r.id
JOIN 
        users u ON p.user_id = u.id  
WHERE 
    e.user_id = @user_id;
"; // Убедитесь, что здесь используется правильное имя параметра

                // Создаем команду
                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", currentUserId); // Используем @userId

                // Заполняем DataTable и связываем с DataGridView
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView3.DataSource = dt;
                    guna2DataGridView3.Columns["id"].Visible = false;
                    guna2DataGridView3.Columns["Дата меропрятия"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                // Закрываем соединение
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView3.SelectedRows[0].Cells["id"].Value);


            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "DELETE FROM speakerevents WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Запись успешно удалена!");
                guna2Button25_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        // спикер

        private void LoadSpikerData() // загрузка данных спикера
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT id, lname AS 'Фамилия', fname AS 'Имя', phone AS 'Телефон', login AS 'Логин' " + "FROM users WHERE id = @currentUserId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@currentUserId", currentUserId);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                guna2DataGridView2.DataSource = dataTable;
                guna2DataGridView2.Columns["id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void UpdateSpikerData(int selectedId) // обновлление данных заказчика
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string lname = guna2DataGridView2.CurrentRow.Cells["Фамилия"].Value.ToString();
                string fname = guna2DataGridView2.CurrentRow.Cells["Имя"].Value.ToString();
                string phone = guna2DataGridView2.CurrentRow.Cells["Телефон"].Value.ToString();
                string login = guna2DataGridView2.CurrentRow.Cells["Логин"].Value.ToString();
                string query = "UPDATE users SET lname = @lname, fname = @fname, phone = @phone, login = @login WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lname", lname);
                    command.Parameters.AddWithValue("@fname", fname);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@id", selectedId);
                    command.ExecuteNonQuery();
                    LoadSpikerData();
                    MessageBox.Show("Данные успешно обновлены.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись для изменения.");
                return;
            }
            int selectedId = Convert.ToInt32(guna2DataGridView2.CurrentRow.Cells["id"].Value);
            UpdateSpikerData(selectedId);
        }
        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox1.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox2.Text.Length >= 2000 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox1.Text) ||
    string.IsNullOrEmpty(textBox2.Text) ||
    string.IsNullOrEmpty(guna2ComboBox4.Text) ||
    guna2NumericUpDown1.Value <= 0)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            int ratingId = GetRatingId(guna2ComboBox4.SelectedItem.ToString());
            if (ratingId == -1)
            {
                MessageBox.Show("Ошибка: рейтинг не найден.");
                return;
            }

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // Проверка на существование выступления
                string checkQuery = "SELECT COUNT(*) FROM performances WHERE topic = @topic AND duration = @duration";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@topic", guna2TextBox1.Text);
                checkCommand.Parameters.AddWithValue("@duration", (int)guna2NumericUpDown1.Value);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Выступление с таким же названием и продолжительностью уже существует.");
                    return;
                }

                // Вставка нового выступления
                string query = "INSERT INTO performances (topic, descriptionn, duration, rating_id, user_id) " +
                               "VALUES (@topic, @description, @duration, @rating_id, @user_id)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@topic", guna2TextBox1.Text);
                command.Parameters.AddWithValue("@description", textBox2.Text);
                command.Parameters.AddWithValue("@duration", (int)guna2NumericUpDown1.Value);
                command.Parameters.AddWithValue("@rating_id", ratingId);
                command.Parameters.AddWithValue("@user_id", currentUserId);
                command.ExecuteNonQuery();

                MessageBox.Show("Выступление успешно добавлено!");

                // Очистка полей
                guna2TextBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                guna2ComboBox4.SelectedIndex = -1;
                guna2NumericUpDown1.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления выступления: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }
        private void guna2Button24_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = @"
            SELECT p.id,
                p.topic AS 'Тема выступления', 
                p.descriptionn AS 'Описание выступления', 
                p.duration AS 'Продолжительность выступления', 
                r.ratingname AS 'Рейтинг выступления'  
            FROM 
                performances p
            JOIN 
                ratings r ON p.rating_id = r.id
            WHERE 
                p.user_id = @user_id";
                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", currentUserId);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView5.DataSource = dt;
                    guna2DataGridView5.Columns["id"].Visible = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void guna2Button23_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView5.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для изменения.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView5.SelectedRows[0].Cells["id"].Value);
            string ratingname = guna2DataGridView5.CurrentRow.Cells["Рейтинг выступления"].Value.ToString();

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // 1. Получаем rating_id из таблицы ratings
                string queryRatingId = "SELECT id FROM ratings WHERE ratingname = @ratingname";
                using (MySqlCommand commandRatingId = new MySqlCommand(queryRatingId, connection))
                {
                    commandRatingId.Parameters.AddWithValue("@ratingname", ratingname);
                    int rating_id = Convert.ToInt32(commandRatingId.ExecuteScalar()); // Получаем скалярное значение (id)

                    // Проверка на существование рейтинга
                    if (rating_id == 0)
                    {
                        MessageBox.Show("Рейтинг '" + ratingname + "' не существует в базе данных.");
                        return;
                    }


                    // 2. Обновляем performances
                    string topic = guna2DataGridView5.CurrentRow.Cells["Тема выступления"].Value.ToString();
                    string descriptionn = guna2DataGridView5.CurrentRow.Cells["Описание выступления"].Value.ToString();
                    string duration = guna2DataGridView5.CurrentRow.Cells["Вместимость выступления"].Value.ToString();
                    string queryPerformances = "UPDATE performances SET topic = @topic, descriptionn = @descriptionn, duration = @duration, rating_id = @rating_id WHERE id = @id";
                    using (MySqlCommand commandPerformances = new MySqlCommand(queryPerformances, connection))
                    {
                        commandPerformances.Parameters.AddWithValue("@topic", topic);
                        commandPerformances.Parameters.AddWithValue("@descriptionn", descriptionn);
                        commandPerformances.Parameters.AddWithValue("@duration", duration);
                        commandPerformances.Parameters.AddWithValue("@rating_id", rating_id);
                        commandPerformances.Parameters.AddWithValue("@id", id);
                        commandPerformances.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные успешно обновлены!");
                    guna2Button24_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void guna2Button14_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView5.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView5.SelectedRows[0].Cells["id"].Value);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

               

                // Delete the performance record
                string query = "DELETE FROM performances WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Запись успешно удалена!");
                guna2Button24_Click(sender, e);
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("foreign key constraint fails"))
                {
                    MessageBox.Show("Невозможно удалить выступление, так как организатор уже добавил его в свое мероприятие.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Обработка других ошибок
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        // администратор


        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox3.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void guna2Button11_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox3.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }



            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "INSERT INTO eventtypes (typename) " +
                               "VALUES (@typename)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@typename", guna2TextBox3.Text);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Тип мероприятия добавлен!");
                guna2Button18_Click_1(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении типа мероприятия: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void guna2Button18_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT * FROM eventtypes";
                command = new MySqlCommand(query, connection);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView8.DataSource = dt;
                    guna2DataGridView8.Columns[0].HeaderText = "ID";
                    guna2DataGridView8.Columns[1].HeaderText = "Название типа мероприятия";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            
        }
        private void guna2Button17_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView8.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для обновления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView8.SelectedRows[0].Cells["id"].Value);
            string typename = guna2DataGridView8.SelectedRows[0].Cells["typename"].Value.ToString();

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string query = "UPDATE eventtypes SET typename = @typename WHERE id = @id";
                using (MySqlCommand commandPerformances = new MySqlCommand(query, connection))
                {
                    commandPerformances.Parameters.AddWithValue("@typename", typename);
                    commandPerformances.Parameters.AddWithValue("@id", id);
                    commandPerformances.ExecuteNonQuery();
                }

                MessageBox.Show("Данные успешно обновлены!");
                guna2Button18_Click_1(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void guna2Button19_Click_1(object sender, EventArgs e)
        {
            if (guna2DataGridView8.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView8.SelectedRows[0].Cells["id"].Value);


            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "DELETE FROM eventtypes WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Запись удалена!");
                guna2Button18_Click_1(sender, e);
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("foreign key constraint fails"))
                {
                    MessageBox.Show("Невозможно удалить тип, так как организаторы уже используют его при создании мероприятий.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Обработка других ошибок
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        private void guna2TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox5.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox7.Text.Length >= 12 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsDigit(e.KeyChar) && e.KeyChar != '+' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '+' && guna2TextBox7.Text.Length > 0)
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox6.Text.Length >= 35 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void guna2Button12_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox5.Text) || string.IsNullOrEmpty(guna2TextBox7.Text) ||
       string.IsNullOrEmpty(guna2TextBox6.Text) || guna2NumericUpDown2.Value == 0)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (guna2TextBox7.Text.Length != 11)
            {
                MessageBox.Show("Номер телефона должен содержать 11 цифр!");
                return;
            }

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "INSERT INTO eventlocations (locationame, capacity, address, phone) " +
                               "VALUES (@locationame, @capacity, @address, @phone)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@locationame", guna2TextBox5.Text);
                    command.Parameters.AddWithValue("@capacity", guna2NumericUpDown2.Value);
                    command.Parameters.AddWithValue("@address", guna2TextBox6.Text);
                    command.Parameters.AddWithValue("@phone", guna2TextBox7.Text);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Место проведения добавлено!");
                guna2Button22_Click_1(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении места проведения: " + ex.Message);
            }
            finally
            {
                connection.Close();
                guna2TextBox5.Text = string.Empty;
                guna2NumericUpDown2.Text = string.Empty;
                guna2TextBox6.Text = string.Empty;
                guna2TextBox7.Text = string.Empty;
            }
        }
        private void guna2Button22_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "SELECT * FROM eventlocations";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        guna2DataGridView9.DataSource = dt;
                        guna2DataGridView9.Columns[0].HeaderText = "ID";
                        guna2DataGridView9.Columns[1].HeaderText = "Название места проведения";
                        guna2DataGridView9.Columns[2].HeaderText = "Вместимость места проведения";
                        guna2DataGridView9.Columns[3].HeaderText = "Адрес места проведения";
                        guna2DataGridView9.Columns[4].HeaderText = "Номер телефона места проведения";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void guna2Button21_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView9.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для обновления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView9.SelectedRows[0].Cells["id"].Value);
            string locationame = guna2DataGridView9.SelectedRows[0].Cells["locationame"].Value.ToString();
            string capacity = guna2DataGridView9.SelectedRows[0].Cells["capacity"].Value.ToString();
            string address = guna2DataGridView9.SelectedRows[0].Cells["address"].Value.ToString();
            string phone = guna2DataGridView9.SelectedRows[0].Cells["phone"].Value.ToString();
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "UPDATE eventlocations SET locationame = @locationame, capacity = @capacity, address = @address, phone = @phone WHERE id = @id";
                using (MySqlCommand commandPerformances = new MySqlCommand(query, connection))
                {
                    commandPerformances.Parameters.AddWithValue("@locationame", locationame);
                    commandPerformances.Parameters.AddWithValue("@capacity", capacity);
                    commandPerformances.Parameters.AddWithValue("@address", address);
                    commandPerformances.Parameters.AddWithValue("@phone", phone);
                    commandPerformances.Parameters.AddWithValue("@id", id);
                    commandPerformances.ExecuteNonQuery();
                }

                MessageBox.Show("Данные успешно обновлены!");
                guna2Button22_Click_1(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void guna2Button20_Click_1(object sender, EventArgs e)
        {
            if (guna2DataGridView9.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView9.SelectedRows[0].Cells["id"].Value);


            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "DELETE FROM eventlocations WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Запись удалена!");
                guna2Button22_Click_1(sender, e);
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("foreign key constraint fails"))
                {
                    MessageBox.Show("Невозможно удалить место проведения, так как организаторы уже используют его при создании мероприятий.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Обработка других ошибок
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                connection.Close();
            }
        }


        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            guna2TextBox4.Text = string.Empty;
            guna2ComboBox1.SelectedIndex = -1;
            guna2ComboBox2.SelectedIndex = -1;
            guna2DateTimePicker1.Value = DateTime.Now;
            textBox1.Text = string.Empty;
            (guna2DataGridView12.DataSource as DataTable)?.Clear();        
            label12.Text = string.Empty;
            label24.Text = string.Empty;
            guna2ComboBox3.SelectedIndex = -1;
            guna2ComboBox7.SelectedIndex = -1;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox8.Text = string.Empty;
            (guna2DataGridView4.DataSource as DataTable)?.Clear();
            (guna2DataGridView7.DataSource as DataTable)?.Clear();
            (guna2DataGridView3.DataSource as DataTable)?.Clear();
            guna2TextBox1.Text = string.Empty;
            guna2ComboBox4.SelectedIndex = -1;
            guna2NumericUpDown1.Value = 0;
            textBox2.Text = string.Empty;
            (guna2DataGridView5.DataSource as DataTable)?.Clear();
            (guna2DataGridView14.DataSource as DataTable)?.Clear();
            (guna2DataGridView6.DataSource as DataTable)?.Clear();
            (guna2DataGridView11.DataSource as DataTable)?.Clear();
            (guna2DataGridView13.DataSource as DataTable)?.Clear();
            (guna2DataGridView10.DataSource as DataTable)?.Clear();
            guna2TextBox5.Text = string.Empty;
            guna2TextBox7.Text = string.Empty;
            guna2TextBox6.Text = string.Empty;
            guna2NumericUpDown2.Value = 0;
            (guna2DataGridView9.DataSource as DataTable)?.Clear();
            guna2TextBox3.Text = string.Empty;
            (guna2DataGridView8.DataSource as DataTable)?.Clear();                    
            Close();
        }

        private void guna2Button27_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, открыто ли соединение
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // SQL запрос
                string query = @"
        SELECT 
    se.id AS 'ID', 
us.lname AS 'Фамилия организатора',
        us.fname AS 'Имя организатора',
        us.phone AS 'Телефон организатора',
    e.eventname AS 'Название мероприятия',
    l.locationame AS 'Место проведения мероприятия',
    t.typename AS 'Тип мероприятия',
    e.eventdate AS 'Дата мероприятия',
    e.descriptionn AS 'Описание мероприятия',
    p.topic AS 'Тема выступления',
    p.descriptionn AS 'Описание выступления',
    p.duration AS 'Длительность выступления',
    r.ratingname AS 'Рейтинг выступления',
u.lname AS 'Фамилия спикера',
        u.fname AS 'Имя спикера',
        u.phone AS 'Телефон спикера'
FROM 
    speakerevents se
JOIN 
    eventss e ON se.event_id = e.id
JOIN 
    performances p ON se.performance_id = p.id
JOIN 
    eventlocations l ON e.location_id = l.id
JOIN 
    eventtypes t ON e.eventtype_id = t.id
JOIN 
    ratings r ON p.rating_id = r.id
JOIN 
        users us ON e.user_id = us.id
JOIN 
        users u ON p.user_id = u.id;";

                // Создаем команду
                command = new MySqlCommand(query, connection);

                // Заполняем DataTable и связываем с DataGridView
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView10.DataSource = dt;
                    guna2DataGridView10.Columns["Дата мероприятия"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                // Закрываем соединение
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

       
        

        private void guna2Button29_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = @"
            SELECT p.id AS 'ID',
                p.topic AS 'Тема выступления', 
                p.descriptionn AS 'Описание выступления', 
                p.duration AS 'Продолжительность выступления', 
                r.ratingname AS 'Рейтинг выступления',
                u.lname AS 'Фамилия спикера',
                u.fname AS 'Имя спикера',
                u.phone AS 'Телефон спикера'
            FROM 
                performances p
            JOIN 
                ratings r ON p.rating_id = r.id
JOIN 
        users u ON p.user_id = u.id;";
                command = new MySqlCommand(query, connection);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView11.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void guna2Button28_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView11.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для обновления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView11.SelectedRows[0].Cells["id"].Value);
            string ratingname = guna2DataGridView11.CurrentRow.Cells["Рейтинг"].Value.ToString();

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string queryRatingId = "SELECT id FROM ratings WHERE ratingname = @ratingname";
                using (MySqlCommand commandRatingId = new MySqlCommand(queryRatingId, connection))
                {
                    commandRatingId.Parameters.AddWithValue("@ratingname", ratingname);
                    int rating_id = Convert.ToInt32(commandRatingId.ExecuteScalar());

                    if (rating_id == 0)
                    {
                        MessageBox.Show("Рейтинг '" + ratingname + "' не найден в базе данных.");
                        return;
                    }

                    string topic = guna2DataGridView11.CurrentRow.Cells["Тема"].Value.ToString();
                    string descriptionn = guna2DataGridView11.CurrentRow.Cells["Описание"].Value.ToString();
                    string duration = guna2DataGridView11.CurrentRow.Cells["Вместимость"].Value.ToString();
                    string queryPerformances = "UPDATE performances SET topic = @topic, descriptionn = @descriptionn, duration = @duration, rating_id = @rating_id WHERE id = @id";
                    using (MySqlCommand commandPerformances = new MySqlCommand(queryPerformances, connection))
                    {
                        commandPerformances.Parameters.AddWithValue("@topic", topic);
                        commandPerformances.Parameters.AddWithValue("@descriptionn", descriptionn);
                        commandPerformances.Parameters.AddWithValue("@duration", duration);
                        commandPerformances.Parameters.AddWithValue("@rating_id", rating_id);
                        commandPerformances.Parameters.AddWithValue("@id", id);
                        commandPerformances.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные успешно обновлены!");
                    guna2Button29_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView11.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView11.SelectedRows[0].Cells["id"].Value);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }


                string query = "DELETE FROM performances WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Запись удалена!");
                guna2Button29_Click(sender, e);
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("foreign key constraint fails"))
                {
                    MessageBox.Show("Невозможно удалить выступление. Сначала удалите связанные события в разделе 'Управление событиями'.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button26_Click_1(object sender, EventArgs e)
        {
            
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            int editingColumnIndex = guna2DataGridView10.CurrentCell.ColumnIndex;
            if (editingColumnIndex == 8 || editingColumnIndex == 9 || editingColumnIndex == 10 || editingColumnIndex == 11)
            {
                MessageBox.Show("Изменить выступление можно только в разделе \"Управление выступлениями\".");
                return;
            }
            if (guna2DataGridView10.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для обновления.");
                return;
            }

            int id1 = Convert.ToInt32(guna2DataGridView10.SelectedRows[0].Cells["id1"].Value);
            string eventname = guna2DataGridView10.SelectedRows[0].Cells["Название"].Value.ToString();
            string locationame = guna2DataGridView10.SelectedRows[0].Cells["Место проведения"].Value.ToString();
            string typename = guna2DataGridView10.SelectedRows[0].Cells["Тип мероприятия"].Value.ToString();
            string descriptionn = guna2DataGridView10.SelectedRows[0].Cells["Описание"].Value.ToString();
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string queryLocationId = "SELECT id FROM eventlocations WHERE locationame = @locationame";
                string queryTypeId = "SELECT id FROM eventtypes WHERE typename = @typename";

                using (MySqlCommand commandLocationId = new MySqlCommand(queryLocationId, connection))
                using (MySqlCommand commandTypeId = new MySqlCommand(queryTypeId, connection))
                {
                    commandLocationId.Parameters.AddWithValue("@locationame", locationame);
                    int location_id = Convert.ToInt32(commandLocationId.ExecuteScalar());

                    commandTypeId.Parameters.AddWithValue("@typename", typename);
                    int eventtype_id = Convert.ToInt32(commandTypeId.ExecuteScalar());

                    if (location_id == 0)
                    {
                        MessageBox.Show("Место проведения '" + locationame + "' не найдено в базе данных.");
                        return;
                    }
                    if (eventtype_id == 0)
                    {
                        MessageBox.Show("Тип мероприятия '" + typename + "' не найден в базе данных.");
                        return;
                    }



                    string queryEventss = "UPDATE eventss SET eventname = @eventname, location_id = @location_id, eventtype_id = @eventtype_id, descriptionn = @descriptionn WHERE id = @id";
                    using (MySqlCommand commandEventss = new MySqlCommand(queryEventss, connection))
                    {
                        commandEventss.Parameters.AddWithValue("@eventname", eventname);
                        commandEventss.Parameters.AddWithValue("@location_id", location_id);
                        commandEventss.Parameters.AddWithValue("@eventtype_id", eventtype_id);
                        commandEventss.Parameters.AddWithValue("@descriptionn", descriptionn);
                        commandEventss.Parameters.AddWithValue("@id", id1);
                        commandEventss.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные успешно обновлены!");
                    guna2Button27_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button35_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string query = @"
        SELECT e.id,
            e.eventname AS 'Название мероприятия',
            l.locationame AS 'Место проведения мероприятия',
            t.typename AS 'Тип мероприятия',
            e.eventdate AS 'Дата мероприятия',
            e.descriptionn AS 'Описание мероприятия'
        FROM 
            eventss e
        JOIN 
            eventlocations l ON e.location_id = l.id
        JOIN 
            eventtypes t ON e.eventtype_id = t.id
        
        WHERE 
            e.user_id = @user_id;";

                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", currentUserId); 

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView12.DataSource = dt;
                    guna2DataGridView12.Columns["id"].Visible = false;
                    guna2DataGridView12.Columns["Дата мероприятия"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void guna2Button34_Click(object sender, EventArgs e)
        {
            
            if (guna2DataGridView12.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для изменения.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView12.SelectedRows[0].Cells["id"].Value);
            string eventname = guna2DataGridView12.SelectedRows[0].Cells["Название мероприятия"].Value.ToString();
            string locationame = guna2DataGridView12.SelectedRows[0].Cells["Место проведения мероприятия"].Value.ToString();
            string typename = guna2DataGridView12.SelectedRows[0].Cells["Тип мероприятия"].Value.ToString();
            string descriptionn = guna2DataGridView12.SelectedRows[0].Cells["Описание мероприятия"].Value.ToString();
            DateTime eventdate = Convert.ToDateTime(guna2DataGridView12.CurrentRow.Cells["Дата мероприятия"].Value);
            
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string queryLocationId = "SELECT id FROM eventlocations WHERE locationame = @locationame";
                string queryTypeId = "SELECT id FROM eventtypes WHERE typename = @typename";

                using (MySqlCommand commandLocationId = new MySqlCommand(queryLocationId, connection))
                using (MySqlCommand commandTypeId = new MySqlCommand(queryTypeId, connection))
                {
                    commandLocationId.Parameters.AddWithValue("@locationame", locationame);
                    int location_id = Convert.ToInt32(commandLocationId.ExecuteScalar());

                    commandTypeId.Parameters.AddWithValue("@typename", typename);
                    int eventtype_id = Convert.ToInt32(commandTypeId.ExecuteScalar());

                    if (location_id == 0)
                    {
                        MessageBox.Show("Место проведения '" + locationame + " не существует в базе данных.");
                        return;
                    }
                    if (eventtype_id == 0)
                    {
                        MessageBox.Show("Тип мероприятия '" + typename + "' не существует в базе данных.");
                        return;
                    }
                    


                    string queryEventss = "UPDATE eventss SET eventname = @eventname, location_id = @location_id, eventtype_id = @eventtype_id, descriptionn = @descriptionn, eventdate = @eventdate WHERE id = @id";
                    using (MySqlCommand commandEventss = new MySqlCommand(queryEventss, connection))
                    {
                        commandEventss.Parameters.AddWithValue("@eventname", eventname);
                        commandEventss.Parameters.AddWithValue("@location_id", location_id);
                        commandEventss.Parameters.AddWithValue("@eventtype_id", eventtype_id);
                        commandEventss.Parameters.AddWithValue("@descriptionn", descriptionn);
                        commandEventss.Parameters.AddWithValue("@eventdate", eventdate);
                        commandEventss.Parameters.AddWithValue("@id", id);
                        commandEventss.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные успешно изменены!");
                    guna2Button35_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void guna2Button33_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView12.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView12.SelectedRows[0].Cells["id"].Value);


            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "DELETE FROM eventss WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Запись успешно удалена!");
                guna2Button35_Click(sender, e);
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("foreign key constraint fails"))
                {
                    MessageBox.Show("Невозможно удалить мероприятие. Сначала удалите связанные события в разделе 'О ваших событиях'.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView10.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView10.SelectedRows[0].Cells["id"].Value);


            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "DELETE FROM speakerevents WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Запись удалена!");
                guna2Button27_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string query = @"
        SELECT e.id AS 'ID',
            
            e.eventname AS 'Название мероприятия',
            l.locationame AS 'Место проведения мероприятия',
            t.typename AS 'Тип мероприятия',
            e.eventdate AS 'Дата мероприятия',
            e.descriptionn AS 'Описание мероприятия',
            u.lname AS 'Фамилия организатора',
            u.fname AS 'Имя организатора',
            u.phone AS 'Телефон организатора'
        FROM 
            eventss e
        JOIN 
            eventlocations l ON e.location_id = l.id
        JOIN 
            eventtypes t ON e.eventtype_id = t.id
        JOIN 
            users u ON e.user_id = u.id;";

                command = new MySqlCommand(query, connection);

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView13.DataSource = dt;
                    guna2DataGridView13.Columns["Дата мероприятия"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView13.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для обновления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView13.SelectedRows[0].Cells["ID"].Value);
            string eventname = guna2DataGridView13.SelectedRows[0].Cells["Название"].Value.ToString();
            string locationame = guna2DataGridView13.SelectedRows[0].Cells["Место проведения"].Value.ToString();
            string typename = guna2DataGridView13.SelectedRows[0].Cells["Тип мероприятия"].Value.ToString();
            string descriptionn = guna2DataGridView13.SelectedRows[0].Cells["Описание"].Value.ToString();

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string queryLocationId = "SELECT id FROM eventlocations WHERE locationame = @locationame";
                string queryTypeId = "SELECT id FROM eventtypes WHERE typename = @typename";

                using (MySqlCommand commandLocationId = new MySqlCommand(queryLocationId, connection))
                using (MySqlCommand commandTypeId = new MySqlCommand(queryTypeId, connection))
                {
                    commandLocationId.Parameters.AddWithValue("@locationame", locationame);
                    int location_id = Convert.ToInt32(commandLocationId.ExecuteScalar());

                    commandTypeId.Parameters.AddWithValue("@typename", typename);
                    int eventtype_id = Convert.ToInt32(commandTypeId.ExecuteScalar());

                    if (location_id == 0)
                    {
                        MessageBox.Show("Место проведения '" + locationame + "' не найдено в базе данных.");
                        return;
                    }
                    if (eventtype_id == 0)
                    {
                        MessageBox.Show("Тип мероприятия '" + typename + "' не найден в базе данных.");
                        return;
                    }



                    string queryEventss = "UPDATE eventss SET eventname = @eventname, location_id = @location_id, eventtype_id = @eventtype_id, descriptionn = @descriptionn WHERE id = @id";
                    using (MySqlCommand commandEventss = new MySqlCommand(queryEventss, connection))
                    {
                        commandEventss.Parameters.AddWithValue("@eventname", eventname);
                        commandEventss.Parameters.AddWithValue("@location_id", location_id);
                        commandEventss.Parameters.AddWithValue("@eventtype_id", eventtype_id);
                        commandEventss.Parameters.AddWithValue("@descriptionn", descriptionn);
                        commandEventss.Parameters.AddWithValue("@ID", id);
                        commandEventss.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные успешно обновлены!");
                    guna2Button9_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            if (guna2DataGridView13.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView13.SelectedRows[0].Cells["id"].Value);


            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "DELETE FROM eventss WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Запись удалена!");
                guna2Button9_Click(sender, e);
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("foreign key constraint fails"))
                {
                    MessageBox.Show("Невозможно удалить мероприятие. Сначала удалите связанные события в разделе 'Управление событиями'.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button32_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = @"
            SELECT u.id AS 'ID',
                u.lname AS 'Фамилия', 
                u.fname AS 'Имя', 
                u.phone AS 'Телефон', 
                u.login AS 'Логин',
                u.urole AS 'Роль'
            FROM 
                users u";
                command = new MySqlCommand(query, connection);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView6.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void guna2Button31_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView6.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для обновления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView6.SelectedRows[0].Cells["id"].Value);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }




                string lname = guna2DataGridView6.CurrentRow.Cells["Фамилия"].Value.ToString();
                    string fname = guna2DataGridView6.CurrentRow.Cells["Имя"].Value.ToString();
                    string phone = guna2DataGridView6.CurrentRow.Cells["Телефон"].Value.ToString();
                string login = guna2DataGridView6.CurrentRow.Cells["Логин"].Value.ToString();
                string urole = guna2DataGridView6.CurrentRow.Cells["Роль"].Value.ToString();

                string queryPerformances = "UPDATE users SET lname = @lname, fname = @fname, phone = @phone, login = @login, urole = @urole WHERE id = @id";
                    using (MySqlCommand commandPerformances = new MySqlCommand(queryPerformances, connection))
                    {
                        commandPerformances.Parameters.AddWithValue("@lname", lname);
                        commandPerformances.Parameters.AddWithValue("@fname", fname);
                        commandPerformances.Parameters.AddWithValue("@phone", phone);
                    commandPerformances.Parameters.AddWithValue("@login", login);

                    commandPerformances.Parameters.AddWithValue("@urole", urole);

                    commandPerformances.Parameters.AddWithValue("@id", id);
                        commandPerformances.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные успешно обновлены!");
                guna2Button32_Click(sender, e);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button30_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView6.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int id = Convert.ToInt32(guna2DataGridView6.SelectedRows[0].Cells["id"].Value);


            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = "DELETE FROM users WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Запись удалена!");
                guna2Button32_Click(sender, e);
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("foreign key constraint fails"))
                {
                    MessageBox.Show("Невозможно удалить пользователя, так как его действия в системе уже записаны в базу данных.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2DataGridView12_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is FormatException)
            {
                MessageBox.Show("Неверный формат даты. Пожалуйста, введите дату в формате ДД.MM.ГГГГ.", "Ошибка формата", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.ThrowException = false;
            }
        }

        private void guna2Button37_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string query = @"
    SELECT 
        se.id,
        u.lname AS 'Фамилия организатора',
        u.fname AS 'Имя организатора',
        u.phone AS 'Телефон организатора',
        e.eventname AS 'Название мероприятия',
        l.locationame AS 'Место проведения мероприятия',
        t.typename AS 'Тип мероприятия',
        e.eventdate AS 'Дата мероприятия',
        e.descriptionn AS 'Описание мероприятия',
        p.topic AS 'Тема выступления',
        p.descriptionn AS 'Описание выступления',
        p.duration AS 'Длительность выступления',
        r.ratingname AS 'Рейтинг выступления'
        
    FROM 
        speakerevents se
    JOIN 
        eventss e ON se.event_id = e.id
    JOIN 
        performances p ON se.performance_id = p.id
    JOIN 
        eventlocations l ON e.location_id = l.id
    JOIN 
        eventtypes t ON e.eventtype_id = t.id
    JOIN 
        ratings r ON p.rating_id = r.id
    JOIN 
        users u ON e.user_id = u.id  
    WHERE 
        p.user_id = @user_id;
";

                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", currentUserId);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    guna2DataGridView14.DataSource = dt;
                    guna2DataGridView14.Columns["id"].Visible = false;
                    guna2DataGridView14.Columns["Дата мероприятия"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

       
    }
}
