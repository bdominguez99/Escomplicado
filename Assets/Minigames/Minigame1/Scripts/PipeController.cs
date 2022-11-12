using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace pipes
{
    public enum PipeType { TR, RB, BL, TL };

    public class PipeController : MonoBehaviour
    {
        [SerializeField] private GameObject[] pipes;
        [SerializeField] private int maxCols = 8, maxRows = 8;
        [SerializeField] private float fillTime = 0.5f, disableTime = 0.5f, waitBeforeOverflow = 0.5f, range = 0.25f, overlapCircleSize = 0.1f;
        [SerializeField] private Color disabledColor, enabledColor, switchColor;

        private int[][] pipeMtrx;

        private void Start()
        {
            pipeMtrx = new int[maxRows][];
            for (int i = 0; i < maxRows; i++)
                pipeMtrx[i] = new int[maxCols];
        }

        public float getOverflowTime()
        {
            return waitBeforeOverflow;
        }

        public float getFillTime()
        {
            return fillTime;
        }

        public Color getDisabledColor()
        {
            return disabledColor;
        }

        public void initialiceMtrx()
        {
            for (int i = 0; i < maxRows; i++)
            {
                for (int j = 0; j < maxCols; j++)
                {
                    if (pipeMtrx[i][j] == 0)
                    {
                        pipeMtrx[i][j] = Random.Range(0, pipes.Length);
                    }
                }
            }
            Pipe script;
            for (int i = 0; i < maxRows; i++)
            {
                for (int j = 0; j < maxCols; j++)
                {
                    script = Instantiate(pipes[pipeMtrx[i][j]], transform.position + new Vector3(j, i, 0), Quaternion.identity, transform).GetComponent<Pipe>();
                    script.setVariables(fillTime, disableTime, waitBeforeOverflow, range, overlapCircleSize);
                    script.setColors(disabledColor, enabledColor, switchColor);
                }
            }
        }

        public void destroyMtrx()
        {
            if (pipeMtrx != null)
            {
                for (int i = 0; i < maxRows; i++)
                {
                    for (int j = 0; j < maxCols; j++)
                    {
                        pipeMtrx[i][j] = 0;
                    }
                }
            }
            GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
            foreach (var pipe in pipes)
            {
                Destroy(pipe);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 rect = new Vector3(maxCols, maxRows, 0);
            Gizmos.DrawWireCube(transform.position + rect/2 - Vector3.one * 0.5f, rect);
        }
    }
}