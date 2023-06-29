# NBP

Aplikacja pobiera aktualne kursy wymiany walut z api NBP.

Można odpalić poprzez docker-compose.

Wykorzystuje Asp.Net Core MVC oraz baze MSSQL.

Aplikajca sprawdza i aktualizuje dane co minute (w realnej aplikacji wystarczy co parę godzin) lub na żadanie użytkownikowa poprzez kliknięcie linku który znajduję się na tabelą kursów.
