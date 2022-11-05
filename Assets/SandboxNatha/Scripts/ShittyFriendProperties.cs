using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyFriendProperties : MonoBehaviour
{
 
    public enum ShittyFriendType {Attack,Heal}
    public ShittyFriendType type;
    private Player player;

    public int number;
    private bool attached = false;
    public float distanceFromPlayer = 1.5f;
    public float distanceBetweenFriends = 1.2f;

    private Vector3 targetPosition;
    public float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    private GameObject previousShittyFriend;

    public float floatAmplitude = 0.05f;
    public float floatFrequency = .3f;
    private float delay;
    private Vector3 floatyness;

    //attack
    //Pour test
    public GameObject projectile;
    //heal
    public float healthHealed=10;


    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        if (attached)
        {
            if (number == 0)
            {
                transform.LookAt(player.transform);
                targetPosition = player.transform.position - distanceFromPlayer * transform.forward;
            }
            else
            {
                previousShittyFriend = player.GetComponent<Player>().GetShittyFriend(number-1);
                transform.LookAt(previousShittyFriend.transform);
                targetPosition = previousShittyFriend.transform.position -  distanceBetweenFriends * transform.forward;
            }
            delay = number * Mathf.PI / 3;
            floatyness = new Vector3(0, floatAmplitude * Mathf.Sin(2 * Mathf.PI* floatFrequency*Time.time-delay),0);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition + floatyness, ref velocity, smoothTime);

            if (transform.position.y < -0.2f)
            {
                transform.position = new Vector3(transform.position.x,-0.2f,transform.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !attached)
        {
            player = other.GetComponent<Player>();
            number = player.shittyFriendsList.Count;
            other.GetComponent<Player>().AddShittyFriend(gameObject);
            attached = true;
        }
    }

    public void usePower()
    {
        switch (type) {
            case ShittyFriendType.Attack:
                Transform playerTransform = GameObject.Find("Player").GetComponent<Transform>();
                Rigidbody rb = Instantiate(projectile, playerTransform.position, playerTransform.rotation).GetComponent<Rigidbody>();
                rb.AddForce(playerTransform.forward * 32f, ForceMode.Impulse);
                break;
            case ShittyFriendType.Heal:
                player.HealPlayer(healthHealed);
                break;
        }
    }

}
