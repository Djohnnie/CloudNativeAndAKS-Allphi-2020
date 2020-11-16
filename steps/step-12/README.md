# Involved Café 2020-07: AKS Workshop


az aks show --resource-group rg-involved-cafe-2020-07 --name aks-involved-cafe-2020-07 --query nodeResourceGroup -o tsv



az network public-ip create --resource-group MC_rg-involved-cafe-2020-07_aks-involved-cafe-2020-07_westeurope --name aks-involved-cafe-2020-07-public-ip --sku Standard --allocation-method static --query publicIp.ipAddress -o tsv


az resource show --query id --resource-type Microsoft.Network/publicIPAddresses -n aks-involved-cafe-2020-07-public-ip -g MC_rg-involved-cafe-2020-07_aks-involved-cafe-2020-07_westeurope



az network public-ip update --ids "/subscriptions/5db7e9b4-a01f-4bd4-b7e8-26ca1d5b3ad3/resourceGroups/MC_rg-involved-cafe-2020-07_aks-involved-cafe-2020-07_westeurope/providers/Microsoft.Network/publicIPAddresses/aks-involved-cafe-2020-07-public-ip" --dns-name involved-cafe-2020-07





----------------




helm install nginx-ingress stable/nginx-ingress --namespace ingress  --set controller.replicaCount=2 --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux --set controller.service.loadBalancerIP="40.74.19.227"