#!/usr/bin/env pwsh

nbgv get-version -f json | Out-File .\nbgv-version.json
.\GenerateVersioningTargets.ps1
docker build -t aksdemo .