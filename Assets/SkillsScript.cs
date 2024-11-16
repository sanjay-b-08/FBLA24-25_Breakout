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

    void Start()
    {
        bribeMoney = 0;

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
        if (g != null && Vector3.Distance(transform.position, g.transform.position) < 4f)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                GuardPursue.moveSpeed += 0.1f;
                Destroy(g.gameObject);
                killCount++;
            }
        }
    }

    private void bribe(GameObject g)
    {
        if (g != null && Vector3.Distance(transform.position, g.transform.position) < 4f)
        {
            if (Input.GetKeyDown(KeyCode.Z) && bribeMoney > bribeCost)
            {
                ///DO SOMETHING HERE FOR BRIBE
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
        yield return new WaitForSeconds(5f);
        g.gameObject.GetComponent<GuardPursue>().movable = true;

        yield return new WaitForSeconds(bribeCooldown);
    }


}
