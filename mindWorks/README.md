Hei,

selline väikene lihtne ülesanne, veebimaailmaga pole üldse seotud. Üle 1-2 tunni selle lahendamiseks ei tohiks minna. Samas pole see puhtalt akadeemiline, päris mitmes rakenduses on midagi sarnast vaja läinud.

Meil on suunatud graaf, mille tippusid identifitseerivad suvalised täisarvud. Graaf on antud seostepaaridena. Näiteks 1,2 & 1,3 & 2,4 & 3,4 & 4,5 & 4,6 mis graafiliselt teeks
graph.png
Your mission, should you choose to accept it on toota programm, mis leiab kahe etteantud tipu vahel kõik võimalikud teed. Graafitsükleid käsitle kui stopp-punkte millest edasi ära otsi. Näiteks ülaloleva graafiga otsides kõiki teid tippude 1 ja 4 vahel peab klassi meetod tagastama 1,2,4 ja 1,3,4.

Realiseeri lahendus klassina, milles on meetod:

public static List<List<int>> ConnectingPaths(List<Tuple<int, int>> graph, int node1, int node2) { ... }

Täiesti piisab brute force lahendusest, meid huvitab ainult see klass, st. igasugune i/o pool pole oluline. Mõistlik veakäsitlus oleks tore.
