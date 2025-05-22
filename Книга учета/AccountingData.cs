using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Книга_учета
{
    // Класс для хранения всех данных
    public class AccountingData
    {
        //private DatabaseHelper dbHelper; // Убрали

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public AccountingData()  // Конструктор по умолчанию
        {
            Categories = new List<Category>();
            Transactions = new List<Transaction>();
        }


        // Конструктор
        /* public AccountingData(DatabaseHelper dbHelper)
         {
             this.dbHelper = dbHelper;
             LoadDataFromDatabase();
         }
         */ // удаляем вызов

        /*
        private void LoadDataFromDatabase()
        {
            Categories = dbHelper.GetAllCategories();
            Transactions = dbHelper.GetAllTransactions();
        }
        */ //удалили

        // Методы CRUD для категорий
        public void AddCategory(Category category)
        {
            Categories.Add(category);
        }

        public void UpdateCategory(string oldName, Category newCategory)
        {
            Category existingCategory = Categories.FirstOrDefault(c => c.Name == oldName);
            if (existingCategory != null)
            {
                existingCategory.Name = newCategory.Name;
                existingCategory.Description = newCategory.Description;
                existingCategory.Id = newCategory.Id;
            }
            else
            {
                Console.WriteLine("Категория не найдена.");
            }
        }

        public void DeleteCategory(string categoryName) //  Удалить параметр
        {
            Category categoryToRemove = Categories.FirstOrDefault(c => c.Name == categoryName);
            if (categoryToRemove != null)
            {
                Categories.Remove(categoryToRemove);
                // Удалить все транзакции, связанные с этой категорией.  Важно!
                Transactions.RemoveAll(t => t.Category.Name == categoryToRemove.Name);  // Обновлено
            }
            else
            {
                Console.WriteLine("Категория не найдена.");
            }
        }

        // Методы CRUD для операций
        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public void UpdateTransaction(Transaction originalTransaction, Transaction newTransaction)
        {
            // Сначала ищем существующую транзакцию по Id
            Transaction existingTransaction = Transactions.FirstOrDefault(t => t.Id == originalTransaction.Id);

            if (existingTransaction != null)
            {
                // Обновляем свойства существующей транзакции в памяти
                existingTransaction.Date = newTransaction.Date;
                existingTransaction.Description = newTransaction.Description;
                existingTransaction.Amount = newTransaction.Amount;
                existingTransaction.CategoryId = newTransaction.CategoryId;  // Обновляем CategoryId
                existingTransaction.Category = newTransaction.Category;
                existingTransaction.Type = newTransaction.Type;
            }
            else
            {
                Console.WriteLine("Транзакция не найдена для обновления.");
            }
        }

        public void DeleteTransaction(DateTime date, string description, decimal amount) // Удаляем параметры
        {
            // Transactions.RemoveAll(t => t.Date == date && t.Description == description && t.Amount == amount);
        }

        // Подсчет баланса
        public decimal CalculateBalance()
        {
            decimal income = Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            decimal expense = Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            return income - expense;
        }

        // Подсчет суммы по категориям
        public Dictionary<string, decimal> CalculateCategoryTotals()
        {
            return Transactions.GroupBy(t => t.Category.Name)
                                   .ToDictionary(g => g.Key, g => g.Sum(t => (t.Type == TransactionType.Income ? t.Amount : -t.Amount)));
        }

        // Подготовка данных для графика (пример)
        public Dictionary<string, decimal> GetDataForChart()
        {
            return CalculateCategoryTotals();  // Используем уже посчитанные суммы по категориям
        }
    }
}