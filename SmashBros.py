#! usr/bin/env python
#Given an array, give me the index of the array that contains the following value. AARONS

smashBrothersCharacters = ["Jigglypuff","Pikachu","Marth","Duck Hunt","Lucina","Rosalina","Kirby","Luigi","Charizard","Mr. Game and Watch","Pac-Man","Peach","Mario","Link","Fox","Zelda","Greninja","Falco","Roy","Ike"]
counter = 0
derp=len(smashBrothersCharacters)
while (counter < derp): 
    if smashBrothersCharacters[counter] == "Peach":
        print "%d" % counter
    counter = counter + 1
   