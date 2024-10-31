using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// PoolNode is a class that is used to store data about the object in the Pool therefore a pool is made of PoolNode 
/// <param name="Pool_Node"></param>
/// </summary>
public class PoolNode // this class is to spawn objects using pooling or simple instantiation
{
    #region Data
    [Header("Pool Node")]
    [Tooltip("The ID of the object in the pool")]
    public float ID;//this is the ID of the object in the pool
    [Tooltip("The object in the pool")]
    public GameObject Obj;//this is the object in the pool
    [Tooltip("The state of the object in the pool")]
    public bool Spawned;//this is the state of the object in the pool
    [Tooltip("The audio of the object in the pool")]
    public List<AudioSource> _Audio;//this is the audio of the object in the pool
    [Tooltip("The particle of the object in the pool")]
    public List<ParticleSystem> _Particle;//this is the particle of the object in the pool
    [Tooltip("The rigid body of the object in the pool")]
    public Rigidbody _Rigid_body;//this is the rigid body of the object in the pool

    [Tooltip("The position that the object will spawn at")]
    public Vector3 Spawn_Position;
    [Tooltip("The scale that the object will spawn at")]
    public Vector3 Spawn_Scale;
    [Tooltip("The rotation that the object will spawn at")]
    public Quaternion Spawn_Rotation;
    #endregion

    #region Constructors    
    /// <summary>
    /// PoolNode is the default constructor
    /// </summary>
    public PoolNode()
    {
        Spawned = false;
        Obj = null;
        _Audio = null;
        _Particle = null;
        Spawn_Position = new();
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Spawned = true;
    }
    /// <summary>
    /// PoolNode is a multi parameter constructor
    /// </summary>
    /// <param name="obj"></param>
    public PoolNode(GameObject obj)
    {
        Spawned = false;
        Obj = obj;
        _Audio = null;
        _Particle = null;
        Spawn_Position = new();
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Spawned = true;
    }
    /// <summary>
    /// PoolNode is a multi parameter constructor
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="spawned"></param>
    public PoolNode(GameObject obj, bool spawned)
    {
        Spawned = spawned;
        Obj = obj;
        _Audio = null;
        _Particle = null;
        Spawn_Position = new();
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Spawned = true;
    }
    /// <summary>
    /// PoolNode is a multi parameter constructor
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="spawned"></param>
    /// <param name="spawn_Position"></param>
    public PoolNode(GameObject obj, bool spawned, Vector3 spawn_Position)
    {
        Spawned = spawned;
        Obj = obj;
        _Audio = null;
        _Particle = null;
        Spawn_Position = spawn_Position;
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Spawned = true;
    }
    /// <summary>
    /// PoolNode is a multi parameter constructor
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="spawned"></param>
    /// <param name="spawn_Position"></param>
    /// <param name="spawn_Scale"></param>
    public PoolNode(GameObject obj, bool spawned, Vector3 spawn_Position, Vector3 spawn_Scale)
    {
        Spawned = spawned;
        Obj = obj;
        _Audio = null;
        _Particle = null;
        Spawn_Position = spawn_Position;
        Spawn_Scale = spawn_Scale;
        Spawn_Rotation = new();
        Spawned = true;
    }
    /// <summary>
    /// PoolNode is a multi parameter constructor
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="spawned"></param>
    /// <param name="spawn_Position"></param>
    /// <param name="spawn_Scale"></param>
    /// <param name="spawn_Rotation"></param>
    public PoolNode(GameObject obj, bool spawned, Vector3 spawn_Position, Vector3 spawn_Scale, Quaternion spawn_Rotation)
    {
        Spawned = spawned;
        Obj = obj;
        _Audio = null;
        _Particle = null;
        Spawn_Position = spawn_Position;
        Spawn_Scale = spawn_Scale;
        Spawn_Rotation = spawn_Rotation;
        Spawned = true;
    }
    public PoolNode(GameObject obj, bool spawned, Vector3 spawn_Position, Vector3 spawn_Scale, Quaternion spawn_Rotation, Rigidbody rb)
    {
        Spawned = spawned;
        Obj = obj;
        _Audio = null;
        _Particle = null;
        Spawn_Position = spawn_Position;
        Spawn_Scale = spawn_Scale;
        Spawn_Rotation = spawn_Rotation;
        Spawned = true;
        _Rigid_body = rb;
    }

    #endregion
}