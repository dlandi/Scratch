namespace QuickGridTest01.MultiState.Core;

/// <summary>
/// Defines and validates state transitions for the multi-state cell.
/// </summary>
public static class StateTransitionRules
{
    /// <summary>
    /// Validates if a transition from one state to another is allowed.
    /// </summary>
    /// <param name="from">Current state</param>
    /// <param name="to">Desired state</param>
    /// <returns>True if transition is valid, false otherwise</returns>
    public static bool IsValidTransition(CellState from, CellState to)
    {
        return (from, to) switch
        {
            // From Reading
            (CellState.Reading, CellState.Editing) => true,
            
            // From Editing
            (CellState.Editing, CellState.Loading) => true,  // Save
            (CellState.Editing, CellState.Reading) => true,  // Cancel
            
            // From Loading
            (CellState.Loading, CellState.Reading) => true,  // Success
            (CellState.Loading, CellState.Error) => true,    // Failure
            
            // From Error
            (CellState.Error, CellState.Loading) => true,    // Retry
            (CellState.Error, CellState.Reading) => true,    // Revert
            (CellState.Error, CellState.Editing) => true,    // Edit again
            
            // Same state (no-op)
            var (s1, s2) when s1 == s2 => true,
            
            // All others invalid
            _ => false
        };
    }

    /// <summary>
    /// Gets all valid next states from the current state.
    /// </summary>
    /// <param name="currentState">The current state</param>
    /// <returns>Array of valid next states</returns>
    public static CellState[] GetValidNextStates(CellState currentState)
    {
        return currentState switch
        {
            CellState.Reading => new[] { CellState.Reading, CellState.Editing },
            CellState.Editing => new[] { CellState.Editing, CellState.Loading, CellState.Reading },
            CellState.Loading => new[] { CellState.Loading, CellState.Reading, CellState.Error },
            CellState.Error => new[] { CellState.Error, CellState.Loading, CellState.Reading, CellState.Editing },
            _ => Array.Empty<CellState>()
        };
    }
}
