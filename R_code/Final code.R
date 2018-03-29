###########################################################################
####################### Analysis (1) BE CR ################################
###########################################################################
filename = "D:\\Results_paper\\BE CR\\"
setwd("H:\\My documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups\\");
results <- read.table("TDT_2.txt", header=F)
results = results[,c(2,1,3,15,12,13,16,17,18)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
grep(pattern=paste("^",'ALGA0081437',"$",sep=""),x=as.character(results[,1]))
results = results[-42716,]
#results = results[-log10(na.omit(results$P)) <= 22, ]
#rownames(results) = seq(1:1,nrow(results))

xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
mhtplot(results[,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=23,by=1),cex.lab=5)
abline(h=0)
title("TDT For CR Disease Belgium")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results, file = paste(filename,"BE_CR.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);


###########################################################################
####################### Analysis (2) BE SH ################################
###########################################################################
filename = "D:\\Results_paper\\BE SH\\"
setwd("H:\\My documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups\\");
results <- read.table("TDT_6.txt", header=F)
results = results[,c(2,1,3,15,12,13,16,17,18)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
grep(pattern=paste("^",'ALGA0100242',"$",sep=""),x=as.character(results[,1]))
results = results[-20683,]
#results = results[-log10(na.omit(results$P)) <= 22, ]
#rownames(results) = seq(1:1,nrow(results))

xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
mhtplot(results[,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=23,by=1),cex.lab=5)
abline(h=0)
title("TDT For SH Disease Belgium")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results, file = paste(filename,"BE_SH.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);


###########################################################################
####################### Analysis (3) BE CR + SH ###########################
###########################################################################
filename = "D:\\Results_paper\\BE CR + SH\\"
setwd("D:\\GIGA lab\\Norway day\\Analysis\\BE_RG\\");
#setwd("D:\\GIGA lab\\Norway day\\Analysis\\BE\\");
results <- read.table("BE_pvalues.txt", header=F)
results = results[,c(2,1,3,7,4,5,9)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","P2")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results = results[- grep(pattern=paste("^",'ALGA0027483',"$",sep=""),x=as.character(results[,1])),]
results = results[- grep(pattern=paste("^",'H3GA0015207',"$",sep=""),x=as.character(results[,1])),]
results = results[- grep(pattern=paste("^",'ALGA0049287',"$",sep=""),x=as.character(results[,1])),]
results = results[- grep(pattern=paste("^",'DRGA0010743',"$",sep=""),x=as.character(results[,1])),]
results = results[- grep(pattern=paste("^",'ALGA0081437',"$",sep=""),x=as.character(results[,1])),]
results = results[- grep(pattern=paste("^",'ALGA0100242',"$",sep=""),x=as.character(results[,1])),]

#### Version 2 ####
results = results[- grep(pattern=paste("^",'ALGA0109178',"$",sep=""),x=as.character(results[,1])),]


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
#mhtplot(results[results$CHR != 19,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mhtplot(results[results$CHR != 19,c(2:3,7)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec[1:19], side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=45,by=1),cex.lab=5)
abline(h=0)
#title("TDT For merging CR & SH Disease Belgium")
title("TDT For combining CR & SH Disease p-values Belgium")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results3, file = paste(filename,"TEST.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);




###########################################################################
####################### Analysis (4) NO CR ################################
###########################################################################
filename = "D:\\Results_paper\\NO CR\\"
setwd("D:\\Results_paper\\NO CR\\");
results <- read.table("TDT_cases.txt", header=F)
results = results[,c(1,2,3,7,4,5,8,9,10)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results = results[- grep(pattern=paste("^",'ALGA0122477',"$",sep=""),x=as.character(results[,1])),]


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
mhtplot(results[,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=23,by=1),cex.lab=5)
abline(h=0)
title("TDT For CR Disease Norway")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results, file = paste(filename,"NO_CR.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);



###########################################################################
####################### Analysis (5) NO SH ################################
###########################################################################
filename = "D:\\Results_paper\\NO SH\\"
#setwd("D:\\GIGA lab\\Norway day\\Analysis\\NO_SR\\");
setwd("D:\\GIGA lab\\Norway day\\Til Mahmoud\\Scrotal\\")
#results <- read.table("TDT_NO_SR.txt", header=F)
results <- read.table("TDT_cases.txt", header=F)
results = results[,c(1,2,3,7,4,5,8,9,10)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results = results[- grep(pattern=paste("^",'ASGA0083479',"$",sep=""),x=as.character(results[,1])),]


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
mhtplot(results[,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=23,by=1),cex.lab=5)
abline(h=0)
title("TDT For SH Disease Norway")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results, file = paste(filename,"NO_SH.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);







###########################################################################
####################### Analysis (6) NO CR & SH ###########################
###########################################################################
filename = "D:\\Results_paper\\NO CR + SH\\"
setwd("D:\\Results_paper\\NO CR + SH\\")
results <- read.table("NO_pvalues.txt", header=F)
results = results[,c(1,2,3,9,6,7,11)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","P2")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results$logp2 = -log10(results$P2)
results = results[- grep(pattern=paste("^",'ALGA0122477',"$",sep=""),x=as.character(results[,1])),]


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
mhtplot(results[,c(2:3,7)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=23,by=1),cex.lab=5)
abline(h=0)
#title("TDT For merging CR & SH Disease Norway")
title("TDT For combining CR & SH Disease p-values Norway")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results, file = paste(filename,"NO_SH.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);


###########################################################################
####################### Analysis (7) CR BE + NO ###########################
###########################################################################
filename = "D:\\Results_paper\\CR BE + NO\\";
setwd("D:\\Results_paper\\CR BE + NO\\");
#setwd("D:\\GIGA lab\\Norway day\\Analysis\\BE\\");
results <- read.table("TDT_CR_BE_NO.txt", header=T)
results = results[,c(1,2,3,17,14,15,19)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","P2")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results$logp2 = -log10(results$P2)
results = results[- grep(pattern=paste("^",'ALGA0122477',"$",sep=""),x=as.character(results[,1])),]


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(1:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
mhtplot(results[results$CHR !=0,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mhtplot(results[results$CHR !=0,c(2:3,7)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec[1:19], side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=45,by=1),cex.lab=5)
abline(h=0)
title("TDT For CR data set")
#title("TDT For combining CR Belgium & Norway p-values")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results3, file = paste(filename,"TEST.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);




###########################################################################
####################### Analysis (8) SH BE + NO ###########################
###########################################################################
filename = "D:\\Results_paper\\SH BE + NO\\";
setwd("D:\\Results_paper\\SH BE + NO\\");
#setwd("D:\\GIGA lab\\Norway day\\Analysis\\BE\\");
results <- read.table("TDT_SH_BE_NO.txt", header=T)
results = results[,c(1,2,3,17,14,15,19)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","P2")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results$logp2 = -log10(results$P2)
results = results[- grep(pattern=paste("^",'ALGA0122477',"$",sep=""),x=as.character(results[,1])),]


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
#mhtplot(results[results$CHR != 0 ,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mhtplot(results[,c(2:3,7)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec[1:19], side=1, line=0, at=xat,cex.lab=0.85)
axis(2,at=seq(from=0,to=45,by=1),cex.axis=0.85)
abline(h=0)
title("TDT For SH data set")
#title("TDT For combining SH Belgium & Norway p-values")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results3, file = paste(filename,"TEST.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);


















results <- read.table("SR.tdt", header=T,sep="\t")
results = na.omit(results)
results$logp = -log10(results$P)
results = results[-log10(na.omit(results$P)) <= 22, ]
rownames(results) = seq(1:1,nrow(results))

xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)

win.graph()
mhtplot(results[,c(1,3,10)],hcontrol=hmht.control(yoffset=0.05), control = mht.control(logscale=F,usepos = FALSE,cex=1.2,colors=mycolors,srt=10,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=23,by=1),cex.lab=5)
abline(h=0)
title("TDT For Scrotal Hernia Disease Belgium")
abline(h=5,col=4,lty=2)
abline(h=6,col=2,lty=2)


mhtplot(results[ results$CHR == 23,c(1,3,10)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,srt=10,labels=rep("",20),yline=2,xline=2),pch = 16)
axis(2,at=0:24)
abline(h=0)
-log10(8.41765383171114E-06)



## Maternal transmission of pesudo-autosomal region
setwd("H:\\My documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups\\")
results <- read.table("RG_SH_both_62.txt", header=F)
results = results[,c(2,1,3,15,12,13)] # the last one is 6 or 9
colnames(results) = c("Marker_ID","CHR","BP","P","T","U")
results$logp = -log10(results$P)
results$BP = results$BP / 1000000
x = c("black","antiquewhite4")
mycolors = rep(x,10)
max(results[7])
win.graph()
mhtplot(results[results$CHR == "X",c(2,3,4)], control = mht.control(usepos = FALSE,cex=1.2,colors=mycolors,srt=10,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=54,by=1),cex.lab=5)
abline(h=0)
title("Paternal Transmission PAR for CR & SH Belgium")
abline(h=5,col=4,lty=2)
abline(h=6,col=2,lty=2)




setwd("D:\\Psorovis_Plink\\")
mds = read.table("plink.mds",header = TRUE);
plot(mds[,4:5],pch=20,xlab = "C1" , ylab= "C2", main= "Multidimensional Scaling Plot", cex=1.5);
legend("topleft",legend=c("Line 12","Line 15", "Line 23", "Line 33", "Line 36"), text.col = c("1","2","3","4","5"))
plot(mds[,1:4],pch=20,col = mds$Line)


results <- read.table("Association_Analysis.qassoc", header=T)
x = c("black","antiquewhite4")
mycolors = rep(x,15)
mycolors = c(mycolors,"black")
results = na.omit(results)
max(results[7])
win.graph()
mhtplot(results[,c(1,3,9)], control = mht.control(logscale=T,usepos = FALSE,cex=1.2,colors=mycolors,srt=10,labels=rep("",31),yline=2,xline=2),pch = 16)
xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:29),"X")
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=23,by=1),cex.lab=5)
abline(h=0)
title("Association Analysis")

qqunif(results$P,type="unif")