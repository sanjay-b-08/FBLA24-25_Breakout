using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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


    //KILL
    public TextMeshProUGUI killCountText;
    public int killCount;
    public float killCooldown;
    private bool onKillCooldown;

    void Start()
    {
        bribeMoney = 0;
        onKillCooldown = false;

        //GuardPursue gp = new GuardPursue();
    }

    // Update is called once per frame
    void Update()
    {

        collectCoin(transform.position);
        kill(getClosestEnemyInRange());
        bribe(getClosestEnemyInRange());

        bribeMoneyText.SetText("Bribe Money: " + bribeMoney);
        killCountText.SetText("Kill Count: " + killCount);
    }

    private void collectCoin(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.5f, coins) != null)
        {

            Collider2D collider = Physics2D.OverlapCircle(targetPos, 0.5f, coins);

            SpriteRenderer sr = collider.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    bribeMoney += Random.Range((int)1, 4);
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    private void kill(GameObject g)
    {
        if (g != null && Vector3.Distance(transform.position, g.transform.position) < 4f && onKillCooldown == false)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                GuardPursue.moveSpeed += 0.25f;
                Destroy(g.gameObject);
                killCount++;
                StartCoroutine(killCD(g));
            }
        }
    }

    private void bribe(GameObject g)
    {
        if (g != null && Vector3.Distance(transform.position, g.transform.position) < 4f)
        {
            if (Input.GetKeyDown(KeyCode.Z) && bribeMoney > bribeCost)
            {
                StartCoroutine(pause(g));
                bribeMoney -= bribeCost;
            }
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
        yield return new WaitForSeconds(2f);
        g.gameObject.GetComponent<GuardPursue>().movable = true;
        g.gameObject.GetComponent<GuardPursue>().canCatch = true;

        //Cooldown for the act of bribing
        yield return new WaitForSeconds(bribeCooldown);
    }

    IEnumerator killCD(GameObject g)
    {
        onKillCooldown = true;
        yield return new WaitForSeconds(killCooldown);
        onKillCooldown = false;
    }


}
