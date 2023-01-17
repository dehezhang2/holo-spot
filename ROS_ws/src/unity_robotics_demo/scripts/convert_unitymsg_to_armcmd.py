import rospy
from geometry_msgs.msg import PoseStamped, TransformStamped, Pose, Quaternion
from unity_robotics_demo_msgs.msg import PosRot, Id
import tf2_ros as tf
from std_msgs.msg import String
from sensor_msgs.msg import JointState
from spot_msgs.srv import HandPose,ArmJointMovement
import tf as tfq


import math
 
def euler_from_quaternion(x, y, z, w):
        """
        Convert a quaternion into euler angles (roll, pitch, yaw)
        roll is rotation around x in radians (counterclockwise)
        pitch is rotation around y in radians (counterclockwise)
        yaw is rotation around z in radians (counterclockwise)
        """
        t0 = +2.0 * (w * x + y * z)
        t1 = +1.0 - 2.0 * (x * x + y * y)
        roll_x = math.atan2(t0, t1)
     
        t2 = +2.0 * (w * y - z * x)
        t2 = +1.0 if t2 > +1.0 else t2
        t2 = -1.0 if t2 < -1.0 else t2
        pitch_y = math.asin(t2)
     
        t3 = +2.0 * (w * z + x * y)
        t4 = +1.0 - 2.0 * (y * y + z * z)
        yaw_z = math.atan2(t3, t4)
     
        return roll_x, pitch_y, yaw_z # in radians


class CommandRepublisher():
    def __init__(self):
        rospy.init_node("hololens_headmotion")

        rospy.Subscriber("/hololens/arm_pos_rot", PosRot, self.received_cmd)
        rospy.Subscriber("/hololens/arm_status", Id, self.activate_slow)
        rospy.Subscriber("/hololens/reset_arm", Id, self.reset_arm)
        rospy.Subscriber("/joint_states",JointState,self.update_joint_state)
        self.cmd_feedback_pub = rospy.Publisher("/hololens/feedback", String, queue_size=10)
        self.arm_cmd_pub = rospy.Publisher("/hololens/arm_cmd", PoseStamped, queue_size=10)
        self.hololens_frame = "hololens"
        self.slow = False
        self.update_hand_offset = False
        self.hand_offset = 0
        rospy.wait_for_service('/spot/gripper_pose')
        rospy.wait_for_service('/spot/arm_joint_move')
        
    def reset_arm(self,msg):
        if msg.identifier == "reset arm":
            self.hand_offset = 0

    def update_joint_state(self,js):
        self.js = js.position[-7:-1]
        if self.update_hand_offset:
            self.hand_offset = self.js[-1]+self.js[-3]
            self.update_hand_offset = False
        
    
    def activate_slow(self,cmd):
        if cmd.identifier == "rotate hand":
            self.slow = True
        elif cmd.identifier == "stop rotate hand":
            self.slow = False
    
    def received_cmd(self, cmd):
        # rospy.loginfo("received command!")
        self.cmd_feedback_pub.publish("Received arm command ")

        rospy.sleep(0.05)
        
        roll_x, pitch_y, yaw_z = euler_from_quaternion(cmd.rot_x,cmd.rot_y,cmd.rot_z,cmd.rot_w)
        
        if self.slow:
            
            arm_move = rospy.ServiceProxy('/spot/arm_joint_move', ArmJointMovement)
            js_temp = self.js
            
            if roll_x > 10.0*math.pi/180.0:
                js_temp = list(js_temp)
                js_temp[-1] += 10*math.pi/180.0
                arm_move(js_temp)
                self.update_hand_offset = True
            elif roll_x < -10.0*math.pi/180.0:
                js_temp = list(js_temp)
                js_temp[-1] -= 10*math.pi/180.0
                arm_move(js_temp)
                self.update_hand_offset = True
            

        else:
            roll_x += self.hand_offset
            target_pose = Pose()
            target_pose.position.x = (cmd.pos_x-0.3)*1+0.7
            target_pose.position.y = cmd.pos_y*1 
            target_pose.position.z = (cmd.pos_z-0.4) 
            quaternion = tfq.transformations.quaternion_from_euler(roll_x, pitch_y, yaw_z)
            # print(quaternion)
            # exit()
            target_pose.orientation = Quaternion(*quaternion)
            # target_pose.orientation.x = cmd.rot_x
            # target_pose.orientation.y = cmd.rot_y
            # target_pose.orientation.z = cmd.rot_z
            # target_pose.orientation.w = cmd.rot_w

            # if (abs(target_pose.position.x) > 0.5 or abs(target_pose.position.y) > 0.5 or abs(target_pose.position.z) > 0.5):
            #     return


            visualize_pose = PoseStamped()
            visualize_pose.pose = target_pose
            visualize_pose.header.frame_id = "body"

            self.arm_cmd_pub.publish(visualize_pose)
            
            try:
                gripper_pose = rospy.ServiceProxy('/spot/gripper_pose', HandPose)
                gripper_pose(target_pose,0.5)
                self.cmd_feedback_pub.publish("Success; Call /spot/gripper_pose service")

            except rospy.ServiceException as e:
                print("Service call failed: %s"%e)

    def spin(self):
        rospy.spin()

if __name__ == "__main__":
    c = CommandRepublisher()
    c.spin()