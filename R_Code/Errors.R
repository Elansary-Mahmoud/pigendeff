setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\Diseases\\RG")
Trios = data.frame(cbind(seq(1:1:1), rep(0,1)));
Genotype = seq(1:1:27)
Gen ="";
type = c("AA","AB","BB");
counter = 1;

 
file =  read.table("Errors_MARC0081387_RG.txt",header=F)
flag = FALSE;
for(i in 1:nrow(file))
{
 
	for(j in 1:nrow(Trios))
	{
		if(Trios[j,1] == paste(file$V4[i]," ",file$V5[i]," ",file$V6[i],collapse = ""))
		{
			Trios[j,2] = Trios[j,2] + 1;
			flag = TRUE;
		}
	}
	if(!flag)
	{
		Trios[(nrow(Trios)+1),1] = paste(file$V4[i]," ",file$V5[i]," ",file$V6[i]," ",collapse = ""); 
		Trios[(nrow(Trios)+1),2] = Trios[(nrow(Trios)+1),2] + 1
	}
	flag = FALSE;
}

new_file = paste("ALGA0027505_RG"," ",AB_indv," ",AA_indv," ",BB_indv," ",AB_par," ",AA_par," ",BB_par)
write.table(new_file, file = "classification_Errors_ALGA0027505_RG.txt", sep  = "\t", quote = FALSE, row.names = FALSE);

