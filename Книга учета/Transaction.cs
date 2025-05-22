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
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public TransactionType Type { get; set; }

        public Transaction()
        {
            // Конструктор по умолчанию необходим для десериализации JSON
            Date = DateTime.Now;    // Инициализация для избежания NullReferenceException
            Description = string.Empty;  // Инициализация для избежания NullReferenceException
            Amount = 0.0m;     // Инициализация для избежания NullReferenceException
            Category = new Category(); // Инициализация для избежания NullReferenceException
            Type = TransactionType.Expense; // Значение по умолчанию
        }

        public Transaction(DateTime date, string description, decimal amount, Category category, TransactionType type)
        {
            Date = date;
            Description = description;
            Amount = amount;
            Category = category;
            Type = type;
        }
    }
}