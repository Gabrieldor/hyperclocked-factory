using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneTrackerUI : MonoBehaviour
{
    public static MilestoneTrackerUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private Transform  contentRoot;

    private readonly List<MilestoneNodeUI> _cards = new();

    private static readonly Color CardBg        = new Color(0.14f, 0.14f, 0.17f, 1f);
    private static readonly Color DetailBg      = new Color(0.10f, 0.10f, 0.13f, 1f);
    private static readonly Color SubtextColor  = new Color(0.60f, 0.60f, 0.65f, 1f);

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        panel.SetActive(false);
    }

    private void Start()
    {
        MilestoneManager.OnMilestoneUnlocked += OnMilestoneUnlocked;
        // Cards built on first open so ContentSizeFitter can measure active elements
    }

    private void OnDestroy()
    {
        MilestoneManager.OnMilestoneUnlocked -= OnMilestoneUnlocked;
    }

    public void Toggle()
    {
        bool open = !panel.activeSelf;
        panel.SetActive(open);
        if (open) OpenPanel();
    }

    public void Close() => panel.SetActive(false);

    private void OpenPanel()
    {
        if (_cards.Count == 0) BuildCards();
        else foreach (var c in _cards) c.Refresh();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRoot as RectTransform);
    }

    // ── Card builder ──────────────────────────────────────────────────────────

    private void BuildCards()
    {
        foreach (Transform child in contentRoot) Destroy(child.gameObject);
        _cards.Clear();

        var mgr = MilestoneManager.Instance;
        if (mgr == null) return;

        foreach (var m in mgr.AllMilestones)
            _cards.Add(CreateCard(m));
    }

    private MilestoneNodeUI CreateCard(MilestoneData data)
    {
        // ── Card root ─────────────────────────────────────────────────────────
        var cardGo = new GameObject("Card_" + data.milestoneName, typeof(RectTransform));
        cardGo.transform.SetParent(contentRoot, false);

        var cardLayout = cardGo.AddComponent<VerticalLayoutGroup>();
        cardLayout.childControlWidth      = true;
        cardLayout.childControlHeight     = true;
        cardLayout.childForceExpandWidth  = true;
        cardLayout.childForceExpandHeight = false;
        cardLayout.padding                = new RectOffset(10, 10, 8, 8);
        cardLayout.spacing                = 4f;

        cardGo.AddComponent<LayoutElement>().minHeight = 72f;
        var cardBgImg = cardGo.AddComponent<Image>();
        cardBgImg.color = CardBg;

        var cardBtn = cardGo.AddComponent<Button>();
        cardBtn.transition    = Selectable.Transition.None;
        cardBtn.targetGraphic = cardBgImg;

        // ── Name label ────────────────────────────────────────────────────────
        var nameGo  = new GameObject("NameLabel", typeof(RectTransform));
        nameGo.transform.SetParent(cardGo.transform, false);
        var nameTmp = nameGo.AddComponent<TextMeshProUGUI>();
        nameTmp.text      = data.milestoneName;
        nameTmp.fontSize  = 16;
        nameTmp.fontStyle = FontStyles.Bold;
        nameGo.AddComponent<LayoutElement>().minHeight = 24f;

        // ── Condition label ────────────────────────────────────────────────────
        var condGo  = new GameObject("ConditionLabel", typeof(RectTransform));
        condGo.transform.SetParent(cardGo.transform, false);
        var condTmp = condGo.AddComponent<TextMeshProUGUI>();
        condTmp.text     = "";
        condTmp.fontSize = 13;
        condTmp.color    = SubtextColor;
        condGo.AddComponent<LayoutElement>().minHeight = 20f;

        // ── Detail panel (hidden) ─────────────────────────────────────────────
        var detailGo = new GameObject("DetailPanel", typeof(RectTransform));
        detailGo.transform.SetParent(cardGo.transform, false);
        detailGo.AddComponent<Image>().color = DetailBg;

        var detailLayout = detailGo.AddComponent<HorizontalLayoutGroup>();
        detailLayout.padding           = new RectOffset(8, 8, 6, 6);
        detailLayout.childControlWidth = true;
        detailLayout.childControlHeight = true;
        detailGo.AddComponent<LayoutElement>().minHeight = 28f;

        var rewardGo  = new GameObject("RewardLabel", typeof(RectTransform));
        rewardGo.transform.SetParent(detailGo.transform, false);
        var rewardTmp = rewardGo.AddComponent<TextMeshProUGUI>();
        rewardTmp.text     = "";
        rewardTmp.fontSize = 12;
        rewardTmp.color    = new Color(0.85f, 0.85f, 0.85f);

        // ── MilestoneNodeUI component ─────────────────────────────────────────
        var nodeUI = cardGo.AddComponent<MilestoneNodeUI>();
        SetPrivate(nodeUI, "background",      cardBgImg);
        SetPrivate(nodeUI, "nameLabel",       nameTmp);
        SetPrivate(nodeUI, "conditionLabel",  condTmp);
        SetPrivate(nodeUI, "detailPanel",     detailGo);
        SetPrivate(nodeUI, "rewardLabel",     rewardTmp);

        cardBtn.onClick.AddListener(nodeUI.OnTap);

        nodeUI.Init(data);
        return nodeUI;
    }

    private static void SetPrivate(object target, string field, object value)
    {
        var f = target.GetType().GetField(field,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        f?.SetValue(target, value);
    }

    // ── Refresh ───────────────────────────────────────────────────────────────

    private void OnMilestoneUnlocked(MilestoneData _)
    {
        foreach (var card in _cards) card.Refresh();
    }
}
