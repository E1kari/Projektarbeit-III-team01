using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField]
    private bool invincible = false;

    public virtual void takeDamage()
    {
        if (invincible) return;

        // Change the color of the sprite to red to indicate damage
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        if (gameObject.CompareTag("Player"))
        {
            Controller controller = gameObject.GetComponent<Controller>();
            controller.HandleDeath();

            // Reload the current scene if the player takes damage
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Destroy the game object after a short delay if it's not the player
            Destroy(gameObject, 0.5f);
        }
    }
}
