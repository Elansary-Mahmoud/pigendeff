library('RODBC')
library('ROCR')
library('Daim')
library('qtl')
library('snow')
install.packages('RODBC')
install.packages('ROCR')

Query_Gender <- function()
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



library("qtl")
data(map10)
typeof(map10)
simcross = sim.cross(map10,type = "f2", n.ind = 100, missing.prob = 0.02)
geno.image(simcross)
######################################################
augmentedcross = mqmaugment(simcross, minprob = 1)
geno.image(augmentedcross)

######################################################
augmentedcross = mqmaugment(simcross, minprob = 0.02)
geno.image(augmentedcross)

#####################################################

data(multitrait)
msim5 <- simulatemissingdata(multitrait, 5)
msim10 <- simulatemissingdata(multitrait, 10)
msim80 <- simulatemissingdata(multitrait, 80)

maug5 <- mqmaugment(msim5)
maug10 <- mqmaugment(msim10, minprob = 0.25)
maug80 <- mqmaugment(msim80, minprob = 0.8)

maug10minprob <- mqmaugment(msim10, minprob = 0.001, verbose = TRUE)
####################################################
?mqmscan
mqm5 <- mqmscan(maug5)
mq10 <- mqmscan(maug10)
mqm80 <- mqmscan(maug80)

msim5 <- calc.genoprob(msim5)
one5 <- scanone(msim5)

msim10 <- calc.genoprob(msim10)
one10 <- scanone(msim10)

msim80 <- calc.genoprob(msim80)
one80 <- scanone(msim80)

################ Augmentation of the Data ######################

maug_min1 <- mqmaugment(multitrait, minprob = 1)
mqm_min1 <- mqmscan(maug_min1)

mgenop <- calc.genoprob(multitrait, step = 5)
m_one <- scanone(mgenop)


maug <- mqmaugment(multitrait)
mqm <- mqmscan(maug)
real_markers <- mqmextractmarkers(mqm)   # real markers without the pseudo markers 

max(mqm)
find.marker(maug, chr = 5, pos = 35)
multitoset <- find.markerindex(maug, "GH.117C")
setcofactors <- mqmsetcofactors(maug, cofactors = multitoset)
mqm_co1 <- mqmscan(maug, setcofactors)


par(mfrow = c(2, 1))
plot(mqmgetmodel(mqm_co1))
plot(mqm_co1)

plot(m_one, mqm_co1, col = c("black", "green"), lty = 1:2)
legend("topleft", c("scanone", "MQM"), col = c("black", "green"),lwd = 1)



multitoset <- c(multitoset, find.markerindex(maug, find.marker(maug,4,10)))
setcofactors <- mqmsetcofactors(maug, cofactors = multitoset)
mqm_co2 <- mqmscan(maug, setcofactors)

#### Ploting data with two cofactor at chromosome 5 35cM and chromosome 4 10cM ####

par(mfrow = c(2, 1))
plot(mqmgetmodel(mqm_co2))
plot(mqm_co1, mqm_co2, col = c("blue", "green"), lty = 1:2)
legend("topleft", c("one cofactor", "two cofactors"), col = c("blue","green"), lwd = 1)

plot(mqm, mqm_co1, mqm_co2, col = c("green", "red", "blue"),lty = 1:3)
legend("topleft", c("no cofactor","one cofactor","two cofactors"), col = c("green","red","blue"), lwd = 1)


#############		Unsupervised cofactor selection through backward elimination:		########

autocofactors = mqmautocofactors(maug , 50)
mqm_auto = mqmscan(maug, autocofactors)
setcofactors <- mqmsetcofactors(maug, 5)
mqm_backw <- mqmscan(maug, setcofactors)

par(mfrow = c(2, 1))
mqmplot.cofactors(maug, autocofactors, justdots = TRUE)
mqmplot.cofactors(maug, setcofactors, justdots = TRUE)

par(mfrow = c(2, 1))
plot(mqmgetmodel(mqm_backw))
plot(mqmgetmodel(mqm_auto))

par(mfrow = c(2, 1))
plot(mqmgetmodel(mqm_backw))
plot(mqm_backw)

plot(m_one, mqm_backw, col = c("black", "green"), lty = 1:2)
legend("topleft", c("scanone", "MQM"), col = c("black", "green"),lwd = 1)


########		Plot with lowered cofactor.significance:		#######

mqm_backw_low <- mqmscan(maug, setcofactors, cofactor.significance = 0.002)
par(mfrow = c(2,1))
plot(mqmgetmodel(mqm_backw_low))
plot(mqm_backw, mqm_backw_low, col = c("blue", "green"), lty = 1:2)
legend("topleft", c("Significant = 0.02", "Significant = 0.002"), col = c("blue", "green"), lwd = 1)

mqmplot.singletrait(mqm_backw_low, extend = TRUE)


###########		Create a directed QTL plot		########################

dirresult = mqmplot.directedqtl(multitrait, mqm_backw_low)
effectplot(multitrait, mname1 =  "GH.117C", mname2 = "GA1")
effectplot(multitrait, mname1 =  "PVV4", mname2 = "GH.117C")

par(mfrow = c(2,2))
plot.pxg(multitrait, marker = "GH.117C")
plot.pxg(multitrait, marker = "GA1")
plot.pxg(multitrait, marker = "PVV4")


########################################################################

library("snow")  
library("nws")
require(snow)	# require same as Library function
results <- mqmpermutation(maug, scanfunction = mqmscan,multicore = TRUE, cofactors = setcofactors, n.perm = 25, batchsize = 25)
resultsrqtl <- mqmprocesspermutation(results)
summary(resultsrqtl)
mqmplot.permutations(results)

data(multitrait)
m_imp <- fill.geno(multitrait)
mqmscanfdr(m_imp, mqmscanall, cofactors = setcofactors)


mqm_imp5 <- mqmscan(m_imp, pheno.col = c(1, 2, 3, 4, 5))
mqmplot.multitrait(mqm_imp5, type = "image")

cofactorlist <- mqmsetcofactors(m_imp, 3)
mqm_imp5 <- mqmscan(m_imp, pheno.col = c(1, 2, 3, 4, 5), cofactors = cofactorlist)
mqmplot.multitrait(mqm_imp5, type = "lines")
mqmplot.multitrait(mqm_imp5, type = "image")

mqmplot.circle(m_imp, mqm_imp5)
mqmplot.circle(m_imp, mqm_imp5, highlight = 2)


###############		the locations of the Markers on the genome		##################

data(locations) 
multiloc <- addloctocross(m_imp, locations)
mqmplot.cistrans(mqm_imp5, multiloc, 5, FALSE, TRUE)

mqmplot.circle(multiloc, mqm_imp5, highlight = 2)
mqmtestnormal(m_imp)


multitrait$geno


9300 - 8745

555 - 365

190 / 30

30 * 6

######################################################################################
library('qtl')
setwd("C:\\Users\\Elansary\\Documents\\Revolution\\QTL_Mapping\\QTL_Mapping\\MQM Analysis")
Data_Set <- read.cross("csv", ".", "MQM2.csv")
Data_Set$pheno

save(list = ls(all=TRUE), file = "Data_Set.RData")


summary(Data_Set)
plot(Data_Set)

data(hyper)
newmap <- est.map(hyper, error.prob=0.01)

summary(hyper)
nind(hyper)
nphe(hyper)
nchr(hyper)
totmar(hyper)
nmar(hyper)

plot(hyper)

plot.missing(hyper)
win.graph();
plot.missing(hyper, reorder=TRUE)

plot.geno(hyper)
plot.pheno(hyper, pheno.col=2)

plot.map(hyper)
plot.map(hyper, chr=c(1, 4, 6, 7, 15), show.marker.names=TRUE)


hyper <- drop.nullmarkers(hyper)
totmar(hyper)

hyper <- est.rf(hyper)


######################		Analysis		################################
setwd("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\porc_Test");
Porc_Data_Set <- read.cross("csv", ".", "MQM.csv");
save(list = ls(all=TRUE), file = "MQM_Porc.RData");
Porc_Data_Set$pheno[,2] = Query_Gender();


sum(nmar(Porc_Data_Set)[2:20]);
plot.missing(Porc_Data_Set);
Porc_Data_Set = calc.errorlod(Porc_Data_Set, error.prob=0.01);
Porc_Data_Set = calc.genoprob(Porc_Data_Set, step=1, error.prob=0.01);
SC = scanone(Porc_Data_Set,model="binary");
SC_perm = scanone(Porc_Data_Set,model="binary",n.perm = 10, perm.Xsp=TRUE);
summary(SC_perm);
summary(SC);
summary(SC, perms = SC_perm, pvalues=TRUE);
summary(SC, thr = 5.72);
plot(SC);
add.threshold(SC, perms = SC_perm, alpha=0.05);
add.threshold(SC, perms = SC_perm, alpha=0.1,col="green", lty=2);

####### Get the map,genotype and phentptype from cross object	###########
map = pull.map(Porc_Data_Set);
genotype = pull.geno(Porc_Data_Set);
phenotype = pull.pheno(Porc_Data_Set);
#########################################################################
###########			Check Marker Order		#############################
rip1 = ripple(Porc_Data_Set, chr=1, window=6)
#########################################################################
Porc_Data_Set = jittermap(Porc_Data_Set)








Porc_Data_Set <- est.rf(Porc_Data_Set)

Data_Set_ALL$geno

newmap <- est.map(Data_Set_ALL, error.prob=0.01)




