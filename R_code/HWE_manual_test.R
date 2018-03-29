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
   mid <- floor(rare * ( 2 * N - rare) / (2 * N))  # expected heterozygous 2*N*pq  and the p = rare/(2*N) , q = common/(2*N)
   if ( (mid %% 2) != (rare %% 2) ) mid <- mid + 1  

   probs[mid + 1] <- 1.0
   mysum <- 1.0

   # Calculate probablities from midpoint down 
   curr_hets <- mid  #expected hetero
   curr_homr <- (rare - mid) / 2  #expected minor homo nAA = (nA - nAB)/2
   curr_homc <- N - curr_hets - curr_homr  #expected comon homo nBB = (nB - nAB)/2

   while ( curr_hets >=  2)
      {
      probs[curr_hets - 1]  <- probs[curr_hets + 1] * curr_hets * (curr_hets - 1.0) / (4.0 * (curr_homr + 1.0)  * (curr_homc + 1.0)) # equation (2) in the paper
      mysum <- mysum + probs[curr_hets - 1]

      # 2 fewer heterozygotes -> add 1 rare homozygote, 1 common homozygote
      curr_hets <- curr_hets - 2
      curr_homr <- curr_homr + 1
      curr_homc <- curr_homc + 1
      }    

   # Calculate probabilities from midpoint up
   # recalculate the starting expected hetero, homor,homoc
   curr_hets <- mid
   curr_homr <- (rare - mid) / 2
   curr_homc <- N - curr_hets - curr_homr
   
   while ( curr_hets <= rare - 2)
      {
      probs[curr_hets + 3] <- probs[curr_hets + 1] * 4.0 * curr_homr * curr_homc / ((curr_hets + 2.0) * (curr_hets + 1.0))  # equation (2) in the paper
      mysum <- mysum + probs[curr_hets + 3]
         
      # add 2 heterozygotes -> subtract 1 rare homozygtote, 1 common homozygote
      curr_hets <- curr_hets + 2
      curr_homr <- curr_homr - 1
      curr_homc <- curr_homc - 1
      }    
	# mysum will contain the total sum of the whole probability table : mysum = sum(probs) 
    # P-value calculation
    target <- probs[obs_hets + 1]

    #plo <- min(1.0, sum(probs[1:obs_hets + 1]) / mysum)

    #phi <- min(1.0, sum(probs[obs_hets + 1: rare + 1]) / mysum)

    # This assignment is the last statement in the fuction to ensure 
    # that it is used as the return value
    p <- min(1.0, sum(probs[probs <= target])/ mysum)
    }

	
	p_values <- SNPHWE(148,35,643);
	p_values <- SNPHWE(33, 175, 4);
	p_values <- SNPHWE(100, 8, 50);
	print(p_values)
	print(format(p_values,scientific = FALSE))
		
	
	pchiq(c(1.2093726379440666), df= 1, lower.tail = FALSE)
	
	if(8.242989e-32 > 0)
	{
		print("inside")
}


######################Allels Test#################################
r = 133
s = 109
n = r + s
pcase = ((2*57) + 69)/(2*r)
pcontrol = ((2*33) + 56)/(2*s)
p = (183 + 122 )/(2*242)

ZL = (2*sqrt(r * s)*(pcase - pcontrol)) / sqrt(2*n*p*(1-p))
ZL2 = ZL * ZL
####################################################################
######################Codominant Test#################################
### x2 = sum ((O-E)^2 / E)
x2 = (7-((27*133)/242))^2/(((27*133)/242)) + (69-((125*133)/242))^2/(((125*133)/242)) + (57-((90*133)/242))^2/(((90*133)/242)) + (20-((27*109)/242))^2/(((27*109)/242)) + (56-((125*109)/242))^2/(((125*109)/242)) + (33-((90*109)/242))^2/(((90*109)/242))
####################################################################
######################Trend Test#################################
### ZT
n1 = 125
n2 = 90
ZT = (2*sqrt(r * s)*(pcase - pcontrol)) / sqrt((4*n2) + n1 - (4*n*(p^2)))
ZT2 = ZT * ZT