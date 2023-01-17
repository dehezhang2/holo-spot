import rospy
from geometry_msgs.msg import PoseStamped, TransformStamped
from unity_robotics_demo_msgs.msg import PosRot,Id
import tf2_ros as tf
from std_msgs.msg import String,Float32
class CommandRepublisher():
    def __init__(self):
        rospy.init_node("hololens_msgtype_transform")

        rospy.Subscriber("/hololens/pos_rot", PosRot, self.received_cmd)
        self.cmd_pub = rospy.Publisher("/command/raw", PoseStamped, queue_size=1)
        self.scale_pub = rospy.Publisher("/hololens/scale", Float32, queue_size=1)
        self.cmd_feedback_pub = rospy.Publisher("/hololens/feedback", String, queue_size=10)

        self.hololens_frame = "hololens"
    
    def received_cmd(self, cmd):
        rospy.loginfo("received command!")
        self.cmd_feedback_pub.publish("Received command")

        rospy.sleep(0.1)
        
        target_pose = PoseStamped()
        
        # target_pose.header.frame_id = self.hololens_frame
        target_pose.header.frame_id = cmd.frame
                
        target_pose.pose.position.x = -cmd.pos_x
        target_pose.pose.position.y = -cmd.pos_y
        target_pose.pose.position.z = -cmd.pos_z
        target_pose.pose.orientation.x = 0
        target_pose.pose.orientation.y = 0
        target_pose.pose.orientation.z = 0
        target_pose.pose.orientation.w = 1

        self.cmd_pub.publish(target_pose)
        self.cmd_feedback_pub.publish("Success; Republished command")
        scale = Float32()
        scale.data = cmd.rot_x
        self.scale_pub.publish(scale)

    def spin(self):
        rospy.spin()

if __name__ == "__main__":
    c = CommandRepublisher()
    c.spin()