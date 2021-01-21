using Manager;
using Models.Cell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    #region Properties

    public float speed = 20f;
    public Rigidbody2D rb;

    public GameObject target;
    public CellModel.CellColor color;

    public float damage;

    #endregion

    #region Start

    private void Start()
    {
        rb.velocity = transform.right * speed;

        Vector3 targ = target.transform.position;
                targ.z = 0f;

                targ.x = targ.x - transform.position.x;
                targ.y = targ.y - transform.position.y;

                float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    #endregion

    #region OnTriggerEnter2D

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == target)
        {
            collision.gameObject.GetComponent<CellModel>().TakeHit(color, damage);
            CellManager.amountOfSeeds--;
            Destroy(gameObject);
        }

        else if(collision.gameObject.tag == "Connection")
        {
            this.GetComponent<Seed>().speed *= 10;
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    #endregion
}
