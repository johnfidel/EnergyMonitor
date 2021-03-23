RASPBERRY_IP=192.168.1.33
TARGET_DIR=home/pi/bin/EnergyMonitor
SSH_PASS=raspberry
SSH_TARGET=pi@$RASPBERRY_IP
ABSOLUTE_TARGET_DIR=$SSH_TARGET:/$TARGET_DIR

sshpass -p $SSH_PASS ssh $SSH_TARGET sudo service EnergyMonitor stop
dotnet clean
dotnet publish EnergyMonitor/EnergyMonitor.csproj -o output -r linux-arm
dotnet publish WebUI/WebUI.csproj -o output -r linux-arm
scp -r output/* $ABSOLUTE_TARGET_DIR
sshpass -p $SSH_PASS ssh $SSH_TARGET sudo service EnergyMonitor start
