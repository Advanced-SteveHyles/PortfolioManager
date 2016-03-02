using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Ref
{
    public  class  AccountType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountTypeID { get; set; }
        
        public string TypeName { get; set; }
        public bool isDeleted {get; set;}
    }

}

