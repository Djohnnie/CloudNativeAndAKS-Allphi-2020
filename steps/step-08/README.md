# Involved Café 2020-07: AKS Workshop

[Previous step](../step-07/README.md) - [Next step](../step-09/README.md)

## Step 8 - Create an ASP.NET Core WebApi and deploy it to Kubernetes

Create a YAML script to configure a Kubernetes service for the ASP.NET Core WebApi:

```yaml
apiVersion: v1
kind: Service
metadata:
  name: webapi
spec:
  selector:
    app: webapi
  ports:
  - port: 80
    targetPort: 80
```

Apply it using the Visual Studio Code command palette:

![dotnet new](sshot-46.png)

Check the Kubernetes Activity pane and find your newly create service within the Network node:

![dotnet new](sshot-47.png)

[Previous step](../step-07/README.md) - [Next step](../step-09/README.md)