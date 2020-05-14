#variables générales
$vCenter_Adress = '' #définir l'adresse IP ou nom FQDN du serveur VCSA selon la présente d'un serveur DNS sur le réseau
$account = '' #définir le nom de compte qui doit se connecter au serveur ci-dessus
$password = '' #définir le mot de passe du compte qui doit se connecter

$datacenterName = '' #définir le nom du Datacenter de l'infrastructure
$clusterName = '' #définir le nom du cluster de l'infrastructure
$resultFilePath = "" #définir le chemin du fichier JSON de résultat final

<#
.NOTES
    *****************************************************************************
	ETML
	Name: 	getClusterInfo.ps1
    	Author:	Nicolas Benitez
    	Date:	13.05.2020
 	*****************************************************************************
    Modifications
 	Date  : 
 	Author: 
 	Reason: 
 	*****************************************************************************
.SYNOPSIS
    Summary 
 	
.DESCRIPTION
    Description (explanation of script)
#>

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
$cluster = Get-Datacenter $datacenterName | Get-Cluster -Name $clusterName

$Hosts = $cluster | Get-VMHost

#création d'un tableau contenant les objets JSON des vms
$hostlist = [System.Collections.ArrayList]@()

foreach($esxiHost in $Hosts)
{
    $hostInfo = [ordered]@{
        "Adress" = $esxiHost.Name;
        "State" = $esxiHost.PowerState;
        "Version" = $esxiHost.Version;
        "Build" = $esxiHost.Build;
        "APIVersion" = $esxiHost.ApiVersion;
    }
    #ajout de l'objet JSON au tableau
    $hostlist.Add($hostInfo) 
}


#création d'un objet JSON pour les informations de l'hôte
$json = [ordered]@{

    "Name" = $cluster.Name;
    "VSANEnabled" = $cluster.VsanEnabled;
    "DRSEnabled" = $cluster.DrsEnabled;
    "HAEnabled" = $cluster.HAEnabled;
    "Hosts" = $hostlist;
}

#ajout de l'objet JSON de l'hôte dans le tableau principal
$jsonarray += $json

#conversion au format JSON et envoi du résultat dans un fichier texte
$jsonarray| ConvertTo-Json -depth 100 | Out-File -FilePath $resultFilePath