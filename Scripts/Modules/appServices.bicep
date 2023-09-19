param location string
param appNamePrefix string

param microServices array = [
  {
    id: 1
    name: 'mvc'
    appServicePlanSku: 'S1'
    createDatabase: false
  }
  {
    id: 2
    name: 'postulations'
    appServicePlanSku: 'B1'
    databaseSku: 'Standard'
    createDatabase: true
  }
  {
    id: 3
    name: 'emplois'
    appServicePlanSku: 'F1'
    databaseSku: 'Basic'
    createDatabase: true
  }
  {
    id: 4
    name: 'documents'
    appServicePlanSku: 'F1'
    createDatabase: false
  }
  {
    id: 5
    name: 'favoris'
    appServicePlanSku: 'F1'
    createDatabase: false
  }
]

resource appServicePlans 'Microsoft.Web/serverfarms@2022-09-01' = [
  for microService in microServices: {
    name: 'sp-${appNamePrefix}-${microService.name}'
    location: location
    properties: {
      name: 'sp-${appNamePrefix}-${microService.name}'
      sku: {
        name: microService.appServicePlanSku
        tier: microService.appServicePlanSku
      }
    }
  }
]

resource webApps 'Microsoft.Web/sites@2022-09-01' = [
  for microService in microServices: {
    name: 'webapp-${appNamePrefix}-${microService.name}-${uniqueString(resourceGroup().id)}'
    location: location
    properties: {
      serverFarmId: microService.id
      name: 'webapp-${appNamePrefix}-${microService.name}-${uniqueString(resourceGroup().id)}'
    }
    tags: {
      Application: appNamePrefix
    }
  }
]

resource appServiceSlots 'Microsoft.Web/sites/slots@2022-09-01' = [
  for microService in microServices: if (microService.appServicePlanSku == 'S1') {
    name: 'webapp-${appNamePrefix}-${microService.name}-staging'
    location: location
    properties: {
      serverFarmId: appServicePlans[microService.id]
      name: 'webapp-${appNamePrefix}-${microService.name}-staging'
    }
    tags: {
      Application: appNamePrefix
    }
  }
]

resource appServiceScaleRules 'Microsoft.Insights/autoscalesettings@2022-10-01' = [
  for microService in microServices: if (microService.appServicePlanSku == 'S1') {
    name: 'scaleRule-${microService.name}'
    properties: {
      direction: 'Increase'
      changeCount: 1
      changeCountDirection: 'Percent'
      scaleInCooldown: 'PT10M'
      scaleOutCooldown: 'PT10M'
      metricTrigger: {
        metricName: 'cpuPercentage'
        metricResourceUri: microService.id
        operator: 'GreaterThan'
        threshold: 80
        timeAggregation: 'Average'
      }
    }
  }  
]
