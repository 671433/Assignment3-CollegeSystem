using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wpf_Test2ScofFolding.Models;

[PrimaryKey("Coursecode", "Studentid")]
[Table("grade")]
public partial class Grade
{
    [Key]
    [Column("studentid")]
    public int Studentid { get; set; }

    [Key]
    [Column("coursecode")]
    [StringLength(6)]
    [Unicode(false)]
    public string Coursecode { get; set; } = null!;

    [Column("grade")]
    [StringLength(1)]
    [Unicode(false)]
    public string Grade1 { get; set; } = null!;

    [ForeignKey("Coursecode")]
    [InverseProperty("Grades")]
    public virtual Course CoursecodeNavigation { get; set; } = null!;

    [ForeignKey("Studentid")]
    [InverseProperty("Grades")]
    public virtual Student Student { get; set; } = null!;
}
