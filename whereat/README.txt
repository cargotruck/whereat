whereat -- a Windows utility designed to locate what workstation a user is logged into.
Written by Nicholas Flesch: nicholas.flesch@outlook.com
Copyright 2020

This utility contacts the Active Directory server to find a user's username as well as 
to gather a list of devices in the domain.

[INSTALL]
There is no installer for this program.

Prerequesits:
	Microsoft .NET Core 3.1 or higher.
	
Copy the 'whereat' directory to a desired location on the computer; C:\utilities, for example.

Add the path to the whereat directory to the computer's environmental variables:
	This PC > Properties > Advanced system settings > Environmental Variables
	> System variables > [highlight] Path > Edit > New > [enter] whereat directory path

After adding whereat to the environmental variables it can be used from cmd.

[USE]
Usage: whereat [OPTIONS]+
Searches for a given user, or user with name like one provided, on network
computers.

Options:
  -u, --user=VALUE           Takes a username or a search request such as one
                               of the following:
                               John*
                               Jone Doe
                               *John Doe*
  -c, --computer=VALUE       Takes a computer or a search request such as: SOO-
                               D-*
  -d, --domain=VALUE         Takes a domain, such as mydomain.com
  -h, --help                 show this message and exit

