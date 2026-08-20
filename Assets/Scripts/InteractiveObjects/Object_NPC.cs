using UnityEngine;

public class Object_NPC : MonoBehaviour,IInteractable
{
    protected Transform player;//玩家的位置
    protected UI ui;
    protected Player_QuestManager questManager;

    [Header("Quest Info")]
    [SerializeField]private string npcTargetQuestId;
    [SerializeField]private RewardType rewardNpc;
    [Space]
    [SerializeField] private Transform npc;//npc的位置
    [SerializeField] private GameObject interactToolTip;//交互提示
    private bool facingRight = true;

    [Header("Floaty Tooltip")]
    [SerializeField] private float floatSpeed = 8f;//漂浮速度
    [SerializeField] private float floatRange = .1f;//漂浮幅度
    private Vector3 startPosition;

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPosition = interactToolTip.transform.position;
        interactToolTip.SetActive(false);
    }
    protected virtual void Start()
    {
        questManager=Player.instance.questManager;
    }

    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }

    private void HandleToolTipFloat()//交互提示图标的上下悬浮效果
    {
        if (interactToolTip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactToolTip.transform.position = startPosition + new Vector3(0, yOffset);
        }
    }

    private void HandleNpcFlip()//npc翻转(朝向玩家)
    {
        if (player == null || npc == null)
            return;

        if (npc.position.x > player.position.x && facingRight)
        {
            npc.transform.Rotate(0, 180,0);
            facingRight = false;
        }
        else if (npc.position.x < player.position.x && facingRight == false)
        {
            npc.transform.Rotate(0, 180, 0);
            facingRight = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactToolTip.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactToolTip.SetActive(false);
    }

    public virtual void Interact()
    {
        questManager.AddProgress(npcTargetQuestId);
        questManager.TryGiveRewardFrom(rewardNpc);
    }
}
