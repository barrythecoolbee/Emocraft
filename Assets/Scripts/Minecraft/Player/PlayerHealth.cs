using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    bool destroy = false;
    public AudioSource killMonster1;
    public AudioSource killMonster2;
    RaycastHit hitInfo;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        AudioSource killMonster;

        if (Random.Range(0f, 1f) < 0.5f)
        {
            killMonster = killMonster1;
        }
        else
        {
            killMonster = killMonster2;
        }
        

        if (Input.GetButtonUp("Fire1"))
        {
            if (Camera.main)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                LayerMask monsterLayer = LayerMask.GetMask("Monsters");

                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 5f, monsterLayer))
                {
                    Debug.Log(hitInfo.collider.gameObject.tag);

                    if (hitInfo.collider.gameObject.tag == "Monster")
                    {
                        if (!killMonster.isPlaying)
                        {
                            killMonster.Play();
                        }
                        hitInfo.collider.gameObject.GetComponentInChildren<ParticleSystem>().Play();
                        hitInfo.collider.transform.Find("Body").gameObject.SetActive(false);
                        StartCoroutine(wait(hitInfo.collider.gameObject));
                    }
                }
            }
        }
    }

    IEnumerator wait(GameObject monster)
    {
        yield return new WaitForSeconds(1f);
        Destroy(monster);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Monster")
        {
            currentHealth -= 5;
            healthBar.setHealth(currentHealth);

            if(currentHealth <= 0)
            {
                Debug.Log("ENTREI");
                Application.Quit();
                Time.timeScale = 0;
            }
        }
    }
}
