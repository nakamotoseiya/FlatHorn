using Unity.VisualScripting;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField] private int damageAmout = 10;
    [SerializeField] private float damageInterval = 0.1f;  //何秒ごとにダメージ

    private float timer = 0;

	private　void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            if(timer>=damageInterval)
            {
                PlayerHP playerHP = other.GetComponent<PlayerHP>();
                if(playerHP!=null)
                {
                    playerHP.TakeDamage(damageAmout);

				}
                timer = 0f;

			}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            timer = 0f;
        }
    }
}
