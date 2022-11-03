# Alien Invasion

Alien Invasion is a 2D space shooter where the player has to defend earth from alien invaders. It is a clone of Space Invaders and the debut game of Starforge Games, a non-commercial organization of hobby enthusiasts with the drive to make games in their spare time.

The game is entirely written in C#, using DirectX 10 with [SlimDX](http://slimdx.org/) and a self-made rendering engine, asynchronous resource management, the [FMOD audio engine](http://www.fmod.org/) and utilizes a component-based game architecture. We even have a little LISP interpreter for our resource files! (yes, we are totally aware that this is absolute overkill for a small game like that. But as I already said before this wasn't about the game, it was about the experience and trying stuff we wanted to do :) )

## Requirements

In order to run Alien Invasion, you need the [.NET Framework 4.6](http://www.microsoft.com/net). Additionally, you may need to install the [SlimDX End User Runtime for .NET 4.0](http://slimdx.org/download.php), available on the official SlimDX website in the download section.

## Build

To build and run the source code, you need [Visual Studio 2015](http://www.visualstudio.com/). In addition, you have to install the [SlimDX Developer SDK](https://slimdx.org/download.php).

When all prerequisites have been installed, open the `AlienInvasion.sln` file in it and compile one of the build targets.

## Credits

I would like to express my sincerest gratitude to all people who contributed to this project. These are, in no particular order:

- [Patrick Bader](http://www.patrickbader.eu/)
- [Daniel Kwast](http://www.affectit.de/)
- [Isolde Scheurer](http://isolde.deviantart.com/)
- [Benjamin Wiedmann](http://www.wied.it/)
- Christian Seitzer

My special thanks goes to Hansheinz Müller Philipps Sohn for providing the server infrastructure to host the development process.

## License

The source code is licensed under the [MIT License](https://opensource.org/licenses/MIT) and all other assets are licensed under a [Creative Commons Attribution-NonCommercial 4.0 International License](http://creativecommons.org/licenses/by-nc/4.0/).
