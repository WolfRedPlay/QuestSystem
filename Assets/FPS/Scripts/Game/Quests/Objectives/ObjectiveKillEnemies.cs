﻿using System;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    //----NEW-----
    [Serializable]
    [CreateAssetMenu(fileName = "ObjectiveKillEnemies", menuName = "Objectives/ObjectiveKillEnemies")]
    public class ObjectiveKillEnemies : Objective
    {
        //[Tooltip("Chose whether you need to kill every enemies or only a minimum amount")]
        //public bool MustKillAllEnemies = true;

        [Tooltip("This is the amount of enemy kills required")]
        public int KillsToCompleteObjective = 5;

        //----NEW-----
        [Tooltip("Type of enemy player needs to kill")]
        public EnemyType RequiredType;

        [Tooltip("Start sending notification about remaining enemies when this amount of enemies is left")]
        public int NotificationEnemiesRemainingThreshold = 3;


        int m_KillTotal = 0;

        public override void OnAcceptance()
        {
            base.OnAcceptance();
            m_KillTotal = 0;

            EventManager.AddListener<EnemyKillEvent>(OnEnemyKilled);

            // set a title and description specific for this type of objective, if it hasn't one
            //if (string.IsNullOrEmpty(Title))
            //    Title = "Eliminate " + (MustKillAllEnemies ? "all the" : KillsToCompleteObjective.ToString()) +
            //            " enemies";

            if (string.IsNullOrEmpty(Description))
                Description = GetUpdatedCounterAmount();
        }

        void OnEnemyKilled(EnemyKillEvent evt)
        {
            if (IsCompleted)
                return;

            //----NEW-----
            if (evt.Type == RequiredType)
            {
                m_KillTotal++;

                //if (MustKillAllEnemies)
                //    KillsToCompleteObjective = evt.RemainingEnemyCount + m_KillTotal;

                int targetRemaining = KillsToCompleteObjective - m_KillTotal;

                // update the objective text according to how many enemies remain to kill
                if (targetRemaining == 0)
                {
                    CompleteObjective(string.Empty, GetUpdatedCounterAmount(), "Objective complete ");
                }
                else if (targetRemaining == 1)
                {
                    string notificationText = NotificationEnemiesRemainingThreshold >= targetRemaining
                        ? "One enemy left"
                        : string.Empty;
                    UpdateObjective(Description, GetUpdatedCounterAmount(), notificationText);
                }
                else
                {
                    // create a notification text if needed, if it stays empty, the notification will not be created
                    string notificationText = NotificationEnemiesRemainingThreshold >= targetRemaining
                        ? targetRemaining + " enemies to kill left"
                        : string.Empty;

                    UpdateObjective(Description, GetUpdatedCounterAmount(), notificationText);
                }
            }

            
        }

        string GetUpdatedCounterAmount()
        {
            return m_KillTotal + " / " + KillsToCompleteObjective;
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<EnemyKillEvent>(OnEnemyKilled);
        }
    }
}