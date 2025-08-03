using UnityEngine;
using core;
using System.Collections;
namespace view
{
    public class BoxView : MonoBehaviour
    {
        public Box box;
        public void Subscribe()
        {
            box.OnMove += Move;
            box.OnFall += Fall;
        }
        public void UnSubScribe()
        {
            box.OnMove -= Move;
            box.OnFall -= Fall;
        }
        public void Move(V2Int targetPosition)
        {
            transform.position = targetPosition.InvertYAxis().ToVector3();
        }
        public void Fall(V2Int targetPosition)
        {
            transform.position = targetPosition.InvertYAxis().ToVector3();
            StartCoroutine(FallCoroutine());
        }
        public void OnDestroy()
        {
            UnSubScribe();
        }

        IEnumerator FallCoroutine()
        {
            WaitForFixedUpdate wait = new WaitForFixedUpdate();

            float fallTime = .5f;
            float timer = 0;
            while (timer < fallTime)
            {
                timer += Time.fixedDeltaTime;
                if (timer > fallTime)
                {
                    timer = fallTime;
                }
                float progress = timer / fallTime;
                //TODO: smooth lerp ? 

                float scale = Mathf.Lerp(1, 0, progress);
                transform.localScale = new Vector3(scale, scale, scale);

                yield return wait;
            }

            Destroy(this.gameObject);
        }
    }
}