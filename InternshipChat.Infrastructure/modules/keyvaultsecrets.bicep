param internshipchatKeyVaultName string
param databaseConnectionString string
param storageAccessKey string
param storageAccountName string

resource internshipchatKeyVaultName_azuresqlconnectionstring 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  name: '${internshipchatKeyVaultName}/azuresqlconnectionstring'
  properties: {
    attributes: {
      enabled: true
    }
    value: databaseConnectionString
  }
}

resource internshipchatKeyVaultName_storageaccesskey 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  name: '${internshipchatKeyVaultName}/storageaccesskey'
  properties: {
    attributes: {
      enabled: true
    }
    value: storageAccessKey
  }
}

resource internshipchatKeyVaultName_storageconnectionstring 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  name: '${internshipchatKeyVaultName}/storageconnectionstring'
  properties: {
    attributes: {
      enabled: true
    }
    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccessKey};EndpointSuffix=core.windows.net'
  }
}
