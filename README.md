# Микросервисное приложение для создания и получения заказов, с сервисом отправки уведомлений о созданном заказе

## Архитектура
Приложение состоит из трёх .sln файлов, структура каждого микросервиса следует принципам чистой архитектуры:
- OrderService.sln - Микросервис, обрабатывающий заказы, их операции с БД и бизнес-логику:
  - Domain layer: предоставляет сущность домена 'Order'
  - Application layer: содержит интерфейсы приложения, ожидаемые ошибки, обработчики PipelineBehavior и логику связанную с CQRS
  - Infrastructure layer: отвечает за работу с БД и внешними сервисами (RabbitMQ)
     - В качестве БД использовался Ms SQL Server, а не in-memory версия, ведь в любом случае приложение будет использовать Docker, а запустить на нём БД не сложнее, чем настроить in-memory версию без него
  - Presentation layer: собирает конфигурацию всего приложения и обеспечивает связь с клиентом путём API и сопутствующими элементами
  - Каждый слой содержит свой собственный файл DependencyInjection с регистрацией и настройкой всех связанных с ним сервисов.
  - Также OrderService имеет отдельную папку с интеграционными тестами, проверяющими весь путь запроса: от его получения, до сохранения в БД и отправки уведомления в RabbitMQ. Благодаря Docker и библиотеке TestContainers тесты запускаются независимо, но в полноценной рабочей среде
      - Были созданы интеграционные тесты, а не юнит, потому как операции обработки заказов подразумевают под собой взаимодействие с БД и отправку события взаимодействия с заказом, что противоречит подходу юнит-тестирования. Ну и с настроенной базовой конфигурацией интеграционных тестов и Docker их написание и запуск становятся не сложнее юнит 
  - В корне микросервиса находится Dockerfile

- NotificationService.sln - Микросервис, отвечающий за обработку событий о создании заказов и последующую отправку уведомлений об этом:
  - Application layer: содержит логику обработки событий RabbitMQ
  - Infrastructure layer: отвечает за конфигурацию RabbitMQ
  - Host layer: собирает конфигурацию всего приложения
  - Каждый слой содержит свой собственный файл DependencyInjection с регистрацией и настройкой всех связанных с ним сервисов
  - В корне микросервиса находится Dockerfile

- Shared.Contracts.sln - Общая библиотека, содержащая описание событий для обмена сообщений между микросервисами.
  
- В корне приложения находится Docker-compose.yaml и связанные с ним для конфигурации .env файлы
    - Все строки подключения, переменные окружения, логины и пароли не были спрятаны и вынесены в Git для удобства запуска репозитория и его проверки

### Использованные технологии
- ASP.Net Core (Web API framework)
- MediatR (CQRS + pipeline behaviors)
- MassTransit + RabbitMQ (Message broker)
- Entity framework Core (ORM)
- MS SQL Server (DB)
- Xunit + TestContainers (Itegration testing)
- ErrorOr (Error handling)
- Fluent Validation (Model validation)
- Serilog (Logging)
- Mapperly (Mapping)
- Docker Desktop

## Инструкция по запуску проекта
### Локальный запуск микросервисов, работающих с заказом, и запуск на Docker внешних сервисов (MsSQL Server и RabbitMQ)
Для этого нужно полностью клонировать репозиторий, затем, находясь в корневой папке приложения, запустить консольную команду:
``` Shell
docker-compose -f Docker-compose.development.yaml up -d
```
Она запустит Docker-compose.development.yaml файл, в котором находятся только конфигурации внешних сервисов.
После использования команды желательно немного подождать, чтобы внешние сервисы успели раскрутиться и у внутренних не возникло ошибки при подключении к недонастроенным сервисам.

Дальше запускаем в компиляторе OrderService.sln и NotificationService.sln, смотрим в консоли OrderService.sln, на каком порте запустилось приложение:

![image](https://github.com/user-attachments/assets/f3d06d1a-7c66-494d-8750-2f7edfc1fd22)

Отправляем к нему запросы напрямую или используем SwaggerUI по ссылке: https://localhost:[app.port]/swagger/index.html
И смотрим результаты в том же SwaggerUI или консолях OrderService.sln и NotificationService.sln

### Запуск всего приложения на Docker
Для этого нужно иметь только:
- Docker-compose.yaml

И связанные с ним .env файлы переменных окружения
- apiconfig.env
- rabbitmqserverconfig.env
- rabbitmquserconfig.env
- sapassword.env
- sqlconfig.env

Также для корректной работы https-подключения ASP.Net Core приложения, нужно настроить сертификат, который используется при разработке приложений.
Если сертификат уже имеется и мы знаем от него пароль, то просто в файле apiconfig.env вставляем этот пароль в переменную:
```d
ASPNETCORE_Kestrel__Certificates__Default__Password=[YourPass]
```
Если пароль мы не помним, то парой консольных команд его можно создать:

Удаляем текущие сертификаты:
```shell
dotnet dev-certs https --clean
```

Создаём новый сертификат и вставляем пароль, который уже используется в apiconfig.env: ASPNETCOREKestrelCertificatesDefault
```shell
dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx"  -p ASPNETCOREKestrelCertificatesDefault
```

Указываем, что этому сертификату можно доверять
```shell
dotnet dev-certs https --trust
```

Ну и чтобы запустить приложение, находясь в корневой папке, запускаем консольную команду:
``` Shell
docker-compose -f Docker-compose.yaml up -d
```

Запросы можно отправлять напрямую:
- http://localhost:5000/orders
- https://localhost:5001/orders

- Или используя SwaggerUI по ссылке: 
https://localhost:5001/swagger/index.html

Cмотрим результаты в том же SwaggerUI или в логах контейнеров, запущенных на Docker

## Примеры API-запросов и результаты
### Endpoints
![Endpoints](https://github.com/user-attachments/assets/e1ad372b-03a0-4d6a-a1a7-f8956e7d05fd)

### Schemas
![Schemas](https://github.com/user-attachments/assets/4309ec32-2d16-4ae0-b099-937cc8a96be1)

### Отравка Post запроса
- Данные для создания валидного Order
  ![image](https://github.com/user-attachments/assets/5c3db64e-d6d6-4ac1-81ca-8f262ecc316f)
- Успешное создание
  ![image](https://github.com/user-attachments/assets/5872ee70-4adb-4cf1-88e3-0c8e19220aaa)
- Получение события RabitMQ
  ![image](https://github.com/user-attachments/assets/11ceefa0-3291-4755-9b02-2b8240efd228)
- Логи в консолях микросервисов (NotificationService слева, OrderService справа) 
  ![image](https://github.com/user-attachments/assets/9b28c642-4a9c-4131-836f-337a9f5af8cb)
- Заполненная БД
  ![image](https://github.com/user-attachments/assets/6536eb73-0011-4955-a68b-7c4a49b0c319)
- Данные для создания невалидного Order
  ![image](https://github.com/user-attachments/assets/89a38903-64d3-48f4-821e-da7858c2d807)
- Ошибка создания
  ![image](https://github.com/user-attachments/assets/285abeab-952d-461f-aea4-63029f79fc8e)

### Отправка Get запроса
- Данные для получение существующего Order
  ![image](https://github.com/user-attachments/assets/716eca1d-e72f-4ee3-9df9-88219f402762)
- Успешное получение
  ![image](https://github.com/user-attachments/assets/4e4855a3-c010-4fbb-b732-8f8c48295688)
- Данные для получения несуществующего Order
  ![image](https://github.com/user-attachments/assets/e3d95fe2-4ec6-4298-934a-90f8581a8e51)
- Ошибка получения
  ![image](https://github.com/user-attachments/assets/3020c5fb-f863-4e0d-b3a0-cfc0c757f53e)
