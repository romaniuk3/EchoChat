param sqlserverName string
param administratorLogin string
@secure()
param administratorLoginPassword string
param databaseName string
param location string

resource SqlServer 'Microsoft.Sql/servers@2022-08-01-preview' = {
  name: sqlserverName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
}

resource Database 'Microsoft.Sql/servers/databases@2022-08-01-preview' = {
  parent: SqlServer
  name: databaseName
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 1073741824
  }
}

resource SqlServerRule 'Microsoft.Sql/servers/firewallRules@2022-08-01-preview' = {
  parent: SqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}

output connectionString string = 'Server=tcp:${SqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${databaseName};Persist Security Info=False;User ID=${administratorLogin};Password=${administratorLoginPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
