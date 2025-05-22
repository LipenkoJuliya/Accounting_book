using System;

namespace Книга_учета
{
    public class Category
    {
        public int Id { get; set; }  // Добавлено свойство Id
        public string Name { get; set; }
        public string Description { get; set; }

        public Category() { }  // Конструктор по умолчанию для десериализации

        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}