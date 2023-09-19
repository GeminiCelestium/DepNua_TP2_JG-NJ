@allowed([
  'CanadaCentral'
  'CanadaEast'
])
param location string
param appNamePrefix string = 'myapp'
param dbAdminPassword string = 'YourAdminPasswordHere'

module appServices './Modules/appServices.bicep' = {
  name: 'appServices'
  params: {
    location: location
    appNamePrefix: appNamePrefix
  }
}

module sqlServer './Modules/sqlServer.bicep' = {
  name: 'sqlServer'
  params: {
    location: location
    appNamePrefix: appNamePrefix
    dbAdminPassword: dbAdminPassword
  }
}

module storageAccount './Modules/storageAccount.bicep' = {
  name: 'storageAccount'
  params: {
    location: location
    appNamePrefix: appNamePrefix
  }
}
