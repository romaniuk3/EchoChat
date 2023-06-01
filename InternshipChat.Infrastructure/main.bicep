param administratorLogin string
@secure()
param administratorLoginPassword string
param databaseName string
param keyVaultName string
param objectId string
param storageAccountName string
param chatApiName string
param chatClientName string
param sqlserverName string
param signalRServiceName string
param functionAppName string
param location string = resourceGroup().location

module sqlServerModule 'modules/sqlserverdb.bicep' = {
  name: 'CreateSqlServer'
  params: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
    databaseName: databaseName
    location: location
    sqlserverName: sqlserverName
  }
}

module webAppsModule 'modules/webapps.bicep' = {
  name: 'CreateWebApps'
  dependsOn: [
    sqlServerModule
  ]
  params: {
    chatApiName: chatApiName
    chatClientName: chatClientName
    location: location
  }
}

module storageModule 'modules/storage.bicep' = {
  name: 'CreateStorage'
  dependsOn: [
    sqlServerModule
  ]
  params: {
    location: location
    storageAccountName: storageAccountName
  }
}

module keyVaultModule 'modules/keyvault.bicep' = {
  name: 'CreateKeyVault'
  dependsOn: [
    webAppsModule
  ]
  params: {
    internshipchatKeyVaultName: keyVaultName
    location: location
    objectId: objectId
    webApiObjectId: webAppsModule.outputs.apiObjectId
  }
}

module signalRModule 'modules/signalr.bicep' = {
  name: 'CreateSignalRService'
  dependsOn: [
    webAppsModule
  ]
  params: {
    location: location
    name: signalRServiceName
  }
}

module secretsModule 'modules/keyvaultsecrets.bicep' = {
  name: 'CreateSecrets'
  dependsOn: [
    keyVaultModule
  ]
  params: {
    databaseConnectionString: sqlServerModule.outputs.connectionString
    internshipchatKeyVaultName: keyVaultName
    storageAccessKey: storageModule.outputs.storageAccountKey
    storageAccountName: storageAccountName
    signalRConnectionString: signalRModule.outputs.signalRConnectionString
  }
}

module azFunctionsModule 'modules/functionsapp.bicep' = {
  name: 'CreateAzFunctionsApp'
  dependsOn: [
    secretsModule
  ]
  params: {
    functionAppName: functionAppName
    location: location
  }
}

resource chatApiAppSettings 'Microsoft.Web/sites/config@2022-09-01' = {
  name: '${chatApiName}/appsettings'
  properties: {
    KeyVaultURL: keyVaultModule.outputs.kvUrl
    StorageAccountName: storageAccountName
  }
  dependsOn: [
    webAppsModule
    secretsModule
  ]
}
