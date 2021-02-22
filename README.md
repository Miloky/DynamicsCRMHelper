# Dynamics Helper



## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

* [.NET Core SDK 3](https://dotnet.microsoft.com/download/dotnet-core/3.0)

### Setup
Follow these steps to get your development environment set up:
  1.  [Create App-Registration in Azure Active Directory](https://docs.microsoft.com/en-us/powerapps/developer/data-platform/walkthrough-register-app-azure-active-directory)
  2. [Create an application user in Dynamics365](https://docs.microsoft.com/en-us/powerapps/developer/data-platform/authenticate-oauth)
  3.  Update ```appsettings.json``` with your ```client_id```, ```client_secret``` and ```tenant_id```
        ```
        "SettingsName": "OrganizationName",
        "DynamicsSettings": {
            "OrganizationName": {
                "Resource": "https://[organization].crm4.dynamics.com/",
                "ClientId": "client_id",
                "ClientSecret": "client_secrect",
                "Tenant": "tenant_id",
                "Authority": "https://login.microsoftonline.com/[tenant_id]"
            }
        }
        ```
    

  1. Clone the repository
  2. At the root directory, restore required packages by running:
      ```
     dotnet restore
     ```
  3. Next, build the solution by running:
     ```
     dotnet build
     ```
  5. Next, within the `\DynamicsHelper` directory, launch the project by running:
     ```
	 dotnet run
	 ```
  
## Technologies
* .NET Core 3
* ASP.NET Core 3


## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/miloky/DynamicsHelper/blob/master/LICENSE.md) file for details.
