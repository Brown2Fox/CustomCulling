using TMPro;
using UnityEngine;

public class InGameLogFormer : Singleton<InGameLogFormer> {
	public int linesCount;
	public float lineLifeTime;
	public TextMeshPro firstLine;
	public bool toUp;
	private TextMeshPro[] lines;

	private int currentLine, shownCount;
	private LogLine[] logs;

	protected override void Awake() {
		base.Awake();

		lines = new TextMeshPro[linesCount];
		lines[0] = firstLine;
		for (int i = linesCount - 1; i > 0; --i) {
			lines[i] = Instantiate(firstLine.gameObject, transform).GetComponent<TextMeshPro>();
			if (toUp)
				lines[i].transform.localPosition += i * lines[i].rectTransform.sizeDelta.y * Vector3.up;
			else
				lines[i].transform.localPosition -= i * lines[i].rectTransform.sizeDelta.y * Vector3.up;
			lines[i].text = "";
		}

		logs = new LogLine[linesCount];
		Rebuild();
	}

	public static void Log(string _text) {
		Log(_text, Color.white);
	}
	public static void Log(string _text, Color _color) {
		if (instance != null)
			instance.AddLine(_text, _color);
	}

	public void AddLine(string _text, Color _color) {
		currentLine = (currentLine + 1) % linesCount;
		shownCount = Mathf.Clamp(shownCount + 1, 0, linesCount);
		logs[currentLine] = new LogLine(_text, _color, Time.time);
		Rebuild();
	}

	private void Rebuild() {
		int i;
		for (i = 0; i < logs.Length && i < shownCount; ++i) {
			lines[i].text = logs[(currentLine - i + linesCount) % linesCount].text;
			lines[i].color = logs[(currentLine - i + linesCount) % linesCount].color;
		}
		for (; i < logs.Length; ++i)
			lines[i].text = "";
	}

	private void Update() {
		if (CheckLast())
			Rebuild();
	}

	private bool CheckLast() {
		if (shownCount > 0)
			if (Time.time > logs[(currentLine - shownCount + 1 + linesCount) % linesCount].time + lineLifeTime) {
				--shownCount;
				CheckLast();
				return true;
			}
		return false;
	}

	private struct LogLine {
		public string text;
		public Color color;
		public float time;
/*
 *
 *            log
 *
 *
 *			   ______________________________________
 *           /./...\.\ _________8___________0_________\
 *			|.|..0..|.|______________0_________________|
 *           \.\..././______________________0_________/
 *		
 * 
 */
		public LogLine(string _text) {
			text = _text;
			color = Color.white;
			time = Time.time;
		}

		public LogLine(string _text, Color _color, float _time) {
			text = _text;
			color = _color;
			time = _time;
		}
	}
}
