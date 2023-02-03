using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rootgenscript : MonoBehaviour
{
    Transform lastpoint;
    public float genDistance;
    public GameObject rootext;
    public GameObject explodefab;
    Queue<GameObject> nodes = new Queue<GameObject>();
    bool enraged = false;
    bool exploding;
    public float exptime;
    GameObject curr;
    // Start is called before the first frame update
    void Start()
    {
        GameObject baseloc = GameObject.FindGameObjectWithTag("base");
        lastpoint = baseloc.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform playerLocation = GetComponent<Transform>();
        float xdiff = playerLocation.position.x - lastpoint.position.x;
        float ydiff = playerLocation.position.y - lastpoint.position.y;
        if(genDistance*genDistance < (xdiff * xdiff) + (ydiff * ydiff))
        {
            float midx = (playerLocation.position.x + lastpoint.position.x) / 2;
            float midy = (playerLocation.position.y + lastpoint.position.y) / 2;
            float angle = Mathf.Rad2Deg * Mathf.Atan2(ydiff, xdiff);
            Vector3 pos = new Vector3(midx, midy, 0);
            curr = Instantiate(rootext, pos, Quaternion.Euler(0, 0, angle));
            lastpoint = curr.transform.GetChild(0);
            nodes.Enqueue(curr);
        }
        if (enraged && !exploding)
        {
            exploding = true;
            StartCoroutine(explode());
        }
    }
    IEnumerator explode()
    {
        GameObject expnode = nodes.Dequeue();
        Transform locationofexp = expnode.transform;
        GameObject expobj = Instantiate(explodefab, locationofexp.position, Quaternion.Euler(0,0,0));
        Destroy(expnode);
        yield return new WaitForSeconds(exptime);
        exptime *= 0.9f;
        Destroy(expobj);
        exploding = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("rootnode") && collision.gameObject != curr)
        {
            enraged = true;
        }
    }
}