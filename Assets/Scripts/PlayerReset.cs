using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Reset the level when the player dies, and keep track of how many
/// times they have done so
/// </summary>
public class PlayerReset : MonoBehaviour {
    static int timesDied = 0;
    /// <summary>
    /// Increment our death counter and reload the level
    /// </summary>
    void OnDestroy()
    {
        timesDied++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
