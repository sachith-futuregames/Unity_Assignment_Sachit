using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private DamageScript _damageScript;
    public float LifeTime;
    public delegate void FOnExplode(Vector3 InPosition);
    public FOnExplode OnExplode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Explode(gameObject.transform.position);
    }

    public void SetupProjectile(int TeamID, int InDamage, float InLifeTime)
    {
        LifeTime = InLifeTime;
        _damageScript.Damage = InDamage;
        _damageScript.TeamID = TeamID;
    }

    public void Shoot(Vector3 InDirection)
    {
        GetComponent<Rigidbody>().AddForce(InDirection, ForceMode.Impulse);
        StartTimer();
    }

    private void StartTimer()
    {
        Invoke("StopTimer", LifeTime);
    }

    private void StopTimer()
    {
        Explode(gameObject.transform.position);
    }

    private void Explode(Vector3 InPosition)
    {
        _damageScript.SendDamage();
        OnExplode(InPosition);
        Destroy(gameObject);
    }

  
}
