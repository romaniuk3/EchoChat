param hostingPlanName string = 'chatplan${uniqueString(resourceGroup().id)}'
param skuName string = 'F1'
param chatApiName string
param functionAppName string
param chatClientName string
param location string

resource ServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: skuName
  }
}

resource ChatApiWebApp 'Microsoft.Web/sites@2022-09-01' = {
  name: chatApiName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: ServicePlan.id
  }
}

resource ChatClientWebApp 'Microsoft.Web/sites@2022-09-01' = {
  name: chatClientName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: ServicePlan.id
  }
}

resource ChatClientAppSettings 'Microsoft.Web/sites/config@2022-09-01' = {
  parent: ChatClientWebApp
  name: 'appsettings'
  properties: {
    AppBase: 'https://${chatApiName}.azurewebsites.net/'
    AzFunctionBase: 'https://${functionAppName}.azurewebsites.net/'
  }
}

output apiObjectId string = ChatApiWebApp.identity.principalId
