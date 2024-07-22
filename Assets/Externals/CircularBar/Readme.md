# Circular Bar Documentation

## Introduction

CircularBar is a custom UI element developed for Unity UI Toolkit. It represents a circular progress bar that can be easily integrated into your Unity projects.

At its core, CircularBar utilizes a combination of a Shader Graph named "CircularBar" and a corresponding CircularBar class that contains the logic behind its functionality. The Shader Graph is responsible for rendering the circular progress visual, while the CircularBar class handles the dynamic updating of the progress.

To use CircularBar, you simply need to include the provided files in your Unity project and configure them accordingly. You can then integrate CircularBar into your UI layouts using UXML files and apply styles using USS files.

Once added to your UI, CircularBar can be updated dynamically either through the UI Builder or programmatically via code. This allows you to control the progress of the circular bar in real-time, making it suitable for various applications such as loading screens, health bars, or any other progress tracking needs in your Unity project.

## Usage

### Example Files

In the Example folder, you will find:

- `main.uxml`: Demonstrates how to integrate CircularBar into your UI using UXML.
- `Main.uss`: Contains the necessary styles for CircularBar.
- `SceneSampleScene.unity`: Contains the scene with the UIDocument configured with main.uxml to see it in runtime

## Updating CircularBar Progress

While you can configure some properties of CircularBar using the UI Builder (manipulating the `fill` propery), its true potential lies in runtime manipulation through code.

CircularBar inherits from VisualElement, which means it doesn't have a Transform and can't be accessed using GetComponent. Instead, you can use Q<CircularBar>() to access it.

Here's an example of how to update the progress of a CircularBar programmatically:

```
//Ensure that the root is properly defined from the UIDocument
CircularBar circularBar = root.Q<CircularBar>(); // Get reference to the CircularBar component
float newFill = 0.5f; // Set the new fill value (between 0 and 1)
circularBar.UpdateBar(newFill); // Update the progress of the CircularBar
```

