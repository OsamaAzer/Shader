﻿using Shader.Enums;

namespace Shader.Data.Entities
{
    public class Expense 
    {
        public int Id { get; set; }
        public ExpenseType Type { get; set; } // نوع المصروف
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public TimeOnly Time { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    }
}
