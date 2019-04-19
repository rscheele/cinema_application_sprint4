Het project kan met de .sln snelkoppeling in Visual Studio geopend worden. De versie die ik gebruikt heb is versie Microsoft Visual Studio Enterprise 2017 15.7.2

Om het project op te kunnen starten moeten eerst alle packages opnieuw geïmporteerd worden.

Daarna moet er in de package manager console 'update-database' als command uitgevoerd worden.
Deze zal de Microsoft Identity database seeden. Alle overige data wordt bij het opstarten van de applicatie geseed door middel van een databaseinitializer. De eerste keer dat de applicatie runt kan het tot 5(!) minuten duren voordat de database geseed is, afhankeling van de processor waarop de applicatie draait. Dit wordt veroorzaakt door de grote volume data die in de database wordt gestopt.

De database bevat op dit moment alleen data voor bioscoop Breda. Er staan films in de database tot en met 26 juni. In de file Sprint4 > Domain > Concrete > Cinemainitializer kunnen de datums van Show objecten aangepast worden.

Het project runt nu in de debugger.