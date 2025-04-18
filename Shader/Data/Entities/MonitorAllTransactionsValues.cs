﻿namespace Shader.Data.Entities
{
    public class MonitorAllTransactionsValues
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; } // المبلغ الكلي
        public decimal TotalAmountPaid { get; set; } // المبلغ المدفوع
        public decimal TotalRemainingAmount { get; set; } // المبلغ المتبقي
        public decimal AmountOfRateTookFromSuppliers { get; set; } // المبلغ الذي تم أخذه من الموردين
        ICollection<Client> Clients { get; set; } = new List<Client>();
        //ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
