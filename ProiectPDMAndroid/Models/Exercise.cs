using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPDMAndroid.Models
{
    public class Exercise
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Equipment { get; set; }
        public string Muscle { get; set; }
        public string MuscleGroup { get; set; }
        public string Date { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
