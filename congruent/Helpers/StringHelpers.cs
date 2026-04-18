using System.Linq;
namespace congruent.Helpers;

public static class StringHelpers{
   public static string CreateTabText(this string targetText){
      targetText = new string(targetText.ToCharArray().Reverse().ToArray());
      var startIdx = targetText.IndexOf(".");
      targetText = targetText.Substring(startIdx +1, targetText.Length - startIdx - 1);
      startIdx = targetText.IndexOf("/");
      System.Console.WriteLine($"startIdx : {startIdx}");
      targetText = targetText.Substring(0, startIdx);
      targetText = new string(targetText.ToCharArray().Reverse().ToArray());
      return targetText;
   }
}
