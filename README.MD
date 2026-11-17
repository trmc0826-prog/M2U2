*Driftsmonitor — Ukesoppgaver*

*Målinger — Oppgave 0 og 1*

I Oppgave 0 testet jeg forskjellen mellom en vanlig List<T> og en liste som bare beholder de siste 10 000 elementene.

*Oppgave 0a — Minnebruk*
Jeg la til 1 000 000 loggmeldinger i en vanlig liste og målte minnebruken til 76 MB.
En vanlig liste fortsetter å vokse når nye elementer legges til. Hvis programmet kjører lenge nok uten å fjerne noe, kan det til slutt gå tom for tilgjengelig minne.

*Oppgave 0b og 0c — Begrenset liste*
testet deretter en liste som beholdt maksimalt 10 000 elementer.
Med 200 000 iterasjoner tok dette 532 ms.
Grunnen til at denne løsningen kan være kostbar, er RemoveAt(0). Hver gang det første elementet fjernes, må de 9 999 resterende elementene flyttes én plass til venstre.
I testen skjedde dette 190 000 ganger.

*Oppgave 1 — RingBuffer*
Jeg testet samme antall iterasjoner med min egen RingBuffer<T>.
Resultatet var 12 ms, sammenlignet med 532 ms for listen.
Ringbufferen er raskere i denne testen fordi den overskriver det eldste elementet uten å flytte de andre elementene.
Målingene er fra én kjøring av hver løsning, så resultatene kan variere.

*Refleksjon — Oppgave 7* *Hvorfor er RingBuffer<T> bedre enn en RingBuffer som lagrer object?*
Med RingBuffer<T> kan vi bestemme hvilken datatype bufferet skal inneholde. Dette gjør koden tryggere og enklere å bruke.
Hvis vi hadde brukt object, måtte vi ha konvertert verdiene tilbake til riktig datatype for å få tilgang til egenskapene deres.
I Oppgave 4 ville dette blant annet gjort det vanskeligere å bruke entry.Time og entry.Message direkte.
Vi kunne også risikert å legge inn feil type objekt i bufferet, noe som kunne ført til feil når vi forsøkte å hente ut loggmeldingene.

**2. Hva ville skjedd hvis Oldest returnerte default(T) på et tomt buffer?*
Hvis bufferet inneholdt int, ville default(T) returnert 0.
Problemet er at 0 også kan være en gyldig verdi. Vi ville derfor ikke nødvendigvis visst om bufferet var tomt eller faktisk inneholdt tallet 0.
For RingBuffer<LogEntry> ville default(T) normalt returnert null, siden LogEntry er en referansetype.
Hvis vi deretter forsøkte å bruke egenskapene til objektet, kunne vi fått en NullReferenceException.
Derfor er det bedre at Oldest kaster et unntak når bufferet er tomt.

*3. Hvorfor fungerer new T[capacity], men ikke new T()?*
new T[capacity] fungerer fordi vi oppretter et array som kan lagre elementer av typen T. Vi trenger ikke å opprette selve objektene.
new T() er annerledes fordi vi vil opprette objekt av typen T.
Siden T kan være mange forskjellige datatyper, vet ikke C# om typen har en konstruktør som kan brukes uten argumenter.
Ved å bruke where T : new() kan vi kreve at typen har en offentlig parameterløs konstruktør.

*4. Hvor mange linjer måtte endres i RingBuffer<T> for den tredje modellen?*
Ingen.
Vi kan bruke den samme implementasjonen for LogEntry, SensorReading og RequestLog uten å endre RingBuffer<T>.
Beauty of generics!

*5. Hva ville skjedd hvis Core hadde en prosjektreferanse til modellene?*
Hvis Driftsmonitor.Core var avhengig av modellene i Driftsmonitor.App, ville datastrukturen blitt tettere knyttet til akkurat denne applikasjonen.
Da ville det vært vanskeligere å gjenbruke ringbufferen i andre prosjekter.
Ved å holde Core uavhengig kan vi bruke den samme datastrukturen med forskjellige datatyper eventuelt med forskjellige applikasjoner.

