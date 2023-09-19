param location string
param spName string
param dbUser string
@minLength(10)
@maxLength(20)
@secure()
param dbPassword string

param microServices array = [
  {
    name: 'mvc'
    appServicePlanSku: 'S1'
    createDatabase: false
  }
  {
    name: 'postulations'
    appServicePlanSku: 'B1'
    databaseSku: 'Standard'
    createDatabase: true
  }
  {
    name: 'emplois'
    appServicePlanSku: 'F1'
    databaseSku: 'Basic'
    createDatabase: true
  }
  {
    name: 'documents'
    appServicePlanSku: 'F1'
    createDatabase: false
  }
  {
    name: 'favoris'
    appServicePlanSku: 'F1'
    createDatabase: false
  }
]

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: 'srv-${spName}'
  location: location
  properties: {
    administratorLogin: dbUser
    administratorLoginPassword: dbPassword
    version: '12.0'
  }
}

resource sqlDatabases 'Microsoft.Sql/servers/databases@2021-11-01' = [
  for microService in microServices : if (microService.createDatabase) {
    name: 'db-${spName}-${microService.name}'
    location: location
    parent: sqlServer   
    sku: {
      name: microService.name == 'postulations' ? 'S0' : 'B0'
      tier: microService.name == 'postulations' ? 'Standard' : 'Basic'
    }
  }
]


resource sqlServerFirewallRule 'Microsoft.Sql/servers/firewallRules@2021-11-01' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}
