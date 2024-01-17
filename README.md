# CurrencyConverterWeb
#CurrencyConverter
#Prerequisites
#Install .NET Core 6.0 SDK.
#Getting Started:
#Clone or download the source code from the repository.
#Open a terminal or command prompt.
#Navigate to the project directory.
#Run dotnet restore to install dependencies.
#Running the Application
#In the project directory, execute dotnet run.
#The API will start on a specified local port (e.g., http://localhost:5000).
#Environment Variables:
#Set exchange rate overrides (optional): set USD_TO_INR_RATE=74.00 (Windows) or export USD_TO_INR_RATE=74.00 (Linux/Mac).
#Using the API Endpoints
#Convert Currency:
#Endpoint: GET /convert
#Parameters: sourceCurrency, targetCurrency, amount
#Example Request: /convert?sourceCurrency=USD&targetCurrency=INR&amount=100
#Response: JSON containing converted amount and exchange rate.
