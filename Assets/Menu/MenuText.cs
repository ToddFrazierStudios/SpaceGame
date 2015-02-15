using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuText : MonoBehaviour {
	
	[Header("Title:")]
	public string title;
	public Font titleFont;
	public int titleFontSize;
	public Color titleTextColor;
//	private Vector2 titleAnchorMin = new Vector2(0.175f, 0.75f);
//	private Vector2 titleAnchorMax = new Vector2(0.3333f, 0.85f);
	[Space(10)]
	[Header("Options:")]
	public string[][] options;
	public Font optionFont;
	public int optionFontSize;
	public Color optionTextColor;
	public Vector2 optionAnchorMin;
	public Vector2 optionAnchorMax;
	public Vector2 optionOffset;
	public Sprite optionSprite;
	public Sprite optionSpriteTransparent;
	[Space(10)]
	[Header("Tabs:")]
	public string[] tabs;
	public Font tabFont;
	public int tabFontSize;
	public Color tabTextColor;
	public GameObject tab;
//	private Vector2 tabAnchorMin;
//	private Vector2 tabAnchorMax;
//	private Vector2 tabOffset;
	private float tabObjectOffset;
	
	public MenuTab[] menuTabs;

	private GameObject[] tabButtons;
//	private int selectedOption;
	private int selectedTab;
	//private int max;
	
	// Use this for initialization
	void Start () {
//		tabAnchorMin = new Vector2(0.2f, 0.05f);
//		tabAnchorMax = new Vector2(0.25f, 0.079f);
//		tabOffset = new Vector2(0.2f, 0f);
		tabObjectOffset = 90.15f;
		//max = options.Length;
//		selectedOption = 0;
		selectedTab = 0;
		initMenu();
		makeTabs();
	}
	
	// Update is called once per frame
	void OnGUI () {

	}
	
	public void initMenu() {
		//		makeText(options, optionFont, optionFontSize, optionTextColor, optionAnchorMin, optionAnchorMax, optionOffset);
		//		makeText(tabs, tabFont, tabFontSize, tabTextColor, tabAnchorMin, tabAnchorMax, tabOffset);
		//		makeText(title, titleFont, titleFontSize, titleTextColor, titleAnchorMin, titleAnchorMax, new Vector2(0f, 0f));
		//		makeButtons(options.Length, optionAnchorMin, optionAnchorMax, optionOffset, optionSprite, optionSpriteTransparent);
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
	
	public void makeText(string[] words, Font font, int fontSize, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 offset) {
		
		GameObject[] text = new GameObject[words.Length];
		Text textComponent;
		// deal with the j
		for (int j = 0; j < words.Length; j++) {
			text[j] = new GameObject();
			text[j].transform.parent = gameObject.transform;
			text[j].AddComponent<Text>();
			textComponent = text[j].GetComponent<Text>();
			textComponent.font = font;
			textComponent.text = words[j];
			text[j].name = words[j];
			textComponent.color = color;
			textComponent.alignment = TextAnchor.MiddleLeft;
			textComponent.resizeTextForBestFit = true;
			textComponent.resizeTextMaxSize = fontSize;
			textComponent.resizeTextMinSize = fontSize;
			textComponent.rectTransform.anchorMin = anchorMin +  j * offset;
			textComponent.rectTransform.anchorMax = anchorMax + j * offset;
			textComponent.rectTransform.localScale = Vector3.one;
			textComponent.rectTransform.anchoredPosition = new Vector2(50f, 0f);
			
		}
	}
	
	public void makeTabs() {
		GameObject[] tabPlanes = new GameObject[tabs.Length];
		for (int i = 0; i < tabs.Length; i++) {
			tabPlanes[i] = (GameObject)Instantiate(tab);
			tabPlanes[i].name = "Tab " + (i + 1);
			tabPlanes[i].transform.position += new Vector3(i * tabObjectOffset, 0f, 0f);
			if (i == selectedTab) {
				tabPlanes[i].transform.position += 2 * Vector3.back;
			}
		}
	}
}

