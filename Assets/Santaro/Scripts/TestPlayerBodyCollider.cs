using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerBodyCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Parts") || true) //まだpartsタグを作っていません
        {
            Debug.Log("部品を回収"); //回収処理
        }
        Destroy(collision.gameObject);
    }
}
