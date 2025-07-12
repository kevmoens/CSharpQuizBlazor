namespace CSharpQuizBlazor.Models
{
    public class Question
    {
        public string Text { get; set; }
        public List<string> Choices { get; set; }
        public int CorrectChoiceIndex { get; set; }
    }
}
