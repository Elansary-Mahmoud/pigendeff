using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Porc
{
    class Pedigree
    {
        public string Indv_Id { get; set; }
        public string Paternal_Id { get; set; }
        public string Maternal_Id { get; set; }
        public string Paternal_Id_Modified { get; set; }
        public string Maternal_Id_Modified { get; set; }
        public int Modified { get; set; }
         public string Num_Offspring { get; set; }
         public string Num_Siblings { get; set; }
 
    }
}
