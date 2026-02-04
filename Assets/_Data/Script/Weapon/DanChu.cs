using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DanChu : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    public AudioClip shootClip,reloadingClip;
    public AudioClip noButton;
    public GameObject casingBullet;
    public float damage = 100f;
    bool canFire = true;
    float shootCount = 0f;
    public Transform casingPos;
    public ParticleSystem[] particle;
    public GameObject lightFX;
   
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        Reload();
    }
    void Shoot()
    {
        shootCount -= Time.deltaTime;
        if (shootCount > 0)  return;
        if (Input.GetMouseButtonDown(0)&& canFire)
        {
            //StartCoroutine(effectShooting());

            EffectShooting();
            audioSource.PlayOneShot(shootClip);
            CasingBullet();
            animator.SetTrigger("Shoot");
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f));
            if (Physics.Raycast(ray, out var hitInfo) )
            {
              
                hitInfo.transform.GetComponent<Animal>()?.TakeDamge(damage);
                hitInfo.transform.GetComponent<BossCtl>()?.TakeDamge(damage);
            }
            shootCount = 0.5f;

        }
    }
    void EffectShooting()
    {
        lightFX.SetActive(true);  
        foreach (var p in particle)
        {
            p.Emit(1);
        }
        Invoke(nameof(TurnOfflightEffect), 0.1f);
      

    }
    void TurnOfflightEffect()
    {
        lightFX.SetActive(false);

    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            audioSource.PlayOneShot(reloadingClip);
            animator.SetTrigger("Reloading");
            canFire = false;
            StartCoroutine(WaitStatesToFire());
        }
    }
    void CasingBullet()
    {
        GameObject casing = Instantiate<GameObject>(casingBullet);
        casing.transform.position = casingPos.position;
        casing.transform.rotation = casingPos.rotation;
        //casing.GetComponent<Rigidbody>().AddForce(this.transform.right*2f+ this.transform.up * 2f, ForceMode.Impulse);

        Vector3 ejectForce =
    transform.right * Random.Range(1.5f, 2.5f) +
    transform.up * Random.Range(1f, 2f);

        casing.GetComponent<Rigidbody>().AddForce(ejectForce, ForceMode.Impulse);
        Destroy(casing, 5f);
    }
    IEnumerator WaitStatesToFire()
    {

        yield return new WaitForSeconds(3);
        canFire = true;
    }
    }
