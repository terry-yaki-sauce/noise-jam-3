using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Util
{
  public class GameUtils
  {
    /// <summary>
    /// Get <b>ALL</b> keybinds for one <c>action</c> attached to <c>playerInput</c> for its current active control scheme
    /// </summary>
    /// <param name="playerInput">the PlayerInput which contains keybinds and action names</param>
    /// <param name="action">the name of the action of which the keybinds should be checked</param>
    /// <returns></returns>
    public static string[] GetKeybinds(PlayerInput playerInput, string action)
    {
      InputAction InteractAction = playerInput.actions.FindAction(action);
      ReadOnlyArray<InputBinding> bindings = InteractAction.bindings;
      InputBinding mask = InputBinding.MaskByGroup(playerInput.currentControlScheme);
      string[] keyStrings = new string[bindings.Count];
      int j = 0;
      for (int i = 0; i < bindings.Count; i++)
      {
        if (mask.Matches(bindings[i]))
          keyStrings[j++] = bindings[i].ToDisplayString();
      }
      return keyStrings;
    }

    /// <summary>
    /// Return the FIRST keybind for one <c>action</c> attached to <c>playerInput</c> for its current active control scheme
    /// </summary>
    /// <param name="playerInput">the PlayerInput which contains keybinds and action names</param>
    /// <param name="action">the name of the action of which the keybinds should be checked</param>
    /// <returns>a</returns>
    public static string GetKeybind(PlayerInput playerInput, string action)
    {
      string keyBind = "";
      InputAction InteractAction = playerInput.actions.FindAction(action);
      ReadOnlyArray<InputBinding> bindings = InteractAction.bindings;
      InputBinding mask = InputBinding.MaskByGroup(playerInput.currentControlScheme);
      for (int i = 0; i < bindings.Count; i++)
      {
        if (mask.Matches(bindings[i]))
          return bindings[i].ToDisplayString();
      }
      return keyBind;
    }
  }
}