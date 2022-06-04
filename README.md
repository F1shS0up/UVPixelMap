# UVMapPixelArt
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