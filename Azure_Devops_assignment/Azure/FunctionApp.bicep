/*
** Parameters
*/
param resourcePrefix string = 'jminh' // Shortened prefix for demonstration
param resourceLocation string = resourceGroup().location

@allowed([
  'Standard_LRS'
])
param storageSkuName string = 'Standard_LRS'

/*
** Variables
*/
var resourceSuffix = substring(uniqueString(resourceGroup().id), 0, 7)
var storageAccountName = '${resourcePrefix}storage${resourceSuffix}'
var storageConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storage.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storage.id, '2021-06-01').keys[0].value}'
var serverFarmName = '${resourcePrefix}-plan-${resourceSuffix}'
var tags = {} // You can define your tags here if needed
var keyVaultName = '${resourcePrefix}-kv-${resourceSuffix}'
var functionAppName = '${resourcePrefix}-funcapp-${resourceSuffix}'
var storageQueue1Name = 'jobrequest'
var storageQueue2Name = 'imagegenerationjob'

/*
** Resources
*/

resource storage 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: storageAccountName
  location: resourceLocation
  sku: {
    name: storageSkuName
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
    encryption: {
      keySource: 'Microsoft.Storage'
      services: {
        blob: {
          enabled: true
        }
        file: {
          enabled: true
        }
        queue: {
          enabled: true
        }
        table: {
          enabled: true
        }
      }
    }
  }
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2021-08-01' = {
  name: '${storage.name}/default'
  properties: {
    // Blob service properties, if needed
  }
}

resource serverFarm 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: serverFarmName
  location: resourceLocation
  tags: tags
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}

resource funcApp 'Microsoft.Web/sites@2021-02-01' = {
  name: functionAppName
  location: resourceLocation
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    httpsOnly: true
    serverFarmId: serverFarm.id
    siteConfig: {
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      netFrameworkVersion: 'v6.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageConnectionString
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: storageConnectionString
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'ContentStorageAccount'
          value: storage.name
        }
        {
          name: 'BlobConnectionString'
          value: storageConnectionString
        }
        {
          name: 'BuienraderAPI'
          value: 'https://data.buienradar.nl/2.0/feed/json'
        }
        {
          name: 'ImageApi'
          value: 'https://image.buienradar.nl/2.0/image/single/RadarMapRainNL?height=512&width=500&renderBackground=True&renderBrandingTrue&renderText=True'
        }
        {
          name: 'GetWeatherDataBaseUrl'
          value: 'https://${functionAppName}.azurewebsites.net/api/GetWeatherData_HttpTrigger/'
        }
      ]
    }
  }
}

resource jobrequest 'Microsoft.Storage/storageAccounts/queueServices/queues@2021-08-01' = {
  name: '${storage.name}/default/${storageQueue1Name}'
  properties: {
    // Queue properties, if needed
  }
}

resource imagegenerationjob 'Microsoft.Storage/storageAccounts/queueServices/queues@2021-08-01' = {
  name: '${storage.name}/default/${storageQueue2Name}'
  properties: {
    // Queue properties, if needed
  }
}
