Min_Pairs = list(NULL)

	for (i in 1:nrow(indv_list))
	{
   
		k = 0;
		for (j in 1:nrow(pred))
		{ 
			if (as.character(indv_list[i,1]) == as.character(pred[j,1]))
			{
				k = k + 1;  
				if(k == 1)
				{
					Min_Pairs[[i]] = paste(as.character(pred[j,1]),as.character(pred[j,2]),as.character(pred[j,5]), collapse= "", sep = "%")
	 
				}
				else 
				{
					Min_Pairs[[i]][k] = paste(as.character(pred[j,1]),as.character(pred[j,2]),as.character(pred[j,5]), collapse= "", sep = "%")
				}
				 
			}
		} 
    }
	
	
	final_pedigree = list(NULL);
	for (i in 1:length(Min_Pairs))
	{ 
		Min_Male = 1;
		Min_Female = 1;
		Father = "";
		Mother = "";
		Indv = "";
			for(j in 1:length(Min_Pairs[[i]]))
			{
				 
				if(!is.null(Min_Pairs[[i]][j]))
				{
				splt <- strsplit(Min_Pairs[[i]][j], "%")
				Indv = splt[[1]][1];
				
				if( Query_Gender(as.character(splt[[1]][2])) == "Male" )
				{
					if(splt[[1]][3] < Min_Male)
					{ 
						print(Indv);
						Min_Male = splt[[1]][3];
						Father = splt[[1]][2];
						print(paste("Father",Father,Min_Male,collapse=""));
					}
				}
			else
 				{
					if(splt[[1]][3] < Min_Female)
					{ 
						print(Indv);
						Min_Female = splt[[1]][3];
						Mother = splt[[1]][2];
						print(paste("Mother",Mother,Min_Female,collapse=""));
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
			if(Father != "")
			{
				final_pedigree[[i]] = paste(Indv,"Fater",Father,Min_Male, collapse = "", sep = "  ");
			}
			if(Mother != "")
			{
				final_pedigree[[i]] = paste(Indv,"Mother",Mother,Min_Female, collapse = "", sep = "  ");
			}
		}
	}

 ###

Pedigree = Query_Pedigree()
Results = data.frame(NULL)
for(i in 1 :length(final_pedigree))
{
	if(length(final_pedigree[[i]])  == 2)
	{
		splt_father <- strsplit(final_pedigree[[i]][1], "  ")
		splt_mother <- strsplit(final_pedigree[[i]][2], "  ")
		Results[i,1] = splt_father[[1]][1];
		Results[i,2] = splt_father[[1]][3];
		Results[i,3] = splt_mother[[1]][3];
		Results[i,4] = splt_father[[1]][4];
		Results[i,5] = splt_mother[[1]][4];
	}
	else if(length(final_pedigree[[i]])  == 1)
	{ 
		splt <- strsplit(final_pedigree[[i]][1], "  ")
		if(splt[[1]][2]  == "Mother")
		{
			Results[i,1] = splt[[1]][1];
			Results[i,2] = "";
			Results[i,3] = splt[[1]][3];
			Results[i,4] = "";
			Results[i,5] = splt[[1]][4];
		}
		else 
		{
			Results[i,1] = splt[[1]][1];
			Results[i,2] = splt[[1]][3];
			Results[i,3] = "";
			Results[i,4] = splt[[1]][4];
			Results[i,5] = "";

		}

	}

	
}

colnames(Results) = c("Indv","Father","Mother","Fater_perc","Mother_perc")

New_Results <- Results[!is.na(Results[,1]),]
rownames(New_Results)   = (seq(1:1:nrow(New_Results)))


for(i in 1: nrow(Pedigree))
{
	for (j in 1: nrow(New_Results))
	{
		#print(paste(as.character(Pedigree[i,1]),as.character(data[j,1]),as.character(Pedigree[i,3]),as.character(data[j,2]),collapse = ""))
		if(as.character(Pedigree[i,1]) == as.character(New_Results[j,1]))
		{
			print(paste(i,"Indv Id",New_Results[j,1],collapse=""));
			if(as.character(Pedigree[i,2]) == as.character(New_Results[j,2]))
			{
				print(paste("Father Id",New_Results[j,2],collapse=""));
			}
			if(as.character(Pedigree[i,3]) == as.character(New_Results[j,3]))
			{
				print(paste("Mother Id",New_Results[j,3],collapse=""));
			}
		}
	}
}


data(listeria)
dat <- pull.geno(listeria)
# image of the genotype data
image(1:ncol(dat),1:nrow(dat),t(dat),ylab="Individuals",xlab="Markers",
      col=c("red","yellow","blue","green","violet"))
abline(v=cumsum(c(0,nmar(listeria)))+0.5)
abline(h=nrow(dat)+0.5)


data(badorder)
summary(badorder)
plot(badorder)
badorder <- est.rf(badorder)
plot.rf(badorder)
plot.rf(badorder, chr=1)

newmap <- est.map(badorder, verbose=TRUE)
plot.map(badorder, newmap)

rip1 <- ripple(badorder, chr=1, window=6)
summary(rip1)







setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\phasebook")
results <- read.table("TDT.txt", header=F)
results = results[,c(1,3,7)]


results = cbind(results$V1,results$V3/1000000,results$V7)
results$V8 = -log10(results$V7)
results = cbind(results$V1,results$V8,results$V6)
colnames(results) = c("CHR","BP","P")
max(results$V8)
results = results[results$V8 > 14,]

library('gap')
 mycolors <- c("red","blue","green","cyan","yellow","black","magenta","red","blue","green",
			"cyan","yellow","black","magenta","red","blue","green","cyan")
par(cex.axis=1.3)
par(las="2",cex=0.6)
mhtplot(results,mht.control(colors=mycolors,xline = 2,labels=1:18,srt=270),pch=19 )	 

axis(2,pos=2,at=0:15)
abline(h=3,col="BLACK",lty=2)
title("Cryptorchidism without permutation my code")


setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\phasebook")
defect_6_low_pairs <- read.table("defect_6_low_pairs.txt", header=F)
defect_6_high_pairs <- read.table("defect_6_high_pairs.txt", header=F)
defect_6= rbind(defect_6_low_pairs,defect_6_high_pairs);
plot(x=seq(1:1,35),y=defect_6_high_pairs$V6,col ="red",xlab = "Offsprings",ylab= "Father Mother Conflict")
points(x=seq(1:1,35),y=defect_6_high_pairs$V3,col="green")
legend("topleft",c("Father","Mother"), lty=c(1,1),lwd=c(2.5,2.5),col=c("red","green")) 


hist(defect_6_low_pairs$V3,xlab="conflict",main="Father offspring Conflict")
 
hist(defect_6_low_pairs$V6,xlab="conflict",main="Mother offspring Conflict")
abline(v=0.005,col="BLACK",lty=2)


defect_6_low_pairs = defect_6_low_pairs[defect_6_low_pairs$V6 <= 0.005,]
hist(defect_6_low_pairs$V6,xlab="conflict",main="Mother offspring Conflict zoom in for the animals with 0-0.005 conflict")


hist(defect_6_low_pairs$V6,breaks=c(seq(0,max(defect_6_low_pairs$V6)+1, .1)), xlim=range(c(seq(0,max(defect_6_low_pairs$V6)+1, .1))))


plot(x=seq(1:1,35),y=defect_6_low_pairs$V3,col ="red",xlab = "Offsprings",ylab= "Father Mother Conflict")
points(x=seq(1:1,35),y=defect_6_low_pairs$V6,col="green")
legend("topright",c("Father","Mother"), lty=c(1,1),lwd=c(2.5,2.5),col=c("red","green")) 
 
count = 0;
for(i in 1:nrow(defect_6_low_pairs))
{
	if(defect_6_low_pairs$V3[i] >= defect_6_low_pairs$V6[i])
	{
		count = count + 1;
	}
}

setwd("D:\\Master of Bioinformatics\\GIGA Lab\\Analysis\\C#\\phasebook")
results <- read.table("tdt.txt", header=F)

results = na.omit(results)
results$V8 =  -log10(results$V7)
results = results[results$V8 < 14,]
results = cbind(results$V1,results$V3/1000000,results$V7)
colnames(results) = c("CHR","BP","P")
library('gap')
mycolors <- c("red","blue","green","cyan","yellow","black","magenta","red","blue","green",
			"cyan","yellow","black","magenta","red","blue","green","cyan")
par(cex.axis=1.3)
par(las="2",cex=0.6)
mhtplot(results,mht.control(colors=mycolors,xline = 2,labels=1:18,srt=270),pch=19 )	 
axis(2,pos=2,at=0:15)
abline(h=4,col="BLACK",lty=2)
title("Scrotal hernia ")

colnames(results) = c("CHR","Marker_Id","BP","T","U","tdt value","pvalue","-log10(pvalue)")
 results[results$"-log10(pvalue)" > 4,]

-log10(0.0003)

10^(3.5)
1/3162.278