# Otomobil
Website menjual kursus mengemudi

Stack: C#, .NET, Blazor, Mudblazor, SQL Server

Slideshow: [link](https://www.canva.com/design/DAG2yRC0Aug/M-FxLOY3jEohRGvCPpWoNQ/view?utm_content=DAG2yRC0Aug&utm_campaign=designshare&utm_medium=link2&utm_source=uniquelinks&utlId=hf5884e5ebf)

#### Kelompok 2
- Aditya Pratama Febriono
- Gregorius Ivan Halim
- Kiet Pascal  

## How to Run
1. Set your DB connection string and SMTP server email in AutomotiveApp.WebAPI/appsettings.json
2. Set SMTP server password using `dotnet user-secrets set "EmailSettings:Password" "your-smtp-password"` on the terminal
3. Run `docker-compose up -d` for external tools (SonarQube, Grafana)
4. Run `dotnet run` in AutomotiveApp.WebAPI and AutomotiveApp.BlazorUI
5. Access the website in `http://localhost:5160`
6. Access the backend in `http://localhost:5001` 

#### Access Grafana Logs:
1. Go to `http://localhost:3000`
2. Login with `user=admin` `password=admin`
3. Click left sidebar menu and Go to 'Explore'
4. Click the topleft dropdown then click open Data Source Picker
5. Click Configure New Data Source then click Loki
6. Fill `http://loki:3100` as the Connection URL and click Save & Test
7. Go back to Explore to view the Logs