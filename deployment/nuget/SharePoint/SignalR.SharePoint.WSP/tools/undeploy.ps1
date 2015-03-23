Add-PSSnapin Microsoft.SharePoint.PowerShell

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

function Uninstall-SPSolutionEx([string] $SolutionName)
{
	$solution = Get-SPSolution | Where-Object { $_.Name -eq $SolutionName }
	if(-not ($solution -eq $null))
	{
		Stop-SPSolutionDeploymentJob $SolutionName
		if($solution.Deployed)
		{
			if($solution.ContainsWebApplicationResource)
			{
				$centralAdministrationWebApp = [Microsoft.SharePoint.Administration.SPAdministrationWebApplication]::Local.Sites[0].WebApplication
				$deployedWebApp = $solution.DeployedWebApplications | Where-Object { $_.Url -eq $centralAdministrationWebApp.Url }
				if($deployedWebApp -eq $null)
				{
					Uninstall-SPSolution $SolutionName -WebApplication $centralAdministrationWebApp -Confirm:$false
					WaitFor-SPSolutionDeploymentJob $SolutionName
				}    
				
				Uninstall-SPSolution $SolutionName -Confirm:$false -AllWebApplications
				WaitFor-SPSolutionDeploymentJob $SolutionName
			}
			else
			{
				Uninstall-SPSolution $SolutionName -Confirm:$false
				WaitFor-SPSolutionDeploymentJob $SolutionName
			}
		}
		
		Remove-SPSolution $SolutionName -Force -Confirm:$false
	}
}

$solutionName = "SignalR.SharePoint.wsp"
Write-Host "Uninstalling $solutionName..."
try
{
	Uninstall-SPSolutionEx -SolutionName $solutionName
}
finally
{
	# Remove-PSSnapin Microsoft.SharePoint.PowerShell
}
