using System;

namespace Книга_учета
{
    // Класс для категории операции
    public class Category
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Category()
        {
            // Конструктор по умолчанию необходим для десериализации JSON
            Name = string.Empty;  // Инициализация для избежания NullReferenceException
            Description = string.Empty; // Инициализация для избежания NullReferenceException
        }

        public Category(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}