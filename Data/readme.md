## Konfiguracja bazy danych i rozwi¹zywanie problemów

Podczas uruchamiania projektu mog¹ pojawiæ siê problemy z po³¹czeniem do bazy danych. Oto kroki, które nale¿y podj¹æ, aby poprawnie skonfigurowaæ projekt:
1. Zainstaluj potrzebne pakiety Nu
Aby zainstalowaæ pakiety Nu, otwórz Package Manager Console w Visual Studio i wykonaj poni¿sze polecenia:

Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
Install-Package Microsoft.AspNetCore.Authentication.Cookies



2. Uruchom migracje Entity Framework
Je¿eli baza danych nie istnieje, musisz j¹ utworzyæ przy pomocy migracji. Aby to zrobiæ:

Otwórz Package Manager Console w Visual Studio.
Wykonaj poni¿sze polecenia:
		Update-Database
