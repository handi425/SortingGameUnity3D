# Sorting Balls in Bottles

Game puzzle built with **Unity** where players must sort colored balls into bottles, following certain rules, to solve puzzles at various levels with increasing difficulty.

![Tangkapan Layar Game](https://github.com/handi425/SortingGameUnity3D/raw/main/Screenshot_3.png)

## Table of Contents

- [Features](#features)
- [Game Mechanics](#game-mechanics)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Project Structure](#project-structure)
- [Script Explanation](#script-explanation)
  - [GameManager.cs](#gamemanagercs)
  - [Bottle.cs](#bottlecs)
- [Customization](#customization)
  - [Setting Up Levels](#setting-up-levels)
  - [Adjusting Ball Spacing](#adjusting-ball-spacing)
- [Known Issues](#known-issues)
- [Contribution](#contribution)
- [License](#license)

---

## Features

- **Multiple Levels**: The game includes several levels with increasing difficulty.
- **Dynamic Scaling**: Bottle and ball sizes adjust based on the number of rows to ensure proportional display.
- **Responsive Design**: The game display adapts to various screen sizes and resolutions.
- **UI Elements**: Displays the current level, win message, and "Next Level" button.
- **Adjustable Spacing**: The spacing between balls in a bottle can be customized.

## Game Mechanics

- **Bottles**: Containers that can hold up to 4 balls.
- **Balls**: Objects with various colors that players need to sort.
- **Player Interaction**: Players can click bottles to select and move the topmost ball to another bottle, following certain rules.

**Rules:**

1. Only the topmost ball from a bottle can be moved.
2. Balls can only be placed on top of a ball with the same color or into an empty bottle.
3. Each bottle can hold a maximum of 4 balls.

## Getting Started

### Prerequisites

- **Unity Editor** (recommended version 2022.3.37f1.4 or newer)
- Basic knowledge of **C#** and the **Unity** interface

### Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/handi425/SortingGameUnity3D.git
   ```

2. **Open the Project in Unity:**

   - Open **Unity Hub**.
   - Click **Add** and select the cloned project directory.
   - Open the project.

3. **Set Up the Scene:**

   - Open the main scene (e.g., `MainScene.unity`) from the `Assets/Scenes` folder.

4. **Run the Game:**

   - Click the **Play** button in the Unity Editor to start the game.

## Project Structure

```
Assets/
├── Scripts/
│   ├── GameManager.cs
│   └── Bottle.cs
├── Prefabs/
│   ├── Bottle.prefab
│   └── Ball.prefab
├── Sprites/
│   ├── Bottle.png
│   └── Ball.png
├── Scenes/
│   └── MainScene.unity
└── UI/
    ├── Canvas/
    │   ├── LevelText
    │   ├── WinMessage
    │   └── PlayAgainButton
    └── Fonts/
        └── (Optional custom fonts)
```

## Script Explanation

### GameManager.cs

The `GameManager` class manages the game's main logic, including level creation, player input, and win conditions.

#### Key Methods:

- `Start()`: Initializes the game by creating the first level and setting up UI elements.
- `CreateLevel()`: Generates bottles and balls based on the current level, positions them in a grid, and adjusts their scale based on the number of rows.
- `OnBottleClicked(Bottle bottle)`: Handles bottle selection and ball movement between bottles.
- `MoveBall(Bottle fromBottle, Bottle toBottle)`: Validates and executes the movement of a ball from one bottle to another.
- `CheckWinCondition()`: Checks if the player has successfully sorted all balls and handles the win scenario.
- `NextLevel()`: Proceeds to the next level by regenerating game elements.
- `AdjustCamera(int totalRows)`: Adjusts the camera size to ensure all bottles are visible on the screen.

#### Important Variables:

- `public GameObject bottlePrefab;`: Reference to the bottle prefab.
- `public GameObject ballPrefab;`: Reference to the ball prefab.
- `public List<Color> ballColors;`: List of colors used for the balls.
- `public Button playAgainButton;`: Reference to the UI button for restarting or proceeding to the next level.
- `public TextMeshProUGUI levelText;`: Displays the current level.
- `public TextMeshProUGUI winMessage;`: Displays the win message after a level is completed.
- `private List<Bottle> bottles;`: List of bottle instances in the current level.
- `private Bottle selectedBottle;`: The bottle currently selected by the player.

#### Full Code of GameManager.cs:

```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject bottlePrefab;
    public GameObject ballPrefab;
    public List<Color> ballColors;

    public Button playAgainButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI winMessage;

    public int currentLevel = 1;
    public int maxLevel = 10;

    // Level options (number of colors)
    private int[] levelColors = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    private List<Bottle> bottles = new List<Bottle>();
    private Bottle selectedBottle = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Disable the play again button and win message at the start
        playAgainButton.gameObject.SetActive(false);
        winMessage.gameObject.SetActive(false);

        CreateLevel();
    }

    public void OnBottleClicked(Bottle bottle)
    {
        if (selectedBottle == null)
        {
            if (bottle.BallCount() == 0)
                return;

            selectedBottle = bottle;
            HighlightBottle(bottle, true);
        }
        else
        {
            if (bottle == selectedBottle)
            {
                HighlightBottle(bottle, false);
                selectedBottle = null;
            }
            else
            {
                MoveBall(selectedBottle, bottle);
                HighlightBottle(selectedBottle, false);
                selectedBottle = null;
            }
        }
    }

    private void MoveBall(Bottle fromBottle, Bottle toBottle)
    {
        if (toBottle.BallCount() >= 4)
            return;

        GameObject ballToMove = fromBottle.PeekBall();
        GameObject targetBall = toBottle.PeekBall();

        if (ballToMove == null)
            return;

        if (targetBall == null || ballToMove.GetComponent<SpriteRenderer>().color == targetBall.GetComponent<SpriteRenderer>().color)
        {
            fromBottle.RemoveBall();
            toBottle.AddBall(ballToMove);
            CheckWinCondition();
        }
    }

    private void HighlightBottle(Bottle bottle, bool highlight)
    {
        SpriteRenderer renderer = bottle.GetComponent<SpriteRenderer>();
        if (highlight)
            renderer.color = Color.yellow;
        else
            renderer.color = Color.white;
    }

    // Grid Parameters
    int columnsPerRow = 5; // Adjust as needed
    float xSpacing = 2f;
    float ySpacing = 3f;

    private void CreateLevel()
    {
        // Remove previous bottles and balls if any
        foreach (Bottle bottle in bottles)
        {
            // Remove all balls in the bottle
            foreach (GameObject ball in bottle.balls)
            {
                Destroy(ball);
            }
            bottle.balls.Clear();

            // Remove the bottle
            Destroy(bottle.gameObject);
        }
        bottles.Clear();

        // Determine the number of colors based on the current level
        int colorCount = levelColors[currentLevel - 1];

        // The number of bottles is the number of colors + 2 empty bottles
        int bottleCount = colorCount + 2; // Each color has 4 balls, and bottle capacity is 4

        int columnsPerRow = 5; // Adjust as needed
        float baseXSpacing = 2f;
        float baseYSpacing = 3f;

        // Calculate total rows and columns
        int totalColumns = Mathf.Min(columnsPerRow, bottleCount);
        int totalRows = Mathf.CeilToInt((float)bottleCount / columnsPerRow);

        // Calculate scaling factor based on the number of rows
        float bottleScaleFactor = 1f;
        if (totalRows == 1)
        {
            bottleScaleFactor = 1f;
        }
        else if (totalRows == 2)
        {
            bottleScaleFactor = 0.5f;
        }
        else
        {
            // For more than 2 rows, you can adjust the scaling factor as needed
            bottleScaleFactor = 1f / totalRows; // Simple example
        }
        // Set bottle size
        Vector3 bottleScale = new Vector3(bottleScaleFactor, bottleScaleFactor, 1f);

        // Adjust spacing based on the scaling factor
        float xSpacing = baseXSpacing * bottleScaleFactor * 2; // Multiply by 2 for sufficient spacing
        float ySpacing = baseYSpacing * bottleScaleFactor * 2;

        // Calculate grid size
        float gridWidth = (totalColumns - 1) * xSpacing;
        float gridHeight = (totalRows - 1) * ySpacing;

        // Starting position to center the grid
        float startX = -gridWidth / 2;
        float startY = gridHeight / 2 - ySpacing / 2;

        // Create bottles and place them in the grid
        for (int i = 0; i < bottleCount; i++)
        {
            int row = i / columnsPerRow;
            int column = i % columnsPerRow;

            float xPos = startX + column * xSpacing;
            float yPos = startY - row * ySpacing;

            GameObject bottleObj = Instantiate(bottlePrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            bottleObj.transform.localScale = bottleScale;
            Bottle bottle = bottleObj.GetComponent<Bottle>();
            bottles.Add(bottle);
        }

        // Adjust the camera to fit the grid
        AdjustCamera(totalRows);

        // Create balls
        List<GameObject> ballsList = new List<GameObject>();
        for (int i = 0; i < colorCount; i++)
        {
            Color color = ballColors[i % ballColors.Count];
            for (int j = 0; j < 4; j++)
            {
                GameObject ballObj = Instantiate(ballPrefab);
                ballObj.GetComponent<SpriteRenderer>().color = color;
                ballsList.Add(ballObj);
            }
        }

        // Shuffle balls
        Shuffle(ballsList);

        // Place balls into bottles
        int ballIndex = 0;
        foreach (Bottle bottle in bottles)
        {
            // Skip empty bottles
            if (ballIndex >= ballsList.Count)
                break;

            // Collect balls for this bottle
            List<GameObject> bottleBalls = new List<GameObject>();

            for (int i = 0; i < 4 && ballIndex < ballsList.Count; i++)
            {
                bottleBalls.Add(ballsList[ballIndex]);
                ballIndex++;
            }

            // Add balls to the bottle from bottom to top
            foreach (GameObject ball in bottleBalls)
            {
                bottle.AddBall(ball);
            }
        }

        // Update level text
        levelText.text = "Level " + currentLevel;

        // Disable win message
        winMessage.gameObject.SetActive(false);
    }

    private void AdjustCamera(int totalRows)
    {
        // Grid parameters (should be the same as used in CreateLevel)
        float ySpacing = 3f;

        // Adjust the orthographic size of the camera
        float requiredHeight = totalRows * ySpacing + 2f; // Additional space for UI
        Camera.main.orthographicSize = Mathf.Max(requiredHeight / 2f, 5f); // Minimum camera size 5

        // Ensure the camera is centered
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void CheckWinCondition()
    {
        bool isWin = true;
        foreach (Bottle bottle in bottles)
        {
            if (bottle.BallCount() == 0)
                continue;

            if (bottle.BallCount() != 4)
            {
                isWin = false;
                break;
            }

            // Get the color of the first ball (bottom-most)
            Color firstColor = bottle.balls[0].GetComponent<SpriteRenderer>().color;

            // Check each ball in the bottle
            foreach (GameObject ball in bottle.balls)
            {
                if (ball.GetComponent<SpriteRenderer>().color != firstColor)
                {
                    isWin = false;
                    break;
                }
            }

            if (!isWin)
                break;
        }

        if (isWin)
        {
            // Display win message
            winMessage.gameObject.SetActive(true);
            winMessage.text = "Congratulations! You completed Level " + currentLevel + "!";

            // Enable the Play Again button
            playAgainButton.gameObject.SetActive(true);

            // Change button text to "Next Level"
            playAgainButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Level";
        }
    }

    public void NextLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 1; // Return to level 1 if maximum level is reached
        }

        // Disable the Play Again button
        playAgainButton.gameObject.SetActive(false);

        // Create a new level
        CreateLevel();
    }

    public void RestartLevel()
    {
        // Disable the Play Again button
        playAgainButton.gameObject.SetActive(false);

        // Recreate the current level
        CreateLevel();
    }
}
```

### Bottle.cs

The `Bottle` class represents each bottle in the game and manages the balls it contains.

#### Key Methods:

- `OnMouseDown()`: Detects when the bottle is clicked and notifies the `GameManager`.
- `AddBall(GameObject ball)`: Adds a ball to the bottle and sets it as a child object.
- `RemoveBall()`: Removes the topmost ball from the bottle and returns it.
- `PeekBall()`: Returns the topmost ball without removing it.
- `BallCount()`: Returns the number of balls currently in the bottle.
- `UpdateBallPositions()`: Updates the positions of the balls within the bottle to ensure they are stacked correctly.

#### Important Variables:

- `public List<GameObject> balls;`: List of ball instances contained within the bottle.

#### Full Code of Bottle.cs:

```csharp
using UnityEngine;
using System.Collections.Generic;

public class Bottle : MonoBehaviour
{
    public List<GameObject> balls = new List<GameObject>();

    private void OnMouseDown()
    {
        GameManager.Instance.OnBottleClicked(this);
    }

    public void AddBall(GameObject ball)
    {
        balls.Add(ball); // Add ball to the end of the list
        ball.transform.SetParent(transform);
        UpdateBallPositions();
    }

    public GameObject RemoveBall()
    {
        if (balls.Count == 0)
            return null;

        int lastIndex = balls.Count - 1;
        GameObject ball = balls[lastIndex];
        balls.RemoveAt(lastIndex);
        UpdateBallPositions();
        return ball;
    }

    public GameObject PeekBall()
    {
        if (balls.Count == 0)
            return null;

        return balls[balls.Count - 1];
    }

    public int BallCount()
    {
        return balls.Count;
    }

    private void UpdateBallPositions()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            float yOffset = 0.5f; // Initial offset from the bottom of the bottle
            float ballHeight = 1f; // Height of the ball (matches the Y scale of the ball)
            float yPos = yOffset + i * ballHeight;
            balls[i].transform.localPosition = new Vector3(0, yPos, 0);
        }
    }
}
```

## Customization

### Setting Up Levels

To modify levels:

1. **Change the Number of Colors:**

   - In `GameManager.cs`, modify the `levelColors` array:

     ```csharp
     private int[] levelColors = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
     ```

   - Each element represents the number of colors for that level. Adjust these numbers to change the difficulty.

2. **Add New Levels:**

   - Increase the `maxLevel` variable and add corresponding entries in the `levelColors` array.

### Adjusting Ball Spacing

To set the spacing between balls within a bottle:

1. **Modify `UpdateBallPositions()` in `Bottle.cs`:**

   - Locate the `UpdateBallPositions()` method in `Bottle.cs`.

   - Adjust the `yOffset` and `ballHeight` values:

     ```csharp
     private void UpdateBallPositions()
     {
         float yOffset = 0.5f; // Adjust this value for initial offset
         float ballHeight = 1f; // Adjust this value for ball spacing

         for (int i = 0; i < balls.Count; i++)
         {
             float yPos = yOffset + i * ballHeight;
             balls[i].transform.localPosition = new Vector3(0, yPos, 0);
         }
     }
     ```

   - **Increase `ballHeight`** to add more space between balls.
   - **Decrease `ballHeight`** to reduce the space between balls.

## Known Issues

- **Balls Overlapping or Exceeding the Bottle**: If `ballHeight` is set too high, balls may exceed the bottle's boundaries.

  - **Solution**: Ensure that the total height of the balls and spacing does not exceed the bottle's height. Adjust `ballHeight` or the bottle's scale factor accordingly.

- **UI Elements Overlapping with Bottles**: On smaller screens, UI elements may overlap with game objects.

  - **Solution**: Adjust the `AdjustCamera()` method and UI anchor settings to ensure proper spacing.

## Contribution

Contributions are welcome! Please fork this repository and submit pull requests for improvements or fixes.

1. **Fork the Repository**

2. **Create a Feature Branch**

   ```bash
   git checkout -b feature/YourFeature
   ```

3. **Commit Your Changes**

   ```bash
   git commit -m "Add your feature"
   ```

4. **Push to the Branch**

   ```bash
   git push origin feature/YourFeature
   ```

5. **Open a Pull Request**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Note:** When sharing your project on GitHub, remember to exclude any proprietary assets or assets you do not have the rights to distribute. Use placeholders or free assets with appropriate licenses.

---

**Assets Used:**

- **Sprites:**
  - `Ball.png`
  - `Bottle.png`

Make sure to place these assets in the `Assets/Sprites/` directory.

---

If you have any questions or need further assistance, feel free to reach out. Happy coding!
