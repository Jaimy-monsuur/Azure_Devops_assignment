# Define the resource group and Bicep file path
$resourceGroupName = "serverside-programming"
$bicepFilePath = "./FunctionApp.bicep"

Write-Host "Running azure commands"

# Deploy the Bicep template using the Azure CLI
$cmd = "az deployment group create --resource-group $resourceGroupName --template-file $bicepFilePath"
Write-Host $cmd
Invoke-Expression  $cmd
