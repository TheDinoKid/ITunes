using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ITunes
{
    class Program
    {

        static void insert(bool album, int albumID)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dakot\source\repos\AcademyPittsburgh\Week5\ITunes\ITunes\Database1.mdf;Integrated Security=True");
            connection.Open();

            string item;
            string variable;
            string setup;
            if (album)
            {
                item = "Album";
                variable = "Genre";
                setup = "Albums(Album, Artist, Genre, AlbumYear)";
            }
            else
            {
                item = "Song";
                variable = "songLength";
                setup = "Songs(Song, AlbumID, SongLength, SongYear)";
            }

            //Ask user for album information
            Console.Write($"{item} Name: ");
            string itemName = Console.ReadLine();
            Console.Write($"{item} Year: ");
            string itemYear = Console.ReadLine();
            Console.Write($"{variable}: ");
            string idk = Console.ReadLine();

            if (album)
            {
                Console.Write("Artist: ");
                string optional = Console.ReadLine();
                Console.WriteLine($"{item}:{itemName} Artist:{optional} {variable}:{idk} {item} Year:{itemYear}");
                //INSERT into table
                SqlCommand insertAlbum = new SqlCommand($"INSERT into {setup} values('{itemName}', '{optional}', '{idk}', '{itemYear}')", connection);
                SqlDataReader reader = insertAlbum.ExecuteReader();
                reader.Close();
            }
            else
            {
                int optional = Convert.ToInt32(albumID);
                Console.WriteLine($"{item}:{itemName} Artist:{optional} {variable}:{idk} {item} Year:{itemYear}");
                //INSERT into table
                SqlCommand insertAlbum = new SqlCommand($"INSERT into {setup} values('{itemName}', '{optional}', '{idk}', '{itemYear}')", connection);
                SqlDataReader reader = insertAlbum.ExecuteReader();
                reader.Close();
            }
            connection.Close();

        }
        static void view( bool album, int albumID)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dakot\source\repos\AcademyPittsburgh\Week5\ITunes\ITunes\Database1.mdf;Integrated Security=True");
            connection.Open();
            string tableViewer;
            if (album)
                tableViewer = "Albums";
            else
                tableViewer = $"Songs where AlbumID = '{albumID}'";


            SqlCommand view = new SqlCommand($"SELECT * from {tableViewer}", connection);
            SqlDataReader reader = view.ExecuteReader();

            if (album)
            {
                //lets user know how to put in singles
                Console.WriteLine("0: Single");
                Console.WriteLine();

                while (reader.Read())
                {
                    Console.WriteLine($"{reader["ID"]}: Album:{reader["Album"]} Artist:{reader["Artist"]} Genre:{reader["Genre"]} Tracks:{reader["#Tracks"]} Album Year:{reader["AlbumYear"]}");
                    Console.WriteLine();
                }
                reader.Close();
            }
            else
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["ID"]}: Song:{reader["Song"]} Length:{reader["SongLength"]} Year:{reader["SongYear"]}");
                    Console.WriteLine();
                }
                reader.Close();
            }

            //for each row in table
        }
        static void Main(string[] args)
        {

            //TEAN Unlike Breeze Family - Justin - Tyler - Kelvin - Dakota - TA:John

            //Table Albums: 
            /*
            
            CREATE TABLE [dbo].[Albums] (
            [Id]        INT          IDENTITY (1, 1) NOT NULL,
            [Album]     VARCHAR (50) NOT NULL,
            [Artist]    VARCHAR (50) NOT NULL,
            [Genre]     VARCHAR (50) NULL,
            [#Tracks]   INT          DEFAULT ((0)) NOT NULL,
            [AlbumYear] INT          NOT NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
            );

            */


            //Table Songs:
            /*
            
             CREATE TABLE [dbo].[Songs] (
            [Id]         INT          IDENTITY (1, 1) NOT NULL,
            [Song]       VARCHAR (50) NOT NULL,
            [SongLength] VARCHAR (50) NOT NULL,
            [AlbumID]    INT          NOT NULL,
            [SongYear]   INT          NOT NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
            );

             */




            // Create iTunes
            // But better, cause it doesn't force a download of U2
            string choice = "";
            while (choice != "q")
                {
                //asks user what they want to do
                Console.WriteLine("Would you like to: \n   a-add an album\n   b-add a song\n   c-view all albums\n   d-view an album\n   e-view all songs\n   f-view a specific song from an album\n   g-view all albums from an artist\n   q-quit\n   u-????");
                choice = Console.ReadLine().ToLower();

                //connects project to our sql database
                SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dakot\source\repos\AcademyPittsburgh\Week5\ITunes\ITunes\Database1.mdf;Integrated Security=True");
                connection.Open();

                // Let a user add albums
                if (choice == "a")
                    insert(true, 0);
                //  Let the user add songs to an album
                else if (choice == "b")
                {
                    //display albums with IDs
                    view(true, 0);

                    //Ask user for Album ID
                    Console.Write("Which album(ID) would you like to add a song to: ");
                    //set album id
                    int albumID = Convert.ToInt32(Console.ReadLine());
                    string moreSongs = "yes";
                    int trackCount = 0;
                    // loop
                    while (moreSongs == "yes")
                    {
                        insert(false, albumID);
                        // tracker + 1
                        trackCount += 1;
                        //ask if there is more songs
                        Console.Write("Would you like to add more songs to this album (yes/no): ");
                        moreSongs = Console.ReadLine().ToLower();
                        //end loop
                    }
                    //update #Tracks
                }
                // Let a user list their albums
                else if (choice == "c" || choice == "g")
                    view(true, 0);
                //  Let the user list the songs on an individual album
                else if (choice == "d")
                {
                    view(true, 0);
                    //Ask user for Album ID
                    Console.Write("Which album(ID) would you like to add a song to: ");
                    //set album id
                    int albumID = Convert.ToInt32(Console.ReadLine());

                    view(false, albumID);
                }

                // Bonus:
                // Let the user show all of their songs and which album that song is on
                else if (choice == "e")
                {
                    SqlCommand viewAlbum = new SqlCommand($"SELECT * from Songs join Albums on Songs.AlbumID = Albums.Id", connection);
                    SqlDataReader reader = viewAlbum.ExecuteReader();

                    //lets user know how to put in singles
                    Console.WriteLine();

                    //for each row in table
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Album"]}: Song:{reader["Song"]} Length:{reader["SongLength"]} Year:{reader["SongYear"]}");
                        Console.WriteLine();
                    }
                    reader.Close();
                }
                // Let user see specific song from an album
                else if (choice == "f")
                {
                    Console.Write("Which song would you like to view: ");
                    //set album id
                    string songName = Console.ReadLine();

                    //print all songs based off albumID
                    SqlCommand view = new SqlCommand($"SELECT * from Songs join Albums on Songs.AlbumID = Albums.Id where Song = '{songName}'", connection);
                    SqlDataReader Reader = view.ExecuteReader();
                    Console.Clear();
                    //for each row in table
                    while (Reader.Read())
                    {
                        Console.WriteLine($"Album:{Reader["Album"]}: Song:{Reader["Song"]} Length:{Reader["SongLength"]} Year:{Reader["SongYear"]} Artist:{Reader["Artist"]} Genre:{Reader["Genre"]}");
                        Console.WriteLine();
                    }
                    Reader.Close();

                }

                else if (choice == "u")
                {
                    string target = "https://www.youtube.com/watch?v=98W9QuMq-2k";
                    System.Diagnostics.Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", target);
                }

                // Bonus:(see line 111)
                // Make an Artist table, so you can see which albums
                // were by a single artist?
                // Or maybe do this with Genre instead?

                Console.WriteLine();
                connection.Close();
            }
        }
    }
}
