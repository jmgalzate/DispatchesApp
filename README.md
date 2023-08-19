# Introduction:
Fiory App is an application created for improve a process that Fiory warehouse executed before manually by Excel file.

# External Connection:
The application has a "Library" for getting connections with the Contapyme API

# Relevant information:
- There is a CLI project because the first version to this application was a Command application.
- The certificate is a Self Certificate
- It was build with MAUI and Blazor for multiplatform application.

# Additional folders:
- You need to add the appsettings.json file in the C:/programdata/FioryApp/Masterfiles/appsettings.json
- The application will create another folder for exports on C:/Users/{username}/documents/FioryApp/Exports/

# appsettings.json
{
    "Server": "URL",
    "Username": "user@mail.com",
    "Password": "password encrypted",
    "MachineID": "Whatever",
    "iapp": "ID provided by Contapyme",
    "itdoper": "Order Type"
}