# UVPixelMap
This is a library for **monogame** which is used to make **UV maps**.

## Why should i get this?
Because you can edit your character at runtime without creating more and more different sprites for each situation.
**For example:** Your character will be wet from water. Normally you would need to add completly new spritesheet with new colors, but thanks to this library you need to just change the UV map and it will change all of the sprites

## How does it work?
It is hard to explain but i will try my best.
So you will create UV map that will be the same size as your player, each pixel needs its unique color.
After that you will create new sprite sheet you want to use, and set the colors to the responding color in the UV map.
Then you will use my library to generate new sprite sheet with colors that will have R and G as position on the UV map.
Finally you can use any new or updated UV map and it will change the full sprite sheet.

Look at this video for more explanation: https://www.youtube.com/watch?v=HsOKwUwL1bE
## How can i use it?
Download:https://milkshake-games.itch.io/uvpixelmap
1. Look at the video above.
2. In this time https://youtu.be/HsOKwUwL1bE?t=263 you can see how the process is made
3. Open your project
4. In the top type using UVPixelMap
5. Create new Source variable
6. In load content create new 2 variables UVMap and Overlay and immediately asign them to new for example:UVMap example = new UVMap(Content.Load<Texture2D>("name"), 32);
7. There asign source using Fuctions.CreateSource(UVMap you just created, Overlay you just created, graphics device)
8. each time you want to change tme uvmap just say yourSource.ChangeUVMap()
9. Bonus: If you have sprite sheet of uv maps you can use different function called decrypt which turns sprite sheet into smaller uv maps.
  
![img](file:///C:/Users/valer/Videos/Captures/ShooterGame%20-%20Microsoft%20Visual%20Studio%2006.06.2022%2022_07_04_LI.jpg)

