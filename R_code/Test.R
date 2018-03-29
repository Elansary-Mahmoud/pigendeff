setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test");

results <- read.table("TDT_6_all.txt", header=F)
results = results[,c(2,1,3,7,4,5,8,9,10)]
colnames(results) = c("Marker_ID","CHR","BP","P","T","U","Nonsense","Conflict","Missing")
results$BP = results$BP/1000000

for(i in 0:18)
{
	win.graph()
	qqunif(results[results$CHR == i,4])
	title(paste("TDT For Scrotal rupture Disease CHR ",i,collapse = "",sep = ""))
}

	qqunif(results[,4])
	abline(h=3)
	title("TDT For Cryptorchidism Disease CHR 1:18,X")

	
results <- read.table("TDT_6_lines.txt", header=F)
results = results[,c(2,1,3,4,5,7,8,9,10,14,15,17,18,19,20,24,25,27,28,29,30)]
colnames(results) = c("Marker_ID","CHR","BP","T23","U23","P23","Nonsense23","Conflict23","Missing23","T36","U36","P36","Nonsense36","Conflict36","Missing36","T12","U12","P12","Nonsense12","Conflict12","Missing12")
results$BP = results$BP/1000000
 
pvalues = results[,c(1,2,3,6,12,18)]
mycolors <- c("red","blue","green","cyan","brown4","black","magenta","red","blue","green",
            "cyan","brown4","black","magenta","red","blue","green","cyan","black","red")
win.graph();
mhtplot(pvalues[,c(2:3,6)],control = mht.control(usepos = FALSE,cutoffs = 3.5,colors=mycolors,yline=3,xline=3))
textvec <- as.character(0:18)
xat <- locator()$x
xat <- round(xat)
mtext(textvec, side=1, line=0, at=xat)
axis(2,at=0:10)
abline(h=0)
title("TDT For Navel rupture Disease")


res = data.frame(cbind(rep(0,38), rep(0,38),rep(0,38), as.double(rep(0,38)),as.double(rep(0,38)), as.double(rep(0,38)),as.double(rep(0,38))));
for(i in 1:nrow(significant_SR))
{
	res[i,1] = (results[results$V2 == as.character(significant_SR$Marker_ID[i]),1])
	res[i,2] = as.character(results[results$V2 == as.character(significant_SR$Marker_ID[i]),2])
	res[i,3] = (results[results$V2 == as.character(significant_SR$Marker_ID[i]),3])
	res[i,4] = log10(1/as.numeric(as.character(results[results$V2 == as.character(significant_SR$Marker_ID[i]),4])))
	res[i,5] = log10(1/as.numeric(as.character(results[results$V2 == as.character(significant_SR$Marker_ID[i]),5])))
	res[i,6] = log10(1/as.numeric(as.character(results[results$V2 == as.character(significant_SR$Marker_ID[i]),6])))
	res[i,7] = log10(1/as.numeric(as.character(results[results$V2 == as.character(significant_SR$Marker_ID[i]),7])))
	
}



results <- read.table("TDT_6_lines.txt", header=F)
qqunif(as.numeric(as.character(results$V4)))
