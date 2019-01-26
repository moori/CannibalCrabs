using DG.Tweening;
using UnityEngine;

public class Meat : MonoBehaviour
{
    public AnimationCurve spriteScaleCurve;
    public SpriteRenderer shadowSprite;
    public SpriteRenderer meatSprite;

    public void Go(Vector2 target)
    {
        float time = 0;
        DOTween.To(() => time, x =>
        {
            time = x;
            meatSprite.transform.localScale = Vector2.one * (1 + spriteScaleCurve.Evaluate(time) * .2f);
            meatSprite.transform.localPosition = Vector2.up * spriteScaleCurve.Evaluate(time) * 3f;
            shadowSprite.transform.localScale = Vector2.one * (3f - spriteScaleCurve.Evaluate(time) * 1.5f);
        }, 1, .8f);
        transform.DOMove(target, 0.8f).OnComplete(() =>
        {
            GetComponent<Collider2D>().enabled = true;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player.canEat)
            {
                player.Eat();
                Destroy(gameObject);
            }
        }
    }
}
