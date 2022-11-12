using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace pipes
{
    public class PipeStart : MonoBehaviour
    {
        [SerializeField] private Vector2 outputDir;
        [SerializeField] private RectTransform longPipe;
        [SerializeField] private Image pipeEnd, pipeEndTop, longPipeTop;
        [SerializeField] private Transform overflowStart;
        [SerializeField] private float longPipeFillingTime = 3f, overlapCircleSize = 0.4f;

        private PipeController pipeController;
        private float initLongPieSize;

        private void Start()
        {
            pipeController = FindObjectOfType<PipeController>();
            pipeEndTop.color = pipeController.getDisabledColor();
            longPipeTop.color = pipeController.getDisabledColor();
            initLongPieSize = longPipe.sizeDelta.y;
            longPipe.sizeDelta = new Vector2(longPipe.sizeDelta.x, 0);
        }

        private void overflow()
        {
            Collider2D collision = Physics2D.OverlapCircle((Vector2)overflowStart.position + outputDir, overlapCircleSize);
            Pipe pipe = collision ? collision.GetComponent<Pipe>() : null;
            if (collision && pipe != null && pipe.canConnect(outputDir))
            {
                pipe.setInputDirection(overflowStart.position);
            }
            else
            {
                FindObjectOfType<GameController>().goToNextLevel();
            }
        }

        public void resetPipe()
        {
            longPipe.sizeDelta = new Vector2(longPipe.sizeDelta.x, 0);
            pipeEnd.fillAmount = 0;
        }

        public IEnumerator startfilling()
        {
            float progress = 0f, fillTime = pipeController.getFillTime(), overflowTime = pipeController.getOverflowTime();
            yield return new WaitForSeconds(overflowTime);
            while (progress < longPipeFillingTime)
            {
                progress += Time.deltaTime * (GameController.isPaused ? 0 : 1);
                longPipe.sizeDelta = new Vector2(longPipe.sizeDelta.x, initLongPieSize * (progress / longPipeFillingTime));
                yield return null;
            }
            longPipe.sizeDelta = new Vector2(longPipe.sizeDelta.x, initLongPieSize);
            progress = 0f;
            yield return new WaitForSeconds(overflowTime);
            while (progress < 1)
            {
                progress += (Time.deltaTime / fillTime) * (GameController.isPaused ? 0: 1);
                pipeEnd.fillAmount = progress;
                yield return null;
            }
            pipeEnd.fillAmount = 1;
            yield return new WaitForSeconds(overflowTime);
            overflow();
        }
    }
}