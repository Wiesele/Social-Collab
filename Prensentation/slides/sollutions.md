# Bestehende L&ouml;sungen

| Quellcode nahe | Quellcode ferne |
| -------------- | --------------- |
| Direkte Integration in IDE | Externes Tool  |
| Dokumentation gespeichert in Quellcode | Dokumentation in Datenbank |
| Eignet sich f&uuml;r Entwicklerdoku | Eignet sich f&uuml;r ausf&uuml;hrliche Doku (Guides, How-To) |

## Wie erstellt Microsoft Dokus?
- [DbContext.SaveChanges()](https://learn.microsoft.com/en-us/ef/core/saving/basic)
- [XmlDoc](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/)

## Weiter Beispiele

```php
/**
 * Listet alle direkten Unterordner eines Verzeichnisses auf (nicht rekursiv).
 *
 * @param string $path Absoluter oder relativer Pfad zum Verzeichnis
 * @return array Liste der Ordnerpfade
 * @throws InvalidArgumentException Wenn der Pfad kein Verzeichnis ist
 */
function listDirectories(string $path): array
{
   ...
}
```

```rust
/// Gives a friendly hello!
///
/// Says "Hello, [name](Person::name)" to the `Person` it is called on.
pub fn hello(&self) {
    println!("Hello, {}!", self.name);
}
```