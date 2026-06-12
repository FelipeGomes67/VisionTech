using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionTech.Models;

[Table("Categoria")]
public partial class Categoria
{
    [Key]
    [StringLength(40)]
    [Unicode(false)]
    public string IdCategoria { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [InverseProperty("IdCategoriaNavigation")]
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
