﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MatchScore.Scores;

namespace MatchScore
{
    sealed class Match
    {
        static readonly Lazy<Match> lazy = new Lazy<Match>(() => new Match());
        public static Match Instance { get { return lazy.Value; } }
        List<IScore> scores = new List<IScore>();
        readonly Stopwatch stopwatch = new Stopwatch();

        Match()
        {
        }

        public void Start(bool youServe)
        {
            scores.Add(new InitialScore(youServe, stopwatch));
            stopwatch.Start();
        }

        public bool OnGoing()
        {
            return scores.Any();
        }

        internal IScore Current()
        {
            return scores[scores.Count - 1];
        }

        internal void SetOppPoint()
        {
            var current = Current();
            if (current.IsEndOfMatch()) return;

            var newScore = current.SetOppPoint();
            scores.Add(newScore);
        }

        internal void SetYouPoint()
        {
            var current = Current();
            if (current.IsEndOfMatch()) return;

            var newScore = current.SetYouPoint();
            scores.Add(newScore);
        }

        internal List<IScore> Scores()
        {
            return scores;
        }

        internal void UndoLastPoint()
        {
            if(scores.Count > 1) scores.RemoveAt(scores.Count-1);
        }

        internal TimeSpan ElapsedTime()
        {
            return stopwatch.Elapsed;
        }

        internal void End()
        {
            stopwatch.Reset();
            scores.RemoveAll((s) => true);
        }
    }
}
