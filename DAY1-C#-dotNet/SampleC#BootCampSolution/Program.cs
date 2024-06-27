// See https://aka.ms/new-console-template for more information
//TODO Problem1: Create a console app solution in visual studio code that will print your full name, age and gender
// Define variables
string firstName = "Franz";
string middleName = "N.";
string lastName = "Lariba";
int age = 22;
string gender = "Male";

// Display personal information
Console.WriteLine($"Name: {firstName} {middleName} {lastName}");
Console.WriteLine($"Age: {age}");
Console.WriteLine($"Gender: {gender}");

//TODO Problem2: Create decision logic with if statement
Random random = new Random();

//Rolls three dies
int die1 = random.Next(1,7);
int die2 = random.Next(1,7);
int die3 = random.Next(1,7);

//Displayed the dies you rolled
Console.WriteLine($"The dies rolled: {die1}, {die2}, {die3}");

//Calculate the base score
int totalScore = die1 + die2 + die3;

//Checks if the dies gets doubled of tripled
if (die1 == die2 && die2 == die3){
    //All three dies are all the same - tripled
    totalScore += 6;
}
else if (die1 == die2 || die2 == die3 || die3 == die1) {
    //Only two dies are the same - doubled
    totalScore +=2;
}
//Display the totalScore and Result
Console.WriteLine($"Total score: {totalScore}");

if (totalScore >=15){
    Console.WriteLine("Well done! you win!");
} 
else {
    Console.WriteLine("Sorry, you lose.");
}

//TODO Problem3: Comparing 3 numbers
// Declare and initialize the three integers
int num1 = 25;
int num2 = 75;
int num3 = 10;
        
// Store the numbers in an array
int[] allNumbers = {num1, num2, num3}  ;

// Find the highest and lowest numbers 
int highestNumber = allNumbers[0];
int lowestNumber = allNumbers[0];

//Iteration through the array.
foreach (int number in allNumbers)
    {
        //Updating highest and lowest during each iteration by comparing the current number
        if (number > highestNumber) highestNumber = number;
        if (number < lowestNumber) lowestNumber = number;
    }

// Display the results
Console.WriteLine($"Highest Number: {highestNumber}");
Console.WriteLine($"Lowest Number: {lowestNumber}");

//TODO: Check if even
//Declaring and Initializing a variable
int numberToCheck = 6;

//Checks if the numberToCheck is even
if (numberToCheck % 2 == 0) {
    Console.WriteLine($"Number {numberToCheck} is even");
} 
else {
    Console.WriteLine($"Number {numberToCheck} is not even");
}

numberToCheck = 7;

//Checks if the numberToCheck is even
if (numberToCheck % 2 == 0) {
    Console.WriteLine($"Number {numberToCheck} is even");
} 
else {
    Console.WriteLine($"Number {numberToCheck} is not even");
}