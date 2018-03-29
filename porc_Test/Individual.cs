using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Porc
{
    class Individual
    {
        public string Indv_Id { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Birth_Date { get; set; }
        public int Line { get; set; }
        public string Location { get; set; }
        public int Included { get; set; }
        public float Call_Rate { get; set; }
        public int New_Dataset { get; set; }
        public int Founder { get; set; }
    }
}
