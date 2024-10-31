using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// FpsCounter is a class that is used to calculate the fps of the game
/// <param name="Fps_Counter"></param>

/// </summary>
public class FpsCounter : MonoBehaviour
{
    #region fps calculation
    private float tempFpsRate = 0;
    private float dt = 0.0f;
    private float currentFps = 0.0f;
    private const float updateRate = 1.0f;
    private uint frameCount = 0;

    [Tooltip("The Delay between each fps calculation")]
    public uint Fps_Delay;

    [Tooltip("The Current Fps")]
    public Text currentFpsText;
    #endregion
    #region singleton
    // Start is called before the first frame update
    void Start()
    {
        if (currentFpsText == null) // check if the text is null
        {
            Transform[] Children = this.transform.GetComponentsInChildren<Transform>(false); // get the children of the object
            if (Children.Length != 0) // check if the children are not null
            {
                foreach (Transform Child in Children) // loop through the children
                {
                    if (Child != this.transform) // check if the child is not the parent
                    {
                        currentFpsText = Child.GetComponentInChildren<Text>(false); // get the text component and assign it to the text variable
                        break; // break out of the loop
                    }
                }
            }
        }
        if (Fps_Delay <= 0)
            Fps_Delay = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFpsText != null)
            FPS_Calculation();
    }

    #endregion

    /// <summary>
    /// this is to calculate the fps
    /// </summary>
    private void FPS_Calculation()
    {
        if (Time.time > tempFpsRate)
        {
            tempFpsRate = Time.time + Fps_Delay;
        }
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate)
        {
            currentFps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
        currentFpsText.text = ((int)currentFps).ToString();
    }
}