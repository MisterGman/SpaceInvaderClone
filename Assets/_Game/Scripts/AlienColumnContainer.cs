using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class AlienColumnContainer : MonoBehaviour
    {
        public int EnemiesInColumn { get; set; }

        public Action<AlienColumnContainer> lastShipDestroyedEvent;

        public int Index { get; set; }

        /// <summary>
        /// Remove from the list until the list becomes empty
        /// Then calls the event of empty column
        /// </summary>
        public void EliminateLowestShipHandler()
        {
            EnemiesInColumn--;
            
            if (EnemiesInColumn == 0) 
                lastShipDestroyedEvent(this);
        }
    }
}
