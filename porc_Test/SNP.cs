using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Porc
{
    class SNP
    {
        public string Sample_Id { get; set; }
        public string Marker_Id { get; set; }
        public int Row_Index { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Theta { get; set; }
        public float R { get; set; }
        public string Top_Allele_A { get; set; }
        public string Top_Allele_B { get; set; }
        public int Selected { get; set; }
        public float Call_Rate { get; set; }
        public string Manifest { get; set; }
        public string Genotype { get; set; }
    }

}
