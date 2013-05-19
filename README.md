beastmud
========

BeastMUD - A basic text based game framework in C#

The core Beast library implements the basic framework for running modules. Modules are a way to provide game functionality. Within the core library is an Application class. This class is the kernel for the game engine and handles processing of input and manging connections.

This is a work in progress but is currently functional. The goal is to make it hands free as possible to start but allow pieces to be swapped out, overridden or replaced for the purpose of your game or application. The core library contains the basics for running modules so it can host any kind of game.




Beast - Core library that contains the Application class, which is the primary entry point.

Beast.Hosting.Self - For running as a console app

Beast.Hosting.Web - For running from ASP.NET using SignalR (Application auto starts and auto sets up SignalR)

Beast.Components.Rpg - A text based RPG component that contains some logic around a role playing game

samples - Some smaple apps to demonstrate how to use the beastmud library

tools - Contains a mapping tool for use with the RPG component to build maps
