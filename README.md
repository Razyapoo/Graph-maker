# Graph maker

1.	Stručné zadání (anotace)
==============================================
**Program řeší grafové algoritmy. Pomáhá vyřešit několik lehkých přikladu z grafy.**
Program umí několik algoritmu: 
-	hledání nejkratší cesty, 
-	minimální kostry, 
-	komponent souvislosti, 
-	topologické třídění grafu.
Pro fungování těch algoritmu použiju několik pomocných algoritmu, např. DFS (pomocí barvení vrcholu)

2.	Přesný popis programu 
==============================================
Poprvé musíme nakreslit graf, pomocí tlačítek vrchol a hrana. Pozor na to, že můžeme nakreslit orientovaný graf. 
----------------------------------------------
1.	Hledání nejkratší cesty:
Pro hledání nejkratší cesty musíme ukázat vrcholy start a stop. Pak ukázat ohodnoceni hrany a její směr.  Pak v tabulce 1. se vypíše hrana, její směr a hmotnost. V tabulce 2. se vypíše směr cesty a její délka.
Algoritmus hledá nejkratší cestu v orientovaných a neorientovaných grafech. Na to použiju trochu opravený Dijkstrův algoritmus.
2.	Minimální kostra: 
Pro hledání minimální kostry musíte nakreslit graf a stisknout tlačítko minimální kostry. Pak v tabulce 2. se vypíší minimální kostry.
Použiju na to Kruskalův algoritmus. Tedy třídím hrany podle hmotnosti. 
3.	Komponenta souvislosti:
Pro hledání komponenty souvislosti musíte nakreslit graf a stisknout tlačítko komponenta souvislosti. Pak v tabulce 2. se vypíší všechny komponenty souvislosti.
Pro hledání komponenty souvislosti použiju DFS, tak, že dělám pole komponent a do každého prvku toho pole ukládám vrcholy, které do té komponenty patří. Pak vypisuju obsah toho pole.
4.	Topologické třídění grafu:
Pro hledání topologického třídění grafu musíte nakreslit DAG graf. Na to použiju boolean DFS a pak vypíšu v obraceném pořadí.


### Graf můžeme uložit jako obrázek.
