plot(results$index,results$logp2,col=results$col,pch=19,bty="l")
CHR_numbers = unlist(lapply(c(1:18,20),function(x){ nrow(results[results$CHR == x,])}))
results$col[results$CHR %in% seq(from=0,to=18,by=2)] = "antiquewhite4"
results$col[results$CHR %in% seq(from=1,to=19,by=2)] = "Black"
results$col[results$CHR == 20] = "Black"

results$index = 1:nrow(results)

results[results$CHR == 20,7] =  seq(from=29284,to=29284 + 1089,by=10)



map_all = read.table('I:/My documents/Visual Studio 2010/Projects/porc_Test/Latest_Formate.map')
ped_all = read.table('I:/My documents/Visual Studio 2010/Projects/porc_Test/Latest_Formate.ped')
colnames(ped_all) = c("sampleID",as.character(map_all[,2]))
subset = read.table('D:/Results_paper/BE CR/RG_Maternal_2.txt')
PAR = intersect(as.character(subset[,2]),as.character(map_all[,2]))
PAR_SNP = cbind(ped_all[,1],ped_all[,PAR])
write.table(PAR_SNP,file='D:/Results_paper/BE_Data.ped',row.names=F,col.names=F,quote=F)
rownames(map_all) = as.character(map_all[,2])

write.table(map_all[PAR,1:3],file='D:/Results_paper/BE_Data.map',row.names=F,col.names=F,quote=F)

test = read.table('D:/Results_paper/BE SH/SH_Maternal_6.txt')