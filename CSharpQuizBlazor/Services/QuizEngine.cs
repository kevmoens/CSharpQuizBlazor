using CSharpQuizBlazor.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CSharpQuizBlazor.Services
{
	public class QuizEngine
	{
		public string UserName { get; set; } = string.Empty;
		public List<Question> Questions { get; private set; } = [];
		public List<int?> SelectedAnswers { get; private set; }
		public int CurrentQuestionIndex { get; private set; }
		public int Score { get; private set; }
		public bool QuizStarted { get; private set; }
		public bool QuizFinished { get; private set; }
		public int MaxQuestionReached { get; private set; }

		public QuizEngine()
		{
			ShuffleQuestions();
			SelectedAnswers = new List<int?>(new int?[Questions.Count]);
			CurrentQuestionIndex = 0;
			Score = 0;
			QuizStarted = false;
			QuizFinished = false;
			MaxQuestionReached = 0;
		}

		public async Task<bool> StartQuiz(string userName,string test)
		{
			UserName = userName;
			QuizStarted = true;
			QuizFinished = false;
			CurrentQuestionIndex = 0;

			try
			{
				using HttpClient client = new HttpClient();
				string url = "https://csharpquizfunc20250713230151.azurewebsites.net/api/GetTestQuestions?name=" + WebUtility.UrlEncode(test);
				var response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();
				string json = await response.Content.ReadAsStringAsync();
				Quiz? quiz = System.Text.Json.JsonSerializer.Deserialize<Quiz>(json, new System.Text.Json.JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				Questions = quiz!.Questions;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			ShuffleQuestions();
			SelectedAnswers = new List<int?>(new int?[Questions.Count]);
			Score = 0;
			MaxQuestionReached = 0;
			return true;
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
		
	}
}
