using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(Minimap))]
public class MinimapEditor : Editor
{
	public int id = 0;

	public override void OnInspectorGUI() {
		Minimap minimap = (Minimap)target;

		base.OnInspectorGUI();

		if (GUILayout.Button("OpenFullMap")) {
			minimap.OpenFullMap(true);
		}

		if (GUILayout.Button("HideFullMap")) {
			minimap.OpenFullMap(false);
		}


		if (GUILayout.Button("Proccess")) {
			minimap.Proccess();

		}


		if (GUILayout.Button("Clear")) {
			minimap.ClearMap();
			id = 0;
		}

	}
}
#endif


public class Minimap : Singleton<Minimap>
{
	[HideInInspector]
	public MapGenerator mapGenerator;

	[Header("Links")]
	public Player			player;
	public Transform		holder;
	public SpriteRenderer	spriteRen;

	[Header("Prefabs")]
	public Image			chankPref;
	public GameObject		playerPref;

	[Header("Canvas")]
	public float minScale		= 1;
	public float maxScale		= 5;
	public float speedScaling	= 1;
	public float speedMove		= 1;

	[Header("Image")]
	public int scaleFactor		= 1;                  // Для отрисовки карты, чем он больше, тем четче картинка

	Texture2D texture;
	Color[] clean_colours_array;
	Vector3 holderTargetMove;

	[SerializeField]
	public List<MinimapElement> minimapElements;
	SpriteRenderer				newPlayerImg;
	RectTransform				rectTransform;

	float halfW;
	float halfH;

	bool inTutor;

	[System.Serializable]
	public struct MinimapElement
	{

		public Image spawnedElement;
		public RoomController room;

		public Vector2 pixelCenter;
		public bool open;

		public Vector2 sizePixelPic;
		public Vector2 posPixelPic;
		public Vector2 lowest;
		public Texture2D source;
		public float angleChank;

		// Проверяем находятся ли координаты в пределах комнаты
		public bool CheckContain(Vector2 pos) {

			Vector2 chankPos = new Vector2(room.transform.position.x, room.transform.position.z);
			Vector2 chankSize = room.roomData.size * (16 / 2);

			if ((pos.x < (chankPos.x + chankSize.x) && pos.x > (chankPos.x - chankSize.x))
					&& (pos.y < (chankPos.y + chankSize.y) && pos.y > (chankPos.y - chankSize.y))) {
				return true;
			}

			return false;

		}


		public void OpenChank(bool val) {
			open = val;
		}
	}


	public void Reset() {
		holder.gameObject.SetActive(false);
		ClearMap();
		minimapElements = new List<MinimapElement>();
		mapGenerator = null;
	}


	private void Start() {
		rectTransform = GetComponent<RectTransform>();
		texture = spriteRen.sprite.texture;

		clean_colours_array = new Color[(int)spriteRen.sprite.rect.width * (int)spriteRen.sprite.rect.height];
		for (int x = 0; x < clean_colours_array.Length; x++)
			clean_colours_array[x] = new Color(0, 0, 0, 0);

		halfW = spriteRen.sprite.texture.width / 2;
		halfH = spriteRen.sprite.texture.height / 2;

		ClearMap();

		LevelManager.SoonLoading += Reset;
	}

	// Инициализация карты
	public void Proccess() {
		ClearMap();
		Apply();

		minimapElements = new List<MinimapElement>();

		// Находим самые отрицательные координаты
		Vector2 lowest = new Vector2(mapGenerator.generated[0].onScene.transform.localPosition.x - ((mapGenerator.generated[0].onScene.roomData.size.x * 16) / 2),
										mapGenerator.generated[0].onScene.transform.localPosition.z - ((mapGenerator.generated[0].onScene.roomData.size.y * 16) / 2));

		foreach (GeneratedRoom room in mapGenerator.generated) {

			float newX = room.onScene.transform.localPosition.x - ((room.onScene.roomData.size.x * 16) / 2);
			float newY = room.onScene.transform.localPosition.y - ((room.onScene.roomData.size.y * 16) / 2);

			lowest.x = (newX < lowest.x) ? newX : lowest.x;
			lowest.y = (newY < lowest.y) ? newY : lowest.y;
		}

		if (lowest.x > 0 && lowest.y > 0) lowest = Vector2.zero;


		foreach (GeneratedRoom room in mapGenerator.generated) {

			MinimapElement me = new MinimapElement();

			// Задаем его размер, положение и поворот
			me.sizePixelPic = room.onScene.roomData.size * 16;
			me.angleChank = new Vector3(0, 0, (360 - room.onScene.transform.eulerAngles.y) + 90).z;
			me.posPixelPic = new Vector2(room.onScene.transform.localPosition.x, room.onScene.transform.localPosition.z);
			me.source = (room.onScene.spriteForMinimap) ? room.onScene.spriteForMinimap.texture : null;
			me.lowest = lowest;
			me.room = room.onScene;

			Vector2 leftBot = new Vector2((me.posPixelPic.x + Mathf.Abs(lowest.x) - (me.sizePixelPic.x / 2)),
										  (me.posPixelPic.y + Mathf.Abs(lowest.y) - (me.sizePixelPic.y / 2)));
			leftBot *= scaleFactor;

			me.pixelCenter = new Vector2(leftBot.x + ((me.sizePixelPic.x * scaleFactor) / 2), leftBot.y + ((me.sizePixelPic.y * scaleFactor) / 2));

			me.OpenChank(false);

			// Добавляем в список сгенеренных кусков карты
			minimapElements.Add(me);
		}

		if (!newPlayerImg) {
			newPlayerImg = Instantiate(playerPref, spriteRen.transform).GetComponent<SpriteRenderer>();
			newPlayerImg.color = Color.cyan;
			newPlayerImg.transform.localScale = Vector3.one * 12;
		}
	}


	private void Update() {
		if (!mapGenerator || !newPlayerImg || inTutor) return;

		PlayerPosition();
		CheckTruePos();
		RecalcHolderPosNew();
		RecalcHolderScale();
		CheckHandPos();
	}

	// Отслеживаем положение руки чтобы скрывать/открывать карту
	private void CheckHandPos() {
		if (Vector3.Angle(player.leftHand.transform.right, Vector3.up) < 60) {
			holder.gameObject.SetActive(true);
		}

		else holder.gameObject.SetActive(false);
	}



	// Отслеживаем какие чанки должны открыться 
	private void CheckTruePos() {

		for (int i = 0; i < minimapElements.Count; i++) {

			MinimapElement me = minimapElements[i];

			Vector2 playerPos = new Vector2(player.movable.transform.position.x, player.movable.transform.position.z);
			//Vector2 chankPos	= new Vector2(minimapElements[i].room.transform.position.x, minimapElements[i].room.transform.position.z);
			//Vector2 chankSize	= minimapElements[i].room.roomData.size * (16 / 2);


			//if ((playerPos.x < (chankPos.x + chankSize.x) && playerPos.x > (chankPos.x - chankSize.x))
			//	&& (playerPos.y < (chankPos.y + chankSize.y) && playerPos.y > (chankPos.y - chankSize.y))) {
			if (me.CheckContain(playerPos)) {

				if (!me.open) {
					me.open = true;
					minimapElements[i] = me;
					DrawImageNew(me);
				}

				CheckNearChanks(me.room);

				Apply();
			}

		}

	}

	// Проверяем соседние чанки
	private void CheckNearChanks(RoomController chank) {
		List<Vector2> nearestCoords = new List<Vector2>();
		foreach (WayConnection one in chank.roomData.ways) {
			Vector2 from = (one.offsetFrom * 8);
			from.x += chank.transform.position.x - ((chank.roomData.size.x - 1) * 8);       // Отсчет идет от левого нижнего квадрата комнаты (если её размер больше 1),
			from.y += chank.transform.position.z - ((chank.roomData.size.y - 1) * 8);       //  если она 1 то от этой самой комнаты

			Vector2 to = (one.offsetTo * 16);               // Нам нужен двойной чтобы было не на границе чанков а в глубь следующего
			nearestCoords.Add(from + to);
		}

		for (int i = 0; i < minimapElements.Count; i++) {
			if (!minimapElements[i].open) {
				MinimapElement me = minimapElements[i];

				foreach (Vector2 one in nearestCoords) {
					if (me.CheckContain(one)) {
						me.open = true;
						minimapElements[i] = me;
						DrawImageNew(me);
						break;
					}
				}
			}
		}

	}

	// Открыть полную карту
	public void OpenFullMap(bool val) {
		for (int i = 0; i < minimapElements.Count; i++) {
			if (minimapElements[i].open != val) {

				MinimapElement me = minimapElements[i];
				me.OpenChank(val);
				minimapElements[i] = me;
				DrawImageNew(me);

			}
		}
		Apply();
	}


	// Рассчитываем новое положение миникарты (она центрируется учитывая открытые чанки)
	private void RecalcHolderPosNew() {


		Vector3 spreadCenter = Vector3.zero;
		int openCount = 0;

		for (int i = 0; i < minimapElements.Count; i++) {
			if (minimapElements[i].open) {
				Vector3 localPos = new Vector3(minimapElements[i].pixelCenter.x - halfW, minimapElements[i].pixelCenter.y - halfH, 0);
				spreadCenter += spriteRen.transform.TransformPoint(localPos);
				openCount++;
			}
		}
		holderTargetMove = holder.parent.position - (spreadCenter / openCount);

		holder.position += holderTargetMove * Time.unscaledDeltaTime;
	}

	// Рассчитываем новый скейл миникарты
	private void RecalcHolderScale() {

		float maxDeltaX = 0;
		float maxDeltaY = 0;

		for (int i = 0; i < minimapElements.Count; i++) {
			for (int j = 0; j < minimapElements.Count; j++) {

				if (minimapElements[i].open && minimapElements[j].open) {

					Vector3 localPosI = new Vector3(minimapElements[i].pixelCenter.x - halfW, minimapElements[i].pixelCenter.y - halfH, 0);
					Vector3 localPosJ = new Vector3(minimapElements[j].pixelCenter.x - halfW, minimapElements[j].pixelCenter.y - halfH, 0);

					float localX = Mathf.Abs(localPosI.x - localPosJ.x);
					float localY = Mathf.Abs(localPosI.y - localPosJ.y);

					if (localX > maxDeltaX) maxDeltaX = localX;
					if (localY > maxDeltaY) maxDeltaY = localY;
				}
			}
		}


		float scaleY = maxDeltaY / (rectTransform.rect.height / 2);
		float scaleX = maxDeltaX / (rectTransform.rect.width / 2);
		float maxXY = Mathf.Max(scaleX, scaleY);

		Vector3 targetScale = Vector3.one * (Mathf.Clamp(maxScale - (maxXY * maxScale), minScale, maxScale));

		holder.localScale = Vector3.Lerp(holder.localScale, targetScale, speedScaling * Time.unscaledDeltaTime);

	}

	// Отслеживаем положение игрока на карте и двигаем/вращаем его курсор
	private void PlayerPosition() {
		float halfW = spriteRen.sprite.texture.width / 2;
		float halfH = spriteRen.sprite.texture.height / 2;

		Vector2 room0 = new Vector2(minimapElements[0].room.transform.position.x, minimapElements[0].room.transform.position.z);
		Vector2 room1 = new Vector2(minimapElements[1].room.transform.position.x, minimapElements[1].room.transform.position.z);

		Vector2 pix0 = new Vector2(minimapElements[0].pixelCenter.x - halfW, minimapElements[0].pixelCenter.y - halfH);
		Vector2 pix1 = new Vector2(minimapElements[1].pixelCenter.x - halfW, minimapElements[1].pixelCenter.y - halfH);

		Vector2 difRoom = room0 - room1;
		Vector3 difPix = pix0 - pix1;

		float difScale = difPix.magnitude / difRoom.magnitude;

		Vector2 playerVec = new Vector2(player.movable.transform.position.x, player.movable.transform.position.z) - room0;
		playerVec *= difScale;

		playerVec = playerVec + pix0;

		newPlayerImg.transform.localPosition = playerVec;
		newPlayerImg.transform.localPosition += new Vector3(0, 0, -0.01f);
		//newPlayerImg.transform.localEulerAngles = new Vector3(0, 0, 360 - (player.movable.transform.localEulerAngles.y - 25));
		newPlayerImg.transform.localEulerAngles = new Vector3(0, 0, 180 - (player.head.transform.eulerAngles.y - 45));

	}

	// Рисует на текстуре чанк
	private void DrawImageNew(MinimapElement me) {

		// Рассчитываем шаг с которым будем брать цвет пикселя у оригинальной картинки
		int difW = (int)(me.source.width / (me.sizePixelPic.x * scaleFactor));
		int difH = (int)(me.source.height / (me.sizePixelPic.y * scaleFactor));

		// левый нижний угол данного чанка		Его положение +	Самый отрицательный поо кориданатм ибо у текстуры нет отрицательной плоскости - половина его ширины/высоты
		Vector2 leftBot = new Vector2((me.posPixelPic.x + Mathf.Abs(me.lowest.x) - (me.sizePixelPic.x / 2)),
									  (me.posPixelPic.y + Mathf.Abs(me.lowest.y) - (me.sizePixelPic.y / 2)));

		leftBot *= scaleFactor;

        // Рисуем чанк
        for (int i = 0; i < me.sizePixelPic.x * scaleFactor; i++) {
			for (int j = 0; j < me.sizePixelPic.y * scaleFactor; j++) {
				
				texture.SetPixel((int)(leftBot.x + i), (int)(leftBot.y + j), GetPixelWithRotation(me.source, me.angleChank, i * difW, j * difH));
			}
		}
	}

	// Очистить всю карту
	public void ClearMap() {
		texture.SetPixels(clean_colours_array);
		Apply();
	}

	// Получаем цвет с учетом поворота чанка
	private Color GetPixelWithRotation(Texture2D texture, float angle, int x, int y) {

		angle = Mathf.Round(angle);
		if (angle < 0) angle = 360 + angle;
		if (angle >= 360) angle -= 360;


		Color c = new Color();

		if (angle == 0)
			c = texture.GetPixel(texture.height - y, x);


		if (angle == 90)
			c = texture.GetPixel(x, y);


		if (angle == 180)
			c = texture.GetPixel(y, texture.width - x);


		if (angle == 270)
			c = texture.GetPixel(texture.width - x, texture.height - y);


		return c;
	}

	// Применяем нарисованное к текстуре
	private void Apply() {
		texture.Apply();
	}

	// Если мы в туториале то карту не показываем
	public void InTutorial(bool value) {
		inTutor = value;
		holder.gameObject.SetActive(!value);
	}
}
