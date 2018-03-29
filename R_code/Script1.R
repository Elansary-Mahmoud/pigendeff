rm(list=ls(all=TRUE)) 
library('robustbase')
library('gap')
source("http://bioinfo-mite.crb.wsu.edu/Rcode/wgplot.R")
setwd("C:\\Users\\Administrator\\Documents\\Revolution\\QTL_Mapping\\Plink")
results <- read.table("plink_RG.tdt", header=T)
results = cbind(results$CHR,results$BP,results$P)
results = results[results[,1] !=0,]
colnames(results) = c("CHR","BP","P")
##################    ploting the manhattan plot   #############
 mycolors <- c("red","blue","green","cyan","blue","gray","magenta","red","blue","green",
            "cyan","yellow","gray","magenta","red","blue","green","cyan")
par(las="2",cex.axis=1.5)
wgplot(results,cutoffs = -1,color=mycolors,labels=as.character(0:19),pch=19)
axis(2,pos=2,at=0:15)
abline(h=10,col="RED",lty=2)

######################  Manhattan plot 2     #######################

mycolors <- c("red","blue","green","cyan","blue","gray","magenta","red","blue","green",
            "cyan","yellow","gray","magenta","red","blue","green","cyan")
par(las="2",cex=0.6)
mhtplot(results,mht.control(colors=mycolors,xline = 2,srt=270),pch=20 )
axis(2,pos=2,at=0:20)
abline(h=10,col="RED",lty=2)
title("cryptorchidism defect");
title("Scrotal rupture defect");
qqnorm(results[,3]) ## QQ plot
 ##################  ##################  ##################  ##########
  ####### SHOW siginificant ones ###########
j=1;
significant = seq(1:1:10)
results$Logpvalue = -log10(results$P)    # CALCULATE THE -log10(pvalue)
 
for(i in 1:nrow(results))
{
	if(!is.na(results[i,11]))
	{
		if(results[i,11] >=10)
		{
			significant[j]=as.character(results[i,2])
			j=j+1;
		}
	}
}
results_omit[results_omit[,11] > 5,11]
results_omit[results_omit$P==1.149e-22,]
boxplot(results_omit[,11])
results[results$SNP=="ALGA0109178",]
results[results$SNP=="H3GA0053809",]
results[results$SNP=="DRGA0006183",]
-log10(1.149e-22)
############# ###################### ####################