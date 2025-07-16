using CSharpQuizBlazor.Services;
using CSharpQuizBlazor.Models;

namespace CSharpQuizTest
{
    public class QuizEngineTests
    {
        [Fact]
        public async Task StartQuiz_InitializesQuizCorrectly()
        {
            var engine = new QuizEngine();
            await engine.StartQuiz("TestUser", "SQL");
            Assert.True(engine.QuizStarted);
            Assert.False(engine.QuizFinished);
            Assert.Equal("TestUser", engine.UserName);
            Assert.Equal(0, engine.CurrentQuestionIndex);
            Assert.All(engine.SelectedAnswers, a => Assert.Null(a));
        }

        [Fact]
        public async Task SelectAnswer_SetsSelectedAnswer()
        {
            var engine = new QuizEngine();
            await engine.StartQuiz("User", "SQL");
            engine.SelectAnswer(2);
            Assert.Equal(2, engine.SelectedAnswers[0]);
        }

        [Fact]
        public async Task NextOrFinish_MovesToNextQuestionOrFinishes()
        {
            var engine = new QuizEngine();
            await engine.StartQuiz("User", "SQL");
            // Only 1 question in GetQuestions, so should finish
            bool finished = false;
            while (!finished)
            {
                finished = engine.NextOrFinish();
			}
            Assert.True(finished);
            Assert.True(engine.QuizFinished);
        }

        [Fact]
        public async Task FinishQuiz_CalculatesScoreCorrectly()
        {
            var engine = new QuizEngine();
            await engine.StartQuiz("User", "SQL");
            // Correct answer for first question is 0
            engine.SelectAnswer(engine.Questions.First().CorrectChoiceIndex);
            engine.FinishQuiz();
            Assert.Equal(1, engine.Score);
        }

        [Fact]
        public async Task Restart_ResetsQuizState()
        {
            var engine = new QuizEngine();
            await engine.StartQuiz("User", "SQL");
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
