library('robustbase')
library('RODBC')
library('ROCR')
library('Daim')
library('qtl')
query_pedigree = function()
{
	
	channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
	query = "SELECT tblindividual.indv_id,tblindividual.`Index`, IF(Gender = 'Male', '1', '2') AS sex,tblindividual.line, IF(tblpedigree.Maternal_Id = 'General', '0', tblpedigree.Maternal_Id) as Maternal_Id ,(select `Index` from tblindividual INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id where tblindividual.indv_id = tblpedigree.Maternal_Id and tblindividual.included = 1 and tblindividual.call_rate is not null ) as Maternal_call_rate,if(tblpedigree.Paternal_Id = 'General','0',tblpedigree.Paternal_Id) as Paternal_Id ,(select `Index` from tblindividual INNER JOIN tblindividual_phenotype ON tblindividual_phenotype.Indv_Id = tblindividual.Indv_Id Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id where tblindividual.indv_id = tblpedigree.Paternal_Id and tblindividual.included = 1 and tblindividual.call_rate is not null ) AS Paternal_call_rate, if(tblindividual_phenotype.phenotype_ID = 2,'1','0') as defect FROM tblindividual INNER JOIN tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id inner join tblindividual_phenotype on  tblindividual.indv_id = tblindividual_phenotype.indv_id Inner Join tblindividual_sample on tblindividual_sample.Indv_id = tblindividual.indv_id WHERE tblindividual.included = 1 and tblindividual.call_rate is not null and (tblindividual_phenotype.Phenotype_Id = 2 or tblindividual_phenotype.Phenotype_Id = 8 or tblindividual_phenotype.Phenotype_Id = 9) order by line,indv_id"
	Data_set = sqlQuery(channel, query)
	odbcClose(channel)
	for(i in 1:nrow(Data_set))
	{
 
		if(is.na(Data_set$Maternal_call_rate[i]))	
		{
			Data_set$Maternal_Id[i] = 0;
			Data_set$Maternal_call_rate[i] = 0;
		}
			if(is.na(Data_set$Paternal_call_rate[i]))	
		{
			Data_set$Paternal_Id[i] = 0;
			Data_set$Paternal_call_rate[i] = 0;
		}
	
	}
	return(Data_set)
}


#####################			Pidegree Drawing			##############

setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data")
library('kinship2')
pre <-read.table("tree.txt", header = T)
pre$famid = rep(1,49)
pre$inf= rep(2,49)
attach(Data_set)
New = cbind(rep(1,nrow(Data_set)),as.numeric(Data_set$Index),  as.numeric(Data_set$Paternal_call_rate),as.numeric(Data_set$Maternal_call_rate), Data_set$sex,Data_set$defect)
ped = pedigree(as.numeric(Data_set$Index),  as.numeric(Data_set$Paternal_call_rate),as.numeric(Data_set$Maternal_call_rate), Data_set$sex,Data_set$defect, missid = 0)
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data\\");
write.table(New, file = "Pedigree.txt", sep  = "\t", quote = FALSE, row.names = FALSE);

par(xpd= T)
 
plot.pedigree(ped)

install.packages("graphviz")

library("gap")
library("graphviz")
pre = cbind(pid = 10081, pre)
dotty(pedtodot(pre))


pedAll <- pedigree(id=Data_set$Index,
dadid=Data_set$Paternal_call_rate, momid=Data_set$Maternal_call_rate,
sex=Data_set$sex, famid=Data_set$line)

plot(ped2basic)