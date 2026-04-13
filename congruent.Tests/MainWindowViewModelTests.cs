using System.Collections.ObjectModel;
using System.Collections.Generic;
using congruent.Models;
using congruent.ViewModels;

namespace congruent.Tests;

public class MainWindowViewModelTests
{
    [Fact]
    async public Task FindBookmarkTest(){
       MainWindowViewModel vm = new();
       Console.WriteLine("I did it!");
    }
}
