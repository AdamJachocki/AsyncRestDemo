param functionName string = 'weather-functions-${uniqueString(resourceGroup().id)}'
param hostingPlanName string = 'weather-functions-hosting'
param location string
param storageAccountName string

resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' existing = {
  name: storageAccountName
}

var storageAccountKey = storageaccount.listKeys().keys[0].value

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}


resource azureFunction 'Microsoft.Web/sites@2020-12-01' = {
  name: functionName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
    alwaysOn: false
    ftpsState: 'FtpsOnly'
    functionAppScaleLimit: 2
      appSettings: [
        {
          name: 'AzureWebJobsDashboard'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccountKey}'
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccountKey}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccountKey}'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
      ]
    }
  }
}
