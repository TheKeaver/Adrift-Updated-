using System;
using System.Collections.Generic;
using Audrey;
using Events;
using GameJam.Events.GameLogic;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class PlayerScoreDirector : BaseDirector
    {
        private readonly Root _root;

        private class ScoreContents
        {
            public int Score;
            public int Index;
            public string OriginalContent;
        }

        private Dictionary<Player, ScoreContents> _playersDict;

        public PlayerScoreDirector(Player[] players,
            Root root,
            Engine engine,
            ContentManager content,
            ProcessManager processManager) : base(engine, content, processManager)
        {
            _root = root;

            _playersDict = new Dictionary<Player, ScoreContents>();
            for(int i = 0; i < players.Length; i++)
            {
                _playersDict.Add(players[i], new ScoreContents
                {
                    Score = 0,
                    Index = i,
                    OriginalContent = ((Label)_root.FindWidgetByID(string.Format("player_{0}_score_label", i))).Content
                });
                _root.FindWidgetByID(string.Format("player_{0}_score_label", i)).Hidden = false;
            }

            UpdateScoresUI();
        }

        public override bool Handle(IEvent evt)
        {
            IncreasePlayerScoreEvent increasePlayerScoreEvent = evt as IncreasePlayerScoreEvent;
            if(increasePlayerScoreEvent != null)
            {
                ScoreContents sc = _playersDict[increasePlayerScoreEvent.Player];
                sc.Score += increasePlayerScoreEvent.ScoreIncrement;
                UpdateScoresUI();
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<IncreasePlayerScoreEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        private void UpdateScoresUI()
        {
            foreach(Player player in _playersDict.Keys)
            {
                ScoreContents sc = _playersDict[player];
                ((Label)_root.FindWidgetByID(string.Format("player_{0}_score_label", sc.Index))).Content = string.Format(sc.OriginalContent, sc.Score);
            }
        }

        public int[] GetScores()
        {
            int[] scores = new int[_playersDict.Keys.Count];
            foreach (Player player in _playersDict.Keys)
            {
                scores[_playersDict[player].Index] = _playersDict[player].Score;
            }
            return scores;
        }
    }
}
