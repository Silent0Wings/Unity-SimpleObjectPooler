using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// PoolManager is used to handle the pools (a pool manager is made of pools and pools are made of pool nodes)
/// <param name="Pool_Manager"></param>
/// </summary>
[SerializeField]
public class PoolManager : MonoBehaviour
{
    #region Data
    [Header("Pool Manager")]
    [Tooltip("this is to clear all the pools")]
    public bool Clear_All; // trigger to clear all the pools

    [Tooltip("Pool Dictionary")]
    public Dictionary<GameObject, Pool> Pool_Dic = new(); // a dictionary of pools indexed by the GameObject

    [Tooltip("a queue of objects to add to the pool")]
    public Queue<PrePoolDataHolder> To_Add = new();  // make me a Queue of type GameObject

    [Header("static instance PoolManager")]
    [Tooltip("static instance of the pool manager")]
    public static PoolManager Pool_Manager_Instance;// a static instance of the pool manager to allow other scripts to access the pool manager
    #endregion

    #region Singleton
    private void Awake()
    {
        Pool_Manager_Instance = this;
    }
    private void Start()
    {
        Pool_Manager_Instance = this;
    }
    private void Update()
    {
        if (Clear_All)// clear all the pools
        {
            Delete_All();
        }

        if (To_Add.Count != 0)// is there any object to add to the pool
        {
            //  get the first item out of the queue into  GameObject Fetch_A_Queued_node and remove the item from the queue
            PrePoolDataHolder Temp_Obj = To_Add.Dequeue();
            if (Pool_Dic.ContainsKey(Temp_Obj.Prefab) == false)// check if the pool manager doesn't contain the object
            {
                Pool Temp_Pool = Pool.Create_Pool(Temp_Obj, this.transform);// create a new pool of size 100
                if (Temp_Pool != null)
                {
                    Pool_Dic.Add(Temp_Obj.Prefab, Temp_Pool); // add the pool to the pool manager dictionary
                    Temp_Pool.Populate_Pool();// populate the pool by filling it with objects
                }
            }
        }
    }
    #endregion

    #region Main Behavior
    public virtual void Handle_Behavior()
    {
        if (Clear_All)// clear all the pools
        {
            Delete_All();
        }

        if (To_Add.Count != 0)// is there any object to add to the pool
        {
            //  get the first item out of the queue into  GameObject Fetch_A_Queued_node and remove the item from the queue
            PrePoolDataHolder Fetch_A_Queued_node = To_Add.Dequeue();
            if (Pool_Dic.ContainsKey(Fetch_A_Queued_node.Prefab) == false)// check if the pool manager doesn't contain the object
            {
                // how to access the function create pool inside pool 

                Pool Current_Pool = Pool.Create_Pool(Fetch_A_Queued_node, this.transform);// create a new pool of size 100
                if (Current_Pool != null)// check if the pool is not null
                {
                    Pool_Dic.Add(Fetch_A_Queued_node.Prefab, Current_Pool); // add the pool to the pool manager dictionary
                    Current_Pool.Populate_Pool();// populate the pool by filling it with objects
                }
            }
        }
    }
    #endregion

    #region Create Pool overloaded functions 

    public PrePoolDataHolder Adding_to_Queue(GameObject prf)
    {
        PrePoolDataHolder Pool_Queue_Node = new(prf);
        To_Add.Enqueue(Pool_Queue_Node);
        return Pool_Queue_Node;
    }
    public PrePoolDataHolder Adding_to_Queue(GameObject prf, uint pools)
    {
        PrePoolDataHolder Pool_Queue_Node = new(prf, pools);
        To_Add.Enqueue(Pool_Queue_Node);
        return Pool_Queue_Node;
    }
    public PrePoolDataHolder Adding_to_Queue(GameObject prf, uint pools, bool allowG)
    {
        PrePoolDataHolder Pool_Queue_Node = new(prf, pools, allowG);
        To_Add.Enqueue(Pool_Queue_Node);
        return Pool_Queue_Node;
    }

    #endregion

    #region Delete everythig
    /// <summary>
    /// delete all pools at once and their content
    /// </summary>
    public void Delete_All()
    {
        foreach (var item in Pool_Dic) // iterate through the pool manager dictionary and delete all the pools and their content
        {
            item.Value.Destroy_Pool();
        }
        foreach (PrePoolDataHolder item in To_Add) // iterate through the queue and delete all the objects in the queue
        {
            item.Destroy_Node();
        }

        To_Add.Clear(); // clear the queue
        Pool_Dic.Clear(); // clear the pool manager dictionary
        Clear_All = !Clear_All; // set the clear all to false
        StopAllCoroutines();
    }
    #endregion

    #region Browse & Find
    /// <summary>
    /// this function is to find a pool by its name
    /// </summary>
    /// <param name="_Name"></param>
    /// <returns></returns>
    public Pool Find_Pool(string _Name)
    {
        Pool Temp_Pool = null;

        if (Pool_Dic.Count != 0)
        {
            foreach (var Current_Pool in Pool_Dic) // iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (Current_Pool.Value._Name == _Name) // check if the name of the pool is equal to the name passed to the function
                    {
                        Temp_Pool = Current_Pool.Value;
                        break;
                    }
            }
        }


        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its tag
    /// </summary>
    /// <param name="Tag"></param>
    /// <returns></returns>
    public Pool Find_Pool_Tag(string Tag)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0)
        {
            foreach (var Current_Pool in Pool_Dic) //   iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (Current_Pool.Value._Tag == Tag) // check if the tag of the pool is equal to the tag passed to the function
                    {
                        Temp_Pool = Current_Pool.Value;
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its ID
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Pool Find_Pool(int ID)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0)
        {
            foreach (var Current_Pool in Pool_Dic) //   iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (Current_Pool.Value.ID == ID) // check if the ID of the pool is equal to the ID passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its layer
    /// </summary>
    /// <param name="Layer"></param>
    /// <returns></returns>
    public Pool Find_Pool(LayerMask Layer)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) //   iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (Current_Pool.Value._Layer == Layer) // check if the layer of the pool is equal to the layer passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its name and its tag
    /// </summary>
    /// <param name="Prefab"></param>
    /// <returns></returns>
    public Pool Find_Pool(GameObject Prefab)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) //   iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (GameObject.Equals(Current_Pool.Value.Prefab, Prefab)) // check if the prefab of the pool is equal to the prefab passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its instance and its name
    /// </summary>
    /// <param name="Prefab"></param>
    /// <param name="_Name"></param>
    /// <returns></returns>
    public Pool Find_Pool(GameObject Prefab, string _Name)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) //  iterate through the pool manager dictionary
            {
                if (GameObject.Equals(Current_Pool.Value.Prefab, Prefab) && Current_Pool.Value._Name == _Name)  // check if the prefab of the pool is equal to the prefab passed to the function and the name of the pool is equal to the name passed to the function
                {
                    if (Current_Pool.Value != null)
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool
                    break;
                }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its instance and its ID
    /// </summary>
    /// <param name="Prefab"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Pool Find_Pool(GameObject Prefab, int ID)
    {
        Pool Temp_Pool = null;

        if (Pool_Dic.Count != 0) // check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) //  iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (GameObject.Equals(Current_Pool.Value.Prefab, Prefab) && Current_Pool.Value.ID == ID) // check if the prefab of the pool is equal to the prefab passed to the function and the ID of the pool is equal to the ID passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its name and its instance ,name and id
    /// </summary>
    /// <param name="Prefab"></param>
    /// <param name="_Name"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Pool Find_Pool(GameObject Prefab, string _Name, int ID)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // this is to check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) //  iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (GameObject.Equals(Current_Pool.Value.Prefab, Prefab) && Current_Pool.Value._Name == _Name && Current_Pool.Value.ID == ID) // check if the prefab of the pool is equal to the prefab passed to the function and the name of the pool is equal to the name passed to the function and the ID of the pool is equal to the ID passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its instance, its tag ,name and ID ,layer
    /// </summary>
    /// <param name="Prefab"></param>
    /// <param name="_Name"></param>
    /// <param name="Tag"></param>
    /// <param name="ID"></param>
    /// <param name="Layer"></param>
    /// <returns></returns>
    public Pool Find_Pool(GameObject Prefab, string _Name, string Tag, int ID, LayerMask Layer)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // this is to check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) //  iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (GameObject.Equals(Current_Pool.Value.Prefab, Prefab) && Current_Pool.Value._Name == _Name && Current_Pool.Value._Tag == Tag && Current_Pool.Value.ID == ID && Current_Pool.Value._Layer == Layer) // check if the prefab of the pool is equal to the prefab passed to the function and the name of the pool is equal to the name passed to the function and the tag of the pool is equal to the tag passed to the function and the ID of the pool is equal to the ID passed to the function and the layer of the pool is equal to the layer passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its instance ,name,ID and layer
    /// </summary>
    /// <param name="Prefab"></param>
    /// <param name="_Name"></param>
    /// <param name="ID"></param>
    /// <param name="Layer"></param>
    /// <returns></returns>
    public Pool Find_Pool(GameObject Prefab, string _Name, int ID, LayerMask Layer)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // this is to check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) //  iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (GameObject.Equals(Current_Pool.Value.Prefab, Prefab) && Current_Pool.Value._Name == _Name && Current_Pool.Value.ID == ID && Current_Pool.Value._Layer == Layer) // check if the prefab of the pool is equal to the prefab passed to the function and the name of the pool is equal to the name passed to the function and the ID of the pool is equal to the ID passed to the function and the layer of the pool is equal to the layer passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its name and its tag
    /// </summary>
    /// <param name="_Name"></param>
    /// <param name="Tag"></param>
    /// <returns></returns>
    public Pool Find_Pool(string _Name, string Tag)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // this is to check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) // this is to iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (Current_Pool.Value._Name == _Name && Current_Pool.Value._Tag == Tag) // this is to check if the name of the pool is equal to the name passed to the function and the tag of the pool is equal to the tag passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its name and its ID
    /// </summary>
    /// <param name="_Name"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Pool Find_Pool(string _Name, int ID)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // this is to check if the pool manager dictionary is not empty
        {

            foreach (var Current_Pool in Pool_Dic) // this is to iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (Current_Pool.Value._Name == _Name && Current_Pool.Value.ID == ID) // this is to check if the name of the pool is equal to the name passed to the function and the ID of the pool is equal to the ID passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its name and its tag an ID
    /// </summary>
    /// <param name="_Name"></param>
    /// <param name="Tag"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Pool Find_Pool(string _Name, string Tag, int ID)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // this is to check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) // this is to iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (Current_Pool.Value._Name == _Name && Current_Pool.Value._Tag == Tag && Current_Pool.Value.ID == ID) // this is to check if the name of the pool is equal to the name passed to the function and the tag of the pool is equal to the tag passed to the function and the ID of the pool is equal to the ID passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    /// <summary>
    /// this function is to find a pool by its instance, name ,tag ,ID and instance
    /// </summary>
    /// <param name="Prefab"></param>
    /// <param name="_Name"></param>
    /// <param name="Tag"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Pool Find_Pool(GameObject Prefab, string _Name, string Tag, int ID)
    {
        Pool Temp_Pool = null;
        if (Pool_Dic.Count != 0) // this is to check if the pool manager dictionary is not empty
        {
            foreach (var Current_Pool in Pool_Dic) // this is to iterate through the pool manager dictionary
            {
                if (Current_Pool.Value != null)
                    if (GameObject.Equals(Current_Pool.Value.Prefab, Prefab) && Current_Pool.Value._Name == _Name && Current_Pool.Value._Tag == Tag && Current_Pool.Value.ID == ID) // this is to check if the prefab of the pool is equal to the prefab passed to the function and the name of the pool is equal to the name passed to the function and the tag of the pool is equal to the tag passed to the function and the ID of the pool is equal to the ID passed to the function
                    {
                        Temp_Pool = Current_Pool.Value; // set the temp pool to the current pool    
                        break;
                    }
            }
        }
        return Temp_Pool;
    }
    #endregion
}