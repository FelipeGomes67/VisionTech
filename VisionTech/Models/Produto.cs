using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionTech.Models;

[Table("Produto")]
public partial class Produto
{
    [Key]
    [StringLength(40)]
    [Unicode(false)]
    public string IdProduto { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Imagem { get; set; }

    [Required]
    public int QuantidadeEstoque { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string? IdCategoria { get; set; }

    [ForeignKey("IdCategoria")]
    [InverseProperty("Produtos")]
    public virtual Categoria? IdCategoriaNavigation { get; set; }
}