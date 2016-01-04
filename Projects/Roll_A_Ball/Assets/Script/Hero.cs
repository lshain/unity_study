using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    public float force = 1.0f;
    public Text scoreText = null;
    public GameObject gameWin = null;

    private int mScore = 0;
    // Use this for initialization
    void Start()
    {
        mScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        GetComponent<Rigidbody>().AddForce(new Vector3(h, 0, v) * force);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            Destroy(other.gameObject);

            mScore++;
            if (scoreText != null)
            {
                scoreText.text = "Score: " + mScore.ToString();
            }

            if (mScore >= 8)
            {
                if (gameWin != null)
                {
                    gameWin.SetActive(true);
                }
            }
        }
    }
}
