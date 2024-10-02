// See https://aka.ms/new-console-template for more information
using Configuration;
using DbModels;
using Models;
using Seido.Utilities.SeedGenerator;

const string _seedSource = "./friends-seeds.json";

Console.WriteLine("AppTest");
var _seeder = new csSeedGenerator();


System.Console.WriteLine("\nUsers:");
var _attractions = _seeder.UniqueItemsToList<csAttractionDbM>(5);
foreach (var item in _attractions){
    item.CommentsDbM  = _seeder.UniqueItemsToList<csCommentDbM>(_seeder.Next(0,21));
    System.Console.WriteLine(item);
}

//todo:
//öndra commentsdbm att inte tillåta null userid
