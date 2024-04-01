using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wpf_Test2ScofFolding.Models;

[Table("student")]
public partial class Student
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("studentname")]
    [StringLength(50)]
    [Unicode(false)]
    public string Studentname { get; set; } = null!;

    [Column("studentage")]
    public int Studentage { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
