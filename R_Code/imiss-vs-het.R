setwd("C:\\Users\\Administrator\\Documents\\Revolution\\QTL_Mapping")
imiss=read.table("porc.imiss",h=T)
imiss$logF_MISS = log10(imiss[,6])
het=read.table("porc.het",h=T)
het$meanHet = (het$N.NM. - het$O.HOM.)/het$N.NM.
het$meanHet[23] = 0
het$meanHet[60] = 0
library("geneplotter")
colors  <- densCols(imiss$logF_MISS,het$meanHet)
pdf("raw-GWA-data.imiss-vs-het.pdf")
plot(imiss$logF_MISS,het$meanHet, xlim=c(-3,0),ylim=c(0,0.5),pch=20, xlab="Proportion of missing genotypes", ylab="Heterozygosity rate",axes=F)
axis(2,at=c(0,0.05,0.10,0.15,0.2,0.25,0.3,0.35,0.4,0.45,0.5),tick=T)
axis(1,at=c(-3,-2,-1,0),labels=c(0.001,0.01,0.1,1))
abline(v=log10(0.05),col="RED",lty=2)
abline(v=log10(0.035),col="RED",lty=2)


group1= imiss[imiss$logF_MISS < log10(0.035),]
rownames(group1) = seq(1:1,nrow(group1))


abline(h=0.3124-(2*sd(het$meanHet)),col="RED",lty=2)
 
abline(h=0.3124 ,col="RED",lty=2)
abline(h=0.3124+(2*sd(het$meanHet)),col="RED",lty=2)
abline(0.338392,0.01794  ,col="BLACK")
abline( 0.25717,-0.03668  ,col="BLUE")
 covMcd(het$meanHet)
res = lmrob(het$meanHet~imiss$logF_MISS)
summary(res)
abline(h=mean(het$meanHet)-(2*sd(het$meanHet)),col="RED",lty=2)
abline(h=mean(het$meanHet)+(2*sd(het$meanHet)),col="RED",lty=2)
abline(v=log10(0.05), col="RED", lty=2)

adjbox(1-imiss$F_MISS)
boxplot(1-imiss$F_MISS)


adjbox(het$meanHet)
boxplot(het$meanHet)


install.packages("postgwas")
install.packages("gap")
library("postgwas")
install.packages("ggplot2")
library("gap")
########################## ingiunial hernia  #######################
setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test")
results <- read.table("TDT.txt", header=F)
results$BP = results$BP/1000000
results = results[,c(1,3,7)]
colnames(results) = c("CHR","BP","P")

perm_results <- read.table("plink_SR.tdt.perm", header=T)
results = cbind(results$CHR,results$BP,perm_results$EMP1)
results = cbind(results$CHR,results$BP,results$P)
colnames(results) = c("CHR","BP","P")

 mycolors <- c("red","blue","green","cyan","yellow","black","magenta","red","blue","green",
            "cyan","yellow","black","magenta","red","blue","green","cyan")
par(cex.axis=1.3)
par(las="2",cex=0.6)
par(cex.axis=1.3)
mhtplot(results,pch=19)
title("Real positions with a gap of 10000 bp between chromosomes")


mhtplot(results,mht.control(colors=mycolors,xline = 2,labels=1:18,srt=270),pch=19 )	 
axis(2,pos=2,at=0:15)
abline(h=4.5,col="BLACK",lty=2)
title("Scrotal Hernia without permutation")
title("Scrotal Hernia with 1 million permutation")


source("http://bioinfo-mite.crb.wsu.edu/Rcode/wgplot.R")
par(las="2",cex.axis=1.5)
wgplot(results,cutoffs = -1,color=mycolors,labels=as.character(1:18),pch=19)
 axis(2,pos=2,at=0:15)
abline(h=10,col="RED",lty=2)

j=1;
results$Logpvalue = -log10(results$P)
significant = data.frame()
for(i in 1:nrow(results))
{
	if(!is.na(results[i,11]))
	{
		if(results[i,11] >=5)
		{
		significant[j,1] = results[i,1]
		significant[j,2] = results[i,2]
		significant[j,3] = results[i,6]
		significant[j,4] = results[i,7]
		significant[j,5] = results[i,11]
		
		j=j+1;
		}
	}
}
 colnames(significant) = c("CHR","SNP","T","U","-log10(p)")
 results = results[results$SNP != "ALGA0109178",]
 results = results[results$SNP != "H3GA0053809",]
 results = results[results$SNP != "DRGA0006183",]
 nrow(results)
 

########################## Cryptorchidism   #####################
setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\phasebook")
results <- read.table("TDT.txt", header=F)
results = results[,c(1,3,7)]
colnames(results) = c("CHR","BP","P")
results$BP = results$BP/1000000
results$P = -log(results$P)
write.table(results, file = "TDT_result.txt", sep  = "\t", quote = FALSE, row.names = FALSE);

 

perm_results <- read.table("plink_RG.tdt.perm", header=T)
results = cbind(results$V1,results$V3/1000000,-log(results$V7))
results$V10 =  -log(results$V7);
results = cbind(results$V1,results$V10,results$V6)

results = results[results]
min(results$P);
results[results$V7 == 0 ,]
(175-42)^2 
17689/(175+42)

library('gap')
 colors <- c("red","blue","green","cyan","yellow","black","magenta","red","blue","green",
			"cyan","yellow","black","magenta","red","blue","green","cyan","black")
 par(cex.axis=1.3)
mhtplot(results,usepos=TRUE,colors=colors,gap=10000,pch=19,bg=colors)
title("Real positions with a gap of 10000 bp between chromosomes")
warnings()

 
source("http://bioinfo-mite.crb.wsu.edu/Rcode/wgplot.R")
par(las="2",cex.axis=1.5)
wgplot(results,cutoffs = -1,color=mycolors,labels=as.character(1:18),pch=19)
 axis(2,pos=2,at=0:15)
abline(h=10,col="RED",lty=2)
title("Cryptorchidism with 1 million permutation")

j=1;
results$Logpvalue = -log10(results$V7)
significant = data.frame()
for(i in 1:nrow(results))
{
	if(!is.na(results[i,8]))
	{
		if(results[i,8] >=6)
		{
		significant[j,1] = results[i,1]
		significant[j,2] = results[i,2]
		significant[j,3] = results[i,4]
		significant[j,4] = results[i,5]
		significant[j,5] = results[i,8]
		
		j=j+1;
		}
	}
}



################ call_rate and HWE  ############################
missing_HWE = function()
{
	channel = odbcDriverConnect(connection = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=db_Proc;User=root;Password=mahmoud;", readOnlyOptimize = TRUE)
	query = "SELECT Chr, Marker_Id, Position, Call_Rate, Founder_Control FROM tblmarker_loc  WHERE (Chr <> 'X')AND (Founder_Control < 10) AND (Call_Rate >=0.8) ORDER BY call_rate";
	Data_Set=sqlQuery(channel, query)
	odbcClose(channel)
	return(Data_Set)
}

miss_hwe = missing_HWE();
miss_hwe = miss_hwe[miss_hwe$Founder_Control !=10,]
rownames(miss_hwe) = seq(1:1,nrow(miss_hwe))
plot(x = miss_hwe$Call_Rate ,y = miss_hwe$Founder_Control , xlab="Call Rate", ylab="HWE P-Value")
plot(x = miss_hwe$Founder_Control ,y = miss_hwe$Call_Rate , xlab="HWE P-Value", ylab="Call Rate")
abline(v = 0.000096 ,col = "RED",lty = 2)


miss_hwe = miss_hwe[miss_hwe$Founder_Control <= 0.0001,]
win.graph();
plot(x = miss_hwe$Founder_Control ,y = miss_hwe$Call_Rate , xlab="HWE P-Value", ylab="Call Rate")
abline(v = 0.000096 ,col = "RED",lty = 2)



miss_hwe = miss_hwe[miss_hwe$Call_Rate >= 0.80,]
win.graph();
plot(x = miss_hwe$Founder_Control ,y = miss_hwe$Call_Rate , xlab="HWE P-Value", ylab="Call Rate")
 
miss_hwe = miss_hwe[miss_hwe$Founder_Control > 0.000096,]
nrow(miss_hwe)
plot(x = miss_hwe$Call_Rate ,y = -log10(miss_hwe$Founder_Control) , xlab="Call Rate", ylab="-log(P-Value)", main = "Line 23")
abline(h = 1 ,col = "RED",lty = 2)

format(10^-1,scientific = F)
-log10(0.22405)
windows = data.frame(rows = 20)
windows[1] = 0;
windows[2] = 0;
upper = 0;
lower = 0;
for(i in 1:nrow(miss_hwe))
{
	if(miss_hwe[i]$Call_Rate >=0.99 & miss_hwe[i]$Call_Rate <= 1)
	{
		if(miss_hwe[i]$founder_control >1)
		{
			upper = upper + 1
		}
		else 
		{
			lower = lower + 1
		}
	}
}
windows[1] = upper/lower

