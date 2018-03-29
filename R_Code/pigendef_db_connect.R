 
rm(list=ls(all=TRUE)) 
library('robustbase')
library('rrcov')
library('RODBC')
library('ROCR')
library('Daim')
library('qtl')
library('ggplot2')
library('gap')
library('andrews')
library('bootstrap')
boot = bootstrap(results$P,100,mean);
?bootstrap
install.packages("bootstrap")

source("http://www.StephenTurner.us/qqman.r")
qqman(results)
manhattan(results)
plot(results[,2], results[,3])


Query <- function()
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = "SELECT tblindividual.Indv_Id, tblindividual.Gender FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id WHERE (tblindividual.call_rate is not null) and (tblindividual.included = 1) order by tblindividual.indv_id";
Data_Set=sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)

}

Query_Lines <- function(Line)
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = paste("SELECT tblindividual.Indv_Id, tblindividual.Line FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id GROUP BY tblindividual.Indv_Id, tblindividual.Line HAVING (tblindividual.Line =",Line,")",collapse = "");
Data_Set=sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)
}

Query_Pedigree <- function()
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = "SELECT        tblpedigree.Indv_Id, tblpedigree.Maternal_Id, tblpedigree.Paternal_Id, tblpedigree.Maternal_Id_Modified, tblpedigree.Paternal_Id_Modified, tblpedigree.Modified,tblpedigree.Num_Offspring, tblpedigree.Num_Siblings FROM tblindividual INNER JOIN tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id WHERE (tblindividual.Included = 1) ORDER BY tblpedigree.Indv_Id";
Data_Set=sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)
}

Query_Gender <- function(Indv)
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = paste("SELECT Gender FROM tblindividual where Indv_Id = '",Indv,"';",collapse = "",sep = "");
Data_Set = sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)
}

Update_Pidegree <- function(pedigree)
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
sqlUpdate(channel, Pedigree[,1:6], "tblpedigree",index = c("Indv_Id","Maternal_Id","Paternal_Id"),verbose = TRUE, test =TRUE, fast = TRUE);
colnames(Pedigree) = c("Indv_Id","Maternal_Id","Paternal_Id","D","E","F")
sqlColumns(channel, "tblpedigree", special = TRUE)
odbcClose(channel)
}
 
Query_Markers <- function()
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = "SELECT Marker_Id, AA_count, AB_count, BB_count, A_Freq, B_Freq, Chi2_P_Value, Chr FROM tblmarker_loc WHERE (Chr <> 'X') AND (Chr <> 'Y')";
Data_Set=sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)
}

Query_Not_NC <- function(Marker_Id)
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = paste("SELECT count(*) FROM tblmarker_loc INNER JOIN tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id INNER JOIN tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id WHERE(tblsnp.Marker_Id = '" , Marker_Id , "') AND (Genotype <> 'NC') AND (tblindividual.Included = 1) AND (genotype = 'NC') AND (tblindividual.indv_id in  (select indv_id from tblindividual_phenotype where phenotype_id = 8))",sep="",collapse="");
Data_Set = sqlQuery(channel, query)
odbcClose(channel)
return(216 - as.integer(Data_Set))
}

Query_Genotype <- function(Marker_Id)
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = paste("SELECT tblsnp.Genotype, count(*) FROM tblmarker_loc INNER JOIN tblsnp ON tblmarker_loc.Marker_Id = tblsnp.Marker_Id INNER JOIN tblindividual_sample ON tblsnp.Sample_Id = tblindividual_sample.Sample_Id INNER JOIN tblindividual ON tblindividual_sample.Indv_Id = tblindividual.Indv_Id INNER JOIN tblpedigree ON tblindividual.Indv_Id = tblpedigree.Indv_Id WHERE (tblsnp.Marker_Id ='" , Marker_Id , "') AND (tblindividual.Included = 1) AND (tblindividual.indv_id in  (SELECT tblindividual.Indv_Id FROM tblindividual INNER JOIN tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id WHERE (tblindividual.Call_Rate IS NOT NULL) AND (tblindividual.Included = 1) AND (tblindividual.Founder = 1) AND (tblindividual_phenotype.Phenotype_Id <> 2))) group by genotype ",sep="",collapse="");
Data_Set = sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)
}


Query_Markers_List_Modified <- function(Marker_Id)
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = paste("select Marker_id from tblmarker_loc where included =1 and chr<> 'X' and chr<> 'Y' and FORMAT(Founder_control,4) <> FORMAT(plink_p_value,4) ");
Data_Set = sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)
}

Query_Gender_2 <- function()
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = "SELECT IF(tblindividual.Gender = 'Male', 1, 0) as sex FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id WHERE (tblindividual.Included = 1) GROUP BY tblindividual.Indv_Id, tblindividual.Line";
Data_Set = sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)
}

Query_All_lines = function()
{
	channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
	query = "SELECT tblindividual.Indv_Id, tblindividual.Line FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN tblsample ON tblindividual_sample.Sample_Id = tblsample.Sample_Id WHERE (tblindividual.Included = 1)order by indv_id";
	Data_Set = sqlQuery(channel, query)
	odbcClose(channel)
	return(Data_Set)
}

Query_old_dataset = function()
{
	channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
	query = "select indv_id,Line from tblindividual where new_dataset = 1 and (Included = 1) AND (Call_Rate IS NOT NULL)";
	Data_Set = sqlQuery(channel, query)
	odbcClose(channel)
	return(Data_Set)
}

SNPHWE <- function(obs_hets, obs_hom1, obs_hom2)
   {
   if (obs_hom1 < 0 || obs_hom2 < 0 || obs_hets < 0)
      return(-1.0)

   # total number of genotypes
   N <- obs_hom1 + obs_hom2 + obs_hets
   
   # rare homozygotes, common homozygotes
   obs_homr <- min(obs_hom1, obs_hom2)
   obs_homc <- max(obs_hom1, obs_hom2)

   # number of rare allele copies
   rare  <- obs_homr * 2 + obs_hets

   # Initialize probability array
   probs <- rep(0, 1 + rare)

   # Find midpoint of the distribution
   mid <- floor(rare * ( 2 * N - rare) / (2 * N))
   if ( (mid %% 2) != (rare %% 2) ) mid <- mid + 1

   probs[mid + 1] <- 1.0
   mysum <- 1.0

   # Calculate probablities from midpoint down 
   curr_hets <- mid
   curr_homr <- (rare - mid) / 2
   curr_homc <- N - curr_hets - curr_homr

   while ( curr_hets >=  2)
      {
      probs[curr_hets - 1]  <- probs[curr_hets + 1] * curr_hets * (curr_hets - 1.0) / (4.0 * (curr_homr + 1.0)  * (curr_homc + 1.0))
      mysum <- mysum + probs[curr_hets - 1]

      # 2 fewer heterozygotes -> add 1 rare homozygote, 1 common homozygote
      curr_hets <- curr_hets - 2
      curr_homr <- curr_homr + 1
      curr_homc <- curr_homc + 1
      }    

   # Calculate probabilities from midpoint up
   curr_hets <- mid
   curr_homr <- (rare - mid) / 2
   curr_homc <- N - curr_hets - curr_homr
   
   while ( curr_hets <= rare - 2)
      {
      probs[curr_hets + 3] <- probs[curr_hets + 1] * 4.0 * curr_homr * curr_homc / ((curr_hets + 2.0) * (curr_hets + 1.0))
      mysum <- mysum + probs[curr_hets + 3]
         
      # add 2 heterozygotes -> subtract 1 rare homozygtote, 1 common homozygote
      curr_hets <- curr_hets + 2
      curr_homr <- curr_homr - 1
      curr_homc <- curr_homc - 1
      }    
 
    # P-value calculation
    target <- probs[obs_hets + 1]

    #plo <- min(1.0, sum(probs[1:obs_hets + 1]) / mysum)

    #phi <- min(1.0, sum(probs[obs_hets + 1: rare + 1]) / mysum)

    # This assignment is the last statement in the fuction to ensure 
    # that it is used as the return value
    p <- min(1.0, sum(probs[probs <= target])/ mysum)
    }
	
Query_missing = function()
{
	channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
	query = "SELECT  Indv_Id,Call_Rate FROM tblindividual WHERE (Call_Rate IS NOT NULL) order by indv_id";
	Data_Set = sqlQuery(channel, query)
	odbcClose(channel)
	return(Data_Set)
}

Query_heterozyosity_rate = function(Indv_Id)
{
	channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
	query = paste("SELECT Genotype,count(Genotype) FROM tblindividual INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id INNER JOIN tblsnp ON tblindividual_sample.Sample_Id = tblsnp.Sample_Id INNER JOIN tblmarker_loc ON tblsnp.Marker_Id = tblmarker_loc.Marker_Id WHERE (tblindividual.Call_Rate IS NOT NULL and tblindividual.Indv_id ='",Indv_Id,"' and Chr <> 'X' and Chr<> 'Y') group by tblindividual.Indv_Id, Genotype",sep="",collapse="");
	Data_set = sqlQuery(channel, query)
	odbcClose(channel)
	AA=0
	AB=0
	BB=0
	NC=0
	for(j in 1:nrow(Data_set))
	{
		if(Data_set[j,1] == "AA")
		{
 			AA = Data_set[j,2];
		}
		else if (Data_set[j,1] == "AB") 
		{
			AB = Data_set[j,2];
		}
		else if (Data_set[j,1] == "BB") 
		{
			BB = Data_set[j,2];
		}
		else 
		{
			NC = Data_set[j,2];
		}
}
	output<-list(AA,AB,BB,NC)
	names(output) <- c("AA","AB","BB","NC")
	return(output)
}

query_mc = function(index)
{
	channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
	query = paste("SELECT tblphenotype.Short_Name FROM tblindividual INNER JOIN tblindividual_phenotype ON tblindividual.Indv_Id = tblindividual_phenotype.Indv_Id INNER JOIN tblphenotype ON tblindividual_phenotype.Phenotype_Id = tblphenotype.Phenotype_Id INNER JOIN tblindividual_sample ON tblindividual.Indv_Id = tblindividual_sample.Indv_Id where tblindividual.indv_id = '",index,"' ",sep="",collapse="");
	Data_set = sqlQuery(channel, query)
	odbcClose(channel)
	return(Data_set)
}

query_index = function(index)
{
	
	 channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
	query = paste("select indv_id from tblindividual where `index`=",index," ",sep="",collapse="");
	Data_set = sqlQuery(channel, query)
	odbcClose(channel)
	return(Data_set)
}


query_index(3)

setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\phasebook")
data = read.table("ped.txt")
dis1 = data.frame("") 
missing_one_parent=0;
missing_two_parent=0;
good =0;
count_PA=0
count_NR=0;
count_SR=0;
count_RG=0;
count_IT=0;
for(i in 1:nrow(data))
{
	#dis1 = query_mc(as.character(data[i,1]))
	if(data[i,2] == "General" && data[i,3] == "General" )
	{
		missing_two_parent = missing_two_parent+1;
 
	dis1 = query_mc(as.character(data[i,1]))
	if(dis1 == "NR")
    {
			count_NR = count_NR + 1;
	}
	else if (dis1 == "PA")
	{
		count_PA = count_PA + 1;
}
	else if (dis1 == "RG")
	{
		count_RG = count_RG + 1;
}
	else if (dis1 == "SR")
	{
		count_SR = count_SR + 1;
}
	else if (dis1 == "IT")
	{
		count_IT = count_IT + 1;
	}
	
	
	}
 
 
}




data = cbind(data,as.data.frame(dis1),as.data.frame(dis2),as.data.frame(dis3))


indv_list = Query();
heter_rate = seq(1:1:nrow(indv_list))
Missing = Query_missing();
	
for(i in 1:nrow(indv_list))
{
	
	data = Query_heterozyosity_rate(indv_list[i,1]);
	N = data$AA+data$AB+data$BB-data$NC
	O = data$AA+data$BB
	heter_rate[i] = (N - O)/N
	 
}

R = covMcd(Missing$Call_Rate)
 heter_rate[151]
boxplot(heter_rate)
adjbox(heter_rate)

boxplot(Missing$Call_Rate)
adjbox(Missing$Call_Rate)

plot( Missing$logF_MISS,heter_rate, xlim=c(-3,0),ylim=c(0,0.5),pch=20, xlab="Proportion of missing genotypes", ylab="Heterozygosity rate")
library("geneplotter")
Missing$logF_MISS = log10(Missing[,2])
colors  <- densCols(Missing$logF_MISS,heter_rate)
pdf("raw-GWA-data.imiss-vs-het.pdf")
plot( Missing$logF_MISS,heter_rate, col=colors, xlim=c(-3,0),ylim=c(0,0.5),pch=20, xlab="Proportion of missing genotypes", ylab="Heterozygosity rate",axes=F)
axis(2,at=c(0,0.05,0.10,0.15,0.2,0.25,0.3,0.35,0.4,0.45,0.5),tick=T)
axis(1,at=c(-3,-2,-1,0),labels=c(0.001,0.01,0.1,1))

 

hist(data$Conflict)

Query_Genotype("ALGA0047675")

###########################		Sex Prediction		##############################
setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups");
Data_Set =  read.table("sex_pred.txt",header=T)
hist(Data_Set$Percentage,xlab= "heterozygsity by animal", main= "Sex Prediction")
abline(v=0.05,lty = 2)
hist(Data_Set[Data_Set$Percentage > 0.05,3],xlab= "heterozygous marker percentage %", main= "Sex Prediction")



Data_Pred = data.frame(Data_Set[1:nrow(Data_Set),1],seq(1:1:nrow(Data_Set)))
	for (i in 1:nrow(Data_Set))
	{ 
		if ( Data_Set[i,3] >= 0.05)
		{
		 Data_Pred[i,2] = "Female";
		}
		else 
		{ 
		Data_Pred[i,2] = "Male";
		}
	}
colnames(Data_Pred) = c("Indv_Id","Prec")

hist(Data_Pred,xlab= "heterozygous marker percentage %", main= "Sex Prediction")


data = cbind(Data_Pred,Data_Set$Gender)
colnames(data) = c("Indv_Id","Prec","Gender");
roc(Data_Set$Percentage, Data_Original$Gender, "Male")
table(data$Gender, data$Prec, dnn = c("From","To"))
##########################################################################
#check missclassification
A = data$Prec
B = data$Gender
C = data$Indv_Id
D = Data_Set$Percentage
wrong = data.frame(C,B,A,D)
wrong = wrong[wrong[,2] != wrong[,3],]
wrong = cbind(wrong[,1:3],wrong[,4])
colnames(wrong) = c("Indv Id","Orginal","Pred","Heter perc")
rownames(wrong) = seq(1:1,nrow(wrong))
 Data_Set[Data_Set$Indv == "BA003432",]

write.table(wrong, file = "Missclassified_Sex.txt", sep  = "\t", quote = FALSE, row.names = FALSE);




#########################			Parental Conflict		#######################
 
##########################				Line 12				#######################
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data\\Line 12")
Line_Individuals = Query_Lines(12);
data =  read.table("BA000532.txt",header=F)
for (i in 2:(nrow(Line_Individuals)-1))
{ 
    data = rbind(data , read.table(paste(Line_Individuals[i,1],".txt",sep =""),header=F))
 
}

 colnames(data) = c("Indv_Id1","Indv_Id2","Conflict","number of markers","Percentage");
 hist(data$Percentage, xlab = " Conflict Percentage ", ylab = " Frequency ", main = "Histogram of line 12")


##########################				Line 23				#######################

setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data\\Line 23")
Line_Individuals = Query_Lines(23);
data =  read.table("BA000464.txt",header=F)
for (i in 2:(nrow(Line_Individuals)-1))
{ 
    data = rbind(data , read.table(paste(Line_Individuals[i,1],".txt",sep =""),header=F))
 
}

 colnames(data) = c("Indv_Id1","Indv_Id2","Conflict","number of markers","Percentage");
 win.graph()
 hist(data$Percentage, xlab = " Conflict Percentage ", ylab = " Frequency ", main = "Histogram of line 23")

##########################				Line 36				#######################

setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data\\Line 36")
Line_Individuals = Query_Lines(36);
data =  read.table("BA000305.txt",header=F)
for (i in 2:(nrow(Line_Individuals)-1))
{ 
    data = rbind(data , read.table(paste(Line_Individuals[i,1],".txt",sep =""),header=F))
 
}

 colnames(data) = c("Indv_Id1","Indv_Id2","Conflict","number of markers","Percentage");
 win.graph()
 hist(data$Percentage, xlab = " Conflict Percentage ", ylab = " Frequency ", main = "Histogram of line 36")

##########################				Line 33				#######################

setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data\\Line 33")
Line_Individuals = Query_Lines(33);
data =  read.table("BA002807.txt",header=F)
for (i in 2:(nrow(Line_Individuals)-1))
{ 
    data = rbind(data , read.table(paste(Line_Individuals[i,1],".txt",sep =""),header=F))
 
}

 colnames(data) = c("Indv_Id1","Indv_Id2","Conflict","number of markers","Percentage");
 win.graph()
 hist(data$Percentage, xlab = " Conflict Percentage ", ylab = " Frequency ", main = "Histogram of line 33")


##########################			ALL	Lines 				#######################

setwd("C:\\Users\\Administrator\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data\\All Lines New")
Line_Individuals = Query();
found = 0;
not_found = "";
data =  read.table("BA000023.txt",header=F)
for (i in 2:(nrow(Line_Individuals)-1))
{ 
	#file.info(paste(Line_Individuals[i,1],".txt",sep =""))
	if(file.exists(paste(Line_Individuals[i,1],".txt",sep ="")))
	{
		data = rbind(data , read.table(paste(Line_Individuals[i,1],".txt",sep =""),header=F))
		found = found +1;
	}
	else
	{
		not_found = paste(not_found,"  ",Line_Individuals[i,1],".txt",sep ="");
	}
}
write.table(data, file = "Conflict.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
hist(data$V5)

setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\phasebook");
Best =  read.table("Best_pedigree.txt",header=F)
hist(Best$V6)



 colnames(data) = c("Indv_Id1","Indv_Id2","Conflict","number of markers","Percentage");
 win.graph()
 hist(data$Percentage, xlab = " Conflict Percentage ", ylab = " Frequency ", main = "Histogram of All Lines")



pedigree_predicted = cbind(data,rep(0,nrow(data)));
pedigree_predicted = cbind(data,seq(1:1:nrow(data)));
colnames(pedigree_predicted) = c("Indv_Id1","Indv_Id2","Conflict","number of markers","Percentage","prediction");

	for (i in 1:nrow(pedigree_predicted))
	{ 
		if ( pedigree_predicted[i,5] <= 0.07)
		{
		 pedigree_predicted[i,6] = 0;
		}
	}
pred =  pedigree_predicted[pedigree_predicted$prediction == 0, ]
rownames(pred)   = (seq(1:1:nrow(pred)))

indv_list = Query();

indv_list = cbind(indv_list, rep(0, nrow(indv_list)),rep(0, nrow(indv_list)) );

colnames(indv_list)   = c("Indv_Id", "Gender","Father","Mother");
 
	Pedigree = Query_Pedigree()
	final_pedigree = list(NULL);
	for (i in 1:nrow(indv_list))
	{
		Min_Male = 1;
		Min_Female = 1;
		Father = "";
		Mother = "";
		Indv = indv_list[i,1];
		for (j in 1:nrow(pred))
		{ 
			if (as.character(indv_list[i,1]) == as.character(pred[j,1]))
			{
				indicator = 1;
			}
		else if (as.character(indv_list[i,1]) == as.character(pred[j,2]))
			{
				indicator = 2;
			}
		else 
		{
			indicator = 0;
		}
		
		if(indicator == 1 )
		{
			if( Query_Gender(as.character(pred[j,2])) == "Male")
				{
					if(pred[j,5] < Min_Male)
					{  
						Min_Male = pred[j,5];
						Father = pred[j,2];
						 
					}
				}
			else
 				{
					if(pred[j,5] < Min_Female)
					{ 
						 
						Min_Female = pred[j,5];
						Mother = pred[j,2];
						 
					}
				}
	}
	
	else if (indicator == 2 )
		{
			if( Query_Gender(as.character(pred[j,1])) == "Male")
				{
					if(pred[j,5] < Min_Male)
					{ 
						 
						Min_Male = pred[j,5];
						Father = pred[j,1];
						 
					}
				}
			else
 				{
					if(pred[j,5] < Min_Female)
					{ 
						 
						Min_Female = pred[j,5];
						Mother = pred[j,1];
						 
					}
				}
		}
		
		
		
	} 
	
		
		if(Father != "" & Mother != "")
		{
			final_pedigree[[i]] = paste(Indv,"Fater",Father,Min_Male, collapse = "", sep = "  ");
			final_pedigree[[i]][2] = paste(Indv,"Mother",Mother,Min_Female, collapse = "", sep = "  ");
		}
		else
		{
			if (Father == "" & Mother == "")
			{
				
			}
			else if(Father != "")
			{
				final_pedigree[[i]] = paste(Indv,"Fater",Father,Min_Male, collapse = "", sep = "  ");
			}
			else
			{
				final_pedigree[[i]] = paste(Indv,"Mother",Mother,Min_Female, collapse = "", sep = "  ");
			}
		}
    }
	
	for (i in 1:nrow(indv_list))
	{
		splt_father <- strsplit(final_pedigree[[i]][1], "  ");
		splt_mother <- strsplit(final_pedigree[[i]][2], "  ");
		Pedigree[i,4] = splt_mother[[1]][3];
		Pedigree[i,5] = splt_father[[1]][3];
		Pedigree[i,6] = 1;
}

Pedigree[1:nrow(Data_Set),7] = rep(0, nrow(indv_list))
Pedigree[1:nrow(Data_Set),8] = rep(0, nrow(indv_list))

Update_Pidegree(Pedigree);


myDataFrame <- read.table(file = "myFile", header = FALSE, sep = ",", row.names = " ", stringsAsFactors = TRUE)






save(list = ls(all=TRUE), file = "QTL Data_All Lines.RData")
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data\\");
write.table(Pedigree, file = "Pedigree.txt", sep  = "\t", quote = FALSE, row.names = FALSE);

#####################			Pidegree Drawing			##############

setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data")
library('kinship2')
pre <-read.table("tree.txt", header = T)
attach(pre)
ped = pedigree(id,  fid,mid, sex)
par(xpd= T)
plot.pedigree(ped)

install.packages("graphviz")

library("gap")
library("graphviz")
pre = cbind(pid = 10081, pre)
dotty(pedtodot(pre))


#######################################################################################

####################################		HWE			##############################

Markers_List = Query_Markers();
HWE = cbind(as.character(Markers_List[,1]), rep(1, nrow(Markers_List)));
colnames(HWE) = c("Marker_Id","Exact_Test");

for (i in 1:nrow(Markers_List))
{
	
	AB = Markers_List$AB_count[i];
	AA = Markers_List$AA_count[i];
	BB = Markers_List$BB_count[i];
	if(AB !=0 & AA !=0 & BB !=0)
	{
		p_values = SNPHWE(AB,AA,BB);
		HWE[i,2] = as.double(p_values);
	}
}
format(1.06400991551122e-050,scientific=F)

count =0;
for(i in 1:nrow(HWE))
{
	if(HWE[i,2] > 0.001)
	{
		count = count +1;
	}
}
setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test")
write.table(HWE, file = "HWE.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
HWE = read.table(file = "HWE.txt",header = TRUE)


####################################################################
Genome = read.table("plink.genome", header=T);

hist(Genome$DST,xlab= "DST", main= "Frequency")

duplicate = 0;
for(i in 1:nrow(Genome))
{
	if(Genome$DST[i] > 0.1)
	{
		duplicate = duplicate + 1;
	}
}

summary(Data_Set)


#############  Multidimensitional scaling plot	Plink #########
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Plink\\");
mds = read.table("plink_latest.mds",header = TRUE);
 Lines = Query_All_lines();
#Lines = Query_old_dataset();
mds = as.data.frame(cbind(mds[,4:7],Lines$Line))
colnames(mds) = c("C1","C2","C3","C4","Line");
for(i in 1:nrow(mds))
{
	if(mds$Line[i] == 15)
	{
		mds$Line[i] = "blue";
	}
	else if(mds$Line[i] == 12)
	{
		mds$Line[i] = "green";
    }
	else if(mds$Line[i] == 23)
	{
		mds$Line[i] = "red";
	}
	else if(mds$Line[i] == 33)
	{
		mds$Line[i] = "black";
	}

	else
	{
		mds$Line[i] = "orange";
	}

}
plot(mds[,1:2],pch=20,xlab = "C1" , ylab= "C2", main= "Multidimensional Scaling Plot", col = mds$Line, cex=1.5);
legend("topleft",legend=c("Line 15","Line 12", "Line 23", "Line 33", "Line 36"), text.col = c("blue","green","red","black","orange"))
plot(mds[,1:4],pch=20,col = mds$Line)

############# Plink.genome for the DST #########
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Plink\\");
Plink.genome = read.table("plink.genome",header = TRUE);
hist(Plink.genome$DST,xlab= "DST", main= "Frequency");
 
 

#############  Multidimensitional scaling plot	Mahmoud #########
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\");
m = as.matrix(read.table("matrix_mds.txt"));
mds = cmdscale(as.dist(1-m),4);
mds = cmdscale(1-m,4,eig = TRUE, add = TRUE);
plot(mds$points[,1:2])
Lines = Query_All_lines();
mds = as.data.frame(cbind(mds,Lines$Line))
colnames(mds) = c("C1","C2","C3","C4","Line");
for(i in 1:nrow(mds))
{
	if(mds$Line[i] ==12)
	{
		mds$Line[i] = "green";
    }
	else if(mds$Line[i] == 23)
	{
		mds$Line[i] = "red";
	}
	else if(mds$Line[i] == 33)
	{
		mds$Line[i] = "black";
	}

	else
	{
		mds$Line[i] = "orange";
	}

}
plot(mds[,1:2],pch=20,xlab = "C1" , ylab= "C2", main= "Multidimensional Scaling Plot", col = mds$Line, cex=1.5);
legend("topright",legend=c("Line 12", "Line 23", "Line 33", "Line 36"), text.col = c("green","red","black","orange"))
plot(mds[,1:4],pch=20,col = mds$Line)


###################################################
PCA = prcomp(as.dist(1-m), retx = TRUE, center =TRUE, scale = TRUE);
win.graph();
plot(PCA$x[,2], PCA$x[,1]);

Lines$Line

setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\");
m = read.table("plink.mds",header= TRUE);
plot(m[,4:5])


####################### FOunder_Control HWE  ########################


#Markers_List = Query_Markers();
Markers_List = Query_Markers_List_Modified();
HWE = cbind(as.character(Markers_List[,1]), rep(1, nrow(Markers_List)));
colnames(HWE) = c("Marker_Id","Exact_Test");

for (i in 1:nrow(Markers_List))
{ 
	Data_set = Query_Genotype(Markers_List[i,1]);
	AA=0;
	AB=0
	BB=0;
	for(j in 1:nrow(Data_set))
	{
		if(Data_set[j,1] == "AA")
		{
 			AA = Data_set[j,2];
		}
		else if (Data_set[j,1] == "AB") 
		{
			AB = Data_set[j,2];
		}
		else if (Data_set[j,1] == "BB") 
		{
			BB = Data_set[j,2];
		}
		else 
		{
			NC = Data_set[i,2];
		}
	}
    p_values = SNPHWE(AB,AA,BB);
	HWE[i,2] = as.double(p_values);
}
count =0;
for(i in 1:nrow(HWE))
{
	if(HWE[i,2] <= 0.001)
	{
		count = count +1;
	}
}
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data")
write.table(HWE, file = "Founder_Control.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
HWE = read.table(file ="HWE.txt", header = TRUE);
####################################################################
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data")
plink.hwe = read.table(file ="plink.hwe", header = TRUE);
 
 
count = 0
j = 1
df = data.frame(matrix(ncol = 2, nrow = 7000))
for(i in seq(3,nrow(plink.hwe)-3,3))
{
	df[j,1] = as.character(plink.hwe[i,2])
	df[j,2] = plink.hwe[i,9]
	count = count +1;
	j = j+1
}

write.table(df, file = "Plink_P_Value.txt", sep  = "\t", quote = FALSE, row.names = FALSE);

count = 0;
for(i in 1:nrow(HWE))
{
	for(j in 1:nrow(df))
	{
		if(df[j,1] == HWE[i,1])
		{
			if(format(df[j,2],scientific = FALSE,digit = 4) == format(HWE[i,2],scientific = FALSE,digit = 4))
			{
				count = count + 1;
			}
		}
	}

}




setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\Diseases\\RG")
Trios = data.frame(cbind(seq(1:1:27), rep(0,27)));
Genotype = seq(1:1:27)
Gen ="";
type = c("AA","AB","BB");
counter = 1;
for(i in 1:3)
{
	Gen = type[i];
	for(j in 1:3)
	{
		Gen = paste(Genotype,type[j],collapse="",sep=" ")
		for(k in 1:3)
		{
			Genotype[counter-1] = paste(Gen,type[k],collapse="",sep=" ")
			counter = counter + 1;
		}
	}
}
 
file =  read.table("Errors_MARC0081387_RG.txt",header=F)
flag = FALSE;
for(i in 1:nrow(file))
{
 
	for(j in 1:nrow(Trios))
	{
		if(Trios[j,1] == paste(file$V4[i]," ",file$V5[i]," ",file$V6[i]," ",collapse = ""))
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




####################### defect 6 high  ######################
setwd("C:\\Users\\Administrator\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data\\defect 6 high")
Line_Individuals = read.table("defect_6_high.txt",header=F)
found = 0;
not_found = "";
data =  read.table("BA000023.txt",header=F)
for (i in 2:(nrow(Line_Individuals)-1))
{ 
	#file.info(paste(Line_Individuals[i,1],".txt",sep =""))
	if(file.exists(paste(Line_Individuals[i,1],".txt",sep ="")))
	{
		data = rbind(data , read.table(paste(Line_Individuals[i,1],".txt",sep =""),header=F))
		found = found +1;
	}
	else
	{
		not_found = paste(not_found,"  ",Line_Individuals[i,1],".txt",sep ="");
	}
}
write.table(data, file = "Conflict.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
best = read.table("Best_pedigree.txt",header=F)
win.graph()
plot(x=seq(1:1,35),y=best$V5[1:35],col="green",pch=19,xlab = "Offsprings",ylab= "Father Mother Conflict")
points(x=seq(1:1,35),y=best$V4[1:35],col ="red")
axis(side = 1, at = c(seq(1:1,35)))
axis(2, at=c(seq(0:0.01,max(best$V5[1:35]))), col.axis="red", las=2)

legend("topleft",c("Father","Mother"), lty=c(1,1),lwd=c(2.5,2.5),col=c("red","green")) 
#################################
 

setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\phasebook")
defect_6_low_pairs <- read.table("defect_6_low_pairs.txt", header=F)
defect_6_high_pairs <- read.table("defect_6_high_pairs.txt", header=F)
defect_6= rbind(defect_6_low_pairs,defect_6_high_pairs);
win.graph();
plot(x=seq(1:1,36),y=defect_6_high_pairs$V5,col ="red",xlab = "Offsprings",ylab= "Father Mother Conflict")
points(x=seq(1:1,36),y=defect_6_high_pairs$V3,col="green")
legend("topleft",c("Mother","Father"), lty=c(1,1),lwd=c(2.5,2.5),col=c("red","green")) 



###############			 Boxes Missing		###################

setwd("C:\\Users\\Administrator\\Documents\\Revolution\\QTL_Mapping\\Parental conflict data")
Box <- read.table("boxes_missing.txt", header=T)

Box = Box[Box$call_rate > 0.80,]
rownames(Box) = seq(1:1,nrow(Box))


box1 = Box[Box$Box_Id==1000,];
box2 = Box[Box$Box_Id==1010,];
box3 = Box[Box$Box_Id==4939,];
box4 = Box[Box$Box_Id==4940,];
box5 = Box[Box$Box_Id==4941,];
box6 = Box[Box$Box_Id==4944,];
box7 = Box[Box$Box_Id==4945,];
box8 = Box[Box$Box_Id==4958,];
box9 = Box[Box$Box_Id==4959,];
box10 = Box[Box$Box_Id==4963,];
box11 = Box[Box$Box_Id==4964,];

rownames(box1) = seq(1:1,nrow(box1))
rownames(box2) = seq(1:1,nrow(box2))
rownames(box3) = seq(1:1,nrow(box3))
rownames(box4) = seq(1:1,nrow(box4))
rownames(box5) = seq(1:1,nrow(box5))
rownames(box6) = seq(1:1,nrow(box6))
rownames(box7) = seq(1:1,nrow(box7))
rownames(box8) = seq(1:1,nrow(box8))
rownames(box9) = seq(1:1,nrow(box9))
rownames(box10) = seq(1:1,nrow(box10))
rownames(box11) = seq(1:1,nrow(box11))

boxplot(box1$call_rate,box2$call_rate,box3$call_rate,box4$call_rate,box5$call_rate,box6$call_rate,box7$call_rate,box8$call_rate,box9$call_rate,box10$call_rate,box11$call_rate,axis = FALSE,xlab = "BOX number", ylab = "Call Rate")
axis(1,at= seq(1, 11, by=1),labels=c("1000","1010","4939","4940","4941","4944","4945","4958","4959","4963","4964"))


boxplot(log10(box1$call_rate),log10(box2$call_rate),log10(box3$call_rate),log10(box4$call_rate),log10(box5$call_rate),log10(box6$call_rate),log10(box7$call_rate),log10(box8$call_rate),log10(box9$call_rate),log10(box10$call_rate),log10(box11$call_rate))
boxplot(log10(box1$call_rate),log10(box2$call_rate))


hist(Box$call_rate)

#################### Hetero Missing			#####################
 
setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test")
imiss = read.table("hetero_missing.txt",h=F)
imiss = imiss[imiss$V3 != 0,]
rownames(imiss) = seq(1:1,nrow(imiss))
imiss$logF_MISS = log10(imiss[,3])
win.graph()
plot(imiss$V3,imiss$V2,ylim=c(0.2,0.40),pch=20)
abline(v = log10(0.05) ,col="RED")
nrow(imiss[imiss$V3 >= 0.05,])
 
plot(imiss$logF_MISS,imiss$V2,ylim=c(0.2,0.40),pch=20, xlab="Proportion of missing genotypes", ylab="Heterozygosity rate",axes=F)
axis(2,at=c(0,0.05,0.10,0.15,0.2,0.25,0.3,0.35,0.4,0.45,0.5),tick=T)
axis(1,at=c(-5,-4,-3,-2,-1,0),labels=format(c(0.00001,0.0001,0.001,0.01,0.1,1),scientific = F))

abline(v= log10(mean(imiss$V3)) ,col="RED")
abline(v= log10( mean(imiss$V3) +  ( 3 *sd(imiss$V3))),col="RED", lty=2)
text(x=log10(mean(imiss$V3)),y=0.40,labels = "Mean")
text(x=log10( mean(imiss$V3) +  ( 3 *sd(imiss$V3))),y=0.40,labels = "Mean + 3 * SD ? 95%")

abline(v= log10( mean(imiss$V3) +  ( 2 *sd(imiss$V3))),col="Blue", lty=2)

abline(h= mean(imiss$V2) ,col="BLUE")
abline(h= mean(imiss$V2) +  ( 3 *sd(imiss$V2)),col="BLUE")
abline(h=  mean(imiss$V3) -  ( 3 *sd(imiss$V2)),col="BLUE")

 
abline(mC <- lm(imiss$V2 ~ imiss$logF_MISS, data = imiss)) 
 




win.graph()
plot(imiss$V3,imiss$V2,pch=20, xlab="Proportion of missing genotypes", ylab="Heterozygosity rate")

tolEllipsePlot(imiss[,c(3,2)],covMcd(imiss[,c(3,2)]))
summary (lmrob(imiss$V2 ~ imiss$logF_MISS))
lm(imiss$V2 ~ imiss$logF_MISS)
lm(imiss$V2 ~ imiss$V3)
covMcd(imiss$V2)
covMve(imiss$V2)
log10(0.0002084)
log10( 5.496e-08)
0.0002084 + (3 * 5.496e-08)

abline(v=log10(0.05),col="RED",lty=2)
abline(v=log10(0.035),col="RED",lty=2)

win.graph();
setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test");
window = read.table(file ="window_X_33.txt", header = FALSE);
win.graph();
plot(x=seq(1:1,80),y=window$V1,type="o",xlab = "window" , ylab = "percentage", main = "Line 33 with 0.25%",ylim = c(0,0.6))
axis(1, at=1:5, labels= seq(from = 1, to =0.8 , by = -0.05))
miss_hwe = missing_HWE();
win.graph();
plot(x = miss_hwe$Call_Rate ,y = -log10(miss_hwe$Founder_Control) , xlab="Call Rate", ylab="-log(P-Value)", main = "Line 15")
seq(from = 0.8, to =1 , by = 0.0025)

 setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test");
window = read.table(file ="discard_marker.txt", header = FALSE);
plot(x=seq(1:1,5),y=window$V1/60835,type="o",xlab = "call rate" , ylab = "percentage")

x = c(0.8,0.2,0.4,0.10,0.02,0.10,0.04)
mean(x)
sd(x)

sqrt(sum((x-mean(x))^2)/(7-1))


##################### Manhattan Plots #########################

setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups\\");
####################################################################################
setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Norway day\\Til Mahmoud\\Cryptorchidism\\Without X\\");
results <- read.table("TDT_All.txt", header=F)
results = results[,c(1,2,3,7,4,5,8,9,10)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing")
qqunif(results$P)
title("QQ plot Cryptorchidism Hernia Disease All 2")

results2 = results[results$CHR == 2, ]
results2 = results2[-log10(results2$P) >= 6, ]
rownames(results2) = seq(1:1,nrow(results2))
results2 = results2[results2$BP >= 47529480, ]
results2 = results2[results2$BP <= 55492906, ]
cases = results2
controls = results2
All = results2

rownames(results2) = seq(1:1,nrow(results2))


mycolors <- c("red","blue","green","cyan","brown4","black","magenta","red","blue","green",
            "cyan","brown4","black","magenta","red","blue","green","cyan","black","green")
win.graph();
textvec <- c(as.character(0:18),"X")
textvec <- c(as.character(0:18))
mhtplot(results[,2:4],control = mht.control(usepos = FALSE,colors=mycolors,yline=3,xline=3),srt = 10)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=16,by=1))
abline(h=0)
title("TDT For Cryptorchidism Disease All")




xat <- locator()$x
xat <- round(xat)
abline(h=4.5,lty = 3)

for(i in 11:11)
{
	win.graph();
	mhtplot(results[ results$CHR == i,c(2,3,4)],control = mht.control(usepos = FALSE,colors=mycolors,yline=3,xline=3))
	axis(2,at=0:10)
	abline(h=0)
	title(paste("TDT For Umbilical Hernia Disease Belgium CHR ",i,collapse = ""))
}

win.graph()
r = results[results$CHR != "19",]
qqunif(results$P)
title("QQ Plot TDT For Umbilical Hernia Disease")

####################################################################################

setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups")
setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Norway day\\Til Mahmoud\\Cryptorchidism\\Without X\\");
results <- read.table("TDT_combined.txt", header=F)
results = results[,c(1,2,3,6)] # the last one is 6 or 9
colnames(results) = c("Marker_ID","CHR","BP","P")
mhtplot(results[,2:4],control = mht.control(usepos = FALSE,colors=mycolors,yline=3,xline=3),srt = 10)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=14,by=1))
abline(h=0)
title("TDT For Cryptorchidism Disease DC")

results = results[results$CHR == 14, ]
results = results[results$BP >= 47529480, ]
results = results[results$BP <= 55492906, ]
-log10(results$P)



results = results[results$CHR == 14, ]
results = results[-log10(results$P) <= 10, ]
rownames(results) = seq(1:1,nrow(results))

####################################################################################






results <- read.table("TDT.txt", header=F)
results = results[,c(2,1,3,15,12,13,16,17,18,5,6,7,8,9,10,11)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing","P23","P36","P12","P33","P15","HWE_23_36_12","HWE_total")
results = results[-log10(results$P) <= 10, ]
rownames(results) = seq(1:1,nrow(results))
results = results[results$CHR != 0, ]
results = results[results$CHR != 19, ]

results[-log10(results$P) >= 7, ]

results = results[,c(2,11,12,7,4,5,8,9,10)]
results = results[results$CHR != "Uwgs",]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing")
rownames(results) = seq(1:1,nrow(results))
results$BP = results$BP/1000000
results = results[results$CHR != "Uwgs",]
 draw = draw[with(draw, order(CHR, BP)), ]
 draw = draw[draw$BP >= 0, ]
rownames(draw) = seq(1:1,nrow(draw))
results$V3 = results$V3/1000000
nrow(results)

test = cbind(as.character(results$CHR),as.numeric(as.character(results$BP)),as.numeric(as.character(results$P)))
mhtplot(results[results$V4 > 1,2:4] )

typeof(results$V3)

mycolors <- c("red","blue","green","cyan","brown4","black","magenta","red","blue","green",
            "cyan","brown4","black","magenta","red","blue","green","cyan","black","green")
win.graph();

mhtplot(results[,2:4],control = mht.control(usepos = FALSE,colors=mycolors,yline=3,xline=3),srt = 10)
textvec <- c(as.character(0:18),"X")
mhtplot(results[results$BP > 25.000000 ,2:4],control = mht.control(usepos = FALSE ,colors=mycolors,yline=3,xline=3))

xat <- locator()$x
xat <- round(xat)
min(results[,4])

mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=9,by=0.25))
abline(h=0)
abline(h=4.5,lty = 3)
title("TDT For Scrotal rupture Disease")
title("TDT For Cryptorchidism Disease")

title("Pseudoautosmal Mothers Cryptorchidism Disease")
title("TDT For Scrotal rupture Disease")
title("TDT For Navel rupture Disease")

significant_SR_X = results[-log10(results$V4) >=3.5 ,]
rownames(significant_SR_X) = seq(1:1,nrow(significant_SR_X))

exp^-0.9255
significant_NR = results[-log10(results$P) >=4 ,]
rownames(significant_NR) = seq(1:1,nrow(significant_NR))

significant_RG = results[-log10(results$P) >=3.5 ,]
rownames(significant_RG) = seq(1:1,nrow(significant_RG))

significant_SR = results[-log10(results$P) >=3.5 ,]
rownames(significant_SR) = seq(1:1,nrow(significant_SR))
plot(significant_SR$T,significant_SR$U)
radius <- sqrt( significant_RG$Nonsense/ pi)
win.graph()
symbols(significant_RG$T, significant_RG$U, circles=radius , 
	inches=0.25, fg="white", bg="red", xlab="T", ylab="U", main = "RG disease significant Markers")
text(significant_RG$T, significant_RG$U, significant_RG$Marker_ID, cex=0.5)

radius <- sqrt( significant_SR$Nonsense/ pi)
symbols(significant_SR$T, significant_SR$U, circles=radius , 
	inches=0.25, fg="white", bg="red", xlab="T", ylab="U", main = "SR disease significant Markers")
text(significant_SR$T, significant_SR$U, significant_SR$Marker_ID, cex=0.5)




andrews(significant_SR,type =2 )
plot(significant_SR[,4:9])


win.graph()
qqfun(-log10(results$P),distribution="unif")
qqunif(results$P)
abline(h=4) 
title("QQ Plot TDT For Cryptorchidism Disease")


min(results$P)
max(results$P)

title("TDT For Navel rupture Disease")
?qqunif
plot(qchisq(ppoints(results$V6),1), sort(results$V6)) 
abline(a=0,b=1) 

ppoints(10)

qq(results$P)


write.table(results, file = "TDT_RG.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
write.table(results, file = "TDT_SR.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
write.table(results, file = "TDT_NR.txt", sep  = "\t", quote = FALSE, row.names = FALSE);

write.table(significant_SR, file = "significant_SR.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
write.table(significant_RG, file = "significant_RG.txt", sep  = "\t", quote = FALSE, row.names = FALSE);



results <- read.table("TDT_RG.txt", header=T)
results <- read.table("TDT_SR.txt", header=T)
results <- read.table("TDT_NR.txt", header=T)
win.graph()
results <- read.table("hwe_x_pvalues.txt", header=F)
qqunif(results[,4],type="unif")

abline(h=3) 
title("TDT For Cryptorchidism Disease ALL CHR")
title("TDT For Scrotal rupture Disease Combined")
title("TDT For Navel rupture Disease")

results[results$V2 == "ALGA0109557",]


results <- read.table("TDT_2_X.txt", header=F)
mhtplot(results[results$V4 > 1.313506e-14,c(1,3,4)],control = mht.control(usepos = FALSE,cutoffs = 4,colors=mycolors,yline=3,xline=3))
min(results$V4)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=0:22)
abline(h=0)
title("TDT For Cryptorchidism Disease")


mhtplot(results[results$CHR == 15,2:4],control = mht.control(usepos = FALSE,cutoffs = 3.5,colors=mycolors,yline=3,xline=3))
axis(2,at=0:22)
abline(h=0)
title("TDT For Cryptorchidism Disease CHR 15")

qqunif(results[results$CHR == 3,4],distribution="unif")

setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test\\Significant RG\\Chromosomes RG\\CHR 2");
results <- read.table("H3GA0006932_neighbours - Copy.txt", header=F)
p.adjust(results$V16, method = "bonferroni", n =length(results$V16))


setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups\\");

hwe <- read.table("input.txt", header=F)
hwe <- hwe[!is.na(hwe$V2),]
nrow(hwe) - nrow(hwe[hwe$V2 <= 0.0003162278,])
HWD = hwe[hwe$V2 <= 0.0003162278,]
rownames(HWD) = seq(1:1,nrow(HWD))
qqunif(hwe$V2)
abline(h=3.5)
nrow(hwe)

log10(1/0.0003162278)

 write.table(HWD, file = "HWD_excluded.txt", sep  = "\t", quote = FALSE, row.names = FALSE);


#################### Combine data ############################

setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups\\");
results <- read.table("Combined_NR.txt", header=F)
results = results[,c(1,2,3,4,5,6)]
colnames(results) = c("Marker_ID","CHR","BP","Me","Norway","P")
results$BP = results$BP/1000000
results = results[-log10(results$P) <= 20,]
rownames(results) = seq(1:1,nrow(results))



mycolors <- c("red","blue","green","cyan","brown4","black","magenta","red","blue","green",
            "cyan","brown4","black","magenta","red","blue","green","cyan","black","green")
mhtplot(results[,c(2,3,6)],control = mht.control(cutoff = 4,usepos = FALSE,colors=mycolors,yline=3,xline=3),srt = 10)
textvec <- c(as.character(0:18),"X")
xat <- locator()$x
xat <- round(xat)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=0:13)
abline(h=0)
title("TDT For Umbilical Hernia Disease Combined Data")

results$

significant_RG = results[-log10(results$P) >=3.5 ,]
rownames(significant_RG) = seq(1:1,nrow(significant_RG))


source("http://bioconductor.org/biocLite.R")
biocLite("qvalue")
library(qvalue)
qvalue.gui()

-log10(3.14E-05)

setwd("D:\\Master of Bioinformatics\\GIGA Lab\\PIGENDEF\\Norway data\\");
results <- read.table("p_values.txt", header=F)

results = results[-log10(results$V15) <= 10,]
rownames(results) = seq(1:1,nrow(results))

win.graph()
qqunif(results$P)
abline(h=2)
title("QQ Plot TDT For Umbilical Hernia Disease My Data")



install.packages("C:\Users\Administrator\Downloads\TDTae.zip")
library(GenABEL)
demo(ge03d2)
0.00003
-log10(0.0001)

10^(4.5)
1/31622.78


setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Norway day\\Norway Data Set\\Umbilical\\");
results <- read.table("allmarkers_UB.txt", header=T)

win.graph();
qqunif(results$q_value);

((40)*((10*9) - (11*10))^2) / (20*20*21*19)

#############################################
source("http://bioconductor.org/biocLite.R")
biocLite("snpMatrix")
library(snpMatrix)
setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Norway day\\Til Mahmoud\\Cryptorchidism\\Plink\\");
results <- read.table("porc2.ped", header=F)
colnames(results) = c("ped","id","father","mother","sex","Aff")
snp.data = results[,7:8]
snp.data = as.matrix(snp.data)
snp = new("snp.matrix", snp.data)

install.packages("marray")
results$Aff results$Aff == "2"

tdt.snp(0,as.character(results$id), as.character(results$father), as.character(results$mother),results$Aff,data=sys.parent(),snp.data = snp)
tdt.snp(snp.data)




snps.class<-new("snp.matrix", matrix(1:10))
snps.class



((175*0.75) + (175*0.75) + (100*0.75) + (100*.85) + (50 * 0.55)) / (175+175+100+100+50)

-log10(1/1)
-log10(1/6.33424836662399E-05)

11+30+33

((0-0)^2)/(0+0)