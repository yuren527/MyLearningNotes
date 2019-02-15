# Console Command And Command-Line Argument
Put `Exec` in the `UFUNCION()` parameters to make a function console command;
```C++
UFUNCTION(Exec)
void Host(){
...
}
```
`Exec` compatible classes:
> PlayerControllers  
> Possessed Pawns  
> HUDs  
> Cheat Managers  
> Game Modes  
> Game Instances  
# Command-Line Argument
Often used argument:
> -game   //Launch the game using uncooked contents  
> -server //Launch the game as server using uncooked contents  

*Example:*  
> MyGame.exe /Game/Maps/MyMap  
> UE4Editor.exe MyGame.uproject /Game/Maps/MyMap?game=MyGameInfo -game  
> UE4Editor.exe MyGame.uproject /Game/Maps/MyMap?listen -server  
> MyGame.exe 127.0.0.1  

For more informations, see the [Command-Line Arguments](https://docs.unrealengine.com/en-US/Programming/Basics/CommandLineArguments)
