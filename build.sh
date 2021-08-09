RASPBERRY_IP=192.168.1.154
TARGET_DIR=home/pi/bin/EnergyMonitor
SSH_PASS=raspberry
SSH_TARGET=pi@$RASPBERRY_IP
ABSOLUTE_TARGET_DIR=$SSH_TARGET:/$TARGET_DIR

#sshpass -p $SSH_PASS ssh $SSH_TARGET sudo service EnergyMonitor stop
#sshpass -p $SSH_PASS ssh $SSH_TARGET sudo service EnergyMonitorWebUI stop
rm -rf output
dotnet clean -v minimal
dotnet build -v minimal
dotnet test --no-build -v normal
dotnet publish EnergyMonitor/EnergyMonitor.csproj -o output -c Release -r linux-arm -v minimal
dotnet publish WebUI/WebUI.csproj -o output -c Release -r linux-arm -v minimal
scp -r output/* $ABSOLUTE_TARGET_DIR
#sshpass -p $SSH_PASS ssh $SSH_TARGET sudo service EnergyMonitor start
#sshpass -p $SSH_PASS ssh $SSH_TARGET sudo service EnergyMonitorWebUI start
