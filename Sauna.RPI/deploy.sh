#!/bin/bash

#When editing this file on windows, CRLF are used and bash will fail to launch this file
#You can clean it with sed -i 's/\r$//' deploy.sh

#Terminate the script whenever a command fails
set -e 
set -o pipefail

RPI_ADDRESS=192.168.1.115
DESTINATION=/home/pi/Desktop/sauna_rpi/

echo "Building..."
dotnet.exe publish -c RPI -r linux-arm 

echo
echo "Uploading..."
rsync -avz --progress ./bin/RPI/netcoreapp3.1/linux-arm/publish/* pi@$RPI_ADDRESS:$DESTINATION

echo
echo "Running..."
ssh pi@$RPI_ADDRESS ${DESTINATION}Sauna.RPI
