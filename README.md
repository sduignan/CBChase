# CBChase

I wanted to learn Unity, so I made a small 2D game as a starter project. The game is now done and can be found here:
https://stylised-wolf.itch.io/cheesed-borger-chase

The bulk of the logic is in the CBController script - which controls Cheesed Borger herself. Phsyics are handled by Unity, so CBController handles user input, when to pause, the current state for the animator variables, and where to reset to on death. It also interacts with the camera's CBFollower script, to tell it when to start and stop following Cheesed Borger, and whether death/winstate has occurred.

Other than that there's just a few small scripts for faking paralax using the texture offsets of the background layers, moving objects vertically and horizontally (separate scripts because I was lazy), a script to determine which screen overlay ought to be shown, and the countdown controller. Oh, and the table controller script & editor script, which made changing the size of the tables much easier - it allows the size of the centre of the table, positions of the side parts, and size & placement of the BoxCollider to be all set/changed with the one button.

It's a bit messy, and does a few things the 'wrong' way, but it all roughly works, so I'm calling it shippable. Time to move on to 3D!
