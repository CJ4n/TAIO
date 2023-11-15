## Wymagania

W celu skompilowania kodu do pliku .exe należy posiadać `.NET SDK` w wersji co najmniej 7.0.
Aby sprawdzić czy system posiada wymagane narzędzie wystarczy wykonać komendę `dotnet --version`. W razie braku wymaganego SDK można zainstalować go komendą  `winget install Microsoft.DotNet.SDK.7` lub skorzystać z pomocy oficjalnej dokumentacji [Microsoft](https://learn.microsoft.com/pl-pl/dotnet/core/install/windows?tabs=net80)

## Kompilacja do pliku .exe

1. Przejdź do folderu **TAIO** w którym znajdują się *algorithms*, *benchmarks*, *graphs* itd.
2. Wywołaj komendę `dotnet build`
3. Po zakończonej kompilacji plik .exe powinien zjadować się w folderze *bin\Debug\netX.Y* (gdzie X.Y oznacza wersję dotneta) pod nazwą **TAIO.exe**

## Użytkowanie

Program umożliwia:
- obliczenie rozmiaru grafu
- znalezienie maksymalnej kliki grafu
- znalezienie maksymalnego podgrafu
- znalezienie odległości dwóch grafów

### Obliczanie rozmiaru grafu

**Wywołanie**: 

`TAIO.exe size <ścieżka do pliku>`

**Argumenty**:
- *ścieżka do pliku* - ścieżka do pliku .txt zawierającego grafy

**Zastosowanie**:
  
Program oblicza wielkość dla każdego grafu zapisanego w pliku, którego ścieżkę podano jako argument.

### Znalezienie maksymalnej kliki grafu

**Wywołania**:

`TAIO.exe clique <ścieżka do pliku>`

`TAIO.exe clique <ścieżka do pliku> isExact=true`

**Argumenty**:
- *ścieżka do pliku* - ścieżka do pliku .txt zawierającego grafy

**Opcje**:
- *isExact* - opcja użycia w programie algorytmu dokładnego do wykonania obliczeń, defaultowo opcja ta ustawiona jest na false (więc bez podania jej program używa algorytmów aproksymacyjnych)

**Zastosowanie**:

Program znajdzie maksymalną klikę dla każdego grafu zapisanego w pliku, którego ścieżkę podano jako argument.

### Znalezienie maksymalnego podgrafu

**Wywołania**:

`TAIO.exe subgraph <ścieżka do pliku>`
`TAIO.exe subgraph <ścieżka do pliku> isExact=true`

**Argumenty**:
- *ścieżka do pliku* - ścieżka do pliku .txt zawierającego grafy

**Opcje**:
- *isExact* - opcja użycia w programie algorytmu dokładnego do wykonania obliczeń, defaultowo opcja ta ustawiona jest na false (więc bez podania jej program używa algorytmów aproksymacyjnych)

**Zastosowanie**:

Program znajduje maksymalny podgraf dla każdej pary grafów podanych w pliku podanych jako argument (tj. jeżeli w pliku zapisane są gragy: G1, G2, G3, G4 obliczone zostanne podgrafy dla G1 z G2 oraz G3 z G4).
W Przypodaku w którym liczba grafów w pliku jest niepzrzysta program zwórci bład informujący o tym.

### Znalezienie odległości dwóch grafów

**Wywołania**:

`TAIO.exe distance <ścieżka do pliku>`
`TAIO.exe distance <ścieżka do pliku> isExact=true`

**Argumenty**:
- *ścieżka do pliku* - ścieżka do pliku .txt zawierającego grafy

**Opcje**:
- *isExact* - opcja użycia w programie algorytmu dokładnego do wykonania obliczeń, defaultowo opcja ta ustawiona jest na false (więc bez podania jej program używa algorytmów aproksymacyjnych)

**Zastosowanie**:

Program znajduje odległość między parami grafów podanych w pliku jako argument (tj. jeżeli w pliku zapisane są gragy: G1, G2, G3, G4 obliczone zostaną odległości G1-G2 oraz G3-G4).
W Przypodaku w którym liczba grafów w pliku jest niepzrzysta program zwórci bład informujący o tym.
