# aad-apsnetcore-k8s

A simple sample to demonstrate running an ASP.NET Core web app with Azure AD authentication on Kubernetes.

## Prerequisites

- A Kubernetes cluster that can be used to deploy the demo.
- The Ingress controller is configured on the Kubernetes cluster.
- Helm
- A client id that is registered in Azure AD.

## Install

- Clone the code: `git clone https://github.com/chunliu/aad-apsnetcore-k8s.git`.
- Create a namespace in k8s: `kubectl create namespace aspnetcore-aad`.
- Update the values in `src/aspnetcore-aad-charts/values.yaml` or supply them with the Helm command in the next step.
- Install the charts with Helm: `helm install aspnetcore-aad --namespace aspnetcore-aad ./src/aspnetcore-aad-charts`.
