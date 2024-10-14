using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DTOs.EducationDtos;
public class EducationToUpdateDto
{
    public int Id { get; set; }
    public string Degree { get; set; }
    public string School { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
