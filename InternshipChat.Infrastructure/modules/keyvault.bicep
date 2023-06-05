param internshipchatKeyVaultName string
param objectId string
param webApiObjectId string
param location string
param azFunctionsObjectId string

resource KeyVault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: internshipchatKeyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: objectId
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'all'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: webApiObjectId
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'get'
            'list'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: azFunctionsObjectId
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
    enabledForDeployment: true
    enabledForDiskEncryption: true
    enabledForTemplateDeployment: true
  }
}

output kvUrl string = KeyVault.properties.vaultUri
