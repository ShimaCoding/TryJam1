using UnityEngine;

public class Misc_RandomSpriteFlipping : MonoBehaviour {

    bool axis;
    float timer;
    public float speed = 0.2f;
    public SpriteRenderer render;

    private void Start () {
        timer = speed;
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            if (axis)
                render.flipX = !render.flipX;
            else render.flipY = !render.flipY;
            axis = !axis;
            timer = speed;
        }
    }
}
