# BOP3000-Bacheloroppgave-Element-Logic

A webshop integration for eManager by Element Logic.

## Problem

Element Logic often appear at events like conventions etc. where they have a stand show of their product:  
eManager, which is their integration for AutoStore, for which they are currently the leading distributer of.  
At these conventions they bring a small demo of AutoStore, and eManager to show of their product/ services.  
In addition to this though, they would like to have a webshop to show customers and example of a finished  
full integration of their product. This would make showing the whole process easier, and would serve as a  
great example for potential customers visitng Element Logic's stand.

## Technologies

The current solution uses a combination of the following technologies to implement the webshop:
- Blazor
- ASP.NET C# (.NET 6)
- Bootstrap 5 css
- Microsoft SQL database
- eManager REST API

> We chose these technologies specifically to resemble the technologies Element Logic use themselves  
> and are familiar with.

## Installation
Change the default connection string in appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "CONNECTION STRING GOES HERE"
  }
}
```
> The connection string can be exported from the Microsoft SQL database with `SQL Server Management Studio`  

Now build the application with .NET 6
