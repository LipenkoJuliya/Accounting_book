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
                        Name TEXT PRIMARY KEY,
                        Description TEXT
                    );";
                SQLiteCommand createCategoriesCommand = new SQLiteCommand(createCategoriesTableQuery, connection);
                createCategoriesCommand.ExecuteNonQuery();

                // Создание таблицы Transactions
                string createTransactionsTableQuery = @"
                    CREATE TABLE Transactions (
                        Date TEXT,
                        Description TEXT,
                        Amount REAL,
                        CategoryName TEXT,
                        Type INTEGER,
                        FOREIGN KEY (CategoryName) REFERENCES Categories(Name)
                    );";
                SQLiteCommand createTransactionsCommand = new SQLiteCommand(createTransactionsTableQuery, connection);
                createTransactionsCommand.ExecuteNonQuery();
            }
        }

        // Методы для работы с категориями
        public void AddCategory(Category category)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Categories (Name, Description) VALUES (@Name, @Description);";
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@Name", category.Name);
                insertCommand.Parameters.AddWithValue("@Description", category.Description);
                insertCommand.ExecuteNonQuery();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE Categories SET Description = @Description WHERE Name = @Name;";
                SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@Name", category.Name);
                updateCommand.Parameters.AddWithValue("@Description", category.Description);
                updateCommand.ExecuteNonQuery();
            }
        }

        public void DeleteCategory(string categoryName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Сначала удаляем транзакции, связанные с этой категорией
                string deleteTransactionsQuery = "DELETE FROM Transactions WHERE CategoryName = @CategoryName;";
                SQLiteCommand deleteTransactionsCommand = new SQLiteCommand(deleteTransactionsQuery, connection);
                deleteTransactionsCommand.Parameters.AddWithValue("@CategoryName", categoryName);
                deleteTransactionsCommand.ExecuteNonQuery();

                // Затем удаляем саму категорию
                string deleteCategoryQuery = "DELETE FROM Categories WHERE Name = @Name;";
                SQLiteCommand deleteCategoryCommand = new SQLiteCommand(deleteCategoryQuery, connection);
                deleteCategoryCommand.Parameters.AddWithValue("@Name", categoryName);
                deleteCategoryCommand.ExecuteNonQuery();
            }
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT Name, Description FROM Categories;";
                SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category category = new Category(reader.GetString(0), reader.GetString(1));
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
                string insertQuery = "INSERT INTO Transactions (Date, Description, Amount, CategoryName, Type) VALUES (@Date, @Description, @Amount, @CategoryName, @Type);";
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@Date", transaction.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                insertCommand.Parameters.AddWithValue("@Description", transaction.Description);
                insertCommand.Parameters.AddWithValue("@Amount", transaction.Amount);
                insertCommand.Parameters.AddWithValue("@CategoryName", transaction.Category.Name);
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
                    CategoryName = @CategoryName, 
                    Type = @Type 
                WHERE Date = @OriginalDate 
                  AND Description = @OriginalDescription 
                  AND Amount = @OriginalAmount;";

                SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@Date", newTransaction.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                updateCommand.Parameters.AddWithValue("@Description", newTransaction.Description);
                updateCommand.Parameters.AddWithValue("@Amount", newTransaction.Amount);
                updateCommand.Parameters.AddWithValue("@CategoryName", newTransaction.Category.Name);
                updateCommand.Parameters.AddWithValue("@Type", (int)newTransaction.Type);
                updateCommand.Parameters.AddWithValue("@OriginalDate", originalTransaction.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                updateCommand.Parameters.AddWithValue("@OriginalDescription", originalTransaction.Description);
                updateCommand.Parameters.AddWithValue("@OriginalAmount", originalTransaction.Amount);
                updateCommand.ExecuteNonQuery();
            }
        }

        public void DeleteTransaction(DateTime date, string description, decimal amount)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Transactions WHERE Date = @Date AND Description = @Description AND Amount = @Amount;";
                SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd HH:mm:ss"));
                deleteCommand.Parameters.AddWithValue("@Description", description);
                deleteCommand.Parameters.AddWithValue("@Amount", amount);
                deleteCommand.ExecuteNonQuery();
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT Date, Description, Amount, CategoryName, Type FROM Transactions;";
                SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.Parse(reader.GetString(0));
                        string description = reader.GetString(1);
                        decimal amount = Convert.ToDecimal(reader.GetValue(2));
                        string categoryName = reader.GetString(3);
                        TransactionType type = (TransactionType)Convert.ToInt32(reader.GetValue(4));

                        // Получаем категорию по имени
                        Category category = GetAllCategories().FirstOrDefault(c => c.Name == categoryName);

                        Transaction transaction = new Transaction(date, description, amount, category, type);
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