using System.Linq;
namespace congruent.Helpers;

public static class StringHelpers{
   public static string CreateTabText(this string targetText){
      targetText = new string(targetText.ToCharArray().Reverse().ToArray());
      var startIdx = targetText.IndexOf(".");
      string subPath = string.Empty;
      //check for an addt'l path like /comicreader
      // if it has it we will get it & use it in the tab name
      if (targetText.Substring(0,startIdx).Contains("/")){
         var slashIdx = targetText.Substring(0,startIdx).IndexOf("/");
         if (slashIdx > 1){
            subPath = new string(targetText.Substring(0,slashIdx).ToCharArray().Reverse().ToArray());
         System.Console.WriteLine($"subPath: {subPath}");
         }
      }
      targetText = targetText.Substring(startIdx +1, targetText.Length - startIdx - 1);
      startIdx = targetText.IndexOf("/");
      System.Console.WriteLine($"startIdx : {startIdx}");
      targetText = targetText.Substring(0, startIdx);
      targetText = new string(targetText.ToCharArray().Reverse().ToArray());
      if (!string.IsNullOrEmpty(subPath)){
         targetText +=  $"-{subPath}";
      }
      return targetText;
   }
}
