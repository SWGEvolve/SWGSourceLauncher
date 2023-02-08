# SWGSourceLauncher
2/7/2023

SWGSource Game Launcher by Bandit aka Aphe Cloudstryder (Flurry)
Modified by RezecNoble (SWGEvolve)


Welcome to the SWGSource Launcher Project. This project is OPEN SOURCE with a couple of rules:

1. Please be sure to give credit where credit is due to those who have contributed to the project and those that have provided assistance to this project.

2. This project is still a work in progress and with my limited experience, I can only do so much to make this project work presently and in the future and due to that, this project was created by following a video tutorial. This project could use a lot of help and skills from experienced programers/coders, etc who have done game launcher projects like this before. Don't be afraid to dive in and see what you can do for the project. Share your ideas and experience with others.

3. Join our SWGSource discord at: https://discord.gg/Va8e6n8

4. And last but not least, our SWGSource Wiki Link: https://github.com/SWG-Source/swg-main/wiki

I would like to thank Tekaoh from SWGSource for giving me some ideas and testing out the launcher.

I would also like to thank the crew at SWGSource for all the hard work they are doing to re-build a game that everyone loved and enjoyed so much.

When it comes to the webbrowser on the launcher, you will also have to create an html page and host that page to get the browser to display the html page content.

You will need Visual Studio 2022 Community (free).

You will also need to Visual Studio Code (for webpage development for webbrowser within launcher) or if you are proficient in a basic html site any text editor will do.

Enjoy this project and May the Force be with you, Always!!!!

Things this project could use:

 - A file checker to check for corrupt files.

 - A Server Population and a Max record population.

Any Future Suggestions for the launcher are greatly appreciated.

Video Tutorial that was used to make this game launcher:
https://www.youtube.com/watch?v=tDopNcE9lOU

Tekaoh also has a wonderful link to his wiki to create a server using VirtualBox and at the bottom is a link to create a website using wordpress and his wordpress plugin (I made a barebones server following his guides and i absolutely love it):
https://tekaohswg.github.io/new.html

# Latest Changes

- Project builds natively in Visual Studio 2022
- Project is rebuilt using native WPF to reduce the number of dependencies and easier for people too get started
- Added the option to download files from zip files on your site (Can still use git with a flag)
- Added a version checker
- Added a .ini file for configuration options
- Added progress bar for downloads


# Guide

1) Create a version.txt file, start with 0.0.1

<p align="center">
  <img src="/screenshots/step1.jpg">
</p>


2) Select all the files in your game directory and pack as a zip. (Zipping the folder itself would create additional subdirectory)

<p align="center">
  <img src="/screenshots/step2.jpg">
</p>


3) Rename as Game.zip

<p align="center">
  <img src="/screenshots/step3.jpg">
</p>


4) Do the same thing for your patches (most people will only need to update their .tre file and occassionally the client exe)

<p align="center">
  <img src="/screenshots/step4.jpg">
</p>


5) Rename as Patch.zip

<p align="center">
  <img src="/screenshots/step5.jpg">
</p>


6) Upload these files to your website in a public html section that can be accessed by a URL. You can leave your base Game.zip alone and just continue to update the Patch.zip and version.txt everytime you have an update. Just overwrite the files so that your links remain the same.


7) You will add these URLs to your .ini file, you can also customize the homepage url and the patch notes url.

<p align="center">
  <img src="/screenshots/step6.jpg">
</p>


8) Make sure you package your .ini file with your game launcher. At this point you can zip up these files for distribution or you can create an installer package for the launcher.

<p align="center">
  <img src="/screenshots/step7.jpg">
</p>


9) If you rather keep patching with Git instead, set UseGit to True in the .ini file. In this case you will need to package the PortableGit folder AND UpdateSwgClient.bat with your Launcher. You must update the .bat file to point to your Git Repo.

<p align="center">
  <img src="/screenshots/git.jpg">
</p>


# Details

Nearly everything aside from the imaages and icons can be driven by the .ini file. They are many examples on creating new buttons for the launcher and how to collapse or make them visible at different stages of patching.

Everytime the launcher opens, it will check for the version.txt file. It there is no version.txt file then the launcher will download the Game.zip which holds the entire game. The update feature will automatically be called after this completes to check for patches. If the launcher finds a version.txt file when it first opens it will check to see if this file has changed, if the file has changed it will download the Patch.zip. If the file did not change, it is ready to launch the game.

<p align="center">
  <img src="/screenshots/launcher.jpg">
</p>