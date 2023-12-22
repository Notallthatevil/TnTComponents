using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveTest.Client.Data;

public class DataGridItem {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Column1 { get; set; }

    public string Column2 { get; set; } = default!;

    public DateTime Column3 { get; set; }
}