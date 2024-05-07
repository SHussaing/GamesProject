using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public bool hasSlider;
    public Slider throwSlider;

    [Header("Sound")]
    public AudioSource throwSound;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;
    private float throwCooldownTimer;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        readyToThrow = true;

    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }

        if(throwCooldownTimer > 0)
        {
            throwCooldownTimer -= Time.deltaTime;
        }
        if (hasSlider)
        {
            throwSlider.value = Mathf.Clamp01(1 - throwCooldownTimer / throwCooldown);
        }
    }

    private void Throw()
    {
        if (throwCooldownTimer > 0)
        {
            return;
        }
        else throwCooldownTimer = throwCooldown;

        readyToThrow = false;
        //play sound
        if(throwSound != null)
        {
            throwSound.Play();
        }
        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
