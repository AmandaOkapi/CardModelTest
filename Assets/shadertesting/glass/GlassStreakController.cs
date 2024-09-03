using UnityEngine;

public class GlassStreakController : MonoBehaviour
{
    public Material glassMaterial;  // Assign this in the Inspector
    public float streakSpeed = 1.0f;
    private float streakInterval = 5.0f;  // Set the interval to 5 seconds
    private float timeElapsed = 0.0f;

    void Update()
    {
        // Accumulate time
        timeElapsed += Time.deltaTime;

        // Check if the time exceeds the streak interval
        if (timeElapsed >= streakInterval)
        {
            // Reset the time counter
            timeElapsed = 0.0f;
            
            // Trigger the streak effect by adjusting the streak speed
            glassMaterial.SetFloat("_StreakSpeed", streakSpeed);
        }
        else
        {
            // Temporarily set streak speed to 0 to halt the effect until the next interval
            glassMaterial.SetFloat("_StreakSpeed", 0.0f);
        }

        // Update the streak interval property in the shader
        glassMaterial.SetFloat("_StreakInterval", streakInterval);
    }
}