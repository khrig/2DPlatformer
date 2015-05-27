2DPlatformer
============

2d Platformer game



http://xnacoding.blogspot.se/2013/12/entity-component-system.html
 
Could do more collision detection with the sample on dropbox,
PlatformerWithRamps.zip in V2 map

TODO:
* Larger maps (more than one screen)
* Camera
* Create a small map (but larger than one screen) for reference (if procedural generation)
* Procedural generation
 - Start with rooms and combine them (http://tinysubversions.com/spelunkyGen/)
 - Generate a room
 - Combine generated rooms
* Animation (sprites)
* Gamepad control
* Ramps (for the map)
* TileEditor needs to set that the tile is sloped and what direction, also what kind of slope (start with 45 degrees)
* TileEditor - Layer type (which is solid)
 - Fixed layer types (http://www.gamedev.net/page/resources/_/technical/game-programming/procedural-level-generation-for-a-2d-platformer-r3794)
   - Background ? multiple images
   - Background objects
   - Collision
   - Foreground
* Smaller hitbox for player (then sprite size)
* Backgrounds (Parallax support)
* Edit backgrounds in TileEditor
* Player shoot
* Enemies
* Player dies if hitting enemy (first version)
* Enemy dies if hit two times (first version)
* Player wins if reach end (collision with specific tile)
* Game over state
   - With menu to exit or go to main menu
* Pause state
