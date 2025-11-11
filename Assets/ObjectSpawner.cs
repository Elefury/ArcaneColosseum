using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public GameObject[] prefabs; // Массив префабов для генерации
    public float[] spawnIntervals = new float[] { 20f, 16f, 10f, 6f, 4f }; // Интервалы генерации для каждого объекта
    public float spawnRadius = 33f; // Радиус круглой зоны
    public Vector3 center = Vector3.zero; // Центр круглой зоны
    public float[] spawnHeights; // Высоты для каждого объекта

    private float[] timers; // Таймеры для каждого объекта

    public void ResetSpawner() {
        // Инициализируем таймеры
        timers = new float[prefabs.Length];
        for (int i = 0; i < timers.Length; i++) {
            timers[i] = spawnIntervals[i]; // Устанавливаем таймеры на начальные значения
        }

        // Проверяем, что массив высот имеет правильный размер
        if (spawnHeights.Length != prefabs.Length) {
            Debug.LogError("Количество высот должно совпадать с количеством префабов!");
            spawnHeights = new float[prefabs.Length]; // Создаём массив с правильным размером
        }
    }

    private void Update() {
        // Обновляем таймеры и генерируем объекты, если время пришло
        for (int i = 0; i < timers.Length; i++) {
            timers[i] -= Time.deltaTime;

            if (timers[i] <= 0f) {
                SpawnObject(i);
                timers[i] = spawnIntervals[i]; // Сбрасываем таймер
            }
        }
    }

    private void SpawnObject(int index) {
        // Генерируем случайную точку внутри круга
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPoint.x, spawnHeights[index], randomPoint.y) + center;

        // Генерируем случайный поворот только по оси Y
        float randomYRotation = Random.Range(0f, 360f); // Случайный угол от 0 до 360 градусов
        Quaternion randomRotation = Quaternion.Euler(-90f, randomYRotation, 0f); // Поворот только по Y

        // Создаем объект с рандомным поворотом
        Instantiate(prefabs[index], spawnPosition, randomRotation);
    }
}