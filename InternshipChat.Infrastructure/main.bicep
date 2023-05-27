param administratorLogin string = 'defaultadminlogin4'
param administratorLoginPassword string = 'def@ultpaSsword1'
param databaseName string = 'InternshipChatDefault'
param keyVaultName string = 'defaultkeyvault9f330'
param objectId string = '02340230f'
param storageAccountName string = 'defaultstora5ge33'
param chatApiName string = 'defaulta3423chatapi'
param chatClientName string = 'default2323chat4client'
param sqlserverName string = 'defaul3tserv8er9'
param location string = resourceGroup().location

module webAppsModule './modules/webapps.bicep' = {
  name: 'CreateWebApps'
  params: {
    chatApiName: chatApiName
    chatClientName: chatClientName
    location: location
  }
}
