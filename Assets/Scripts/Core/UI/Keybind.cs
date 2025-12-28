using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Keybind : MonoBehaviour
{
    [Header("References")]
    [SerializeField] InputActionReference actionReference;
    [SerializeField] PlayerMovement movementScript;
    [SerializeField] TextMeshProUGUI label;

    [Header("Composite Binding Settings")]
    [Header("If this action is part of a composite (like Move/WASD/Left), set this to the name of the part, with the first letter lowercase (left).")]
    [Header("Leave empty for single bindings.")]
    [SerializeField] string nameOfCompositeKeybind;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebinding()
    {
        label.text = "Awaiting input";

        // Switch to the rebinding-friendly action map
        movementScript.PlayerInput.SwitchCurrentActionMap("InputPause");

        // Find the binding index
        int bindingIndex = -1;

        // Check if the keybind is a composite part
        if (!string.IsNullOrEmpty(nameOfCompositeKeybind))
        {
            // Look for the composite part
            for (int i = 0; i < actionReference.action.bindings.Count; i++)
            {
                var binding = actionReference.action.bindings[i];
                //if (binding.isPartOfComposite && binding.name == nameOfCompositeKeybind)
                if (binding.name == nameOfCompositeKeybind)
                {
                    bindingIndex = i;
                    break;
                }
            }
        }
        else
        {
            // Normal single binding: pick the first binding
            if (actionReference.action.bindings.Count > 0)
                bindingIndex = 0;
        }

        if (bindingIndex == -1)
        {
            Debug.LogError("Could not find binding " + nameOfCompositeKeybind + " in action " + actionReference.action.name);
            return;
        }

        rebindingOperation = actionReference.action.PerformInteractiveRebinding()
            .WithTargetBinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(bindingIndex))
            .Start();  
    }

    private void RebindComplete(int bindingIndex)
    {
        // Update the label to show the new control
        label.text = InputControlPath.ToHumanReadableString(
            actionReference.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        // Clean up the rebinding operation
        rebindingOperation.Dispose();

        // Switch back to the main action map
        // This is going to have to be changed in the future because we don't want player to be able to do any moving and such while in settings
        movementScript.PlayerInput.SwitchCurrentActionMap("Player");
    }
}
