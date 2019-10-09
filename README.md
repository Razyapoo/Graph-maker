# Graph maker

## 1.	Stručné zadání (anotace)


**V tomto programu řešíme grafové algoritmy.**

Program umí několik algoritmů: 
-	hledání nejkratší cesty 
-	hledání minimální kostry 
-	hledání komponent souvislosti
-	topologické třídění grafu
Pro fungování jednotlivých algoritmů použijeme několik pomocných algoritmů, např. DFS.

## 2.	Přesný popis programu 

**Poprvé musíme nakreslit graf, pomocí tlačítek ***vrchol*** a ***hrana***. Program podporuje i orientované grafy.**

1.	Hledání nejkratší cesty:
Pro hledání nejkratší cesty musíme ukázat vrcholy ***start*** a ***stop***. Pak ukázat ohodnocení hrany a její směr. V tabulce 1. se vypíše hrana, její směr a váha. V tabulce 2. se vypíše směr cesty a její délka.
Algoritmus hledá nejkratší cestu v orientovaných a neorientovaných grafech (používá se trochu opravený Dijkstrův algoritmus).
2.	Minimální kostra grafu: 
Pro hledání minimální kostry musíme nakreslit graf a stisknout tlačítko minimální kostry. V tabulce 2. se vypíší minimální kostry.
Použijeme na to Kruskalův algoritmus. Tedy třídíme hrany podle váh. 
3.	Komponenta souvislosti:
Pro hledání komponenty souvislosti musíme nakreslit graf a stisknout tlačítko komponenta souvislosti. V tabulce 2. se vypíší všechny komponenty souvislosti.
Pro hledání komponenty souvislosti použijeme DFS, tak, že vytvoříme pole komponent a do každého prvku toho pole uložíme vrcholy, které do té komponenty patří. Nakonec vypíšeme obsah výsledného pole.
4.	Topologické třídění grafu:
Pro hledání topologického třídění grafu musíme nakreslit DAG graf. Na to použijeme boolean DFS. Výslednek DFS vypíšeme v obraceném pořadí.


**Graf můžeme uložit jako obrázek.**
