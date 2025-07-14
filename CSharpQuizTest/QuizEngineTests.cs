using CSharpQuizBlazor.Services;
using CSharpQuizBlazor.Models;

namespace CSharpQuizTest
{
    public class QuizEngineTests
    {
        [Fact]
        public void StartQuiz_InitializesQuizCorrectly()
        {
            var engine = new QuizEngine();
            engine.StartQuiz("TestUser");
            Assert.True(engine.QuizStarted);
            Assert.False(engine.QuizFinished);
            Assert.Equal("TestUser", engine.UserName);
            Assert.Equal(0, engine.CurrentQuestionIndex);
            Assert.All(engine.SelectedAnswers, a => Assert.Null(a));
        }

        [Fact]
        public void SelectAnswer_SetsSelectedAnswer()
        {
            var engine = new QuizEngine();
            engine.StartQuiz("User");
            engine.SelectAnswer(2);
            Assert.Equal(2, engine.SelectedAnswers[0]);
        }

        [Fact]
        public void NextOrFinish_MovesToNextQuestionOrFinishes()
        {
            var engine = new QuizEngine();
            engine.StartQuiz("User");
            // Only 1 question in GetQuestions, so should finish
            var finished = engine.NextOrFinish();
            Assert.True(finished);
            Assert.True(engine.QuizFinished);
        }

        [Fact]
        public void FinishQuiz_CalculatesScoreCorrectly()
        {
            var engine = new QuizEngine();
            engine.StartQuiz("User");
            // Correct answer for first question is 0
            engine.SelectAnswer(0);
            engine.FinishQuiz();
            Assert.Equal(1, engine.Score);
        }

        [Fact]
        public void Restart_ResetsQuizState()
        {
            var engine = new QuizEngine();
            engine.StartQuiz("User");
            engine.SelectAnswer(1);
            engine.FinishQuiz();
            engine.Restart();
            Assert.False(engine.QuizStarted);
            Assert.False(engine.QuizFinished);
            Assert.Equal(string.Empty, engine.UserName);
            Assert.Equal(0, engine.CurrentQuestionIndex);
            Assert.All(engine.SelectedAnswers, a => Assert.Null(a));
        }
    }
}
