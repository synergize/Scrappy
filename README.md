# Scrappy
Service that uses Selenium to scrape [MTGGoldFish](https://www.mtggoldfish.com/movers/paper/modern) every 45 minutes. Storing data in JSON Documents only if the data has changed. Data is consumed by [KickAssMTGBot](https://github.com/synergize/KickAssMTGBot) every 30 minutes.

## Feedback
I'm always open for feedback on feature implementation or code structure. Please don't hesitate to reach out and let me know! 

## Installation

Currently not being distributed.

## Third-Party Implementation
- [Scraping MTGGoldfish Movers and Shakers](https://www.mtggoldfish.com/movers/paper/modern)
- [.NET Core 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1)
- [Selenium](https://selenium.dev/)
- [VTFileManagement Library](https://github.com/synergize/VTFileSystemManagement)

## Current To-Add List
1. Implement logging system. 
1. Update data model to support configuration of channel on format-by-format basis.
1. Implement a Command Line Interface
1. Implement web service page built in [Angular](https://angular.io/) to monitor and maintain the scraping service. 

## Idle Display
<img align="left" width="353" height="229" src="https://i.imgur.com/GNJZnR7.png">


<p align="center">
  <img width="633" height="75" src="https://i.imgur.com/VvFQo6q.png">
  
  <img width="633" height="60" src="https://i.imgur.com/5f2hMna.png">
</p>


Enjoy!
