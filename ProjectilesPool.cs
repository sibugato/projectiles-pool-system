using System;
using UnityEngine;

public class ProjectilesPool : ObjectsPool
{
    // enum names indexes must correspond to indexes of arrays from parent (any array[0] must be for pistol projectile in this example, array[1] for assaultRifle e.t.c.) 
    public enum Type
    {
        pistol,
        assaultRifle,
        shotgun,
        rifle,
        bazooka,
        grenadeLauncher,
        minigun
    }

    // for realization the singleton pattern
    public static ProjectilesPool PoolOfProjectiles;

    // only player can shoot, so there is always only one spot to spawn bullets
    private Transform _gunPosition;

    // for screen shaking
    private Transform _activeCamera;
    private Vector3 _cameraOriginalPosition;

    private void Awake()
    {
        // Unity duplicate GameObject when calling "DontDestroyOnLoad()", so you need destroy any copies
        if (PoolOfProjectiles == null)
        {
            PoolOfProjectiles = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        _typeEnumNamesCount = Enum.GetNames(typeof(Type)).Length;
        CreateStartingPool();
    }

    // New stage is a new scene, so you need set current camera and player weapon transform when new stage loaded
    public void SetNewStage(Transform activeCamera, Transform gunPosition)
    {
        _activeCamera = activeCamera;
        _gunPosition = gunPosition;
        _cameraOriginalPosition = activeCamera.position;
    }

    public void SpawnProjectile(Type type, Vector2 shootDirection)
    {
        // enum to int for interactions with arrays
        int projectileTypeIndex = (int)type; 

        Quaternion projectileRotation = Quaternion.Euler(0, shootDirection.x > 0 ? 0 : 180, 0);
        Vector3 projectileSpawnPosition = _gunPosition.position;

        GameObject projectile;

        if (_poolsArray[projectileTypeIndex].Count > 0)
        {
            projectile = _poolsArray[projectileTypeIndex].Pop();
            projectile.transform.position = projectileSpawnPosition;
            projectile.transform.rotation = projectileRotation;
        }
        else
        {
            projectile = Instantiate(_prefabsArray[projectileTypeIndex], projectileSpawnPosition, projectileRotation, transform);
        }
        // ProjectileController class missing in this example. This class response for projectile logic, like it's movement, collision control, e.t.c.
        projectile.GetComponent<ProjectileController>().SetStartingParameters(shootDirection, _activeCamera, _cameraOriginalPosition);
    }

    public void ReturnObjectIntoPool(GameObject returnedObject, Type type)
    {
        int projectileTypeIndex = (int)type;
        returnedObject.SetActive(false);
        _poolsArray[projectileTypeIndex].Push(returnedObject);
    }
}
