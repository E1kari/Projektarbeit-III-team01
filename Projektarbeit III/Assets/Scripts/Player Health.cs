using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public override void takeDamage()
    {
        SceneManager.LoadScene("Selection Screen");
    }
}
