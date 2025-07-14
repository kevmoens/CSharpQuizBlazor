namespace CSharpQuizBlazor.Models
{
    public class QuizResultDto
    {
        public string UserName { get; set; }
        public int Score { get; set; }
        public List<string> WrongQuestions { get; set; }
    }
}
