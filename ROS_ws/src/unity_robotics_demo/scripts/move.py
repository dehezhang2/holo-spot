#!/usr/bin/env python3

import rospy
from math import cos, sin, sqrt, pi
from geometry_msgs.msg import PoseStamped
from unity_robotics_demo_msgs.msg import PosRot,Id
from std_msgs.msg import Duration, String
from std_msgs.msg import Duration, Float32
import tf


class CommandExecuter():
    def __init__(self):
        rospy.init_node("command_executer")
        rospy.Subscriber('/command/processed', PoseStamped, self.received_cmd)
        rospy.Subscriber("/hololens/spin", Id, self.received_spin)
        rospy.Subscriber("/hololens/scale", Float32, self.received_scale)
        
        self.tf_listener = tf.TransformListener()

        # rospy.loginfo("spot/trajecotry server is online. Ready to take commands.")
        self.cmd_feedback_pub = rospy.Publisher('command/feedback', String, queue_size=10)
        self.cmd_pub = rospy.Publisher("/spot/go_to_pose", PoseStamped, queue_size=1)
        self.rate = rospy.Rate(1)
        self.scale = 0.8
    def received_scale(self,cmd):
        self.scale = cmd.data
    def received_spin(self,cmd):
        pose = PoseStamped() 
        if cmd.identifier == "spin left":
            pose.header.frame_id="body"
            pose.pose.position.x=0
            pose.pose.position.y=0
            pose.pose.position.z=0
            pose.pose.orientation.x=0
            pose.pose.orientation.y=0
            pose.pose.orientation.z=0.707
            pose.pose.orientation.w=0.707
            self.cmd_pub.publish(pose)
        elif cmd.identifier == "spin right":
            pose.header.frame_id="body"
            pose.pose.position.x=0
            pose.pose.position.y=0
            pose.pose.position.z=0
            pose.pose.orientation.x=0
            pose.pose.orientation.y=0
            pose.pose.orientation.z=0.707
            pose.pose.orientation.w=-0.707
            self.cmd_pub.publish(pose)
        
    def received_cmd(self, data):
        self.cmd_feedback_pub.publish(String("Received command"))
        pose_in_body = self.tf_listener.transformPose("body",data)
        
        if self.scale < 0:
            x = pose_in_body.pose.position.x
            y = pose_in_body.pose.position.y
            dist = sqrt(x**2+y**2)
            dist_new = max(0.0,dist-1.0)
            s = float(dist_new)/float(dist)
            pose_in_body.pose.position.x = s*x
            pose_in_body.pose.position.y = s*y;
            pose_in_body.pose.position.z = 0
        else:
            pose_in_body.pose.position.x *= self.scale
            pose_in_body.pose.position.y *= self.scale
            pose_in_body.pose.position.z = 0
        
        dy = pose_in_body.pose.position.y
        dx = pose_in_body.pose.position.x
        
        sinTheta = dy/sqrt(dy**2+dx**2)
        cosTheta = dx/sqrt(dy**2+dx**2) 
        sinHalfTheta = sqrt((1-cosTheta)/2)
        if sinHalfTheta*sinTheta < 0:
            sinHalfTheta = -sinHalfTheta
        cosHalfTheta = sqrt(1-sinHalfTheta**2)
        
        pose_in_body.pose.orientation.x = 0
        pose_in_body.pose.orientation.y = 0
        pose_in_body.pose.orientation.z = sinHalfTheta
        pose_in_body.pose.orientation.w = cosHalfTheta
        
        if (abs(pose_in_body.pose.position.x) < 0.3 and abs(pose_in_body.pose.position.y) < 0.2):
            return
            
        self.cmd_pub.publish(pose_in_body)
            # self.rate.sleep()
        
        
        
    def command_finished(self):
        self.cmd_feedback_pub.publish("Executed command")

    def spin(self):
        rospy.spin()


if __name__ == "__main__":
    e = CommandExecuter()
    e.spin()
