using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace pipes
{
    public class Pipe : MonoBehaviour
    {
        [SerializeField] private bool T, R, B, L, isCorner;
        [SerializeField] private Image image;

        private Vector2 outputDir, inputDir;
        private Color disabledColor, enabledColor, switchColor;
        private Image pipe;
        private float fillTime, disableTime, waitBeforeOverflow, range, overlapCircleSize;
        private bool fillClockwise, isPositionLocked, isPreparedToSwitch;

        private void Start()
        {
            pipe = GetComponent<Image>();
        }

        public bool canConnect(Vector2 outPutDirection)
        {
            Vector2 inputDirection = outPutDirection * -1;
            if (isCorner)
            {
                if (inputDirection.y > range)
                    return T;
                if (inputDirection.x > range)
                    return R;
                if (inputDirection.y < -range)
                    return B;
                if (inputDirection.x < -range)
                    return L;
            }
            else
            {
                if (inputDirection.y > range)
                    return B;
                if (inputDirection.x > range)
                    return L;
                if (inputDirection.y < -range)
                    return T;
                if (inputDirection.x < -range)
                    return R;
            }
            return false;
        }

        public void setVariables(float fillTime, float disableTime, float waitBeforeOverflow, float range, float overlapCircleSize)
        {
            this.fillTime = fillTime;
            this.disableTime = disableTime;
            this.waitBeforeOverflow = waitBeforeOverflow;
            this.range = range;
            this.overlapCircleSize = overlapCircleSize;
        }

        public void setColors(Color disabledColor, Color enabledColor, Color switchColor)
        {
            this.disabledColor = disabledColor;
            this.enabledColor = enabledColor;
            this.switchColor = switchColor;
        }

        public void setInputDirection(Vector2 pipePos)
        {
            inputDir = pipePos - (Vector2)transform.position;
            verifyFillDirection();
            isPositionLocked = true;
            if (isPreparedToSwitch && !GameController.isSiwtching)
            {
                GameController.switchStart = null;
                pipe.color = enabledColor;
                isPreparedToSwitch = false;
            }
            StartCoroutine(switchToDisableColor());
            StartCoroutine(fillPipe());
        }

        public void tryPrepareToSwitch()
        {
            if (!isPositionLocked && !GameController.isSiwtching) {
                if (GameController.switchStart != null)
                {
                    GameController.switchEnd = gameObject;
                    FindObjectOfType<GameController>().trySwitchPipes();
                }
                else
                {
                    if (isPreparedToSwitch)
                    {
                        isPreparedToSwitch = false;
                        GameController.switchStart = null;
                        pipe.color = enabledColor;
                    }
                    else
                    {
                        isPreparedToSwitch = true;
                        GameController.switchStart = gameObject;
                        pipe.color = switchColor;
                    }
                }
            }
        }

        public void resetAfterSwitch()
        {
            pipe.color = enabledColor;
            isPreparedToSwitch = false;
        }

        public void prepareColorToSwitch()
        {
            pipe.color = switchColor;
        }

        private void verifyFillDirection()
        {
            if (isCorner)
            {
                if (inputDir.y > range)
                    outputDir = ((fillClockwise = R) ? 1 : -1) * Vector2.right;
                else if (inputDir.x > range)
                    outputDir = ((fillClockwise = B) ? 1 : -1) * Vector2.down;
                else if (inputDir.y < -range)
                    outputDir = ((fillClockwise = L) ? 1 : -1) * Vector2.left;
                else if (inputDir.x < -range)
                    outputDir = ((fillClockwise = T) ? 1 : -1) * Vector2.up;
                image.fillClockwise = !fillClockwise;
            }
            else
            {
                if (Mathf.Abs(inputDir.y) > range)
                {
                    outputDir = ((inputDir.y > range) ? -1 : 1) * Vector2.up;
                    image.fillOrigin = (inputDir.y < range) ? -1 : 1;
                }
                else if (Mathf.Abs(inputDir.x) > range)
                {
                    outputDir = ((inputDir.x > range) ? -1 : 1) * Vector2.right;
                    image.fillOrigin = (inputDir.x < range) ? -1 : 1;
                }
            }
        }

        private void overflow()
        {
            Collider2D collision = Physics2D.OverlapCircle((Vector2)transform.position + outputDir, overlapCircleSize);
            if (collision)
            {
                if (collision.CompareTag("Pipe"))
                {
                    Pipe pipe = collision.GetComponent<Pipe>();
                    if (pipe.canConnect(outputDir))
                        pipe.setInputDirection(transform.position);
                    else
                        FindObjectOfType<GameController>().goToNextLevel();
                }
                else if (collision.CompareTag("PipeAns"))
                    collision.GetComponent<PipeAns>().selectAnswer();
                else
                    Debug.LogWarning("There is one pipe without tag!");
            }
            else
                FindObjectOfType<GameController>().goToNextLevel();
        }

        private IEnumerator switchToDisableColor()
        {
            float progress = 0;
            while(progress < 1)
            {
                progress += Time.deltaTime / disableTime;
                pipe.color = Color.Lerp(enabledColor, disabledColor, progress);
                yield return null;
            }
            pipe.color = disabledColor;
        }

        private IEnumerator fillPipe()
        {
            float progress = 0;
            while (progress < 1)
            {
                progress += (Time.deltaTime / fillTime) * (GameController.isPaused ? 0f : 1f);
                image.fillAmount = progress;
                yield return null;
            }
            image.fillAmount = 1;
            yield return new WaitForSeconds(waitBeforeOverflow);
            overflow();
        }
    }
}