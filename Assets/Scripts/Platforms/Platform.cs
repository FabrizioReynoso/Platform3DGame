using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    protected GameManager m_gameManager;
    protected LevelManager m_levelManager;
    protected Player m_player;
 
    public virtual void Start(){

        m_gameManager = GameManager.instance;
        m_levelManager = LevelManager.instance;
        m_player = Player.instance;
    }
}
