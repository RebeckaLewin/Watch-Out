using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//A container for sprites, used for animations
public class SpriteContainer : MonoBehaviour, IDeselectHandler
{
    [SerializeField] private Sprite[] sprites = new Sprite[2];
    [SerializeField] private float animTime;

    public void ChangeSprite()
    {
        StopAllCoroutines();
        GetComponent<Image>().sprite = sprites[1];
        GetComponent<Image>().SetNativeSize();
        StartCoroutine(Change());
    }

    private IEnumerator Change()
    {
        yield return new WaitForSeconds(animTime);
        GetComponent<Image>().sprite = sprites[0];
        GetComponent<Image>().SetNativeSize();
    }
    public void OnDeselect(BaseEventData data)
    {
        StopAllCoroutines();
        GetComponent<Image>().sprite = sprites[0];
        GetComponent<Image>().SetNativeSize();
    }
}
