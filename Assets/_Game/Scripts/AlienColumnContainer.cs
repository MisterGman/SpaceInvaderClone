using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class AlienColumnContainer : MonoBehaviour
    {
        public int EnemiesInColumn { get;  set; }
        
        public int Index { get; set; }

        public Action<AlienColumnContainer> LastShipDestroyedEvent { get; set; }


        /// <summary>
        /// Remove from the list until the list becomes empty
        /// Then calls the event of empty column
        /// </summary>
        public void EliminateLowestShipHandler()
        {
            EnemiesInColumn--;
            
            if (EnemiesInColumn == 0) 
                LastShipDestroyedEvent(this);
        }
    }
}
