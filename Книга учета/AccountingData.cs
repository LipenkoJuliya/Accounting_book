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
            //if (!Categories.Any(c => c.Name == category.Name))
            //{
            //dbHelper.AddCategory(category);
            //   Categories.Add(category);
            //}
            //else
            //{
            //    Console.WriteLine("Категория с таким именем уже существует.");
            //}
            Categories.Add(category);
        }

        public void UpdateCategory(string oldName, Category newCategory)
        {
            Category existingCategory = Categories.FirstOrDefault(c => c.Name == oldName);
            if (existingCategory != null)
            {
                // dbHelper.UpdateCategory(newCategory);
                existingCategory.Name = newCategory.Name;
                existingCategory.Description = newCategory.Description;
            }
            else
            {
                Console.WriteLine("Категория не найдена.");
            }
        }

        public void DeleteCategory(string categoryName)
        {
            Category categoryToRemove = Categories.FirstOrDefault(c => c.Name == categoryName);
            if (categoryToRemove != null)
            {
                // dbHelper.DeleteCategory(categoryName);
                Categories.Remove(categoryToRemove);
                // Удалить все транзакции, связанные с этой категорией.  Важно!
                Transactions.RemoveAll(t => t.Category.Name == categoryName);
            }
            else
            {
                Console.WriteLine("Категория не найдена.");
            }
        }

        // Методы CRUD для операций
        public void AddTransaction(Transaction transaction)
        {
            // dbHelper.AddTransaction(transaction);
            Transactions.Add(transaction);
        }

        public void UpdateTransaction(Transaction originalTransaction, Transaction newTransaction)
        {
            // Сначала ищем существующую транзакцию по дате, описанию и сумме
            Transaction existingTransaction = Transactions.FirstOrDefault(t =>
                t.Date == originalTransaction.Date &&
                t.Description == originalTransaction.Description &&
                t.Amount == originalTransaction.Amount);

            if (existingTransaction != null)
            {
                // Обновляем существующую транзакцию в базе данных
                //  dbHelper.UpdateTransaction(newTransaction, originalTransaction);

                // Обновляем свойства существующей транзакции в памяти
                existingTransaction.Date = newTransaction.Date;
                existingTransaction.Description = newTransaction.Description;
                existingTransaction.Amount = newTransaction.Amount;
                existingTransaction.Category = newTransaction.Category;
                existingTransaction.Type = newTransaction.Type;
            }
            else
            {
                Console.WriteLine("Транзакция не найдена для обновления.");
            }
        }

        public void DeleteTransaction(DateTime date, string description, decimal amount)
        {
            //  dbHelper.DeleteTransaction(date, description, amount);
            Transactions.RemoveAll(t => t.Date == date && t.Description == description && t.Amount == amount);
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