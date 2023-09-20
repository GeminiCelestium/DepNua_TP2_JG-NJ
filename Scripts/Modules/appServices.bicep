param location string
param spName string

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
    name: 'sp-${spName}-${microService.name}'
    location: location    
    sku: {
      name: microService.appServicePlanSku
      tier: microService.appServicePlanSku
    }    
  }
]

resource webApps 'Microsoft.Web/sites@2022-09-01' = [
  for microService in microServices: {
    name: 'webapp-${microService.name}-${uniqueString(resourceGroup().id)}'
    location: location
    properties: {
      serverFarmId: microService.id
    }
    tags: {
      Application: spName
    }
  }
]

resource appServiceSlots 'Microsoft.Web/sites/slots@2022-09-01' = [
  for microService in microServices: if (microService.appServicePlanSku == 'S1') {
    name: 'webapp-${spName}-${microService.name}-staging'
    location: location
    properties: {
      serverFarmId: microService.id      
    }
    tags: {
      Application: spName
    }
  }
]

resource appServiceScaleRules 'Microsoft.Insights/autoscalesettings@2022-10-01' = [
  for microService in microServices: if (microService.appServicePlanSku == 'S1') {
    location: location
    name: 'scaleRule-${microService.name}'
    properties: {
      profiles: [
        {
          name: 'default'
          capacity: {
            minimum: '1'
            maximum: '10'
            default: '1'
          }
          rules: [
            {
              metricTrigger: {
                metricName: 'cpuPercentage'
                metricResourceUri: microService.id
                operator: 'GreaterThan'
                threshold: 80
                timeAggregation: 'Average'
                statistic: 'Average'
                timeGrain: 'PT1M'
                timeWindow: 'PT5M'
              }
              scaleAction: {
                direction: 'Increase'
                type: 'ChangeCount'
                value: '1'
                cooldown: 'PT5M'
              }
            }
          ]
        }
      ]
    }
  }    
]
