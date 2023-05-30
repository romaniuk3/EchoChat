param location string
param name string


resource SignalR 'Microsoft.SignalRService/signalR@2023-03-01-preview' = {
  name: name
  location: location
  sku: {
    capacity: 1
    name: 'Free_F1'
    tier: 'Free'
  }
  kind: 'SignalR'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    features: [
      {
        flag: 'ServiceMode'
        value: 'Default'
      }
      {
        flag: 'EnableConnectivityLogs'
        value: 'true'
      }
      {
        flag: 'EnableMessagingLogs'
        value: 'true'
      }
    ]
    cors: {
      allowedOrigins: [
        '*'
      ]
    }
  }
}

output signalRConnectionString string = 'Endpoint=${SignalR.properties.hostName};AccessKey=${listKeys(SignalR.id, SignalR.apiVersion).primaryKey}'
