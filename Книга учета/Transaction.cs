using System;

namespace Книга_учета
{
    // Тип операции (доход/расход)
    public enum TransactionType
    {
        Expense,
        Income
    }

    // Класс для операции
    public class Transaction
    {
        public int Id { get; set; }  // Добавлено свойство Id
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }  // Ссылка на Category.Id
        public Category Category { get; set; } // Ссылка на Category
        public TransactionType Type { get; set; }

        public Transaction()
        {
            // Конструктор по умолчанию необходим для десериализации JSON
            Date = DateTime.Now;    // Инициализация для избежания NullReferenceException
            Description = string.Empty;  // Инициализация для избежания NullReferenceException
            Amount = 0.0m;     // Инициализация для избежания NullReferenceException
            Category = new Category(); // Инициализация для избежания NullReferenceException
            Type = TransactionType.Expense; // Значение по умолчанию
            CategoryId = 0; // Инициализация для избежания NullReferenceException
        }

        public Transaction(DateTime date, string description, decimal amount, Category category, TransactionType type)
        {
            Date = date;
            Description = description;
            Amount = amount;
            Category = category;
            CategoryId = category.Id;
            Type = type;
        }
    }
}