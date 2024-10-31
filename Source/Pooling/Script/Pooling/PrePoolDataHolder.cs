using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
[SerializeField]
/// <summary>
/// PrePoolDataHolder is a transitional class that is used to store data before it is passed to the pool manager since we add the pool queued node to the queue before we process it
/// <param name="PrePoolDataHolder"></param>
/// </summary>
public class PrePoolDataHolder
{
    #region Data
    [Tooltip("The prefab that will be spawned")]
    public GameObject Prefab;
    [Tooltip("The amount of objects that will be spawned")]
    public uint poolSize;
    [Tooltip("If the pool is allowed to grow")]
    public bool Allow_Growth;
    [Tooltip("The position that the object will spawn at")]
    public Vector3 Spawn_Position;
    [Tooltip("The scale that the object will spawn at")]
    public Vector3 Spawn_Scale;
    [Tooltip("The rotation that the object will spawn at")]
    public Quaternion Spawn_Rotation;
    [Tooltip("The parent that the object will spawn at")]
    public Transform Parent;
    [Tooltip("The audio system handler that will be used to play audio")]
    public Audio_System_Handler _Audio_System_Handler;
    [Tooltip("The particle system handler that will be used to play particles")]
    public Particle_System_Handler _Particle_Handler;
    [Tooltip("The type of spawn that will be used")]
    public Spawn_Type_Handler Type_Of_Spawn;
    [Tooltip("The physics handler that will be used")]
    public Physics_Handler physics_Handler;

    #endregion

    #region Constructors
    /// <summary>
    /// PrePoolDataHolder is the default constructor 
    /// </summary>
    public PrePoolDataHolder()
    {
        Prefab = null;
        poolSize = 0;
        Allow_Growth = false;
        Spawn_Position = new();
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Parent = null;
        _Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        _Particle_Handler = Particle_System_Handler.stopOnDespawn;
        Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        physics_Handler = Physics_Handler.Ignore_Rigid_Body;
    }
    /// <summary>
    /// PrePoolDataHolder is a multi parameter constructor 
    /// </summary>
    /// <param name="prefab"></param>
    public PrePoolDataHolder(GameObject prefab)
    {
        Prefab = prefab;
        poolSize = 0;
        Allow_Growth = false;
        Spawn_Position = new();
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Parent = null;
        _Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        _Particle_Handler = Particle_System_Handler.stopOnDespawn;
        Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        physics_Handler = Physics_Handler.Ignore_Rigid_Body;
    }
    /// <summary>
    /// PrePoolDataHolder is a multi parameter constructor 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="ps"></param>
    public PrePoolDataHolder(GameObject prefab, uint ps)
    {
        Prefab = prefab;
        poolSize = ps;
        Allow_Growth = false;
        Spawn_Position = new();
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Parent = null;
        _Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        _Particle_Handler = Particle_System_Handler.stopOnDespawn;
        Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        physics_Handler = Physics_Handler.Ignore_Rigid_Body;
    }
    /// <summary>
    /// PrePoolDataHolder is a multi parameter constructor 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="ps"></param>
    /// <param name="ag"></param>
    public PrePoolDataHolder(GameObject prefab, uint ps, bool ag)
    {
        Prefab = prefab;
        poolSize = ps;
        Allow_Growth = ag;
        Spawn_Position = new();
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Parent = null;
        _Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        _Particle_Handler = Particle_System_Handler.stopOnDespawn;
        Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        physics_Handler = Physics_Handler.Ignore_Rigid_Body;
    }
    /// <summary>
    /// PrePoolDataHolder is a multi parameter constructor 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="ps"></param>
    /// <param name="ag"></param>
    /// <param name="pos"></param>
    public PrePoolDataHolder(GameObject prefab, uint ps, bool ag, Vector3 pos)
    {
        Prefab = prefab;
        poolSize = ps;
        Allow_Growth = ag;
        Spawn_Position = pos;
        Spawn_Scale = new();
        Spawn_Rotation = new();
        Parent = null;
        _Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        _Particle_Handler = Particle_System_Handler.stopOnDespawn;
        Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        physics_Handler = Physics_Handler.Ignore_Rigid_Body;
    }
    /// <summary>
    /// PrePoolDataHolder is a multi parameter constructor 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="ps"></param>
    /// <param name="ag"></param>
    /// <param name="pos"></param>
    /// <param name="sca"></param>
    public PrePoolDataHolder(GameObject prefab, uint ps, bool ag, Vector3 pos, Vector3 sca)
    {
        Prefab = prefab;
        poolSize = ps;
        Allow_Growth = ag;
        Spawn_Position = pos;
        Spawn_Scale = sca;
        Spawn_Rotation = new();
        Parent = null;
        _Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        _Particle_Handler = Particle_System_Handler.stopOnDespawn;
        Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        physics_Handler = Physics_Handler.Ignore_Rigid_Body;
    }
    /// <summary>
    /// PrePoolDataHolder is a multi parameter constructor 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="ps"></param>
    /// <param name="ag"></param>
    /// <param name="pos"></param>
    /// <param name="sca"></param>
    /// <param name="rot"></param>
    public PrePoolDataHolder(GameObject prefab, uint ps, bool ag, Vector3 pos, Vector3 sca, Quaternion rot)
    {
        Prefab = prefab;
        poolSize = ps;
        Allow_Growth = ag;
        Spawn_Position = pos;
        Spawn_Scale = sca;
        Spawn_Rotation = rot;
        Parent = null;
        _Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        _Particle_Handler = Particle_System_Handler.stopOnDespawn;
        Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        physics_Handler = Physics_Handler.Ignore_Rigid_Body;
    }
    /// <summary>
    /// PrePoolDataHolder is a multi parameter constructor 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="ps"></param>
    /// <param name="ag"></param>
    /// <param name="pos"></param>
    /// <param name="sca"></param>
    /// <param name="rot"></param>
    /// <param name="pr"></param>
    public PrePoolDataHolder(GameObject prefab, uint ps, bool ag, Vector3 pos, Vector3 sca, Quaternion rot, Transform pr)
    {
        Prefab = prefab;
        poolSize = ps;
        Allow_Growth = ag;
        Spawn_Position = pos;
        Spawn_Scale = sca;
        Spawn_Rotation = rot;
        Parent = pr;
        _Audio_System_Handler = Audio_System_Handler.playOnSpawn;
        _Particle_Handler = Particle_System_Handler.stopOnDespawn;
        Type_Of_Spawn = Spawn_Type_Handler.Disable_Enable;
        physics_Handler = Physics_Handler.Ignore_Rigid_Body;
    }
    /// <summary>
    /// Destroy_Node is a function that destroys the prefab stored in the node and not the node itself
    /// </summary>
    public void Destroy_Node()
    {
        UnityEngine.Object.Destroy(Prefab);
    }
    #endregion
}