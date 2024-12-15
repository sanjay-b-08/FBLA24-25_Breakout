using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillsScript : MonoBehaviour
{
    // Start is called before the first frame update

    public LayerMask coins;
    public LayerMask guard;

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

    void Start()
    {
        bribeMoney = 0;
        onKillCooldown = false;

        killColorAlpha = killFillCirc.color;
        bribeImageAlpha = bribeImage.color;

        //GuardPursue gp = new GuardPursue();
    }

    // Update is called once per frame
    void Update()
    {
        killFillCirc.color = killColorAlpha;
        bribeImage.color = bribeImageAlpha;

        collectCoin(transform.position);
        kill(getClosestEnemyInRange());
        bribe(getClosestEnemyInRange());

        bribeMoneyText.SetText("Bribe Money: " + bribeMoney);
        killCountText.SetText("Kill Count: " + killCount);

        if (killFillCirc.fillAmount < 8)
        {
            killFillCirc.fillAmount += 0.00064f;
        }

        if (bribeMoney >= bribeCost)
        {
            bribeText.alpha = 1f;
        } else
        {
            bribeText.alpha = 0.4f;
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
                Destroy(collider.gameObject);
            }
        }
    }

    private void kill(GameObject g)
    {
        if (g != null && Vector3.Distance(transform.position, g.transform.position) < 4f && onKillCooldown == false)
        {
            if (canUseSkill(g)) {
                killColorAlpha.a = 1f;

                if (Input.GetKeyDown(KeyCode.X))
                {
                    GuardPursue.moveSpeed += 0.32f;
                    Destroy(g.gameObject);
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
        if (g != null && Vector3.Distance(transform.position, g.transform.position) < 4f)
        {
            /*if (bribeMoney >= bribeCost)
            {
                bribeImageAlpha.a = 1f;
            } */

            if (canUseSkill(g))
            {
                if (bribeMoney >= bribeCost)
                {
                    bribeImageAlpha.a = 1f;

                    if (Input.GetKeyDown(KeyCode.Z))
                    {
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
        g.gameObject.GetComponent<GuardPursue>().movable = true;
        g.gameObject.GetComponent<GuardPursue>().canCatch = true;

        //Cooldown for the act of bribing
        yield return new WaitForSeconds(bribeCooldown);
    }

    IEnumerator killCD()
    {
        onKillCooldown = true;
        killFillCirc.fillAmount = 0;
        killText.alpha = 0.2f;
        yield return new WaitForSeconds(killCooldown);
        killText.alpha = 1;
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
            foreGround  // Optional layer mask
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
