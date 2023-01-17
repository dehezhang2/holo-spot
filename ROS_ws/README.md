# Holo-Spot ROS workspace
Different from other files of this repo, this ROS package needs to be installed in the onboarded computer of the Boston Dynamics Spot robots.

## Tested Environment
Ubuntu 20.04 & ROS Noetic

## Dependency
### Microsoft Azure Spatial Anchor

* Azure Spatial Anchors (for co-localization): [Tutorial](https://learn.microsoft.com/en-us/azure/spatial-anchors/tutorials/tutorial-new-unity-hololens-app?tabs=azure-portal)

* Azure Spatial Anchors Linux SDK (for spot): [Github Repo](https://github.com/microsoft/azure_spatial_anchors_ros).
This one has a ton of information in the wiki.

* You have to sign up here: [Microsoft Forms](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbRx1PiClb2ndIsvKW1oq51RFUQ0Y0SUJUQjk5OEM0ODVBVUUwSFlQTEQySC4u) to get access to the sdk binaries. That might take 1-2 days. 

## Install & Usage
First need to complie this ROS package,
```bash
cd ROS_ws
catkin_make
```
For every opened terminal, ```cd``` into this ```ROS_ws```.
```bash
source devel/setup.bash
```
Fill in the login information of Spot robot in the [launch file](src/unity_robotics_demo/launch/robo_demo.launch) and the [driver-only launch file](src/unity_robotics_demo/launch/driver.launch). Replace ```YOUR_SPOT_USERNAME```, ```YOUR_SPOT_PASSWORD```, and ```YOUR_SPOT_HOSTNAME``` with your login information.

Fill in the login information of Microsoft Azure in the [Spatial Anchor launch file](src/azure_spatial_anchors_ros/asa_ros/launch/asa_ros.launch). Replace ```YOUR_ACCOUNT_ID```, ```YOUR_ACCOUNT_KEY```, and ```YOUR_ACCOUNT_DOMAIN``` with your login information.

To run this driver,
```bash
roslaunch unity_robotics_demo robo_demo.launch
```

To ensure the safety, open another terminal and ```source``` the package. Once the robot is under unsafety situation, run 
```bash
rosservice call /spot/estop/gentle
```

## Note
* Once the robot recognizes the spatial anchor created by Hololens, there will be a transformation matrix of the anchor printed in the terminal.

* The robot might need to walk around to find the created anchor.