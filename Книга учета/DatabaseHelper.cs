using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Книга_учета
{
    public class DatabaseHelper : IDisposable
    {
        private string dbFilePath;
        private string connectionString;
        public SQLiteConnection connection { get; private set; } // getter public

        public DatabaseHelper(string dbFilePath)
        {
            this.dbFilePath = dbFilePath;
            this.connectionString = $"Data Source={dbFilePath};Version=3;";
            connection = new SQLiteConnection(connectionString);
        }

        public void CreateDatabaseIfNotExists()
        {
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
                CreateTables();
            }
        }

        private void CreateTables()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Создание таблицы Categories
                string createCategoriesTableQuery = @"
                    CREATE TABLE Categories (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,  -- Добавлено Id с автоинкрементом
                        Name TEXT,
                        Description TEXT
                    );";
                SQLiteCommand createCategoriesCommand = new SQLiteCommand(createCategoriesTableQuery, connection);
                createCategoriesCommand.ExecuteNonQuery();

                // Создание таблицы Transactions
                string createTransactionsTableQuery = @"
                    CREATE TABLE Transactions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, -- Добавлено Id с автоинкрементом
                        Date TEXT,
                        Description TEXT,
                        Amount REAL,
                        CategoryId INTEGER,  -- Ссылка на Id категории
                        Type INTEGER,
                        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                    );";
                SQLiteCommand createTransactionsCommand = new SQLiteCommand(createTransactionsTableQuery, connection);
                createTransactionsCommand.ExecuteNonQuery();
            }
        }

        // Методы для работы с категориями
        public int AddCategory(Category category) // Возвращаем ID
        {
            int newCategoryId = -1; //  По умолчанию -1, если что-то пойдет не так
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Categories (Name, Description) VALUES (@Name, @Description); SELECT last_insert_rowid();"; // Получаем ID
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@Name", category.Name);
                insertCommand.Parameters.AddWithValue("@Description", category.Description);
                newCategoryId = Convert.ToInt32(insertCommand.ExecuteScalar());  // Получаем ID

            }
            return newCategoryId;
        }

        public void UpdateCategory(Category category)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id;"; // Обновление по Id
                SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@Id", category.Id);  // Используем Id
                updateCommand.Parameters.AddWithValue("@Name", category.Name); // Добавил Name
                updateCommand.Parameters.AddWithValue("@Description", category.Description);
                updateCommand.ExecuteNonQuery();
            }
        }

        public void DeleteCategory(int categoryId) // Принимаем id
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Сначала удаляем транзакции, связанные с этой категорией
                string deleteTransactionsQuery = "DELETE FROM Transactions WHERE CategoryId = @CategoryId;"; // используем CategoryId
                SQLiteCommand deleteTransactionsCommand = new SQLiteCommand(deleteTransactionsQuery, connection);
                deleteTransactionsCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                deleteTransactionsCommand.ExecuteNonQuery();

                // Затем удаляем саму категорию
                string deleteCategoryQuery = "DELETE FROM Categories WHERE Id = @Id;"; // используем Id
                SQLiteCommand deleteCategoryCommand = new SQLiteCommand(deleteCategoryQuery, connection);
                deleteCategoryCommand.Parameters.AddWithValue("@Id", categoryId);
                deleteCategoryCommand.ExecuteNonQuery();
            }
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT Id, Name, Description FROM Categories;"; // Получаем Id
                SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category category = new Category(reader.GetString(1), reader.GetString(2));
                        category.Id = reader.GetInt32(0); // Устанавливаем Id
                        categories.Add(category);
                    }
                }
            }
            return categories;
        }

        // Методы для работы с транзакциями
        public void AddTransaction(Transaction transaction)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Transactions (Date, Description, Amount, CategoryId, Type) VALUES (@Date, @Description, @Amount, @CategoryId, @Type);";
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@Date", transaction.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                insertCommand.Parameters.AddWithValue("@Description", transaction.Description);
                insertCommand.Parameters.AddWithValue("@Amount", transaction.Amount);
                insertCommand.Parameters.AddWithValue("@CategoryId", transaction.CategoryId); // Используем CategoryId
                insertCommand.Parameters.AddWithValue("@Type", (int)transaction.Type);
                insertCommand.ExecuteNonQuery();
            }
        }

        public void UpdateTransaction(Transaction newTransaction, Transaction originalTransaction)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string updateQuery = @"
                UPDATE Transactions 
                SET Date = @Date, 
                    Description = @Description, 
                    Amount = @Amount, 
                    CategoryId = @CategoryId,  -- Обновляем CategoryId
                    Type = @Type 
                WHERE Id = @Id;";  // используем ID

                SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@Date", newTransaction.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                updateCommand.Parameters.AddWithValue("@Description", newTransaction.Description);
                updateCommand.Parameters.AddWithValue("@Amount", newTransaction.Amount);
                updateCommand.Parameters.AddWithValue("@CategoryId", newTransaction.CategoryId); // Используем CategoryId
                updateCommand.Parameters.AddWithValue("@Type", (int)newTransaction.Type);
                updateCommand.Parameters.AddWithValue("@Id", originalTransaction.Id); // используем Id
                updateCommand.ExecuteNonQuery();
            }
        }

        public void DeleteTransaction(int transactionId) // Принимаем ID
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Transactions WHERE Id = @Id;"; // используем ID
                SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@Id", transactionId);
                deleteCommand.ExecuteNonQuery();
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = @"
                SELECT t.Id, t.Date, t.Description, t.Amount, t.CategoryId, t.Type, c.Name, c.Description
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.Id;"; // Используем JOIN

                SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Transaction transaction = new Transaction();
                        transaction.Id = reader.GetInt32(0); // Получаем ID транзакции
                        transaction.Date = DateTime.Parse(reader.GetString(1));
                        transaction.Description = reader.GetString(2);
                        transaction.Amount = Convert.ToDecimal(reader.GetValue(3));
                        transaction.CategoryId = reader.GetInt32(4); // Получаем CategoryId
                        transaction.Type = (TransactionType)Convert.ToInt32(reader.GetValue(5));

                        Category category = new Category();
                        category.Id = transaction.CategoryId;  // Устанавливаем Id категории
                        category.Name = reader.GetString(6); // Получаем имя категории из результата запроса
                        category.Description = reader.GetString(7); // Получаем описание категории из результата запроса

                        transaction.Category = category; //  Присваиваем объект Category
                        transactions.Add(transaction);
                    }
                }
            }
            return transactions;
        }
        public void ClearAllData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Удаляем все данные из таблицы Transactions
                string deleteTransactionsQuery = "DELETE FROM Transactions;";
                SQLiteCommand deleteTransactionsCommand = new SQLiteCommand(deleteTransactionsQuery, connection);
                deleteTransactionsCommand.ExecuteNonQuery();

                // Удаляем все данные из таблицы Categories
                string deleteCategoriesQuery = "DELETE FROM Categories;";
                SQLiteCommand deleteCategoriesCommand = new SQLiteCommand(deleteCategoriesQuery, connection);
                deleteCategoriesCommand.ExecuteNonQuery();
            }
        }

        // Implementation of IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                        connection = null;
                    }
                }
                // Dispose unmanaged resources

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DatabaseHelper()
        {
            Dispose(false);
        }
    }
}