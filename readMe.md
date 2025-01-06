# Game Controls Instructions
## Movement
- Use the following keys to move your character:
  - **W**: Move forward
  - **A**: Move left
  - **S**: Move backward
  - **D**: Move right

## Jumping
- Press **Spacebar** to make your character jump.

## Camera Rotation
- Move the **Mouse** to rotate the camera and change your view.
  - **Move mouse left**: Rotate camera left
  - **Move mouse right**: Rotate camera right
  - **Move mouse up**: Look up
  - **Move mouse down**: Look down  

## Other Controls
- Press the **P** key to bring up the pause menu.
  - **Continue**: Can be used to resume the game
  - **Main Menu**: Can be used to go back to the Main Menu

<br/><br/>

# Game Design 

## Gameplay
The game is centred around a drone, which is sent to an ancient underwater temple to find a lost treasure. We play as the drone and have to collect gold coins scattered around the levels to proceed to the next one, to eventually reach the treasure. Each level introduces different types of enemies controlled by AI which the player needs to avoid.

Along with this, we have created different types of platforms, namely, `One Time Use`, `Moving`, `Jump Pad` and `Static` which the player can use to navigate the levels and avoid the enemies. The floor of the temple is covered in swords, hence, if the player falls then the player instantly dies.

We have also introduced the concept of lives, where the player has a set of number of lives in which the game needs to be completed and when the player runs out of lives, the player starts from the Main Menu again.

Along with this, we also implemented a difficulty system, which introduces two difficulties that can be set by the player. Easy mode results in the player receiving a guide throughout the game, lesser enemy damage, higher number of lives in which the game can be completed in, and lesser enemy damage in the last level. Whereas for Hard mode, it is the complete oppopsite.
