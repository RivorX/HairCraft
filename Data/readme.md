## Konfiguracja bazy danych i rozwi�zywanie problem�w

Podczas uruchamiania projektu mog� pojawi� si� problemy z po��czeniem do bazy danych. Oto kroki, kt�re nale�y podj��, aby poprawnie skonfigurowa� projekt:
1. Zainstaluj potrzebne pakiety Nu
Aby zainstalowa� pakiety Nu, otw�rz Package Manager Console w Visual Studio i wykonaj poni�sze polecenia:

Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
Install-Package Microsoft.AspNetCore.Authentication.Cookies



2. Uruchom migracje Entity Framework
Je�eli baza danych nie istnieje, musisz j� utworzy� przy pomocy migracji. Aby to zrobi�:

Otw�rz Package Manager Console w Visual Studio.
Wykonaj poni�sze polecenia:
		Update-Database
