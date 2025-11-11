using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CastingSpells : MonoBehaviour {

    [SerializeField] private InputActionProperty castSpellButton1;
    [SerializeField] private InputActionProperty castSpellButton2;
    [SerializeField] private InputActionProperty castSpellButton3;
    [SerializeField] private InputActionProperty castSpellButton4;

    [SerializeField] private float globalCooldown = 1f; // Общий кулдаун для всех спеллов
    [SerializeField] private float[] spellCooldowns = new float[4] { 6.0f, 9.0f, 15.0f, 20.0f }; // Индивидуальные кулдауны для каждого спелла

    [SerializeField] private float raycastDistance = 5f; // Дистанция Raycast
    


    private float lastCastTime = -1.0f; // Время последнего использования любого спелла
    private float[] lastSpellCastTimes; // Время последнего использования для каждого спелла

    private void Start() {
        // Инициализируем массив времени последнего использования для каждого спелла
        lastSpellCastTimes = new float[4] { -6.0f, -9.0f, -15.0f, -20.0f };
    }



    private void Update() {

        // Проверяем, прошло ли достаточно времени с момента последнего использования любого спелла (глобальный кулдаун)
        if (castSpellButton1.action.WasPressedThisFrame() || castSpellButton2.action.WasPressedThisFrame() || castSpellButton3.action.WasPressedThisFrame() || castSpellButton4.action.WasPressedThisFrame()) {
            if (Time.time - lastCastTime >= globalCooldown) {

                if (castSpellButton1.action.WasPressedThisFrame()) {
                    if (IsSpellReady(0)) {
                        CastSpell(0);
                    } else {
                        Debug.Log("Frostblast перезаряжается.. (еще " + (spellCooldowns[0] - (Time.time - lastSpellCastTimes[0])) + ")");
                    }
                }

                if (castSpellButton2.action.WasPressedThisFrame()) {
                    if (IsSpellReady(1)) {
                        CastSpell(1);
                    } else {
                        Debug.Log("Fireblast перезаряжается..  (еще " + (spellCooldowns[1] - (Time.time - lastSpellCastTimes[1])) + ")");
                    }
                }

                if (castSpellButton3.action.WasPressedThisFrame()) {
                    if (IsSpellReady(2)) {
                        CastSpell(2);
                    } else {
                        Debug.Log("Earthshock перезаряжается..  (еще " + (spellCooldowns[2] - (Time.time - lastSpellCastTimes[2])) + ")");
                    }
                }

                if (castSpellButton4.action.WasPressedThisFrame()) {
                    if (IsSpellReady(3)) {
                        CastSpell(3);
                    } else {
                        Debug.Log("Lightflash перезаряжается..  (еще " + (spellCooldowns[3] - (Time.time - lastSpellCastTimes[3])) + ")");
                    }
                }
            } else {
                Debug.Log("Global Cooldown (" + (globalCooldown - (Time.time - lastCastTime)) + ")");
            }
        }
    }

    private bool IsSpellReady(int spellIndex) {
        // Проверяем, прошло ли достаточно времени с момента последнего использования этого спелла
        return Time.time - lastSpellCastTimes[spellIndex] >= spellCooldowns[spellIndex];
    }

    private void CastSpell(int spellIndex) {
    // Обновляем время последнего использования любого спелла
    lastCastTime = Time.time;

    // Обновляем время последнего использования этого конкретного спелла
    lastSpellCastTimes[spellIndex] = Time.time;

    // Вызываем Raycast для обнаружения объекта
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance)) {
        GameObject target = hit.collider.gameObject;

        // Проверяем тег объекта и сравниваем с уязвимостью к спеллу
        switch (spellIndex) {
            case 0: // Ледяной спелл
                if (target.CompareTag("Ice_Vulnerable") || target.CompareTag("Any_Vulnerable")) { 

                    if (target.CompareTag("Ice_Vulnerable")) {
                    GameManager.instance.AddScore(2); 
                    } else {
                    GameManager.instance.AddScore(1);
                    }

                    Destroy(target);
                    Debug.Log("Frostblast разрушил объект: " + target.name);                    

                }
                break;
            case 1: // Огненный спелл
                if (target.CompareTag("Fire_Vulnerable") || target.CompareTag("Any_Vulnerable")) {

                    if (target.CompareTag("Fire_Vulnerable")) {
                    GameManager.instance.AddScore(3); 
                    } else {
                    GameManager.instance.AddScore(1);
                    }

                    Destroy(target);
                    Debug.Log("Fireblast разрушил объект: " + target.name);                 
                }
                break;
            case 2: // Земляной спелл
                if (target.CompareTag("Earth_Vulnerable") || target.CompareTag("Any_Vulnerable")) {

                    if (target.CompareTag("Earth_Vulnerable")) {
                    GameManager.instance.AddScore(4); 
                    } else {
                    GameManager.instance.AddScore(1);
                    }

                    Destroy(target);
                    Debug.Log("Earthshock разрушил объект: " + target.name);
                }
                break;
            case 3: // Светлый спелл
                if (target.CompareTag("Radiant_Vulnerable") || target.CompareTag("Any_Vulnerable")) {

                    if (target.CompareTag("Radiant_Vulnerable")) {
                    GameManager.instance.AddScore(5); 
                    } else {
                    GameManager.instance.AddScore(1);
                    }

                    Destroy(target);
                    Debug.Log("Lightflash разрушил объект: " + target.name);
                }
                break;
        }
    }

    // Логика для визуализации Raycast (опционально)
    Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red, 1f);
}
}