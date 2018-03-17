# IngeBot (deprecated)
## WARNING! This program is outdated, as well as the code in this repository. However, you can have a look at [Angela](https://github.com/IngeniousA/Angela), the successor of IngeBot.
**Bot for Discord written with C# and Discord.Net**
### How to setup IngeBot?
1. Download and run `IngeBot_Installation.exe`
2. Install IngeBot into some folder.
3. `config.txt` will be opened. What's in it?
```
<TOKEN>
<BOT INTERACTION ROLE ID>
<TURN SOUNDBOARD ON? (1 - YES, 0 - NO)>
<SOUNDBOARD ROLE ID>
<SOUNDBOARD DIRECTORY>
<SOUNDBOARD SIZE>
<SOUNDBOARD LIST FILE PATH>
```
Replace `<TOKEN>` with your server's bot app token. How to get it? Read [here](https://github.com/reactiflux/discord-irc/wiki/Creating-a-discord-bot-&-getting-a-token)
Replace `<BOT INTERACTION ROLE ID>` with Moderator role ID. How to get role ID? Type '\' in chat followed by @role. You must be able to mention the role.
Replace `<TURN SOUNDBOARD ON? (1 - YES, 0 - NO)>` with 1 if you want to turn on SoundBoard on your server, else type 0.
Replace `<SOUNDBOARD ROLE ID>` with SoundBoard users' role ID.
Replace `<SOUNDBOARD DIRECTORY>` with path to folder with SoundBoard sounds.
Replace `<SOUNDBOARD SIZE>` with size of SoundBoard items.
Replace `<SOUNDBOARD LIST FILE PATH>` with path to text file where SoundBoard items are written.

4. Save and close `config.txt`.
5. Run `IngeBot.exe`. It will be runned on your server.

Read about how to use IngeBot commands in IngeBot's wiki (English).
