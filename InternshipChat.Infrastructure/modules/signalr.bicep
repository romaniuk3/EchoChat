param location string
param name string


resource SignalR 'Microsoft.SignalRService/SignalR@2022-08-01-preview' = {
  name: name
  location: location
  sku: {
    name: 'Free_F1'
    tier: 'Free'
    capacity: 1
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

output signalRConnectionString string = 'Endpoint=https://${SignalR.properties.hostName};AccessKey=${listKeys(SignalR.id, SignalR.apiVersion).primaryKey};Version=1.0'
