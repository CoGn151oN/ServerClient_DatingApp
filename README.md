# Server-Client Dating App

Készíts egy kliens-szerver alkalmazást egy társkereső rendszer megvalósítására!
A szerver egyszerre több klienst is tudjon kiszolgálni!
A kliens képes legyen konzolról beolvasni tetszőleges számú utasítást (külön sorokban) és a szervernek továbbítani azokat! Legyen felkészülve arra, hogy a szerver nem csak egysoros, de többsoros választ is küldhet!

 

A szervernek kezelnie kell felhasználókat. Minden felhasználóról tároljuk: usernév, jelszó. A felhasználók adatait egy XML fájl tartalmazza, melyet Neked kell összeállítanod pár user adatával. A szerver induláskor olvassa be az adatokat!

A rendszerben randimeghívások adatait kell beolvasni a dates.xml fájlból. A szerver indulásakor olvasd be ezeket és ehhez hozz létre egy megfelelő osztályt.
Megjegyzés: A status-ban három érték lehet megadva: pending (=függőben), accepted (=elfogadva) és rejected (=elutasítva).

A szervernek támogatni kell az alábbi funkciókat:

LOGIN|<usernév>|<jelszó>: Bejelentkezteti az adott felhasználót.
LOGOUT: Kijelentkezteti az adott klienst a szerverről.
EXIT: A kliens kilép.

A következő funkciók csak bejelentkezés után érhetőek el:

INVITE|<usernév>|<tárgy>: A bejelentkezett felhasználó meghívja randira a megadott usert az adott tárggyal (pl. "vacsora"). Ha a két felhasználó között van még függőben levő meghívás (bármilyen tárggyal), akkor a mostani meghívást eldobjuk és erről egy hibaüzenetet küld a szerver a kliensnek.

ACCEPT|<usernév>: A bejelentkezett felhasználó elfogadja a megadott user még függőben levő meghívását. Ha nincs ilyen meghívás, akkor erről egy hibaüzenetet küld a szerver a kliensnek.

REJECT|<usernév>: A bejelentkezett felhasználó elutasítja a megadott user még függőben levő meghívását. Ha nincs ilyen meghívás, akkor erről egy hibaüzenetet küld a szerver a kliensnek.

INVITATIONS: Listázzuk az összes olyan meghívást (bármilyen status-szal), amely a bejelentkezett felhasználóhoz érkezett (azaz őt hívták meg).
