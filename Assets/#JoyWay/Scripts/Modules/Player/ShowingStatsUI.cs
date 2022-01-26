using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShowingStat{
	List<ShowingStatsUI.NameDiscr> GetStatsInfo();
}


public class ShowingStatsUI : MonoBehaviour
{

	public Owner						targetOwner;
	public Transform					elementHolder;
	public Transform					totalHolder;
	public ShowingStatsUIElement		UI_prefab;
	public SettingsUIController			settingsUIController;
	private List<ShowingStatsUIElement> elements = new List<ShowingStatsUIElement>();
	private int currentCountEl;

	[SerializeField]
	public struct NameDiscr{
		public string	group;
		public string	name;
		public string	descr;
		public bool		show;

		public void Fill(string g, string n, string d, bool s) {
			group = g;
			name = n;
			descr = d;
			show = s;
		}

	}

	private void Start() {
		PauseManager.instance.onPause += ShowStat;
	}

	public void ShowStat(bool value) {
		
		if (!value) return;

		currentCountEl = 0;
		List<NameDiscr> haveGroup = new List<NameDiscr>();

		foreach (IShowingStat one in targetOwner.showingStats) {
			
			List<NameDiscr> fromOne = one.GetStatsInfo();
			foreach (NameDiscr each in fromOne) {
			
				// Сначала отображаем без группы
				if (each.group == "") {

					if (each.show) {
						ShowingStatsUIElement newEl = GetFreeElement();
						newEl.nameText.text = each.name;
						newEl.descrText.text = each.descr;
					}
				}

				else haveGroup.Add(each);
			}
		}

		List<string> groupCreated = new List<string>();
		foreach (NameDiscr one in haveGroup) {

			// Если такой группы еще нет
			if (groupCreated.IndexOf(one.group) == -1) {

				// Создаём группу
				ShowingStatsUIElement newEl = GetFreeElement();
				newEl.nameText.text = one.group;
				newEl.descrText.text = "";

				groupCreated.Add(one.group);

				// Пробегаемся по списку еще раз создаем все элементы данной группы
				foreach (NameDiscr each in haveGroup) {

					if (each.group == one.group) {

						ShowingStatsUIElement newEl2 = GetFreeElement();
						newEl2.nameText.text	= "  " + each.name; 							// С отступом чтобы показать принадлежность к группе
						newEl2.descrText.text	= each.descr;
					}
				}
			}
		}
	}

	ShowingStatsUIElement GetFreeElement() {
		
		ShowingStatsUIElement localEl;
		
		if (currentCountEl >= elements.Count) localEl = Instantiate(UI_prefab, elementHolder);
		else localEl = elements[currentCountEl];

		elements.Add(localEl);

		currentCountEl ++;
		return localEl;

	}


	public void Toggle() {		
		totalHolder.gameObject.SetActive(!settingsUIController.gameObject.activeSelf);
	}

}



