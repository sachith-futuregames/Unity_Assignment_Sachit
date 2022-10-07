using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EWeaponType
{
    Melee,
    RayCast,
    Projectile,
    AirDrop,
    None,
}

[CreateAssetMenu(fileName ="WeaponsData", menuName ="DataObjects/WeaponsData", order =1)]
public class WeaponsData : ScriptableObject
{
    [Header("General Data")]
    public EWeaponType TypeWeapon;
    public int DamageAmt;
    public Mesh LauncherMesh;
    public GameObject ParticleToEmit;
    public string WeaponName;
    public Image Icon;

    [Header("Raycast Data")]
    public float Distance;

    [Header("Projectile Data")]
    public GameObject ProjectilePrefab;

    [Header("AirDrop Data")]
    public int NoOfDrops;
    public GameObject DropPrefab;


}
