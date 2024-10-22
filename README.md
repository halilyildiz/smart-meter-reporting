# Akıllı Sayaç Verisi İşleme ve Raporlama Sistemi

Bu proje, akıllı sayaçlardan gelen verilerin toplanması, işlenmesi ve raporlanması amacıyla geliştirilmiş bir mikroservis tabanlı uygulamadır. Uygulama, .NET Core ile geliştirilmiş olup, RabbitMQ mesajlaşma kuyruğu ile asenkron raporlama işlemlerini gerçekleştirmektedir.

Proje üç ana mikroservisten oluşmaktadır:

1. Meter Microservice: Sayaç verilerini toplar, işler ve veritabanına kaydeder.
2. Report Microservice: Sayaç verilerine dayanarak raporlar oluşturur ve asenkron olarak işleyip saklar.
3. Web API: Kullanıcıların sayaç ve rapor verilerini görüntüleyebileceği bir arayüz sağlar.

### Kullanılan Teknolojiler

- .NET Core 8.0
- RabbitMQ
- MsSQL
- xUnit
- Docker

## Kurulum Adımları

### Gereksinimler

- .NET SDK 8.0
- Docker
- RabbitMQ
- MsSQL

## Projenin Çalıştırılması

### 1. Projeyi klonlayın:

    git clone https://github.com//halilyildiz/smart-meter-reporting.git

### 2. RabbitMQ'nun yapılandırılması:

Projeyi başlatmadan önce bir veritabanı ve RabbitMQ instance'larının çalıştığından emin olun. Eğer Docker kullanıyorsanız, aşağıdaki komutla bir RabbitMQ container'ı çalıştırabilirsiniz:

    docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
appsettings.json dosyasında RabbitMQ bağlantı bilgilerinizi ekleyin

    "RabbitMQ": {
	    "Host": "localhost",
	    "Port": 5672
    }

### 3. Veritabanı bağlantısını yapılandırılması:

Servislerdeki appsettings.json dosyasında veritabanı bağlantı bilgilerinizi güncelleyin.

    "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MeterDb;User Id=sa;Password=your_password;"
    }

### 4. Migration'ları çalıştırın:

Veritabanı için migration'ları uygulayın.

```
cd src  
dotnet ef database update --project MeterService
dotnet ef database update --project ReportService
```


### 5. Projeyi çalıştırın:

```
    cd src
    dotnet run --project MeterService
    dotnet run --project ReportService
    dotnet run --project WebUI
```

### Test

Projede yer alan unit testleri çalıştırmak için aşağıdaki komutu kullanabilirsiniz:

    dotnet test

### API Dokümantasyonu

Projede yer alan servislerin API dokümantasyonuna erişmek için Swagger kullanabilirsiniz. Proje çalıştırıldığında, Swagger arayüzüne tarayıcınızdan şu URL ile erişebilirsiniz:

    http://localhost:{port}/swagger
