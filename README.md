# Mixed_Reality_Accessible_Control

## Project website:
[https://zhangganlin.github.io/Holo-Spot-Page/index.html](https://zhangganlin.github.io/Holo-Spot-Page/index.html)

## Set Up

! remember to remove the password when publish the source code

! the Unity version is really important (recommend `3.40`)

### Spot Robot Connection Using WSL2

1. Connect to the WIFI `CVG_5GHz-2`, the password is: `ka3u1nfkr977`

2. Use WSL2, connect the spot robot `ssh -X cvg-nuc-1@192.168.1.243`, the password is: `nuc1`

3. Launch

   ```shell
   source spot_ws/devel/setup.bash
   
   roslaunch spot_driver driver.launch hostname:=192.168.50.3 username:=admin password:=ka3u1nfkr977 
   
   # another terminal
   roslaunch spot_viz view_robot.launch
   ```

### Build Unity Project and Deploy to the Hololens2

* **Please don’t move the project folder once you create it!!**

* Basic packages [Tutorial](https://learn.microsoft.com/en-us/training/modules/learn-mrtk-tutorials/1-1-introduction)
   * Make sure the unity version is consist. 
   * we need to connect the Hololens2 to the USB and use the remote setting. 
   
* Eye tracking:
  
   ```c#
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;
   using Microsoft.MixedReality.Toolkit;
   using UnityEngine.Events;
   
   public class eyegaze : MonoBehaviour
   {
       // Start is called before the first frame update
       void Start()
       {
           
       }
   
       // Update is called once per frame
       void Update()
       {
           var p = CoreServices.InputSystem.EyeGazeProvider.GazeOrigin +
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized * 1f;
   
           p = new Vector3(1f, 1f, 1f);
   
           gameObject.GetComponent<Transform>().position = p;
   
       }
   }
   ```
   
   * [Tutorial](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/features/input/eye-tracking/eye-tracking-basic-setup?view=mrtkunity-2022-05)
   * [Change add in](https://stackoverflow.com/questions/73116543/hololens-2-unity-app-rarely-renders-anything-to-the-screen) (no need of this in new unity version)
     * For a successful build it seems as though XR Plug-in Management > Windows Mixed Reality > UWP > **Use Primary Window** should be checked **on**. Seems weird since I'm using OpenXR anyways, but this might help someone so thought I'd update. Works most of the time now - needs more testing.
   * Remember to add OpenXR XRSDK Eye Gaze Provider in Input Data Providers
   * Remember to remove the collider for the cursor
   
* Visual Anchor
   * [Tutorial](https://learn.microsoft.com/en-us/azure/spatial-anchors/tutorials/tutorial-new-unity-hololens-app?tabs=azure-portal)
   * [Cannot change camera setting](https://github.com/microsoft/MixedRealityToolkit-Unity/issues/3948)
      * You need to edit the Camera Profile Settings instead of directly setting up the Camera.
         - edit the MixedRealityToolkit
         - access the Caemra Settings from here (you can clone the Camera Profile if you want to change it)
   * Creating script
      * **Account ID**: 433b2918-4bc3-4103-88fb-ac95d2854393
      * **Account Key**: zn7cf5meq5zNxJxLTtJTaUDAphikx8nii71VSGhVssQ= 
      * **Account Domain**: westeurope.mixedreality.azure.com
      * **s key**: BcwR62Nnf6b04wx5cQ8NRx2xGVLzhqak1n2KRCVHYj4=
   * `AzureSpatialAnchors` should be placed under `MixedRealityPlayspace`
   * AR session origin: Need to set the camera to the main camera.

### Anchor for ROS

[Tutorial](https://github.com/microsoft/azure_spatial_anchors_ros)

[Coordinate Transform](https://github.com/EricVoll/spot-mr-core)

### Unity Robotic packages

This page provides brief instructions on installing the Unity Robotics packages. Head over to the [Pick-and-Place Tutorial](pick_and_place/README.md) for more detailed instructions and steps for building a sample project.

1. Create or open a Unity project.

    > Note: If you are adding the URDF-Importer, ensure you are using a [2020.2.0](https://unity3d.com/unity/whats-new/2020.2.0)+ version of Unity Editor.

1. Open `Window` -> `Package Manager`.

1. In the Package Manager window, find and click the `+` button in the upper left-hand corner of the window. Select `Add package from git URL...`.

    ![packman](images/packman-1666875202041.png)

1. Enter the git URL for the desired package. Note: you can append a version tag to the end of the git URL, like `#v0.4.0` or `#v0.5.0`, to declare a specific package version, or exclude the tag to get the latest from the package's `main` branch.
   1. For the [ROS-TCP-Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector), enter `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector`.
   1. For the [URDF-Importer](https://github.com/Unity-Technologies/URDF-Importer), enter `https://github.com/Unity-Technologies/URDF-Importer.git?path=/com.unity.robotics.urdf-importer`.

1. Click `Add`.

To install from a local clone of the repository, see [installing a local package](https://docs.unity3d.com/Manual/upm-ui-local.html) in the Unity manual.

