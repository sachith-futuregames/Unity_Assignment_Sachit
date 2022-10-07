using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class WeaponsManager : MonoBehaviour
{
    //States
    [SerializeField] private WeaponsData _currentWeapon;// Remove Serialise Field Before Final
    [SerializeField] private MeshFilter _launcherMesh;
    [SerializeField] private BoxCollider _meleeTrigger;
    [SerializeField] private Player_Controller _pc;
    private GameObject _weaponParticle;
    private bool bIsRayCasting = false;
    private RaycastHit RayWeapon = new RaycastHit();
    public int TeamID;
    public delegate void AttackComplete();
    public AttackComplete OnAttackComplete;

    public void Update()
    {
        ShowRayParticle();
    }

    //Getter Settter functions
    public void SetCurrentWeapon(WeaponsData InNewWeapon)
    {
        _currentWeapon = InNewWeapon;
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
        if(_currentWeapon.TypeWeapon == EWeaponType.Melee)
        {
            StartMeleeAttack();
        }
        else if(_currentWeapon.TypeWeapon == EWeaponType.RayCast)
        {
            StartRaycastAttack();
        }
    }

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

    private void StartRaycastAttack()
    {
        bool bDidHit = Physics.Raycast(_pc.FPPCameraTarget.transform.position, Camera.main.transform.forward, out RayWeapon, _currentWeapon.Distance);
        Debug.Log(bDidHit);
        if (RayWeapon.collider && bDidHit)
        {
            Debug.Log(RayWeapon.collider.GetType());
            Player_Controller HitEnemy = RayWeapon.collider.gameObject.gameObject.GetComponent<Player_Controller>();
            if (HitEnemy && HitEnemy.TeamID != TeamID)
            {
                HitEnemy.TakeDamage(_currentWeapon.DamageAmt);
                _weaponParticle = _currentWeapon.ParticleToEmit;
                _weaponParticle.transform.position = _pc.FPPCameraTarget.transform.position;
                bIsRayCasting = true;
            }
        }

        
        
    }
    
    private void ShowRayParticle()
    {
        if(_currentWeapon.TypeWeapon == EWeaponType.RayCast && bIsRayCasting)
        {
            _weaponParticle.transform.position = Vector3.Lerp(_weaponParticle.transform.position, RayWeapon.point, Time.deltaTime);
            if((RayWeapon.point - _weaponParticle.transform.position).sqrMagnitude < 1.0f)
            {

            }
        }
    }

    private void StopRaycastAttack()
    {
        bIsRayCasting = false;
        Destroy(_weaponParticle);
        _weaponParticle = null;
        OnAttackComplete();
    }

}
