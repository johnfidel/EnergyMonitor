RASPBERRY_IP=192.168.1.33
TARGET_DIR=home/pi/bin/EnergyMonitor
SSH_PASS=raspberry
SSH_TARGET=pi@$RASPBERRY_IP
ABSOLUTE_TARGET_DIR=$SSH_TARGET:/$TARGET_DIR

sshpass -p $SSH_PASS ssh $SSH_TARGET sudo service EnergyMonitor stop
dotnet publish -r linux-arm
#scp -r EnergyMonitor/bin/Debug/net5.0/linux-arm/publish/* $ABSOLUTE_TARGET_DIR
scp -r WebUI/bin/Debug/net5.0/linux-arm/publish/* $ABSOLUTE_TARGET_DIR
sshpass -p $SSH_PASS ssh $SSH_TARGET sudo service EnergyMonitor start
