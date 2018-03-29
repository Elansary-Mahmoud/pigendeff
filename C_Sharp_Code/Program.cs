using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data.OleDb;
using MatrixLibrary;
//using System.Data.OleDb;

    
using System.Threading.Tasks;
using System.Threading;

    // using RDotNet;

    //using STATCONNECTORCLNTLib;
    //using StatConnectorCommonLib;
    //using STATCONNECTORSRVLib;


    namespace Porc
    {
        class Program
        {


            public static void inser_defect_excel()
            {

                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Call_Rate
                                FROM tblindividual LEFT OUTER JOIN tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id
                                WHERE (tblindividual.Call_Rate IS NOT NULL) AND (tblindividual.Included = 1) AND (tblindividual_phenotype.Phenotype_Id IS NULL)
                                ORDER BY tblindividual_phenotype.Phenotype_Id, tblindividual.Indv_Id";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();

                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";
                string CnStr = string.Empty;
                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }
                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                DA.Fill(ds, "samples");



                foreach (DataRow dr in ds.Tables["samples"].Rows)
                {

                    foreach (string indv_id in Indv_List)
                    {
                        if (dr[0].ToString().Trim().CompareTo(indv_id) == 0)
                        {
                            Individual_Phenotype indv_phen = new Individual_Phenotype();
                            indv_phen.Indv_Id = indv_id;
                            var defect = hshTable[dr[22].ToString().Trim()];
                            indv_phen.Phenotype_Id = int.Parse(defect.ToString());
                            excute_Individual_Phenotype(indv_phen);
                            break;
                        }
                    }
                }

                ///////////////////////////////////////////////////////////////////////

                //F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\New_Data_Aneleen_31_08.xlsx";
                //CnStr = string.Empty;
                //if (Path.GetExtension(F1Name) == ".xlsx")
                //{
                //    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                //        ";Extended Properties=Excel 12.0;";
                //}
                //else
                //{
                //    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                //}
                //ds.Clear();
                //DA = new OleDbDataAdapter("Select * from [Originele lijst$]", CnStr);
                //DA.Fill(ds, "Originele lijst");

                //foreach (DataRow dr in ds.Tables["Originele lijst"].Rows)
                //{

                //    foreach (string indv_id in Indv_List)
                //    {
                //        if (dr[0].ToString().Trim().CompareTo(indv_id) == 0)
                //        {
                //            Individual_Phenotype indv_phen = new Individual_Phenotype();
                //            indv_phen.Indv_Id = indv_id;
                //            var defect = hshTable[dr[22].ToString().Trim()];
                //            indv_phen.Phenotype_Id = int.Parse(defect.ToString());
                //            excute_Individual_Phenotype(indv_phen);
                //            break;
                //        }
                //    }
                //}



            }

            public static void insert_defect_access()
            {

                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"select * from tblindividual where (indv_id != '0' AND indv_id != 'General')";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                string conStr = @"Provider=Microsoft.JET.OLEDB.4.0; data source=PIGENDEF_data.mdb; Jet OLEDB:Database Password=;";
                con = new OleDbConnection(conStr);  // connection string change database name and password here.
                con.Open(); //connection must be openned
                OleDbCommand cmd = new OleDbCommand("SELECT * from Defect;", con); // creating query command
                OleDbDataReader reader = cmd.ExecuteReader(); // executes query
                while (reader.Read()) // if can read row from database
                {
                    foreach (string indv_id in Indv_List)
                    {
                        if (reader.GetValue(0).ToString().CompareTo(indv_id) == 0)
                        {
                            Individual_Phenotype indv_phen = new Individual_Phenotype();
                            indv_phen.Indv_Id = indv_id;
                            var defect = hshTable[reader.GetValue(1).ToString()];
                            indv_phen.Phenotype_Id = int.Parse(defect.ToString());
                            excute_Individual_Phenotype(indv_phen);
                            break;
                        }
                    }
                }


            }

            public static void check_RG()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Line
FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                ArrayList Acess = new ArrayList();
                ArrayList Excel = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                string conStr = @"Provider=Microsoft.JET.OLEDB.4.0; data source=PIGENDEF_data.mdb; Jet OLEDB:Database Password=;";
                con = new OleDbConnection(conStr);  // connection string change database name and password here.
                con.Open(); //connection must be openned
                OleDbCommand cmd = new OleDbCommand("SELECT * from Defect;", con); // creating query command
                OleDbDataReader reader = cmd.ExecuteReader(); // executes query
                bool found_acess = false;
                int Acess_count = 0;
                while (reader.Read()) // if can read row from database
                {
                    if (Acess_count < 720)
                    {
                        found_acess = false;
                        foreach (string indv_id in Indv_List)
                        {
                            if (reader.GetValue(0).ToString().CompareTo(indv_id) == 0)
                            {
                                Acess.Add(indv_id + "  " + reader.GetValue(1).ToString());
                                found_acess = true;
                                Acess_count++;
                                break;
                            }
                        }
                        if (!found_acess)
                        {
                            Excel.Add(reader.GetValue(0).ToString() + " not found ");
                        }
                    }
                }

                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                DA.Fill(ds, "samples");
                bool found = false;
                try
                {
                    int Excel_count = 0;
                    foreach (DataRow dr in ds.Tables["samples"].Rows)
                    {
                        if (Excel_count < 720)
                        {
                            found = false;
                            for (int j = 0; j < Indv_List.Count; j++)
                            {
                                if (Indv_List[j].Equals(dr[0].ToString().Trim()))
                                {
                                    Excel.Add(Indv_List[j] + " " + dr[22].ToString().Trim());
                                    found = true;
                                    Excel_count++;
                                    break;
                                }

                            }
                            if (!found)
                            {
                                //Excel.Add(dr[0].ToString().Trim() + " not found ");
                                //Excel_count++;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("finished");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                DA.Dispose();
                for (int i = 0; i < Indv_List.Count; i++)
                {
                    System.Console.WriteLine(Acess[i].ToString());
                    System.Console.WriteLine(Excel[i].ToString());
                }

            }

            public static double chisqr(int Dof, double Cv)
            {
                if (Cv < 0 || Dof < 1)
                {
                    return 0.0;
                }
                double K = ((double)Dof) * 0.5;
                double X = Cv * 0.5;
                if (Dof == 2)
                {
                    return Math.Exp(-1.0 * X);
                }

                double PValue = igf(K, X);
                if (double.IsNaN(PValue) || double.IsInfinity(PValue) || PValue <= 1e-8)
                {
                    return 1e-14;
                }

                PValue /= approx_gamma(K);
                //PValue /= tgamma(K); 

                return (1.0 - PValue);
            }

            public static double igf(double S, double Z)
            {
                if (Z < 0.0)
                {
                    return 0.0;
                }
                double Sc = (1.0 / S);
                Sc *= Math.Pow(Z, S);
                Sc *= Math.Exp(-Z);

                double Sum = 1.0;
                double Nom = 1.0;
                double Denom = 1.0;

                for (int I = 0; I < 200; I++)
                {
                    Nom *= Z;
                    S++;
                    Denom *= S;
                    Sum += (Nom / Denom);
                }

                return Sum * Sc;
            }

            public static double approx_gamma(double Z)
            {
                const double RECIP_E = 0.36787944117144232159552377016147;  // RECIP_E = (E^-1) = (1.0 / E)
                const double TWOPI = 6.283185307179586476925286766559;  // TWOPI = 2.0 * PI

                double D = 1.0 / (10.0 * Z);
                D = 1.0 / ((12 * Z) - D);
                D = (D + Z) * RECIP_E;
                D = Math.Pow(D, Z);
                D *= Math.Sqrt(TWOPI / Z);

                return D;
            }

            static public void excute_Sample(Sample sample)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select * from db_proc.tblSample where Sample_Id = '" + sample.Sample_Id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                if (Found == false)
                {
                    DateTime dat = DateTime.Parse(sample.Imaging_Date);
                    string formatForMySql = dat.ToString("yyyy-MM-dd HH:mm");
                    cmd.CommandText = "INSERT INTO db_Proc.tblSample(Sample_Id, DNA_Id, Row_Index, Sentrix_Id, Sentrix_Position, Call_Rate, Imaging_Date, Scanner_ID, Software_Version, User) "
                    + "VALUES (@Sample_Id, @DNA_Id, @Index , @Sentrix_Id, @Sentrix_Position, @Call_Rate, @Imaging_Date, @Scanner_ID, @Software_Version, @User)";
                    cmd.Parameters.AddWithValue("@Sample_Id", sample.Sample_Id);
                    cmd.Parameters.AddWithValue("@DNA_Id", sample.DNA_Id);
                    cmd.Parameters.AddWithValue("@Index", sample.Index);
                    cmd.Parameters.AddWithValue("@Sentrix_Id", sample.Sentrix_Id);
                    cmd.Parameters.AddWithValue("@Sentrix_Position", sample.Sentrix_Position);
                    cmd.Parameters.AddWithValue("@Call_Rate", sample.Call_Rate);
                    cmd.Parameters.AddWithValue("@Imaging_Date", formatForMySql);
                    cmd.Parameters.AddWithValue("@Scanner_ID", sample.Scanner_Id);
                    cmd.Parameters.AddWithValue("@Software_Version", sample.Software_Version);
                    cmd.Parameters.AddWithValue("@User", sample.User);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            static public void excute_DNA(DNA dna)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select * from db_proc.tblDNA where DNA_Id = '" + dna.DNA_Id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();

                if (Found == false)
                {

                    cmd.CommandText = "INSERT INTO db_Proc.tblDNA(DNA_Id, DNA_Conc)"
                    + "VALUES (@DNA_Id, @DNA_Conc)";
                    cmd.Parameters.AddWithValue("@DNA_Id", dna.DNA_Id);
                    cmd.Parameters.AddWithValue("@DNA_Conc", dna.DNA_Conc);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Console.WriteLine("The DNA ID " + dna.DNA_Id + " has been succesfully inserted");
                }
                conn.Close();
            }

            static public void excute_Box(Box box)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";


                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "select * from db_proc.tblBox where Box_Id = '" + box.Box_Id + "'";
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();

                if (Found == false)
                {
                    MySqlCommand cmd = new MySqlCommand(ConString, conn);

                    cmd.CommandText = "INSERT INTO db_Proc.tblBox(Box_Id, Box_Row, Box_Col, Ref, Client_Id) "
                    + "VALUES (@Box_Id, @Box_Row, @Box_Col, Ref, @Client_Id)";
                    cmd.Parameters.AddWithValue("@Box_Id", box.Box_Id);
                    cmd.Parameters.AddWithValue("@Box_Row", box.Box_Row);
                    cmd.Parameters.AddWithValue("@Box_Col", box.Box_Col);
                    cmd.Parameters.AddWithValue("@Ref", box.Ref);
                    cmd.Parameters.AddWithValue("@Client_Id", box.Client_Id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            static public void excute_Box_DNA(Box_DNA box_dna)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select * from db_proc.tblBox_DNA where Box_Id = '" + box_dna.Box_Id + "' AND DNA_Id = '" + box_dna.DNA_Id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                if (Found == false)
                {

                    cmd.CommandText = "INSERT INTO db_Proc.tblBox_DNA(Box_Id, DNA_Id) "
                    + "VALUES (@Box_Id, @DNA_Id)";
                    cmd.Parameters.AddWithValue("@Box_Id", box_dna.Box_Id);
                    cmd.Parameters.AddWithValue("@DNA_Id", box_dna.DNA_Id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            static public void excute_Individual(Individual indv)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select * from db_proc.tblIndividual where Indv_Id = '" + indv.Indv_Id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                if (Found == false)
                {

                    cmd.CommandText = "INSERT INTO db_Proc.tblIndividual(Indv_Id, Gender, Age, Birth_Date, Line, Location) "
                    + "VALUES (@Indv_Id, @Gender, @Age,  @Birth_Date, @Line, @Location)";
                    cmd.Parameters.AddWithValue("@Indv_Id", indv.Indv_Id);
                    cmd.Parameters.AddWithValue("@Gender", indv.Gender);
                    cmd.Parameters.AddWithValue("@Age", indv.Age);
                    cmd.Parameters.AddWithValue("@Birth_Date", indv.Birth_Date);
                    cmd.Parameters.AddWithValue("@Line", indv.Line);
                    cmd.Parameters.AddWithValue("@Location", indv.Location);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("The Indvidiual " + indv.Indv_Id + " has been succesfully inserted");
                }
                conn.Close();
            }

            static public void excute_Individual_Phenotype(Individual_Phenotype indv_phen)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select * from db_proc.tblIndividual_Phenotype where Indv_Id = '" + indv_phen.Indv_Id + "' AND Phenotype_Id = '" + indv_phen.Phenotype_Id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                if (Found == false)
                {
                    cmd.CommandText = "INSERT INTO db_Proc.tblIndividual_Phenotype(Indv_Id, Phenotype_ID) "
                    + "VALUES (@Indv_Id, @Phenotype_ID)";
                    cmd.Parameters.AddWithValue("@Indv_Id", indv_phen.Indv_Id);
                    cmd.Parameters.AddWithValue("@Phenotype_ID", indv_phen.Phenotype_Id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            static public void excute_Individual_Sample(Individual_Sample indv_sample)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select * from db_proc.tblIndividual_Sample where Indv_Id = '" + indv_sample.Indv_Id + "' AND Sample_Id = '" + indv_sample.Sample_Id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                if (Found == false)
                {

                    cmd.CommandText = "INSERT INTO db_Proc.tblIndividual_Sample(Indv_Id, Sample_Id) "
                    + "VALUES (@Indv_Id, @Sample_Id)";
                    cmd.Parameters.AddWithValue("@Indv_Id", indv_sample.Indv_Id);
                    cmd.Parameters.AddWithValue("@Sample_Id", indv_sample.Sample_Id);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            static public string Sample_Lookup(string indv_id)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                string Sample_Id = "";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select Sample_Id from db_proc.tblIndividual_Sample where Indv_Id = '" + indv_id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Sample_Id = Reader.GetValue(0).ToString();
                }
                Reader.Close();
                conn.Close();
                return Sample_Id;
            }

            static public void excute_Phenotype(Phenotype phen)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(ConString, conn);

                cmd.CommandText = "INSERT INTO db_Proc.tblPhenotype(Phenotype_Id, Phenotype_Name, Measure, Value) "
                + "VALUES (@Phenotype_Id, @Phenotype_Name, @Measure, @Value)";
                cmd.Parameters.AddWithValue("@Indv_Id", phen.Phenotype_Id);
                cmd.Parameters.AddWithValue("@Phenotype_Name", phen.Phenotype_Name);
                cmd.Parameters.AddWithValue("@Measure", phen.Measure);
                cmd.Parameters.AddWithValue("@Value", phen.Value);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            static public void excute_SNP(SNP snp)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(ConString, conn);
                cmd.CommandTimeout = 30000;
                cmd.CommandText = "INSERT INTO db_Proc.tblSNP(Sample_Id, Marker_Id, Row_Index, X, Y, Theta, R, Top_Allele_A, Top_Allele_B, Selected, Call_Rate, Manifest, Genotype) "
                 + "VALUES ('" + snp.Sample_Id + "', '" + snp.Marker_Id + "', " + snp.Row_Index + " , " + snp.X + "," + snp.Y + "," + snp.Theta + "," + snp.R + ",'" + snp.Top_Allele_A + "','" + snp.Top_Allele_B + "'," + snp.Selected + "," + snp.Call_Rate + ", null ,'" + snp.Genotype + "')";
                cmd.ExecuteNonQuery();
                conn.Close();

            }

            static public void excute_Marker_Loc(Marker_Loc marker)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(ConString, conn);
                cmd.CommandText = "INSERT INTO db_Proc.tblmarker_loc(Marker_Id, Allele1, Allele2, Chr, Position, Row_Index, Ref_Name, Autosomal, Bead_Type_A, Bead_Type_B, GenTrain_Score, Manifest) "
                + "VALUES (@Marker_Id, @Allele1, @Allele2, @Chr, @Position, @Row_Index, @Ref_Name, @Autosomal,  @Bead_Type_A, @Bead_Type_B, @GenTrain_Score, @Manifest)";
                cmd.Parameters.AddWithValue("@Marker_Id", marker.Marker_Id);
                cmd.Parameters.AddWithValue("@Allele1", marker.Allele1);
                cmd.Parameters.AddWithValue("@Allele2", marker.Allele2);
                cmd.Parameters.AddWithValue("@Chr", marker.Chr);
                cmd.Parameters.AddWithValue("@Position", marker.Position);
                cmd.Parameters.AddWithValue("@Row_Index", marker.Row_Index);
                cmd.Parameters.AddWithValue("@Ref_Name", marker.Ref_Name);
                cmd.Parameters.AddWithValue("@Autosomal", marker.Autosomal);
                cmd.Parameters.AddWithValue("@Bead_Type_A", marker.Bead_Type_A);
                cmd.Parameters.AddWithValue("@Bead_Type_B", marker.Bead_Type_B);
                cmd.Parameters.AddWithValue("@GenTrain_Score", marker.GenTrain_Score);
                cmd.Parameters.AddWithValue("@Manifest", marker.Manifest);
                cmd.ExecuteNonQuery();
                conn.Close();
                Console.WriteLine("The Marker " + marker.Marker_Id + " with index " + marker.Row_Index + " has been succesfully inserted");
            }

            static public Boolean excute_Pedigree(Pedigree ped)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                if (ped.Paternal_Id.CompareTo("0") == 0 || ped.Paternal_Id.CompareTo("") == 0)
                {
                    ped.Paternal_Id = "General";
                }
                if (ped.Maternal_Id.CompareTo("0") == 0 || ped.Maternal_Id.CompareTo("") == 0)
                {
                    ped.Maternal_Id = "General";
                }

                cmd.CommandText = "select * from db_proc.tblPedigree where Indv_Id = '" + ped.Indv_Id + "' AND Maternal_Id = '" + ped.Maternal_Id + "' AND paternal_Id ='" + ped.Paternal_Id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                if (Found == false)
                {
                    cmd.CommandText = "INSERT INTO db_Proc.tblPedigree(Indv_Id, Maternal_Id, Paternal_Id, Maternal_Id_Modified, Paternal_Id_Modified, Modified, Num_Offspring, Num_Siblings) "
                    + "VALUES (@Indv_Id, @Maternal_Id, @Paternal_Id, @Maternal_Id_Modified, @Paternal_Id_Modified, @Modified, @Num_Offspring, @Num_Siblings) ";
                    cmd.Parameters.AddWithValue("@Indv_Id", ped.Indv_Id);
                    cmd.Parameters.AddWithValue("@Maternal_Id", ped.Maternal_Id);
                    cmd.Parameters.AddWithValue("@Paternal_Id", ped.Paternal_Id);
                    cmd.Parameters.AddWithValue("@Maternal_Id_Modified", ped.Maternal_Id_Modified);
                    cmd.Parameters.AddWithValue("@Paternal_Id_Modified", ped.Paternal_Id_Modified);
                    cmd.Parameters.AddWithValue("@Modified", ped.Modified);
                    cmd.Parameters.AddWithValue("@Num_Offspring", ped.Num_Offspring);
                    cmd.Parameters.AddWithValue("@Num_Siblings", ped.Num_Siblings);
                    cmd.ExecuteNonQuery();
                    //cmd.CommandText = @"INSERT INTO db_Proc.tblPedigree(Indv_Id, Maternal_Id, Paternal_Id, Maternal_Id_Modified, Paternal_Id_Modified, Modified, Num_Offspring, Num_Siblings) " +
                    //"VALUES ('" + ped.Indv_Id + @"','" + ped.Maternal_Id + @"','" + ped.Paternal_Id + @"', null, null, 0, null,null)";
                }
                conn.Close();
                return Found;

            }

            public static Boolean In_skip_List(ArrayList Skip_list, string indv)
            {
                Boolean Skip = false;
                for (int i = 0; i < Skip_list.Count; i++)
                {
                    if (indv.CompareTo(Skip_list[i]) == 0)
                    {
                        Skip = true;
                        break;
                    }
                }
                return Skip;
            }

            public static void Insert_Sample()
            {
                int count = 0;
                string line = "";
                char[] delimiters = new char[] { '\t' };
                char[] delimiters2 = new char[] { '.' };

                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Samples Table.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        if (count != 0)
                        {
                            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                //if (!Indv_Exist(line_parts[17]))
                                //{
                                    DNA dna = new DNA();        // Insert The DNA information
                                    dna.DNA_Id = line_parts[0];
                                    dna.DNA_Conc = line_parts[6];
                                    excute_DNA(dna);
                                    /////////////////////////////////////////////////////////////////////////
                                    Sample sample = new Sample();       // Insert The Sample information
                                    sample.DNA_Id = line_parts[0];
                                    sample.Call_Rate = float.Parse(line_parts[9]);
                                    sample.Index = int.Parse(line_parts[7]);
                                    sample.Sample_Id = line_parts[8];
                                    sample.Sentrix_Id = line_parts[11];
                                    sample.Sentrix_Position = line_parts[12];
                                    sample.Imaging_Date = line_parts[13];
                                    sample.Scanner_Id = line_parts[14];
                                    sample.Software_Version = line_parts[15];
                                    sample.User = line_parts[16];
                                    excute_Sample(sample);
                                    //////////////////////////////////////////////////////////////////////////
                                    Box box = new Box();            // Insert The Box information
                                    box.Box_Id = line_parts[1];
                                    box.Box_Row = line_parts[2];
                                    box.Box_Col = int.Parse(line_parts[3]);
                                    box.Ref = line_parts[4];
                                    box.Client_Id = line_parts[5];
                                    excute_Box(box);
                                    //////////////////////////////////////////////////////////////////////////
                                    Box_DNA box_dna = new Box_DNA();        // Insert The DNA Box information
                                    box_dna.Box_Id = box.Box_Id;
                                    box_dna.DNA_Id = dna.DNA_Id;
                                    excute_Box_DNA(box_dna);
                                    //////////////////////////////////////////////////////////////////////////
                                    Individual indv = new Individual();     // Insert The Individual information
                                    indv.Indv_Id = line_parts[17];
                                    indv.Gender = line_parts[10];
                                    indv.Age = null;
                                    excute_Individual(indv);
                                    //////////////////////////////////////////////////////////////////////////
                                    Individual_Sample indv_sample = new Individual_Sample();        // Insert The Indvidual sample information
                                    indv_sample.Indv_Id = line_parts[17];
                                    indv_sample.Sample_Id = line_parts[8];
                                    excute_Individual_Sample(indv_sample);
                                    //////////////////////////////////////////////////////////////////////////
                                //}       //end of If (Indvidual doesn't exist so we have to insert it)
                                //else
                                //{   // else the animal have already be added before
                                //    if (!Have_Sample(line_parts[17]))   // The animal doesn't have a sample so we will add a sample
                                //    {
                                //        DNA dna = new DNA();
                                //        dna.DNA_Id = line_parts[0];
                                //        dna.DNA_Conc = line_parts[6];
                                //        excute_DNA(dna);
                                //        Sample sample = new Sample();
                                //        sample.DNA_Id = line_parts[0];
                                //        sample.Call_Rate = float.Parse(line_parts[9]);
                                //        sample.Index = int.Parse(line_parts[7]);
                                //        sample.Sample_Id = line_parts[8];
                                //        sample.Sentrix_Id = line_parts[11];
                                //        sample.Sentrix_Position = line_parts[12];
                                //        sample.Imaging_Date = line_parts[13];
                                //        sample.Scanner_Id = line_parts[14];
                                //        sample.Software_Version = line_parts[15];
                                //        sample.User = line_parts[16];
                                //        excute_Sample(sample);
                                //        //////////////////////////////////////////////////////////////////////////
                                //        Box box = new Box();
                                //        box.Box_Id = line_parts[1];
                                //        box.Box_Row = line_parts[2];
                                //        box.Box_Col = int.Parse(line_parts[3]);
                                //        box.Ref = line_parts[4];
                                //        box.Client_Id = line_parts[5];
                                //        excute_Box(box);
                                //        //////////////////////////////////////////////////////////////////////////
                                //        Box_DNA box_dna = new Box_DNA();
                                //        box_dna.Box_Id = box.Box_Id;
                                //        box_dna.DNA_Id = dna.DNA_Id;
                                //        excute_Box_DNA(box_dna);
                                //        //////////////////////////////////////////////////////////////////////////
                                //        Individual_Sample indv_sample = new Individual_Sample();
                                //        indv_sample.Indv_Id = line_parts[17];
                                //        indv_sample.Sample_Id = line_parts[8];
                                //        excute_Individual_Sample(indv_sample);
                                //        //////////////////////////////////////////////////////////////////////////
                                //    }// end of if
                                //} // end of the else

                            
                        }       // in order to skip the first row that contains the coulmn names 
                        count++;
                    }       // Read the next row in the file
                }       // end of the file 

                line = "";
                count = 0;
                //using (StreamReader file = new StreamReader("C:/Users/Elansary/Documents/Visual Studio 2010/Projects/Porc/SNP Table.txt"))
                //{//
                //    while ((line = file.ReadLine()) != null)
                //    {
                //        if (count != 0)
                //        {
                //            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                //            Marker_Loc marker = new Marker_Loc();
                //            marker.Row_Index = line_parts[0];
                //            marker.Marker_Id = line_parts[1];
                //            marker.Chr = line_parts[2];

                //            if (string.Compare(marker.Chr, "X", true) == 0)
                //            {
                //                marker.Autosomal = 0;
                //            }
                //            else
                //            {
                //                marker.Autosomal = 1;
                //            }

                //            marker.Position = int.Parse(line_parts[3]);
                //            marker.Allele1 = line_parts[4].Substring(1, 1);
                //            marker.Allele2 = line_parts[4].Substring(3, 1);
                //            marker.Ref_Name = "";
                //            marker.Bead_Type_A = line_parts[7];
                //            marker.Bead_Type_B = line_parts[10];
                //            marker.GenTrain_Score = float.Parse(line_parts[8]);
                //            marker.Manifest = line_parts[9];
                //            excute_Marker_Loc(marker);
                //        }
                //        count++;
                //    }

                //}

                int[] start_arr = new int[144];
                string[] Sample_arr = new string[144];
                start_arr[0] = 2;
                for (int i = 0; i < 143; i++)
                {
                    start_arr[i + 1] = start_arr[i] + 7;
                }

                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 30000;
                MySqlDataReader Reader;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Full Data Table.txt"))
                {//
                    string marker_print = "";
                    while ((line = file.ReadLine()) != null)
                    {
                        if (count == 0)
                        {
                            for (int i = 0; i <= 143; i++)
                            {
                                string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                string[] Sample_Name_parts = line_parts[start_arr[i]].Split(delimiters2, StringSplitOptions.RemoveEmptyEntries);
                                Sample_arr[i] = Sample_Name_parts[0];
                            }
                        }
                        if (count >= 1)
                        {
                            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            marker_print = line_parts[1];
                            ArrayList snp_list = new ArrayList();
                            for (int i = 0; i <= 143; i++)
                            {
                                SNP snp = new SNP();
                                //snp.Sample_Id = Sample_Lookup(Sample_arr[i]);
                                snp.Sample_Id = Sample_arr[i];
                                snp.Row_Index = int.Parse(line_parts[0]);
                                snp.Marker_Id = line_parts[1];
                                snp.Genotype = line_parts[start_arr[i]];
                                snp.Call_Rate = float.Parse(line_parts[start_arr[i] + 1]);
                                if (float.IsNaN(snp.Call_Rate))
                                {
                                    snp.Call_Rate = 0;
                                }
                                snp.Theta = float.Parse(line_parts[start_arr[i] + 2]);
                                if (float.IsNaN(snp.Theta))
                                {
                                    snp.Theta = 0;
                                }
                                snp.R = float.Parse(line_parts[start_arr[i] + 3]);
                                if (float.IsNaN(snp.R))
                                {
                                    snp.R = 0;
                                }
                                snp.X = float.Parse(line_parts[start_arr[i] + 4]);
                                if (float.IsNaN(snp.X))
                                {
                                    snp.X = 0;
                                }
                                snp.Y = float.Parse(line_parts[start_arr[i] + 5]);
                                if (float.IsNaN(snp.Y))
                                {
                                    snp.Y = 0;
                                }
                                snp.Top_Allele_A = line_parts[start_arr[i] + 6].Substring(0, 1);
                                snp.Top_Allele_B = line_parts[start_arr[i] + 6].Substring(1, 1);
                                snp.Manifest = null;
                                snp.Selected = 1;
                                snp_list.Add(snp);
                            }
                            MySqlTransaction myTrans = conn.BeginTransaction(); ;
                            var iCounter = 0;
                            foreach (SNP item in snp_list)
                            {
                                Boolean Found = false;
                                command.CommandText = "select * from db_proc.tblSNP where Marker_Id = '" + item.Marker_Id + "' AND Sample_Id = '" + item.Sample_Id + "'";
                                Reader = command.ExecuteReader();
                                while (Reader.Read())
                                {
                                    Found = true;
                                }
                                Reader.Close();
                                if (Found == false)
                                {
                                    excute_SNP(item);
                                }


                                iCounter++;
                                if (iCounter >= 143)
                                {
                                    myTrans.Commit();
                                    iCounter = 0;
                                }
                            }
                            System.Console.WriteLine("Line number " + count.ToString() + " for marker " + marker_print.ToString());

                        }

                        count++;
                        conn.Close();
                        conn.Open();
                    }
                    conn.Close();
                }

            }

            public static void Parent_offspring_test(string Indv1, string Indv2, ArrayList Data_Set1, ArrayList Data_Set2, System.IO.StreamWriter Indv_File)
            {
                int Conflict = 0;
                double Conflict_Ratio = 0;
                int markers = 48137;
                int Missing_Genotype = 0;
                char[] delimiters = new char[] { '\t' };
                for (int i = 0; i < markers; i++)
                {
                    if (Data_Set1[i].ToString().CompareTo("NC") != 0 || Data_Set2[i].ToString().CompareTo("NC") != 0)
                    {
                        if (Data_Set1[i].ToString().CompareTo("AA") == 0 & Data_Set2[i].ToString().CompareTo("BB") == 0 || Data_Set1[i].ToString().CompareTo("BB") == 0 & Data_Set2[i].ToString().CompareTo("AA") == 0)
                        {
                            Conflict++;
                        }
                    }
                    else
                    {
                        Missing_Genotype++;
                    }
                    Conflict_Ratio = (double)Conflict / (markers - Missing_Genotype);
                }
                string text = Indv1 + "\t" + Indv2 + "\t" + Conflict.ToString() + "\t" + (markers - Missing_Genotype).ToString() + "\t" + Conflict_Ratio.ToString();
                Indv_File.WriteLine(text);
            }

            public static void Parent_offspring_test(int index1, int index2, string Indv1, string Indv2, System.IO.StreamWriter Indv_File)
            {
                int Conflict = 0;
                double Conflict_Ratio = 0;
                int markers = 54541;
                int Missing_Genotype = 0;
                char[] delimiters = new char[] { '\t' };
                for (int j = 1; j <= markers; j++)
                {
                    if (Data_Set[index1, j].ToString().CompareTo("NC") != 0 || Data_Set[index2, j].ToString().CompareTo("NC") != 0)
                    {
                        if (Data_Set[index1, j].ToString().CompareTo("AA") == 0 & Data_Set[index2, j].ToString().CompareTo("BB") == 0 || Data_Set[index1, j].ToString().CompareTo("BB") == 0 & Data_Set[index2, j].ToString().CompareTo("AA") == 0)
                        {
                            Conflict++;
                        }
                    }
                    else
                    {
                        Missing_Genotype++;
                    }
                }
                Conflict_Ratio = (double)Conflict / (markers - Missing_Genotype);
                string text = Indv1 + "\t" + Indv2 + "\t" + Conflict.ToString() + "\t" + (markers - Missing_Genotype).ToString() + "\t" + Conflict_Ratio.ToString();
                Indv_File.WriteLine(text);
            }

            public static double Parent_offspring_test(int index1, int index2)
            {
                int Conflict = 0;
                double Conflict_Ratio = 0;
                int markers = 54541;
                int Missing_Genotype = 0;
                char[] delimiters = new char[] { '\t' };
                for (int j = 1; j <= markers; j++)
                {
                    if (Data_Set[index1, j].ToString().CompareTo("NC") != 0 && Data_Set[index2, j].ToString().CompareTo("NC") != 0)
                    {
                        if (Data_Set[index1, j].ToString().CompareTo("AA") == 0 & Data_Set[index2, j].ToString().CompareTo("BB") == 0 || Data_Set[index1, j].ToString().CompareTo("BB") == 0 & Data_Set[index2, j].ToString().CompareTo("AA") == 0)
                        {
                            Conflict++;
                        }
                    }
                    else
                    {
                        Missing_Genotype++;
                    }
                }
                return Conflict_Ratio = (double)Conflict / (markers - Missing_Genotype); 
            }

            public static double Parent_offspring_test(string Indv1, string Indv2)
            {
                int index1 = -1;
                int index2 = -1;
                for (int i = 0; i < 689; i++)
                {
                    if (Indv1.CompareTo(Data_Set[i, 1]) == 0)
                    {
                        index1 = i;
                    }
                    if (Indv2.CompareTo(Data_Set[i, 1]) == 0)
                    {
                        index2 = i;
                    }
                }

                int Conflict = 0;
                double Conflict_Ratio = 0;
                int markers = 48137 + 6 - 1;
                int Missing_Genotype = 0;
                char[] delimiters = new char[] { '\t' };
                for (int j = 6; j < markers; j++)
                {
                    if (Data_Set[index1, j].ToString().CompareTo("NC") != 0 || Data_Set[index2, j].ToString().CompareTo("NC") != 0)
                    {
                        if (Data_Set[index1, j].ToString().CompareTo("AA") == 0 & Data_Set[index2, j].ToString().CompareTo("BB") == 0 || Data_Set[index1, j].ToString().CompareTo("BB") == 0 & Data_Set[index2, j].ToString().CompareTo("AA") == 0)
                        {
                            Conflict++;
                        }
                    }
                    else
                    {
                        Missing_Genotype++;
                    }
                }
                Conflict_Ratio = (double)Conflict / (markers - Missing_Genotype);
                return Conflict_Ratio;
            }

            public static void pair_wise_compare(int index1, int index2, string Indv1, string Indv2, System.IO.StreamWriter Indv_File)
            {
                int Conflict = 0;
                double Conflict_Ratio = 0;
                int markers = 48137 + 6 - 1;
                int Missing_Genotype = 0;
                char[] delimiters = new char[] { '\t' };
                for (int j = 6; j < markers; j++)
                {
                    if (Data_Set[index1, j].ToString().CompareTo("NC") != 0 || Data_Set[index2, j].ToString().CompareTo("NC") != 0)
                    {
                        if (Data_Set[index1, j].ToString().CompareTo("AA") == 0 & Data_Set[index2, j].ToString().CompareTo("BB") == 0 || Data_Set[index1, j].ToString().CompareTo("BB") == 0 & Data_Set[index2, j].ToString().CompareTo("AA") == 0)
                        {
                            Conflict++;
                        }
                    }
                    else
                    {
                        Missing_Genotype++;
                    }
                    Conflict_Ratio = (double)Conflict / (markers - Missing_Genotype);
                }
                string text = Indv1 + "\t" + Indv2 + "\t" + Conflict.ToString() + "\t" + (markers - Missing_Genotype).ToString() + "\t" + Conflict_Ratio.ToString();
                Indv_File.WriteLine(text);
            }

            public static double IBS(int Indv1, int Indv2, string[,] Data_Set)
            {
                int IBS_2 = 0;
                int IBS_1 = 0;
                int IBS_0 = 0;
                double DST = 0;
                int markers = 49404;
                int Missing_Genotype = 0;
                SNP snp1 = new SNP();
                SNP snp2 = new SNP();
                char[] delimiters = new char[] { '\t' };
                string ID1 = "";
                string ID2 = "";
                for (int i = 0; i < markers; i++)
                {
                    string[] line_parts1 = Data_Set[Indv1, i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string[] line_parts2 = Data_Set[Indv2, i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    snp1.Genotype = line_parts1[3];
                    snp2.Genotype = line_parts2[3];
                    ID1 = line_parts1[0].ToString();
                    ID2 = line_parts2[0].ToString();
                    if (snp1.Genotype.CompareTo("NC") != 0 && snp2.Genotype.CompareTo("NC") != 0)
                    {
                        if (snp1.Genotype.CompareTo("AA") == 0 & snp2.Genotype.CompareTo("AA") == 0 ||
                            snp1.Genotype.CompareTo("BB") == 0 & snp2.Genotype.CompareTo("BB") == 0 ||
                            snp1.Genotype.CompareTo("AB") == 0 & snp2.Genotype.CompareTo("AB") == 0)
                        {
                            IBS_2++;
                        }
                        else if (snp1.Genotype.CompareTo("AA") == 0 & snp2.Genotype.CompareTo("AB") == 0 ||
                                snp1.Genotype.CompareTo("AB") == 0 & snp2.Genotype.CompareTo("AA") == 0 ||
                                snp1.Genotype.CompareTo("BB") == 0 & snp2.Genotype.CompareTo("AB") == 0 ||
                                snp1.Genotype.CompareTo("AB") == 0 & snp2.Genotype.CompareTo("BB") == 0)
                        {
                            IBS_1++;
                        }
                        else
                        {
                            IBS_0++;
                        }
                    }
                    else
                    {
                        Missing_Genotype++;
                    }

                }
                DST = (double)(IBS_2 + (0.5 * IBS_1)) / (markers - Missing_Genotype);
                DST = Math.Round(DST * 100, 4);
                return (double)DST / 100;
            }

            public static double IBS2(int Indv1, int Indv2, string[] Data_Set1, string[] Data_Set2)
            {
                int IBS_2 = 0;
                int IBS_1 = 0;
                int IBS_0 = 0;
                double DST = 0;
                int markers = 48137;
                int Missing_Genotype = 0;
                SNP snp1 = new SNP();
                SNP snp2 = new SNP();
                char[] delimiters = new char[] { '\t' };
                string ID1 = "";
                string ID2 = "";
                for (int i = 0; i < markers; i++)
                {
                    string[] line_parts1 = Data_Set1[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string[] line_parts2 = Data_Set2[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    snp1.Genotype = line_parts1[3];
                    snp2.Genotype = line_parts2[3];
                    ID1 = line_parts1[0].ToString();
                    ID2 = line_parts2[0].ToString();
                    if (snp1.Genotype.CompareTo("NC") != 0 && snp2.Genotype.CompareTo("NC") != 0)
                    {
                        if (snp1.Genotype.CompareTo("AA") == 0 & snp2.Genotype.CompareTo("AA") == 0 ||
                            snp1.Genotype.CompareTo("BB") == 0 & snp2.Genotype.CompareTo("BB") == 0 ||
                            snp1.Genotype.CompareTo("AB") == 0 & snp2.Genotype.CompareTo("AB") == 0)
                        {
                            IBS_2++;
                        }
                        else if (snp1.Genotype.CompareTo("AA") == 0 & snp2.Genotype.CompareTo("AB") == 0 ||
                                snp1.Genotype.CompareTo("AB") == 0 & snp2.Genotype.CompareTo("AA") == 0 ||
                                snp1.Genotype.CompareTo("BB") == 0 & snp2.Genotype.CompareTo("AB") == 0 ||
                                snp1.Genotype.CompareTo("AB") == 0 & snp2.Genotype.CompareTo("BB") == 0)
                        {
                            IBS_1++;
                        }
                        else
                        {
                            IBS_0++;
                        }
                    }
                    else
                    {
                        Missing_Genotype++;
                    }

                }
                DST = (double)(IBS_2 + (0.5 * IBS_1)) / (markers - Missing_Genotype);
                DST = Math.Round(DST * 100, 4);
                return (double)DST / 100;
            }

            public static void Marker_Genotype_Frequency(string Marker_Id)
            {

                int AA_count = 0;
                int AB_count = 0;
                int BB_count = 0;
                int count = 0;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblsnp.Marker_Id, tblsnp.Genotype, tblindividual.Founder, tblindividual.Included, tblpedigree.Maternal_Id, tblpedigree.Paternal_Id
                         FROM tblmarker_loc INNER JOIN
                         tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN
                         tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN
                         tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id INNER JOIN
                         tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                         WHERE (tblsnp.Marker_Id = '" + Marker_Id + @"') AND (tblindividual.Included = 1)
                         AND (tblindividual.indv_id in 
                         (select indv_id from tblindividual_phenotype where phenotype_id = 8))";
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    if (Reader.GetValue(4).ToString().CompareTo("General") == 0 && Reader.GetValue(5).ToString().CompareTo("General") == 0)
                    {

                        if (Reader.GetValue(1).ToString().CompareTo("NC") != 0)
                        {
                            count++;
                            if (Reader.GetValue(1).ToString().CompareTo("AA") == 0)
                            {
                                AA_count++;
                            }
                            else if (Reader.GetValue(1).ToString().CompareTo("BB") == 0)
                            {
                                BB_count++;
                            }
                            else
                            {
                                AB_count++;
                            }
                        }



                    }
                    else
                    {
                        if (sample_exists(Reader.GetValue(4).ToString()) == 0 && sample_exists(Reader.GetValue(5).ToString()) == 0)
                        {
                            if (Reader.GetValue(1).ToString().CompareTo("NC") != 0)
                            {
                                count++;
                                if (Reader.GetValue(1).ToString().CompareTo("AA") == 0)
                                {
                                    AA_count++;
                                }
                                else if (Reader.GetValue(1).ToString().CompareTo("BB") == 0)
                                {
                                    BB_count++;
                                }
                                else
                                {
                                    AB_count++;
                                }
                            }

                        }
                    }

                }
                conn.Close();
                Reader.Close();
                double AA_Frequency;
                double BB_Frequency;
                double AB_Frequency;
                double Allele_A_Frequency;
                double Allele_B_Frequency;
                double AA_Expected;
                double BB_Expected;
                double AB_Expected;
                double chi2;
                double P_Value;
                if (AA_count > 0 || AB_count > 0 || BB_count > 0)
                {
                    AA_Frequency = (double)AA_count / count;
                    BB_Frequency = (double)BB_count / count;
                    AB_Frequency = (double)AB_count / count;
                    Allele_A_Frequency = AA_Frequency + (0.5 * AB_Frequency);
                    Allele_B_Frequency = 1 - Allele_A_Frequency;
                    AA_Expected = Math.Pow(Allele_A_Frequency, 2) * count;
                    BB_Expected = Math.Pow(Allele_B_Frequency, 2) * count;
                    AB_Expected = 2 * Allele_A_Frequency * Allele_B_Frequency * count;
                    chi2 = (double)Math.Pow((AA_count - AA_Expected), 2) / AA_Expected + (double)Math.Pow((BB_count - BB_Expected), 2) / BB_Expected + (double)Math.Pow((AB_count - AB_Expected), 2) / AB_Expected;
                    P_Value = chisqr(1, chi2);
                }
                else
                {
                    AA_Frequency = 0;
                    BB_Frequency = 0;
                    AB_Frequency = 0;
                    Allele_A_Frequency = 0;
                    Allele_B_Frequency = 0;
                    P_Value = 0;
                }


                Marker_Loc Marker = new Marker_Loc();
                Marker.Marker_Id = Marker_Id;
                Marker.A_Freq = Allele_A_Frequency;
                Marker.B_Freq = Allele_B_Frequency;
                Marker.AA_Freq = AA_Frequency;
                Marker.BB_Freq = BB_Frequency;
                Marker.AB_Freq = AB_Frequency;
                Marker.Chi2_P_Value = P_Value;
                Update_Marker(Marker);

            }

            public static void Marker_Genotype_Frequency_Faster(string Marker_Id)
            {

                int AA_count = 0;
                int AB_count = 0;
                int BB_count = 0;
                int NC_count = 0;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblsnp.Genotype, count(*)
                         FROM tblmarker_loc INNER JOIN
                         tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN
                         tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN
                         tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id INNER JOIN
                         tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                         WHERE (tblsnp.Marker_Id = '" + Marker_Id + @"') AND (tblindividual.Included = 1)
                         AND (tblindividual.indv_id in 
                         (select indv_id from tblindividual where call_rate is not null and included = 1 and founder = 1))
                         group by genotype";

                //(select indv_id from tblindividual_phenotype where phenotype_id = 8)) control animals
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    switch (Reader.GetValue(0).ToString())
                    {
                        case "NC":
                            NC_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "AA":
                            AA_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "BB":
                            BB_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "AB":
                            AB_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                    }
                }
                conn.Close();
                Reader.Close();
                double AA_Frequency;
                double BB_Frequency;
                double AB_Frequency;
                double Allele_A_Frequency;
                double Allele_B_Frequency;
                double AA_Expected;
                double BB_Expected;
                double AB_Expected;
                double chi2;
                double P_Value;
                if (AA_count > 0 || AB_count > 0 || BB_count > 0)
                {
                    AA_Frequency = (double)AA_count / ((AA_count + AB_count + BB_count) - NC_count);
                    BB_Frequency = (double)BB_count / ((AA_count + AB_count + BB_count) - NC_count);
                    AB_Frequency = (double)AB_count / ((AA_count + AB_count + BB_count) - NC_count);
                    Allele_A_Frequency = AA_Frequency + (0.5 * AB_Frequency);
                    Allele_B_Frequency = 1 - Allele_A_Frequency;
                    AA_Expected = Math.Pow(Allele_A_Frequency, 2) * ((AA_count + AB_count + BB_count) - NC_count);
                    BB_Expected = Math.Pow(Allele_B_Frequency, 2) * ((AA_count + AB_count + BB_count) - NC_count);
                    AB_Expected = 2 * Allele_A_Frequency * Allele_B_Frequency * ((AA_count + AB_count + BB_count) - NC_count);
                    chi2 = (double)Math.Pow((AA_count - AA_Expected), 2) / AA_Expected + (double)Math.Pow((BB_count - BB_Expected), 2) / BB_Expected + (double)Math.Pow((AB_count - AB_Expected), 2) / AB_Expected;
                    P_Value = chisqr(1, chi2);
                }
                else
                {
                    AA_Frequency = 0;
                    BB_Frequency = 0;
                    AB_Frequency = 0;
                    Allele_A_Frequency = 0;
                    Allele_B_Frequency = 0;
                    P_Value = 0;
                }


                Marker_Loc Marker = new Marker_Loc();
                Marker.Marker_Id = Marker_Id;
                Marker.A_Freq = Allele_A_Frequency;
                Marker.B_Freq = Allele_B_Frequency;
                Marker.AA_Freq = AA_Frequency;
                Marker.BB_Freq = BB_Frequency;
                Marker.AB_Freq = AB_Frequency;
                Marker.Chi2_P_Value = P_Value;
                Update_Marker(Marker);

            }

            public static void Marker_Genotype_Frequency_File(int Marker_Id, string Marker_Name,int line_number,string Line )
            {
                int AA_count = 0;
                int AB_count = 0;
                int BB_count = 0;
                int NC_count = 0;
                string field = "";
                switch (Line)
                {
                    case "23":
                        field = "P23";
                        break;
                   case "36":
                        field = "P36";
                        break;
                   case "12":
                        field = "P12";
                        break;
                   case "33":
                        field = "P33";
                        break;
                   case "15":
                        field = "P15";
                        break;
                }
                for (int i = 0; i <= line_number-1; i++)
                {
                    switch (Data_Set_Founders[i, Marker_Id])
                    {
                        case "NC":
                            NC_count++;
                            break;
                        case "AA":
                            AA_count++;
                            break;
                        case "BB":
                            BB_count++;
                            break;
                        case "AB":
                            AB_count++;
                            break;
                    }
                }

                if (AA_count > 0 || AB_count > 0 || BB_count > 0)
                {
                    Marker_Loc Marker = new Marker_Loc();
                    Marker.Marker_Id = Marker_Name;
                    Marker.Exact_Test = SNPHWE(AB_count, AA_count, BB_count);
                    Marker.AA_Freq = (double)AA_count / ((AA_count + AB_count + BB_count) );
                    Marker.BB_Freq = (double)BB_count / ((AA_count + AB_count + BB_count) );
                    Marker.AB_Freq = (double)AB_count / ((AA_count + AB_count + BB_count) );
                    Marker.A_Freq = Marker.AA_Freq + (0.5 * Marker.AB_Freq);
                    Marker.B_Freq = 1 - Marker.A_Freq;
                    Marker.AB_count = AB_count;
                    Marker.AA_count = AA_count;
                    Marker.BB_count = BB_count;
                    Marker.NC_count = NC_count;
                    Update_Exact_Test(Marker,field);
                }
                else
                {
                    Marker_Loc Marker = new Marker_Loc();
                    Marker.Marker_Id = Marker_Name;
                    Marker.Exact_Test = 10;
                    Marker.AB_count = AB_count;
                    Marker.AA_count = AA_count;
                    Marker.BB_count = BB_count;
                    Marker.NC_count = NC_count;
                    Update_Exact_Test(Marker,field);
                    System.Console.WriteLine("Marker ID " + Marker_Id.ToString() + " ALL AA,AB,BB have Zero Cound ");
                }
            }

            public static void Marker_Genotype_Female(string Marker_Id)
            {

                int AA_count = 0;
                int AB_count = 0;
                int BB_count = 0;
                int NC_count = 0;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblsnp.Genotype, count(*)
                         FROM tblmarker_loc INNER JOIN
                         tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN
                         tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN
                         tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id INNER JOIN
                         tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                         WHERE (tblsnp.Marker_Id = '" + Marker_Id + @"') AND (tblindividual.Included = 1)
                         AND (tblindividual.indv_id in 
                         (SELECT tblindividual.Indv_Id FROM tblindividual INNER JOIN tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id
                         WHERE (tblindividual.Call_Rate IS NOT NULL) AND (tblindividual.Included = 1) 
                         AND (tblindividual.Founder = 1) and (tblindividual.Gender = 'Female') and (tblindividual_phenotype.phenotype_id <> 2) ))
                         group by genotype";

                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    switch (Reader.GetValue(0).ToString())
                    {
                        case "NC":
                            NC_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "AA":
                            AA_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "BB":
                            BB_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "AB":
                            AB_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                    }
                }
                conn.Close();
                Reader.Close();
                double AA_Frequency;
                double BB_Frequency;
                double AB_Frequency;
                double Allele_A_Frequency;
                double Allele_B_Frequency;
                double AA_Expected;
                double BB_Expected;
                double AB_Expected;
                double chi2;
                double P_Value;
                if (AA_count > 0 || AB_count > 0 || BB_count > 0)
                {
                    AA_Frequency = (double)AA_count / ((AA_count + AB_count + BB_count) - NC_count);
                    BB_Frequency = (double)BB_count / ((AA_count + AB_count + BB_count) - NC_count);
                    AB_Frequency = (double)AB_count / ((AA_count + AB_count + BB_count) - NC_count);
                    Allele_A_Frequency = AA_Frequency + (0.5 * AB_Frequency);
                    Allele_B_Frequency = 1 - Allele_A_Frequency;
                    AA_Expected = Math.Pow(Allele_A_Frequency, 2) * ((AA_count + AB_count + BB_count) - NC_count);
                    BB_Expected = Math.Pow(Allele_B_Frequency, 2) * ((AA_count + AB_count + BB_count) - NC_count);
                    AB_Expected = 2 * Allele_A_Frequency * Allele_B_Frequency * ((AA_count + AB_count + BB_count) - NC_count);
                    chi2 = (double)Math.Pow((AA_count - AA_Expected), 2) / AA_Expected + (double)Math.Pow((BB_count - BB_Expected), 2) / BB_Expected + (double)Math.Pow((AB_count - AB_Expected), 2) / AB_Expected;
                    P_Value = chisqr(1, chi2);
                }
                else
                {
                    AA_Frequency = 0;
                    BB_Frequency = 0;
                    AB_Frequency = 0;
                    Allele_A_Frequency = 0;
                    Allele_B_Frequency = 0;
                    P_Value = 0;
                }


                Marker_Loc Marker = new Marker_Loc();
                Marker.Marker_Id = Marker_Id;
                Marker.A_Freq = Allele_A_Frequency;
                Marker.B_Freq = Allele_B_Frequency;
                Marker.AA_Freq = AA_Frequency;
                Marker.BB_Freq = BB_Frequency;
                Marker.AB_Freq = AB_Frequency;
                Marker.Chi2_P_Value = P_Value;
                Marker.Exact_Test = SNPHWE(AB_count, AA_count, BB_count);
                Update_Marker(Marker);
                //Update_Exact_Test(Marker);

            }

            public static int total_markers_x()
            {
                string ConString = "SERVER=localhost;" +
                            "DATABASE=db_Proc;" +
                            "UID=root;" +
                            "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 3000000;
                command.CommandText = @"select count(*) from tblmarker_loc where included = 1 and chr = 'X'";
                Reader = command.ExecuteReader();
                int total_markers = 0;
                while (Reader.Read())
                {
                    total_markers = int.Parse(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                conn.Open();
                return total_markers;
            }

            public static int total_markers()
            {
                string ConString = "SERVER=localhost;" +
                    "DATABASE=db_Proc;" +
                    "UID=root;" +
                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 3000000;
                command.CommandText = @"select count(*) from tblmarker_loc where included = 1";
                Reader = command.ExecuteReader();
                int total_markers = 0;
                while (Reader.Read())
                {
                    total_markers = int.Parse(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                conn.Open();
                return total_markers;
            }

            public static double sex_prediction(string indv)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual_sample.Indv_Id, tblsnp.Genotype, COUNT(*) AS Expr1
                                    FROM tblsnp INNER JOIN
                                    tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN
                                    tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                    WHERE (tblindividual_sample.Indv_Id = '" + indv + @"') AND (tblmarker_loc.Chr = 'X') AND (tblmarker_loc.Included = 1)
                                    GROUP BY tblsnp.Genotype
                                    ORDER BY tblsnp.Genotype";
                Reader = command.ExecuteReader();
                int total_markers = total_markers_x();
                int missing_markers = 0;
                double percentage = 0;
                int hetero = 0;
                SNP snp = new SNP();
                while (Reader.Read())
                {
                    switch (Reader.GetValue(1).ToString())
                    {
                        case "AB":
                            hetero = int.Parse(Reader.GetValue(2).ToString());
                            break;
                        case "NC":
                            missing_markers = int.Parse(Reader.GetValue(2).ToString());
                            break;
                    }
                }
                percentage = (double)hetero / (total_markers - missing_markers);
                Reader.Close();
                conn.Close();
                return percentage;
            }


            public static double sex_prediction_file(int indv_index)
            {
                int total_markers = total_markers_x() - 70;
                int Start = 54542 + 70 ;
                int missing_markers = 0;
                double percentage = 0;
                int hetero = 0;
                for (int j = Start; j < Data_Set.GetLength(1); j++)
                {
                    switch (Data_Set[indv_index, j])
                    {
                        case "AB":
                            hetero++;
                            break;
                        case "NC":
                            missing_markers++;
                            break;
                    }
                }
                percentage = (double)hetero / (total_markers - missing_markers);
                return percentage;
            }

            public static void Poseudoautosomal()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = "select Marker_Id from tblmarker_loc where chr = 'X' and included = 1 order by Position,marker_id asc";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Marker_List.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                System.IO.StreamWriter output_file_Male = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Male_modified.txt");
                System.IO.StreamWriter output_file_Female = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Female_modified.txt");
                for (int i = 0; i < Marker_List.Count; i++)
                {
                    System.Console.WriteLine("Inserted " + i);
                    conn.Open();
                    command.CommandText = @"SELECT tblsnp.Marker_Id, tblsnp.Genotype, tblindividual.Gender, COUNT(tblsnp.Genotype) AS Expr1
                                    FROM tblsnp INNER JOIN
                                    tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN
                                    tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id
                                    WHERE (tblsnp.Marker_Id = '" + Marker_List[i].ToString() + @"') AND (tblindividual.Included = 1)
                                    GROUP BY tblsnp.Genotype, tblindividual.Gender
                                    ORDER BY tblindividual.Gender, tblsnp.Genotype";

                    Reader = command.ExecuteReader();
                    int M_AA = 0;
                    int M_AB = 0;
                    int M_BB = 0;
                    int M_NC = 0;
                    int F_AA = 0;
                    int F_AB = 0;
                    int F_BB = 0;
                    int F_NC = 0;

                    while (Reader.Read())
                    {
                        if (Reader.GetValue(2).ToString().CompareTo("Male") == 0)
                        {
                            switch (Reader.GetValue(1).ToString())
                            {
                                case "AB":
                                    M_AB = int.Parse(Reader.GetValue(3).ToString());
                                    break;
                                case "AA":
                                    M_AA = int.Parse(Reader.GetValue(3).ToString());
                                    break;
                                case "BB":
                                    M_BB = int.Parse(Reader.GetValue(3).ToString());
                                    break;
                                case "NC":
                                    M_NC = int.Parse(Reader.GetValue(3).ToString());
                                    break;
                            }
                        }
                        else
                        {
                            switch (Reader.GetValue(1).ToString())
                            {
                                case "AB":
                                    F_AB = int.Parse(Reader.GetValue(3).ToString());
                                    break;
                                case "AA":
                                    F_AA = int.Parse(Reader.GetValue(3).ToString());
                                    break;
                                case "BB":
                                    F_BB = int.Parse(Reader.GetValue(3).ToString());
                                    break;
                                case "NC":
                                    F_NC = int.Parse(Reader.GetValue(3).ToString());
                                    break;
                            }
                        }
                    }
                    double M_perc = (double)M_AB / (M_AB + M_AA + M_BB);
                    double F_perc = (double)F_AB / (F_AB + F_AA + F_BB);
                    string Male_Line = Marker_List[i].ToString() + " " + M_perc.ToString();
                    string Female_Line = Marker_List[i].ToString() + " " + F_perc.ToString();
                    output_file_Male.WriteLine(Male_Line);
                    output_file_Female.WriteLine(Female_Line);
                    Reader.Close();
                    conn.Close();
                }
                output_file_Male.Flush();
                output_file_Male.Close();
                output_file_Female.Flush();
                output_file_Female.Close();

            }

            public static int sex_prob(string indv)
            {
                int problem = 0;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = "select Gender_modified from tblindividual where indv_id = '" + indv + "'";
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    problem = int.Parse(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                return problem;
            }

            public static void Poseudoautosomal_file()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = "select Marker_Id from tblmarker_loc where chr = 'X' and included = 1 order by Position,Marker_Id";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Marker_List.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();

                char[] delimiters = new char[] { '\t' };
                ArrayList info = new ArrayList();
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        if (sex_prob(line_parts[0]) == 0)
                        {
                            for (int j = 0; j < line_parts.Length; j++)
                            {
                                Data_Set[i, j] = line_parts[j];
                            }
                            i++;
                        }
                        else
                        {
                            System.Console.WriteLine("Sex problem " + Data_Set[i, 0]);
                        }
                        
                    }
                }

                System.IO.StreamWriter output_file_Male = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Male_modified.txt");
                System.IO.StreamWriter output_file_Female = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Female_modified.txt");
                int marker_index = 54542;
                int end = i;
                for (int k = 0; k < Marker_List.Count; k++)
                {
                    System.Console.WriteLine("Inserted " + k);
                    int M_AA = 0;
                    int M_AB = 0;
                    int M_BB = 0;
                    int M_NC = 0;
                    int F_AA = 0;
                    int F_AB = 0;
                    int F_BB = 0;
                    int F_NC = 0;

                    for (i = 0; i < end; i++) // 12 animals with sex_problems 
                    {
                        if (check_sex(Data_Set[i,0]).CompareTo("Male") == 0) //Male
                        {

                            switch (Data_Set[i, marker_index])
                            {
                                case "AB":
                                    M_AB++;
                                    break;
                                case "AA":
                                    M_AA++;
                                    break;
                                case "BB":
                                    M_BB++;
                                    break;
                                case "NC":
                                    M_NC++;
                                    break;
                            }
                        }
                        else
                        {
                            switch (Data_Set[i, marker_index])
                            {
                                case "AB":
                                    F_AB++;
                                    break;
                                case "AA":
                                    F_AA++;
                                    break;
                                case "BB":
                                    F_BB++;
                                    break;
                                case "NC":
                                    F_NC++;
                                    break;
                            }
                        }
                    }

                    double M_perc = (double)M_AB / (M_AB + M_AA + M_BB);
                    double F_perc = (double)F_AB / (F_AB + F_AA + F_BB);
                    string Male_Line = Marker_List[k].ToString() + " " + M_perc.ToString();
                    string Female_Line = Marker_List[k].ToString() + " " + F_perc.ToString();
                    output_file_Male.WriteLine(Male_Line);
                    output_file_Female.WriteLine(Female_Line);
                    marker_index++;
                }
                output_file_Male.Flush();
                output_file_Male.Close();
                output_file_Female.Flush();
                output_file_Female.Close();

            }

            public static void exc_mark_my_formate()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = "select Marker_Id from tblmarker_loc where included = 0 order by 0 + chr,Position asc";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Marker_List.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();

                string map_line = "";
                ArrayList skip = new ArrayList();
                int counter = 7;
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\Plink RG disease\porc.map"))
                {//
                    while ((map_line = file.ReadLine()) != null)
                    {
                        string[] line_parts = map_line.Split(' ');
                        for (int i = 0; i < Marker_List.Count; i++)
                        {
                            if (line_parts[1].CompareTo(Marker_List[i].ToString()) == 0)
                            {
                                skip.Add(counter);
                                break;
                            }
                        }
                        counter++;
                    }
                }

                char[] delimiters = new char[] { '\t' };

                string line = "";
                bool found = false;
                System.IO.StreamWriter output_file_my = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Pseudo_Region\My.ped");
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\My.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        StringBuilder text = new StringBuilder("");
                        text.Clear();
                        for (int i = 7; i < line_parts.Length; i++)
                        {
                            foreach (int skip_index in skip)
                            {
                                if (i == skip_index)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                            {

                            }
                            else
                            {
                                text.Append(text + " " + line_parts[i].ToString());
                            }

                        }
                        output_file_my.WriteLine(text);

                    }
                }




            }



            public static double SNPHWE(int obs_hets, int obs_hom1, int obs_hom2)
            {
                if (obs_hom1 < 0 || obs_hom2 < 0 || obs_hets < 0)
                {
                    System.Console.WriteLine("FATAL ERROR - SNP-HWE: Current genotype configuration (" + obs_hets + obs_hom1 + obs_hom2 + ") includes a negative count");

                }

                int obs_homc = obs_hom1 < obs_hom2 ? obs_hom2 : obs_hom1;
                int obs_homr = obs_hom1 < obs_hom2 ? obs_hom1 : obs_hom2;

                int rare_copies = 2 * obs_homr + obs_hets;
                int genotypes = obs_hets + obs_homc + obs_homr;

                double[] het_probs = new double[rare_copies + 1];

                int i;
                for (i = 0; i <= rare_copies; i++)
                    het_probs[i] = 0.0F;

                /* start at midpoint */
                double x = (double)(rare_copies * (2 * genotypes - rare_copies)) / (double)(2 * genotypes);
                int mid = int.Parse(Math.Floor(x).ToString());

                /* check to ensure that midpoint and rare alleles have same parity */
                if ((rare_copies % 2) != (mid % 2))
                    mid++;

                int curr_hets = mid;
                int curr_homr = (rare_copies - mid) / 2;
                int curr_homc = genotypes - curr_hets - curr_homr;

                het_probs[mid] = 1.0F;
                double sum = het_probs[mid];
                for (curr_hets = mid; curr_hets > 1; curr_hets -= 2)
                {
                    het_probs[curr_hets - 2] = (double)(het_probs[curr_hets] * curr_hets * (curr_hets - 1.0))
                                             / (double)(4 * (curr_homr + 1) * (curr_homc + 1));
                    sum += het_probs[curr_hets - 2];

                    /* 2 fewer heterozygotes for next iteration -> add one rare, one common homozygote */
                    curr_homr++;
                    curr_homc++;
                }

                curr_hets = mid;
                curr_homr = (rare_copies - mid) / 2;
                curr_homc = genotypes - curr_hets - curr_homr;
                for (curr_hets = mid; curr_hets <= rare_copies - 2; curr_hets += 2)
                {
                    het_probs[curr_hets + 2] = (double)(het_probs[curr_hets] * 4.0 * curr_homr * curr_homc)
                                          / (double)((curr_hets + 2) * (curr_hets + 1));
                    sum += het_probs[curr_hets + 2];

                    /* add 2 heterozygotes for next iteration -> subtract one rare, one common homozygote */
                    curr_homr--;
                    curr_homc--;
                }

                for (i = 0; i <= rare_copies; i++)
                    het_probs[i] /= sum;

                /* alternate p-value calculation for p_hi/p_lo
                double p_hi = het_probs[obs_hets];
                for (i = obs_hets + 1; i <= rare_copies; i++)
                  p_hi += het_probs[i];
   
                double p_lo = het_probs[obs_hets];
                for (i = obs_hets - 1; i >= 0; i--)
                   p_lo += het_probs[i];

   
                double p_hi_lo = p_hi < p_lo ? 2.0 * p_hi : 2.0 * p_lo;
                */

                double p_hwe = 0.0F;
                /*  p-value calculation for p_hwe  */
                for (i = 0; i <= rare_copies; i++)
                {
                    if ((double)het_probs[i] > (double)het_probs[obs_hets])
                        continue;
                    p_hwe += het_probs[i];
                }

                p_hwe = p_hwe > 1.0F ? 1.0F : p_hwe;


                return p_hwe;
            }

            public static void Update_Individual(Individual indv)
            {

                string ConString = "SERVER=localhost;" +
              "DATABASE=db_Proc;" +
              "UID=root;" +
              "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "Update db_proc.tblIndividual set Birth_Date='" + indv.Birth_Date + "' , Line =" + indv.Line + " , Location = '" + indv.Location + "' Where Indv_Id='" + indv.Indv_Id + "';";
                Reader = command.ExecuteReader();
                Reader.Close();
                conn.Close();

            }

            public static void Update_Individual_id(string indv,string new_indv)
            {

                string ConString = "SERVER=localhost;" +
              "DATABASE=db_Proc;" +
              "UID=root;" +
              "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "Update db_proc.tblIndividual set Indv_id='" + new_indv + "' where indv_id ='" + indv + "'";
                Reader = command.ExecuteReader();
                Reader.Close();
                conn.Close();

            }

            public static void Update_Individual_line(string indv,string line)
            {

                string ConString = "SERVER=localhost;" +
              "DATABASE=db_Proc;" +
              "UID=root;" +
              "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "Update db_proc.tblIndividual set line='" + line + "' where indv_id ='" + indv + "'";
                Reader = command.ExecuteReader();
                Reader.Close();
                conn.Close();

            }

            public static void Update_Individual_Birthdate(Individual indv)
            {

                string ConString = "SERVER=localhost;" +
              "DATABASE=db_Proc;" +
              "UID=root;" +
              "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "Update db_proc.tblIndividual set Birth_Date='" + indv.Birth_Date + "' Where Indv_Id='" + indv.Indv_Id + "';";
                Reader = command.ExecuteReader();
                Reader.Close();
                conn.Close();

            }

            public static void Update_Pedigree(Pedigree ped)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "Update db_proc.tblpedigree set Maternal_Id_Modified='" + ped.Maternal_Id_Modified + "' , Paternal_Id_Modified ='" + ped.Paternal_Id_Modified + "' , Modified = 1 Where Indv_Id='" + ped.Indv_Id + "';";
                Reader = command.ExecuteReader();
                Reader.Close();
                conn.Close();
            }

            public static void Pedigree_update_con(string Gender, string F_M,string indv_id)
            {
                string ConString = "SERVER=localhost;" +
                                "DATABASE=db_Proc;" +
                                "UID=root;" +
                                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                string query = "";
                string line = "";
                string new_id ="";
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\PIGENDEF\found.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        if (F_M.CompareTo(line_parts[0]) == 0)
                        {
                            new_id = line_parts[1];
                            break;
                        }
                    }
                }

                if (Gender.CompareTo("Male") == 0)
                {
                    query = "Update db_proc.tblpedigree set Paternal_Id='" + new_id + "' Where Indv_Id='" + indv_id + "';";
                }
                else
                {
                    query = "Update db_proc.tblpedigree set Maternal_Id='" + new_id + "' Where Indv_Id='" + indv_id + "';";
                }
                command.CommandText = query;
                command.ExecuteNonQuery();
                conn.Close();
            }

            public static void Pedigree_update(string indv_id)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "select Maternal_Id,Paternal_ID from tblpedigree where indv_id ='" + indv_id + "'";
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    if (Reader.GetValue(0).ToString().Substring(0, 2).CompareTo("NS") == 0)
                    {
                        Pedigree_update_con("Female", Reader.GetValue(0).ToString(),indv_id);
                    }
                    if (Reader.GetValue(1).ToString().Substring(0, 2).CompareTo("NS") == 0)
                    {
                        Pedigree_update_con("Male", Reader.GetValue(1).ToString(),indv_id);
                    }
                }
                Reader.Close();
                conn.Close();
            }

            public static void Update_original_Pedigree(Pedigree ped)
            {
 

                string ConString = "SERVER=localhost;" +
                                "DATABASE=db_Proc;" +
                                "UID=root;" +
                                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select * from db_proc.tblpedigree where Indv_Id = '" + ped.Indv_Id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                conn.Close();
                if (Found == false)
                {
                    excute_Pedigree(ped);
                }
                else
                {
                    conn.Open();
                    cmd.CommandText = "Update db_proc.tblpedigree set Maternal_Id='" + ped.Maternal_Id + "' , Paternal_Id ='" + ped.Paternal_Id + "' Where Indv_Id='" + ped.Indv_Id + "';";
                    Reader = cmd.ExecuteReader();
                    Reader.Close();
                    conn.Close();
                }


            }

            public static void Update_Marker(Marker_Loc Marker)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblmarker_loc set AA_Freq = " + Marker.AA_Freq + " , AB_Freq = " + Marker.AB_Freq + ", BB_Freq= " + Marker.BB_Freq + " , A_Freq= " + Marker.A_Freq + " , B_Freq=  " + Marker.B_Freq + " , Chi2_P_Value= " + Marker.Chi2_P_Value + " where Marker_Id = '" + Marker.Marker_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                //Reader.Close();
                conn.Close();
            }

            public static void Exclude_Individual(string Indv_Id)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblindividual set Included = 0 where Indv_Id = '" + Indv_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                //Reader.Close();
                conn.Close();
            }

            public static void Include_Individual(string Indv_Id)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblindividual set Included = 1 where Indv_Id = '" + Indv_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                //Reader.Close();
                conn.Close();
            }

            public static void Exclude_Marker(string Marker_Id)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblMarker_Loc set Included = 0 where Marker_Id = '" + Marker_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }

            public static void Include_Marker(string Marker_Id)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblMarker_Loc set Included = 1 where Marker_Id = '" + Marker_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }

            public static void Update_Call_Rate(Individual Indv)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblindividual set Call_Rate = " + Indv.Call_Rate + " where Indv_Id = '" + Indv.Indv_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                //Reader.Close();
                conn.Close();
            }

            public static void Update_Call_Rate_Marker(Marker_Loc Marker)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblMarker_Loc set Call_Rate = " + Marker.Call_Rate + " where Marker_Id = '" + Marker.Marker_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }

            public static void Update_Exact_Test(Marker_Loc Marker,string field)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                //string updateSql = "Update db_proc.tblmarker_loc set Founder_Control = " + Marker.Exact_Test + " , A_freq = " + Marker.A_Freq + " , B_freq = " + Marker.B_Freq + " , AA_freq = " + Marker.AA_Freq + " , BB_freq = " + Marker.BB_Freq + " , AB_freq = " + Marker.AB_Freq + " , AA_count = " + Marker.AA_count + " , BB_count = " + Marker.BB_count + " , AB_count = " + Marker.AB_count + " , NC_count = " + Marker.NC_count +  " where Marker_Id = '" + Marker.Marker_Id + "';";
                string updateSql = "Update db_proc.tblmarker_loc set " + field + " = " + Marker.Exact_Test + " , AA_count = " + Marker.AA_count + " , BB_count = " + Marker.BB_count + " , AB_count = " + Marker.AB_count + " , NC_count = " + Marker.NC_count + " where Marker_Id = '" + Marker.Marker_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.CommandTimeout = 300000;
                command.ExecuteNonQuery();
                conn.Close();
            }

            public static void Update_Exact_Test2(Marker_Loc Marker)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblmarker_loc set Founder_Control = " + Marker.Exact_Test + " where Marker_Id = '" + Marker.Marker_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }

            public static Boolean individual_exists(string Indv_Id)
            {
                Boolean exists = false;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "select Indv_Id from db_proc.tblIndividual where Indv_Id= '" + Indv_Id.ToString() + "';";
                Reader = command.ExecuteReader();
                if (Reader.HasRows)
                {
                    exists = true;
                }
                else
                {
                    exists = false;
                }

                Reader.Close();
                conn.Close();
                return exists;

            }

            public static void check_individuals(ArrayList Indv_list)
            {
                ArrayList Indv_found = new ArrayList();
                ArrayList Indv_not_found = new ArrayList();
                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                DA.Fill(ds, "samples");

                try
                {

                    foreach (DataRow dr in ds.Tables["samples"].Rows)
                    {
                        for (int j = 0; j < Indv_list.Count; j++)
                        {
                            if (Indv_list[j].Equals(dr[0].ToString().Trim()))
                            {
                                Individual indv = new Individual();
                                if (dr[7].ToString().Trim().CompareTo("") != 0)
                                {
                                    DateTime dat = DateTime.Parse(dr[7].ToString().Trim());
                                    string formatForMySql = dat.ToString("yyyy-MM-dd");
                                    indv.Birth_Date = formatForMySql;
                                }
                                else
                                {
                                    indv.Birth_Date = "0000-00-00";
                                }


                                indv.Indv_Id = (string)Indv_list[j];
                                indv.Line = int.Parse(dr[23].ToString().Trim());
                                indv.Location = dr[24].ToString().Trim();
                                Update_Individual(indv);

                                Pedigree ped = new Pedigree();
                                ped.Indv_Id = indv.Indv_Id;
                                ped.Paternal_Id = dr[11].ToString().Trim();
                                ped.Maternal_Id = dr[16].ToString().Trim();


                                if (!individual_exists(ped.Paternal_Id))
                                {
                                    Individual fater = new Individual();
                                    fater.Indv_Id = ped.Paternal_Id;
                                    fater.Gender = "Male";
                                    fater.Line = indv.Line;
                                    fater.Location = indv.Location;
                                    fater.Age = null;
                                    excute_Individual(fater);
                                }
                                if (!individual_exists(ped.Maternal_Id))
                                {
                                    Individual mother = new Individual();
                                    mother.Indv_Id = ped.Maternal_Id;
                                    mother.Gender = "Female";
                                    mother.Line = indv.Line;
                                    mother.Location = indv.Location;
                                    mother.Age = null;
                                    excute_Individual(mother);
                                }

                                ped.Maternal_Id_Modified = null;
                                ped.Paternal_Id_Modified = null;
                                ped.Modified = 0;
                                ped.Num_Offspring = null;
                                ped.Num_Siblings = null;
                                excute_Pedigree(ped);

                            }

                        }


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                DA.Dispose();
            }

            public static Hashtable hshTable = new Hashtable();

            public static void defects_names()
            {
                hshTable.Add("HP", "1");
                hshTable.Add("RG", "2");
                hshTable.Add("BA", "3");
                hshTable.Add("NR", "4");
                hshTable.Add("IT", "5");
                hshTable.Add("SR", "6");
                hshTable.Add("SP", "7");
                hshTable.Add("PA", "8");
                hshTable.Add("NONE", "8");
                hshTable.Add("None", "8");
                hshTable.Add("none", "8");
                hshTable.Add("", "9");
                hshTable.Add("unknown", "9");
                hshTable.Add("Unknown", "9");
            }

            public static float Test_Individual(string Indv_Id)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 300000;
                command.CommandText = "SELECT COUNT(*)/62163 AS Expr1 FROM tblindividual_sample INNER JOIN tblsnp ON tblindividual_sample.Sample_Id = tblsnp.Sample_Id where (tblindividual_sample.Indv_Id ='" + Indv_Id + "')  AND (tblsnp.Genotype <> 'NC')";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                float result = 0;
                while (Reader.Read())
                {
                    result = float.Parse(Reader.GetValue(0).ToString());

                }
                Reader.Close();
                conn.Close();
                return result;
            }

            public static float Test_Marker(string Marker_Id)
            {

                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 300000;
                command.CommandText = @"SELECT COUNT(*) / 689 AS rate
                         FROM tblmarker_loc INNER JOIN
                         tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN
                         tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN
                         tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id
                         WHERE (tblsnp.Genotype <> 'NC') AND (tblsnp.Marker_Id = '" + Marker_Id + "') AND (tblindividual.Included = 1)";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                float result = 0;
                while (Reader.Read())
                {
                    result = float.Parse(Reader.GetValue(0).ToString());

                }
                Reader.Close();
                conn.Close();
                return result;
            }

            public static int sample_exists(string indv_id)
            {
                int return_value = 0;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = "SELECT COUNT(*) AS counter FROM tblindividual_sample WHERE (Indv_Id = '" + indv_id.ToString() + "')";
                Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    if (int.Parse(Reader.GetValue(0).ToString()) == 0)
                    {
                        return_value = 0;
                    }
                    else
                    {
                        return_value = 1;
                    }
                }
                conn.Close();
                Reader.Close();
                return return_value;

            }

            public static int Total_Markers_No()
            {
                int markers = 0;
                string ConString = "SERVER=localhost;" +
                                "DATABASE=db_Proc;" +
                                "UID=root;" +
                                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"Select count(*) from tblmarker_Loc where included = 1 ";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    markers = int.Parse(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                return markers;
            }

            public static void Plink_Format()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Line, tblindividual.Call_Rate, tblindividual_phenotype.Phenotype_Id
                                    FROM  tblindividual INNER JOIN
                                    tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id
                                    WHERE  tblindividual.included = 1 AND tblindividual.Gender_Modified = 0";

                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                
                Reader.Close();
                conn.Close();
                System.IO.StreamWriter output_file_map = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\plink_porc_founder.map");
                System.IO.StreamWriter output_file_ped = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\plink_porc_founder.ped");
                //conn.Open();

                //command.CommandText = "select mar.chr,mar.Marker_Id,mar.Position from tblmarker_loc as mar where Included = 1 and chr <> 'X' order by 0 + chr,Position asc";
                //Reader = command.ExecuteReader();
                //while (Reader.Read())
                //{
                //    output_file_map.WriteLine(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " 0 " + Reader.GetValue(2).ToString());
                //}
                //Reader.Close();
                //conn.Close();
                //output_file_map.Flush();
                //conn.Open();
                //command.CommandText = "select mar.chr,mar.Marker_Id,mar.Position from tblmarker_loc as mar where Included = 1 and chr = 'X' order by chr,Position asc";
                //Reader = command.ExecuteReader();
                //while (Reader.Read())
                //{
                //    output_file_map.WriteLine(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " 0 " + Reader.GetValue(2).ToString());
                //}
                //Reader.Close();
                //conn.Close();
                //output_file_map.Flush();
                //output_file_map.Close();


                for (int i = 0; i < Indv_List.Count; i++)
                {
                    System.Console.WriteLine("indv number " + i + " " + Indv_List[i] + " )");
                    StringBuilder text = new StringBuilder("");
                    int counter = 0;

                    text.Clear();
                    for (int j = 0; j < 21; j++)
                    {
                        string chr = j.ToString();
                        if (chr.CompareTo("19") == 0)
                        {
                            chr = "X";
                        }
                        else if (chr.CompareTo("20") == 0)
                        {
                            chr = "Y";
                        }
                        conn.Open();
                        command.CommandText = "SELECT tblsnp.Top_Allele_A, tblsnp.Top_Allele_B, tblsnp.Marker_Id, tblmarker_loc.Chr, tblmarker_loc.Position, tblindividual_sample.Indv_Id, "
                        + "tblindividual_phenotype.Phenotype_Id, tblindividual.Gender, tblpedigree.Paternal_Id, tblpedigree.Maternal_Id,tblindividual.Line, tblmarker_loc.chr , tblmarker_loc.Marker_Id , tblmarker_loc.position "
                        + "FROM tblmarker_loc INNER JOIN "
                        + "tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN "
                        + "tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN "
                        + "tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id INNER JOIN "
                        + "tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id INNER JOIN "
                        + "tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id "
                        + "WHERE (tblindividual_sample.Indv_Id = '" + Indv_List[i].ToString() + "') AND (tblmarker_loc.Chr = '" + chr.ToString() + "') AND (tblmarker_loc.Included = 1)"
                        + "ORDER BY tblmarker_loc.Position,tblmarker_loc.Marker_Id";
                        command.CommandTimeout = 300000;
                        Reader = command.ExecuteReader();
                        string[] Genotype = { "", "" };

                        while (Reader.Read())
                        {

                            if (j == 0 && counter == 0)
                            {
                                string paternal_id = Reader.GetValue(8).ToString();
                                string maternal_id = Reader.GetValue(9).ToString();
                                int line = int.Parse(Reader.GetValue(10).ToString());
                                if (paternal_id.CompareTo("General") == 0)
                                {
                                    paternal_id = "0";
                                }
                                else
                                {
                                    if (sample_exists(paternal_id) == 0)
                                    {
                                        paternal_id = "0";
                                    }
                                }


                                if (maternal_id.CompareTo("General") == 0)
                                {
                                    maternal_id = "0";
                                }
                                else
                                {
                                    if (sample_exists(maternal_id) == 0)
                                    {
                                        maternal_id = "0";
                                    }
                                }
                                int sex = 0;
                                if (Reader.GetValue(7).ToString().CompareTo("Male") == 0)
                                {
                                    sex = 1;
                                }
                                else
                                {
                                    sex = 2;
                                }

                                if ((int)Reader.GetValue(6) == 2) // the disease 
                                {
                                    text.Append(line + " " + Indv_List[i].ToString() + " " + paternal_id + " " + maternal_id + " " + sex.ToString() + " 2");

                                }
                                else
                                {
                                    text.Append(line + " " + Indv_List[i].ToString() + " " + paternal_id + " " + maternal_id + " " + sex.ToString() + " 1");

                                }
                            }
                            for (int k = 0; k < 2; k++)
                            {
                                switch (Reader.GetValue(k).ToString())
                                {
                                    case "A":
                                        Genotype[k] = "1";
                                        break;
                                    case "C":
                                        Genotype[k] = "2";
                                        break;
                                    case "G":
                                        Genotype[k] = "3";
                                        break;
                                    case "T":
                                        Genotype[k] = "4";
                                        break;
                                    case "-":
                                        Genotype[k] = "0";
                                        break;
                                }
                            }

                            text.Append(" " + Genotype[0].ToString() + " " + Genotype[1].ToString());
                            counter++;
                            if (i == 0)
                            {
                                output_file_map.WriteLine(Reader.GetValue(11).ToString() + " " + Reader.GetValue(12).ToString() + " 0 " + Reader.GetValue(13).ToString());
                                output_file_map.Flush();
                            }

                        }
                        Reader.Close();
                        conn.Close();


                    }

                    output_file_ped.WriteLine(text);
                    output_file_ped.Flush();

                }
                output_file_ped.Close();
                output_file_map.Close();

            }

            public static void My_Format()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT Indv_Id
                                    FROM tblindividual
                                    WHERE (Call_Rate IS NOT NULL) AND (included = 0)";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                System.IO.StreamWriter output_file_ped = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate_exc.ped"); // the new markers with only 98% call rate and no HWD (55 marker excluded)
                System.IO.StreamWriter output_file_map = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate_exc.map");
                StringBuilder text = new StringBuilder("");
                for (int i = 0; i < Indv_List.Count; i++)
                {
                    System.Console.WriteLine("indv number " + i + " " + Indv_List[i] + " )");
                    int counter = 0;
                    text.Clear();
                    text.Append(Indv_List[i].ToString());
                    for (int j = 0; j < 21; j++)
                    {
                        string chr = j.ToString();
                        if (chr.CompareTo("19") == 0)
                        {
                            chr = "X";
                        }
                        else if (chr.CompareTo("20") == 0)
                        {
                            chr = "Y";
                        }
                        conn.Open();
                        command.CommandText = @"SELECT tblsnp.Genotype,tblmarker_loc.chr,tblmarker_loc.marker_id,tblmarker_loc.position 
                         FROM tblmarker_loc INNER JOIN
                         tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN
                         tblsample ON tblsnp.Sample_Id = tblsample.Sample_Id INNER JOIN
                         tblindividual_sample ON tblsample.Sample_Id = tblindividual_sample.Sample_Id
                         WHERE (tblindividual_sample.Indv_Id = '" + Indv_List[i].ToString() + "') AND (tblmarker_loc.Chr = '" + chr.ToString() + @"') AND (tblmarker_loc.Included = 1)
                         ORDER BY tblmarker_loc.Position,tblmarker_loc.Marker_Id";
                        command.CommandTimeout = 300000;
                        Reader = command.ExecuteReader();
                        string[] Genotype = { "", "" };
                        while (Reader.Read())
                        {
                            text.Append(" " + Reader.GetValue(0).ToString());
                            counter++;
                            if (i == 0)
                            {
                                output_file_map.WriteLine(Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString() + " " + Reader.GetValue(3).ToString());
                            }
                        }
                        Reader.Close();
                        conn.Close();
                    }
                    output_file_ped.WriteLine(text);
                    output_file_ped.Flush();
                    if (i == 0)
                    {
                        output_file_map.Flush();
                        output_file_map.Close();
                    }
                }
                 output_file_ped.Flush();
                 output_file_ped.Close();

            }
              
            public static void My_Format_founders()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, Sample_ID, tblpedigree.Maternal_Id,
                                    (select sample_id from tblindividual  
                                    INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id
                                    Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                    where tblindividual.indv_id = tblpedigree.Maternal_Id and tblindividual.call_rate is not null ) AS Maternal_sample_id,
                                    tblpedigree.Paternal_Id,
                                    (select sample_id from tblindividual  
                                    INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id
                                    Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                    where tblindividual.indv_id = tblpedigree.Paternal_Id and tblindividual.call_rate is not null ) AS Paternal_sample_id,
                                    tblindividual_phenotype.phenotype_ID,Paternal_Id_Modified,Maternal_Id_Modified
                                    FROM tblindividual 
                                    INNER JOIN
                                    tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                                    inner join tblindividual_phenotype on  tblindividual.indv_id = tblindividual_phenotype.indv_id 
                                    Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                    WHERE tblindividual.call_rate is not null
                                    and (Maternal_id != 'General' and Paternal_id != 'General') 
                                    Having Maternal_sample_id is not  null or  Paternal_sample_id is  not null
                                    order by Maternal_sample_id";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                System.IO.StreamWriter output_file_founders = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\List_Founders.txt");
                while (Reader.Read())
                {
                    if (Reader.GetValue(3).ToString().CompareTo("") !=0)
                    {
                        if (!exists(Reader.GetValue(2).ToString(),Indv_List))
                        {
                            Indv_List.Add(Reader.GetValue(2).ToString());
                            output_file_founders.WriteLine(Reader.GetValue(2).ToString());
                        }
                        
                    }
                    else
                    {
                        System.Console.WriteLine("no sample");
                    }

                    if (Reader.GetValue(5).ToString().CompareTo("") != 0)
                    {
                        if (!exists(Reader.GetValue(4).ToString(), Indv_List))
                        {
                            Indv_List.Add(Reader.GetValue(4).ToString());
                            output_file_founders.WriteLine(Reader.GetValue(4).ToString());
                        }

                    }
                    else
                    {
                        System.Console.WriteLine("no sample");
                    }
                    
                }
                Reader.Close();
                conn.Close();
                output_file_founders.Flush();
                output_file_founders.Close();
                System.IO.StreamWriter output_file_ped = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\My_Founders.ped");
                System.IO.StreamWriter output_file_map = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\My_Founders.map");
                for (int i = 0; i < Indv_List.Count; i++)
                {
                    System.Console.WriteLine("indv number " + i + " " + Indv_List[i] + " )");
                    StringBuilder text = new StringBuilder("");
                    int counter = 0;
                    text.Clear();
                    text.Append(Indv_List[i].ToString());
                    for (int j = 0; j < 21; j++)
                    {
                        string chr = j.ToString();
                        if (chr.CompareTo("19") == 0)
                        {
                            chr = "X";
                        }
                        else if (chr.CompareTo("20") == 0)
                        {
                            chr = "Y";
                        }
                        conn.Open();
                        command.CommandText = @"SELECT tblsnp.Genotype,tblmarker_loc.chr,tblmarker_loc.marker_id,tblmarker_loc.position
                         FROM tblmarker_loc INNER JOIN
                         tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN
                         tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN
                         tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id
                         WHERE (tblindividual_sample.Indv_Id = '" + Indv_List[i].ToString() + @"') AND (tblmarker_loc.Chr = '" + chr.ToString() + @"')
                         ORDER BY tblmarker_loc.Position, tblmarker_loc.Marker_Id";
                        command.CommandTimeout = 300000;
                        Reader = command.ExecuteReader();
                        string[] Genotype = { "", "" };

                        while (Reader.Read())
                        {
                            text.Append(" " + Reader.GetValue(0).ToString());
                            if (i == 0)
                            {
                                output_file_map.WriteLine(Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString() + " 0 " + Reader.GetValue(3).ToString());
                                output_file_map.Flush();
                            }
                        }
                        Reader.Close();
                        conn.Close();
                    }
                    output_file_ped.WriteLine(text);
                    output_file_ped.Flush();
                }
                output_file_ped.Close();
                output_file_map.Close();
            }

            public static void Plink_RG_marker()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, Sample_ID, tblpedigree.Maternal_Id,
                            (select Sample_ID from tblindividual  
                            INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id
                            Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                            where tblindividual.indv_id = tblpedigree.Maternal_Id and tblindividual.included = 1 and tblindividual.call_rate is not null ) AS Maternal_sample_id,
                            tblpedigree.Paternal_Id,
                            (select Sample_ID from tblindividual  
                            INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id
                            Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                            where tblindividual.indv_id = tblpedigree.Paternal_Id and tblindividual.included = 1 and tblindividual.call_rate is not null ) AS Paternal_sample_id,
                            tblindividual_phenotype.phenotype_ID
                            FROM tblindividual 
                            INNER JOIN
                            tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                            inner join tblindividual_phenotype on  tblindividual.indv_id = tblindividual_phenotype.indv_id 
                            Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                            WHERE tblindividual.included = 1 and tblindividual.call_rate is not null and (Maternal_id != 'General' and Paternal_id != 'General') and tblindividual_phenotype.Phenotype_Id = 2
                            Having Maternal_sample_id is not  null and  Paternal_sample_id is  not null";
                //WHERE (tblindividual.Included = 1)";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();

                while (Reader.Read())
                {
                    string Indv_Id = Reader.GetValue(0).ToString();
                    string Paternal_Id = Reader.GetValue(4).ToString();
                    string Maternal_Id = Reader.GetValue(2).ToString();
                    if (!exists(Indv_Id, Indv_List, 2))
                    {
                        Indv_List.Add(Indv_Id);

                    }
                    if (!exists(Paternal_Id, Indv_List, 2))
                    {
                        Indv_List.Add(Paternal_Id);

                    }
                    if (!exists(Maternal_Id, Indv_List, 2))
                    {
                        Indv_List.Add(Maternal_Id);

                    }
                }
                Reader.Close();
                conn.Close();

                //System.IO.StreamWriter output_file_map = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\Modified\porc.map");
                System.IO.StreamWriter output_file_ped = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\Modified\porc.ped");



                for (int i = 0; i < Indv_List.Count; i++)
                {

                    System.Console.WriteLine("indv number " + i + " " + Indv_List[i] + " )");
                    StringBuilder text = new StringBuilder("");
                    int counter = 0;
                    //string[] txt = new string[Total_Markers_No()];
                    text.Clear();
                    for (int j = 0; j < 1; j++)
                    {
                        string chr = j.ToString();
                        if (chr.CompareTo("19") == 0)
                        {
                            chr = "X";
                        }
                        else if (chr.CompareTo("20") == 0)
                        {
                            chr = "Y";
                        }
                        conn.Open();
                        command.CommandText = "SELECT tblsnp.Top_Allele_A, tblsnp.Top_Allele_B, tblsnp.Marker_Id, tblmarker_loc.Chr, tblmarker_loc.Position, tblindividual_sample.Indv_Id, "
                        + "tblindividual_phenotype.Phenotype_Id, tblindividual.Gender, tblpedigree.Paternal_Id, tblpedigree.Maternal_Id,tblindividual.Line, tblmarker_loc.chr , tblmarker_loc.Marker_Id , tblmarker_loc.position "
                        + "FROM tblmarker_loc INNER JOIN "
                        + "tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN "
                        + "tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN "
                        + "tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id INNER JOIN "
                        + "tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id INNER JOIN "
                        + "tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id "
                        + "WHERE (tblindividual_sample.Indv_Id = '" + Indv_List[i].ToString() + "') AND (tblmarker_loc.Chr = '" + chr.ToString() + "') AND (tblmarker_loc.Marker_Id = 'ALGA0105472')"
                        + "ORDER BY tblmarker_loc.Position,tblmarker_loc.Marker_Id";
                        command.CommandTimeout = 300000;
                        Reader = command.ExecuteReader();
                        string[] Genotype = { "", "" };

                        while (Reader.Read())
                        {

                            if (j == 0 && counter == 0)
                            {
                                string paternal_id = Reader.GetValue(8).ToString();
                                string maternal_id = Reader.GetValue(9).ToString();
                                int line = int.Parse(Reader.GetValue(10).ToString());
                                if (paternal_id.CompareTo("General") == 0)
                                {
                                    paternal_id = "0";
                                }
                                else
                                {
                                    if (sample_exists(paternal_id) == 0)
                                    {
                                        paternal_id = "0";
                                    }
                                }


                                if (maternal_id.CompareTo("General") == 0)
                                {
                                    maternal_id = "0";
                                }
                                else
                                {
                                    if (sample_exists(maternal_id) == 0)
                                    {
                                        maternal_id = "0";
                                    }
                                }
                                int sex = 0;
                                if (Reader.GetValue(7).ToString().CompareTo("Male") == 0)
                                {
                                    sex = 1;
                                }
                                else
                                {
                                    sex = 2;
                                }

                                if ((int)Reader.GetValue(6) == 2) // the disease 
                                {
                                    text.Append(line + " " + Indv_List[i].ToString() + " " + paternal_id + " " + maternal_id + " " + sex.ToString() + " 2");

                                }
                                else
                                {
                                    text.Append(line + " " + Indv_List[i].ToString() + " " + paternal_id + " " + maternal_id + " " + sex.ToString() + " 1");

                                }
                            }
                            for (int k = 0; k < 2; k++)
                            {
                                switch (Reader.GetValue(k).ToString())
                                {
                                    case "A":
                                        Genotype[k] = "1";
                                        break;
                                    case "C":
                                        Genotype[k] = "2";
                                        break;
                                    case "G":
                                        Genotype[k] = "3";
                                        break;
                                    case "T":
                                        Genotype[k] = "4";
                                        break;
                                    case "-":
                                        Genotype[k] = "0";
                                        break;
                                }
                            }

                            text.Append(" " + Genotype[0].ToString() + " " + Genotype[1].ToString());
                            counter++;


                        }
                        Reader.Close();
                        conn.Close();


                    }

                    //int leng = text.Length;
                    //char[] delimiters = new char[] { ' ' };
                    //string[] line_parts = text.ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    output_file_ped.WriteLine(text);
                    output_file_ped.Flush();

                }
                output_file_ped.Flush();
                output_file_ped.Close();
                //output_file_map.Close();
            }

            public static void CSV_Format()
            {
                /////////////////////////////////       CSV Formate for R       /////////////////////////////
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Line
                                                FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
                                                tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id
                                                WHERE (tblindividual.Included = 1)
                                                GROUP BY tblindividual.Indv_Id, tblindividual.Line";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();


                string line_1 = "RG,";
                string line_2 = ",";
                string line_3 = ",";

                for (int i = 0; i < 20; i++)
                {
                    string chr = i.ToString();
                    if (chr.CompareTo("19") == 0)
                    {
                        chr = "X";
                    }
                    else if (chr.CompareTo("20") == 0)
                    {
                        chr = "Y";
                    }
                    conn.Open();
                    command.CommandText = "select  Marker_Id, chr,position from tblmarker_loc where chr = '" + chr + "' AND Included = 1 order by position asc, Marker_Id";
                    Reader = command.ExecuteReader();
                    while (Reader.Read())
                    {
                        line_1 += Reader.GetValue(0).ToString() + ",";
                        line_2 += Reader.GetValue(1).ToString() + ",";
                        line_3 += (double)int.Parse(Reader.GetValue(2).ToString()) / 1000000 + ",";
                    }
                    Reader.Close();
                    conn.Close();

                }

                System.IO.StreamWriter output_file = new System.IO.StreamWriter(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\MQM.txt");
                output_file.WriteLine(line_1);
                output_file.WriteLine(line_2);
                output_file.WriteLine(line_3);
                output_file.Flush();
                output_file.Close();

                for (int i = 0; i < Indv_List.Count; i++)
                {
                    System.Console.WriteLine("indv number " + i + " " + Indv_List[i] + " )");
                    string text = "";
                    int counter = 0;
                    for (int j = 0; j < 20; j++)
                    {
                        string chr = j.ToString();
                        if (chr.CompareTo("19") == 0)
                        {
                            chr = "X";
                        }
                        else if (chr.CompareTo("20") == 0)
                        {
                            chr = "Y";
                        }
                        conn.Open();
                        command.CommandText = "SELECT tblsnp.Genotype, tblsnp.Marker_Id, tblmarker_loc.Included, tblmarker_loc.Chr, tblmarker_loc.Position, tblindividual_sample.Indv_Id, "
                               + "tblindividual_phenotype.Phenotype_Id "
                               + "FROM tblmarker_loc INNER JOIN "
                               + "tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN "
                               + "tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN "
                               + "tblindividual_phenotype ON tblindividual_sample.Indv_Id = tblindividual_phenotype.Indv_Id "
                               + "WHERE (tblindividual_sample.Indv_Id = '" + (string)Indv_List[i] + "') AND (tblmarker_loc.Included = 1) AND (tblmarker_loc.Chr = '" + chr.ToString() + "') "
                               + "ORDER BY tblmarker_loc.Position asc, tblmarker_loc.Marker_Id";
                        Reader = command.ExecuteReader();
                        string Genotype = "";
                        while (Reader.Read())
                        {
                            if (j == 0 && counter == 0)
                            {
                                if ((int)Reader.GetValue(6) == 2)
                                {
                                    text = "1";
                                }
                                else
                                {
                                    text = "0";
                                }
                            }
                            switch (Reader.GetValue(0).ToString())
                            {
                                case "AB":
                                    Genotype = "H";
                                    break;
                                case "AA":
                                    Genotype = "A";
                                    break;
                                case "BB":
                                    Genotype = "B";
                                    break;
                                case "NC":
                                    Genotype = "-";
                                    break;
                            }
                            text += "," + Genotype.ToString();
                            counter++;
                        }
                        Reader.Close();
                        conn.Close();
                    }
                    output_file.WriteLine(text);
                    output_file.Flush();
                }
                output_file.Close();
            }

            public static void total_p()
            {
                string ConString = "SERVER=localhost;" +
                    "DATABASE=db_Proc;" +
                    "UID=root;" +
                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT Marker_Id FROM tblmarker_loc  WHERE (Chr <> 'X') AND (Chr <> 'Y') AND (Founder_Control < 10) AND (call_rate >=0.98) ORDER BY 0 + Chr, Position, Marker_Id";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Marker_List.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\My_data.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }
            }

            public static void execlude_marker_individual()
            {
                /////////////////////////////////////         Quality control marker individual      ///////////////////////////////////
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 3000000;

                int start = 0;

                for (int i = 0; i < 32; i++)
                {
                    start = i * 2000;
                    command.CommandText = @"SELECT Marker_Id,call_rate from tblMarker_Loc limit " + start + ", 2000";
                    Reader = command.ExecuteReader();
                    int counter = 0;
                    while (Reader.Read())
                    {

                        Marker_Loc Marker = new Marker_Loc();
                        Marker.Marker_Id = Reader.GetValue(0).ToString();
                        Marker.Call_Rate = float.Parse(Reader.GetValue(1).ToString());

                        if (Marker.Call_Rate < 0.98)
                        {
                            Exclude_Marker(Reader.GetValue(0).ToString());
                            System.Console.WriteLine("Excluded marker " + Marker.Marker_Id);
                        }

                        counter++;

                    }
                    conn.Close();
                    Reader.Close();
                    conn.Open();
                    System.Console.WriteLine("satrt " + start);
                }
                conn.Close();

            }

            public static void HWE_markers()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = "select Marker_Id from tblmarker_loc where chr <> 'X'  and chr <> 'Y' and included = 1 order by 0 + chr,Position,marker_id";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Marker_List.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();

                for (int j = 0; j < Marker_List.Count; j++)
                {

                    Marker_Genotype_Frequency_Faster((string)Marker_List[j]);
                    System.Console.WriteLine(j);

                }

            }

            //public static void HWE_X_Chr()
            //{
            //    string ConString = "SERVER=localhost;" +
            //    "DATABASE=db_Proc;" +
            //    "UID=root;" +
            //    "PASSWORD=mahmoud;";
            //    MySqlConnection conn = new MySqlConnection(ConString);
            //    conn.Open();
            //    MySqlCommand command = conn.CreateCommand();
            //    command.CommandTimeout = 3000000;
            //    MySqlDataReader Reader;
            //    command.CommandText = "select Marker_Id from tblmarker_loc where chr = 'X'  and included = 1 order by 0 + chr,Position asc";
            //    Reader = command.ExecuteReader();
            //    ArrayList Marker_List = new ArrayList();
            //    while (Reader.Read())
            //    {
            //        Marker_List.Add(Reader.GetValue(0).ToString());
            //    }
            //    Reader.Close();
            //    conn.Close();

            //    for (int j = 0; j < Marker_List.Count; j++)
            //    {

            //        Marker_Genotype_Female((string)Marker_List[j]);
            //        System.Console.WriteLine(j);

            //    }


            //}

            public static void execlude_marker_MAF()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = @"select Marker_Id from tblmarker_loc where A_Freq < 0.05 OR B_Freq < 0.05 AND call_rate >=0.95";
                Reader = command.ExecuteReader();
                ArrayList Result_list = new ArrayList();
                int count = 0;
                while (Reader.Read())
                {
                    Exclude_Marker(Reader.GetValue(0).ToString());
                    count++;
                    System.Console.WriteLine(count);
                }
            }

            public static void SNP_sex()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Gender
                                FROM tblindividual Where tblindividual.Included = 1";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(1).ToString());
                }
                Reader.Close();
                conn.Close();

                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }

                ArrayList sex_pred = new ArrayList();
                System.IO.StreamWriter output_file_sex = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\sex_pred.txt");
                output_file_sex.WriteLine("Indv" + "\t" + "Gender" + "\t" + "Percentage");
                int indv_index = -1;
                for (i = 0; i < Indv_list.Count; i++)
                {
                    indv_index = -1;
                    for (int j = 0; j < Data_Set.GetLength(0); j++)
                    {
                        if (Indv_list[i].ToString().Split('\t')[0].CompareTo(Data_Set[j, 0]) == 0)
                        {
                            indv_index = j;
                            break;
                        }
                    }
                    sex_pred.Add(sex_prediction_file(indv_index));
                    output_file_sex.WriteLine(Indv_list[i].ToString() + "\t" + sex_pred[i].ToString());
                    System.Console.WriteLine("Indv " + i + " is finished");
                }
                output_file_sex.Flush();
                output_file_sex.Close();
            }

            public static void IBS_Test()
            {

                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 300000;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Line
                                FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
                                tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id
                                where  tblindividual.Included = 1
                                GROUP BY tblindividual.Indv_Id, tblindividual.Line";

                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();


                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                string[,] Data_Set = new string[Indv_list.Count, 49404];
                for (int i = 0; i < Indv_list.Count; i++)
                {

                    command.CommandText = @"SELECT tblindividual_sample.Indv_Id, tblindividual_sample.Sample_Id, tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                                                        FROM tblsnp INNER JOIN
                                                        tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                                        INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                                        WHERE (tblindividual_sample.Indv_Id = '" + (string)Indv_list[i] + "') AND (tblmarker_loc.Included = 1) AND  (tblmarker_loc.Chr <> 'X') AND  (tblmarker_loc.Chr <> 'Y') ORDER BY tblmarker_loc.Marker_Id";
                    Reader = command.ExecuteReader();
                    //schemaTable  = Reader.GetSchemaTable();
                    //System.Console.WriteLine("Indvidual " + i + " is added to data_set");

                    int j = 0;
                    while (Reader.Read())
                    {

                        Data_Set[i, j] = (Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(1).ToString() + "\t" + Reader.GetValue(2).ToString() + "\t" + Reader.GetValue(3).ToString() + "\t" + Reader.GetValue(4).ToString());
                        j++;

                    }
                    Reader.Close();
                }
                conn.Close();
                System.Console.WriteLine(GC.GetTotalMemory(true));
                Matrix mds = new Matrix(Indv_list.Count, Indv_list.Count);
                for (int i = 0; i < Indv_list.Count; i++)
                {

                    for (int j = 0; j < Indv_list.Count; j++)
                    {
                        if (i == j)
                        {
                            mds[i, j] = 1;

                        }
                        else if (j > i)
                        {
                            mds[i, j] = IBS(i, j, Data_Set);
                        }
                        else
                        {
                            mds[i, j] = mds[j, i];
                        }
                        System.Console.Write(mds[i, j].ToString() + "  ");
                    }
                    System.Console.WriteLine("");


                }
                System.IO.File.AppendAllText(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\matrix_mds.txt", mds.ToString());

            }

            public static void IBS_Test_v2()
            {
                string ConString = "SERVER=localhost;" +
              "DATABASE=db_Proc;" +
              "UID=root;" +
              "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 300000;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Line
                                FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
                                tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id
                                where  tblindividual.Included = 1
                                GROUP BY tblindividual.Indv_Id, tblindividual.Line";

                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();


                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();

                Matrix mds = new Matrix(Indv_list.Count, Indv_list.Count);
                string[] Data_Set1 = new string[48137];
                string[] Data_Set2 = new string[48137];
                for (int i = 0; i < Indv_list.Count; i++)
                {
                    System.Console.WriteLine("Individual number " + i + " Started");
                    Array.Clear(Data_Set1, 0, Data_Set1.Length);
                    conn.Open();
                    command.CommandText = @"SELECT tblindividual_sample.Indv_Id, tblindividual_sample.Sample_Id, tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                                        FROM tblsnp INNER JOIN
                                        tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                        INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                        WHERE (tblindividual_sample.Indv_Id = '" + (string)Indv_list[i] + "') AND (tblmarker_loc.Included = 1) AND  (tblmarker_loc.Chr <> 'X') AND  (tblmarker_loc.Chr <> 'Y') ORDER BY tblmarker_loc.Marker_Id";
                    Reader = command.ExecuteReader();
                    int k = 0;
                    while (Reader.Read())
                    {

                        Data_Set1[k] = (Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(1).ToString() + "\t" + Reader.GetValue(2).ToString() + "\t" + Reader.GetValue(3).ToString() + "\t" + Reader.GetValue(4).ToString());
                        k++;

                    }
                    Reader.Close();
                    conn.Close();
                    for (int j = 0; j < Indv_list.Count; j++)
                    {
                        if (i == j)
                        {
                            mds[i, j] = 1;

                        }
                        else if (j > i)
                        {
                            Array.Clear(Data_Set2, 0, Data_Set2.Length);
                            conn.Open();
                            command.CommandText = @"SELECT tblindividual_sample.Indv_Id, tblindividual_sample.Sample_Id, tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                                        FROM tblsnp INNER JOIN
                                        tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                        INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                        WHERE (tblindividual_sample.Indv_Id = '" + (string)Indv_list[i] + "') AND (tblmarker_loc.Included = 1) AND  (tblmarker_loc.Chr <> 'X') AND  (tblmarker_loc.Chr <> 'Y') ORDER BY tblmarker_loc.Marker_Id";
                            Reader = command.ExecuteReader();
                            k = 0;
                            while (Reader.Read())
                            {

                                Data_Set2[k] = (Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(1).ToString() + "\t" + Reader.GetValue(2).ToString() + "\t" + Reader.GetValue(3).ToString() + "\t" + Reader.GetValue(4).ToString());
                                k++;

                            }
                            Reader.Close();
                            conn.Close();
                            mds[i, j] = IBS2(i, j, Data_Set1, Data_Set2);
                        }
                        else
                        {
                            mds[i, j] = mds[j, i];
                        }

                    }

                }
                System.IO.File.AppendAllText(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\matrix_mds.txt", mds.ToString());
            }

            public static Boolean Have_Sample(string Indv_Id)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "SELECT tblindividual.Indv_Id FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id WHERE (tblindividual.Indv_Id = '" + Indv_Id.ToString() + "')";
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                conn.Close();
                return Found;
            }

            public static Boolean Indv_Exist(string Indv_Id)
            {
                string ConString = "SERVER=localhost;" +
               "DATABASE=db_Proc;" +
               "UID=root;" +
               "PASSWORD=mahmoud;";
                Boolean Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "SELECT tblindividual.Indv_Id FROM tblindividual WHERE (tblindividual.Indv_Id = '" + Indv_Id.ToString() + "')";
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Found = true;
                }
                Reader.Close();
                conn.Close();
                return Found;
            }

            public static void excute_update_offspring(string Indv_Id, int offspring_num)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblpedigree set Num_Offspring = " + offspring_num.ToString() + " where Indv_Id = '" + Indv_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }

            public static void Update_Num_of_offspring()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "select indv_id,gender from tblindividual where call_rate is not null order by indv_id";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString());

                }
                Reader.Close();
                conn.Close();

                for (int i = 0; i < Indv_List.Count; i++)
                {
                    string text = "";
                    int off_num = 0;
                    if (Indv_List[i].ToString().Split(' ')[1].CompareTo("Male") == 0)
                    {
                        text = "Paternal_id = '" + Indv_List[i].ToString().Split(' ')[0] + "'";
                    }
                    else
                    {
                        text = "Maternal_id = '" + Indv_List[i].ToString().Split(' ')[0] + "'";
                    }
                    conn.Open();
                    command.CommandText = @"SELECT count(*)
                         FROM tblpedigree INNER JOIN
                         tblindividual ON tblpedigree.Indv_Id = tblindividual.Indv_Id INNER JOIN
                         tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id
                          where " + text;
                    Reader = command.ExecuteReader();
                    while (Reader.Read())
                    {
                        excute_update_offspring(Indv_List[i].ToString().Split(' ')[0], int.Parse(Reader.GetValue(0).ToString()));
                    }
                    Reader.Close();
                    conn.Close();
                }
            }

            public static void flag_old_dataset(ArrayList Indv_List)
            {

                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);

                foreach (string indv_id in Indv_List)
                {
                    conn.Open();
                    string updateSql = "Update db_proc.tblindividual set New_dataset = 0 where Indv_Id = '" + indv_id.ToString() + "';";
                    MySqlCommand command = new MySqlCommand(updateSql, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                }

            }

            public static void insert_new_file(ArrayList Indv_list)
            {
                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\New_Data_Aneleen_31_08.xlsx";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [Originele lijst$]", CnStr);
                DA.Fill(ds, "Originele lijst");



                foreach (DataRow dr in ds.Tables["Originele lijst"].Rows)
                {

                    for (int j = 0; j < Indv_list.Count; j++)
                    {
                        if (dr[0].ToString().Trim().CompareTo("") != 0)
                        {
                            int indv_number = int.Parse(Indv_list[j].ToString().Substring(2, Indv_list[j].ToString().Length - 2));
                            if (int.Parse(dr[0].ToString().Trim()) == indv_number)
                            {
                                System.Console.WriteLine(" Indv ID found " + Indv_list[j].ToString());

                                if (dr[0].ToString().CompareTo("") != 0)
                                {
                                    Individual indv = new Individual();
                                    indv.Indv_Id = Indv_list[j].ToString();
                                    if (dr[7].ToString().CompareTo("") != 0)
                                    {
                                        indv.Line = int.Parse(dr[7].ToString().Trim().Substring(0, 2));
                                    }
                                    else
                                    {
                                        indv.Line = 0;
                                    }
                                    indv.Location = "Unknown";
                                    if (dr[9].ToString().Trim().CompareTo("") != 0)
                                    {
                                        DateTime date = DateTime.Parse(dr[9].ToString().Trim());
                                        string formatForMySql = date.ToString("yyyy-MM-dd");
                                        indv.Birth_Date = formatForMySql;
                                    }
                                    else
                                    {
                                        indv.Birth_Date = "0000-00-00";
                                    }

                                    Update_Individual(indv);
                                    //Update_Individual_Birthdate(indv);
                                    Individual_Phenotype indv_phen = new Individual_Phenotype();
                                    indv_phen.Indv_Id = indv.Indv_Id;
                                    var defect = hshTable[dr[5].ToString().Trim()];
                                    indv_phen.Phenotype_Id = int.Parse(defect.ToString());
                                    excute_Individual_Phenotype(indv_phen);
                                    ///////////////////////////////////////////////////
                                    //Pedigree ped = new Pedigree();
                                    //ped.Indv_Id = indv.Indv_Id;
                                    //ped.Paternal_Id = dr[7].ToString().Trim();
                                    //ped.Maternal_Id = dr[8].ToString().Trim();
                                    //ped.Maternal_Id_Modified = null;
                                    //ped.Paternal_Id_Modified = null;
                                    //ped.Modified = 0;
                                    //ped.Num_Offspring = null;
                                    //ped.Num_Siblings = null;
                                    //excute_Pedigree(ped);
                                }
                            }
                        }

                    }

                }
                DA.Dispose();

            }

            public static void insert_indv_pheno(ArrayList Indv_list)
            {
                ///////////////////////////////    Individual Defect      /////////////////////////////

                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                DA.Fill(ds, "samples");



                foreach (DataRow dr in ds.Tables["samples"].Rows)
                {

                    for (int j = 0; j < Indv_list.Count; j++)
                    {
                        if (Indv_list[j].Equals(dr[0].ToString().Trim()))
                        {
                            if (dr[0].ToString().CompareTo("") != 0)
                            {
                                Individual_Phenotype indv_phen = new Individual_Phenotype();
                                indv_phen.Indv_Id = dr[0].ToString().Trim();
                                var defect = hshTable[dr[22].ToString().Trim()];
                                indv_phen.Phenotype_Id = int.Parse(defect.ToString());
                                excute_Individual_Phenotype(indv_phen);
                            }
                        }

                    }

                }
                DA.Dispose();

            }

            public static int chr_marker_number(string chr)
            {
                int marker_count = 0;
                string ConString = "SERVER=localhost;" +
               "DATABASE=db_Proc;" +
               "UID=root;" +
               "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT count(*) from tblmarker_loc where included = 1 and chr = '" + chr + "'";
                Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    marker_count = int.Parse(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                return marker_count;
            }

            public static void Phasebook_format()
            {
                string ConString = "SERVER=localhost;" +
               "DATABASE=db_Proc;" +
               "UID=root;" +
               "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Line
                                    FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
                                    tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id
                                    WHERE (tblindividual.Included = 1)";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();


                conn.Open();
                for (int j = 0; j < 21; j++)
                {
                    string chr = j.ToString();
                    if (chr.CompareTo("19") == 0)
                    {
                        chr = "X";
                    }
                    else if (chr.CompareTo("20") == 0)
                    {
                        chr = "Y";
                    }
                    System.IO.StreamWriter output_file_map = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\marker" + j + ".txt");
                    command.CommandText = "select  Marker_Id,Position/1000000 from tblmarker_loc where Included = 1 and chr ='" + chr + "' order by Position asc";
                    Reader = command.ExecuteReader();
                    int counter = 0;
                    while (Reader.Read())
                    {
                        counter++;
                        output_file_map.WriteLine("\t" + counter + "\t" + Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(1).ToString());
                    }
                    output_file_map.Flush();
                    output_file_map.Close();
                    Reader.Close();
                    conn.Close();
                    conn.Open();

                }
                conn.Close();
                conn.Open();

                for (int j = 0; j < 21; j++)
                {
                    System.Console.WriteLine("Chr number " + j + "is inserted ");
                    StringBuilder text = new StringBuilder("");

                    string chr = j.ToString();
                    if (chr.CompareTo("19") == 0)
                    {
                        chr = "X";
                    }
                    else if (chr.CompareTo("20") == 0)
                    {
                        chr = "Y";
                    }
                    //string[] txt = new string[chr_marker_number(chr)];
                    System.IO.StreamWriter output_file_ped = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\geno" + j + ".txt");
                    for (int i = 0; i < Indv_List.Count; i++)
                    {

                        command.CommandText = "SELECT Genotype, tblindividual.`index` "
                        + "FROM tblmarker_loc INNER JOIN "
                        + "tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN "
                        + "tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN "
                        + "tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id INNER JOIN "
                        + "tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id INNER JOIN "
                        + "tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id "
                        + "WHERE (tblindividual_sample.Indv_Id = '" + Indv_List[i].ToString() + "') AND (tblmarker_loc.Included = 1) AND (tblmarker_loc.Chr = '" + chr.ToString() + "')"
                        + "ORDER BY tblmarker_loc.Position, tblmarker_loc.Marker_Id";
                        command.CommandTimeout = 3000000;
                        Reader = command.ExecuteReader();
                        string[] Genotype = { "", "" };
                        int counter = 0;
                        text.Clear();
                        while (Reader.Read())
                        {
                            if (counter == 0)
                            {
                                text.Append(" " + Reader.GetValue(1).ToString());
                                counter++;
                            }

                            for (int k = 0; k < 2; k++)
                            {
                                if (Reader.GetValue(0).ToString().CompareTo("NC") == 0)
                                {
                                    Genotype[k] = "0";
                                }
                                else
                                {
                                    switch (Reader.GetValue(0).ToString().Substring(k, 1))
                                    {
                                        case "A":
                                            Genotype[k] = "1";
                                            break;
                                        case "B":
                                            Genotype[k] = "2";
                                            break;
                                    }
                                }
                            }
                            text.Append(" " + Genotype[0].ToString() + " " + Genotype[1].ToString());
                            counter++;
                        }
                        Reader.Close();
                        conn.Close();
                        conn.Open();
                        output_file_ped.WriteLine(text);
                        output_file_ped.Flush();

                    }
                    output_file_ped.Close();
                }
            }

            public static OleDbConnection con;

            public static string Map_tatoe_id(string tatoe)
            {
                string indv_id = "notfound";
                string F1Name = "C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test\\Id_Link.xlsx";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [Sheet1$]", CnStr);
                DA.Fill(ds, "Sheet1");
                foreach (DataRow dr in ds.Tables["Sheet1"].Rows)
                {
                    if (tatoe.Equals(dr[1].ToString().Trim()))
                    {
                        indv_id = dr[0].ToString().Trim();
                        break;
                    }
                }
                return indv_id;
            }

            public static void create_pedigree()
            {

                string ConString = "SERVER=localhost;" +
              "DATABASE=db_Proc;" +
              "UID=root;" +
              "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id
                         FROM tblindividual INNER JOIN
                         tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id
                         WHERE (tblindividual.Call_Rate IS NOT NULL) AND (tblindividual_phenotype.Phenotype_Id <> 8)";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();

                    string conStr = @"Provider=Microsoft.JET.OLEDB.4.0; data source=PIGENDEF_data.mdb; Jet OLEDB:Database Password=;";
                    con = new OleDbConnection(conStr);  // connection string change database name and password here.
                    con.Open(); //connection must be openned
                    OleDbCommand cmd = new OleDbCommand("SELECT * from bu_pedigree190404;", con); // creating query command
                    OleDbDataReader reader = cmd.ExecuteReader(); // executes query
                    System.IO.StreamWriter output_file_ped = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Pedigree.txt");
                    bool found = false;
                    bool found_excel = false;
                    string sample_paternal = "";
                    string sample_maternal = "";
                    foreach (string indv_id in Indv_List)
                    {
                        //if (indv_id.CompareTo("BA004582") == 0)
                        //{
                        //    int c = 21;
                        //}
                        while (reader.Read())
                        {
                            if (reader.GetValue(0).ToString().CompareTo(indv_id) == 0)
                            {
                                Pedigree ped = new Pedigree();
                                ped.Indv_Id = indv_id;


                                if (reader.GetValue(5).ToString().CompareTo("") == 0)
                                {
                                    ped.Paternal_Id = Map_tatoe_id(reader.GetValue(3).ToString());
                                    

                                }
                                else if (reader.GetValue(5).ToString().Substring(0, 2).CompareTo("NS") == 0 || reader.GetValue(5).ToString().CompareTo("0") == 0)
                                {
                                    ped.Paternal_Id = Map_tatoe_id(reader.GetValue(3).ToString());
                                }
                                else
                                {
                                    ped.Paternal_Id = reader.GetValue(5).ToString();
                                }
                                ////////////////////////////////////
                                if (reader.GetValue(6).ToString().CompareTo("") == 0)
                                {
                                    ped.Maternal_Id = Map_tatoe_id(reader.GetValue(4).ToString());
                                }
                                else if (reader.GetValue(6).ToString().Substring(0, 2).CompareTo("NS") == 0 || reader.GetValue(6).ToString().CompareTo("0") == 0)
                                {
                                    ped.Maternal_Id = Map_tatoe_id(reader.GetValue(4).ToString());
                                }
                                else
                                {
                                    ped.Maternal_Id = reader.GetValue(6).ToString();
                                }

                                sample_paternal = sample_exists(ped.Paternal_Id).ToString();
                                sample_maternal = sample_exists(ped.Maternal_Id).ToString();
                                ped.Maternal_Id_Modified = null;
                                ped.Paternal_Id_Modified = null;
                                ped.Modified = 0;
                                ped.Num_Offspring = null;
                                ped.Num_Siblings = null;
                                System.Console.WriteLine("Indv " + indv_id + " inserted sccessfully");
                                output_file_ped.WriteLine(indv_id + " " + ped.Paternal_Id + " " + ped.Maternal_Id + " " + sample_paternal + " " + sample_maternal);
                                excute_Pedigree(ped);
                                found = true;
                                break;

                            }

                        }
                        if (!found)
                        {
                            //output_file_ped.WriteLine(indv_id + " Is not there");
                             string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";

                            string CnStr = string.Empty;

                            if (Path.GetExtension(F1Name) == ".xlsx")
                            {
                                CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                                    ";Extended Properties=Excel 12.0;";
                            }
                            else
                            {
                                CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                            }

                            DataSet ds = new DataSet();
                            OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                            DA.Fill(ds, "samples");
                                foreach (DataRow dr in ds.Tables["samples"].Rows)
                                {
                                    if (indv_id.CompareTo(dr[0].ToString().Trim()) == 0)
                                    {
                                        Pedigree ped = new Pedigree();
                                        ped.Indv_Id = indv_id;
                                        ped.Paternal_Id = dr[11].ToString().Trim();
                                        ped.Maternal_Id = dr[16].ToString().Trim();
                                        ped.Maternal_Id_Modified = null;
                                        ped.Paternal_Id_Modified = null;
                                        ped.Modified = 0;
                                        ped.Num_Offspring = null;
                                        ped.Num_Siblings = null;
                                        System.Console.WriteLine("Indv " + indv_id + " inserted sccessfully");
                                        if (ped.Paternal_Id.CompareTo("") == 0 && ped.Maternal_Id.CompareTo("") == 0)
                                        {
                                            sample_paternal = "0";
                                            sample_maternal = "0";
                                            output_file_ped.WriteLine("*" + indv_id + " " + "notfound" + " " + "notfound" + " " + sample_paternal + " " + sample_maternal);
                                        }
                                        else if (ped.Paternal_Id.CompareTo("") == 0)
                                        {
                                            sample_paternal = "0";
                                            sample_maternal = sample_exists(ped.Maternal_Id).ToString();
                                            output_file_ped.WriteLine("*" + indv_id + " " + "notfound" + " " + ped.Maternal_Id + " " + sample_paternal + " " + sample_maternal);
                                        }
                                        else if (ped.Maternal_Id.CompareTo("") == 0)
                                        {
                                            sample_paternal = sample_exists(ped.Paternal_Id).ToString();
                                            sample_maternal = "0";
                                            output_file_ped.WriteLine("*" + indv_id + " " + ped.Paternal_Id + " " + "notfound" + " " + sample_paternal + " " + sample_maternal);
                                        }
                                        else
                                        {
                                            sample_paternal = sample_exists(ped.Paternal_Id).ToString();
                                            sample_maternal = sample_exists(ped.Maternal_Id).ToString();
                                            output_file_ped.WriteLine("*" + indv_id + " " + ped.Paternal_Id + " " + ped.Maternal_Id + " " + sample_paternal + " " + sample_maternal);
                                        }
                                        found_excel = true;
                                        excute_Pedigree(ped);
                                        break;

                                    }
                                }
                                if (!found_excel)
                                {
                                    output_file_ped.WriteLine("*" + indv_id + " Not found in excel nor acess");
                                }
                                found_excel = false;
                                sample_maternal = "M";
                                sample_paternal = "P";

                            
                        }
                        found = false;
                        reader.Close();
                        reader = cmd.ExecuteReader();

                    }
                    reader.Close();
                    output_file_ped.Flush();
                    output_file_ped.Close();

            }

            public static void check_birth_date()
            {
                string ConString = "SERVER=localhost;" +
                            "DATABASE=db_Proc;" +
                            "UID=root;" +
                            "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"select Indv_Id,Birth_date from tblindividual where call_rate is not null and included = 1;";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString());
                }
                conn.Close();
                Reader.Close();
                System.IO.StreamWriter output_file_tdt = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Birth_Date.txt");
                for (int i = 0; i < Indv_List.Count; i++)
                {

                    string conStr = @"Provider=Microsoft.JET.OLEDB.4.0; data source=PIGENDEF_data.mdb; Jet OLEDB:Database Password=;";
                    con = new OleDbConnection(conStr);  // connection string change database name and password here.
                    con.Open(); //connection must be openned
                    OleDbCommand cmd = new OleDbCommand("SELECT * from bu_pedigree190404;", con); // creating query command
                    OleDbDataReader reader = cmd.ExecuteReader(); // executes query
                    while (reader.Read()) // if can read row from database
                    {
                        if (reader.GetValue(0).ToString().CompareTo(Indv_List[i].ToString().Split(' ')[0]) == 0)
                        {
                            Indv_List[i] = Indv_List[i].ToString() + " " + reader.GetValue(7).ToString();
                            System.Console.WriteLine(Indv_List[i].ToString());
                            break;
                        }

                    }
                    reader.Close();
                }
            }

            // public static string[,] Data_Set = new string[689, 48137];

            public static void Parental_Conflict_All()
            {
                string ConString = "SERVER=localhost;" +
                                  "DATABASE=db_Proc;" +
                                  "UID=root;" +
                                  "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Call_Rate, tblindividual.Gender, tblindividual.Age, tblindividual.Birth_Date, tblindividual.Line, tblindividual.Location, 
                                    tblindividual.Included 
                                    FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id 
                                    WHERE (tblindividual.Included = 1)";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                conn.Open();
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Newest_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }

                for (i = 0; i < Indv_list.Count; i++)
                {
                    System.Console.WriteLine("individual " + i);
                    System.IO.StreamWriter Indv_File = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\P_C_conflict\" + (string)Indv_list[i] + ".txt");
                    for (int j = i; j < Indv_list.Count; j++)
                    {
                        if (i != j)
                        {
                            Parent_offspring_test(i, j, (string)Indv_list[i], (string)Indv_list[j], Indv_File);
                        }
                    }
                    Indv_File.Flush();
                    Indv_File.Close();
                }
            }

            public static void Parental_Conflict()
            {
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Call_Rate, tblindividual.Gender, tblindividual.Age, tblindividual.Birth_Date, tblindividual.Line, tblindividual.Location, 
                                    tblindividual.Included 
                                    FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id 
                                    WHERE (tblindividual.Included = 1) and  (tblindividual.call_rate is not null)";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                conn.Open();
                ArrayList Data_Set1 = new ArrayList();
                ArrayList Data_Set2 = new ArrayList();
                for (int i = 0; i < Indv_list.Count; i++)
                {
                    Data_Set1.Clear();
                    command.CommandText = @"SELECT tblsnp.Genotype
                                    FROM tblsnp INNER JOIN
                                    tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                    INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                    WHERE (tblindividual_sample.Indv_Id = '" + (string)Indv_list[i] + "') AND (tblmarker_loc.Chr <> 'X') AND  (tblmarker_loc.Chr <> 'Y') AND (tblmarker_loc.included = 1) ORDER BY tblsnp.marker_id";
                    command.CommandTimeout = 300000;
                    Reader = command.ExecuteReader();
                    while (Reader.Read())
                    {
                        Data_Set1.Add(Reader.GetValue(0).ToString());
                    }
                    Reader.Close();
                    conn.Close();
                    conn.Open();
                    System.Console.WriteLine("individual " + i + " Started");
                    System.IO.StreamWriter Indv_File = new System.IO.StreamWriter(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\" + (string)Indv_list[i] + ".txt");
                    for (int j = i; j < Indv_list.Count; j++)
                    {
                        if (i != j)
                        {
                            Data_Set2.Clear();
                            command.CommandText = @"SELECT tblsnp.Genotype
                                    FROM tblsnp INNER JOIN
                                    tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                    INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                    WHERE (tblindividual_sample.Indv_Id = '" + (string)Indv_list[j] + "') AND (tblmarker_loc.Chr <> 'X') AND  (tblmarker_loc.Chr <> 'Y') AND (tblmarker_loc.included = 1) ORDER BY tblsnp.marker_id";
                            command.CommandTimeout = 300000;
                            Reader = command.ExecuteReader();
                            while (Reader.Read())
                            {
                                Data_Set2.Add(Reader.GetValue(0).ToString());
                            }
                            Reader.Close();
                            conn.Close();
                            conn.Open();
                            Parent_offspring_test((string)Indv_list[i], (string)Indv_list[j], Data_Set1, Data_Set2, Indv_File);
                        }
                    }
                    Indv_File.Flush();
                    Indv_File.Close();
                    System.Console.WriteLine("individual " + i + " Finished");
                }
                conn.Close();

            }

            public static void Parental_Conflict_Fast()
            {
                ////////////
                int markers = 48137;
                int Conflict = 0;
                int Missing_Genotype = 0;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Call_Rate, tblindividual.Gender, tblindividual.Age, tblindividual.Birth_Date, tblindividual.Line, tblindividual.Location, 
                                    tblindividual.Included 
                                    FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id 
                                    WHERE (tblindividual.Included = 1)";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                conn.Open();

                for (int i = 0; i < Indv_list.Count; i++)
                {
                    System.Console.WriteLine("individual " + i + " Started");
                    for (int j = i; j < Indv_list.Count; j++)
                    {
                        if (i != j)
                        {
                            Conflict = 0;
                            Missing_Genotype = 0;
                            string text = "";
                            int counter = 0;
                            command.CommandText = @"select count(*) from
                                            (SELECT tblindividual_sample.Indv_Id, tblindividual_sample.Sample_Id, tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                                            FROM tblsnp INNER JOIN
                                            tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                            INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                            WHERE (tblindividual_sample.Indv_Id = '" + Indv_list[i].ToString() + @"')
                                            AND (tblmarker_loc.Chr <> 'X') 
                                            AND  (tblmarker_loc.Chr <> 'Y')
                                            AND (tblmarker_loc.included = 1)
                                            order by tblsnp.marker_id) as tblderived1 
                                            left join (SELECT tblindividual_sample.Indv_Id, tblindividual_sample.Sample_Id, tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                                            FROM tblsnp INNER JOIN
                                            tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                            INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                            WHERE (tblindividual_sample.Indv_Id = '" + Indv_list[j].ToString() + @"')
                                            AND (tblmarker_loc.Chr <> 'X') 
                                            AND  (tblmarker_loc.Chr <> 'Y')
                                            AND (tblmarker_loc.included = 1)
                                            order by tblsnp.marker_id) as tblderived2 on tblderived1.Marker_Id=tblderived2.Marker_Id
                                            where (tblderived1.Genotype = 'AA' and tblderived2.Genotype = 'BB' )
                                            OR (tblderived1.Genotype = 'BB' and tblderived2.Genotype = 'AA')
                                            group by tblderived1.Genotype = 'NC';";
                            Reader = command.ExecuteReader();
                            while (Reader.Read())
                            {
                                if (counter == 0)
                                {
                                    Conflict = int.Parse(Reader.GetValue(0).ToString());
                                    counter++;
                                }
                                else
                                {
                                    Missing_Genotype = int.Parse(Reader.GetValue(0).ToString());
                                }
                            }
                            Reader.Close();
                            conn.Close();
                            conn.Open();
                            double Conflict_Ratio = (double)Conflict / (markers - Missing_Genotype);
                            text = Indv_list[i].ToString() + "\t" + Indv_list[j].ToString() + "\t" + Conflict.ToString() + "\t" + (markers - Missing_Genotype).ToString() + "\t" + Conflict_Ratio.ToString() + "\n";
                            System.IO.File.AppendAllText(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Parental_Conflict\" + Indv_list[i].ToString() + ".txt", text);

                        }

                    }
                }

            }

            public static void Exact_Test()
            {
                /////////////////////////////////////         Exact_Test      ///////////////////////////////////
                char[] delimiters = new char[] { '\t' };
                int count_exact = 0;
                string line = "";
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\HWE.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        if (count_exact != 0)
                        {
                            
                            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            Marker_Loc Marker = new Marker_Loc();
                            Marker.Marker_Id = line_parts[0];
                            Marker.Exact_Test = double.Parse(line_parts[1]);

                            Update_Exact_Test2(Marker);
                            System.Console.WriteLine("Marker " + Marker.Marker_Id + " Updated");
                        }
                        count_exact++;
                    }
                }
            }

            public static void exclude_marker_exact_test()
            {
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"select marker_id,exact_test from tblmarker_loc where included = 1 AND chr <> 'X' AND chr <> 'Y'";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                int counter = 0;
                while (Reader.Read())
                {
                    if (Reader.GetValue(1).ToString() != null || Reader.GetValue(1).ToString().CompareTo("") != 0)
                    {
                        double x = double.Parse(Reader.GetValue(1).ToString());
                        if (x >= 1.04e-6)
                        {
                            System.Console.WriteLine(Reader.GetValue(0).ToString());
                            //Exclude_Marker(Reader.GetValue(0).ToString());
                            counter++;
                        }
                    }
                    else
                    {
                        System.Console.WriteLine(Reader.GetValue(0).ToString());
                        //Exclude_Marker(Reader.GetValue(0).ToString());
                        counter++;
                    }
                }
                System.Console.WriteLine(counter);
                conn.Close();
                Reader.Close();
            }

            public static void Update_Founder(Individual indv)
            {
                string ConString = "SERVER=localhost;" +
                                "DATABASE=db_Proc;" +
                                "UID=root;" +
                                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                string updateSql = "Update db_proc.tblindividual set founder = " + indv.Founder + " where Indv_Id = '" + indv.Indv_Id + "';";
                MySqlCommand command = new MySqlCommand(updateSql, conn);
                command.ExecuteNonQuery();
                //Reader.Close();
                conn.Close();
            }

            //public static void excute_founder()
            //{
            //    string ConString = "SERVER=localhost;" +
            //            "DATABASE=db_Proc;" +
            //            "UID=root;" +
            //            "PASSWORD=mahmoud;";
            //    MySqlConnection conn = new MySqlConnection(ConString);
            //    conn.Open();
            //    MySqlCommand command = conn.CreateCommand();
            //    command.CommandTimeout = 300000;
            //    MySqlDataReader Reader;
            //    command.CommandText = "select indv_id,paternal_id,maternal_id from tblpedigree";
            //    Reader = command.ExecuteReader();
            //    string[] Genotype = { "", "" };

            //    while (Reader.Read())
            //    {
            //        string paternal_id = Reader.GetValue(1).ToString();
            //        string maternal_id = Reader.GetValue(2).ToString();

            //        if (paternal_id.CompareTo("General") == 0)
            //        {
            //            paternal_id = "0";
            //        }
            //        else
            //        {
            //            if (sample_exists(paternal_id) == 0)
            //            {
            //                paternal_id = "0";
            //            }
            //        }


            //        if (maternal_id.CompareTo("General") == 0)
            //        {
            //            maternal_id = "0";
            //        }
            //        else
            //        {
            //            if (sample_exists(maternal_id) == 0)
            //            {
            //                maternal_id = "0";
            //            }
            //        }

            //        if (maternal_id.CompareTo("0") == 0 && paternal_id.CompareTo("0") == 0)
            //        {
            //            Individual indv = new Individual();
            //            indv.Indv_Id = Reader.GetValue(0).ToString();
            //            indv.Founder = 1;
            //            Update_Founder(indv);
            //        }
            //        else
            //        {
            //            Individual indv = new Individual();
            //            indv.Indv_Id = Reader.GetValue(0).ToString();
            //            indv.Founder = 0;
            //            Update_Founder(indv);
            //        }

            //    }

            //}

            public static void test()
            {
                char[] delimiters = new char[] { ' ' };
                int count_exact = 0;
                string line = "";
                int homoc = 0;
                int homor = 0;
                int heter = 0;
                using (StreamReader file = new StreamReader("C:/Users/Elansary/Documents/Visual Studio 2010/Projects/Porc/porc.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        if (count_exact != 0)
                        {
                            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (int.Parse(line_parts[98808]) == 1 && int.Parse(line_parts[98809]) == 1)
                            {
                                homoc++;
                            }
                            else if (int.Parse(line_parts[98808]) == 3 && int.Parse(line_parts[98809]) == 3)
                            {
                                homor++;
                            }
                            else
                            {
                                heter++;
                            }
                        }
                        count_exact++;
                    }
                }
            }

            public static void testing()
            {

                string Genotype_offspring = "00";
                string Genotype_Paternal = "00";
                string Genotype_Maternal = "00";
                int trA = 0;
                int trB = 0;
                int unA = 0;
                int unB = 0;
                int T_A = 0;
                int U_A = 0;
                int t1 = 0;
                int t2 = 0;
                int Conflict = 0;
                int Missing = 0;
                string line = "";
                int counter = 0;
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\plink latest data\Test\new2.ped"))
                {//

                    while ((line = file.ReadLine()) != null)
                    {
                        char[] delimiters = new char[] { ' ' };
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        if (line_parts[5].CompareTo("2") == 0)
                        {
                            Genotype_offspring = line_parts[6] + line_parts[7];
                            string line2 = "";
                            using (StreamReader file2 = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\plink latest data\Test\new2.ped"))
                            {//
                                Genotype_Paternal = "00";
                                while ((line2 = file2.ReadLine()) != null)
                                {
                                    string[] line2_parts = line2.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                                    if (line2_parts[1].CompareTo(line_parts[2]) == 0)
                                    {
                                        Genotype_Paternal = line2_parts[6] + line2_parts[7];
                                        break;
                                    }
                                }

                            }
                            string line3 = "";
                            using (StreamReader file3 = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\plink latest data\Test\new2.ped"))
                            {//
                                Genotype_Maternal = "00";
                                while ((line3 = file3.ReadLine()) != null)
                                {
                                    string[] line3_parts = line3.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);


                                    if (line3_parts[1].CompareTo(line_parts[3]) == 0)
                                    {
                                        Genotype_Maternal = line3_parts[6] + line3_parts[7];
                                        break;
                                    }
                                }

                            }

                            if (line_parts[1].CompareTo("BA001263") == 0)
                            {
                                int o = 1;
                            }

                            if (Genotype_offspring.CompareTo("00") != 0 && Genotype_Maternal.CompareTo("00") != 0 && Genotype_Paternal.CompareTo("00") != 0)
                            {

                                //// Kid is 00

                                //if (Genotype_offspring.CompareTo("AA") == 0)
                                //{
                                //    if ((Genotype_Paternal.CompareTo("AB") == 0) && (Genotype_Maternal.CompareTo("AB") == 0))
                                //    { trA = 1; unA = 2; trB = 1; unB = 2; }
                                //    else
                                //    { trA = 1; unA = 2; }
                                //}
                                //else if (Genotype_offspring.CompareTo("AB") == 0)  // Kid is 01
                                //{
                                //    // het dad
                                //    if (Genotype_Paternal.CompareTo("AB") == 0)
                                //    {
                                //        // het mum
                                //        if (Genotype_Maternal.CompareTo("AB") == 0)
                                //        { trA = 1; trB = 2; unA = 2; unB = 1; }
                                //        else if (Genotype_Maternal.CompareTo("AA") == 0)
                                //        { trA = 2; unA = 1; }
                                //        else { trA = 1; unA = 2; }
                                //    }
                                //    else if (Genotype_Paternal.CompareTo("AA") == 0)
                                //    {
                                //        trA = 2; unA = 1;
                                //    }
                                //    else
                                //    {
                                //        trA = 1; unA = 2;
                                //    }
                                //}
                                //else // kid is 1/1
                                //{

                                //    if (Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("AB") == 0)
                                //    { trA = 2; unA = 1; trB = 2; unB = 1; }
                                //    else
                                //    {
                                //        trA = 2; unA = 1;
                                //    }
                                //}


                                if ((Genotype_Paternal.CompareTo("13") == 0 && Genotype_Maternal.CompareTo("33") == 0) || (Genotype_Maternal.CompareTo("13") == 0 && Genotype_Paternal.CompareTo("33") == 0))
                                {
                                    switch (Genotype_offspring)
                                    {
                                        case "13":
                                            T_A++;

                                            break;
                                        case "33":
                                            U_A++;

                                            break;
                                        case "11":
                                            Conflict++;
                                            break;

                                    }
                                }
                                else if ((Genotype_Paternal.CompareTo("13") == 0 && Genotype_Maternal.CompareTo("11") == 0) || (Genotype_Maternal.CompareTo("13") == 0 && Genotype_Paternal.CompareTo("11") == 0))
                                {
                                    switch (Genotype_offspring)
                                    {
                                        case "11":
                                            T_A++;

                                            break;
                                        case "13":
                                            U_A++;

                                            break;
                                        case "33":
                                            Conflict++;
                                            break;
                                    }
                                }

                                else if (Genotype_Paternal.CompareTo("13") == 0 && Genotype_Maternal.CompareTo("13") == 0)
                                {
                                    switch (Genotype_offspring)
                                    {
                                        case "11":
                                            T_A = T_A + 2;
                                            break;
                                        case "13":
                                            T_A++;
                                            U_A++;
                                            break;
                                        case "33":
                                            U_A = U_A + 2;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                Missing++;
                            }

                            //if (trA == 1) t1++;
                            //if (trB == 1) t1++;
                            //if (trA == 2) t2++;
                            //if (trB == 2) t2++;
                        }
                        counter++;
                    }
                }

                double TDT_A = (double)(Math.Pow((t1 - t2), 2)) / (double)(t1 + t2);

            }


            public static string Marker_Genotype(string Sample_Id, string Marker_Id)
            {
                string Genotype = "";
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT Genotype FROM  tblsnp
                                WHERE (Sample_Id = '" + Sample_Id + @"')
                                AND (Marker_Id = '" + Marker_Id + @"')";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Genotype = Reader.GetValue(0).ToString();
                }
                Reader.Close();
                conn.Close();
                return Genotype;
            }

            public static string Marker_Genotype(string Indv_Id, int Marker_Id)
            {
                string Genotype = "";
                foreach (string line in Indv_List)
                {
                    char[] delimiters = new char[] { ' ' };
                    string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (line_parts[1].ToString().CompareTo(Indv_Id) == 0)
                    {
                        Genotype = Data_Set[int.Parse(line_parts[0].ToString()), Marker_Id];
                        break;
                    }
                }
                return Genotype;
            }

            public static bool exists(string indv_id, ArrayList Indv_List)
            {
                bool exist = false;

                foreach (string indv in Indv_List)
                {
                    //char[] delimiters = new char[] { ' ' };
                    //string[] line_parts = indv.ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (indv.CompareTo(indv_id) == 0)
                    {
                        exist = true;
                    }
                }
                return exist;
            }
            public static bool exists(string indv_id, ArrayList Indv_List, int function)
            {
                bool exist = false;

                foreach (string indv in Indv_List)
                {
                    if (indv.CompareTo(indv_id) == 0)
                    {
                        exist = true;
                    }
                }
                return exist;
            }

            public static string[,] Data_Set = new string[861, 62165];  // after removing animals with less than 95% call rate and removing markers with less than 98% and HWD 
            public static int[] Data_Set_Conflict = new int[55720];
            public static ArrayList Data_Set_map = new ArrayList();
            public static string[,] Data_Set_Founders = new string[331, 62169];
            public static ArrayList Data = new ArrayList();
            public static ArrayList Indv_List = new ArrayList();

            public static void print_pedigree()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = "select CHR,Marker_Id,position from tblmarker_loc where chr <> 'X'  and chr <> 'Y' and included = 1  order by 0+chr,position,marker_id";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Marker_List.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString());
                }
                Reader.Close();
                conn.Close();
                conn.Open();
                System.IO.StreamWriter output_file_tdt = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\TDT.txt");
                //System.IO.StreamWriter output_file_errors = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Errors_MARC0025211_RG.txt");
                //output_file_errors.WriteLine("Indv_Id" + "\t" + "Paternal_Id" + "\t" + "Maternal_Id" + "\t" + "Offspring Genotype" + "\t" + "Paternal Genotype" + "\t" + "Maternal Genotype" + "\t" + "T" + "\t" + "U");
                command.CommandText = @"SELECT tblindividual.Indv_Id, Sample_ID, tblpedigree.Maternal_Id_Modified,
                                (select Sample_ID from tblindividual  
                                INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id
                                Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                where tblindividual.indv_id = tblpedigree.Maternal_Id_Modified and tblindividual.included = 1 and tblindividual.call_rate is not null ) AS Maternal_sample_id,
                                tblpedigree.Paternal_Id_Modified,
                                (select Sample_ID from tblindividual  
                                INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id
                                Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                where tblindividual.indv_id = tblpedigree.Paternal_Id_Modified and tblindividual.included = 1 and tblindividual.call_rate is not null ) AS Paternal_sample_id,
                                tblindividual_phenotype.phenotype_ID
                                FROM tblindividual 
                                INNER JOIN
                                tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                                inner join tblindividual_phenotype on  tblindividual.indv_id = tblindividual_phenotype.indv_id 
                                Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                WHERE tblindividual.included = 1
                                and tblindividual.call_rate is not null
                                and (Maternal_id != 'General' and Paternal_id != 'General') 
                                and tblindividual_phenotype.Phenotype_Id = 2
                                Having Maternal_sample_id is not  null and  Paternal_sample_id is  not null";


                Reader = command.ExecuteReader();
                int counter = 0;
                while (Reader.Read())
                {
                    string Indv_Id = Reader.GetValue(0).ToString();
                    string Paternal_Id = Reader.GetValue(4).ToString();
                    string Maternal_Id = Reader.GetValue(2).ToString();
                    //string Paternal_sample_id = Reader.GetValue(5).ToString();
                    //string Maternal_sample_id = Reader.GetValue(3).ToString();
                    Data.Add(Indv_Id + " " + Paternal_Id + " " + Maternal_Id);
                    if (!exists(Indv_Id, Indv_List))
                    {
                        Indv_List.Add(counter + " " + Indv_Id);
                        counter++;
                    }
                    if (!exists(Paternal_Id, Indv_List))
                    {
                        Indv_List.Add(counter + " " + Paternal_Id);
                        counter++;
                    }
                    if (!exists(Maternal_Id, Indv_List))
                    {
                        Indv_List.Add(counter + " " + Maternal_Id);
                        counter++;
                    }

                }
                Reader.Close();
                conn.Close();
                conn.Open();





                for (int i = 0; i < Indv_List.Count; i++)
                {
                    char[] delimiters = new char[] { ' ' };
                    string[] line_parts = Indv_List[i].ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    command.CommandText = @"SELECT tblindividual_sample.Indv_Id, tblindividual_sample.Sample_Id, tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                                                        FROM tblsnp INNER JOIN
                                                        tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                                        INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                                        WHERE (tblindividual_sample.Indv_Id = '" + line_parts[1].ToString() + "') AND (tblmarker_loc.Included = 1) AND  (tblmarker_loc.Chr <> 'X') AND  (tblmarker_loc.Chr <> 'Y') ORDER BY 0+tblmarker_loc.chr,tblmarker_loc.position,tblmarker_loc.marker_id";
                    Reader = command.ExecuteReader();
                    int j = 0;
                    while (Reader.Read())
                    {

                        // Data_Set[i, j] = (Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(1).ToString() + "\t" + Reader.GetValue(2).ToString() + "\t" + Reader.GetValue(3).ToString() + "\t" + Reader.GetValue(4).ToString());
                        Data_Set[i, j] = Reader.GetValue(3).ToString();
                        j++;

                    }
                    Reader.Close();
                    conn.Close();
                    conn.Open();
                }
                conn.Close();


                for (int i = 0; i < Marker_List.Count; i++)
                {
                    if (i % 1000 == 0)
                    {
                        System.Console.WriteLine("Marker number " + i);
                    }
                    int T_A = 0;
                    int U_A = 0;
                    int Conflict = 0;
                    int Missing = 0;


                    ArrayList inserted = new ArrayList();
                    foreach (string line in Data)
                    {
                        char[] delimiters = new char[] { ' ' };
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        string indv_id = line_parts[0].ToString();
                        string Paternal_id = line_parts[1].ToString();
                        string Maternal_id = line_parts[2].ToString();
                        string Genotype_offspring = Marker_Genotype(indv_id, i);
                        string Genotype_Paternal = Marker_Genotype(Paternal_id, i);
                        string Genotype_Maternal = Marker_Genotype(Maternal_id, i);


                        if (Genotype_offspring.CompareTo("NC") != 0 && Genotype_Maternal.CompareTo("NC") != 0 && Genotype_Paternal.CompareTo("NC") != 0)
                        {



                            if ((Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("BB") == 0) || (Genotype_Maternal.CompareTo("AB") == 0 && Genotype_Paternal.CompareTo("BB") == 0))
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AB":
                                        T_A++;

                                        break;
                                    case "BB":
                                        U_A++;

                                        break;
                                    case "AA":
                                        Conflict++;
                                        break;

                                }
                            }
                            else if ((Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("AA") == 0) || (Genotype_Maternal.CompareTo("AB") == 0 && Genotype_Paternal.CompareTo("AA") == 0))
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AA":
                                        T_A++;

                                        break;
                                    case "AB":
                                        U_A++;

                                        break;
                                    case "BB":
                                        Conflict++;
                                        break;
                                }
                            }

                            else if (Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("AB") == 0)
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AA":
                                        T_A = T_A + 2;
                                        break;
                                    case "AB":
                                        T_A++;
                                        U_A++;
                                        break;
                                    case "BB":
                                        U_A = U_A + 2;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Missing++;
                        }

                        //if (trA == 1) t1++;
                        //if (trB == 1) t1++;
                        //if (trA == 2) t2++;
                        //if (trB == 2) t2++;
                        //output_file_errors.WriteLine(Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(4).ToString() + "\t" + Reader.GetValue(2).ToString() + "\t" + Genotype_offspring + "\t" + Genotype_Paternal + "\t" + Genotype_Maternal + "\t" + T_A + "\t" + U_A);

                    }
                    //output_file_errors.Flush();
                    //output_file_errors.Close();
                    double TDT = (double)(Math.Pow((T_A - U_A), 2)) / (double)(T_A + U_A);
                    double P_value = 1 - SpecialFunction.chisq(1, TDT);
                    //char[] delimiters_space = new char[] { ' ' };
                    //string[] line_parts_marker = Marker_List[i].ToString().Split(delimiters_space, StringSplitOptions.RemoveEmptyEntries);
                    output_file_tdt.WriteLine(Marker_List[i].ToString() + "\t" + T_A.ToString() + "\t" + U_A.ToString() + "\t" + TDT.ToString() + "\t" + P_value.ToString());
                }
                output_file_tdt.Flush();
                output_file_tdt.Close();

            }

            public static void print_pedigree_2()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = "select CHR,Marker_Id,position from tblmarker_loc where chr <> 'X'  and chr <> 'Y' and included = 1  order by 0+chr,position,marker_id";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Marker_List.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString());
                }
                Reader.Close();
                conn.Close();

                System.IO.StreamWriter output_file_tdt = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\TDT.txt");
                int counter = 0;
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\defect_6_low.txt"))
                {//
                    string line = "";
                    while ((line = file.ReadLine()) != null)
                    {
                        string Indv_Id = line.Split('\t')[0];
                        string Paternal_Id = line.Split('\t')[1];
                        string Maternal_Id = line.Split('\t')[2];

                        Data.Add(Indv_Id + " " + Paternal_Id + " " + Maternal_Id);
                        if (!exists(Indv_Id, Indv_List))
                        {
                            Indv_List.Add(counter + " " + Indv_Id);
                            counter++;
                        }
                        if (!exists(Paternal_Id, Indv_List))
                        {
                            Indv_List.Add(counter + " " + Paternal_Id);
                            counter++;
                        }
                        if (!exists(Maternal_Id, Indv_List))
                        {
                            Indv_List.Add(counter + " " + Maternal_Id);
                            counter++;
                        }


                    }
                }





                conn.Open();
                for (int i = 0; i < Indv_List.Count; i++)
                {
                    System.Console.WriteLine(i);
                    char[] delimiters = new char[] { ' ' };
                    string[] line_parts = Indv_List[i].ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    command.CommandText = @"SELECT tblindividual_sample.Indv_Id, tblindividual_sample.Sample_Id, tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                                                        FROM tblsnp INNER JOIN
                                                        tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                                        INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                                        WHERE (tblindividual_sample.Indv_Id = '" + line_parts[1].ToString() + "') AND (tblmarker_loc.Included = 1) AND  (tblmarker_loc.Chr <> 'X') AND  (tblmarker_loc.Chr <> 'Y') ORDER BY 0+tblmarker_loc.chr,tblmarker_loc.position,tblmarker_loc.marker_id";
                    Reader = command.ExecuteReader();
                    int j = 0;
                    while (Reader.Read())
                    {

                        // Data_Set[i, j] = (Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(1).ToString() + "\t" + Reader.GetValue(2).ToString() + "\t" + Reader.GetValue(3).ToString() + "\t" + Reader.GetValue(4).ToString());
                        Data_Set[i, j] = Reader.GetValue(3).ToString();
                        j++;

                    }
                    Reader.Close();
                    conn.Close();
                    conn.Open();
                }
                conn.Close();


                for (int i = 0; i < Marker_List.Count; i++)
                {
                    if (i % 1000 == 0)
                    {
                        System.Console.WriteLine("Marker number " + i);
                    }
                    int T_A = 0;
                    int U_A = 0;
                    int Conflict = 0;
                    int Missing = 0;


                    ArrayList inserted = new ArrayList();
                    foreach (string line in Data)
                    {
                        char[] delimiters = new char[] { ' ' };
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        string indv_id = line_parts[0].ToString();
                        string Paternal_id = line_parts[1].ToString();
                        string Maternal_id = line_parts[2].ToString();
                        string Genotype_offspring = Marker_Genotype(indv_id, i);
                        string Genotype_Paternal = Marker_Genotype(Paternal_id, i);
                        string Genotype_Maternal = Marker_Genotype(Maternal_id, i);


                        if (Genotype_offspring.CompareTo("NC") != 0 && Genotype_Maternal.CompareTo("NC") != 0 && Genotype_Paternal.CompareTo("NC") != 0)
                        {



                            if ((Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("BB") == 0) || (Genotype_Maternal.CompareTo("AB") == 0 && Genotype_Paternal.CompareTo("BB") == 0))
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AB":
                                        T_A++;

                                        break;
                                    case "BB":
                                        U_A++;

                                        break;
                                    case "AA":
                                        Conflict++;
                                        break;

                                }
                            }
                            else if ((Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("AA") == 0) || (Genotype_Maternal.CompareTo("AB") == 0 && Genotype_Paternal.CompareTo("AA") == 0))
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AA":
                                        T_A++;

                                        break;
                                    case "AB":
                                        U_A++;

                                        break;
                                    case "BB":
                                        Conflict++;
                                        break;
                                }
                            }

                            else if (Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("AB") == 0)
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AA":
                                        T_A = T_A + 2;
                                        break;
                                    case "AB":
                                        T_A++;
                                        U_A++;
                                        break;
                                    case "BB":
                                        U_A = U_A + 2;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Missing++;
                        }

                        //if (trA == 1) t1++;
                        //if (trB == 1) t1++;
                        //if (trA == 2) t2++;
                        //if (trB == 2) t2++;
                        //output_file_errors.WriteLine(Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(4).ToString() + "\t" + Reader.GetValue(2).ToString() + "\t" + Genotype_offspring + "\t" + Genotype_Paternal + "\t" + Genotype_Maternal + "\t" + T_A + "\t" + U_A);

                    }
                    //output_file_errors.Flush();
                    //output_file_errors.Close();
                    double TDT = (double)(Math.Pow((T_A - U_A), 2)) / (double)(T_A + U_A);
                    double P_value = 1 - SpecialFunction.chisq(1, TDT);
                    //char[] delimiters_space = new char[] { ' ' };
                    //string[] line_parts_marker = Marker_List[i].ToString().Split(delimiters_space, StringSplitOptions.RemoveEmptyEntries);
                    output_file_tdt.WriteLine(Marker_List[i].ToString() + "\t" + T_A.ToString() + "\t" + U_A.ToString() + "\t" + TDT.ToString() + "\t" + P_value.ToString());
                }
                output_file_tdt.Flush();
                output_file_tdt.Close();

            }

            public static void Marker_Error(string Marker_ID)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = "select CHR,Marker_Id,position from tblmarker_loc where chr <> 'X'  and chr <> 'Y' and included = 1 and marker_Id ='" + Marker_ID + @"'  order by 0+chr,position,marker_id";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                while (Reader.Read())
                {
                    Marker_List.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString());
                }
                Reader.Close();
                conn.Close();

                System.IO.StreamWriter output_file_tdt = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\TDT.txt");
                System.IO.StreamWriter output_file_errors = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Errors_" + Marker_ID + "_SR.txt");
                //output_file_errors.WriteLine("Indv_Id" + "\t" + "Paternal_Id" + "\t" + "Maternal_Id" + "\t" + "Offspring Genotype" + "\t" + "Paternal Genotype" + "\t" + "Maternal Genotype" + "\t" + "T" + "\t" + "U");
                int counter = 0;
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\defect_6_low.txt"))
                {//
                    string line = "";
                    while ((line = file.ReadLine()) != null)
                    {
                        string Indv_Id = line.Split('\t')[0];
                        string Paternal_Id = line.Split('\t')[1];
                        string Maternal_Id = line.Split('\t')[2];

                        Data.Add(Indv_Id + " " + Paternal_Id + " " + Maternal_Id);
                        if (!exists(Indv_Id, Indv_List))
                        {
                            Indv_List.Add(counter + " " + Indv_Id);
                            counter++;
                        }
                        if (!exists(Paternal_Id, Indv_List))
                        {
                            Indv_List.Add(counter + " " + Paternal_Id);
                            counter++;
                        }
                        if (!exists(Maternal_Id, Indv_List))
                        {
                            Indv_List.Add(counter + " " + Maternal_Id);
                            counter++;
                        }


                    }
                }





                conn.Open();
                for (int i = 0; i < Indv_List.Count; i++)
                {
                    System.Console.WriteLine("Inserted " + i);
                    char[] delimiters = new char[] { ' ' };
                    string[] line_parts = Indv_List[i].ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    command.CommandText = @"SELECT tblindividual_sample.Indv_Id, tblindividual_sample.Sample_Id, tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                                                        FROM tblsnp INNER JOIN
                                                        tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id 
                                                        INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                                                        WHERE (tblindividual_sample.Indv_Id = '" + line_parts[1].ToString() + "') AND (tblmarker_loc.Included = 1) AND  (tblmarker_loc.Chr <> 'X') AND  (tblmarker_loc.Chr <> 'Y') AND tblmarker_loc.Marker_Id = '" + Marker_ID + @"' ORDER BY 0+tblmarker_loc.chr,tblmarker_loc.position,tblmarker_loc.marker_id";
                    Reader = command.ExecuteReader();
                    int j = 0;
                    while (Reader.Read())
                    {

                        // Data_Set[i, j] = (Reader.GetValue(0).ToString() + "\t" + Reader.GetValue(1).ToString() + "\t" + Reader.GetValue(2).ToString() + "\t" + Reader.GetValue(3).ToString() + "\t" + Reader.GetValue(4).ToString());
                        Data_Set[i, j] = Reader.GetValue(3).ToString();
                        j++;

                    }
                    Reader.Close();
                    conn.Close();
                    conn.Open();
                }
                conn.Close();


                for (int i = 0; i < Marker_List.Count; i++)
                {
                    if (i % 1000 == 0)
                    {
                        System.Console.WriteLine("Marker number " + i);
                    }
                    int T_A = 0;
                    int U_A = 0;
                    int Conflict = 0;
                    int Missing = 0;


                    ArrayList inserted = new ArrayList();
                    foreach (string line in Data)
                    {
                        char[] delimiters = new char[] { ' ' };
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        string indv_id = line_parts[0].ToString();
                        string Paternal_id = line_parts[1].ToString();
                        string Maternal_id = line_parts[2].ToString();
                        string Genotype_offspring = Marker_Genotype(indv_id, i);
                        string Genotype_Paternal = Marker_Genotype(Paternal_id, i);
                        string Genotype_Maternal = Marker_Genotype(Maternal_id, i);


                        if (Genotype_offspring.CompareTo("NC") != 0 && Genotype_Maternal.CompareTo("NC") != 0 && Genotype_Paternal.CompareTo("NC") != 0)
                        {



                            if ((Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("BB") == 0) || (Genotype_Maternal.CompareTo("AB") == 0 && Genotype_Paternal.CompareTo("BB") == 0))
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AB":
                                        T_A++;

                                        break;
                                    case "BB":
                                        U_A++;

                                        break;
                                    case "AA":
                                        Conflict++;
                                        break;

                                }
                            }
                            else if ((Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("AA") == 0) || (Genotype_Maternal.CompareTo("AB") == 0 && Genotype_Paternal.CompareTo("AA") == 0))
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AA":
                                        T_A++;

                                        break;
                                    case "AB":
                                        U_A++;

                                        break;
                                    case "BB":
                                        Conflict++;
                                        break;
                                }
                            }

                            else if (Genotype_Paternal.CompareTo("AB") == 0 && Genotype_Maternal.CompareTo("AB") == 0)
                            {
                                switch (Genotype_offspring)
                                {
                                    case "AA":
                                        T_A = T_A + 2;
                                        break;
                                    case "AB":
                                        T_A++;
                                        U_A++;
                                        break;
                                    case "BB":
                                        U_A = U_A + 2;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Missing++;
                        }

                        //if (trA == 1) t1++;
                        //if (trB == 1) t1++;
                        //if (trA == 2) t2++;
                        //if (trB == 2) t2++;
                        output_file_errors.WriteLine(indv_id.ToString() + "\t" + Paternal_id.ToString() + "\t" + Maternal_id.ToString() + "\t" + Genotype_offspring + "\t" + Genotype_Paternal + "\t" + Genotype_Maternal + "\t" + T_A + "\t" + U_A);

                    }
                    output_file_errors.Flush();
                    output_file_errors.Close();
                    double TDT = (double)(Math.Pow((T_A - U_A), 2)) / (double)(T_A + U_A);
                    double P_value = 1 - SpecialFunction.chisq(1, TDT);
                    //char[] delimiters_space = new char[] { ' ' };
                    //string[] line_parts_marker = Marker_List[i].ToString().Split(delimiters_space, StringSplitOptions.RemoveEmptyEntries);
                    output_file_tdt.WriteLine(Marker_List[i].ToString() + "\t" + T_A.ToString() + "\t" + U_A.ToString() + "\t" + TDT.ToString() + "\t" + P_value.ToString());
                }
                output_file_tdt.Flush();
                output_file_tdt.Close();

            }

            public static string checkdisease(string Indv_Id)
            {
                string defect = "0";
                string ConString = "SERVER=localhost;" +
                                "DATABASE=db_Proc;" +
                                "UID=root;" +
                                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual_phenotype.Phenotype_Id
                         FROM tblindividual INNER JOIN
                         tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id
                         where tblindividual.Indv_Id = '" + Indv_Id + @"'";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    defect = Reader.GetValue(0).ToString();
                }
                Reader.Close();
                conn.Close();
                return defect;
            }


            public static void change_plink_format()
            {

                System.IO.StreamWriter output_file = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\porc_SR.ped");
                //StringBuilder newFile = new StringBuilder();

                string[] file = File.ReadAllLines(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\Plink SR disease\porc.ped");
                int i = 0;
                foreach (string line in file)
                {
                    System.Console.WriteLine("line number " + i);
                    char[] delimiters = new char[] { ' ' };
                    string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string defect = checkdisease(line_parts[1]);
                    string newline = "";
                    if (defect.CompareTo("2") == 0)
                    {
                        int Replace_position = line_parts[0].Length + 1 + line_parts[1].Length + 1 + line_parts[2].Length + 1 + line_parts[3].Length + 1 + line_parts[4].Length + 1;
                        newline = line.Remove(Replace_position, 1);
                        newline = newline.Insert(Replace_position, "2");

                    }
                    else
                    {
                        int Replace_position = line_parts[0].Length + 1 + line_parts[1].Length + 1 + line_parts[2].Length + 1 + line_parts[3].Length + 1 + line_parts[4].Length + 1;
                        newline = line.Remove(Replace_position, 1);
                        newline = newline.Insert(Replace_position, "1");
                    }
                    //newFile.Append(newline + "\r\n");
                    output_file.WriteLine(newline);
                    i++;
                }
                output_file.Flush();
                output_file.Close();

                //File.WriteAllText(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\Plink RG disease\porc_SR.ped", newFile.ToString());

            }

            public static void change_plink_family()
            {

                System.IO.StreamWriter output_file = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\families\RG disease\porc_RG.ped");
                string[] file = File.ReadAllLines(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\families\RG disease\porc.ped");
                int i = 0;
                foreach (string line in file)
                {
                    System.Console.WriteLine("line number " + i);
                    char[] delimiters = new char[] { ' ' };
                    string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string newline = "";
                    //int Replace_position = line_parts[0].Length + 1 + line_parts[1].Length + 1 + line_parts[2].Length + 1 + line_parts[3].Length + 1 + line_parts[4].Length + 1;
                    newline = line.Remove(0, 2);
                    newline = newline.Insert(0, "1");
                    output_file.WriteLine(newline);
                    i++;
                }
                output_file.Flush();
                output_file.Close();
            }

            public static void Marker_frequency(string Marker_Id)
            {

                int AA_count = 0;
                int AB_count = 0;
                int BB_count = 0;
                int NC_count = 0;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 3000000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblsnp.Genotype, COUNT(*) AS Expr1
                         FROM tblsnp INNER JOIN
                         tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN
                         tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id
                         WHERE (tblsnp.Marker_Id = '" + Marker_Id + @"') AND (tblindividual.Included = 1)  
                         GROUP BY tblsnp.Genotype";

                //(select indv_id from tblindividual_phenotype where phenotype_id = 8)) control animals
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    switch (Reader.GetValue(0).ToString())
                    {
                        case "NC":
                            NC_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "AA":
                            AA_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "BB":
                            BB_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                        case "AB":
                            AB_count = int.Parse(Reader.GetValue(1).ToString());
                            break;
                    }
                }
                conn.Close();
                Reader.Close();
                double AA_Frequency;
                double BB_Frequency;
                double AB_Frequency;
                double Allele_A_Frequency;
                double Allele_B_Frequency;
                double AA_Expected;
                double BB_Expected;
                double AB_Expected;
                double chi2;
                double P_Value;
                if (AA_count > 0 || AB_count > 0 || BB_count > 0)
                {
                    AA_Frequency = (double)AA_count / ((AA_count + AB_count + BB_count) - NC_count);
                    BB_Frequency = (double)BB_count / ((AA_count + AB_count + BB_count) - NC_count);
                    AB_Frequency = (double)AB_count / ((AA_count + AB_count + BB_count) - NC_count);
                    Allele_A_Frequency = AA_Frequency + (0.5 * AB_Frequency);
                    Allele_B_Frequency = 1 - Allele_A_Frequency;
                    AA_Expected = Math.Pow(Allele_A_Frequency, 2) * ((AA_count + AB_count + BB_count) - NC_count);
                    BB_Expected = Math.Pow(Allele_B_Frequency, 2) * ((AA_count + AB_count + BB_count) - NC_count);
                    AB_Expected = 2 * Allele_A_Frequency * Allele_B_Frequency * ((AA_count + AB_count + BB_count) - NC_count);
                    chi2 = (double)Math.Pow((AA_count - AA_Expected), 2) / AA_Expected + (double)Math.Pow((BB_count - BB_Expected), 2) / BB_Expected + (double)Math.Pow((AB_count - AB_Expected), 2) / AB_Expected;
                    P_Value = chisqr(1, chi2);
                }
                else
                {
                    AA_Frequency = 0;
                    BB_Frequency = 0;
                    AB_Frequency = 0;
                    Allele_A_Frequency = 0;
                    Allele_B_Frequency = 0;
                    P_Value = 0;
                }

                Marker_Loc Marker = new Marker_Loc();
                Marker.Marker_Id = Marker_Id;
                Marker.A_Freq = Allele_A_Frequency;
                Marker.B_Freq = Allele_B_Frequency;
                Marker.AA_Freq = AA_Frequency;
                Marker.BB_Freq = BB_Frequency;
                Marker.AB_Freq = AB_Frequency;
                Marker.Chi2_P_Value = P_Value;

                System.Console.WriteLine(Marker.Marker_Id);
                System.Console.WriteLine(Marker.B_Freq);
                System.Console.WriteLine(Marker.A_Freq);
            }

            public static void fun(string filename)
            {
                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };
                int count = 0;
                string line = "";
                ArrayList Trios = new ArrayList();
                bool flag = false;
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\" + filename + ".txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < Trios.Count; j++)
                        {
                            string[] Trios_part = Trios[j].ToString().Split(delimiters_space, StringSplitOptions.RemoveEmptyEntries);
                            string test = Trios_part[0] + " " + Trios_part[1] + " " + Trios_part[2];
                            if (test.ToString().CompareTo(line_parts[3] + " " + line_parts[4] + " " + line_parts[5]) == 0)
                            {
                                Trios[j] = Trios_part[0] + " " + Trios_part[1] + " " + Trios_part[2] + " " + (int.Parse(Trios_part[3]) + 1).ToString();
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            Trios.Add(line_parts[3] + " " + line_parts[4] + " " + line_parts[5] + " 1");

                        }
                        flag = false;
                    }

                }
                System.IO.StreamWriter output_file_tdt = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Trios_" + filename + ".txt");
                foreach (string L in Trios)
                {
                    output_file_tdt.WriteLine(L);
                }
                output_file_tdt.Flush();
                output_file_tdt.Close();
            }

            public static string check_sex(string Indv_Id)
            {
                string sex = "";
                string ConString = "SERVER=localhost;" +
                    "DATABASE=db_Proc;" +
                    "UID=root;" +
                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT Gender
                                    FROM tblindividual
                                    WHERE (Call_Rate IS NOT NULL) AND (Indv_Id ='" + Indv_Id + @"')
                                    ORDER BY Indv_Id";
                Reader = command.ExecuteReader();
                ArrayList List = new ArrayList();
                while (Reader.Read())
                {
                    sex = Reader.GetValue(0).ToString();
                }
                conn.Close();
                Reader.Close();
                return sex;
            }

            public static void defect_6_low()
            {
                string line = "";
                char[] delimiters = new char[] { '\t' };
                ArrayList my_list = new ArrayList();
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\defect_6_low.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        my_list.Add(line);
                    }
                }
                System.IO.StreamWriter output_file_pairs = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\defect_6_low_pairs.txt");
                for (int j = 0; j < my_list.Count; j++)
                {
                    System.Console.WriteLine("inserted " + j);
                    string Indv_Id = "";
                    string father = "";
                    string mother = "";
                    string line_father = "";
                    string line_mother = "";
                    using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Revolution\QTL_Mapping\Parental conflict data\All Lines\conflict.txt"))
                    {//
                        while ((line = file.ReadLine()) != null)
                        {
                            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            string Indv_Id_1 = line_parts[0];
                            string Indv_Id_2 = line_parts[1];
                            Indv_Id = my_list[j].ToString().Split('\t')[0];
                            father = my_list[j].ToString().Split('\t')[1];
                            mother = my_list[j].ToString().Split('\t')[2];
                            double conflict = double.Parse(line_parts[4]);

                            if ((Indv_Id.CompareTo(Indv_Id_1) == 0 && father.CompareTo(Indv_Id_2) == 0) || (Indv_Id.CompareTo(Indv_Id_2) == 0 && father.CompareTo(Indv_Id_1) == 0))
                            {
                                line_father = Indv_Id + " " + father + " " + conflict;

                            }
                            else if ((Indv_Id.CompareTo(Indv_Id_1) == 0 && mother.CompareTo(Indv_Id_2) == 0) || (Indv_Id.CompareTo(Indv_Id_2) == 0 && mother.CompareTo(Indv_Id_1) == 0))
                            {
                                line_mother = Indv_Id + " " + mother + " " + conflict;
                            }
                        }
                    }
                    output_file_pairs.WriteLine(line_father + " " + line_mother);
                }
                output_file_pairs.Flush();
                output_file_pairs.Close();
            }

            public static void defect_6_high()
            {
                string line = "";
                char[] delimiters = new char[] { '\t' };
                ArrayList my_list = new ArrayList();
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\defect_6_high.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        my_list.Add(line);
                    }
                }
                System.IO.StreamWriter output_file_pairs = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\defect_6_high_pairs.txt");
                for (int j = 0; j < my_list.Count; j++)
                {
                    System.Console.WriteLine("inserted " + j);
                    string Indv_Id = "";
                    string father = "";
                    string mother = "";
                    string line_father = "";
                    string line_mother = "";
                    using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Revolution\QTL_Mapping\Parental conflict data\All Lines New\conflict.txt"))
                    {//
                        while ((line = file.ReadLine()) != null)
                        {
                            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            string Indv_Id_1 = line_parts[0];
                            string Indv_Id_2 = line_parts[1];
                            Indv_Id = my_list[j].ToString().Split('\t')[0];
                            father = my_list[j].ToString().Split('\t')[1];
                            mother = my_list[j].ToString().Split('\t')[2];
                            double conflict = double.Parse(line_parts[4]);

                            if ((Indv_Id.CompareTo(Indv_Id_1) == 0 && father.CompareTo(Indv_Id_2) == 0) || (Indv_Id.CompareTo(Indv_Id_2) == 0 && father.CompareTo(Indv_Id_1) == 0))
                            {
                                line_father = Indv_Id + " " + father + " " + conflict;

                            }
                            else if ((Indv_Id.CompareTo(Indv_Id_1) == 0 && mother.CompareTo(Indv_Id_2) == 0) || (Indv_Id.CompareTo(Indv_Id_2) == 0 && mother.CompareTo(Indv_Id_1) == 0))
                            {
                                line_mother = Indv_Id + " " + mother + " " + conflict;
                            }
                        }
                    }
                    output_file_pairs.WriteLine(line_father + " " + line_mother);
                }
                output_file_pairs.Flush();
                output_file_pairs.Close();
            }

            public static void DataBase_Pairs()
            {
                string ConString = "SERVER=localhost;" +
               "DATABASE=db_Proc;" +
               "UID=root;" +
               "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Gender
                                    FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id 
                                    WHERE (tblindividual.Included = 1) and  (tblindividual.Gender_Modified = 0)";
                Reader = command.ExecuteReader();
                ArrayList Males = new ArrayList();
                ArrayList Females = new ArrayList();
                while (Reader.Read())
                {
                    if (Reader.GetValue(1).ToString().CompareTo("Male") == 0)
                    {
                        Males.Add(Reader.GetValue(0).ToString());
                    }
                    else
                    {
                        Females.Add(Reader.GetValue(0).ToString());
                    }
                }
                conn.Close();
                Reader.Close();
                conn.Open();

                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }

                char[] delimiters = new char[] { ' ' };
                ArrayList my_list = new ArrayList();
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Database_trios.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        if(double.Parse(line_parts[3].ToString()) >= 0.05)
                        {
                            my_list.Add(line);
                        }
                    }
                }

                System.IO.StreamWriter output_file_pairs = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Database_Pairs_2.txt");
                for (int j = 0; j < my_list.Count; j++)
                {
                    System.Console.WriteLine("inserted " + j);
                    string[] line_parts = my_list[j].ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string Indv_Id = line_parts[0];
                    string father_O = line_parts[1];
                    string mother_O = line_parts[2];
                    string father_P = "notfound";
                    string mother_P = "notfound";
                    double min_male = 10;
                    double min_female = 10;

                    int index1 = -1;
                    int index2 = -1;
                    for (int k = 0; k < Data_Set.GetLength(0); k++)
                    {
                        if (Data_Set[k, 0].CompareTo(Indv_Id) == 0)
                        {
                            index1 = k;
                            break;
                        }
                    }

                    ArrayList fathers = new ArrayList();
                    foreach (string father in Males)
                    {
                        if (father.CompareTo(Indv_Id) != 0)
                        {
                            for (int k = 0; k < Data_Set.GetLength(0); k++)
                            {
                                if (Data_Set[k, 0].CompareTo(father) == 0)
                                {
                                    index2 = k;
                                    break;
                                }
                            }
                            fathers.Add(index2 + " " + Parent_offspring_test(index1, index2));
                        }
                        
                    }

                    ArrayList mothers = new ArrayList();
                    foreach (string mother in Females)
                    {
                        if (mother.CompareTo(Indv_Id) != 0)
                        {
                            for (int k = 0; k < Data_Set.GetLength(0); k++)
                            {
                                if (Data_Set[k, 0].CompareTo(mother) == 0)
                                {
                                    index2 = k;
                                    break;
                                }
                            }
                            mothers.Add(index2 + " " + Parent_offspring_test(index1, index2));
                        }
                    }

                    foreach (string mother in mothers)
                    {
                        if (double.Parse(mother.ToString().Split(' ')[1]) <= min_female)
                        {
                            min_female = double.Parse(mother.ToString().Split(' ')[1]);
                            mother_P = mother.ToString().Split(' ')[0];
                        }
                         
                    }

                    foreach (string father in fathers)
                    {
                        if (double.Parse(father.ToString().Split(' ')[1]) <= min_male)
                        {
                            min_male = double.Parse(father.ToString().Split(' ')[1]);
                            father_P = father.ToString().Split(' ')[0];
                        }

                    }

                    output_file_pairs.WriteLine(Indv_Id + " " + father_O + " " + mother_O + " " + father_P + " " + mother_P + " " + min_male.ToString() + " " + min_female.ToString());
                    output_file_pairs.Flush();
                }
                output_file_pairs.Flush();
                output_file_pairs.Close();
            }

            public static void translate()
            {
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }
                System.IO.StreamWriter output_file_trans = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\transalte.txt");
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Database_Pairs_2.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        output_file_trans.WriteLine(Data_Set[int.Parse(line_parts[3]), 0].ToString() + " " + Data_Set[int.Parse(line_parts[4]), 0].ToString());
                    }
                }
                output_file_trans.Flush();
                output_file_trans.Close();
            }

            public static void conflict()
            {
                //            string ConString = "SERVER=localhost;" +
                //                            "DATABASE=db_Proc;" +
                //                            "UID=root;" +
                //                            "PASSWORD=mahmoud;";
                //            MySqlConnection conn = new MySqlConnection(ConString);
                //            conn.Open();
                //            MySqlCommand command = conn.CreateCommand();
                //            command.CommandTimeout = 300000;
                //            MySqlDataReader Reader;
                //            command.CommandText = @"SELECT Indv_Id, Gender
                //                                    FROM tblindividual
                //                                    WHERE (Call_Rate IS NOT NULL) AND (Included = 1)
                //                                    ORDER BY Indv_Id";
                //            Reader = command.ExecuteReader();
                //            ArrayList List = new ArrayList();
                //            while (Reader.Read())
                //            {
                //                List.Add(Reader.GetValue(0).ToString());
                //            }
                //            conn.Close();
                //            Reader.Close();
                string line = "";
                ArrayList List = new ArrayList();
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\defect_6_high.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        List.Add(line.Split('\t')[0]);
                    }
                }


                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };


                float Min_Male = 100;
                float Min_Female = 100;
                string Father = "General";
                string Mother = "General";
                float conflict_ratio_male = 0;
                float conflict_ratio_female = 0;
                ArrayList Best_pedigree = new ArrayList();
                System.IO.StreamWriter output_file_pedigree = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Best_pedigree.txt");
                for (int j = 0; j < List.Count; j++)
                {
                    System.Console.WriteLine("inserted " + j);
                    using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Revolution\QTL_Mapping\Parental conflict data\defect 6 high\Conflict_defect6.txt"))
                    {//
                        while ((line = file.ReadLine()) != null)
                        {
                            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                            if (List[j].ToString().CompareTo(line_parts[0].ToString()) == 0)
                            {
                                if (check_sex(line_parts[1].ToString()).CompareTo("Male") == 0)
                                {
                                    conflict_ratio_male = float.Parse(line_parts[4].ToString());
                                    if (conflict_ratio_male < Min_Male)
                                    {

                                        Min_Male = conflict_ratio_male;
                                        Father = line_parts[1].ToString();
                                    }
                                }
                                else
                                {
                                    conflict_ratio_female = float.Parse(line_parts[4].ToString());
                                    if (conflict_ratio_female < Min_Female)
                                    {
                                        Min_Female = conflict_ratio_female;
                                        Mother = line_parts[1].ToString();
                                    }
                                }
                            }
                            else if (List[j].ToString().CompareTo(line_parts[1].ToString()) == 0)
                            {

                                if (check_sex(line_parts[0].ToString()).CompareTo("Male") == 0)
                                {
                                    conflict_ratio_male = float.Parse(line_parts[4].ToString());
                                    if (conflict_ratio_male < Min_Male)
                                    {
                                        Min_Male = conflict_ratio_male;
                                        Father = line_parts[0].ToString();
                                    }
                                }
                                else
                                {
                                    conflict_ratio_female = float.Parse(line_parts[4].ToString());
                                    if (conflict_ratio_female < Min_Female)
                                    {
                                        Min_Female = conflict_ratio_female;
                                        Mother = line_parts[0].ToString();
                                    }
                                }
                            }
                        }
                    }
                    Best_pedigree.Add(List[j].ToString() + " " + Father + " " + Mother + " " + Min_Male + " " + Min_Female);
                    output_file_pedigree.WriteLine(Best_pedigree[j]);
                    Min_Male = 100;
                    Min_Female = 100;
                    conflict_ratio_male = 0;
                    conflict_ratio_female = 0;
                    Father = "General";
                    Mother = "General";

                }
                output_file_pedigree.Flush();
                output_file_pedigree.Close();
            }

            public static float check_Age(string Indv_Id)
            {
                float Age = 0;
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT Birth_Date from tblindividual where Indv_Id ='" + Indv_Id + "'";
                Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    //System.Console.Write(Reader.GetValue(0).ToString());
                    if (Reader.GetValue(0).ToString().CompareTo("") != 0 && Reader.GetValue(0) != null)
                    {
                        System.DateTime Birth = DateTime.Parse(Reader.GetValue(0).ToString());
                        System.DateTime Now = new System.DateTime(2012, 10, 4);
                        Age = float.Parse(Now.Subtract(Birth).ToString().Split('.')[0]);
                    }
                    else
                    {
                        Age = 0;
                    }
                    //Age = float.Parse(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                return Age;
            }

            public static double check_Call_Rate(string Indv_Id)
            {
                double call_rate = 0;
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT call_rate from tblindividual where Indv_Id ='" + Indv_Id + "'";
                Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    call_rate = double.Parse(Reader.GetValue(0).ToString());

                }
                conn.Close();
                Reader.Close();
                return call_rate;
            }

            public static int check_Defect(string Indv_Id)
            {
                int defect = 0;
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual_phenotype.Phenotype_Id
                                FROM tblindividual_phenotype INNER JOIN
                                tblphenotype ON tblindividual_phenotype.Phenotype_Id = tblphenotype.Phenotype_Id
                                where (tblindividual_phenotype.Indv_Id = '" + Indv_Id + @"')";
                Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    //System.Console.Write(Reader.GetValue(0).ToString());
                    if (Reader.GetValue(0).ToString().CompareTo("") != 0 && Reader.GetValue(0) != null)
                    {
                        defect = int.Parse(Reader.GetValue(0).ToString());
                    }
                    else
                    {
                        defect = 0;
                    }

                }
                conn.Close();
                Reader.Close();
                return defect;
            }

            public static void function2()
            {
                string line = "";
                System.IO.StreamWriter output_file_pedigree = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Database_trios_M.txt");
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\Database_trios.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        int defect = check_Defect(line.Split(' ')[0]);
                        output_file_pedigree.WriteLine(line + " " + defect.ToString());
                    }
                }
                output_file_pedigree.Flush();
                output_file_pedigree.Close();
            }


            public static void compare_pedigree()
            {
                string ConString = "SERVER=localhost;" +
                    "DATABASE=db_Proc;" +
                    "UID=root;" +
                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblpedigree.Paternal_Id, tblpedigree.Maternal_Id
                                    FROM tblindividual INNER JOIN
                                    tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id INNER JOIN
                                    tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id
                                    WHERE (tblindividual.Call_Rate IS NOT NULL) AND (tblindividual.Included = 1)
                                    ORDER BY tblindividual.Indv_Id";
                Reader = command.ExecuteReader();
                ArrayList List = new ArrayList();
                while (Reader.Read())
                {
                    List.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString());
                }
                conn.Close();
                Reader.Close();
                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };

                string line = "";
                ArrayList Best_pedigree = new ArrayList();
                System.IO.StreamWriter output_file_pedigree = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Age_pedigree.txt");

                int j = 0;
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Best_pedigree.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Best_pedigree_line = line.Split(delimiters_space, StringSplitOptions.RemoveEmptyEntries);
                        string[] pedigree_line = List[j].ToString().Split(delimiters_space, StringSplitOptions.RemoveEmptyEntries);
                        string Indv_Id = pedigree_line[0];
                        string father = pedigree_line[1];
                        string mother = pedigree_line[2];
                        string file_line = "";
                        if (Indv_Id.ToString().CompareTo(Best_pedigree_line[0].ToString()) == 0)
                        {
                            if (pedigree_line[1].CompareTo(Best_pedigree_line[1].ToString()) == 0 && pedigree_line[2].CompareTo(Best_pedigree_line[2].ToString()) == 0)
                            {
                                //file_line = pedigree_line[0] + " " + "TRUE" + " " + "TRUE";
                                file_line = Indv_Id + " " + check_Defect(Indv_Id).ToString() + " " + check_Age(Indv_Id).ToString() + " " + Best_pedigree_line[1] + " " + check_Defect(Best_pedigree_line[1]).ToString() + " " + check_Age(Best_pedigree_line[1]).ToString() + " " + Best_pedigree_line[2] + " " + check_Defect(Best_pedigree_line[2]).ToString() + " " + check_Age(Best_pedigree_line[2]).ToString();
                            }
                            else
                            {
                                if (pedigree_line[1].CompareTo(Best_pedigree_line[1].ToString()) == 0)
                                {
                                    //file_line = pedigree_line[0] + " " + "TRUE" + " " + "FALSE";
                                    file_line = Indv_Id + " " + check_Defect(Indv_Id).ToString() + " " + check_Age(Indv_Id).ToString() + " " + Best_pedigree_line[1] + " " + check_Defect(Best_pedigree_line[1]).ToString() + " " + check_Age(Best_pedigree_line[1]).ToString() + " " + Best_pedigree_line[2] + " " + check_Defect(Best_pedigree_line[2]).ToString() + " " + check_Age(Best_pedigree_line[2]).ToString();

                                }
                                else
                                {
                                    // file_line = pedigree_line[0] + " " + "FALSE" + " " + "TRUE";
                                    file_line = Indv_Id + " " + check_Defect(Indv_Id).ToString() + " " + check_Age(Indv_Id).ToString() + " " + Best_pedigree_line[1] + " " + check_Defect(Best_pedigree_line[1]).ToString() + " " + check_Age(Best_pedigree_line[1]).ToString() + " " + Best_pedigree_line[2] + " " + check_Defect(Best_pedigree_line[2]).ToString() + " " + check_Age(Best_pedigree_line[2]).ToString();
                                }
                            }
                            output_file_pedigree.WriteLine(file_line);

                        }
                        j++;
                    }
                }
                output_file_pedigree.Flush();
                output_file_pedigree.Close();
            }

            public static double check_Trios(int Indv_Id, int Father, int Mother)
            {
                int Conflict = 0;
                double Conflict_Ratio = 0;
                //int markers = 60835 + 6 - 1;
                int markers = 62169;
                System.IO.StreamWriter output_check = new System.IO.StreamWriter(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\Plink\Check.txt");
                int Missing_Genotype = 0;
                char[] delimiters = new char[] { '\t' };
                for (int j = 6; j <= markers; j++)
                {
                    if (j < 60840)
                    {
                        if ((Data_Set[Father, j].ToString().CompareTo("NC") == 0 && Data_Set[Mother, j].ToString().CompareTo("NC") == 0) || Data_Set[Indv_Id, j].ToString().CompareTo("NC") == 0)
                        {
                            Missing_Genotype++;
                            output_check.WriteLine(Data_Set_map[j - 6] + " 0");
                        }
                        else
                        {
                            if ((Data_Set[Indv_Id, j].ToString().CompareTo("AB") == 0 && Data_Set[Father, j].ToString().CompareTo("AA") == 0 && Data_Set[Mother, j].ToString().CompareTo("AA") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("AB") == 0 && Data_Set[Father, j].ToString().CompareTo("BB") == 0 && Data_Set[Mother, j].ToString().CompareTo("BB") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("AA") == 0 && Data_Set[Mother, j].ToString().CompareTo("BB") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("AA") == 0 && Data_Set[Father, j].ToString().CompareTo("BB") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("AA") == 0 && Data_Set[Father, j].ToString().CompareTo("BB") == 0 && Data_Set[Mother, j].ToString().CompareTo("BB") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("BB") == 0 && Data_Set[Mother, j].ToString().CompareTo("AA") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("BB") == 0 && Data_Set[Father, j].ToString().CompareTo("AA") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("BB") == 0 && Data_Set[Father, j].ToString().CompareTo("AA") == 0 && Data_Set[Mother, j].ToString().CompareTo("AA") == 0))
                            {
                                Conflict++;
                                output_check.WriteLine(Data_Set_map[j - 6] + " 1");
                            }
                            else
                            {
                                output_check.WriteLine(Data_Set_map[j - 6] + " 0");
                            }

                        }
                    }
                    else
                    {
                        if (check_sex(Data_Set[Indv_Id, 1]).CompareTo("Male") == 0)
                        {
                            if (j < 62150)
                            {
                                if ((Data_Set[Mother, j].ToString().CompareTo("NC") == 0) || Data_Set[Indv_Id, j].ToString().CompareTo("NC") == 0)
                                {
                                    Missing_Genotype++;
                                    output_check.WriteLine(Data_Set_map[j - 6] + " 0");
                                } // end if
                                else
                                {
                                    if ((Data_Set[Indv_Id, j].ToString().CompareTo("BB") == 0 && Data_Set[Mother, j].ToString().CompareTo("AA") == 0)
                                        || (Data_Set[Indv_Id, j].ToString().CompareTo("AA") == 0 && Data_Set[Mother, j].ToString().CompareTo("BB") == 0))
                                    {
                                        Conflict++;
                                        output_check.WriteLine(Data_Set_map[j - 6] + " 1");
                                    } //end if
                                    else
                                    {
                                        output_check.WriteLine(Data_Set_map[j - 6] + " 0");
                                    } // end else
                                }// end else
                            }// end if
                        } // end if 
                    }// end else
                }// end for
                output_check.Flush();
                output_check.Close();
                Conflict_Ratio = (double)Conflict / (markers - Missing_Genotype);
                return Conflict_Ratio;

            }

            public static double check_Trios(string Indv_Id_s, string Father_s, string Mother_s)
            {
                int Conflict = 0;
                double Conflict_Ratio = 0;
                int markers = total_markers();
                int Missing_Genotype = 0;
                int Father = -1;
                int Mother = -1;
                int Indv_Id = -1;
                for (int i = 0; i < Data_Set.GetLength(0); i++)
                {
                    if (Indv_Id_s.CompareTo(Data_Set[i, 0]) == 0)
                    {
                        Indv_Id = i;
                    }
                    else if (Father_s.CompareTo(Data_Set[i, 0]) == 0)
                    {
                        Father = i;
                    }
                    else if (Mother_s.CompareTo(Data_Set[i, 0]) == 0)
                    {
                        Mother = i;
                    }
                    else if (Father != -1 && Mother != -1 && Indv_Id != -1)
                    {
                        break;
                    }
                }
                char[] delimiters = new char[] { '\t' };
                for (int j = 1; j <= markers; j++)
                {
                    if (j <= 54541)
                    {
                        if ((Data_Set[Father, j].ToString().CompareTo("NC") == 0 && Data_Set[Mother, j].ToString().CompareTo("NC") == 0) || Data_Set[Indv_Id, j].ToString().CompareTo("NC") == 0)
                        {
                            Missing_Genotype++;
                            //output_check.WriteLine(Data_Set_map[j - 6] + " 0");
                        }
                        else
                        {
                            if ((Data_Set[Indv_Id, j].ToString().CompareTo("AB") == 0 && Data_Set[Father, j].ToString().CompareTo("AA") == 0 && Data_Set[Mother, j].ToString().CompareTo("AA") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("AB") == 0 && Data_Set[Father, j].ToString().CompareTo("BB") == 0 && Data_Set[Mother, j].ToString().CompareTo("BB") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("AA") == 0 && Data_Set[Mother, j].ToString().CompareTo("BB") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("AA") == 0 && Data_Set[Father, j].ToString().CompareTo("BB") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("AA") == 0 && Data_Set[Father, j].ToString().CompareTo("BB") == 0 && Data_Set[Mother, j].ToString().CompareTo("BB") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("BB") == 0 && Data_Set[Mother, j].ToString().CompareTo("AA") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("BB") == 0 && Data_Set[Father, j].ToString().CompareTo("AA") == 0)
                                || (Data_Set[Indv_Id, j].ToString().CompareTo("BB") == 0 && Data_Set[Father, j].ToString().CompareTo("AA") == 0 && Data_Set[Mother, j].ToString().CompareTo("AA") == 0))
                            {
                                Conflict++;
                                Data_Set_Conflict[j] = Data_Set_Conflict[j] + 1;
                                // output_check.WriteLine(Data_Set_map[j - 6] + " 1");
                            }
                            else
                            {
                                //output_check.WriteLine(Data_Set_map[j - 6] + " 0");
                            }

                        }
                    }
                    else
                    {
                        if (check_sex(Data_Set[Indv_Id, 0]).CompareTo("Male") == 0)
                        {
                            if ((Data_Set[Mother, j].ToString().CompareTo("NC") == 0) || Data_Set[Indv_Id, j].ToString().CompareTo("NC") == 0)
                            {
                                Missing_Genotype++;
                                //output_check.WriteLine(Data_Set_map[j - 6] + " 0");
                            } // end if
                            else
                            {
                                if ((Data_Set[Indv_Id, j].ToString().CompareTo("BB") == 0 && Data_Set[Mother, j].ToString().CompareTo("AA") == 0)
                                    || (Data_Set[Indv_Id, j].ToString().CompareTo("AA") == 0 && Data_Set[Mother, j].ToString().CompareTo("BB") == 0))
                                {
                                    Conflict++;
                                    Data_Set_Conflict[j] = Data_Set_Conflict[j] + 1;
                                    //output_check.WriteLine(Data_Set_map[j - 6] + " 1");
                                } //end if
                                else
                                {
                                    //output_check.WriteLine(Data_Set_map[j - 6] + " 0");
                                } // end else
                            }// end else
                        } // end if 
                    }// end else
                }// end for
                //output_check.Flush();
                //output_check.Close();
                Conflict_Ratio = (double)Conflict / (markers - Missing_Genotype);
                return Conflict_Ratio;
            }


            public static void compare_Trios()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id from tblindividual
                                    WHERE (tblindividual.Call_Rate IS NOT NULL) AND (tblindividual.Included = 1)
                                    ORDER BY tblindividual.Indv_Id";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                int k = 0;
                while (Reader.Read())
                {
                    Indv_List.Add(k + " " + Reader.GetValue(0).ToString());
                    k++;
                }
                conn.Close();
                Reader.Close();
                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };
                string line = "";
                ArrayList Best_pedigree = new ArrayList();
                System.IO.StreamWriter output_file_pedigree = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\All_Trios.txt");
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\My.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }

                i = 0;
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\Best_pedigree.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        System.Console.WriteLine(i);
                        string file_line = "";
                        string[] Best_pedigree_line = line.Split(delimiters_space, StringSplitOptions.RemoveEmptyEntries);
                        string Indv_Id = Best_pedigree_line[0];
                        string father = Best_pedigree_line[1];
                        string mother = Best_pedigree_line[2];
                        double conflict = check_Trios(find_indv_in_list(Indv_List, Indv_Id), find_indv_in_list(Indv_List, father), find_indv_in_list(Indv_List, mother));
                        file_line = Indv_Id + " " + father + " " + mother + " " + conflict;
                        output_file_pedigree.WriteLine(file_line);

                        file_line = "";
                        conflict = check_Trios(find_indv_in_list(Indv_List, father), find_indv_in_list(Indv_List, Indv_Id), find_indv_in_list(Indv_List, mother));
                        file_line = father + " " + Indv_Id + " " + mother + " " + conflict;
                        output_file_pedigree.WriteLine(file_line);

                        file_line = "";
                        conflict = check_Trios(find_indv_in_list(Indv_List, mother), find_indv_in_list(Indv_List, father), find_indv_in_list(Indv_List, Indv_Id));
                        file_line = mother + " " + father + " " + Indv_Id + " " + conflict;
                        output_file_pedigree.WriteLine(file_line);
                        i++;

                    }
                }
                output_file_pedigree.Flush();
                output_file_pedigree.Close();
            }

            public static void Hardy_Wienburg_Test(string Line)
            {
                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\My_Founders.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        if (check_line(line_parts[0]).CompareTo(Line) == 0)
                        {
                            Data_Set_Founders[i, 0] = line_parts[0];
                            for (int j = 1; j < line_parts.Length; j++)
                            {
                                Data_Set_Founders[i, j] = line_parts[j];
                            }
                            i++;
                        }
                    }
                }

                int Marker_Index = 1;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\My_Founders.map"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Split(' ')[0].CompareTo("X") == 0 || line.Split(' ')[0].CompareTo("Y") == 0)
                        {
                            System.Console.WriteLine("X chromose");
                            break;
                        }
                        else
                        {
                            //System.Console.WriteLine("Inserted " + (Marker_Index).ToString());
                            Marker_Genotype_Frequency_File(Marker_Index, line.Split(' ')[1], i, Line);
                          
                        }
                        Marker_Index++;
                    }
                }
            }

            public static void Hardy_Wienburg_per_marker(string marker_id,string Line)
            {
                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\My_Founders.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        if (check_line(line_parts[0]).CompareTo(Line) == 0 && sex_prob(line_parts[0]) ==0 && check_Call_Rate(line_parts[0]) >=0.95 )
                        {
                            Data_Set_Founders[i, 0] = line_parts[0];
                            for (int j = 1; j < line_parts.Length; j++)
                            {
                                Data_Set_Founders[i, j] = line_parts[j];
                            }
                            i++;
                        }
                    }
                }

                int Marker_Index = 1;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\My_Founders.map"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Split(' ')[1].CompareTo(marker_id) == 0)
                        {
                            Marker_Genotype_Frequency_File(Marker_Index, line.Split(' ')[1], i, Line);
                            break;
                        }
                        Marker_Index++;
                    }
                }
            }

            public static void Hardy_Wienburg_Test_Female(string Line)
            {
                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\My_Founders.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        if (check_line(line_parts[0]).CompareTo(Line) == 0 && check_sex(line_parts[0]).CompareTo("Female") == 0)
                        {
                            Data_Set_Founders[i, 0] = line_parts[0];
                            for (int j = 1; j < line_parts.Length; j++)
                            {
                                Data_Set_Founders[i, j] = line_parts[j];
                            }
                            i++;
                        }
                    }
                }

                int Marker_Index = 1;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\My_Founders.map"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Split(' ')[0].CompareTo("X") == 0)
                        {
                            System.Console.WriteLine("X chromose");
                            Marker_Genotype_Frequency_File(Marker_Index, line.Split(' ')[1], i, Line);
                        }
                        Marker_Index++;
                    }
                }
            }

            public static void X_CHR_pvalues()
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"select marker_id, P23,P33,P36,P12,P15 from tblmarker_loc where chr  = 'X' and included =1 order by position,marker_id";
                Reader = command.ExecuteReader();
                ArrayList Marker_List = new ArrayList();
                System.IO.StreamWriter output_file_markers = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\X_CHR_pvalues.txt");
                int k = 0;
                while (Reader.Read())
                {
                    output_file_markers.WriteLine(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString() + " " + Reader.GetValue(3).ToString() + " " + Reader.GetValue(4).ToString());
                    k++;
                }
                conn.Close();
                Reader.Close();
            }

            public static void compare_Trios_2(string filename)  // calculate the conflict for the trios with the .ped file
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id from tblindividual
                                    WHERE (tblindividual.Call_Rate IS NOT NULL) AND (tblindividual.Included = 1)
                                    ORDER BY tblindividual.Indv_Id";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                int k = 0;
                while (Reader.Read())
                {
                    Indv_List.Add(k + " " + Reader.GetValue(0).ToString());
                    k++;
                }
                conn.Close();
                Reader.Close();
                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };
                string line = "";
                ArrayList Best_pedigree = new ArrayList();
                System.IO.StreamWriter output_file_pedigree = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Database_trios.txt");
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\" + filename + ".ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }

                conn.Open();
                command.CommandText = @"SELECT tblindividual.Indv_Id, Sample_ID, tblpedigree.Maternal_Id,
                                (select Sample_ID from tblindividual  
                                INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id
                                Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                where tblindividual.indv_id = tblpedigree.Maternal_Id and tblindividual.included = 1 and gender_modified = 0  and tblindividual.call_rate is not null ) AS Maternal_sample_id,
                                tblpedigree.Paternal_Id,
                                (select Sample_ID from tblindividual  
                                INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id
                                Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                where tblindividual.indv_id = tblpedigree.Paternal_Id and tblindividual.included = 1 and gender_modified = 0  and tblindividual.call_rate is not null ) AS Paternal_sample_id,
                                tblindividual_phenotype.phenotype_ID,Paternal_Id_Modified,Maternal_Id_Modified
                                FROM tblindividual 
                                INNER JOIN
                                tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                                inner join tblindividual_phenotype on  tblindividual.indv_id = tblindividual_phenotype.indv_id 
                                Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id
                                WHERE tblindividual.included = 1 and gender_modified = 0  and tblindividual.call_rate is not null and (Maternal_id != 'General' and Paternal_id != 'General') 
                                Having Maternal_sample_id is not  null and  Paternal_sample_id is  not null";
                Reader = command.ExecuteReader();
                int counter = 0;
                while (Reader.Read())
                {
                    System.Console.WriteLine("checked " + counter);
                    string file_line = "";
                    string Indv_Id = Reader.GetValue(0).ToString();
                    string father = Reader.GetValue(4).ToString();
                    string mother = Reader.GetValue(2).ToString();
                    string Defect = Reader.GetValue(6).ToString();
                    double conflict = check_Trios(Indv_Id, father, mother);
                    file_line = Indv_Id + " " + father + " " + mother + " " + conflict + " " + Defect;
                    output_file_pedigree.WriteLine(file_line);
                    counter++;
                }
                conn.Close();
                Reader.Close();
                output_file_pedigree.Flush();
                output_file_pedigree.Close();
            }

            public static void pred_trios()
            {
                char[] delimiters = new char[] { '\t' };
                char[] delimiters_space = new char[] { ' ' };
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }
                System.IO.StreamWriter output_file_pred = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\pred_trios.txt");
                i = 1;
                string file_line = "";
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Database_Pairs_2.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        string Indv_Id = line_parts[0];
                        string father = Data_Set[int.Parse(line_parts[3]), 0];
                        string mother = Data_Set[int.Parse(line_parts[4]), 0];
                        double conflict = check_Trios(Indv_Id, father, mother);
                        file_line = Indv_Id + " " + father + " " + mother + " " + conflict + " " + i;
                        output_file_pred.WriteLine(file_line);
                        i++;
                    }
                }
                output_file_pred.Flush();
                output_file_pred.Close();
            }

            public static void plink_markers()
            {
                string line = "";
                int i = 0;
                //ArrayList marker_list = new ArrayList();
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\families\RG disease\Markers.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {

                        //marker_list.Add(line);
                        Update_plink_marker(line);
                        i++;
                    }
                }
            }

            public static void Update_plink_marker(string Marker_Id)
            {
                string ConString = "SERVER=localhost;" +
              "DATABASE=db_Proc;" +
              "UID=root;" +
              "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "Update db_proc.tblmarker_loc set Plink_Markers = 1 Where Marker_Id = '" + Marker_Id + "'";
                Reader = command.ExecuteReader();
                Reader.Close();
                conn.Close();

            }

            public static int find_indv_in_list(ArrayList Indv_List, string Indv_Id)
            {
                int index = 0;
                foreach (string line in Indv_List)
                {
                    if (line.Split(' ')[1].CompareTo(Indv_Id) == 0)
                    {
                        index = int.Parse(line.Split(' ')[0].ToString());
                        break;
                    }
                }
                return index;
            }

            public static void selected_trios()
            {
                string line = "";
                int counter = 0;
                double min_trios = 1;
                string print_trios = "";
                ArrayList Data = new ArrayList();
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\All_Trios.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        Data.Add(line);
                    }
                }
                System.IO.StreamWriter output_file_pedigree = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\selected_trios.txt");
                for (int i = 0; i < Data.Count; i = i + 3)
                {
                    System.Console.WriteLine(i);
                    for (int j = i; j < i + 3; j++)
                    {
                        if (double.Parse(Data[j].ToString().Split(' ')[3]) < min_trios)
                        {
                            min_trios = double.Parse(Data[j].ToString().Split(' ')[3]);
                            print_trios = Data[j].ToString();
                        }
                    }
                    output_file_pedigree.WriteLine(print_trios);
                    min_trios = 1;
                    print_trios = "";
                }
                output_file_pedigree.Flush();
                output_file_pedigree.Close();
            }

            public static void modified_pedigree()
            {
                string line = "";
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\selected_trios.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        Pedigree ped = new Pedigree();
                        ped.Indv_Id = line.Split(' ')[0];
                        ped.Paternal_Id_Modified = line.Split(' ')[1];
                        ped.Maternal_Id_Modified = line.Split(' ')[2];
                        Update_Pedigree(ped);

                    }
                }
            }

            public static void save_sample(string sample_id, string indv_id)
            {
                string ConString = "SERVER=localhost;" +
                                "DATABASE=db_Proc;" +
                                "UID=root;" +
                                "PASSWORD=mahmoud;";

                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = @"SELECT tblsnp.Marker_Id, tblsnp.Genotype, tblmarker_loc.Chr
                             FROM tblsnp INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                             WHERE (Sample_Id = '" + sample_id + @"') AND (tblmarker_loc.Chr <> 'X') AND (tblmarker_loc.Chr <> 'Y')
                             ORDER BY 0 + tblmarker_loc.Chr, tblmarker_loc.Position, tblsnp.Marker_Id";
                Reader = cmd.ExecuteReader();
                System.IO.StreamWriter output_file_sample = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Pseudo_Region\Sample_" + sample_id + ".txt");
                while (Reader.Read())
                {
                    output_file_sample.WriteLine(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString());
                }
                Reader.Close();
                output_file_sample.Flush();
                output_file_sample.Close();


                string line = "";
                char[] delimiters_tab = new char[] { '\t' };
                char[] delimiters_dot = new char[] { '.' };
                System.IO.StreamWriter output_file_s = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Pseudo_Region\Samplefile_" + sample_id + ".txt");

                int counter = 0;
                using (StreamReader file = new StreamReader(@"D:\Old_Full Data Table.txt"))
                {//
                    int indv_index = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts_tab = line.Split(delimiters_tab, StringSplitOptions.RemoveEmptyEntries);

                        if (counter == 0)
                        {

                            ArrayList temp = new ArrayList();
                            for (int i = 2; i < line_parts_tab.Length; i++)
                            {
                                temp.Add(line_parts_tab[i]);
                            }

                            for (int i = 0; i < temp.Count; i = i + 7)
                            {
                                string[] line_parts_dot = temp[i].ToString().Split(delimiters_dot, StringSplitOptions.RemoveEmptyEntries);
                                if (line_parts_dot[0].CompareTo(indv_id) == 0)
                                {
                                    indv_index = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (!check(line_parts_tab[1]))
                            {
                                System.Console.WriteLine(counter);
                                output_file_s.WriteLine(line_parts_tab[1] + " " + line_parts_tab[indv_index + 2]);
                            }
                        }
                        counter++;
                    }
                }
                output_file_s.Flush();
                output_file_s.Close();
            }

            public static bool check(string marker_id) // check if marker is on the X,Y chromosome
            {

                string ConString = "SERVER=localhost;" +
                    "DATABASE=db_Proc;" +
                    "UID=root;" +
                    "PASSWORD=mahmoud;";
                bool Found = false;
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = @"select chr from tblmarker_loc where marker_id ='" + marker_id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {

                    if (Reader.GetValue(0).ToString().CompareTo("X") == 0 || Reader.GetValue(0).ToString().CompareTo("Y") == 0)
                    {
                        Found = true;
                    }
                }
                Reader.Close();
                conn.Close();
                return Found;
            }

            public static void paire_conflict()  // latest function for the conflict
            {
                string ConString = "SERVER=localhost;" +
                       "DATABASE=db_Proc;" +
                       "UID=root;" +
                       "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Call_Rate, tblindividual.Gender, tblindividual.Age, tblindividual.Birth_Date, tblindividual.Line, tblindividual.Location, 
                                    tblindividual.Included 
                                    FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id 
                                    WHERE (tblindividual.Included = 1) and  (tblindividual.call_rate is not null)";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                conn.Open();
                string line = "";
                ArrayList defect_6 = new ArrayList();
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\defect_6_high.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        defect_6.Add(line.Split('\t')[0]);
                    }
                }


                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\My_data.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }

                for (i = 0; i < defect_6.Count; i++)
                {
                    System.Console.WriteLine("individual " + i);
                    System.IO.StreamWriter Indv_File = new System.IO.StreamWriter(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\" + (string)defect_6[i] + ".txt");
                    for (int j = i; j < Indv_list.Count; j++)
                    {
                        if (i != j)
                        {
                            Parent_offspring_test(i, j, (string)defect_6[i], (string)Indv_list[j], Indv_File);
                        }
                    }
                    Indv_File.Flush();
                    Indv_File.Close();
                }
            }

            public static void paire_conflict_All()  // latest function for the conflict
            {
                string ConString = "SERVER=localhost;" +
                       "DATABASE=db_Proc;" +
                       "UID=root;" +
                       "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Call_Rate, tblindividual.Gender, tblindividual.Age, tblindividual.Birth_Date, tblindividual.Line, tblindividual.Location, 
                                    tblindividual.Included 
                                    FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id 
                                    WHERE (tblindividual.Included = 1) and  (tblindividual.Gender_Modified = 0)";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();
                conn.Open();
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }
                int index1, index2 = 0;
                for (i = 0; i < Indv_list.Count; i++)
                {
                    index1 = -1;
                    index2 = -1;
                    for (int k = 0; k < Data_Set.GetLength(0); k++)
                    {
                        if (Data_Set[k, 0].CompareTo(Indv_list[i]) == 0)
                        {
                            index1 = k;
                            break;
                        }
                    }
                    System.Console.WriteLine("individual " + i);
                    System.IO.StreamWriter Indv_File = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\P_C_conflict\" + (string)Indv_list[i] + ".txt");
                    for (int j = i; j < Indv_list.Count; j++)
                    {
                        if (i != j)
                        {
                            for (int k = 0; k < Data_Set.GetLength(0); k++)
                            {
                                if (Data_Set[k, 0].CompareTo(Indv_list[j]) == 0)
                                {
                                    index2 = k;
                                    break;
                                }
                            }
                            Parent_offspring_test(index1, index2, (string)Indv_list[i], (string)Indv_list[j], Indv_File);
                        }
                    }
                    Indv_File.Flush();
                    Indv_File.Close();
                }
            }

            public static void original_paire()
            {
                string line = "";
                char[] delimiters = new char[] { '\t' };
                ArrayList my_list = new ArrayList();
                using (StreamReader file = new StreamReader(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\phasebook\defect_6_high.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        my_list.Add(line);
                    }
                }
                System.IO.StreamWriter output_file_pairs = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\defect_6_high_pairs.txt");

                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\My_data.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }

                for (int j = 0; j < my_list.Count; j++)
                {
                    System.Console.WriteLine("inserted " + j);
                    string[] line_parts = my_list[j].ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string Indv_Id = line_parts[0];
                    string father = line_parts[1];
                    string mother = line_parts[2];
                    double conflict_father = Parent_offspring_test(Indv_Id, father);
                    double conflict_mother = Parent_offspring_test(Indv_Id, mother);
                    output_file_pairs.WriteLine(Indv_Id + " " + father + " " + conflict_father + " " + mother + " " + conflict_mother);
                }
                output_file_pairs.Flush();
                output_file_pairs.Close();
            }


            public static void insert_call_rate()
            {
                int count = 0;
                string line = "";
                //char[] delimiters = new char[] { '\t' };
                //char[] delimiters2 = new char[] { '.' };

                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Samples Table.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        if (count != 0)
                        {
                            Individual indv = new Individual();
                            string[] part =  line.Split('\t');
                            indv.Indv_Id = part[17];
                            indv.Call_Rate = float.Parse(part[9].ToString());
                            Update_Call_Rate(indv);
                        }
                        count++;
                    }
                }
            }

            public static void check_new_patch()
            {
                string ConString = "SERVER=localhost;" +
                                   "DATABASE=db_Proc;" +
                                   "UID=root;" +
                                   "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual_sample.Sample_Id, tblsample.DNA_Id, tblbox_dna.Box_Id, tblindividual.Call_Rate, tblpedigree.Maternal_Id, 
                         tblpedigree.Paternal_Id
                         FROM  tblindividual INNER JOIN
                         tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
                         tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id INNER JOIN
                         tbldna ON tblsample.DNA_Id = tbldna.DNA_Id INNER JOIN
                         tblbox_dna ON tbldna.DNA_Id = tblbox_dna.DNA_Id LEFT OUTER JOIN
                         tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                         WHERE (tblbox_dna.Box_Id = '4963') OR
                         (tblbox_dna.Box_Id = '4964')";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                   Indv_list.Add(Reader.GetValue(0).ToString());
                }
                conn.Close();
                Reader.Close();

                ArrayList Indv_found = new ArrayList();
                ArrayList Indv_not_found = new ArrayList();
                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                DA.Fill(ds, "samples");
                int count = 0;
                bool found = false;
                for (int j = 0; j < Indv_list.Count; j++)
                    {
                        foreach (DataRow dr in ds.Tables["samples"].Rows)
                        {
                            if (Indv_list[j].ToString().Equals(dr[0].ToString().Trim()))
                            {
                                if (Indv_list[j].ToString().CompareTo("BA005112") == 0)
                                {
                                    int c = 1;
                                }
 
                                Pedigree ped = new Pedigree();
                                ped.Indv_Id = Indv_list[j].ToString();
                                if (dr[16].ToString().Trim().CompareTo("") == 0 || dr[16].ToString().Trim().CompareTo(null) == 0)
                                {
                                    ped.Maternal_Id = "General";
                                }
                                else
                                {
                                    ped.Maternal_Id = dr[16].ToString().Trim();
                                    if (!individual_exists(ped.Maternal_Id))
                                    {
                                        Individual mother = new Individual();
                                        mother.Indv_Id = ped.Maternal_Id;
                                        mother.Gender = "Female";
                                        foreach (DataRow dr_2 in ds.Tables["samples"].Rows)
                                        {
                                            if (mother.Indv_Id.Equals(dr_2[0].ToString().Trim()))
                                            {

                                                if (dr_2[7].ToString().Trim().CompareTo("") != 0)
                                                {
                                                    DateTime dat = DateTime.Parse(dr_2[7].ToString().Trim());
                                                    string formatForMySql = dat.ToString("yyyy-MM-dd");
                                                    mother.Birth_Date = formatForMySql;
                                                }
                                                else
                                                {
                                                    mother.Birth_Date = "0000-00-00";
                                                }


                                                
                                                mother.Line = int.Parse(dr_2[23].ToString().Trim());
                                                mother.Location = dr_2[24].ToString().Trim();
                                                mother.Age = null;
                                                excute_Individual(mother);
                                                break;
                                            }
                                        }

                                    }
                                    
                                }
                                if (dr[11].ToString().Trim().CompareTo("") == 0 || dr[11].ToString().Trim().CompareTo(null) == 0)
                                {
                                    ped.Paternal_Id = "General";
                                }
                                else
                                {
                                    ped.Paternal_Id = dr[11].ToString().Trim();
                                    if (!individual_exists(ped.Paternal_Id))
                                    {
                                        Individual father = new Individual();
                                        father.Indv_Id = ped.Paternal_Id;
                                        father.Gender = "Male";
                                        foreach (DataRow dr_2 in ds.Tables["samples"].Rows)
                                        {
                                            if (father.Indv_Id.Equals(dr_2[0].ToString().Trim()))
                                            {

                                                if (dr_2[7].ToString().Trim().CompareTo("") != 0)
                                                {
                                                    DateTime dat = DateTime.Parse(dr_2[7].ToString().Trim());
                                                    string formatForMySql = dat.ToString("yyyy-MM-dd");
                                                    father.Birth_Date = formatForMySql;
                                                }
                                                else
                                                {
                                                    father.Birth_Date = "0000-00-00";
                                                }


                                                father.Indv_Id = (string)Indv_list[j];
                                                father.Line = int.Parse(dr_2[23].ToString().Trim());
                                                father.Location = dr_2[24].ToString().Trim();
                                                father.Age = null;
                                                excute_Individual(father);
                                                break;
                                            }
                                        }

                                    }
                                }
                                ped.Maternal_Id_Modified = null;
                                ped.Paternal_Id_Modified = null;
                                ped.Modified = 0;
                                ped.Num_Offspring = null;
                                ped.Num_Siblings = null;
                                Update_original_Pedigree(ped);
                                System.Console.WriteLine(count + " " + Indv_list[j] + " inserted");
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            System.Console.WriteLine(Indv_list[j] + " not found");
                        }
                        found = false;
                        count++;
                    }
                     
            }

            public static void check_data()  // check anneleen excel file and my database
            {

                string ConString = "SERVER=localhost;" +
                    "DATABASE=db_Proc;" +
                    "UID=root;" +
                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblpedigree.Paternal_Id, tblpedigree.Maternal_Id
                                     FROM tblindividual INNER JOIN
                                     tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString());
                }
                conn.Close();
                Reader.Close();

                ArrayList Indv_found = new ArrayList();
                ArrayList Indv_not_found = new ArrayList();
                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                DA.Fill(ds, "samples");
                int count = 0;
                bool found = false;
                System.IO.StreamWriter output_file_compare = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Compare.txt");
                for (int j = 0; j < Indv_list.Count; j++)
                {
                    foreach (DataRow dr in ds.Tables["samples"].Rows)
                    {
                        if (Indv_list[j].ToString().Split(' ')[0].Equals(dr[0].ToString().Trim()))
                        {
                            found = true;
                            if (dr[11].ToString().Trim().CompareTo(Indv_list[j].ToString().Split(' ')[1]) == 0 && dr[16].ToString().Trim().CompareTo(Indv_list[j].ToString().Split(' ')[2]) == 0)
                            {
                                System.Console.WriteLine("found " + count);
                                count++;
                                
                            }
                            else
                            {
                                System.Console.WriteLine(dr[0].ToString().Trim() + " " + dr[11].ToString().Trim() + " " + dr[16].ToString().Trim() + " " + Indv_list[j].ToString().Split(' ')[1] + " " + Indv_list[j].ToString().Split(' ')[2]);
                            }
                            string father = "";
                            string mother = "";
                            if (dr[11].ToString().Trim().CompareTo("") == 0)
                            {
                                father = "General";
                            }
                            else
                            {
                                father = dr[11].ToString().Trim();
                            }
                            if (dr[16].ToString().Trim().CompareTo("") == 0)
                            {
                                mother = "General";
                            }
                            else
                            {
                                mother = dr[16].ToString().Trim();
                            }
                            output_file_compare.WriteLine(dr[0].ToString().Trim() + " " + father + " " + mother + " " + Indv_list[j].ToString().Split(' ')[1] + " " + Indv_list[j].ToString().Split(' ')[2]);
                            break;
                        }
                        
                    }
                    if (!found)
                    {
                        output_file_compare.WriteLine(Indv_list[j].ToString().Split(' ')[0]  + " Notfound " +  "NotFound " + Indv_list[j].ToString().Split(' ')[1] + " " + Indv_list[j].ToString().Split(' ')[2] + " Notfound");
                    }
                    found = false;
 
                }
                output_file_compare.Flush();
                output_file_compare.Close();
            }

            public static void check_data_defect()  // check anneleen excel file and my database
            {
                defects_names();
                string ConString = "SERVER=localhost;" +
                    "DATABASE=db_Proc;" +
                    "UID=root;" +
                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Gender, tblindividual.Call_Rate, tblindividual.Line, tblindividual_phenotype.Phenotype_Id
                        FROM tblindividual INNER JOIN
                        tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id
                        WHERE (tblindividual.Call_Rate IS NOT NULL)";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                while (Reader.Read())
                {
                    Indv_list.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString() + " " + Reader.GetValue(3).ToString() + " " + Reader.GetValue(4).ToString());
                }
                conn.Close();
                Reader.Close();

                ArrayList Indv_found = new ArrayList();
                ArrayList Indv_not_found = new ArrayList();
                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                DA.Fill(ds, "samples");
                int count = 0;
                bool found = false;
                System.IO.StreamWriter output_file_compare = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Compare_defect.txt");
                output_file_compare.WriteLine("Indv_Id" + " " +  "Gender" + " " + "Call_Rate" + " " + "Line" + " " + "Defect" + " " + "Excel_Gender" + " " + "Excel_Line" + " " + "Excel_Defect");
                for (int j = 0; j < Indv_list.Count; j++)
                {
                    foreach (DataRow dr in ds.Tables["samples"].Rows)
                    {
                        if (Indv_list[j].ToString().Split(' ')[0].Equals(dr[0].ToString().Trim()))
                        {
                            string[] parts = Indv_list[j].ToString().Split(' ');
                            string Excel_Gender = "";
                            if (dr[8].ToString().Trim().CompareTo("1") == 0)
                            {
                                Excel_Gender = "Male";
                            }
                            else if (dr[8].ToString().Trim().CompareTo("2") == 0)
                            {
                                Excel_Gender = "Female";
                            }
                            else
                            {
                                Excel_Gender = "Unknown";
                            }
                            
                            var Excel_defect = hshTable[dr[22].ToString().Trim()];
                            output_file_compare.WriteLine(parts[0] + " " + parts[1] + " " + parts[2] + " " + parts[3] + " " + parts[4] + " " + Excel_Gender + " " + dr[23].ToString().Trim() + " " + Excel_defect.ToString());
                            found = true;
                            break;
                        }

                    }
                    if (!found)
                    {
                        output_file_compare.WriteLine(Indv_list[j].ToString().Split(' ')[0]);
                    }
                    found = false;

                }
                output_file_compare.Flush();
                output_file_compare.Close();
            }

            public static void insert_box_44_45()
            {
                int count = 0;
                string line = "";
                char[] delimiters = new char[] { '\t' };
                char[] delimiters2 = new char[] { '.' };

                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Old_Samples Table.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        if (count != 0)
                        {
                            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (line_parts[1].CompareTo("4944") == 0 || line_parts[1].CompareTo("4945") == 0)
                            {
                                Box_DNA box_dna = new Box_DNA();
                                box_dna.DNA_Id = line_parts[0];
                                box_dna.Box_Id = line_parts[1];
                                excute_Box_DNA(box_dna);        // Insert The BOX_DNA information
                            }
                        }
                        count++;
                    }
                }
            }

            public static int marker_included()
            {
 

                return 1;
            }

            public static void hetero_missing()
            {
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"select count(*) from tblmarker_loc where included = 1";
                Reader = command.ExecuteReader();
                ArrayList Indv_list = new ArrayList();
                int total_markers = 0;
                while (Reader.Read())
                {
                    total_markers = int.Parse(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                conn.Open();
                /////////////////////////////////////
                command.CommandText = @"SELECT tblindividual.Indv_Id
                            FROM tblindividual INNER JOIN
                            tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id
                            WHERE (tblindividual.Call_Rate > 0.6)";
                Reader = command.ExecuteReader();
                ArrayList indv_list = new ArrayList();
                while (Reader.Read())
                {
                    indv_list.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                conn.Open();
                int hetero = 0;
                int missing_markers = 0;
                int row = 1;
                /////////////////////////////////////////
                System.IO.StreamWriter output_file_hetero = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\hetero_missing.txt");
                foreach (string indv in indv_list)
                {
                    row = 1;
                    hetero = 0;
                    missing_markers = 0;
                    command.CommandText = @"SELECT tblindividual.Indv_Id, COUNT(*) AS counter, tblsnp.Genotype, tblindividual.Call_Rate
                         FROM tblindividual INNER JOIN
                         tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
                         tblsnp ON tblindividual_sample.Sample_Id = tblsnp.Sample_Id INNER JOIN
                         tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                         WHERE (tblmarker_loc.Included = 1) AND (tblindividual.Indv_Id = '" + indv + @"') AND (tblsnp.Genotype = 'AB' OR
                         tblsnp.Genotype = 'NC')
                         GROUP BY tblsnp.Genotype
                         ORDER BY tblsnp.Genotype";
                    Reader = command.ExecuteReader();
                    ArrayList marker_list = new ArrayList();
                    while (Reader.Read())
                    {
                        if (float.Parse(Reader.GetValue(3).ToString()) == 1)
                        {
                            hetero = int.Parse(Reader.GetValue(1).ToString());
                            missing_markers = 0;
                            output_file_hetero.WriteLine(Reader.GetValue(0).ToString() + " " + String.Format("{0:0.####}", (double)hetero / (total_markers - missing_markers)) + " " + String.Format("{0:0.####}", ((double)missing_markers / total_markers)));
                        }
                        else
                        {
                            if (row == 1)
                            {
                                hetero = int.Parse(Reader.GetValue(1).ToString());
                            }
                            else
                            {
                                missing_markers = int.Parse(Reader.GetValue(1).ToString());
                                output_file_hetero.WriteLine(Reader.GetValue(0).ToString() + " " + String.Format("{0:0.#######}", (double)hetero / (total_markers - missing_markers)) + " " + String.Format("{0:0.#######}", ((double)missing_markers / total_markers)));
                            }
                        }
                        row++;
                    }
                    Reader.Close();
                    conn.Close();
                    conn.Open();
                }
                output_file_hetero.Flush();
                output_file_hetero.Close();
            }

//            static public void call_rate_update()
//            {
//                string line = "";
//                int i = 0;
//                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Newest_formate.ped"))
//                {//
//                    while ((line = file.ReadLine()) != null)
//                    {
//                        string[] line_parts = line.Split(' ');
//                        for (int j = 0; j < line_parts.Length; j++)
//                        {
//                            Data_Set[i, j] = line_parts[j];
//                        }
//                        i++;
//                       
//                    }
//                }
//
//                int marker_index = 1;
//                string line_map = "";
//                string line_ped = "";
//                int missing = 0;
//                System.IO.StreamWriter output_file_hetero = new System.IO.StreamWriter(@"D:\Master of Bioinformatics\GIGA Lab\Analysis\C#\Phasebook\hetero_missing.txt");
//                using (StreamReader file_map = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Newest_formate.map"))
//                {//
//                    while ((line_map = file_map.ReadLine()) != null)
//                    {
//
//                            for (i = 0; i < 861; i++)
//                            {
//                                if (Data_Set[i, marker_index].CompareTo("NC") == 0)
//                                {
//                                    missing++;
//                                }
//                            }
//                            Marker_Loc marker = new Marker_Loc();
//                            marker.Marker_Id = line_map.Split(' ')[1];
//                            marker.Call_Rate = 1 - float.Parse(String.Format("{0:0.#####}", (double)missing / 861));
//                            Update_Call_Rate_Marker(marker);
//                            missing = 0;
//                            System.Console.WriteLine(marker_index);
//                            marker_index++;
//                        
//                    }
//                }
//            }

            public static int Indv_exists(string indv_id)
            {
                int return_value = 0;
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = "SELECT COUNT(*) AS counter FROM tblindividual WHERE (Indv_Id = '" + indv_id.ToString() + "')";
                Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    if (int.Parse(Reader.GetValue(0).ToString()) == 0)
                    {
                        return_value = 0;
                    }
                    else
                    {
                        return_value = 1;
                    }
                }
                conn.Close();
                Reader.Close();
                return return_value;
            }

            public static void change_old_ids()
            {
                string ConString = "SERVER=localhost;" +
                                  "DATABASE=db_Proc;" +
                                  "UID=root;" +
                                  "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblpedigree.Maternal_Id, tblpedigree.Paternal_Id, tblindividual_phenotype.Phenotype_Id
                         FROM  tblindividual INNER JOIN
                         tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id INNER JOIN
                         tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id
                         WHERE (tblindividual.Call_Rate IS NOT NULL) AND (tblindividual_phenotype.Phenotype_Id <> 8) AND (tblindividual_phenotype.Phenotype_Id <> 9)";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                ArrayList change_List = new ArrayList();
                while (Reader.Read())
                {
                    if (Reader.GetValue(1).ToString().Substring(0,2).CompareTo("NS") == 0 )
                    {
                        if (!exists(Reader.GetValue(1).ToString(), Indv_List))
                        {
                            Indv_List.Add(Reader.GetValue(1).ToString());
                        }
                        if (!exists(Reader.GetValue(0).ToString(), change_List))
                        {
                            change_List.Add(Reader.GetValue(0).ToString());
                        }
                        
                    }
                    if (Reader.GetValue(2).ToString().Substring(0, 2).CompareTo("NS") == 0)
                    {
                        if (!exists(Reader.GetValue(2).ToString(), Indv_List))
                        {
                            Indv_List.Add(Reader.GetValue(2).ToString());
                        }
                        if (!exists(Reader.GetValue(0).ToString(), change_List))
                        {
                            change_List.Add(Reader.GetValue(0).ToString());
                        }
                    }

                }
                conn.Close();
                Reader.Close();

                foreach (string indv in change_List)
                {
                    Pedigree_update(indv);
                }

           
            }

            static public void read()
            {
                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Id_Book.xlsx";
                string CnStr = string.Empty;
                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }
                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [Sheet1$]", CnStr);
                DA.Fill(ds, "Sheet1");
                int counter = 0;

                counter = 0;
                foreach (DataRow dr in ds.Tables["Sheet1"].Rows)
                {
                    if (counter > 0)
                    {
                        string tatoe = dr[1].ToString().Trim();
                        System.Console.WriteLine(tatoe);

                    }
                    counter++;
                }
            }

            public static void No_offspring()
            {
               
                string line = "";
                ArrayList Indv_List = new ArrayList();
                System.IO.StreamWriter output_file_offspring = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\offspring2.txt");
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\ANimals with no offspring.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        Indv_List.Add(line);
                    }
                }
                string F1Name = "C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test\\Echantillons_Pigendef_Louvain.xls";
                string CnStr = string.Empty;
                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }
                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [Sheet1$]", CnStr);
                DA.Fill(ds, "Sheet1");
                foreach (string indv_id in Indv_List)
                {
                    foreach (DataRow dr in ds.Tables["Sheet1"].Rows)
                    {
                        if (dr[11].ToString().Trim().CompareTo(indv_id) == 0 || dr[16].ToString().Trim().CompareTo(indv_id) == 0)
                        {
                            output_file_offspring.WriteLine(dr[0].ToString() + " " + dr[11].ToString() + " " + dr[16].ToString() + " " + dr[22].ToString() + " " + dr[23].ToString() + " " + dr[24].ToString());
                        }
                    }
                }
                output_file_offspring.Flush();
                output_file_offspring.Close();
            }

            public static void FDR()
            {
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT Marker_Id, P23,P33,P36,P12,P15 FROM tblmarker_loc  WHERE (Chr <> 'X') AND (Chr <> 'Y') AND (call_rate >=0.98) AND ((AA_count != 0 AND BB_count != 0) or (AA_count != 0 AND AB_count != 0)  or (AB_count != 0 AND BB_count != 0) ) ORDER BY 0 + Chr, Position, Marker_Id";
                Reader = command.ExecuteReader();
                System.IO.StreamWriter output_file_FDR = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\autosome_pvalues.txt");
                while (Reader.Read())
                {
                    output_file_FDR.WriteLine(Reader.GetValue(0).ToString() + "\t" + double.Parse(Reader.GetValue(1).ToString()) + "\t" +  double.Parse(Reader.GetValue(2).ToString()) + "\t" +  double.Parse(Reader.GetValue(3).ToString()) + "\t" +  double.Parse(Reader.GetValue(4).ToString()) + "\t" +  double.Parse(Reader.GetValue(5).ToString()));
                }
                conn.Close();
                Reader.Close();
                output_file_FDR.Flush();
                output_file_FDR.Close();
            }

            public static void insert_parent()
            {
                string ConString = "SERVER=localhost;" +
                                "DATABASE=db_Proc;" +
                                "UID=root;" +
                                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT tblindividual.Indv_Id
                         FROM tblindividual INNER JOIN
                         tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id
                         WHERE (tblindividual_phenotype.Phenotype_Id = 8) AND (tblindividual.Call_Rate IS NOT NULL)";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Pedigree ped = new Pedigree();
                    ped.Indv_Id = Reader.GetValue(0).ToString();
                    ped.Maternal_Id = "0";
                    ped.Paternal_Id = "0";
                    ped.Maternal_Id_Modified = null;
                    ped.Paternal_Id_Modified = null;
                    ped.Modified = 0;
                    ped.Num_Offspring = null;
                    ped.Num_Siblings = null;
                    excute_Pedigree(ped);

                }
                Reader.Close();
                conn.Close();
            }

            public static void window(string Line)
            {
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT call_rate , log10(1/P" + Line.ToString() + @"),Marker_id  FROM tblmarker_loc  WHERE (Chr <> 'X') AND (Chr <> 'Y') AND (P" + Line.ToString() + @" < 10) and (call_rate >= 0.8) AND ((AA_count != 0 AND BB_count != 0) or (AA_count != 0 AND AB_count != 0)  or (AB_count != 0 AND BB_count != 0) ) ORDER BY call_rate";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString());
                }
                Reader.Close();
                conn.Close();

                ArrayList win = new ArrayList();

                double[] win_space = new double[] 
                { 0.80,0.8025,0.8050,0.8075,0.8100,0.8125,0.8150,0.8175,0.8200,0.8225,
                0.8250,0.8275,0.8300,0.8325,0.8350,0.8375,0.8400,0.8425,0.8450,0.8475,
                0.8500,0.8525,0.8550,0.8575,0.8600,0.8625,0.8650,0.8675,0.8700,0.8725,
                0.8750,0.8775,0.8800,0.8825,0.8850,0.8875,0.8900,0.8925,0.8950,0.8975,
                0.9000,0.9025,0.9050,0.9075,0.9100,0.9125,0.9150,0.9175,0.9200,0.9225,
                0.9250,0.9275,0.9300,0.9325,0.9350,0.9375,0.9400,0.9425,0.9450,0.9475,
                0.9500,0.9525,0.9550,0.9575,0.9600,0.9625,0.9650,0.9675,0.9700,0.9725,
                0.9750,0.9775,0.9800,0.9825,0.9850,0.9875,0.9900,0.9925,0.9950,0.9975,
                1 };
                int[] upper = new int[win_space.Length - 1];
                int[] lower = new int[win_space.Length - 1];
                for (int j = 0; j <= win_space.Length - 1; j++)
                {
                    if (j < 80)
                    {
                        upper[j] = 1;
                        lower[j] = 1;
                    }
                    for (int i = 0; i < Indv_List.Count; i++)
                    {
                        if (double.Parse(Indv_List[i].ToString().Split(' ')[0]) > win_space[j] && double.Parse(Indv_List[i].ToString().Split(' ')[0]) <= win_space[j + 1])
                        {
                            if (double.Parse(Indv_List[i].ToString().Split(' ')[1]) > 1)
                            {
                                upper[j] = upper[j] + 1;
                            }
                            else
                            {
                                lower[j] = lower[j] + 1;
                            }
                        }
                    }
                }
                System.IO.StreamWriter output_file_window = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\window_X_" + Line + ".txt");
                for (int i = lower.Length - 1; i >= 0 ; i--)
                {
                    output_file_window.WriteLine((double)upper[i] / (lower[i]+ upper[i]));
                }
                output_file_window.Flush();
                output_file_window.Close();
            }

            public static void update_line()
            {
                string ConString = "SERVER=localhost;" +
                                "DATABASE=db_Proc;" +
                                "UID=root;" +
                                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @"SELECT Indv_Id
                                    FROM tblindividual
                                    WHERE (Line = 0) AND (Call_Rate IS NOT NULL)";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                while (Reader.Read())
                {
                    Indv_List.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                string F1Name = "D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Echantillons_Pigendef_Louvain.xls";

                string CnStr = string.Empty;

                if (Path.GetExtension(F1Name) == ".xlsx")
                {
                    CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + F1Name +
                        ";Extended Properties=Excel 12.0;";
                }
                else
                {
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + F1Name + ";Extended Properties=Excel 8.0;";
                }

                DataSet ds = new DataSet();
                OleDbDataAdapter DA = new OleDbDataAdapter("Select * from [samples$]", CnStr);
                DA.Fill(ds, "samples");
                for (int j = 0; j < Indv_List.Count; j++)
                    {
                        foreach (DataRow dr in ds.Tables["samples"].Rows)
                        {
                            if (Indv_List[j].Equals(dr[0].ToString().Trim()))
                            {
                                Update_Individual_line(dr[0].ToString().Trim(), dr[23].ToString().Trim());
                            }

                        }


                    }

                DA.Dispose();
            }

            public static string check_line(string indv_id)
            {
                string Line = "";
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlDataReader Reader;
                cmd.CommandText = "select line from db_proc.tblIndividual where Indv_Id = '" + indv_id + "'";
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Line = Reader.GetValue(0).ToString();
                }
                Reader.Close();
                conn.Close();
                return Line;

            }

            public static IEnumerable<float> Range(float min, float max, float step)
            {
                float i;
                for (i = min; i <= max; i += step)
                    yield return i;

                if (i != max + step) // added only because you want max to be returned as last item
                    yield return max;
            }

            public static void exclude_Hwe()
            {
                string line = "";
                ArrayList HWD = new ArrayList();
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\autosome_pvalues.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split('\t');
                        if (double.Parse(line_parts[12].ToString()) <= 0.0003162278)
                        {
                            HWD.Add(line_parts[0]);
                        }
                    }
                }
                foreach (string marker in HWD)
                {
                    Exclude_Marker(marker.ToString());
                }
            }

            public static void update_indv_callrate()
            {
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 3000000;
                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Line
                                    FROM tblindividual INNER JOIN
                                    tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id
                                    WHERE (tblindividual.Call_Rate > 0.6)";
                Reader = command.ExecuteReader();
                ArrayList indv_list = new ArrayList();
                while (Reader.Read())
                {
                    indv_list.Add(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                conn.Open();
                /////////////////////////////////////////////////
                command.CommandText = @"select count(*) from tblmarker_loc where included = 1";
                Reader = command.ExecuteReader();
                int total_markers = 0;
                while (Reader.Read())
                {
                    total_markers = int.Parse(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                conn.Open();
                /////////////////////////////////////////////////
                foreach (string indv_id in indv_list)
                {
                    command.CommandText = @"SELECT tblindividual.Indv_Id, COUNT(*) AS missing
                         FROM tblindividual INNER JOIN
                         tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
                         tblsnp ON tblindividual_sample.Sample_Id = tblsnp.Sample_Id INNER JOIN
                         tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id
                         WHERE (tblmarker_loc.Included = 1) AND (tblindividual.Indv_Id = '" + indv_id + @"') AND (tblsnp.Genotype = 'NC')";
                    Reader = command.ExecuteReader();
                    while (Reader.Read())
                    {
                        Individual indv = new Individual();
                        indv.Indv_Id = indv_id;
                        indv.Call_Rate = float.Parse( ((float)(total_markers - float.Parse(Reader.GetValue(1).ToString()))/total_markers).ToString() );
                        Update_Call_Rate(indv);
                    }
                    Reader.Close();
                    conn.Close();
                    conn.Open();
                }
                conn.Close();

            }

            public static void excluded_Indv()
            {
                string ConString = "SERVER=localhost;" +
                                    "DATABASE=db_Proc;" +
                                    "UID=root;" +
                                    "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 3000000;
                command.CommandText = @"SELECT tblindividual.Indv_Id, call_rate
                                      FROM tblindividual where call_rate >= 0.95";
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Include_Individual(Reader.GetValue(0).ToString());
                }
                Reader.Close();
                conn.Close();
                conn.Open();
            }

            public static void duplicates(string indv1,string indv2)
            {
                string line = "";
                int i = 0;

                int IBS2 = 0;
                int IBS1 = 0;
                int IBS0 = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                         string[] line_parts = line.Split(' ');
                         if (line_parts[0].CompareTo(indv1) == 0 || line_parts[0].CompareTo(indv2) == 0)
                         {

                             for (int j = 0; j < line_parts.Length; j++)
                             {
                                 Data_Set[i, j] = line_parts[j];
                             }
                             i++;
                         }
                    }

                   //double res = Parent_offspring_test(0, 1);

                    for (i = 0; i < Data_Set.GetLength(1); i++)
                    {
                        if(Data_Set[0, i].CompareTo(Data_Set[1, i]) == 0)
                        {
                            IBS2++;
                        }
                        else if (Data_Set[0, i].CompareTo("AB") == 0 && Data_Set[1, i].CompareTo("AA") == 0
                              || Data_Set[0, i].CompareTo("AA") == 0 && Data_Set[1, i].CompareTo("AB") == 0
                              || Data_Set[0, i].CompareTo("AB") == 0 && Data_Set[1, i].CompareTo("BB") == 0
                              || Data_Set[0, i].CompareTo("BB") == 0 && Data_Set[1, i].CompareTo("AB") == 0)
                        {
                            IBS1++;
                        }
                        else
                        {
                            IBS0++;
                        }
                    }
                }
                System.Console.WriteLine((double)(IBS2 + 0.5 * IBS1) / (IBS2+IBS1+IBS0));
            }

            public static void Markers_Conflict()
            {
                string line = "";
                int i = 0;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.ped"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');

                        for (int j = 0; j < line_parts.Length; j++)
                        {
                            Data_Set[i, j] = line_parts[j];
                        }
                        i++;
                    }
                }

                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\good_trios.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split('\t');
                         
                        string Indv_Id = line_parts[0];
                        string father = line_parts[1];
                        string mother = line_parts[2];
                        double conflict = check_Trios(Indv_Id, father, mother);

                    }
                }
                ArrayList Marker_List = new ArrayList();
                Marker_List.Add("Skip first line");
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\my_formate.map"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split(' ');
                        Marker_List.Add(line_parts[0] + " " + line_parts[1]);
                    }
                }


                System.IO.StreamWriter output_file_conflict = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Markers_Conflict.txt");
                for (i = 1; i < Data_Set_Conflict.Length; i++)
                {
                    output_file_conflict.WriteLine(Marker_List[i] + " " + Data_Set_Conflict[i].ToString());
                }
                output_file_conflict.Flush();
                output_file_conflict.Close();
            }

            public static void construct_trios()
            {
                string line = "";
                ArrayList Father = new ArrayList();
                ArrayList Mother = new ArrayList();
                ArrayList No_parents = new ArrayList();
                ArrayList Both = new ArrayList();
                System.IO.StreamWriter output_file_bad = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Bad_Trios.txt");
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Database_Pairs_2.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split('\t');
                        if (double.Parse(line_parts[5]) <= 0.005 && double.Parse(line_parts[6]) <= 0.005)
                        {
                            Both.Add(line);
                        }
                        else if (double.Parse(line_parts[5]) > 0.005 && double.Parse(line_parts[6]) > 0.005)
                        {
                            No_parents.Add(line);
                            output_file_bad.WriteLine(line_parts[0] + " 0 0 1");
                        }
                        else if (double.Parse(line_parts[5]) <= 0.005)
                        {
                            Father.Add(line);
                            output_file_bad.WriteLine(line_parts[0] + " " + line_parts[3] + " 0 " + line_parts[5]);
                        }
                        else if (double.Parse(line_parts[6]) <= 0.005)
                        {
                            Mother.Add(line);
                            output_file_bad.WriteLine(line_parts[0] + " 0 " + line_parts[4] + " " + line_parts[6]);
                        }
                        else
                        {
                        }

                    }
                }
                output_file_bad.Flush();
                output_file_bad.Close();
            }

            public static void Low_trios_check()
            {
                string line = "";
                
                System.IO.StreamWriter output_file_check = new System.IO.StreamWriter(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\130_Trios_Checked.txt");
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\130_Trios.txt"))
                {//
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] line_parts = line.Split('\t');
                        output_file_check.WriteLine(line_parts[0] + " " + line_parts[1] + " " + line_parts[2] + " " + get_parents(line_parts[0]) + " " + line_parts[3]);

                    }
                }
                output_file_check.Flush();
                output_file_check.Close();
            }
            public static string get_parents(string indv_id)
            {
                string parents = " ";
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 300000;
                MySqlDataReader Reader;
                command.CommandText = @" SELECT tblpedigree.Indv_Id, tblpedigree.Paternal_Id, tblpedigree.Maternal_Id
                         FROM tblindividual INNER JOIN
                         tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id
                         where tblindividual.indv_id ='" + indv_id + "'";
                Reader = command.ExecuteReader();
                ArrayList Indv_List = new ArrayList();
                
                while (Reader.Read())
                {
                    parents =   Reader.GetValue(1).ToString() + " " + Reader.GetValue(2).ToString();
                     
                }
                conn.Close();
                Reader.Close();
                return parents;
            }


            //public static void Update_Genotype(string Genotype,string marker_id, string indv_id)
            //{
            //    string ConString = "SERVER=localhost;" +
            //    "DATABASE=db_Proc;" +
            //    "UID=root;" +
            //    "PASSWORD=mahmoud;";
            //    MySqlConnection conn = new MySqlConnection(ConString);
            //    conn.Open();
            //    MySqlCommand command = conn.CreateCommand();
            //    MySqlDataReader Reader;
            //    command.CommandText = "Update db_proc.tblsnp set Genotype_2='" + ped.Maternal_Id_Modified + "' , Paternal_Id_Modified ='" + ped.Paternal_Id_Modified + "' , Modified = 1 Where Indv_Id='" + ped.Indv_Id + "';";
            //    Reader = command.ExecuteReader();
            //    Reader.Close();
            //    conn.Close();
            //}

            public static string[,] file_Data = new string[864, 62164];
            public static void update_snp()
            {
                ArrayList result = new ArrayList();
                string ConString = "SERVER=localhost;" +
                "DATABASE=db_Proc;" +
                "UID=root;" +
                "PASSWORD=mahmoud;";
                MySqlConnection conn = new MySqlConnection(ConString);
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                MySqlDataReader Reader;
                command.CommandTimeout = 300000;
                command.CommandText = "SELECT indv_id,sample_id from db_Proc.tblindividual_sample";
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    result.Add(Reader.GetValue(0).ToString() + " " + Reader.GetValue(1).ToString());

                }
                Reader.Close();
                conn.Close();
                

                ArrayList indv_list = new ArrayList();
                ArrayList marker_list = new ArrayList();
                ArrayList marker_list_old = new ArrayList();
                string lines;
                int i = 0;
                int marker_id = 1;
                using (StreamReader file = new StreamReader(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\porc_Test\Full Data Table_v2.txt"))
                {//
                    while ((lines = file.ReadLine()) != null)
                    {

                        if (i == 0)
                        {
                            string[] line_parts = lines.Split('\t');
                            for (int j = 1; j < line_parts.GetLength(0); j++)
                            {
                                indv_list.Add(line_parts[j].Split('.')[0]);
                            }

                            for (int k = 0; k < 864; k++)
                            {
                                file_Data[k, 0] = indv_list[k].ToString();
                            }
                        }
                        else
                        {
                            string[] line_parts = lines.Split('\t');
                            marker_list.Add(line_parts[0]);

                            for (int j = 0; j < line_parts.Length - 1; j++)
                            {
                                file_Data[j, marker_id] = line_parts[j + 1];
                            }
                            marker_id++;
                        }
                        i++;
                    }
                }


                int found = 0;
                bool exists = false;
                ArrayList notfound = new ArrayList();
                foreach (string indv1 in indv_list)
                {
                    foreach (string indv2 in result)
                    {
                        if (indv1.CompareTo(indv2.Split(' ')[0]) == 0 || indv1.CompareTo(indv2.Split(' ')[1]) == 0)
                        {
                            found++;
                            exists = true;
                            break;
                        }


                    }
                    if (!exists)
                    {
                        notfound.Add(indv1);
                    }
                    exists = false;
                }
            
            }

            static void Main(string[] args)
            {

                update_snp();
                //My_Format();
                //Low_trios_check();
                //construct_trios();
                //Markers_Conflict();
                //pred_trios();
                //Plink_Format();
                //duplicates("BA003516", "BA004737");
                //duplicates("BA003516", "BA002738");

                //duplicates("BA001575", "BA003299");
                //duplicates("BA001575", "BA004760");

                //duplicates("BA004660", "BA002544");
                //duplicates("BA004660", "BA002748");

                //duplicates("BA003264", "BA002544");
                //duplicates("BA003264", "BA002676");


                //translate();
                //DataBase_Pairs();
                //paire_conflict_All();
                //compare_Trios_2();
                //My_Format();
                //Poseudoautosomal_file();
                //SNP_sex();
                //excluded_Indv();
                //hetero_missing();
                //update_indv_callrate();
                //execlude_marker_individual();
                //exclude_Hwe();
                //Hardy_Wienburg_Test("33");
                //Hardy_Wienburg_Test_Female("15");
                //Hardy_Wienburg_Test_Female("36");
                //Hardy_Wienburg_Test_Female("12");
                //Hardy_Wienburg_Test_Female("15");
                ////double[] result = Enumerable.Repeat(zmin, N).Select(iv => (iv + (zmax - zmin) / count)).ToArray();
                //IEnumerable<float> res = Range(0.8F, 1F, 0.01F);
                //float[] arr = res.ToArray();
                //update_line();
                //Exact_Test();
                //call_rate_update();
                 //window("33");     
                //window("36");
                //window("12");
                //window("33");
                //window("15");
                //double x = SNPHWE(11,56,263);
                //insert_parent();
                //Plink_Format();
                //FDR();
                
                
                //execlude_marker_individual();

                //No_offspring();
               
                //My_Format_founders();
                //create_pedigree();
                //read();
                //change_old_ids();
                //call_rate_update();
                //hetero_missing();
                //My_Format();
                //insert_box_44_45();
                //defects_names();
                //check_new_patch();
                //check_data();
                //check_data_defect();
                //defects_names();
                //insert_defect_access();
                //create_pedigree();
               //insert_call_rate();
                //original_paire();
                //conflict();
                //paire_conflict_All();

                //compare_Trios_2();
                //function2();
                //Parental_Conflict_All();
                // Plink_Format();
                //int i = 0;
                //string line = "";
                //using (StreamReader file = new StreamReader(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\My.ped"))
                //{//
                //    while ((line = file.ReadLine()) != null)
                //    {
                //        string[] line_parts = line.Split(' ');

                //        for (int j = 0; j < line_parts.Length; j++)
                //        {
                //            Data_Set[i, j] = line_parts[j];
                //        }
                //        i++;
                //    }
                //}
                //line = "";
                //i = 0;
                //using (StreamReader file = new StreamReader(@"C:\Users\Elansary\Documents\Visual Studio 2010\Projects\Porc\P_C_conflict\porc.map"))
                //{//
                //    while ((line = file.ReadLine()) != null)
                //    {
                //        Data_Set_map.Add(line);
                //    }
                //}

                //check_Trios(0, 1, 2);



                //Marker_Error("ASGA0094373");
                //fun("Errors_ASGA0094373_SR");

                //Marker_Error("ASGA0102530");
                //fun("Errors_ASGA0102530_SR");

                //Marker_Error("H3GA0053809");
                //fun("Errors_H3GA0053809_SR");

                //Marker_Error("MARC0056935");
                //fun("Errors_MARC0056935_SR");

                //Marker_Error("ASGA0089696");
                //fun("Errors_ASGA0089696_SR");

                //Marker_Error("DRGA0006183");
                //fun("Errors_ASGA0094373_SR");

                //Marker_Error("ASGA0038543");
                //fun("Errors_ASGA0038543_SR");
                //defect_6_high();
                //defect_6_low();

                //My_Format();
                //save_sample("7031539133_R05C01", "BA000283");
                //save_sample("6899176091_R02C02", "BA002681");
                //save_sample("4963349019_R05C01", "BA002741");
                //print_pedigree_2();
                //function2();
                //print_pedigree_2();
                //Update_Num_of_offspring();
                //Hardy_Wienburg_Test();
                //My_Format_founders();
                //plink_markers();
                //System.Console.WriteLine("");
                //compare_Trios_2();
                //Poseudoautosomal_file();


                //conflict();
                //compare_Trios();
                //selected_trios();
                //modified_pedigree();
                //exc_mark_my_formate();
                //My_Format();
                //SNP_sex();
                //compare_Trios();
                //Poseudoautosomal_file();
                //check_birth_date();
                //check_Age("BA002559");
                // compare_pedigree();
                // compare_pedigree();
                //conflict();
                //Plink_RG_marker();
                //fun("Errors_DRGA0006183_RG");
                //fun("Errors_ALGA0043906_RG");
                //fun("Errors_ALGA0105472_RG");
                //fun("Errors_ALGA0109178_RG");
                //fun("Errors_ALGA0109508_RG");
                //fun("Errors_ASGA0010510_RG");
                //fun("Errors_ASGA0089051_RG");
                //fun("Errors_ASGA0093913_RG");
                //fun("Errors_DRGA0006183_RG");
                //fun("Errors_H3GA0010203_RG");
                //fun("Errors_H3GA0053809_RG");
                //fun("Errors_M1GA0002940_RG");
                //fun("Errors_MARC0025211_RG");
                //fun("Errors_MARC0081387_RG");

                //check_RG();
                //change_plink_family();
                //change_plink_format();
                //string[] marker = new string[] { "ALGA0105472", "ALGA0109178", "ALGA0109508", "ASGA0089051", "ASGA0093913", "H3GA0053809", "MARC0081387", "M1GA0002940", "ASGA0010510", "H3GA0010203", "ALGA0027505", "DRGA0006183", "ALGA0043906", "MARC0025211" };
                //for (int i = 0; i < marker.Length; i++)
                //{
                //    Marker_frequency(marker[i]);
                //}
                //Marker_frequency("ALGA0109178");
                //Marker_frequency("DRGA0006183");
                //Marker_frequency("MARC0025211");
                //Marker_frequency("ALGA0043906");

                //change_plink_format();
                // print_pedigree();
                // testing();
                //System.Console.WriteLine(1 - SpecialFunction.chisq(1, 4.571));
                //Plink_Format();
                //Parental_Conflict_All();
                //Parental_Conflict();
                //HWE_X_Chr();
                // float res = SNPHWE(100, 8, 50);
                //System.Console.Write(res);
                //IBS_Test_v2();
                // test();
                //excute_founder();
                //Plink_Format();
                //exclude_marker_exact_test();
                //Exact_Test();
                //HWE_markers();
                //create_pedigree();
                //Phasebook_format();
                //  string ConString = "SERVER=localhost;" +
                //"DATABASE=db_Proc;" +
                //"UID=root;" +
                //"PASSWORD=mahmoud;";
                //  MySqlConnection conn = new MySqlConnection(ConString);
                //  conn.Open();
                //  MySqlCommand command = conn.CreateCommand();
                //  command.CommandTimeout = 300000;
                //  MySqlDataReader Reader;
                //  command.CommandText = @"select * from tblindividual where line = 0;";
                //  Reader = command.ExecuteReader();
                //  ArrayList Indv_List = new ArrayList();
                //  while (Reader.Read())
                //  {
                //      Indv_List.Add(Reader.GetValue(0).ToString());
                //  }
                //  conn.Close();
                //  Reader.Close();

 
                //Insert_Sample();
                //check_individuals(Indv_List);
                //Update_Num_of_offspring();
                //defects_names();
                //flag_old_dataset(Indv_List);
                //insert_indv_pheno(Indv_List);
                //execlude_marker_individual();
                //execlude_marker_MAF();
                //insert_new_file(Indv_List);

                //SNP_sex();

                //IBS_Test();
                //CSV_Format();
                //HWE_markers();

                //check_founder();
                //Plink_Format();
                //System.Console.WriteLine("Finished");






                //        /////////////////////////////////////////////////////////////////////////////////////////
                //                command.CommandText = @"SELECT Marker_Id, AA_Freq, AB_Freq, BB_Freq, A_Freq, B_Freq, Chi2_P_Value, Chr FROM tblmarker_loc
                //                         WHERE (Chr <> 'X') AND (Chr <> 'Y') AND (AA_Freq > 0) OR
                //                         (Chr <> 'X') AND (Chr <> 'Y') AND (AB_Freq > 0) OR
                //                         (Chr <> 'X') AND (Chr <> 'Y') AND (BB_Freq > 0)";
                //                Reader = command.ExecuteReader();
                //               // ArrayList Marker_List = new ArrayList();
                //                int count = 0;
                //                int HWE = 0;
                //                while (Reader.Read())
                //                {

                //                    if (double.Parse(Reader.GetValue(6).ToString()) > 0.05)
                //                    {
                //                        HWE++;
                //                    }
                //                    System.Console.WriteLine("Marker " + count + " is checked successfully");
                //                    count++;
                //                }
                //                conn.Close();
                //                Reader.Close();

                //                ///////////////////// Modifing pidegree   ////////////////////////
                //                //char[] delimiters = new char[] { '\t' };
                //                //int count = 0;
                //                //string line = "";
                //                //using (StreamReader file = new StreamReader("C:/Users/Elansary/Documents/Visual Studio 2010/Projects/Porc/pedigree.txt"))
                //                //{//
                //                //    while ((line = file.ReadLine()) != null)
                //                //    {
                //                //        if (count != 0)
                //                //        {
                //                //            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                //                //            Pedigree ped = new Pedigree();
                //                //            ped.Indv_Id = line_parts[0];
                //                //            ped.Maternal_Id = line_parts[1];
                //                //            ped.Paternal_Id = line_parts[2];
                //                //            ped.Maternal_Id_Modified = line_parts[3];
                //                //            ped.Paternal_Id_Modified = line_parts[4];
                //                //            ped.Modified = int.Parse(line_parts[5]);
                //                //            Update_Pedigree(ped);

                //                //        }
                //                //        count++;
                //                //    }
                //                //}

                //               //////////////////////////////////////////////////////////////////////////////////
                //              // Insert_Sample();

                //              // double x = SNPHWE(100, 100, 40);
                //                //defects_names();

                //                command.CommandText = @"SELECT tblindividual.Indv_Id, tblindividual.Line
                //                FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN
                //                tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id
                //                GROUP BY tblindividual.Indv_Id, tblindividual.Line";
                //                // HAVING (tblindividual.Line = 36)";
                //                Reader = command.ExecuteReader();
                //                ArrayList Indv_list = new ArrayList();


                //                while (Reader.Read())
                //                {
                //                    Indv_list.Add(Reader.GetValue(0).ToString());
                //                }
                //                Reader.Close();

                // check_individuals(Indv_list);



                //////////////////////////  Update Sex  ////////////////////////////
                //int count = 0;
                //string line = "";
                //char[] delimiters = new char[] { '\t' };
                //char[] delimiters2 = new char[] { '.' };
                //using (StreamReader file = new StreamReader("C:/Users/Elansary/Documents/Visual Studio 2010/Projects/Porc/Samples Table.txt"))
                //{//
                //    while ((line = file.ReadLine()) != null)
                //    {
                //        if (count != 0)
                //        {
                //            string[] line_parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                //            Individual indv = new Individual();
                //            indv.Indv_Id = line_parts[17];
                //            indv.Gender = line_parts[10];
                //            Update_Individual(indv);
                //        }
                //        count++;
                //    }
                //}

            }
        }
    }
