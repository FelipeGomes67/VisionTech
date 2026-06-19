using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VisionTech.Models;

[Table("tarefa_tb")]
public partial class TarefaTb
{
    [Key]
    public Guid IdTarefa { get; set; }

    [Column("título_da_tarefa")]
    [StringLength(255)]
    public string TítuloDaTarefa { get; set; } = null!;

    [Column("descrição", TypeName = "text")]
    public string Descrição { get; set; } = null!;

    [Column("status_de_conclusão")]
    [StringLength(255)]
    public string StatusDeConclusão { get; set; } = null!;

    [Column("data_de_criacao", TypeName = "datetime")]
    public DateTime DataDeCriacao { get; set; }
}
