﻿using System;
using System.Diagnostics;
using MatchScore.Points;
using MatchScore.Rules;

namespace MatchScore.Scores
{
    class NewGame : Score
    {
        bool isTiebreak;

        internal NewGame(IScore previous, bool youWon, Stopwatch stopwatch)
            : base(previous, youWon, stopwatch)
        {
            if (youWon) YouGames++;
            else OppGames++;

            YouServe = !previous.YouServe;

            if ( new MatchRules().IsSetOver(OppGames, YouGames) ) 
            {
                if (youWon) YouSets++;
                else OppSets++;
                YouGames = 0;
                OppGames = 0;
            }
            else 
            {
                if (new MatchRules().IsTiebreak(OppGames, YouGames)) 
                {
                    isTiebreak = true;
                }
            }
        }

        public override Point OppPoint => Point.Love();

        public override Point YouPoint => Point.Love();

        public override IScore SetOppPoint()
        {
            if (isTiebreak) return new TiebreakScore(1, 0, this, false, stopwatch);

            if (YouServe) return new LoveFifteen(this, false, stopwatch);
            return new FifteenLove(this, false, stopwatch);
        }

        public override IScore SetYouPoint()
        {
            if (isTiebreak) return new TiebreakScore(0, 1, this, true, stopwatch);

            if (YouServe) return new FifteenLove(this, true, stopwatch);
            return new LoveFifteen(this, true, stopwatch);
        }
    }
}
