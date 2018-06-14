using UnityEngine;

public class AnimationManager : MonoBehaviour {

    private Animator anim;
    
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	void FixedUpdate () {
        float h = Input.GetAxis("Horizontal");              // setup h variable as our horizontal input axis
        float v = Input.GetAxis("Vertical");                // setup v variables as our vertical input axis
        anim.SetFloat("Speed", v);                          // set our animator's float parameter 'Speed' equal to the vertical input axis				
        anim.SetFloat("Direction", h);                      // set our animator's float parameter 'Direction' equal to the horizontal input axis	
    }

    void OnCollisionEnter(Collision other)
    {
        string colTag = other.collider.tag;
        if (colTag == "Hydrogen" || colTag == "Oxygen" || colTag == "Calcium" || colTag == "Carbon")
        {
            GameController controller = FindObjectOfType<GameController>();
            controller.updateColliderTag(colTag, other.gameObject);
        }
    }

    void OnCollisionExit(Collision other)
    {
        string colTag = other.collider.tag;
        if (colTag == "Hydrogen" || colTag == "Oxygen" || colTag == "Calcium" || colTag == "Carbon")
        {
            GameController controller = FindObjectOfType<GameController>();
            controller.updateColliderTag("", other.gameObject);
        }
    }
}
