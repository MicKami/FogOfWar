# FogOfWar
This Unity project is an implementation of a popular gameplay mechanic found in many strategy games.
Inspired by Riot Games [blog](https://technology.riotgames.com/news/story-fog-and-war) post.

**WebGL [DEMO](https://mickami.itch.io/fogofwardemo)** (*No download required*).
## Overview
**General implementation:**
The method consist of several steps.
*  Generating low resolution visibility information that is stored on the CPU and used to interact with the game
* Upscaling the results x4 on the shader
* Bluring the upscaled image
* Rendering it to the screen

**More details:**
* The underalying data is stored using C# `Hashset<Vector2Int>` for efficient `Contains()` calls, deduplication and set operations such as `UnionWith` and `Intersect`.
* To generate visiblity information we use **Bresenham's line algorithm** foreach cell in the visibility radius outline.
*  From the visibility information we create low resolution binary texture that is then being sent to an upscaling shader which uses lookup texture to identify upscaling pattern. (As described in the [blog](https://technology.riotgames.com/news/story-fog-and-war) post).
* Next the high resolution output is being blurred using separable Gaussian filter with linear sampling.
* The final result is rendered on a flat plane with transparent shader.
## Credits
Riot Games blog post:
 (https://technology.riotgames.com/news/story-fog-and-war).
 
Separable Gaussian blur with linear sampling:
  https://www.rastergrid.com/blog/2010/09/efficient-gaussian-blur-with-linear-sampling/
  
Rounded corners shader for Unity UI:
 https://github.com/kirevdokimov/Unity-UI-Rounded-Corners.

## License
 [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


