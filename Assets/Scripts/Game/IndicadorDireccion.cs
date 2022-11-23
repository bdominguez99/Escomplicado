using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicadorDireccion : MonoBehaviour
{
    [System.Serializable]
    private class PositionInFloor{
        public Transform transform;
        public int floor;
        public bool deactivateAfterPass;
    }

    [SerializeField] private Transform player, classroomReference;
    [SerializeField] private float rango = 0.5f;
    [SerializeField] private Color floorColor;
    [SerializeField] private PositionInFloor[] positions;
    [SerializeField] private Image[] floorLayers;
    [SerializeField] private StoryController storyController;

    private PositionInFloor target;
    private SpriteRenderer spriteRenderer;
    private Mapa mapa;
    private Vector2 distance;
    private Color startColor;
    private bool showArrow, arrowDisabled;
    private int minijuegoActual;

    private void Start()
    {
        startColor = floorLayers[0].color;
        spriteRenderer = GetComponent<SpriteRenderer>();
        mapa = FindObjectOfType<Mapa>();
    }

    void Update()
    {
        distance = target.transform.position - transform.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(distance.y, distance.x) * (180/Mathf.PI) - 45);
        transform.position = player.position;
        verifyShowingArrow();
    }

    public void setMinigame(int idMinijuego, float score = -1)
    {
        if (score >= 0.6f)
        {
            minijuegoActual = idMinijuego + 1;
            if (minijuegoActual <= 5)
            {
                if (target.deactivateAfterPass)
                    target.transform.gameObject.SetActive(false);
                target = positions[minijuegoActual];
                if (target.deactivateAfterPass)
                    target.transform.gameObject.SetActive(true);
                storyController.passMinigame();
                setArrowVisibility(!positions[idMinijuego].deactivateAfterPass);
                verifyFloorlayersVisivility();
            }
            else
            {
                arrowDisabled = true;
            }
        }
        else{
            minijuegoActual = idMinijuego;
            target = positions[idMinijuego];
            if(score == -1) for(int i = 0; i < positions.Length; i++)
            {
                if(i == idMinijuego)
                {
                    if (positions[i].deactivateAfterPass)
                        positions[i].transform.gameObject.SetActive(true);
                }
                else
                {
                    if (positions[i].deactivateAfterPass)
                        positions[i].transform.gameObject.SetActive(false);
                }
            }
            setArrowVisibility(player.position.x < classroomReference.position.x);
            verifyFloorlayersVisivility();
        }
    }

    public void setArrowVisibility(bool showArrow)
    {
        this.showArrow = showArrow;
    }

    public void verifyFloorlayersVisivility()
    {
        int pisoActual = mapa.getActiveFloor();
        if(target.floor == pisoActual)
        {
            foreach (var layer in floorLayers)
            {
                layer.color = new Color(1, 1, 1, 0);
            }
        }
        else
        {
            for (int i = 0; i < floorLayers.Length; i++)
            {
                if (i == target.floor)
                    floorLayers[i].color = floorColor;
                else if (i == pisoActual)
                    floorLayers[i].color = Color.white;
                else
                    floorLayers[i].color = startColor;
            }
        }
    }

    private bool isSameFloor()
    {
        return mapa.getActiveFloor() == target.floor;
    }

    private void verifyShowingArrow()
    {
        if (!arrowDisabled && showArrow && isSameFloor() && Vector2.Distance(transform.position, target.transform.position) > rango)
            spriteRenderer.color = new Color(1, 1, 1, 1);
        else
            spriteRenderer.color = new Color(1, 1, 1, 0);
    }
}
