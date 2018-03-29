using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Porc
{
    class Marker_Loc
    {
        public string Marker_Id { get; set; }
        public string Ref_SNP { get; set; } 
        public string Allele1 { get; set; }
        public string Allele2 { get; set; }
        public string Chr { get; set; }
        public int Position { get; set; }
        public string Row_Index { get; set; }
        public string Ref_Name { get; set; }
        public int Autosomal { get; set; }
        public string Bead_Type_A { get; set; }
        public string Bead_Type_B { get; set; }
        public double GenTrain_Score { get; set; }
        public string Manifest { get; set; }
        public double AA_Freq { get; set; }
        public double AB_Freq { get; set; }
        public double BB_Freq { get; set; }
        public double A_Freq { get; set; }
        public double B_Freq { get; set; }
        public double Chi2_P_Value { get; set; }
        public double Exact_Test { get; set; }
        public int Included { get; set; }
        public float Call_Rate { get; set; }
        public int AA_count { get; set; }
        public int AB_count { get; set; }
        public int BB_count { get; set; }
        public int NC_count { get; set; }
        public double P23 { get; set; }
        public double P36 { get; set; }
        public double P12 { get; set; }
        public double P33 { get; set; }
        public double P15 { get; set; }

    }
}
