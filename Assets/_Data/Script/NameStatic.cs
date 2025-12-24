using System.Collections.Generic;

public class NameStatic
{
    //List name
    public static List<string> FamilyTree = new List<string> { "Pine tree", "Willow tree", "Oak tree", "Deciduous tree", "Birch tree" };
    public static List<string> FamilyAnimal = new List<string> { "Rabbit" };
    public static List<string> FamilyConstructed = new List<string>() { WoodenFloor, WoodenWall };


    //single name
    public const string WoodenFloor = "Wooden floor";
    public const string WoodenWall = "Wooden wall";

}