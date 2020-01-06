# TWitcher3
---------
A Twitch Integration with Witcher 3 allowing viewers to use console commands directly from chat.

---------

### Setup

#### Game Requirements

You will need the GOG Galaxy version of the game, and game version 1.32 along with the following DLCs installed:
* Free DLC Program (16 DLC)
* Hearts of Stone
* Overlay

This has not been tested with the Steam version nor Blood and Wine. There are no guarantees that it will work. I might update it in the future as I dont have Blood and Wine. nor the steam version.


#### Mod requirements
You will need to install the provided mod from this repository, You will find it inside your misc folder. Just copy the ```misc\Witcher3 Mod\mods``` into your root installation folder for Witcher 3. It will look something like 

```c:\program files (x86)\The Witcher 3 Wild Hunt\mods```

Other than that no particular mod is required for this to run, however. You will need to enable the console for your game. 

Open the file
```c:\program files (x86)\The Witcher 3 Wild Hunt\bin\config\base\general.ini```

and add the following line at the end of the file

```DBGConsoleOn=true```


#### Bot setup

To set up the bot, you will need to make sure you have a Twitch account with 2FA enabled. This will allow you to generate a Twitch Access Token [https://twitchtokengenerator.com/](https://twitchtokengenerator.com/)

Click on Bot Chat Token, to get your token.


Then update your settings.json file accordingly

```JSON

{
  "twitchBotUsername": "bot username",
  "twitchBotAuthToken": "bot access token",
  "twitchChannel":  "channel name"
}

```

Then you're pretty much set! You can run TWitcher3.exe and have fun :) Make sure you run the game before you run the bot though, it might crash during the game startup otherwise.

----

### Interacting with the bot
To interact with the chat bot, you can use a couple of different commands available for you.

    !w3 help - will give you a link for available ingame commands
               there are a couple custom commands not in that list
               please check below.

    !w3 help COMMAND - will show a help message and credit cost for
                       that particular command               

    !w3 COMMAND - executes the command :3

    --- Unlisted commands from the !w3 help
    
    !w3 setplayerscale X Y Z
    !w3 setscale X Y Z

    More commands will be available in the future, as a lot more
    exists in the ConsoleExtensions.ws file that is not enabled 
    from within the bot


### Command Credits

When people use console commands, those commands will use credits. These credits are meant for making sure that people dont spam too much commands. Right now all commands cost the same and credits cannot be earned over time. its something I will add in the future.

Right now to add more credits to a viewer, you will have to use the command

    !credits add USERNAME AMOUNT    

Other credits commands

    !credts - shows the current amount of credits



--------

Clone or fork the repo, have fun with it!
If you want to help improving this bot, please do a fork and pull request! its always welcome :)