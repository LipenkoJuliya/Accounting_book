using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Книга_учета;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SQLite;

namespace Книга_учета
{
    public partial class Form1 : Form
    {
        private AccountingData accountingData;
        private BindingSource categoriesBindingSource = new BindingSource();
        private BindingSource transactionsBindingSource = new BindingSource();
        private DatabaseHelper dbHelper;
        private string dbFilePath = "accounting.db";
        private string jsonFilePath = "initial_data.json"; // Путь к файлу JSON по умолчанию

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Инициализация DatabaseHelper и создание/открытие БД
            dbHelper = new DatabaseHelper(dbFilePath);
            dbHelper.CreateDatabaseIfNotExists();

            Console.WriteLine($"dbHelper initialized: {dbHelper != null}"); // Добавили вывод

            // 2. Инициализация AccountingData
            accountingData = new AccountingData();
            accountingData.Categories = dbHelper.GetAllCategories();
            accountingData.Transactions = dbHelper.GetAllTransactions();

            Console.WriteLine($"accountingData initialized: {accountingData != null}");// Добавили вывод

            // 3. Привязка данных к DataGridView
            categoriesBindingSource.DataSource = accountingData.Categories;
            dgvCategories.DataSource = categoriesBindingSource;
            dgvCategories.AutoGenerateColumns = false;
            SetupCategoriesDataGridViewColumns(); // Настройка колонок

            transactionsBindingSource.DataSource = accountingData.Transactions;
            dgvTransactions.DataSource = transactionsBindingSource;
            dgvTransactions.AutoGenerateColumns = false;
            SetupTransactionsDataGridViewColumns(); // Настройка колонок

            // 4. Настройка DataGridView (обработка ошибок, редактирование и т.д.)
            dgvTransactions.DataError += dgvTransactions_DataError;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Выделяем строку целиком
            dgvTransactions.MultiSelect = false; // Запрещаем множественный выбор
            dgvTransactions.SelectionChanged += dgvTransactions_SelectionChanged; // Подписываемся на SelectionChanged

            // 5. Настройка фильтров для OpenFileDialog и SaveFileDialog
            openFileDialog1.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog1.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

            // 6. Загрузка данных при запуске (если нужно)
            LoadDataFromFileOnStartup();

            // 8. Обновление интерфейса (баланс, итого по категориям, график)
            UpdateBalance();
            UpdateCategoryTotals();
            UpdateChart();
            UpdateCategoryComboBox(); // Обновляем ComboBox категорий
        }

        private void SetupCategoriesDataGridViewColumns()
        {
            dgvCategories.Columns.Clear();

            // Column Id
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "ID";
            idColumn.Name = "Id";
            dgvCategories.Columns.Add(idColumn);
            idColumn.Visible = false; // Скрываем столбец Id

            // Column Name
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Название";
            nameColumn.Name = "Name";
            dgvCategories.Columns.Add(nameColumn);

            // Column Description
            DataGridViewTextBoxColumn descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.HeaderText = "Описание";
            descriptionColumn.Name = "Description";
            dgvCategories.Columns.Add(descriptionColumn);

            // Запрещаем редактирование ячеек напрямую
            foreach (DataGridViewColumn column in dgvCategories.Columns)
            {
                column.ReadOnly = true;
            }
            dgvCategories.AllowUserToAddRows = false; //Отключаем добавление строк
            dgvCategories.AllowUserToDeleteRows = false; //Отключаем удаление строк
        }

        private void SetupTransactionsDataGridViewColumns()
        {
            dgvTransactions.Columns.Clear();

            // Column Id
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "ID";
            idColumn.Name = "Id";
            dgvTransactions.Columns.Add(idColumn);
            idColumn.Visible = false; // Скрываем столбец Id

            // Column Date
            DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
            dateColumn.DataPropertyName = "Date";
            dateColumn.HeaderText = "Дата";
            dateColumn.Name = "Date";
            dateColumn.DefaultCellStyle.Format = "yyyy-MM-dd"; // Format date
            dgvTransactions.Columns.Add(dateColumn);

            // Column Description
            DataGridViewTextBoxColumn descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.HeaderText = "Описание";
            descriptionColumn.Name = "Description";
            dgvTransactions.Columns.Add(descriptionColumn);

            // Column Amount
            DataGridViewTextBoxColumn amountColumn = new DataGridViewTextBoxColumn();
            amountColumn.DataPropertyName = "Amount";
            amountColumn.HeaderText = "Сумма";
            amountColumn.Name = "Amount";
            amountColumn.DefaultCellStyle.Format = "C"; // Format currency
            dgvTransactions.Columns.Add(amountColumn);

            // Column Category
            DataGridViewTextBoxColumn categoryColumn = new DataGridViewTextBoxColumn();
            categoryColumn.DataPropertyName = "Category";
            categoryColumn.HeaderText = "Категория";
            categoryColumn.Name = "Category";
            dgvTransactions.Columns.Add(categoryColumn);

            // Column Type
            DataGridViewTextBoxColumn typeColumn = new DataGridViewTextBoxColumn();
            typeColumn.DataPropertyName = "Type";
            typeColumn.HeaderText = "Тип";
            typeColumn.Name = "Type";
            dgvTransactions.Columns.Add(typeColumn);

            // Запрещаем редактирование ячеек напрямую
            foreach (DataGridViewColumn column in dgvTransactions.Columns)
            {
                column.ReadOnly = true;
            }
            dgvTransactions.AllowUserToAddRows = false; //Отключаем добавление строк
            dgvTransactions.AllowUserToDeleteRows = false; //Отключаем удаление строк

        }
        private void LoadDataFromFileOnStartup()
        {
            // Проверяем, существует ли база данных. Если нет, пытаемся загрузить данные из JSON.
            if (!File.Exists(dbFilePath))
            {
                if (File.Exists(jsonFilePath))
                {
                    LoadDataFromFile(jsonFilePath, true); // Загружаем и перезаписываем
                }
            }
        }

        private void dgvTransactions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string columnName = "";
            if (dgvTransactions.Columns.Count > 0 && e.ColumnIndex >= 0 && e.ColumnIndex < dgvTransactions.Columns.Count)
            {
                columnName = dgvTransactions.Columns[e.ColumnIndex].Name;  // Получаем имя колонки
            }
            if (e.Exception is FormatException)
            {
                MessageBox.Show($"Некорректный формат данных в колонке '{columnName}'. Пожалуйста, проверьте введенные данные.", "Ошибка формата", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.ThrowException = false;
                e.Cancel = true;
            }
            else
            {
                MessageBox.Show($"Произошла ошибка в колонке '{columnName}': {e.Exception.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.ThrowException = false;
            }
        }

        private void LoadDataFromFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                LoadDataFromFile(filePath, false);
            }
        }

        private void LoadDataFromFile(string filePath, bool isStartupLoad = false)
        {
            try
            {
                // 1. Чтение файла
                string jsonData = null;
                try
                {
                    jsonData = File.ReadAllText(filePath);
                    Console.WriteLine($"JSON data read successfully:\n{jsonData}"); // Выводим содержимое JSON в консоль
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Десериализация JSON
                AccountingData loadedData = null;
                try
                {
                    loadedData = JsonConvert.DeserializeObject<AccountingData>(jsonData);
                    if (loadedData == null)
                    {
                        Console.WriteLine("DeserializeObject вернул NULL!");  //добавили вывд
                    }
                    else
                    {
                        Console.WriteLine($"Data after deserialization: Categories count = {loadedData.Categories?.Count}, Transactions count = {loadedData.Transactions?.Count}"); // Добавили вывод
                    }
                }
                catch (JsonReaderException jex)
                {
                    MessageBox.Show($"Ошибка при десериализации JSON (JsonReaderException): {jex.Message}\n{jex.Path}\n{jex.LineNumber}\n{jex.LinePosition}", "Ошибка десериализации", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                catch (Exception ex) // Обрабатываем другие возможные исключения при десериализации
                {
                    MessageBox.Show($"Ошибка при десериализации JSON (Exception): {ex.Message}\n{ex.StackTrace}", "Ошибка десериализации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (loadedData != null && loadedData.Categories != null && loadedData.Transactions != null)
                {
                    DialogResult result = DialogResult.Yes; // По умолчанию перезапись для initial load

                    if (!isStartupLoad)
                    {
                        // Предлагаем пользователю выбор: перезаписать текущие данные или добавить к текущим
                        result = MessageBox.Show("Выберите действие:\nДа - Перезаписать текущие данные\nНет - Добавить к текущим данным", "Загрузка данных", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    }

                    if (result == DialogResult.Yes)
                    {
                        // Перезаписать: очищаем текущие данные и загружаем новые
                        try
                        {
                            // Очищаем таблицы в базе данных
                            dbHelper.ClearAllData();

                            // Очищаем списки в памяти
                            accountingData.Categories.Clear();
                            accountingData.Transactions.Clear();

                            // Загружаем категории и транзакции из файла
                            foreach (var category in loadedData.Categories)
                            {
                                int newCategoryId = dbHelper.AddCategory(category); // Добавляем категорию в БД и получаем ID
                                category.Id = newCategoryId; // Устанавливаем ID
                                accountingData.AddCategory(category); // Добавляем в список
                            }
                            foreach (var transaction in loadedData.Transactions)
                            {
                                transaction.CategoryId = transaction.Category.Id; // Устанавливаем CategoryId
                                dbHelper.AddTransaction(transaction); // Добавляем транзакцию в БД
                                accountingData.AddTransaction(transaction); // Добавляем в список
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при перезаписи данных: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        // Добавить: добавляем данные из файла к текущим
                        try
                        {
                            // Добавляем категории, только если их еще нет в списке и в базе данных
                            foreach (var category in loadedData.Categories)
                            {
                                if (!accountingData.Categories.Any(c => c.Name == category.Name))
                                {
                                    // Проверяем, есть ли такая категория в БД
                                    Category existingCategory = dbHelper.GetAllCategories().FirstOrDefault(c => c.Name == category.Name);
                                    if (existingCategory == null)
                                    {
                                        int newCategoryId = dbHelper.AddCategory(category); // Добавляем категорию в БД и получаем ID
                                        category.Id = newCategoryId; // Устанавливаем ID
                                        accountingData.AddCategory(category); // Добавляем в список
                                    }
                                    else
                                    {
                                        category.Id = existingCategory.Id;
                                        accountingData.AddCategory(category);
                                    }

                                }
                            }

                            // Добавляем транзакции, только если их еще нет в списке и в базе данных
                            foreach (var transaction in loadedData.Transactions)
                            {
                                // Проверяем, есть ли такая транзакция в БД
                                Transaction existingTransaction = dbHelper.GetAllTransactions().FirstOrDefault(t =>
                                        t.Date == transaction.Date &&
                                        t.Description == transaction.Description &&
                                        t.Amount == transaction.Amount &&
                                        t.Category.Name == transaction.Category.Name &&
                                        t.Type == transaction.Type);
                                if (existingTransaction == null)
                                {
                                    transaction.CategoryId = transaction.Category.Id;
                                    dbHelper.AddTransaction(transaction);
                                    accountingData.AddTransaction(transaction);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Обновляем BindingSource и UI
                    categoriesBindingSource.ResetBindings(false);
                    transactionsBindingSource.ResetBindings(false);
                    UpdateCategoryComboBox();
                    UpdateBalance();
                    UpdateCategoryTotals();
                    UpdateChart();
                }
                else
                {
                    MessageBox.Show("Ошибка: Не удалось загрузить данные из файла. Возможно, файл поврежден или имеет неверный формат.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string message = $"Ошибка загрузки данных: {ex.Message}";
                if (ex.InnerException != null)
                {
                    message += $"\nInner Exception: {ex.InnerException.Message}";
                }
                MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveDataToFile()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;

                try
                {
                    // Создаем объект для сериализации, содержащий категории и транзакции
                    var dataToSerialize = new
                    {
                        Categories = accountingData.Categories,
                        Transactions = accountingData.Transactions
                    };

                    // Сериализуем данные в JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSerialize, Newtonsoft.Json.Formatting.Indented);

                    // Записываем JSON в файл
                    File.WriteAllText(filePath, jsonData);

                    MessageBox.Show("Данные успешно сохранены в файл.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных в файл: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Методы для категорий
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtNameCategory.Text.Trim();
            string description = txtDescriptionCategory.Text.Trim();

            if (!string.IsNullOrEmpty(name))
            {
                // Добавляем категорию в базу данных и в список в памяти
                Category newCategory = new Category(name, description);
                int newCategoryId = dbHelper.AddCategory(newCategory); // Получаем ID
                newCategory.Id = newCategoryId;  //  Устанавливаем ID
                accountingData.AddCategory(newCategory);
                categoriesBindingSource.ResetBindings(false);
                UpdateCategoryComboBox();
                txtNameCategory.Clear();
                txtDescriptionCategory.Clear();
                UpdateCategoryTotals();
                UpdateChart();
            }
            else
            {
                MessageBox.Show("Введите название категории.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count > 0)
            {
                Category selectedCategory = (Category)dgvCategories.SelectedRows[0].DataBoundItem;
                if (selectedCategory != null)
                {
                    string oldName = selectedCategory.Name;
                    string newName = txtNameCategory.Text.Trim();
                    string newDescription = txtDescriptionCategory.Text.Trim();

                    if (!string.IsNullOrEmpty(newName))
                    {
                        Category newCategory = new Category(newName, newDescription);
                        newCategory.Id = selectedCategory.Id; // Сохраняем старый ID

                        // Обновляем категорию в базе данных
                        dbHelper.UpdateCategory(newCategory);

                        // Обновляем категорию в списке в памяти
                        accountingData.UpdateCategory(oldName, newCategory);

                        categoriesBindingSource.ResetBindings(false);
                        transactionsBindingSource.ResetBindings(false);
                        UpdateCategoryComboBox();
                        UpdateCategoryTotals();
                        UpdateChart();
                        txtNameCategory.Clear();
                        txtDescriptionCategory.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Введите новое название категории.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для редактирования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count > 0)
            {
                Category selectedCategory = (Category)dgvCategories.SelectedRows[0].DataBoundItem;
                if (selectedCategory != null)
                {
                    DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить категорию '{selectedCategory.Name}'?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {

                        // Удаляем категорию из базы данных
                        dbHelper.DeleteCategory(selectedCategory.Id);

                        // Удаляем категорию из списка в памяти
                        accountingData.DeleteCategory(selectedCategory.Name);

                        categoriesBindingSource.ResetBindings(false);
                        transactionsBindingSource.ResetBindings(false);
                        UpdateCategoryComboBox();
                        UpdateCategoryTotals();
                        UpdateChart();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateCategoryComboBox()
        {
            cmbTransactionCategory.DataSource = null;
            cmbTransactionCategory.DataSource = accountingData.Categories;
            cmbTransactionCategory.DisplayMember = "Name";
            cmbTransactionCategory.ValueMember = "Id";  // ValueMember теперь Id
        }

        // Методы для транзакций
        private void btnAddTransaction_Click(object sender, EventArgs e)
        {
            if (cmbTransactionCategory.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите категорию.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Category selectedCategory = (Category)cmbTransactionCategory.SelectedItem;

            // Проверяем, существует ли выбранная категория в списке категорий
            if (!accountingData.Categories.Any(c => c.Id == selectedCategory.Id))
            {
                MessageBox.Show("Выбранная категория не существует. Пожалуйста, выберите другую категорию.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime date = dtpTransactionDate.Value.Date; // Сохраняем только дату
            string description = txtTransactionDescription.Text.Trim();
            decimal amount = nudTransactionAmount.Value;

            TransactionType type = rdbTransactionExpense.Checked ? TransactionType.Expense : TransactionType.Income; // Получаем тип из RadioButton

            if (!string.IsNullOrEmpty(description))
            {
                Transaction transaction = new Transaction(date, description, amount, selectedCategory, type);
                transaction.CategoryId = selectedCategory.Id; // Устанавливаем CategoryId

                // Добавляем транзакцию в базу данных
                dbHelper.AddTransaction(transaction);

                // Добавляем транзакцию в список в памяти
                accountingData.AddTransaction(transaction);

                transactionsBindingSource.ResetBindings(false);
                UpdateBalance();
                UpdateCategoryTotals();
                UpdateChart();
                ClearTransactionFields();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите описание.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditTransaction_Click(object sender, EventArgs e)
        {
            if (dgvTransactions.SelectedRows.Count > 0)
            {
                Transaction selectedTransaction = (Transaction)dgvTransactions.SelectedRows[0].DataBoundItem;
                if (selectedTransaction != null)
                {
                    // 1.  Получаем данные из UI
                    DateTime newDate = dtpTransactionDate.Value.Date;
                    string newDescription = txtTransactionDescription.Text.Trim();
                    decimal amount = nudTransactionAmount.Value;
                    TransactionType type = rdbTransactionExpense.Checked ? TransactionType.Expense : TransactionType.Income;
                    Category selectedCategory = (Category)cmbTransactionCategory.SelectedItem;

                    if (selectedCategory != null && !string.IsNullOrEmpty(newDescription))
                    {
                        // Создаем *новую* транзакцию с обновленными значениями
                        Transaction newTransaction = new Transaction(newDate, newDescription, amount, selectedCategory, type);
                        newTransaction.Id = selectedTransaction.Id;  // сохраняем старый ID
                        newTransaction.CategoryId = selectedCategory.Id; // Устанавливаем CategoryId

                        // Обновляем существующую транзакцию
                        dbHelper.UpdateTransaction(newTransaction, selectedTransaction);

                        accountingData.UpdateTransaction(selectedTransaction, newTransaction); // Используем существующую транзакцию

                        // Обновляем DataGridView
                        transactionsBindingSource.ResetBindings(false);

                        UpdateBalance();
                        UpdateCategoryTotals();
                        UpdateChart();
                        ClearTransactionFields();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите категорию и введите описание.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите транзакцию для редактирования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteTransaction_Click(object sender, EventArgs e)
        {
            if (dgvTransactions.SelectedRows.Count > 0)
            {
                Transaction selectedTransaction = (Transaction)dgvTransactions.SelectedRows[0].DataBoundItem;
                if (selectedTransaction != null)
                {
                    DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить транзакцию '{selectedTransaction.Description}'?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Удаляем транзакцию из базы данных
                        dbHelper.DeleteTransaction(selectedTransaction.Id);
                        // Удаляем транзакцию из списка в памяти
                        accountingData.DeleteTransaction(selectedTransaction.Date, selectedTransaction.Description, selectedTransaction.Amount);
                        transactionsBindingSource.ResetBindings(false);
                        UpdateBalance();
                        UpdateCategoryTotals();
                        UpdateChart();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите транзакцию для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearTransactionFields()
        {
            txtTransactionDescription.Clear();
            nudTransactionAmount.Value = 0;
            rdbTransactionIncome.Checked = true;
            dtpTransactionDate.Value = DateTime.Now; // Добавлено: сброс даты
        }

        private void UpdateBalance()
        {
            decimal balance = accountingData.CalculateBalance();
            lblBalance.Text = $"Баланс: {balance:C}";
        }

        private void UpdateCategoryTotals()
        {
            Dictionary<string, decimal> categoryTotals = accountingData.CalculateCategoryTotals();

            // Вывод в ListBox
            lstCategoryTotals.Items.Clear();
            foreach (var kvp in categoryTotals)
            {
                lstCategoryTotals.Items.Add($"{kvp.Key}: {kvp.Value:C}");
            }
        }

        private void UpdateChart()
        {
            chtCategoryTotals.Series.Clear();

            Series series = new Series("Category Totals");
            series.ChartType = SeriesChartType.Pie;

            Dictionary<string, decimal> chartData = accountingData.GetDataForChart();

            foreach (var kvp in chartData)
            {
                series.Points.AddXY(kvp.Key, kvp.Value);
            }

            chtCategoryTotals.Series.Add(series);
        }

        private void btnSaveTransactions_Click(object sender, EventArgs e)
        {
            SaveDataToFile();
        }

        private void btnLoadTransactions_Click(object sender, EventArgs e)
        {
            LoadDataFromFile();
        }
        // Обработчики событий для текстовых полей (можно добавлять валидацию)
        private void txtNameCategory_TextChanged(object sender, EventArgs e) { }
        private void txtDescriptionCategory_TextChanged(object sender, EventArgs e) { }
        private void dtpTransactionDate_ValueChanged(object sender, EventArgs e) { }
        private void cmbTransactionCategory_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtTransactionDescription_TextChanged(object sender, EventArgs e) { }
        private void nudTransactionAmount_ValueChanged(object sender, EventArgs e) { }
        private void rdbTransactionIncome_CheckedChanged(object sender, EventArgs e) { }
        private void rdbTransactionExpense_CheckedChanged(object sender, EventArgs e) { }

        private void btnCalculateBalance_Click(object sender, EventArgs e)
        {
            UpdateBalance();
        }

        // Обработчик события SelectionChanged для DataGridView
        private void dgvTransactions_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTransactions.SelectedRows.Count > 0)
            {
                Transaction selectedTransaction = (Transaction)dgvTransactions.SelectedRows[0].DataBoundItem;
                if (selectedTransaction != null)
                {
                    // Заполняем поля ввода данными выбранной транзакции
                    dtpTransactionDate.Value = selectedTransaction.Date;
                    txtTransactionDescription.Text = selectedTransaction.Description;
                    nudTransactionAmount.Value = selectedTransaction.Amount;

                    //пытаемся найти категорию по Id
                    Category selectedCategory = accountingData.Categories.FirstOrDefault(c => c.Id == selectedTransaction.CategoryId);

                    if (selectedCategory != null)
                    {
                        cmbTransactionCategory.SelectedItem = selectedCategory;
                    }
                    else
                    {
                        MessageBox.Show("Категория не найдена.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbTransactionCategory.SelectedIndex = -1; // сбрасываем выбор
                    }


                    if (selectedTransaction.Type == TransactionType.Income)
                    {
                        rdbTransactionIncome.Checked = true;
                    }
                    else
                    {
                        rdbTransactionExpense.Checked = true;
                    }
                }
            }
        }
    }
}