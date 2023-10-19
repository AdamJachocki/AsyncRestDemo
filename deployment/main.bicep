param location string = resourceGroup().location

@allowed(
    [
    'Premium_LRS'
    'Premium_ZRS'
    'Standard_GRS'
    'Standard_LRS'
    'Standard_RAGRS'
    ]
)
param storageSku string = 'Standard_LRS'

module cosmosModule 'bicep-modules/cosmos.bicep' = {
    name: 'CosmosModule'
    params: {
        location: location
    }
}

module storageModule 'bicep-modules/storage.bicep' = {
    name: 'StorageModule'
    params: {
        location: location
        sku: storageSku
    }
}

module functionsModule 'bicep-modules/function-app.bicep' = {
    name: 'FunctionsModule'
    params: {
        location: location
        storageAccountName: storageModule.outputs.storageName
    }
}