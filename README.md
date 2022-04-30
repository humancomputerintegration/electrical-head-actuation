# Electrical Head Actuation: Enabling Interactive Systems to Directly Manipulate Head Orientation

This is the repository for software (Unity) pipeline of "Electrical Head Actuation: Enabling Interactive Systems to Directly Manipulate Head Orientation" paper (ACM CHI2022).

## source codes

In order to make our system work with your Unity project, you basically have to put the following prefab objects in your scene (all in the Electrical Head Actuation folder in our Assets):

- EHA Camera
- EHA 
- Static or/and trajectories according to your usage

Please refer to our SampleScene for figuring out how to link scripts/GameObjects with each other.

### stimulator (work-in-progress)

Currently, our software pipeine is only compatible with a medical-grade electrical stimulator, Rehamove3.
We are currently working on releasing an open-source electrical stimulator and making this project compatible with it; so please stay tuned!

Also, if you have Rehamove3 and want to learn more about how to control its stimulation output from your laptop (Unity, python), you should check the project page of our open-source Rehamove3 library: https://github.com/humancomputerintegration/rehamove-integration-lib

## citing

When using or building upon this device in an academic publication, please consider citing as follows:

Yudai Tanaka, Jun Nishida, and Pedro Lopes. 2022. Electrical Head Actuation: Enabling Interactive Systems to Directly Manipulate Head Orientation. In CHI Conference on Human Factors in Computing Systems (CHI ’22), April 29–May 05, 2022, New Orleans, LA, USA. ACM, New York, NY, USA, 15 pages. https://doi.org/10.1145/3491102.3501910

## contact

For any questions about this repository, please contact yudaitanaka@uchicago.edu

