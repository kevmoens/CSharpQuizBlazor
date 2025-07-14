using System.Collections.Generic;
using System.Linq;
using CSharpQuizBlazor.Models;

namespace CSharpQuizBlazor.Services
{
    public class QuizEngine
    {
        public string UserName { get; set; }
        public List<Question> Questions { get; private set; }
        public List<int?> SelectedAnswers { get; private set; }
        public int CurrentQuestionIndex { get; private set; }
        public int Score { get; private set; }
        public bool QuizStarted { get; private set; }
        public bool QuizFinished { get; private set; }
        public int MaxQuestionReached { get; private set; }

        public QuizEngine()
        {
            Questions = GetQuestions();
            ShuffleQuestions();
            SelectedAnswers = new List<int?>(new int?[Questions.Count]);
            CurrentQuestionIndex = 0;
            Score = 0;
            QuizStarted = false;
            QuizFinished = false;
            MaxQuestionReached = 0;
        }

        public void StartQuiz(string userName)
        {
            UserName = userName;
            QuizStarted = true;
            QuizFinished = false;
            CurrentQuestionIndex = 0;
            Questions = GetQuestions();
            ShuffleQuestions();
            SelectedAnswers = new List<int?>(new int?[Questions.Count]);
            Score = 0;
            MaxQuestionReached = 0;
        }

        public void SelectAnswer(int choiceIndex)
        {
            if (CurrentQuestionIndex == MaxQuestionReached)
            {
                SelectedAnswers[CurrentQuestionIndex] = choiceIndex;
            }
        }

        public bool NextOrFinish()
        {
            if (CurrentQuestionIndex < Questions.Count - 1)
            {
                CurrentQuestionIndex++;
                if (CurrentQuestionIndex > MaxQuestionReached)
                {
                    MaxQuestionReached = CurrentQuestionIndex;
                }
                return false;
            }
            else
            {
                FinishQuiz();
                return true;
            }
        }

        public void PreviousQuestion()
        {
            if (CurrentQuestionIndex > 0)
            {
                CurrentQuestionIndex--;
            }
        }

        public void FinishQuiz()
        {
            Score = 0;
            for (int i = 0; i < Questions.Count; i++)
            {
                if (SelectedAnswers[i] == Questions[i].CorrectChoiceIndex)
                {
                    Score++;
                }
            }
            QuizFinished = true;
        }

        public void Restart()
        {
            QuizStarted = false;
            QuizFinished = false;
            UserName = string.Empty;
            CurrentQuestionIndex = 0;
            Questions = GetQuestions();
            ShuffleQuestions();
            SelectedAnswers = new List<int?>(new int?[Questions.Count]);
            Score = 0;
            MaxQuestionReached = 0;
        }

        private void ShuffleQuestions()
        {
            var rng = new System.Random();
            Questions = Questions.OrderBy(_ => rng.Next()).ToList();
        }

        private List<Question> GetQuestions()
        {
            return new List<Question>
            {
                new Question { Text = "What is the correct way to do structured logging?", Choices = new List<string>{"logger.Trace(\"The current value is {currentValue}\", currentValue);","logger.Trace(\"The current value is \" + currentValue.ToString());","logger.Trace($\"The current value is {currentValue}\");","None of the above"}, CorrectChoiceIndex = 0 },
            };
        }
    }
}
