// See https://aka.ms/new-console-template for more information
using Configuration;
using DbModels;
using Models;
using Seido.Utilities.SeedGenerator;

const string _seedSource = "./friends-seeds.json";

Console.WriteLine("AppTest");
var _seeder = new csSeedGenerator();


System.Console.WriteLine("\nAttractions:");
var _attractions = _seeder.UniqueItemsToList<csAttractionDbM>(5);
foreach (var item in _attractions){
    item.CommentsDbM = _seeder.ItemsToList<csCommentDbM>(5);
    System.Console.WriteLine(item);
}

