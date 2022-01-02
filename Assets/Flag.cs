using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController player;
    Vector3 originpos;
    public static Flag Instance;
    public Text redscore;
    public Text bluesocre;
    public bool own;
    public int red;
    public int blue;
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    void Start()
    {
        own = false;
        originpos = transform.position;
        red = 0;
        blue = 0;
    }
    void Update()
    {
        redscore.text = red.ToString();
        bluesocre.text = blue.ToString();
        if (player)
        {
            transform.position = player.transform.position + new Vector3(0, 2, 0);
            own = true;
        }
        else
        {
            
            own = false;
            
        }
        
    }
    public void FlagMaster(PlayerController _player)
    {
        if (player == null)
        {
            player = _player;
        }
        else
        {
            return;
        }
    }

    public void FlagSetOrigin()
    {
        player = null;
        own = false;
    }

    public void GetScore(int team)
    {
        if (team == 0)
        {
            red += 1;
        }
        else if(team == 1)
        {
            blue += 1;
        }
        else
        {
            Debug.Log("Player Die");
        }
        player.GetComponent<PlayerController>().CurrenFlagtState = PlayerController.flagState.FLAGISFREE;
        player = null;
        
        transform.position = originpos;

    }

    public IEnumerator showScorewithReset()//나중에 득점하는 연출
    {
        yield return new WaitForSecondsRealtime(2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("BlueScoreUp") && player.GetComponent<PlayerController>().CompareTag("TeamRed"))
        {
            GetScore(0);
        }
        else if (other.CompareTag("RedScoreUp") && player.GetComponent<PlayerController>().CompareTag("TeamBlue"))
        {
            GetScore(1);
        }
    }
}
