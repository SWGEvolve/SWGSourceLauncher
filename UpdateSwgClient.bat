@ECHO OFF
IF EXIST .\.git\ (
ECHO Looking for updates...
.\PortableGit\bin\git.exe pull
) ELSE (
ECHO New Client Detected! Initializing...
.\PortableGit\bin\git.exe init .
.\PortableGit\bin\git.exe config pull.rebase false
.\PortableGit\bin\git.exe remote add -f origin https://github.com/SWG-Source/client-assets.git
.\PortableGit\bin\git.exe checkout master
)
PAUSE