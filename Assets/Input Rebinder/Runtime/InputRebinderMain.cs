using UnityEngine;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Component at the root of the Input Rebinder runtime suite.
    /// To display the UI in the game, you need to enable the game object
    /// attached to this component.
    /// </summary>
    public class InputRebinderMain : MonoBehaviour
    {
        /// <summary>
        /// Listener to when the close button is clicked
        /// </summary>
        public void ClickClose()
        {
            this.gameObject.SetActive(false);
        }
    }
}