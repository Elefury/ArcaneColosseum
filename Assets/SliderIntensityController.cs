using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderIntensityController : MonoBehaviour, IPointerDownHandler
{
    public Light directionalLight; // Ссылка на ваш Directional Light

    public void OnPointerDown(PointerEventData eventData)
    {
        // Этот метод не используется для обработки изменения значения слайдера,
        // так как он вызывается при нажатии, а не изменении значения.
    }

    public void OnValueChanged(float value) // Метод для обработки изменения значения
    {
        directionalLight.intensity = value; // Устанавливаем интенсивность света равной значению слайдера
    }
}