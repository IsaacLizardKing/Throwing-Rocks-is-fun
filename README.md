# Throwing-Rocks-is-fun
Game Name: Throwing Rocks is Fun

Intent: 

Narrative Structure: 

Mechanics: Move with "WASD" or the arrow keys. Interact with rocks using "E."

Aesthetics: Meant to be a calm, semi-monotonous game.

Joys: It felt good coding the rock throwing and picking up code and then seeing it function as I wanted.

Struggles: 

Audio Assets:
https://opengameart.org/content/chill-bgm 
https://pixabay.com/sound-effects/walking-sound-effect-272246/
https://opengameart.org/content/sfxthrow
https://opengameart.org/content/pick-up-item-yo-frankie

Visual Assets:
https://www.mixamo.com
https://polyhaven.com/a/rock_boulder_dry
https://www.poliigon.com/texture/flat-grass-texture/4585


What Andrew Did: Andrew implemented the player movement, the rock picking up and throwing mechanics, the rock and grass textures, the player animations, SFX, invisible walls, the score system, and the backbone of the Title scene.

What Colten Did: 

What Jauss Did:
Jauss worked tirelessly on the terrain, learning that Unity's terrain tools are outdated and incompatible with shader graphs, and resolved to create her own terrain, dynamically, aiming to impliment as many of the resolution features of the built in tools herself. She then worked on the shaders, making the terrain look somewhat presentable as ground. 

In all seriousness, the only leg up the custom terrain has over the built in terrain is that it is compatible with shader graphs. It is less performant, as (i think) the mesh sampling is done in parallel on the gpu for the built-in terrain, and the built in terrain also has seamless LOD and smooth sampling, which I simply did not have the time to impliment. 

Other than that, Jauss added a fullscreen pass shader to add distance fog, and fleshed out the Title scene.