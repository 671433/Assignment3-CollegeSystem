using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wpf_Test2ScofFolding.Models;

[Table("course")]
public partial class Course
{
    [Key]
    [Column("coursecode")]
    [StringLength(6)]
    [Unicode(false)]
    public string Coursecode { get; set; } = null!;

    [Column("coursename")]
    [StringLength(50)]
    [Unicode(false)]
    public string Coursename { get; set; } = null!;

    [Column("semester")]
    [StringLength(1)]
    [Unicode(false)]
    public string Semester { get; set; } = null!;

    [Column("teacher")]
    [StringLength(50)]
    [Unicode(false)]
    public string Teacher { get; set; } = null!;

    [InverseProperty("CoursecodeNavigation")]
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
