param location string
param appNamePrefix string
param dbAdminPassword string
var isPasswordValid = length(dbAdminPassword) >= 10 && length(dbAdminPassword) <= 20

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

resource sqlServer 'Microsoft.Sql/servers@2023-02-01-preview' = if (isPasswordValid) {
  name: 'srv-${appNamePrefix}'
  location: location
  properties: {
    administratorLogin: 'sqladmin'
    administratorLoginPassword: dbAdminPassword
    version: '12.0'
  }
}

resource sqlDatabases 'Microsoft.Sql/servers/databases@2023-02-01-preview' = [
  for microService in microServices : if (microService.createDatabase && isPasswordValid) {
    name: 'db-${appNamePrefix}-${microService.name}'
    location: location
    properties: {
      edition: microService.name == 'postulations' ? 'Standard' : 'Basic'
      maxSizeBytes: 1073741824
      collation: 'SQL_Latin1_General_CP1_CI_AS'
      requestedServiceObjectiveName: microService.name == 'postulations' ? 'S0' : 'B0'
      elasticPoolId: ''
      serverName: sqlServer.name
    }
  }
]

resource sqlServerFirewallRule 'Microsoft.Sql/servers/firewallRules@2022-11-01-preview' = {
  name: '${sqlServer.name}/AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}
