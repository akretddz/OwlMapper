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
	  │	 ├─ Shared
	  │  │  ├─ Api
	  │  │  ├─ Application  
	  │  │  ├─ Domain 
	  │  │  └─ Infrastructure	  
	  │  │	  
	  │	 ├─ Identity
	  │  │  ├─ Api
	  │  │  ├─ Application  
	  │  │  ├─ Domain 
	  │  │  └─ Infrastructure	
	  │  │
      │  ├─ Management
	  │  │  ├─ Api
	  │  │  ├─ Application  
	  │  │  ├─ Domain 
	  │  │  └─ Infrastructure	
	  │  │	  
      │  └─ UserProfile
	  │     ├─ Api
	  │     ├─ Application  
	  │     ├─ Domain 
	  │     └─ Infrastructure	
	  │  	  
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
   ├─ Account.UnitTests/
   │   ├─ Shared.UnitTests   
   │   ├─ Identity.UnitTests
   │   ├─ Management.UnitTests
   │   └─ UserProfile.UnitTests
   │
   ├─ Places.UnitTests/
   │
   ├─ OwlMapper.IntegrationTests/
   │
   ├─ Notifications.IntegrationTests/
   │
   ├─ Variants.IntegrationTests/
   │
   ├─ Account.IntegrationTests/
   │
   └─ OwlMapper.ArchitectureTests/
