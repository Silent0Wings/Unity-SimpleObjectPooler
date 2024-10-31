using UnityEngine;

/// <summary>
/// Spawn_Test is used to test the ability to spawn objects using pooling or simple instantiation
/// <param name="Emit_Test"></param>
/// </summary>
[SerializeField]
public class Spawn_Test : MonoBehaviour
{
    #region Data
    [Tooltip("The number of spawned objects")]
    public int _Increment;
    [Tooltip("The number of objects to spawn")]
    public int Number_to_be_Spawned; //represents the number of objects to spawn in total from this emitter
    [Tooltip("The size of the pool in the ")]
    public uint Pool_Size; // the size of the pool in the pool manager linked to this emitter emitted prefab
    [Tooltip("The rate/speed spawned objects")]
    public float Rate;
    [Tooltip("The Life of a particle")]
    public float Delay;
    [Tooltip("The Force applied to the spawned objects")]
    public float Force;
    [Tooltip("offset the position of the spawned objects")]
    public Vector3 Offset;

    private float Temp_Rate; // spawn rate to time the spawning of the objects

    #endregion
    #region bool
    [Tooltip("_Reset")]
    public bool _Reset;
    [Tooltip("Use Pooling as a way to create instances")]
    public bool Pooling;
    #endregion
    #region Dependencies
    private PoolManager The_Manager;
    private SceneManager current_Scene;
    public GameObject Prefab;
    public Pool This_Pool;

    #endregion

    #region Singleton

    void Start()
    {
        The_Manager = PoolManager.Pool_Manager_Instance; // get the static instance of the pool manager 
        current_Scene = FindObjectOfType<SceneManager>();

    }

    void LateUpdate()
    {
        if (This_Pool == null) // check if the pool is null so that we can get the pool from the pool manager or create a new one
        {
            if (The_Manager.Pool_Dic.ContainsKey(Prefab))// check if the pool already contains the prefab
            {
                This_Pool = The_Manager.Pool_Dic[Prefab]; // get the pool from the pool manager dictionary that links the prefab to the pool using dictionary
            }
            else
            {
                // add the prefab to the queue TheManager.To_Add 
                The_Manager.Adding_to_Queue(Prefab, (uint)Pool_Size);
                This_Pool = The_Manager.Find_Pool(Prefab);
            }
        }
        if (_Reset) //reset the increment if the reset bool is true
        {
            current_Scene.Reset();
        }
        if (Pooling)//check if pooling is true
        {
            if (This_Pool != null) // check if the pool is not null
            {
                if (Time.time > Temp_Rate)
                {
                    Temp_Rate = Time.time + Rate;

                    if (Number_to_be_Spawned > 0) // check if the size is greater than 0
                    {
                        if (_Increment < Number_to_be_Spawned) // check if the increment is less than the size
                        {
                            _Increment++;
                            //  GameObject Temp = Instantiate(Prefab, this.transform.position + Offset, Quaternion.identity, this.transform);
                            //Vector3 rotation = new Vector3(Mathf.Cos(Time.time), 0, Mathf.Sin(Time.time)) * 100;
                            PoolNode Temp = This_Pool.Spawn(this.transform.position + Offset, Quaternion.identity, this.transform);
                            //PoolNode Temp = This_Pool.Spawn(rotation + Offset, Quaternion.identity, this.transform);
                            This_Pool.DeSpawn(Temp, Delay);
                            Temp.Obj.GetComponent<Rigidbody>().AddForce(Force * this.transform.up);

                        }
                    }
                }
            }
        }
        else // use simple instantiation handled by unity instantiation no pooling behavior is implemented 
        {
            if (Time.time > Temp_Rate)
            {
                Temp_Rate = Time.time + Rate;
                if (Number_to_be_Spawned > 0)
                {
                    if (_Increment < Number_to_be_Spawned)
                    {
                        _Increment++;
                        GameObject Temp = Instantiate(Prefab, this.transform.position + Offset, Quaternion.identity, this.transform); // instantiate the object
                        Temp.GetComponent<Rigidbody>().AddForce(Force * this.transform.up);// add force to the spawned object
                    }
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        The_Manager.Delete_All(); // delete all the pools
    }

    #endregion
}