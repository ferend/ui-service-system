# ui-service-system
## A service locator pattern in C# uses Unity with the UI system's implementation.

# Implementation with ServiceLocator

Service Locator: Used to manage the IUIService instance.

UI Service (IUIService): Interface for managing UI screens.

OpenPauseScreen(): In UISystem, use the IUIService to open the pause screen.

ClosePauseScreen(): In UISystem, closes the pause screen using the IUIService.

By implementing the IUIService interface within the UISystem class, you can effectively manage UI screens and interactions while maintaining control over screen creation and destruction.
