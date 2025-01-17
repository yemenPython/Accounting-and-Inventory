﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUL
{
    public class PointOfPurchaseCUL
    {
        public int Id { get; set; }
        public int InvoiceNo { get; set; }
        public int PaymentTypeId { get; set; }
        public int SupplierId { get; set; }
        public decimal TotalProductQuantity { get; set; }
        public decimal GrossCostTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Vat { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public int AssetId { get; set; }
        public DateTime AddedDate { get; set; }
        public int AddedBy { get; set; }
    }
}
