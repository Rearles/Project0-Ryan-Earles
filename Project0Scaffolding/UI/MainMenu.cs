using Models;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI
{
    public class MainMenu : IMenu
    {
        //the below code allows us to access the methods from IPetBL below in Start(); due to IMenu menu = new MainMenu(new PetBL(new PetRepo(context)));
        private IReviewBL _reviewb1;
        public MainMenu(IReviewBL bl) //called from Program.cs line 22
        {
            _reviewb1 = bl;
        }

        public void Start() //this is called from Program.cs line 23
        {
            bool repeat = true;
            do
            {
                //have another do while loop and switch case, where it asks if you are a user or an admin, and gives an option to go to the next method. 
                //case 1 is user where you call a method and either create a new user or access an already existing user, case two is admin and log in either using default user and pass or access one that already exists, case three is exit
                //

                /*
                Adduser, AddAdmin, 
                AddReview, SearchUser, SearchRestaurant, SearchReview
                - add a new user AddUser
                - ability to search user as admin SearchUser
                - display details of a restaurant for user SearchRestaurant
                - add reviews to a restaurant as a user AddReview
                - view details of restaurants as a user SearchRestaurant
                - view reviews of restaurants as a user SearchRestaurant
                - calculate reviews’ average rating for each restaurant SearchRestuarant
                - search restaurant (by name, rating, zip code, etc.)  SearchRestaurant.
                */
                Console.WriteLine("Welcome to Scream, your local guide to anything Restaurant Reviews!");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[1] Add a User");
                Console.WriteLine("[2] Add a Admin");
                Console.WriteLine("[3] Add a Restaurant");
                Console.WriteLine("[4] Add a Review");
                Console.WriteLine("[5] Search a User");
                Console.WriteLine("[6] Display a Restaurant");
                Console.WriteLine("[7] Search a Review");
                Console.WriteLine("[8] Display the contents of a Review");
                Console.WriteLine("[9] Search a Restaurant");

                switch(Console.ReadLine())
                {
                    case "0":
                        Console.WriteLine("Thanks for using Scream! Remember that whenever you need to Scream into the void about a Restaurant, think about Scream!");
                        repeat = false;
                    break;

                    case "1": //adding a new user
                    AddUser();
                    break;

                    case "2": //adding a new admin
                    AddAdmin();
                    break;

                    case "3": //adding a new restaurant
                    AddRestaurant();
                    break;

                    case "4": //adding a new review
                    AddReview();
                    break;

                    case "5": //search user work in progress
                    SearchUser();
                    break;

                    case "6": //search restaurant
                    SearchRestaurant();
                    break;

                    case "7": //search review
                    SearchReview();
                    break;

                    case "8":
                    DisplayReviewsofRestaurants();
                    break;

                    case "9":
                    SearchRestaurant();
                    break;

                    default:
                        Console.WriteLine("We don't understand what you're doing");
                    break;
                }
            } while(repeat);
        }

        private void AddUser() 
        {
            //to ask for name, username and password
            string name;
            string username;
            string password;
            bool check = true;
            User userToAdd;
            do
            {
                System.Console.WriteLine("What is your name? ");
                name = Console.ReadLine();

            } while(String.IsNullOrWhiteSpace(name));
            do
            {
                System.Console.WriteLine("What is the username you want for your account? "); //check if its included in the database already
                username = Console.ReadLine();
                //if username already exists, ask for another
                if (SearchUsernameID(username) == false)
                    check = false;
                else
                    System.Console.WriteLine("We are sorry, " + username + " already exists in our system. Choose a Username that doesn't exist in our system.");
            } while(check);
            do
            {
                System.Console.WriteLine("What is your password? "); //potentially check against good passwords
                password = Console.ReadLine();

            } while(String.IsNullOrWhiteSpace(password));
            userToAdd = new User(name, username, password, false);
            userToAdd = _reviewb1.AddUser(userToAdd);
            System.Console.WriteLine($"{userToAdd.Name} was successfully added!");

        }
        private bool SearchUsernameID(string username)
        {
            List<User> users = AllUsers();
            foreach (User user in users)
            {
                if (user.Username == username)
                    return true;
            }
            return false;
        }
        private void AddAdmin()
        {
            //to ask for default password, if its correct, ask for username and password
            bool check = false;
            string username;
            string input; 
            string name; 
            string password; 
            
            do
            {
                System.Console.WriteLine("Please enter the default Admin Password, or press [0] to exit");
                input = Console.ReadLine();
                if (input == "7294") {
                    check = true;
                }
                else if (input == "0") {
                    goto Endofmethod;
                }
            }while(check != true);
            System.Console.WriteLine("Welcome Admin!");
            do
            {
                System.Console.WriteLine("What's your name? ");
                name = Console.ReadLine();
            }while (String.IsNullOrWhiteSpace(name));
            do 
            {
                System.Console.WriteLine("What do you want your username to be? ");
                username = Console.ReadLine();
            }while (String.IsNullOrWhiteSpace(username));
            do
            {
                System.Console.WriteLine("What do you want to change your default password to? ");
                password = Console.ReadLine();
            }while (String.IsNullOrWhiteSpace(password));
            User userToAdd;
            userToAdd = new User(name, username, password, check);
            userToAdd = _reviewb1.AddUser(userToAdd);
            System.Console.WriteLine($"{userToAdd.Name} was successfully added as an Admin!");
            Endofmethod: Console.WriteLine("Please choose another option!");
        }
        private void AddReview()
        {
            string rast = "";
            string title = "";
            string body = "";
            decimal ratinghere;
            bool check = true;
            System.Console.WriteLine("Welcome to adding a review!");
           do 
            {
                System.Console.WriteLine("What's the name of the Restaurant you want to add a review for? or press [0] to exit");
                rast = Console.ReadLine();
                if (rast == "0")
                {
                    check = false;
                }
                if (SearchRestaurantName(rast) == true)
                    check = false;
                else
                    Console.WriteLine("We are sorry, the Restaurant you are trying to write a review for does not exist");
            }while(check);
            if (rast == "0")
                goto endofadd;
            bool Screaming = true;
             do
            {
                System.Console.WriteLine("What Review do you give this restaurant out of 5? ");
                if (decimal.TryParse(Console.ReadLine(), out ratinghere))
                    Screaming = false;
            }while (Screaming);
            do
            {
                System.Console.WriteLine("What title do you give this review? ");
                title = Console.ReadLine();
            }while (String.IsNullOrWhiteSpace(title));
            do
            {
                System.Console.WriteLine("What is the content, the body, of your review? ");
                body = Console.ReadLine();
            }while (String.IsNullOrWhiteSpace(body));
            Review reviewToAdd;
            Restaurant thisrestaurant = SearchRestaurantID(rast);
            int id = thisrestaurant.Id;
            thisrestaurant.Cnt = thisrestaurant.Cnt+1; //I want to change the 
            reviewToAdd = new Review(title, body, ratinghere, id);
            reviewToAdd = _reviewb1.AddReview(reviewToAdd);
            System.Console.WriteLine(reviewToAdd.IRestuarant);
            System.Console.WriteLine($"The Review with the title of {reviewToAdd.Title} has successfully been added!");
            endofadd: System.Console.WriteLine("Returning to options");
        }
        private void AddRestaurant()
        {
            string rast = "";
            bool check = true;
            int zipcode;
            System.Console.WriteLine("Welcome to adding a restaurant! ");
            do 
            {
                System.Console.WriteLine("What's the name of the Restaurant you want to add? or press [0] to exit");
                rast = Console.ReadLine();
                if (SearchRestaurantName(rast) == false)
                    check = false;
                else 
                    Console.WriteLine("We are sorry, the Restaurant you are trying to add is already in our system");
            }while(check);
            check = true;
            do
            {
                System.Console.WriteLine("What's the zipcode of your Restaurant? ");
                if (int.TryParse(Console.ReadLine(), out zipcode))
                    check = false;
            }while(check);
            Restaurant AddRestaurant;
            AddRestaurant = new Restaurant(rast, zipcode, 0, 0);
            AddRestaurant = _reviewb1.AddRestaurant(AddRestaurant);
            System.Console.WriteLine($"{AddRestaurant.Name} was successfully added as a Restaurant in the system!");
        }
        private Restaurant SearchRestaurantID(string name)
        {
           List<Restaurant> restaurants = AllRestaurants();
           foreach (var res in restaurants)
           {
               if (res.Name == name)
                    return res;
           }
           return null;

        }
        private void DisplayRestaurant() //to implement. 
        {
            //display details of restaurant to user
            string rast = "";
            bool check = true; 
            do 
            {
                System.Console.WriteLine("What restaurant do you want to look up? ");
                rast = Console.ReadLine();
                if (SearchRestaurantName(rast) == true) //this checks if it exists, the method returns true if it does and false if it doesn't
                    check = false;
                else 
                    Console.WriteLine("We are sorry, the Restaurant you are trying to add is already in our system");
            }while(check);
            Restaurant sleep = SearchRestaurantID(rast);
            System.Console.WriteLine();
            System.Console.WriteLine($"Name: {sleep.Name} ");
            System.Console.WriteLine($"Body: {sleep.Zipcode} ");
            System.Console.WriteLine($"Rating: {sleep.Rating} ");
            System.Console.WriteLine($"---------------------------");


        }
        private bool SearchRestaurantName(string name)
        {
           List<Restaurant> restaurants = AllRestaurants();
           foreach (var res in restaurants)
           {
              if (res.Name == name)
                return true;
           }
           return false;

        }
        private List<Restaurant> AllRestaurants() //helper method to return restaurants with their name, zipcode and rating
        {
            return _reviewb1.AllRestaurants();
        }
        private void SearchReview()
        {
            List<Review> reviews = AllReviews();
        }
        private List<Review> AllReviews() //helper method to return reviews with title, body, rating, name, restaurant name
        {
            return _reviewb1.AllReviews();
            //this will need to grab the name of the restaurant and name of the user for that restaurant
        }
        private void SearchUser() //to implement
        {
            List<User> users = AllUsers();
            //to start find out if this is an admin user
            string username;
            bool check = true;
            do
            {
                System.Console.WriteLine("What's your username? or press [0] to exit");
                username = Console.ReadLine();
                if (username == "0")
                    check = false;
                if (SearchUserName(username) == true)
                    check = false;
                else
                    System.Console.WriteLine("We are sorry " + username + " either isn't a username in our database or isn't a username that has Admin permission. Please try again.");
            } while (check);
            if (username == "0")
                goto scream;
            check = true;
            do
            {
                System.Console.WriteLine("Do you want to search Users on [0] Username, [1] Name, [2] isAdmin status, or [3] Exit");
                switch(Console.ReadLine())
                {
                    case "0":
                    SearchByUsername(users);
                    break;

                    case "1":
                    SearchByName(users);
                    break;

                    case "2":
                    SearchByAdmin(users);
                    break;

                    case "3":
                    check = false;
                    break;

                    default:
                    System.Console.WriteLine("We are sorry, we don't know what you just entered");
                    break;
                }
            } while (check);
            
            scream: System.Console.WriteLine("Returning you to the options");
        }
        private void SearchByUsername(List<User> users) //searches users by username
        {
            startofthismethod:
            bool check = false;
            string annoyed;
            System.Console.WriteLine("What username do you want to search on? ");
            string username = Console.ReadLine();
            foreach (User x in users)
            {
                if(x.Username == username)
                {
                    System.Console.WriteLine("This is the User you requested:");
                    System.Console.WriteLine($"Username: {x.Username}");
                    System.Console.WriteLine($"Name: {x.Name}");
                    System.Console.WriteLine($"isAdmin: {x.isAdmin}");
                    System.Console.WriteLine("-----------------------");
                    check = true;
                }
            }
            if(check == false)
            {
                do
                {
                    System.Console.WriteLine("We are sorry, we couldn't find the user " + username + ". Would you want to search again? [0] No [1] Yes");
                    annoyed = Console.ReadLine();
                } while (annoyed != "0" || annoyed != "1");
                if (annoyed == "1")
                    goto startofthismethod;
                else
                    goto endofmethod;

            }
            endofmethod: System.Console.WriteLine("Returning you to the search options");
        }
        private void SearchByName(List<User> users) //searches users by name
        {
            startofthismethod:
            bool check = false;
            string annoyed;
            System.Console.WriteLine("What name do you want to search on? ");
            string name = Console.ReadLine();
            foreach (User x in users)
            {
                if(x.Name == name)
                {
                    System.Console.WriteLine("This is the User you requested:");
                    System.Console.WriteLine($"Name: {x.Name}");
                    System.Console.WriteLine($"Username: {x.Username}");
                    System.Console.WriteLine($"isAdmin: {x.isAdmin}");
                    System.Console.WriteLine("-----------------------");
                    check = true;
                }
            }
            if(check == false) //we didn't find the name
            {
                do
                {
                    System.Console.WriteLine("We are sorry, we couldn't find the name " + name + ". Would you want to search again? [0] No [1] Yes");
                    annoyed = Console.ReadLine();
                } while (annoyed != "0" || annoyed != "1");
                if (annoyed == "1")
                    goto startofthismethod; //search again on name
                else
                    goto endofmethod; //go back to search options

            }
            endofmethod: System.Console.WriteLine("Returning you to the search options");

        }
        private void SearchByAdmin(List<User> users) //searches users by if they are an Admin
        {
            startofthismethod:
            bool answer = true;
            string admin;
            bool sp = true;
            string annoyed;
            do
            {
                System.Console.WriteLine("Enter True to search for Users who are Admins and False to search for Users who aren't Admin ");
                admin = Console.ReadLine();
                if (admin == "True")
                {
                    sp = false;
                    answer = true;
                }
                else if (admin == "False")
                {
                    sp = false;
                    answer = false;
                }
                else 
                    System.Console.WriteLine($"{admin} is not True or False. Please enter a valid input");
                
            } while (sp);
            foreach (User x in users)
            {
                if(x.isAdmin == answer)
                {
                    System.Console.WriteLine("This is the User you requested:");
                    System.Console.WriteLine($"Name: {x.Name}");
                    System.Console.WriteLine($"Username: {x.Username}");
                    System.Console.WriteLine($"isAdmin: {x.isAdmin}");
                    System.Console.WriteLine("------------------------");
                }
            }
            sp = true;
            do
            {
                System.Console.WriteLine("Do you want to search again? [0] No [1] Yes");
                annoyed = Console.ReadLine();
                if (annoyed == "0")
                    sp = false;
                else if (annoyed == "1")
                    sp = false;
                else 
                    System.Console.WriteLine($"{annoyed} is not a 0 or a 1. Please enter a valid input");    
            } while (sp);
            if (annoyed == "1")
                goto startofthismethod;
            System.Console.WriteLine("Returning you to the search options");

        }
        private bool SearchUserName(string username)//takes a username and checks if that username is an admin
        {
           List<User> users = AllUsers();
           foreach (var user in users)
           {
              if (user.Username == username && user.isAdmin == true) //if that username exists and that user is an admin return true, otherwise return false. 
                return true;
           }
           return false;

        }
        private List<User> AllUsers()
        {
            return _reviewb1.AllUsers();
        }
        // private User Search(string search) {
        //     List<User> users = AllUsers();
        //     foreach (var user in users){
        //         if (user.Name == user.search || user.Username == user.search)
        //             return user;
        //     }
        //     return null;
        // }
        private void DisplayReviewsofRestaurants()
        {
            //be able to display details of reviews for a restaurant to the user
            bool check = true; 
            string rast = "";
            do 
            {
                System.Console.WriteLine("What restaurant do you want to look up? ");
                rast = Console.ReadLine();
                if (SearchRestaurantName(rast) == true) //this checks if it exists, the method returns true if it does and false if it doesn't
                    check = false;
                else 
                    Console.WriteLine("We are sorry, the Restaurant you are trying to add is already in our system");
            }while(check);
            Restaurant DRest = SearchRestaurantID(rast); //Gets the restaurant with that name, and because I already checked that its valid I don't need to check it here
            List<Review> reviewsDRest = AllReviews(); //gets all the reviews
            List<Review> dispreviewsDRest = new List<Review>();
            foreach (Review revi in reviewsDRest)
            {

                if((revi.IRestuarant+1) == DRest.Id)
                {
                    dispreviewsDRest.Add(revi);
                }
            }

            //gets all the reviews where the ids match from the restaurant ID to the reviews in the list
            foreach (Review rev in dispreviewsDRest)
            {
                System.Console.WriteLine($"Title: {rev.Title} ");
                System.Console.WriteLine($"Body: {rev.Body} ");
                System.Console.WriteLine($"Rating: {rev.Rating} ");
                System.Console.WriteLine($"---------------------------");
            }
        }
        private void SearchRestaurant()
        {
            start:
            List<Restaurant> restaurants = AllRestaurants(); 
            //search by name, zipcode, rating
            string AAAAAAAAA;
            string answer;
            do
            {
                System.Console.WriteLine("Do you want to look up via [0] Zipcode, [1] Name, or [2] Rating? ");
                AAAAAAAAA = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(AAAAAAAAA));
            if (AAAAAAAAA == "0")
            {
                do
                {
                    System.Console.WriteLine("what is the zipcode you wnat to search on? ");
                    answer = Console.ReadLine();
                } while (String.IsNullOrEmpty(answer));

                System.Console.WriteLine("The restaurants with the requested zipcode are as follows:");
                foreach (Restaurant i in restaurants)
                {
                    if (i.Zipcode.ToString() == answer)
                    {
                        System.Console.WriteLine("---------------------------");
                        System.Console.WriteLine(i.Name);
                        System.Console.WriteLine(i.Zipcode);
                        System.Console.WriteLine(i.Rating);
                        System.Console.WriteLine("---------------------------");
                    }
                }
            } else if (AAAAAAAAA == "1")
            {
                 do
                {
                    System.Console.WriteLine("what is the name you want to search on? ");
                    answer = Console.ReadLine();
                } while (String.IsNullOrEmpty(answer));
                System.Console.WriteLine("The restaurants with the requested name are as follows:");
                foreach (Restaurant i in restaurants)
                {
                    if (i.Name == answer)
                    {
                        System.Console.WriteLine("---------------------------");
                        System.Console.WriteLine(i.Name);
                        System.Console.WriteLine(i.Zipcode);
                        System.Console.WriteLine(i.Rating);
                        System.Console.WriteLine("---------------------------");
                    }
                }
            } else if (AAAAAAAAA == "2")
            {
                 do
                {
                    System.Console.WriteLine("what is the rating you want to search on? ");
                    answer = Console.ReadLine();
                } while (String.IsNullOrEmpty(answer));
                System.Console.WriteLine("The restaurants with the requested rating are as follows:");
                foreach (Restaurant i in restaurants)
                {
                    if (i.Rating.ToString() == answer)
                    {
                        System.Console.WriteLine("---------------------------");
                        System.Console.WriteLine(i.Name);
                        System.Console.WriteLine(i.Zipcode);
                        System.Console.WriteLine(i.Rating);
                        System.Console.WriteLine("---------------------------");
                    }
                }
            }
            bool check = true;
            string hello;
            do
            {
                System.Console.WriteLine("Do you want to search again? [0] No or [1] Yes");
                hello = Console.ReadLine();
                if (hello == "0")
                    check = false;
                else if (hello == "1")
                    check = false;
                else
                    System.Console.WriteLine("Please enter a valid input of either [0] or [1]");
            } while (check);
            if (hello == "1")
                goto start;
            System.Console.WriteLine("Returning you to the options");
        }
        private void SeeReviewRating()
        {
            startof:
            decimal count = 0;
            int i = 0;
            string answer = "";
            //calculate reviews’ average rating for each restaurant
            do
            {
                System.Console.WriteLine("What restaurant do you want to see the Review Rating for? ");
                answer = Console.ReadLine();
            } while (SearchRestaurantName(answer));
            Restaurant restaurant = SearchRestaurantID(answer);
            List<Review> reviews = AllReviews();
            foreach (Review review in reviews)
            {
                if (review.IRestuarant == restaurant.Id)
                {
                    count = (count + review.Rating); 
                    i++;
                }
            }
            decimal rating = count/i;
            System.Console.WriteLine($"{rating} is the rating of {answer}");
            bool Revature = true;
            string Tired = "";
            do
            {
                System.Console.WriteLine("Do you want to look for another Restaurant? [0] No [1] Yes");
                Tired = Console.ReadLine();
                if (Tired == "0")
                    Revature = false;
                else if (Tired == "1")
                    Revature = false;
                else 
                    System.Console.WriteLine("We are sorry but, " + Tired + ", is not a 0 or a 1, try again");
            } while (Revature);
            if (Tired == "1")
                goto startof;
            System.Console.WriteLine("Returning to options");
        }
        //potential join method for ReviewJoin?


    }

}