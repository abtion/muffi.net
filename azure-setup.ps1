# Set names & passwords (complex passwords required)
$webAppName="mywebapp$(Get-Random)"
$appInsightsName="myappinsights$(Get-Random)"
$resourceGroupName="myResourceGroup"
$location="West Europe"
$sqlServerName="webappwithsql$Random"
$sqlServerPassword="123!as2t&%¤asf-,q34tg<sdf"
$sqlServerUsername="ServerAdmin"
$sqlServerPasswordObj=New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $sqlServerUsername,(ConvertTo-SecureString -String $sqlServerPassword -AsPlainText -Force)
$sqlDatabaseName="MySampleDatabase"


# Create a resource group.
New-AzResourceGroup -Name $resourceGroupName -Location $location

# Create an App Service plan in Free tier.
New-AzAppServicePlan -Name $webAppName -Location $location -ResourceGroupName $resourceGroupName -Tier Free

# Create a web app.
$app = New-AzWebApp -Name $webAppName -Location $location -AppServicePlan $webAppName -ResourceGroupName $resourceGroupName

# Setup Application Insights
$appinsights = New-AzApplicationInsights -Name $appInsightsName -ResourceGroupName $resourceGroupName -Location $location

# Configure app with Application Insights
$newAppSettings = @{} # case-insensitive hash map
$app.SiteConfig.AppSettings | %{$newAppSettings[$_.Name] = $_.Value} # preserve non Application Insights application settings.
$newAppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"] = $appinsights.InstrumentationKey; # set the Application Insights instrumentation key
$newAppSettings["APPLICATIONINSIGHTS_CONNECTION_STRING"] = $appinsights.ConnectionString; # set the Application Insights connection string
$newAppSettings["ApplicationInsightsAgent_EXTENSION_VERSION"] = "~2"; # enable the ApplicationInsightsAgent
Set-AzWebApp -AppSettings $newAppSettings -ResourceGroupName $app.ResourceGroup -Name $app.Name -ErrorAction Stop

# Create a SQL Database Server
New-AzSQLServer -ServerName $sqlServerName -Location $location -SqlAdministratorCredentials $sqlServerPasswordObj -ResourceGroupName $resourceGroupName

# Create Firewall Rule for SQL Database Server
New-AzSqlServerFirewallRule -ResourceGroupName $resourceGroupName -ServerName $sqlServerName -AllowAllAzureIPs

# Create serverless low-price SQL Database in SQL Database Server
New-AzSQLDatabase -ServerName $sqlServerName -DatabaseName $sqlDatabaseName -ResourceGroupName $resourceGroupName -ComputeModel Serverless -VCore 1 -Edition GeneralPurpose -ComputeGeneration Gen5 -MaxSizeBytes 1gb

# Configure app database connection string
Set-AzWebApp -ConnectionStrings @{ DefaultConnection = @{ Type="SQLAzure"; Value ="Server=tcp:$sqlServerName.database.windows.net;Database=$sqlDatabaseName;User ID=$sqlServerUsername@$sqlServerName;Password=$password;Trusted_Connection=False;Encrypt=True;" } } -Name $webAppName -ResourceGroupName $resourceGroupName
