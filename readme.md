<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h3 align="center">Social Collaboration & Knowledge Management</h3>

  <p align="center">
    Demoprojekt zu dem Thema Quellcodedokumentation
    <br />
    <a href="https://github.com/othneildrew/Best-README-Template"><strong>Hier gehts zum Blogpost »</strong></a>
    <br />
</div>


<!-- ABOUT THE PROJECT -->
## Über das Projekt
Das Projekt wurde im Rahmen der Lehrveranstaltung „Social Collaboration & Knowledge Management“ an der Hochschule Karlsruhe entwickelt. Ziel ist es, Wissen in Entwicklungsabteilungen besser zugänglich und nutzbar zu machen. Der in diesem Repository enthaltene Quellcode bildet eine Demoanwendung ab, die ein Tool prototypisch demonstriert, mit dem sich Lücken in der Quellcodedokumentation identifizieren und anschließend ergänzen lassen.



### Built With

* <img src="https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white" />  
* <img src="https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white" /> 
*  <img src="https://img.shields.io/badge/Google%20Gemini-8E75B2?style=for-the-badge&logo=googlegemini&logoColor=white" /> 



<!-- GETTING STARTED -->
## Getting Started

### Voraussetzungen
Vorraussetzung ist eine verfügbare MsSQL Datenbank. Für die lokale Entwicklung kann eine lokale Entwicklerinstanz verwendet werden. Zum Download geht es hier: [MsSQL Download](https://www.microsoft.com/de-de/sql-server/sql-server-downloads).

Alternativ kann die Datenbank in einem Docker-Container gehostet werden. Dazu einfach das entsprechende [Image](https://hub.docker.com/r/microsoft/mssql-server) pullen. Eine detailierte Anleitung gibt es dazu hier: [Quickstart: Run SQL Server Linux container images with Docker](https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver17&tabs=cli&pivots=cs1-bash).

### Installation

1. Klone das Projekt in Visual Studio
   ```sh
   git clone https://github.com/Wiesele/Social-Collab.git
   ```
2. Kopiere die Datei ```AppSettings.json``` mit dem Namen ```AppSettings.Development.json``` 
3. Konfigurieren der Umgebung in ```AppSettings.Development.json```
   ```json
   {
        "ConnectionStrings": {
            "debug.Db": "[Connection String zu Datenbank]"
        },
        "Git": {
            "LocalFolder": "[Path for Git-Repositories]"
        }
    }

   ```
4. Erstellen und aktualisieren der Datenbank. Eine detailierte Anleitung gibt es hier: [Migrations Overview](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) 
   ```sh
   dotnet ef database update
   ```
5. Das Projekt kann jetzt in VisualStudio gestartet werden.
