# sauna
Sauna automation

## Deploy
For convenience, it's better to build and deploy from a WSL terminal (so that you can use rsync). The following command will build the app and deploy the assemblies on the RPI. Launch this command from the root folder:

dotnet.exe publish -c RPI -r linux-arm ; rsync -avz --progress ./Sauna.Server/bin/RPI/netcoreapp3.1/linux-arm/publish/* pi@192.168.1.221:/home/pi/Desktop/sauna_rpi