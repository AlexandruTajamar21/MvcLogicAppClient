using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeguridadApiAlumnosPractica.Model
{
    [Table("Alumnos")]
    public class Alumno
    {
        [Key]
        [Column("idalumno")]
        public int IdAlumno { get; set; }
        [Column("curso")]
        public string Curso { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("apellidos")]
        public string Apellidos { get; set; }
        [Column("nota")]
        public int Nota { get; set; }
    }
}
