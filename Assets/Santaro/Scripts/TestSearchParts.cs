using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSearchParts : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Parts") || true) //partsタグはまだ作ってません
        {
            collision.transform.Find("WoodTexture").gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Parts") || true)
        {
            collision.transform.Find("WoodTexture").gameObject.SetActive(false);
        }
    }
}
