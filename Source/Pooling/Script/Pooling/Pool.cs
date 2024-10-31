using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
// this is the Pool node class and it is used to store the data of the pool and handle the spawning and respawning of the objects
/// <param name="Pool"></param>
/// </summary>
[SerializeField]
public class Pool : MonoBehaviour
{
    #region Data
    [Header("Pool Data")]
    [Tooltip("The ID of the pool")]
    public int ID;
    [Tooltip("The name of the pool")]
    public string _Name;
    [Tooltip("The tag of the pool")]
    public string _Tag;
    [Tooltip("The layer of the pool")]
    public LayerMask _Layer;
    [Tooltip("The prefab of the pool")]
    public GameObject Prefab;
    [Tooltip("The list of the pool")]
    public List<PoolNode> Pool_List = new();
    #endregion

    #region Settings
    [Header("Pool Settings")]
    [Tooltip("The size of the pool")]
    public uint Pool_Size; // the size of the pool
    [Tooltip("Allow the pool size to grow")]
    public bool Allow_Growth; // allow the pool to grow

    [Header("Spawn Settings")]
    [Tooltip("The type of spawn")]
    public Spawn_Type_Handler Type_Of_Spawn; // the type of spawn
    [Tooltip("The particle system handler")]
    public Particle_System_Handler _Particle_Handler; // the particle handler
    [Tooltip("The audio system handler")]
    public Audio_System_Handler _Audio_System_Handler; //   
    [Tooltip("The physics handler")]
    public Physics_Handler physics_Handler; // the physics handler
    #endregion

    #region Constructors
    public static Pool Create_Pool(GameObject Prefab, Transform Parent)
    {
        GameObject NewObj = new(Prefab.GetHashCode() + Prefab.name + Prefab.tag + LayerMask.LayerToName(Prefab.layer) + System.DateTime.UtcNow.ToString());// create a new game object and set its name to the hash code of the prefab + the name of the prefab + the tag of the prefab + the layer of the prefab
        NewObj.transform.parent = Parent;
        Pool NewPool = NewObj.AddComponent<Pool>();
        NewPool.Prefab = Prefab;
        NewPool.ID = Prefab.GetHashCode();
        NewPool._Name = Prefab.name;
        NewPool._Tag = Prefab.tag;
        NewPool._Layer = Prefab.layer;
        NewPool.Pool_Size = 0;
        NewPool.Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        NewPool._Particle_Handler = Particle_System_Handler.playOnSpawn;
        NewPool._Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        NewPool.physics_Handler = Physics_Handler.Create_Rigid_Body;

        return NewPool;
    }
    public static Pool Create_Pool(PrePoolDataHolder Queue_Node, Transform Parent)
    {
        GameObject NewObj = new(Queue_Node.GetHashCode() + Queue_Node.Prefab.name + Queue_Node.Prefab.tag + LayerMask.LayerToName(Queue_Node.Prefab.layer) + System.DateTime.UtcNow.ToString());// create a new game object and set its name to the hash code of the prefab + the name of the prefab + the tag of the prefab + the layer of the prefab
        NewObj.transform.parent = Parent;

        Pool NewPool = NewObj.AddComponent<Pool>();
        NewPool.Prefab = Queue_Node.Prefab;
        NewPool.ID = Queue_Node.GetHashCode();
        NewPool._Name = Queue_Node.Prefab.name;
        NewPool._Tag = Queue_Node.Prefab.tag;
        NewPool._Layer = Queue_Node.Prefab.layer;
        NewPool.Pool_Size = Queue_Node.poolSize;
        NewPool.Type_Of_Spawn = Queue_Node.Type_Of_Spawn;
        NewPool._Particle_Handler = Queue_Node._Particle_Handler;
        NewPool._Audio_System_Handler = Queue_Node._Audio_System_Handler;
        NewPool.physics_Handler = Queue_Node.physics_Handler;

        return NewPool;
    }
    #endregion
    #region Spawn & Despawn
    /// <summary>
    /// this is to create an item
    /// </summary>
    /// <returns></returns>
    public PoolNode Create_Item()
    {
        PoolNode Temp_Node = new(); // create a new node
        Temp_Node.Obj = Instantiate(Prefab, this.transform.position, Quaternion.identity, this.transform); // instantiate the prefab
        if (Temp_Node.Obj != null) // if the object is not null
        {
            Temp_Node.ID = Temp_Node.Obj.GetHashCode(); // get the hash code of the object
            Temp_Node.Spawned = false; // set the object to not spawned
            Temp_Node._Audio = Temp_Node.Obj.GetComponentsInChildren<AudioSource>().Cast<AudioSource>().ToList(); // get the audio source of the object
            Temp_Node._Particle = Temp_Node.Obj.GetComponentsInChildren<ParticleSystem>().Cast<ParticleSystem>().ToList(); // get the particle system of the object
            Temp_Node._Rigid_body = Temp_Node.Obj.GetComponentInChildren<Rigidbody>(); // get the particle system of the object

            Temp_Node.Obj.SetActive(false); // set the object to inactive
            return Temp_Node;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// this is to handle the audio particle despawn
    /// </summary>
    /// <param name="Current_Pool"></param>
    private void Handle_Audio_Particle_Spawn(PoolNode Current_Pool)
    {
        if (Current_Pool._Audio.Count != 0) // if the audio count is not 0
        {
            if ((_Audio_System_Handler) == Audio_System_Handler.playOnSpawn) // if the audio handler is play on spawn
            {
                Prefab.GetComponent<AudioSource>().Play(); // play the audio source
            }
        }
    }
    /// <summary>
    /// this is to handle the audio particle despawn
    /// </summary>
    /// <param name="Current_Pool"></param>
    private void Handle_Audio_Particle_DeSpawn(PoolNode Current_Pool)
    {
        if (Current_Pool._Particle.Count != 0) // if the particle count is not 0
        {
            if ((_Particle_Handler) == Particle_System_Handler.stopOnDespawn) // if the particle handler is stop on despawn
            {
                Prefab.GetComponent<ParticleSystem>().Stop(); // stop the particle system
            }
        }
    }

    #region Spawn
    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <returns></returns>
    public PoolNode Spawn(Vector3 _Position, Quaternion _Rotation, Transform _Parent)
    {
        PoolNode Current_Pool = null;
        if (Is_Full()) // if the pool is full
        {
            if (Allow_Growth) // if the pool is allowed to grow
            {
                Current_Pool = Create_Item();
                if (Current_Pool != null)
                {
                    Pool_Size++;
                    Pool_List.Add(Current_Pool);
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            Current_Pool = Find_Free_Node();
        }

        if (Current_Pool != null)
        {
            Current_Pool.Obj.transform.SetPositionAndRotation(_Position, _Rotation);
            Current_Pool.Obj.transform.transform.parent = _Parent;
            if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable) // if the spawn type is disable enable
            {
                Current_Pool.Obj.SetActive(true); // set the object to active
            }
            Current_Pool.Spawned = true;
            Handle_Audio_Particle_Spawn(Current_Pool); // call the audio particle spawn method
        }
        return Current_Pool;
    }
    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <returns></returns>
    public PoolNode Spawn(Vector3 _Position, Quaternion _Rotation)
    {
        PoolNode Current_Pool = null;
        if (Is_Full())
        {
            if (Allow_Growth)
            {
                Current_Pool = Create_Item(); // create an item
                if (Current_Pool != null)
                {
                    Pool_Size++;
                    Pool_List.Add(Current_Pool); // add the item to the pool list
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            Current_Pool = Find_Free_Node();
        }

        if (Current_Pool != null)
        {
            if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable)
            {
                Current_Pool.Obj.SetActive(true);
            }
            Current_Pool.Spawned = true;
            Current_Pool.Obj.transform.SetPositionAndRotation(_Position, _Rotation);
            Handle_Audio_Particle_Spawn(Current_Pool);
        }

        return Current_Pool;
    }
    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="_Parent"></param>
    /// <returns></returns>
    public PoolNode Spawn(Transform _Parent)
    {
        PoolNode Current_Pool = null;
        if (Is_Full())
        {
            if (Allow_Growth)
            {
                Current_Pool = Create_Item();
                if (Current_Pool != null)
                {
                    Pool_Size++;
                    Pool_List.Add(Current_Pool);
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            Current_Pool = Find_Free_Node(); // find the free node
        }
        if (Current_Pool != null)
        {
            Current_Pool.Obj.transform.transform.parent = _Parent;
            if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable) // if the spawn type is disable enable
            {
                Current_Pool.Obj.SetActive(true); // set the object to active
            }
            Current_Pool.Spawned = true;
            Handle_Audio_Particle_Spawn(Current_Pool); // call the audio particle spawn method
        }
        return Current_Pool;
    }
    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="_Parent"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    public PoolNode Spawn(Transform _Parent, float Delay)
    {
        PoolNode Current_Pool = null;
        if (Is_Full()) // if the pool is full
        {
            if (Allow_Growth) // if the pool is allowed to grow
            {
                Current_Pool = Create_Item(); // create an item
                if (Current_Pool != null) // if the current pool is not null
                {
                    Pool_Size++;
                    Pool_List.Add(Current_Pool);
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            Current_Pool = Find_Free_Node();// find the free node
        }
        if (Current_Pool != null)
        {
            StartCoroutine(Delay_Spawn(Current_Pool, Vector3.zero, Quaternion.identity, _Parent, Delay));// call the delay spawn method
        }
        return Current_Pool;
    }
    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    public PoolNode Spawn(Vector3 _Position, Quaternion _Rotation, Transform _Parent, float Delay)
    {
        PoolNode Current_Pool = null;
        if (Is_Full())
        {
            if (Allow_Growth)
            {
                Current_Pool = Create_Item();

                if (Current_Pool != null)
                {
                    Pool_Size++;
                    Pool_List.Add(Current_Pool);
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            Current_Pool = Find_Free_Node();
        }
        if (Current_Pool != null)
        {
            StartCoroutine(Delay_Spawn(Current_Pool, _Position, _Rotation, _Parent, Delay));
        }
        return Current_Pool;
    }

    /// <summary>
    /// this is to mass spawn the objects based on the position, rotation and parent
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <returns></returns>
    public List<PoolNode> Mass_Spawn(List<Vector3> _Position, List<Quaternion> _Rotation, List<Transform> _Parent)
    {
        List<PoolNode> Temp = null;

        if (_Position.Count == _Rotation.Count && _Rotation.Count == _Parent.Count)
        {
            for (int i = 0; i < _Position.Count; i++)
            {
                PoolNode Current_Pool = null;
                if (Is_Full()) // if the pool is full
                {
                    if (Allow_Growth)
                    {
                        Current_Pool = Create_Item(); // create an item
                        if (Current_Pool != null)
                        {
                            Pool_Size++;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Current_Pool = Find_Free_Node();
                }
                if (Current_Pool != null)
                {
                    Temp.Add(Current_Pool);
                    Current_Pool.Obj.transform.SetPositionAndRotation(_Position[i], _Rotation[i]);
                    Current_Pool.Obj.transform.transform.parent = _Parent[i];
                    if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable)
                    {
                        Current_Pool.Obj.SetActive(true);
                    }
                    Current_Pool.Spawned = true;
                    Handle_Audio_Particle_Spawn(Current_Pool);
                }
            }
        }
        return Temp;
    }
    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    public List<PoolNode> Mass_Spawn_Delayed(List<Vector3> _Position, List<Quaternion> _Rotation, List<Transform> _Parent, float Delay)
    {
        if (_Position.Count == _Rotation.Count && _Rotation.Count == _Parent.Count) // check if the position count is equal to the rotation count and the rotation count is equal to the parent count
        {
            for (int i = 0; i < _Position.Count; i++)
            {
                PoolNode Current_Pool = null;
                if (Is_Full())
                {
                    if (Allow_Growth) // if the pool is allowed to grow
                    {
                        Current_Pool = Create_Item();
                        if (Current_Pool != null)
                        {
                            Pool_Size++;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Current_Pool = Find_Free_Node();
                }
                if (Current_Pool != null)
                {
                    StartCoroutine(Delay_Spawn(Current_Pool, _Position[i], _Rotation[i], _Parent[i], Delay)); // call the delay spawn method
                }
            }
        }
        return null;
    }
    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    public List<PoolNode> Mass_Spawn_Delayed(List<Vector3> _Position, List<Quaternion> _Rotation, List<Transform> _Parent, List<float> Delay)
    {
        if (_Position.Count == _Rotation.Count && _Rotation.Count == _Parent.Count && _Rotation.Count == Delay.Count)
        {
            for (int i = 0; i < _Position.Count; i++)
            {
                PoolNode Current_Pool = null;
                if (Is_Full())
                {
                    if (Allow_Growth)
                    {
                        Current_Pool = Create_Item();
                        if (Current_Pool != null)
                        {
                            Pool_Size++;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Current_Pool = Find_Free_Node();
                }
                if (Current_Pool != null)
                {
                    StartCoroutine(Delay_Spawn(Current_Pool, _Position[i], _Rotation[i], _Parent[i], Delay[i]));// call the delay spawn method
                }
            }
        }
        return null;
    }
    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    IEnumerator Delay_Spawn(PoolNode Current_Pool, Vector3 _Position, Quaternion _Rotation, Transform _Parent, float Delay)
    {
        //   Example :       StartCoroutine(CoroutineWithMultipleParameters(1.0F, 2.0F, "foo"));
        yield return new WaitForSeconds(Delay); // wait for the delay
        if (Current_Pool != null)
        {
            Current_Pool.Obj.transform.SetPositionAndRotation(_Position, _Rotation);
            Current_Pool.Obj.transform.transform.parent = _Parent;
            if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable) // if the spawn type is disable enable
            {
                Current_Pool.Obj.SetActive(true);
            }
            Current_Pool.Spawned = true;
            Handle_Audio_Particle_Spawn(Current_Pool); // call the audio particle spawn method
        }
    }

    /// <summary>
    /// this is to spawn the object after a delay using coroutine
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    public void Spawn_All(Vector3 _Position, Quaternion _Rotation, Transform _Parent)
    {
        foreach (PoolNode item in Pool_List)
        {
            if (item != null && item.Spawned == false)
            {
                StartCoroutine(Delay_Spawn(item, _Position, _Rotation, _Parent, 0));
            }
        }
    }
    /// <summary>
    /// this is to spawn all the objects in the pool
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <param name="Delay"></param>
    public void Spawn_All(Vector3 _Position, Quaternion _Rotation, Transform _Parent, float Delay)
    {
        foreach (PoolNode item in Pool_List)
        {
            if (item != null && item.Spawned == false)
            {
                StartCoroutine(Delay_Spawn(item, _Position, _Rotation, _Parent, Delay)); // call the delay spawn method
            }
        }
    }
    #endregion
    #region Despawn

    /// <summary>
    /// this is to despawn the object after a delay using coroutine
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <returns></returns>
    public GameObject DeSpawn(PoolNode Current_Pool, Vector3 _Position, Quaternion _Rotation, Transform _Parent)
    {
        if (Current_Pool != null)
        {
            Current_Pool.Obj.transform.SetPositionAndRotation(_Position, _Rotation);
            Current_Pool.Obj.transform.transform.parent = _Parent;
            if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable) // if the spawn type is disable enable
            {
                Current_Pool.Obj.SetActive(false);
            }
            Current_Pool.Spawned = false;
            Handle_Audio_Particle_DeSpawn(Current_Pool);

            return Current_Pool.Obj;
        }
        return null;
    }
    /// <summary>
    /// this is to despawn the object after a delay using coroutine
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <returns></returns>
    public GameObject DeSpawn(PoolNode Current_Pool, Vector3 _Position, Quaternion _Rotation)
    {
        if (Current_Pool != null)
        {
            Current_Pool.Obj.transform.SetPositionAndRotation(_Position, _Rotation);
            if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable) // if the spawn type is disable enable
            {
                Current_Pool.Obj.SetActive(false);
            }
            Current_Pool.Spawned = false;
            Handle_Audio_Particle_DeSpawn(Current_Pool);

            return Current_Pool.Obj;
        }
        return null;
    }
    /// <summary>
    /// this is to despawn the object after a delay using coroutine
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <param name="_Parent"></param>
    /// <returns></returns>
    public GameObject DeSpawn(PoolNode Current_Pool, Transform _Parent)
    {
        if (Current_Pool != null)
        {
            Current_Pool.Obj.transform.transform.parent = _Parent;
            if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable) // if the spawn type is disable enable
            {
                Current_Pool.Obj.SetActive(false);
            }
            Current_Pool.Spawned = false;
            Handle_Audio_Particle_DeSpawn(Current_Pool); // call the audio particle despawn method
            return Current_Pool.Obj;
        }
        return null;
    }
    /// <summary>
    /// this is to despawn the object after a delay using coroutine
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <returns></returns>
    public GameObject DeSpawn(PoolNode Current_Pool)
    {
        if (Current_Pool != null)
        {
            Current_Pool.Obj.transform.transform.parent = this.transform;
            if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable) // if the spawn type is disable enable
            {
                Current_Pool.Obj.SetActive(false);
            }
            Current_Pool.Spawned = false;
            Handle_Audio_Particle_DeSpawn(Current_Pool); // call the audio particle despawn method
            return Current_Pool.Obj;
        }
        return null;
    }
    /// <summary>
    /// this is to despawn the object after a delay using coroutine
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    public GameObject DeSpawn(PoolNode Current_Pool, float Delay)
    {
        if (Current_Pool != null)
        {
            StartCoroutine(Delay_DeSpawn(Current_Pool, this.transform.position, this.transform.rotation, this.transform, Delay)); // call the delay despawn method
            return Current_Pool.Obj;
        }
        return null;
    }
    /// <summary>
    /// this is to despawn the object after a delay using coroutine
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <returns></returns>
    public List<GameObject> Mass_DeSpawn(List<PoolNode> Current_Pool)
    {
        if (Current_Pool != null)
        {
            for (int i = 0; i < Current_Pool.Count; i++)
            {
                if (Current_Pool != null)
                {
                    StartCoroutine(Delay_DeSpawn(Current_Pool[i], this.transform.position, this.transform.rotation, this.transform, 0));// call the delay despawn method
                }
            }
        }
        return null;
    }
    /// <summary>
    /// this is to despawn all the objects in the pool
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    public List<GameObject> Mass_DeSpawn_Delayed(List<PoolNode> Current_Pool, float Delay)
    {
        if (Current_Pool != null)
        {
            for (int i = 0; i < Current_Pool.Count; i++)
            {
                if (Current_Pool != null)
                {
                    StartCoroutine(Delay_DeSpawn(Current_Pool[i], this.transform.position, this.transform.rotation, this.transform, Delay)); // call the delay despawn method
                }
            }
        }
        return null;
    }
    /// <summary>
    /// this is to despawn all the objects in the pool
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    public List<GameObject> Mass_DeSpawn_Delayed(List<PoolNode> Current_Pool, List<float> Delay)
    {
        if (Current_Pool != null)
        {
            if (Current_Pool.Count == Delay.Count)
            {
                for (int i = 0; i < Current_Pool.Count; i++)
                {
                    if (Current_Pool != null) // if the node is not null
                    {
                        StartCoroutine(Delay_DeSpawn(Current_Pool[i], this.transform.position, this.transform.rotation, this.transform, Delay[i])); // call the delay despawn method
                        Handle_Audio_Particle_DeSpawn(Current_Pool[i]); // call the audio particle despawn method
                    }
                }
            }
        }
        return null;
    }
    /// <summary>
    /// this is to despawn the object after a delay using coroutine 
    /// </summary>
    /// <param name="Current_Pool"></param>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <param name="Delay"></param>
    /// <returns></returns>
    IEnumerator Delay_DeSpawn(PoolNode Current_Pool, Vector3 _Position, Quaternion _Rotation, Transform _Parent, float Delay)
    {
        if (Current_Pool != null)
        {
            yield return new WaitForSeconds(Delay);
            if (Current_Pool != null) // if the node is not null
            {
                Current_Pool.Obj.transform.SetPositionAndRotation(_Position, _Rotation);
                Current_Pool.Obj.transform.transform.parent = _Parent;
                if ((Type_Of_Spawn) == Spawn_Type_Handler.Disable_Enable) // if the spawn type is disable enable
                {
                    Current_Pool.Obj.SetActive(false);
                }
                Current_Pool.Spawned = false;
                Handle_Audio_Particle_Spawn(Current_Pool);
            }
        }
    }
    /// <summary>
    /// this is to despawn all the objects in the pool
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    public void DeSpawn_All(Vector3 _Position, Quaternion _Rotation, Transform _Parent)
    {
        if (Pool_List != null)
        {
            foreach (PoolNode item in Pool_List)
            {
                if (item.Spawned == false)// if the node is not spawned
                {
                    StartCoroutine(Delay_DeSpawn(item, _Position, _Rotation, _Parent, 0));// call the delay despawn method
                }
            }
        }
    }
    /// <summary>
    /// this is to despawn all the objects in the pool
    /// </summary>
    /// <param name="_Position"></param>
    /// <param name="_Rotation"></param>
    /// <param name="_Parent"></param>
    /// <param name="Delay"></param>
    public void DeSpawn_All(Vector3 _Position, Quaternion _Rotation, Transform _Parent, float Delay)
    {
        if (Pool_List != null)
        {
            foreach (PoolNode item in Pool_List)
            {
                if (item.Spawned == false) // if the node is not spawned
                {
                    StartCoroutine(Delay_DeSpawn(item, _Position, _Rotation, _Parent, Delay)); // call the delay despawn method
                }
            }
        }
    }

    #endregion
    #endregion
    #region Populate & Depopulate & Destroy
    /// <summary>
    /// this is to populate the pool
    /// </summary>
    public void Populate_Pool()
    {
        Pool_List.Clear();
        if (Pool_Size != 0)
        {
            for (int i = 0; i < Pool_Size; i++)
            {
                Pool_List.Add(Create_Item());// add the item to the pool list
            }
        }
    }
    /// <summary>
    /// this is to depopulate the pool
    /// </summary>
    public void DePopulate_Pool()
    {
        foreach (PoolNode item in Pool_List)// loop through the pool
        {
            Destroy(item.Obj.gameObject);
        }
        Pool_List.Clear();
    }
    /// <summary>
    /// this is to destroy the pool
    /// </summary>
    public void Destroy_Pool()
    {
        foreach (PoolNode item in Pool_List)// loop through the pool
        {
            Destroy(item.Obj.gameObject);
        }
        Pool_List.Clear();// clear the pool list
        Destroy(this);// destroy the pool
    }
    #endregion
    #region Operations
    /// <summary>
    /// this returns the first free node detected
    /// </summary>
    /// <returns></returns>
    public PoolNode Find_Free_Node()
    {
        foreach (PoolNode Temp_Node in Pool_List)
        {
            if (Temp_Node.Spawned == false && Temp_Node.Obj != null) // if the node is not spawned and is not null
            {
                return Temp_Node;
            }
        }
        return null;
    }
    /// <summary>
    /// this returns the a list of free node detected
    /// </summary>
    /// <returns></returns>
    public List<PoolNode> Find_Free_Node_List()
    {
        List<PoolNode> temp = new();
        foreach (PoolNode Temp_Node in Pool_List)
        {
            if (Temp_Node.Spawned == false && Temp_Node.Obj != null) // if the node is not spawned and is not null
            {
                temp.Add(Temp_Node);
            }
        }
        return temp;
    }
    /// <summary>
    /// this returns the number of empty nodes
    /// </summary>
    /// <returns></returns>
    public int Number_Of_Empty()
    {
        int _Num = 0;
        foreach (var item in Pool_List)
        {
            if (item != null && item.Spawned == false) // if the node is not null and is not spawned
            {
                _Num++;
            }
        }
        return _Num;
    }
    /// <summary>
    /// this returns the number of used nodes
    /// </summary>
    /// <returns></returns>
    public int Number_Of_Spawned()
    {
        int _Num = 0;
        foreach (var item in Pool_List)
        {
            if (item != null && item.Spawned) // if the node is not null and is spawned
            {
                _Num++;
            }
        }
        return _Num;
    }
    /// <summary>
    /// this is to check if the pool is full
    /// </summary>
    /// <returns></returns>
    public bool Is_Full()
    {
        int Spawned = 0;

        if (Pool_List.Count != 0)// if the pool is not empty
        {
            foreach (PoolNode Temp_Node in Pool_List) // loop through the pool
            {
                if (Temp_Node.Spawned)// if the node is spawned
                {
                    Spawned++;// increment the number of spawned nodes
                }
            }
        }
        return (Spawned == Pool_List.Count);
    }
    /// <summary>
    /// this is to check if the pool is Empty
    /// </summary>
    /// <returns></returns>
    public bool Is_Empty()
    {
        if (Pool_List.Count != 0)// if the pool is not empty
        {
            foreach (PoolNode Temp_Node in Pool_List) // loop through the pool
            {
                if (Temp_Node.Spawned)// if the node is spawned
                {
                    return false;
                }
            }
        }
        return true;
    }

    #endregion
}
#region enum Type Handlers

[System.Flags]
// the type of spawn type handler
public enum Spawn_Type_Handler
{
    None = 0,
    Disable_Enable = 1,
}
[System.Flags]
// the particle system handler
public enum Particle_System_Handler
{
    playOnSpawn = (1),
    stopOnDespawn = (2),
    Rest_On_Spawn = (3)

}
[System.Flags]
// the audio system handler
public enum Audio_System_Handler
{
    playOnSpawn = (1),
    stopOnDespawn = (2)
    //Reset_On_Spawn = (1 << 2),
}
public enum Physics_Handler
{
    Create_Rigid_Body = (1),
    Ignore_Rigid_Body = (2)
}
#endregion