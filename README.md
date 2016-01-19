# KiCASS: Kinect-Controlled Artistic Sensing System


Please note that this guide is a work in progress; new things are being added every day. Thanks!

---

##### Intro

- [What is KiCASS?](#what-is-kicass)
- [Topology](#topology)

##### Performance Requirements

- [Performer](#performer)
- [Performance Space](#performance-space)
- [Sensor Positioning](#sensor-positioning)

##### KiCASS-desktop

- [Kinect Requirements](#kinect-requirements)
- [Download KiCASS-desktop](#download-kicass-desktop)
- [Using KiCASS-desktop](#using-kicass-desktop)
- [Troubleshooting](#troubleshooting)

##### KiCASS-patch

- [Max Requirements](#max-requirements)
- [Download KiCASS-patch](#download-kicass-patch)
- [Using the Patch](#using-the-patch)

##### Other

- [Team](#team)
- [Feedback](#feedback)
- [License](#license)

---

## Intro
#### What is KiCASS?
In simple terms, KiCASS is a system that:

- uses the [Microsoft Kinect v2 for PC](http://dev.windows.com/en-us/kinect/develop) to perform skeletal tracking of a performer (dancer, musician, etc.)
- packages and sends requested positional data as [OSC messages](http://archive.cnmat.berkeley.edu/OpenSoundControl/) to machines running [MaxMSP](https://cycling74.com/products/max/), for further audio manipulation

It was developed as a final year project by a team of [UBC ECE](http://www.ece.ubc.ca) students, in collaboration with the UBC Laptop Orchestra (also known as [UBC IMPART](http://www.ubcimpart.wordpress.com)), with the goal of adding the new Kinect sensor to the list of available motion capture technologies used by IMPART to create interactive audio-art installations/performances.

Learn more about UBC IMPART and get updates from the KiCASS team on the IMPART [blog](http://www.ubcimpart.wordpress.com)!

### Topology
The system is composed of three modules:

- *Kinect v2 sensor*
 - captures performer's skeletal movements and streams it to KiCASS-desktop

- *KiCASS-desktop*
 - Windows desktop program that interfaces with the sensor, grabbing the relevant positional data of the performer
 - interfaces with the Max patch, receiving requests and responding with the requested data in [OSC format](http://archive.cnmat.berkeley.edu/OpenSoundControl/).

- *KiCASS-patch*
 - Max patch that allows the user to remotely request and receive live OSC data from KiCASS-desktop

![kicass-topology](http://i.imgur.com/ISos9bh.png)

---

## Performerance Requirements
#### Performer

- *Clothing:* Tight-fitting clothes are ideal. Baggy clothing can obscure the skeleton of the performer.

#### Performance Space

- *dimensions:* Clear the area between the sensor and the performer (up to 15ft x 15ft)
- One performer: Ideal neutral position: 4 ½ ft
- Two performers: Ideal neutral position: 6 ft
- *environment:* Avoid positioning the sensor in direct sunlight or within 1 foot (0.3 m) of audio speakers.
- *interference:* If there’s interference from an external IR source, it will register as another body on the kinect.
- *heat:* The Kinect sensor needs room for the vents and fans to maintain an optimal operating temperature. Keep the area around your sensor free from clutter, and don't cover the vents.

*Something like this...*

![performance-space-image](http://i.imgur.com/sDJrKca.png)

#### Sensor Positioning

- *height:* The surface should be elevated above the performance floor height by 4ft.
- *surface:* Place your Kinect as close to the edge of a flat, stable surface as you are comfortable with, and make sure no other nearby objects are obstructing the field of view.
- *angle:* Angle the sensor so that it rests at 0 degrees, or slightly downward.
- *damage:* Be careful not to drop the sensor.
- *smudges:* Don't touch the front of the sensor when positioning it; the face of the Kinect sensor has two cameras (one VGA and one infrared camera), as well as IR sensors. They work best when the lens is free from smudges, so avoid touching the front of the sensor.

---

## KiCASS-desktop

#### Kinect Requirements

- Windows 8, 8.1, or 10*
- USB 3.0 port
- 4GB+ RAM
- DX11-capable graphics adapter
- [Kinect for Windows Runtime 2.0](https://www.microsoft.com/en-ca/download/details.aspx?id=44559)

**Unfortunately, Windows 7 is not supported by the latest Kinect runtime. Please refer to the [System Requirements](https://www.microsoft.com/en-ca/download/details.aspx?id=44559&e6b34bbe-475b-1abd-2c51-b5034bcdd6d2=True) for more details.*

#### Download KiCASS-desktop

- [download]() the latest stable release, or
- build the latest version using [Visual Studio](https://www.visualstudio.com/products/visual-studio-community-vs)

*Archived releases can be found in the Builds folder.*

#### Using KiCASS-desktop

- unzip and run the KiCASS executable.
- make sure the Kinect is powered on, and plug in the USB to your computer.
- KiCASS-desktop is now live, awaiting Max users to send a connection request.

![kicass-desktop-image](http://i.imgur.com/MsWsYDn.png)

#### Troubleshooting

[Here](https://support.xbox.com/en-US/xbox-on-windows/accessories/kinect-for-windows-v2-known-issues) are the most common hardware issues with the Kinect v2 sensor.

---

## KiCASS-patch

[Download](https://cycling74.com/products/max/) and [learn more](https://cycling74.com/wiki/index.php?title=Main_Page) about Max.

#### Max Requirements 

- Intel Mac (OS X 10.7 or later) OR Windows PC (Windows 7 or later)
- Multicore processor
- 2 GB RAM
- 1024×768 display

Full system requirements [here](https://cycling74.com/downloads/sys-reqs/#.Vp3eM1lyzR8).

#### Download KiCASS-patch

- [download]() the latest stable release

#### Using the Patch

- Unzip and import the patch into a new instance of Max. 
- Input the IP addresses of both computers into the corresponding lines (run "ipconfig" in cmd to find your IP)
- the default port number can be left as-is
- check "Metro" and "Display Data", then press "start

![KiCASS-patch-image](http://i.imgur.com/Tm1Q3uz.png)

---

## Other

#### Team

UBC ECE 2015-2016 Capstone team members:

- Isaac Cheng
- Kelsey Hawley
- Kevin Hui
- Michael Sargent
- Russil Glover

#### Feedback

Questions? Comments? [Send us your feedback](https://docs.google.com/forms/d/1tdtCNqIar6QnxPKrEYW6vaafCXlBPwTBE7qGl9ej7ow/viewform).

#### License

todo