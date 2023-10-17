using UnityEditor;
using UnityEngine;

namespace _Scripts.Menu {
    [CustomEditor(typeof(DeckManager))]
    public class EditorDeckManager : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            DeckManager deckManager = (DeckManager)target;

            if (GUILayout.Button("UpdateCardList in \"AvailableDeckUI\"")) {
                deckManager._availableDeckUI.SetAllCardsCount(deckManager.library.cards);
                Debug.Log("UpdateCardList in \"AvailableDeckUI\"");
            }
        }
    }
}