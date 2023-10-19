param accountName string = 'weatherst${uniqueString(resourceGroup().id)}'
param location string
param queueName string = 'weather-requests-queue'

@allowed(
    [
    'Premium_LRS'
    'Premium_ZRS'
    'Standard_GRS'
    'Standard_LRS'
    'Standard_RAGRS'
    ]
)
param sku string

resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: accountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: sku
  }
  properties: {
    accessTier: 'Cool'
    allowBlobPublicAccess: true
    supportsHttpsTrafficOnly: true
  }
}

resource queueService 'Microsoft.Storage/storageAccounts/queueServices@2022-09-01' = {
    name: 'default'
    parent: storageaccount
}

resource queue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-09-01' = {
  name: queueName
  parent: queueService
  properties: {
    metadata: {}
  }
}

output storageName string = storageaccount.name


