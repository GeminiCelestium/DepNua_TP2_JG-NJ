@allowed([
  'CanadaCentral'
  'CanadaEast'
])
param location string
param spName string = 'TP2-Script'
@secure()
param dbPassword string 
param dbUser string = 'User123'

module appServices './Modules/appServices.bicep' = {
  name: 'appServices'
  params: {
    location: location
    spName: spName
  }
}

module sqlServer './Modules/sqlServer.bicep' = {
  name: 'sqlServer'
  params: {
    location: location
    spName: spName
    dbPassword: dbPassword
    dbUser: dbUser
  }
}

module storageAccount './Modules/storageAccount.bicep' = {
  name: 'storageAccount'
  params: {
    location: location
    spName: spName
  }
}
