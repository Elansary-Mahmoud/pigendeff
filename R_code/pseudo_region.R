library('robustbase')
library('RODBC')
library('ROCR')
library('Daim')
library('qtl')
library("gap")
Query <- function()
{
channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
query = "select Marker_Id, chr, position, call_rate,Founder_Control from tblmarker_loc where chr <> 'X' and chr <> 'Y' order by 0+chr,position,marker_id";
Data_Set=sqlQuery(channel, query)
odbcClose(channel)
return(Data_Set)
}


Founders = Query();
Founders = Founders[Founders$Founder_Control != 10,]
rownames(Founders) = seq(1:1,nrow(Founders))
plot(x=-log10(Founders$Founder_Control),y=Founders$call_rate,xlab="-log10(pvalue)", ylab ="Call Rate")
Founders_high = Founders[Founders$call_rate >= 0.95,]
rownames(Founders_high) = seq(1:1,nrow(Founders_high))
Founders_low  = Founders[Founders$call_rate < 0.8,]
rownames(Founders_low) = seq(1:1,nrow(Founders_low))

significant = Founders_high[Founders_high$Founder_Control <= 0.001,]
rownames(significant) = seq(1:1,nrow(significant))


plot(x=-log10(Founders_high$Founder_Control),y=Founders_high$call_rate,xlab="-log10(pvalue)", ylab ="Call Rate")



results = cbind(Founders$chr,Founders$position/1000000,Founders$Founder_Control)
colnames(results) = c("CHR","BP","P")

mycolors <- c("red","blue","green","cyan","yellow","black","magenta","red","blue","green",
            "cyan","yellow","black","magenta","red","blue","green","cyan","black")
par(cex.axis=1.3)
par(las="2",cex=0.6)


results <- read.table("TDT - Copy.txt", header=F)
results = results[,c(1,3,7)]
colnames(results) = c("CHR","BP","P")
results$BP = results$BP/1000000
?mhtplot
mhtplot(results,control = mht.control(usepos = FALSE,cutoffs = 4,colors=mycolors,yline=3,xline=3))
textvec <- as.character(0:18)
xat <- locator()$x
xat <- round(xat)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=0:14)
abline(h=0)


axis(2,pos=2,at=0:15)
abline(h=4.5,col="BLACK",lty=2)




min(pairs_pred$V4)
setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test")
pairs <- read.table("Database_Pairs_2.txt", header=F)
pairs_pred <- read.table("pred_trios.txt", header=F)
hist(pairs_pred[,4],xlab = "Parental Conflict",main = "Histogram of Predicted Trios")
 abline(v=0.02)
pairs_high = pairs_pred[pairs_pred$V4 > 0.002,]
pairs_low = pairs_pred[pairs_pred$V4 <= 0.002,]

hist(pairs_high[,4],xlab = "Parental Conflict",main = "Histogram of Predicted Trios")


pairs_low = pairs_pred[pairs_pred$V4 <= 0.002,]
nrow(pairs_high)
nrow(pairs_low)
rownames(pairs_low)=seq(1:1:nrow(pairs_low))
rownames(pairs_high)=seq(1:1:nrow(pairs_high))
good_trios = rbind(good_trios[,1:4],pairs_low[,1:4])
good_trios = rbind(good_trios[,1:4],pairs_high[,1:4])
write.table(good_trios, file = "Good_Trios.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
write.table(pairs_low, file = "76_Trios.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
X =  (marker_conflict[marker_conflict$V1 == "X",])


hist(good_trios[,4],xlab = "Parental Conflict",main = "Histogram of Good trios")
good_trios = (good_trios[good_trios$V4 <0.002,])
rownames(good_trios)=seq(1:1:nrow(good_trios))
setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test")
write.table(results, file = "TDT_Result.txt", sep  = "\t", quote = FALSE, row.names = FALSE);

hist(pairs_low[ ,4],xlab = "Parental Conflict",main = "Histogram of predicted trios")
pairs_high = pairs_pred[pairs_pred$V4 > 0.002,]
nrow(pairs_high)

for(i in 1: nrow(pairs_high))
{
	for(j in 1:nrow(pairs))
	{
		if(pairs_high[i,1] == pairs[j,1])
		{
			pairs_high$V6[i] =  pairs[j,6]
			pairs_high$V7[i] =  pairs[j,7]
		}
	}
}
hist(pairs_high[pairs_high$V6 <=0.005,6],xlab = "Parental Conflict",main = "Histogram of predicted Fathers")
hist(pairs_high[pairs_high$V7 <= 0.005,7],xlab = "Parental Conflict",main = "Histogram of predicted Mothers")



pairs_pred <- read.table("good_trios.txt", header=F)
hist(pairs_pred$V4,xlab = "Parental Conflict",main = "Histogram of Good Trios")



hist(pairs_high[,4],xlab = "Parental Conflict",main = "Histogram of predicted trios high")
hist(pairs_low[,4],xlab = "Parental Conflict",main = "Histogram of predicted trios low")


res <- read.table("Database_trios.txt", header=F)
hist(res[,4],xlab = "Parental Conflict",main = "Histogram of Database Trios")
hist(res[res$V4 <= 0.01,4],xlab = "Parental Conflict",main = "Histogram of Database Trios")
good = (res[res$V4 <= 0.002,]) 
rownames(good)=seq(1:1:nrow(good))

BAD = res[res$V4 > 0.002,]
rownames(BAD)=seq(1:1:nrow(BAD))


nrow(good)
res
good_trios

nrow(res[res$V4 > 0.01,]) 
nrow(res)
good_trios = res[res[,4] <= 0.002,]
rownames(good_trios)=seq(1:1:nrow(good_trios))
nrow(good_trios)
hist(res[res[,4] >= 0.002,4],xlab = "Parental Conflict",main = "Histogram of Database Trios")
 nrow(res[res[,4] >= 0.002,])

RG = res[res$V5 == 2,]
nrow(RG)
nrow(SR)
nrow(NR)
SR = res[res$V5 == 6,]
NR = res[res$V5 == 4,]
low = res[res$V4 <=0.05,]
high = res[res$V4 > 0.05,]
hist(low[,4],xlab = "Parental Conflict",main = "Histogram of Database Trios LOW")


nrow(high)
hist(RG[,4])
win.graph()
hist(SR[,4])
win.graph()
hist(NR[,4])
win.graph()
hist(pairs[,6],xlab = "Parental Conflict",main = "Histogram of Database fathers pred")
hist(pairs[,7],xlab = "Parental Conflict",main = "Histogram of Database Mothers pred")
pairs_high = pairs[pairs$V6 >=0.005,]
pairs_low = pairs[pairs$V6 <0.005,]
hist(pairs_high[,6],xlab = "Parental Conflict",main = "Histogram of Database fathers pred high")
hist(pairs_low[,6],xlab = "Parental Conflict",main = "Histogram of Database fathers pred low")


pairs_high = pairs[pairs$V7 >=0.005,]
pairs_low = pairs[pairs$V7 <0.005,]
hist(pairs_high[,7],xlab = "Parental Conflict",main = "Histogram of Database mothers pred high")
hist(pairs_low[,7],xlab = "Parental Conflict",main = "Histogram of Database mothers pred low")
min(pairs_low$V7)




win.graph()
hist(pairs[,7],xlab = "Parental Conflict",main = "Histogram of Database mothers pred")
abline(v=0.005)

text(x=0.0025,y=150,labels="0.5%")
text(x=0.0025,y=130,labels="conflict")

nrow(high)
nrow(low)
nrow(res)
hist(high[,4],xlab = "Parental Conflict",main = "Histogram of Database Trios")
text(x=0.0025,y=150,labels="0.5%")
text(x=0.0025,y=130,labels="conflict")

Female <- read.table("Female.txt", header=F)


Female <- read.table("Female_modified.txt", header=F)
Male <- read.table("Male_modified.txt", header=F)


hist(Male$V2,xlab="Male Heterozygosity",main= "Male histogram")



hist(Female$V2,xlab="Female Heterozygosity",main= "Female histogram")
plot(x=seq(1:1:nrow(Male)), y =Male$V2, ylab="Male Heterozygosity", xlab = "Markers")
abline(v=185,col="BLACK",lty=2)
win.graph()
plot(x=seq(1:1:nrow(Female)), y =Female$V2, ylab="Female Heterozygosity", xlab = "Markers")


setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test")
sex_ped <- read.table("sex_pred.txt", header=T)
sex_ped_male = sex_ped[sex_ped$Gender == "Male",];
rownames(sex_ped_male)=seq(1:1:nrow(sex_ped_male))
sex_ped_female = sex_ped[sex_ped$Gender =="Female",];
rownames(sex_ped_female)=seq(1:1:nrow(sex_ped_female))

nrow(sex_ped_male)
nrow(sex_ped_female)

hist(sex_ped_male$Percentage,xlab="Male Heterozygosity",main= "Male histogram")
hist(sex_ped_male[sex_ped_male$Percentage <= 0.025,3],xlab="Male Heterozygosity",main= "Male histogram")
abline(v=0.05);
nrow(sex_ped_male[sex_ped_male$Percentage > 0.02,])
MALES_miss = miss[miss[,3] > 0.05,]
for(i in 1:8)
{
abline(v=MALES_miss[i,3]);
}

hist(sex_ped_female$Percentage,xlab="Female Heterozygosity",main= "Female histogram")
hist(sex_ped_female[sex_ped_female$Percentage > 0.08,3],xlab="Female Heterozygosity",main= "Female histogram")
nrow(sex_ped_female[sex_ped_female$Percentage <= 0.08,])

hist(sex_ped$V3,xlab="ALL Heterozygosity",main= "All histogram")
	for (i in 1:nrow(sex_ped))
	{ 
		if ( sex_ped[i,3] >= 0.08)
		{
		 sex_ped[i,4] = "Female";
		}
		else if( sex_ped[i,3] <= 0.02)
		{ 
		sex_ped[i,4] = "Male";
	}
	else 
	{
		sex_ped[i,4] = "in between"
	}
}
table(sex_ped[,2],sex_ped[,4])
miss = sex_ped[sex_ped$Gender != sex_ped$V4,]
rownames(miss)=seq(1:1:nrow(miss))
FEMALES_miss = miss[miss[,3] <= 0.05,]

Missclassified = cbind(rep("a",8),rep("a",8),rep("a",8),rep("a",8))
count =1;
for(i in 1:nrow(sex_ped))
{
	if(sex_ped$Gender[i] != sex_ped$V4[i])
	{
		Missclassified[count,1] = as.character(sex_ped[i,1])
		Missclassified[count,2] = as.character(sex_ped[i,2])
		Missclassified[count,3] = sex_ped[i,3]
		Missclassified[count,4] = sex_ped[i,4]
		count =count +1
	}
}

	for (i in 1:nrow(sex_ped_female))
	{ 
		if ( sex_ped_female[i,3] <= 0.10)
		{
		print(sex_ped_female[i,])
		}
	
}

setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\phasebook")
all <- read.table("selected_trios.txt", header=F)
hist(all$V4,xlab="Conflict",main= "Conflict histogram")
abline(v=0.04,col="BLACK",lty=2)
small = all[all$V4 <0.04,]
rownames(small) = seq(1:1,nrow(small))
hist(small$V4,xlab="small Conflict",main= "Conflict histogram")
max(small$V4)
min(small$V4)

setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\families\\RG disease")

fmendal <- read.table("plink.fmendel", header=T)
sum(fmendal$CHLD)
win.graph()
hist(fmendal$N/49248,xlab="Conflict",main= "Conflict histogram")

 


   1 10680
  23     BA002551     BA002675      1 5364
  23     BA002551     BA002692      1 6304
  23     BA002551     BA002712      1 11643
  23     BA002551     BA002718      1 5006
  23     BA002551     BA002721      2 14090
  23     BA002551     BA002725      1 8251
  23     BA002551     BA002726      1   34
  23     BA002551     BA002727      2 1551
  23     BA002551     BA002754      2 10739
  23     BA002551     BA003359      3 7622
  23     BA002551     BA003361      1



setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test")
all <- read.table("Database_trios_M.txt", header=F)
hist(all$V4,xlab="Conflict",main= "Conflict histogram")

defect_2 = all[all$V5  == 2,]
hist(defect_2$V4,xlab="Conflict",main= "RG Conflict histogram")
axis(2,pos=2,at=seq(0:0.01,0.6))
nrow(defect_2)

win.graph()
defect_6 = all[all$V5  == 6,]
hist(defect_6$V4,xlab="Conflict",main= "SH Conflict histogram")
axis(2,pos=2,at=seq(0:0.01,0.6))
defect_6_low = defect_6[defect_6$V4  <= 0.05,]
defect_6_high = defect_6[defect_6$V4  > 0.05,]
rownames(defect_6_low) = seq(1:1,nrow(defect_6_low))
rownames(defect_6_high) = seq(1:1,nrow(defect_6_high))

rownames(defect_6) = seq(1:1,nrow(defect_6))
write.table(pairs_high, file = "Database_Pairs_3.txt", sep  = "\t", quote = FALSE, row.names = FALSE);

write.table(defect_6_low, file = "defect_6_low.txt", sep  = "\t", quote = FALSE, row.names = FALSE);
write.table(defect_6_high, file = "defect_6_high.txt", sep  = "\t", quote = FALSE, row.names = FALSE);


win.graph()
defect_8 = all[all$V5  == 8,]
hist(defect_8$V4,xlab="Conflict",main= "Parents Conflict histogram")
win.graph()
defect_4 = all[all$V5  == 4,]
hist(defect_8$V4,xlab="Conflict",main= "NR Conflict histogram")
nrow(defect_4)



rownames(small) = seq(1:1,nrow(small))
hist(small$V4,xlab="small Conflict",main= "Conflict histogram")





################# Male female pseudo region   ###################

setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\Pseudo_Region")
	Female <- read.table("Female.txt", header=F)
	Male <- read.table("Male.txt", header=F)
plot(x=seq(1:1,nrow(Female)),y = Female$V2,xlab = "X chromosome markers",ylab = "Female hetero")
plot(x=seq(1:1,nrow(Male)),y = Male$V2,xlab = "X chromosome markers",ylab = "Male hetero")



setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test")
marker_conflict <- read.table("Markers_Conflict.txt", header=F)
hist(problem[,3] ,xlab="Conflict",main= "histogram")
hist(marker_conflict[marker_conflict$V3 < 5,3] ,xlab="Conflict",main= "histogram")
abline(v=40)
problem = (marker_conflict[marker_conflict$V3 >= 40  ,])
rownames(problem)=seq(1:1:nrow(problem))
nrow(problem)
write.table(problem, file = "Exclude_Markers_Conflict.txt", sep  = "\t", quote = FALSE, row.names = FALSE);


max(marker_conflict[marker_conflict$V3 >= 5,3])
nrow(marker_conflict[marker_conflict$V3 <= 4,])

Bad_Trios <- read.table("Bad_Trios.txt", header=F)
No_parent = Bad_Trios[Bad_Trios$V2 == 0 & Bad_Trios$V3 == 0,]
father_mother = Bad_Trios[Bad_Trios$V2 != 0 | Bad_Trios$V3 != 0 ,]
hist(father_mother[father_mother$V2 == 0,4] ,xlab="Conflict",main= "histogram of animals with Mother")
hist(father_mother[father_mother$V3 == 0,4] ,xlab="Conflict",main= "histogram of animals with Father")
min(father_mother[father_mother$V2 == 0,4])

 