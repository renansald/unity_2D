using System;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cherriesText;
    private int _cherriesCount = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Cherry"))
        {
            return;
        }
        
        Destroy(collision.gameObject);
        _cherriesCount++;
        _cherriesText.text = String.Concat("X", _cherriesCount);
    }
}   