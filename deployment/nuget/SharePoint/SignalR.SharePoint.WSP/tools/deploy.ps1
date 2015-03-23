param([string] $WebAppUrl, [switch] $NoWebApp,  [switch] $NoFeature, [switch] $Update)

$deployScriptDirectory = (Split-Path $MyInvocation.MyCommand.Path)

$solutionName = "SignalR.SharePoint.wsp"
$solutionFileName = Join-Path $deployScriptDirectory "..\content\$solutionName"

$nuspecFile = (Get-ChildItem -Path (Join-Path $deployScriptDirectory "\..\") -Filter "$solutionName.nuspec")
if($nuspecFile -eq $null)
{
 	throw "Missing package definition file"
}

Add-PSSnapin Microsoft.SharePoint.PowerShell

function Get-PackageVersion()
{
	$xml = New-Object XML
	$xml.Load($nuspecFile.FullName)
	return $xml.package.metadata.version
}

function Update-SPSolutionVersion([string] $SolutionName, [string] $Version)
{
	$solution = (Get-SPSolution | Where-Object { $_.Name -eq $solutionName })
	if($solution -ne $null)
	{
		$solution.Properties["Version"] = $Version
		$solution.Update()
	}
}

function WaitFor-SPManagedMetadataService()
{
	$service = Get-SPServiceInstance | Where-Object { $_.TypeName -eq "Managed Metadata Web Service" -and $_.Status -eq [Microsoft.SharePoint.Administration.SPObjectStatus]::Online }
	if($service -eq $null)
	{
		$service = Get-SPServiceInstance | Where-Object { $_.TypeName -eq "Managed Metadata Web Service"}
		if($service.Status -eq  [Microsoft.SharePoint.Administration.SPObjectStatus]::Disabled)
		{
			Start-SPServiceInstance $service.Id
		}
	}
	
	if(-not ($service.Status -eq [Microsoft.SharePoint.Administration.SPObjectStatus]::Online))
	{
		Write-Host "Waiting for service 'Managed Metadata Web Service'..."
		
		$startTime = [DateTime]::Now
		do
		{
			Start-Sleep -Seconds 2
			$service = Get-SPServiceInstance | Where-Object { $_.TypeName -eq "Managed Metadata Web Service"}
			if([DateTime]::Now.Subtract($startTime).TotalMinutes -gt 5)
			{
			    throw "Something is wrong with the required service 'Managed Metadata Web Service'"
			}
		} 
		while(-not ($service.Status -eq [Microsoft.SharePoint.Administration.SPObjectStatus]::Online))
	}
	
	$taxonymySession = Get-SPTaxonomySession -Site ([Microsoft.SharePoint.Administration.SPAdministrationWebApplication]::Local.Sites[0])
	
	$startTime = [DateTime]::Now
	while($taxonymySession.TermStores.Count -eq 0)
	{
		Start-Sleep -Seconds 2
		if([DateTime]::Now.Subtract($startTime).TotalMinutes -gt 5)
		{
			throw "Something is wrong with the required service 'Managed Metadata Web Service'"
		}
		
		$taxonymySession = Get-SPTaxonomySession -Site ([Microsoft.SharePoint.Administration.SPAdministrationWebApplication]::Local.Sites[0])
	}
}

function Restart-IIS()
{
	[array]$servers= Get-SPServer | ? {$_.Role -eq "Application"}
	$farm = Get-SPFarm
	foreach ($server in $servers)
	{
		Write-Host -ForegroundColor Yellow "Attempting to reset IIS for $server"
		iisreset $server  "\\"$_.Address
		iisreset $server /status "\\"$_.Address
		Write-Host
		Write-Host -ForegroundColor Green "IIS has been reset for $server"
		Write-Host
	}
}

function WaitFor-SPSolutionDeploymentJob([string] $solutionName)
{
	$job = $null
	do
	{
		$job = Get-SPTimerJob | Where-Object { $_.Name -like "*$solutionName*" }
		if(-not ($job -eq $null))
		{
			Start-Sleep -Seconds 2
		}
	}
	while(-not ($job -eq $null))
}

function Stop-SPSolutionDeploymentJob([string] $solutionName)
{
	$job = Get-SPTimerJob | Where-Object { $_.Name -like "*$solutionName*" }
	if(-not ($job -eq $null))
	{
		$job.Delete()
	}
}

function Install-SPSolutionEx([string] $SolutionName, [string] $Version, [switch] $Update)
{
	Stop-SPSolutionDeploymentJob $SolutionName

	if ((Get-SPSolution | Where-Object { $_.Name -eq $SolutionName }) -eq $null)
	{
		Write-Host "Installing $SolutionName..."
		Add-SPSolution $solutionFileName
		$solution = (Get-SPSolution | Where-Object { $_.Name -eq $SolutionName })

	}
	elseif($Update)
	{
		Write-Host "Updating $SolutionName..."
		Update-SPSolution -Identity $SolutionName -LiteralPath $solutionFileName  -Force -GACDeployment
		WaitFor-SPSolutionDeploymentJob $SolutionName
	}

	Update-SPSolutionVersion -SolutionName $SolutionName -Version $Version
	#Restart-IIS
}

try
{
	Install-SPSolutionEx -SolutionName $solutionName -Update:$Update -Version (Get-PackageVersion)
}
finally
{
	Remove-PSSnapin Microsoft.SharePoint.PowerShell
}