using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TripasDeGato;

namespace SpaceInvaders
{
    public class MultiOptionQuestion
    {
        [TextArea] public string sentence;
        [Range(0, 3)] public int correctId;
        public string optionA;
        public string optionB;
        public string optionC;
        public string optionD;

        public MultiOptionQuestion(MultipleOptionQuestion question)
        {
            sentence = question.question;
            ExtensionMethods.Shuffle(question.answers);
            optionA = question.answers[0].text;
            optionB = question.answers[1].text;
            optionC = question.answers[2].text;
            optionD = question.answers[3].text;
            for(int i = 0; i < 4; i++)
            {
                if (question.answers[i].isCorrect)
                {
                    correctId = i;
                }
            }
        }
    }
}