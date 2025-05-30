This project is for my Back-End Development with .NET course in Coursera for my Microsoft Full Stack developer certificate

THe DOcumentation folder has the notes for Activity 1, 2, and 3.

Please note that I tested using the UserManagementAPI.http file. 
- first use dotnet run
- open the above file
- just under the word login, press "Send Request".  another window will open and you should get a token
- copy the token and then highlight <Your Token here> and do a find and replace all replacing the text with your token.
- you should be able to run all the tests.

- One way to get a 500 error is to shorten the program.cs IssuerSigningKey to only "YourSuperSecretKeyHere" as it will then be too short and throw an error.

Enjoy!
