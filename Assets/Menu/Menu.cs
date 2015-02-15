using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	[System.Serializable]
	public class Tab {
		[Space(10)]
		[Header("Title:")]
		public string title;
		public Font titleFont;
		public int titleFontSize;
		public Color titleTextColor;
		public static Vector2 titleAnchorMin = new Vector2(0.175f, 0.75f);
		public static Vector2 titleAnchorMax = new Vector2(0.5f, 0.85f);
		[Space(10)]
		[Header("Options:")]
		public string[] options;
		public Font optionFont;
		public int optionFontSize;
		public Color optionTextColor;
		public static Vector2 optionAnchorMin = new Vector2(0.225f, 0.55f);
		public static Vector2 optionAnchorMax = new Vector2(0.5f, 0.65f);
		public static Vector2 optionOffset = new Vector2(0f, -0.1f);
		public Sprite optionSprite;
		public Sprite optionSpriteTransparent;
	}

	[Header("Tabs:")]
	public string[] tabs;
	public Font tabFont;
	public int tabFontSize;
	public Color tabTextColor;
	public GameObject tab;
	private Vector2 tabAnchorMin = new Vector2(0.2f, 0.05f);
	private Vector2 tabAnchorMax = new Vector2(0.25f, 0.079f);
	private Vector2 tabSpriteAnchorMin = new Vector2(0.075f, 0.03f);
	private Vector2 tabSpriteAnchorMax = new Vector2(0.362f, 0.102f);
	private Vector2 tabOffset = new Vector2(0.2f, 0f);
	private Vector2 tabSpriteOffset = new Vector2(0.215f, 0f);
	private float tabObjectOffset = 90.15f;
	public Sprite tabSpriteLeft;
	public Sprite tabSpriteRight;
	public Sprite tabSpriteTransparent;

	public Tab[] theseTabsTho;

	private GameObject[] tabButtons;
	private GameObject[] optionButtons;
	private int selectedOption;
	private int selectedTab;
	private int max;

	// Use this for initialization
	void Start () {
		selectedTab = 0;
		refreshMenu ();
	}
	
	// Update is called once per frame
	void OnGui () {
		for (int i = 0; i < tabButtons.Length; i++) {
			tabButtons[i].GetComponent<Button>().interactable = true;
		}
		tabButtons[selectedTab].GetComponent<Button>().interactable = false;
		refreshMenu();
	}

	public void refreshMenu() {
		makeTitle();
		makeOptions();
		drawTabs();
		makeTabs();
	}

	public void makeTitle() {
		GameObject titleText = new GameObject();
		Text textComponent;
		titleText.transform.parent = gameObject.transform;
		titleText.AddComponent<Text>();
		textComponent = titleText.GetComponent<Text>();
		textComponent.font = theseTabsTho[selectedTab].titleFont;
		textComponent.text = theseTabsTho[selectedTab].title;
		titleText.name = theseTabsTho[selectedTab].title + " (Title)";
		textComponent.color = theseTabsTho[selectedTab].titleTextColor;
		textComponent.alignment = TextAnchor.MiddleLeft;
		textComponent.resizeTextForBestFit = true;
		textComponent.resizeTextMaxSize = theseTabsTho[selectedTab].titleFontSize;
		textComponent.resizeTextMinSize = theseTabsTho[selectedTab].titleFontSize;
		textComponent.rectTransform.anchorMin = Tab.titleAnchorMin;
		textComponent.rectTransform.anchorMax = Tab.titleAnchorMax;
		textComponent.rectTransform.localScale = Vector3.one;
		textComponent.rectTransform.anchoredPosition = new Vector2(50f, 0f);
	}
	
	public void makeButtons(int number, Vector2 anchorMin, Vector2 anchorMax, Vector2 offset, Sprite sprite, Sprite transparentSprite) {
		GameObject[] buttons = new GameObject[number];
		Button buttonComponent;
		Image imageComponent;
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i] = new GameObject();
			buttons[i].name = "Button " + (i + 1);
			buttons[i].transform.parent = gameObject.transform;
			buttons[i].AddComponent<Button>();
			buttons[i].AddComponent<Image>();
			imageComponent = buttons[i].GetComponent<Image>();
			imageComponent.sprite = transparentSprite;
			buttonComponent = buttons[i].GetComponent<Button>();
			buttonComponent.image = buttons[i].GetComponent<Image>();
			buttonComponent.transition = Selectable.Transition.SpriteSwap;
			SpriteState highlightSprite = buttonComponent.spriteState;
			highlightSprite.highlightedSprite = sprite;
			buttonComponent.spriteState = highlightSprite;
			RectTransform rect = buttons[i].GetComponent<RectTransform>();
			rect.anchorMin = anchorMin +  i * offset + new Vector2(-.025f, 0.018f);
			rect.anchorMax = anchorMax + i * offset + new Vector2(-0.05f, -0.04f);
			rect.localScale = Vector3.one;
			rect.offsetMax = Vector2.zero;
			rect.offsetMin = Vector2.zero;
		}
	}

	public void makeOptions() {
		
		GameObject[] text = new GameObject[theseTabsTho[selectedTab].options.Length];
		Text textComponent;
		// deal with the j
		for (int j = 0; j < theseTabsTho[selectedTab].options.Length; j++) {
			text[j] = new GameObject();
			text[j].transform.parent = gameObject.transform;
			text[j].AddComponent<Text>();
			textComponent = text[j].GetComponent<Text>();
			textComponent.font = theseTabsTho[selectedTab].optionFont;
			textComponent.text = theseTabsTho[selectedTab].options[j];
			text[j].name = theseTabsTho[selectedTab].options[j];
			textComponent.color = theseTabsTho[selectedTab].optionTextColor;
			textComponent.alignment = TextAnchor.MiddleLeft;
			textComponent.resizeTextForBestFit = true;
			textComponent.resizeTextMaxSize = theseTabsTho[selectedTab].optionFontSize;
			textComponent.resizeTextMinSize = theseTabsTho[selectedTab].optionFontSize;
			textComponent.rectTransform.anchorMin = Tab.optionAnchorMin +  j * Tab.optionOffset;
			textComponent.rectTransform.anchorMax = Tab.optionAnchorMax +  j * Tab.optionOffset;
			textComponent.rectTransform.localScale = Vector3.one;
			textComponent.rectTransform.anchoredPosition = new Vector2(50f, 0f);
		}
		makeButtons(theseTabsTho[selectedTab].options.Length, Tab.optionAnchorMin, Tab.optionAnchorMax, Tab.optionOffset, theseTabsTho[selectedTab].optionSprite, theseTabsTho[selectedTab].optionSpriteTransparent);
	}

	public void makeTabs() {
		GameObject[] text = new GameObject[tabs.Length];
		Text textComponent;
		// deal with the j
		for (int j = 0; j < tabs.Length; j++) {
			text[j] = new GameObject();
			text[j].transform.parent = gameObject.transform;
			text[j].AddComponent<Text>();
			textComponent = text[j].GetComponent<Text>();
			textComponent.font = tabFont;
			textComponent.text = tabs[j];
			text[j].name = tabs[j];
			textComponent.color = tabTextColor;
			textComponent.alignment = TextAnchor.MiddleLeft;
			textComponent.resizeTextForBestFit = true;
			textComponent.resizeTextMaxSize = tabFontSize;
			textComponent.resizeTextMinSize = tabFontSize;
			textComponent.rectTransform.anchorMin = tabAnchorMin +  j * tabOffset;
			textComponent.rectTransform.anchorMax = tabAnchorMax +  j * tabOffset;
			textComponent.rectTransform.localScale = Vector3.one;
			textComponent.rectTransform.anchoredPosition = new Vector2(50f, 0f);
		}
		tabButtons = new GameObject[tabs.Length];
		Button buttonComponent;
		Image imageComponent;
		for (int i = 0; i < tabButtons.Length; i++) {
			tabButtons[i] = new GameObject();
			tabButtons[i].name = "Button " + (i + 1);
			tabButtons[i].transform.parent = gameObject.transform;
			tabButtons[i].AddComponent<Button>();
			tabButtons[i].AddComponent<Image>();
			imageComponent = tabButtons[i].GetComponent<Image>();
			imageComponent.sprite = tabSpriteTransparent;
			buttonComponent = tabButtons[i].GetComponent<Button>();
//			buttonComponent.onClick = new 
			buttonComponent.image = tabButtons[i].GetComponent<Image>();
			buttonComponent.transition = Selectable.Transition.SpriteSwap;
			SpriteState highlightSprite = buttonComponent.spriteState;
			if (i > selectedTab) {
				highlightSprite.highlightedSprite = tabSpriteRight;
			} else  if (i < selectedTab) {
				highlightSprite.highlightedSprite = tabSpriteLeft;
			} else {
				highlightSprite.highlightedSprite = tabSpriteTransparent;
				buttonComponent.interactable = false;
			}
			buttonComponent.spriteState = highlightSprite;
			RectTransform rect = tabButtons[i].GetComponent<RectTransform>();
			rect.anchorMin = tabSpriteAnchorMin +  i * tabSpriteOffset;
			rect.anchorMax = tabSpriteAnchorMax +  i * tabSpriteOffset;
			rect.localScale = Vector3.one;
			rect.offsetMax = Vector2.zero;
			rect.offsetMin = Vector2.zero;
		}
	}

	public void drawTabs() {
		GameObject[] tabPlanes = new GameObject[theseTabsTho.Length];
		for (int i = 0; i < theseTabsTho.Length; i++) {
			tabPlanes[i] = (GameObject)Instantiate(tab);
			tabPlanes[i].name = "Tab " + (i + 1);
			tabPlanes[i].transform.position += new Vector3(i * tabObjectOffset, 0f, 0f);
			if (i == selectedTab) {
				tabPlanes[i].transform.position += 2 * Vector3.back;
			}
		}
	}
}
