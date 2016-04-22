using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.BackEnd.Repository.Entities
{
    [Table("CashCheckpoint")]
    public class CashCheckpoint
    {
        [Key]
        public int CashCheckpointId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        
    }
}