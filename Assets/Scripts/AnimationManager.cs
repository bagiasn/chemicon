using UnityEngine;

public class AnimationManager : MonoBehaviour {

    private Animator anim;
    
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	void Update () {
        float h = Input.GetAxis("Horizontal");              // setup h variable as our horizontal input axis
        float v = Input.GetAxis("Vertical");                // setup v variables as our vertical input axis
        anim.SetFloat("Speed", v);                          // set our animator's float parameter 'Speed' equal to the vertical input axis				
        anim.SetFloat("Direction", h);                      // set our animator's float parameter 'Direction' equal to the horizontal input axis	
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hydrogen" || other.tag == "Oxygen" || other.tag == "Calcium" || other.tag == "Carbon")
        {
            GameController controller = FindObjectOfType<GameController>();
            controller.updateColliderTag(other.tag, other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hydrogen" || other.tag == "Oxygen" || other.tag == "Calcium" || other.tag == "Carbon")
        {
            GameController controller = FindObjectOfType<GameController>();
            controller.updateColliderTag("", other.gameObject);
        }
    }
}
