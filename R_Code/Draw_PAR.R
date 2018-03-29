###########################################################################
####################### Analysis (1) BE CR ################################
###########################################################################
filename = "D:\\Results_paper\\BE CR\\"
setwd("H:\\My documents\\Visual Studio 2010\\Projects\\porc_Test\\Backups\\");
results <- read.table("RG_SH_Paternal_62.txt", header=F)
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
axis(2,at=seq(from=0,to=60,by=1),cex.lab=5)
abline(h=0)
title("Paternal TDT For CR & SH Disease Belgium PAR")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results, file = paste(filename,"BE_CR.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);

###########################################################################
####################### Analysis (4) NO CR ################################
###########################################################################
filename = "D:\\Results_paper\\NO CR\\"
setwd("D:\\Results_paper\\NO CR\\");
results <- read.table("CR_Maternal_2_M.txt", header=F)
results = results[,c(1,2,3,7,4,5,8,9,10)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results = results[- grep(pattern=paste("^",'ALGA0122477',"$",sep=""),x=as.character(results[,1])),]
N= 34753+200
-log10(0.05/N)
-log10(0.37/N)

plot(results$BP,results$logp,pch = 16,xlab = "-PAR- Chrmosome X (MB)",ylab="-log10(P)",main="Maternal transmission NO SH")
points(tail(results,n=4)[,c(3,10)],col="red",pch = 16)
abline(h=4.975283)


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
mhtplot(results[,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=23,by=1),cex.lab=5)
abline(h=0)
title("Maternal TDT For SH Disease Norway PAR")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results, file = paste(filename,"NO_CR.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);


###########################################################################
####################### Analysis (6) CR NO & BE ###########################
###########################################################################
filename = "D:\\Results_paper\\NO CR + SH\\"
setwd("D:\\Results_paper\\CR BE + NO\\")
results <- read.table("TDT_Maternal_CR_BE_NO.txt", header=T)
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
mhtplot(results[,c(2:3,7)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=30,by=1),cex.lab=5)
abline(h=0)
#title("Maternal TDT For merging CR Belgium & Norway data set PAR")
title("Maternal TDT For CR PAR")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results, file = paste(filename,"NO_SH.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);




###########################################################################
####################### Analysis (8) SH BE + NO ###########################
###########################################################################
filename = "D:\\Results_paper\\SH BE + NO\\";
setwd("D:\\Results_paper\\SH BE + NO\\");
#setwd("D:\\GIGA lab\\Norway day\\Analysis\\BE\\");
results <- read.table("TDT_Maternal_SH_BE_NO.txt", header=T)
results = results[,c(1,2,3,17,14,15,19)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","P2")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results$logp2 = -log10(results$P2)
results = results[- grep(pattern=paste("^",'ALGA0122477',"$",sep=""),x=as.character(results[,1])),]


plot(results$BP,results$logp,pch = 16,xlab = "-PAR- Chrmosome X (MB)",ylab="-log10(P)",main="Maternal transmission NO SH")
points(tail(results,n=4)[,c(3,10)],col="red",pch = 16)

xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
#mhtplot(results[,c(2:4)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mhtplot(results[,c(2:3,7)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec[1:19], side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=45,by=1),cex.lab=5)
abline(h=0)
#title("Maternal TDT For merging SH Belgium & Norway data set PAR")
title("Maternal TDT For SH PAR")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results3, file = paste(filename,"TEST.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);


###########################################################################
####################### Analysis (9) All		 ##########################
###########################################################################
filename = "D:\\Results_paper\\SH BE + NO\\";
setwd("D:\\Results_paper\\All\\");
results <- read.table("All_Maternal_X.txt", header=T)
results = results[,c(1,2,3,7,4,5)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U")
results$BP = results$BP / 1000000
results$logp = -log10(results$P)
results = results[- grep(pattern=paste("^",'ALGA0122477',"$",sep=""),x=as.character(results[,1])),]


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
#mhtplot(results[ ,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mhtplot(results[results$CHR != 0,c(2:4)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec[1:20], side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=45,by=1),cex.axis=0.85)
abline(h=0)
title("Maternal TDT For CR & SH")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results3, file = paste(filename,"TEST.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);



NO_CR = read.table('D:\\Results_paper\\NO CR\\TDT_cases.txt')
NO_SH = read.table('D:\\Results_paper\\NO SH\\TDT_cases.txt')
BE_CR = read.table('D:\\Results_paper\\BE CR\\TDT_2.txt')
BE_SH = read.table('D:\\Results_paper\\BE SH\\TDT_6.txt')


int_SNPs = intersect(intersect(intersect(as.character(NO_CR[,1]),as.character(NO_SH[,1])),as.character(BE_CR[,2])),as.character(BE_SH[,2]) )

NO_CR =  NO_CR[NO_CR[,1] %in% int_SNPs,]
NO_SH =  NO_SH[NO_SH[,1] %in% int_SNPs,]
BE_CR =  BE_CR[BE_CR[,2] %in% int_SNPs,]
BE_SH =  BE_SH[BE_SH[,2] %in% int_SNPs,]

rownames(NO_CR) = as.character(NO_CR[,1])
rownames(NO_SH) = as.character(NO_SH[,1])
rownames(BE_CR) = as.character(BE_CR[,2])
rownames(BE_SH) = as.character(BE_SH[,2])

NO_CR =  NO_CR[as.character(int_SNPs),]
NO_SH =  NO_SH[as.character(int_SNPs),]
BE_CR =  BE_CR[as.character(int_SNPs),]
BE_SH =  BE_SH[as.character(int_SNPs),]


isTRUE(all.equal(as.character(NO_SH[,1]),as.character(NO_CR[,1]) ));
isTRUE(all.equal(as.character(BE_CR[,2]),as.character(BE_SH[,2])));

temp = cbind(NO_SH[,1:3],  NO_SH[,6] +   NO_CR[,6] + BE_CR[,14] + BE_SH[,14] )
colnames(temp) = c("Marker_ID","CHR","BP","X2")
temp$P2 = pchisq(q=temp$X2,df=4,lower.tail=F)

results = temp
results$BP = results$BP / 1000000
results$logp = -log10(results$P2)
results = results[- grep(pattern=paste("^",'ALGA0122477',"$",sep=""),x=as.character(results[,1])),]


xat <- locator()$x
xat <- round(xat)
textvec <- c(as.character(0:18),"X")

x = c("black","antiquewhite4")
mycolors = rep(x,10)
#mhtplot(results[ ,2:4], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mhtplot(results[results$CHR == 19,c(2:3,5)], control = mht.control(usepos = TRUE,cex=1.2,colors=mycolors,labels=rep("",20),yline=2,xline=2),pch = 16)
mtext(textvec[1:20], side=1, line=0, at=xat)
axis(2,at=seq(from=0,to=70,by=1),cex.axis=0.85)
abline(h=0)
title("TDT For CR & SH CHR X")
abline(h=5,col=4,lty=2,lwd =2)
abline(h=6,col=2,lty=2,lwd =2)
write.table(results3, file = paste(filename,"TEST.txt",sep=""), sep  = "\t", quote = FALSE, row.names = FALSE);
-log10(0.05/31833)
-log10(0.37/31833)

-log10(0.05/41088)
-log10(0.37/41088)