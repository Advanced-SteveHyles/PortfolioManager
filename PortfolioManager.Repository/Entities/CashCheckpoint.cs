using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.BackEnd.Repository.Entities
{
    [Table("CashCheckpoint")]
    public class CashCheckpoint
    {
        [Key]
        public int CashCheckpointId { get; set; }
        public int AccountId {get;set;}
        
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal ClosingValue { get; set; }
        public decimal OpeningValue { get; set; }
        public string Reference { get; set; }
    }
}