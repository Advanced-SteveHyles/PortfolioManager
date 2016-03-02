using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Ref
{
  public  class DBGenerator
    {
      [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
      public int VersionID { get; set; }

    }
}
