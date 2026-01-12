```
OwlMapper
└─ src/
   ├─ Bootstrapper/
   │
   ├─ ApiGateway/ #Zależy czy będziemy używać jednego na jedną apkę frontendową czy czy jednego dla wszystkich
   │
   ├─ Shared
   │
   ├─ Tools
   │
   └─ Modules/
      ├─ RoutePlanner/
      │  ├─ Api
	  │  ├─ Application  
	  │  ├─ Domain 
	  │  └─ Infrastructure
	  │
      ├─ Timetable/
      │  ├─ Api
	  │  ├─ Application  
	  │  ├─ Domain 
	  │  └─ Infrastructure
	  │
	  ├─ BusStops/
      │  ├─ Api
	  │  ├─ Application  
	  │  ├─ Domain 
	  │  └─ Infrastructure
	  │
	  ├─ Notifications/
	  │	 ├─ Shared
	  │  │  ├─ Api
	  │  │  ├─ Application  
	  │  │  ├─ Domain 
	  │  │  └─ Infrastructure	  
	  │  │
	  │	 ├─ Templates
	  │  │  ├─ Api
	  │  │  ├─ Application  
	  │  │  ├─ Domain 
	  │  │  └─ Infrastructure
	  │  │
      │  ├─ Audience
	  │  │  ├─ Api
	  │  │  ├─ Application  
	  │  │  ├─ Domain 
	  │  │  └─ Infrastructure	
      │  │	  
      │  └─ Delivery
	  │     ├─ Api
	  │     ├─ Application  
	  │     ├─ Domain 
	  │     └─ Infrastructure	  
	  │
	  ├─ Variants/
	  │	 ├─ Shared
	  │  │  ├─ Api
	  │  │  ├─ Application  
	  │  │  ├─ Domain 
	  │  │  └─ Infrastructure	  
	  │  │	  
	  │	 └─ Trips
	  │     ├─ Api
	  │     ├─ Application  
	  │     ├─ Domain 
	  │     └─ Infrastructure	  
      │
	  ├─ Journeys/
      │  ├─ Api
	  │  ├─ Application  
	  │  ├─ Domain 
	  │  └─ Infrastructure
	  │      
	  ├─ Account/
	  │  ├─ Account.csproj/
	  │  │ ├─ Registration # Do użytku dla `Bootstrappera`. Rejestracja zależności DI.
	  │  │ ├─ ModuleConfiguration  
	  │	 ├─ Account.Contracts.csproj
	  │  │ ├─ Events
	  │  │ ├─ Commands
	  │  │ └─ Interfaces
	  │  │  
	  │	 ├─ Identity
	  │  │  ├─ Account.Identity.csproj 
	  │  │	├─ Account.Identity.Contracts.csproj # Opcjonalny w zależności czy będzie niezbędny
	  │  │  └─ Account.Identity.Core.csproj	
	  │  │
      │  ├─ Management
	  │  │  ├─ Account.Management.csproj 
	  │  │	├─ Account.Management.Contracts.csproj # Opcjonalny w zależności czy będzie niezbędny
	  │  │  └─ Account.Management.Core.csproj
	  │  │	  
      │  ├─ UserProfile
	  │  │  ├─ Account.UserProfile.csproj 
	  │  │	├─ Account.UserProfile.Contracts.csproj # Opcjonalny w zależności czy będzie niezbędny
	  │  │  └─ Account.UserProfile.Core.csproj
	  │  ├─ Security
	  │  │  ├─ Account.Security.csproj 
	  │  │	├─ Account.Security.Contracts.csproj # Opcjonalny w zależności czy będzie niezbędny
	  │  │  └─ Account.Security.Core.csproj	
	  │  	  
      └─ Places/
         ├─ Api
	     ├─ Application  
	     ├─ Domain 
	     └─ Infrastructure	  

└─ tests/
   ├─ RoutePlanner.UnitTests/
   │
   ├─ Timetable.UnitTests/
   │
   ├─ BusStops.UnitTests/
   │
   ├─ Notifications.UnitTests/
   │   ├─ Shared.UnitTests
   │   ├─ Templates.UnitTests
   │   ├─ Audience.UnitTests
   │   └─ Delivery.UnitTests
   │
   ├─ Variants.UnitTests/
   │   ├─ Shared.UnitTests
   │   ├─ Trips.UnitTests
   │
   ├─ Journeys.UnitTests/
   │
   ├─ Account/
   │   ├─ Account.Identity.UnitTests.csproj
   │   ├─ Account.Management.UnitTests.csproj
   │   ├─ Account.UserProfile.UnitTests.csproj
   │   ├─ Account.Security.UnitTests.csproj
   │   └─ Account.IntegrationTests.csproj
   │
   ├─ Places.UnitTests/
   │
   ├─ OwlMapper.IntegrationTests/
   │
   ├─ Notifications.IntegrationTests/
   │
   ├─ Variants.IntegrationTests/
   │
   └─ OwlMapper.ArchitectureTests/
```