using System.Linq;
namespace congruent.Helpers;

public static class StringHelpers{
   public static string CreateTabText(this string targetText){
      targetText = new string(targetText.ToCharArray().Reverse().ToArray());
      var startIdx = targetText.IndexOf(".");
      targetText = targetText.Substring(startIdx, targetText.Length-1);
      return targetText;
   }
}
