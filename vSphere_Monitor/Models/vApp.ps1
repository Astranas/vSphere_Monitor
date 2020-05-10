param (
    [string]$vmhost
)

#variables générales
$vCenter_Adress = '192.168.1.171'
$account = 'monitor@vsphere.local'
$password = '.Etml-44'

$datacenterName = 'Datacenter'
$clusterName = 'Clu-lab01'

$vmhostFileName = $vmhost.Replace('.', '-')
$resultFilePath = "C:\Users\User\source\repos\vSphere_Monitor\vSphere_Monitor\Models\$vmhostFileName.json"

<#
.NOTES
    *****************************************************************************
	ETML
	Name: 	getHostInfo.ps1
    	Author:	Anthony Coke
    	Date:	23.03.2020
 	*****************************************************************************
    Modifications
 	Date  : 03.04.2020
 	Author: Nicolas Benitez
 	Reason: Adaptation du code pour une autre infrastructure (adresses IP) & vSAN
    + Modification de la sortie du script afin d'avoir un format JSON facilement utilisable dans le projet ASP.NET MVC

    Date  : 30.04.2020
 	Author: Nicolas Benitez
 	Reason: Modification de connexion au serveur vCenter (compte read-only)
    + Modification de la sortie du script afin d'avoir un tableau bien structuré contenant tous les hôtes en un seul fichier

    Date  : 08.05.2020
 	Author: Nicolas Benitez
 	Reason: Modification du code pour ne demander les informations que d'un seul hôte à la fois
    + Modification de la structure de l'object JSON en sortie (plus besoin d'un tableau puisqu'il concerne UN seul hôte)
 	*****************************************************************************
.SYNOPSIS
    Summary 
 	
.DESCRIPTION
    Description (explanation of script)
#>

if($vmhost -ne "" -and $vmhost -notlike $null)
{
    #nettoyage si le fichier existe déjà
    $fileToCheck = $resultFilePath
    if (Test-Path $fileToCheck -PathType leaf)
    {
        Clear-Content $resultFilePath
    }

    #création d'un tableau principal d'objets JSON
    $jsonarray = [System.Collections.ArrayList]@()

    #connexion au vcsa
    Connect-VIServer $vCenter_Adress -User $account -Password $password | Out-Null

    #récupération des hôtes esxi
    $esxiHost = Get-Datacenter $datacenterName | Get-Cluster -Name $clusterName | Get-VMHost -Name $vmhost | Where-Object {$_.PowerState -eq 'PoweredOn'}

    #récupération des datastores de l'hôte
    $localstorages = $esxiHost | Get-Datastore | Where-Object {$_.Name -ne "vsanDatastore"}

    $localstoragesarray = [System.Collections.ArrayList]@()

    #remise à zero des variables pour l'itération suivante
    $CapacityRounded = 0
    $FreeRounded = 0

    #boucle sur les datastore pour avoir les valeurs des totaux
    foreach($datastore in $localstorages)
    {
        $storage = @{
        "CapacityRounded" = [Math]::Round($datastore.CapacityGB,0);
        "FreeRounded" = [Math]::Round($datastore.FreeSpaceGB,0);
        }
        $localstoragesarray.Add($storage)
    }

    $vsanDisks = Get-VsanDisk -VMHost $esxiHost.Name

    $vsandisksarray = [System.Collections.ArrayList]@()

    foreach($disk in $vsanDisks)
    {
        $diskInfo = @{
        "Uuid" = $disk.Uuid;
        "CapacityGB" = [Math]::Round($disk.CapacityGB, 0);
        "UsedPercent" = [Math]::Round($disk.UsedPercent, 2);
        "IsCacheDisk" = $disk.IsCacheDisk;
        "IsSSD" = $disk.IsSsd;
        }
        $vsandisksarray.Add($diskInfo)
    }
    
    #récupération des vms de l'hôte
    $vms = Get-VMHost $esxiHost.Name | Get-VM

    #création d'un tableau contenant les objets JSON des vms
    $vmlist = [System.Collections.ArrayList]@()
    
    #boucle sur les vms pour créer des objets JSON de vms
    foreach($vm in $vms)
    {
        $vsanHealth = (Get-VsanObject -VM $vm.Name | Where-Object {$_.Type -eq 'VDisk'} | Select VsanHealth).VsanHealth

        $vmInfo = @{
        "Name" = $vm.Name;
        "Cpu_number" = $vm.NumCpu;
        "Memory" = $vm.MemoryGB;
        "State" = $vm.PowerState;
        "Host" = $esxiHost.Name;
        "VsanHealth" = $vsanHealth;
        }
        #ajout de l'objet JSON au tableau
        $vmlist.Add($vmInfo) 
    }

    #création d'un objet JSON pour les informations de l'hôte
    $json = [ordered]@{

        "Adress" = $esxiHost.Name;
        "Memory_usage" = ([Math]::Round($esxiHost.MemoryUsageGB, 2));
        "Memory_total" = ([Math]::Round($esxiHost.MemoryTotalGB, 2));
        "Cpu_usage" = ([Math]::Round($esxiHost.CpuUsageMhz, 2));
        "Cpu_total" = ([Math]::Round($esxiHost.CpuTotalMhz, 2));

        "localDisks" = $localstoragesarray;

        "vSanDisks" = $vsandisksarray

        #ajout du tableau des objets JSON des vms dans l'objet JSON de l'hôte
        "VMs" = $vmlist
    }

    #ajout de l'objet JSON de l'hôte dans le tableau principal
    $jsonarray += $json

    #conversion au format JSON et envoi du résultat dans un fichier texte
    $jsonarray| ConvertTo-Json -depth 100 | Out-File -FilePath $resultFilePath
}