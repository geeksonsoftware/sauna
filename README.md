# sauna
Sauna automation

## Setup the solution
```
dotnet tool restore
```

## Build the solution
```
dotnet cake build
```

## Deploy
For convenience, it's better to build and deploy from a WSL terminal (so that you can use rsync). The following command will build the app and deploy the assemblies on the RPI. Launch this command from the root folder:

```
dotnet cake -target=deploy -host="192.168.1.228"
```

## Icons
https://feathericons.com/