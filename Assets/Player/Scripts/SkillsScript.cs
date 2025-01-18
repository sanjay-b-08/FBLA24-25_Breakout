using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillsScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Timer timer;

    public LayerMask coins;
    public LayerMask guard;

    public AudioSource caughtSound;
    private bool didCatchSoundPlay;

    public AudioSource killSound;
    public AudioSource bribeSound;
    public AudioSource coinSound;
    public AudioSource guardDeathSound;

    //BRIBE
    private int bribeMoney;
    public TextMeshProUGUI bribeMoneyText;
    public int bribeCost;
    public float bribeCooldown;

    public Image bribeImage;
    public TextMeshProUGUI bribeText;
    private Color bribeImageAlpha;


    //KILL
    public TextMeshProUGUI killCountText;
    public int killCount;
    public float killCooldown;
    private bool onKillCooldown;

    private Color killColorAlpha;
    public Image killFillCirc;
    public TextMeshProUGUI killText;

    public LayerMask foreGround;
    private float maxRayDistance = 4f;

    public bool isGameOver;
    public Animator screenBloodSplatter;
    private float deltaTime = 0.0f;

    //Blood splatters
    public GameObject blood;

    void Start()
    {
        bribeMoney = 0;
        onKillCooldown = false;
        isGameOver = false;

        killColorAlpha = killFillCirc.color;
        bribeImageAlpha = bribeImage.color;

        didCatchSoundPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        killFillCirc.color = killColorAlpha;
        bribeImage.color = bribeImageAlpha;

        collectCoin(transform.position);
        kill(getClosestEnemyInRange());
        bribe(getClosestEnemyInRange());

        bribeMoneyText.SetText("Bribe Money: " + bribeMoney);
        killCountText.SetText("Kill Count: " + killCount);

        if (killFillCirc.fillAmount < 1)
        {
            killFillCirc.fillAmount += (1 / (10 * fps));
        }

        if (bribeMoney >= bribeCost)
        {
            bribeText.alpha = 1f;
        } else
        {
            bribeText.alpha = 0.4f;
        }

        if (isGameOver)
        {
            if (!didCatchSoundPlay)
            {
                caughtSound.Play();
                didCatchSoundPlay = true;
            }
        }
    }

    private void collectCoin(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.7f, coins) != null)
        {

            Collider2D collider = Physics2D.OverlapCircle(targetPos, 0.7f, coins);

            SpriteRenderer sr = collider.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                bribeMoney += Random.Range((int)1, 4);
                coinSound.Play();
                Destroy(collider.gameObject);
            }
        }
    }

    private void kill(GameObject g)
    {
        if (g != null && Vector3.Distance(transform.position, g.transform.position) < 4f && onKillCooldown == false && isGameOver != true)
        {
            if (canUseSkill(g)) {
                killColorAlpha.a = 1f;

                if (Input.GetKeyDown(KeyCode.X))
                {
                    GuardPursue.moveSpeed += 0.32f;

                    timer.setTimeScore(50);

                    killSound.Play();
                    screenBloodSplatter.SetBool("isKilling", true);
                    guardDeathSound.Play();

                    Instantiate(blood, g.transform.position, gameObject.transform.rotation);
                    Destroy(g.gameObject);

                    killFillCirc.fillAmount = 0;
                    killCount++;
                    StartCoroutine(killCD());
                }
            } else {
                killColorAlpha.a = 0.4f;
            }
        } else
        {
            killColorAlpha.a = 0.4f;
        }
    }

    private void bribe(GameObject g)
    {
        if (g != null && Vector3.Distance(transform.position, g.transform.position) < 4f && isGameOver != true)
        {
            if (bribeMoney >= bribeCost)
            {
                bribeImageAlpha.a = 1f;
            }

            if (canUseSkill(g))
            {
                if (bribeMoney >= bribeCost)
                {
                    bribeImageAlpha.a = 1f;

                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        bribeSound.Play();
                        StartCoroutine(pause(g));
                        bribeMoney -= bribeCost;

                        bribeImageAlpha.a = 0.4f;
                    }
                }
            } else {
                bribeImageAlpha.a = 0.4f;
            }
        } else { 
           bribeImageAlpha.a = 0.4f;
        }
    }

    private GameObject getClosestEnemyInRange()
    {
        float distToClosestGuard = Mathf.Infinity;
        GameObject closestGuard = null;
        GameObject[] allGuards = GameObject.FindGameObjectsWithTag("Guard");

        if (allGuards.Length > 0)
        {
            foreach (GameObject go in allGuards)
            {
                float distToGuard = (go.transform.position - transform.position).sqrMagnitude;
                if (distToGuard < distToClosestGuard)
                {
                    distToClosestGuard = distToGuard;
                    closestGuard = go;
                }
            }
            return closestGuard;
        }
        return null;
    }

    IEnumerator pause(GameObject g)
    {
        g.gameObject.GetComponent<GuardPursue>().movable = false;
        g.gameObject.GetComponent<GuardPursue>().canCatch = false;
        //Cooldown for guard to stop moving
        yield return new WaitForSeconds(5f);
        if (g != null)
        {
            g.gameObject.GetComponent<GuardPursue>().movable = true;
            g.gameObject.GetComponent<GuardPursue>().canCatch = true;
        }

        //Cooldown for the act of bribing
        yield return new WaitForSeconds(bribeCooldown);
    }

    IEnumerator killCD()
    {
        onKillCooldown = true;
        //killFillCirc.fillAmount = 0;
        killText.alpha = 0.2f;
        yield return new WaitForSeconds(killCooldown);
        screenBloodSplatter.SetBool("isKilling", false);
        killText.alpha = 1;
        //killFillCirc.fillAmount = 8;
        onKillCooldown = false;
    }

    //Checks to see if there is an obstacle in between player and officer -> if yes -> can't kill or bribe
    private bool canUseSkill(GameObject g)
    {
        Vector2 direction = (g.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, g.transform.position);

        //Debug.DrawRay(transform.position, direction * Mathf.Min(distance, maxRayDistance), Color.red);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,  // Starting point
            direction,  // Direction
            Mathf.Min(distance, maxRayDistance),  // Maximum distance
            foreGround  // layer mask
        );

        if (hit.collider == null)
        {
            return true;
        }
        else
        {
            //Debug.Log("Obstacle in between");
            return false;
        }
    }
}
