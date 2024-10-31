using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// SceneManager is used to control the scene that includes UI and Scene Operations
/// <param name="Scene_Manager"></param>
/// </summary>
public class SceneManager : MonoBehaviour
{
    #region Dependencies
    public Spawn_Test CurrentEmit;
    private PoolManager TheManager;
    #endregion
    #region UI
    [Tooltip("The Size of the pool")]
    public Text Pool_S;
    [Tooltip("The Number of spawned objects")]
    public Text Sp_S;
    [Tooltip("The Number of spawned allowed")]
    public Text Sp_S_A;
    [Tooltip("The Number of Unspawned objects")]
    public Text UnSp_S;
    [Tooltip("The value from Pool Size Slider")]
    public Slider Pool_Size_Slider;
    [Tooltip("The value from Size Slider")]
    public Slider Size_Slider;
    [Tooltip("The value from Rate Slider")]
    public Slider Rate_Slider;
    [Tooltip("The value from Delay Slider")]
    public Slider Delay_Slider;
    [Tooltip("The bool from Can Grow Toggle")]
    public Toggle Can_Grow_Toggle;
    #endregion

    #region Singleton
    // Start is called before the first frame update
    void Start()
    {
        if (CurrentEmit == null)
        {
            //find the emit in the entire scene
            CurrentEmit = FindObjectOfType<Spawn_Test>();
        }
        TheManager = PoolManager.Pool_Manager_Instance; // get the static instance of the pool manager
    }

    void OnApplicationQuit()
    {
        TheManager.Delete_All(); // delete all the pools
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVariablesFromUI();// update the UI
        UpdateText();// update the text of the UI

    }
    #endregion

    #region Scene Operations
    /// <summary>
    /// this is to reload the entire scene from scratch
    /// </summary>
    public void Reload()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); // reload the scene
    }
    /// <summary>
    /// End the application completely
    /// </summary>
    public void Exit()
    {
        Application.Quit(); // end the application
    }
    #endregion

    #region UI Operations
    /// <summary>
    /// reset the increment
    /// </summary>
    public void Reset()
    {
        CurrentEmit._Increment = 0;
        CurrentEmit._Reset = !CurrentEmit._Reset;
    }
    /// <summary>
    /// this is to change the pooling state from using optimization to using simple instantiation
    /// </summary>
    public void Change()
    {
        CurrentEmit.This_Pool.DePopulate_Pool();
        TheManager.Delete_All(); // delete all the pools
        CurrentEmit.Pooling = !CurrentEmit.Pooling; // change the pooling state
        CurrentEmit.Pool_Size = 0;
    }
    /// <summary>
    /// Update the UI elements
    /// </summary>
    private void UpdateVariablesFromUI()
    {
        if (CurrentEmit.This_Pool != null) // check if the pool size slider value is different from the pool size
        {
            CurrentEmit.This_Pool.Pool_Size = CurrentEmit.Pool_Size;//set the value of the slider to the casted one
            CurrentEmit.This_Pool.Allow_Growth = Can_Grow_Toggle.isOn;//set the value of the slider to the casted one
        }
        CurrentEmit.Pool_Size = (uint)Pool_Size_Slider.value; //cast the value of the slider to int
        CurrentEmit.Number_to_be_Spawned = (int)Size_Slider.value; // cast the value of the slider to int
        CurrentEmit.Rate = (float)Rate_Slider.value; // cast the value of the slider to float
        CurrentEmit.Delay = (float)Delay_Slider.value; // cast the value of the slider to float

    }
    /// <summary>
    /// Update the text of the UI
    /// </summary>
    public void UpdateText()
    {
        if (CurrentEmit.This_Pool != null) // check if the pool size slider value is different from the pool size
        {
            Pool_S.text = ((int)CurrentEmit.This_Pool.Pool_Size).ToString();
            UnSp_S.text = CurrentEmit.This_Pool.Number_Of_Empty().ToString();
            Sp_S.text = ((int)CurrentEmit.This_Pool.Number_Of_Spawned()).ToString();
        }
        Sp_S_A.text = ((int)Size_Slider.value).ToString();
    }
    #endregion
}
