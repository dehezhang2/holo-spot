import rospy
import roslaunch
from unity_robotics_demo_msgs.msg import Id
from std_msgs.msg import String
# class CommandRepublisher():
#     def __init__(self):
#         rospy.init_node("query_anachor")
#         rospy.Subscriber("/hololens/anchor_id", Id, self.received_cmd)
#         self.init = False
        
#         param = 'anchor_id:='+'11234'
#         cli_args = ['/home/cvg-nuc-1/team14_ws/src/azure_spatial_anchors_ros/asa_ros/launch/asa_ros.launch',param]
#         roslaunch_args = cli_args[1:]
#         roslaunch_file = [(roslaunch.rlutil.resolve_launch_arguments(cli_args)[0], roslaunch_args)]
#         uuid = roslaunch.rlutil.get_or_generate_uuid(None, False)
#         parent = roslaunch.parent.ROSLaunchParent(uuid, roslaunch_file)

#     def received_cmd(self, cmd):
#         if not self.init:
#             self.init = True
#             param = 'anchor_id:='+cmd.identifier
#             cli_args = ['/home/cvg-nuc-1/team14_ws/src/azure_spatial_anchors_ros/asa_ros/launch/asa_ros.launch',param]
#             roslaunch_args = cli_args[1:]
#             roslaunch_file = [(roslaunch.rlutil.resolve_launch_arguments(cli_args)[0], roslaunch_args)]
#             uuid = roslaunch.rlutil.get_or_generate_uuid(None, False)
#             parent = roslaunch.parent.ROSLaunchParent(uuid, roslaunch_file)
#             parent.start()
            

#     def spin(self):
#         rospy.spin()
#     def spinOnce(self):
#         rospy.spin()

if __name__ == "__main__":
    # c = CommandRepublisher()
    rospy.init_node('query_anachor')
    msg = rospy.wait_for_message("/hololens/anchor_id", Id)
    param = 'anchor_id:='+msg.identifier
        
    # rospy.signal_shutdown(param)
    

    cli_args = ['/home/cvg-nuc-1/team14_ws/src/azure_spatial_anchors_ros/asa_ros/launch/asa_ros.launch',param]
    roslaunch_args = cli_args[1:]
    roslaunch_file = [(roslaunch.rlutil.resolve_launch_arguments(cli_args)[0], roslaunch_args)]
    uuid = roslaunch.rlutil.get_or_generate_uuid(None, False)
    parent = roslaunch.parent.ROSLaunchParent(uuid, roslaunch_file)
    parent.start()
    
    while not rospy.is_shutdown():
        msg = rospy.wait_for_message("/hololens/anchor_id", Id)
        parent.shutdown()
        param = 'anchor_id:='+msg.identifier
        cli_args = ['/home/cvg-nuc-1/team14_ws/src/azure_spatial_anchors_ros/asa_ros/launch/asa_ros.launch',param]
        roslaunch_args = cli_args[1:]
        roslaunch_file = [(roslaunch.rlutil.resolve_launch_arguments(cli_args)[0], roslaunch_args)]
        uuid = roslaunch.rlutil.get_or_generate_uuid(None, False)
        parent = roslaunch.parent.ROSLaunchParent(uuid, roslaunch_file)
        parent.start()

        
