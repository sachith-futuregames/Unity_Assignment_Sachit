using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class WeaponsManager : MonoBehaviour
{
    //States
    public WeaponsData[] Inventory;
    private WeaponsData _currentWeapon;
    [SerializeField] private MeshFilter _launcherMesh;
    [SerializeField] private BoxCollider _meleeTrigger;
    [SerializeField] private Player_Controller _pc;
    private GameObject _weaponParticle;
    private bool bIsRayCasting = false;
    private RaycastHit RayWeapon = new RaycastHit();
    public int TeamID;
    public delegate void AttackComplete();
    public AttackComplete OnAttackComplete;

    private void Awake()
    {
        SetCurrentWeapon(Inventory[0]);
    }

    public void Update()
    {
        ShowRayParticle();
    }

    //Getter Settter functions
    public void SetCurrentWeapon(WeaponsData InNewWeapon)
    {
        _currentWeapon = InNewWeapon;
        _weaponParticle = _currentWeapon.ParticleToEmit;
        SetLauncherMesh();
    }

    public WeaponsData GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    //Setup Functions
    private void SetLauncherMesh()
    {
        if(_currentWeapon.LauncherMesh)
        {
            _launcherMesh.mesh = _currentWeapon.LauncherMesh;
        }
        else
        {
            _launcherMesh.mesh = null;
        }
    }

    public void DeathState()
    {
        _launcherMesh.mesh = null;
    }

    
    //Monobehaviour Functions
    public void Start()
    {
        SetLauncherMesh();
    }

    public void Shoot()
    {
        switch(_currentWeapon.TypeWeapon)
        {
            case EWeaponType.Melee:
                StartMeleeAttack();
                break;
            case EWeaponType.RayCast:
                StartRaycastAttack();
                break;
            case EWeaponType.Projectile:
                StartProjectileAttack();
                break;
            case EWeaponType.AirDrop:
                break;
        }
    }

    //Melee Attack
    private void StartMeleeAttack()
    {
        _weaponParticle = Instantiate(_currentWeapon.ParticleToEmit);
        _weaponParticle.transform.parent = gameObject.transform;
        _weaponParticle.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        _meleeTrigger.gameObject.GetComponent<DamageScript>().TeamID = TeamID;
        _meleeTrigger.gameObject.GetComponent<DamageScript>().Damage = _currentWeapon.DamageAmt;
        _meleeTrigger.enabled = true;
        _meleeTrigger.gameObject.GetComponent<DamageScript>().SendDamage();
        Invoke("StopMeleeAttack", 1);
        

    }

    private void StopMeleeAttack()
    {
        
        _meleeTrigger.enabled = false;
        Destroy(_weaponParticle);
        _weaponParticle = null;
        OnAttackComplete();
    }

    //RayCast Attack 
    private void StartRaycastAttack()
    {
        bool bDidHit = Physics.Raycast(_pc.FPPCameraTarget.transform.position, Camera.main.transform.forward, out RayWeapon, _currentWeapon.Distance);
        if (RayWeapon.collider && bDidHit)
        {
           
            _weaponParticle = Instantiate(_currentWeapon.ParticleToEmit);
            _weaponParticle.transform.parent = null;
            _weaponParticle.transform.position = _pc.FPPCameraTarget.transform.position;
            bIsRayCasting = true;
        }
        else
        {
            OnAttackComplete();
        }
        
    }
    
    private void ShowRayParticle()
    {
        if(_currentWeapon.TypeWeapon == EWeaponType.RayCast && bIsRayCasting)
        {
            _weaponParticle.transform.position = Vector3.Lerp(_weaponParticle.transform.position, RayWeapon.point, Time.deltaTime * 4);
            if((RayWeapon.point - _weaponParticle.transform.position).sqrMagnitude < 1.0f)
            {
                StopRaycastAttack();
            }
        }
    }

    private void StopRaycastAttack()
    {
        Player_Controller HitEnemy = RayWeapon.collider.gameObject.gameObject.GetComponent<Player_Controller>();
        if(HitEnemy && HitEnemy.TeamID != TeamID)
        {
            HitEnemy.TakeDamage(_currentWeapon.DamageAmt);
        }
        bIsRayCasting = false;
        Destroy(_weaponParticle);
        _weaponParticle = null;
        OnAttackComplete();
    }

    //Projectile Attack
    private void StartProjectileAttack()
    {
        GameObject Projectile = Instantiate(_currentWeapon.ProjectilePrefab);
        Projectile.GetComponent<ProjectileBehaviour>().SetupProjectile(TeamID, _currentWeapon.DamageAmt, _currentWeapon.LifeTime);
        Projectile.GetComponent<ProjectileBehaviour>().OnExplode = Explosion;
        Projectile.transform.position = _pc.FPPCameraTarget.transform.position + (Camera.main.transform.forward * 10);
        Projectile.GetComponent<ProjectileBehaviour>().Shoot(Camera.main.transform.forward * 20);
    }

    public void Explosion(Vector3 InPosition)
    {
        _weaponParticle = Instantiate(_currentWeapon.ParticleToEmit);
        _weaponParticle.transform.position = InPosition;
        Invoke("StopProjectileAttack", 1.0f);
    }

    private void StopProjectileAttack()
    {
        Destroy(_weaponParticle);
        _weaponParticle = null;
        OnAttackComplete();
    }

    
}
