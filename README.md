# TODO:
1. Set the database configuration for the Book Entity:
    - Title - maximum 100 characters, mandatory
    - Description - maximum 500 characters, optional
    - ReleaseDate - mandatory
2. Add validation for Books and Authors.
3. Implement a new features, the implementation should include both the frontend and backend.
    - Adding new books
        - Newly added books should be linked with an author via a many-to-one relation (many books can have one author).
        - The "BookCount" property on the Author entity should increment when a new book is added to the author.
        - Prevent duplication of records
    - Listing books
    - Deleting books
5. Add pagination (using the already existing extension method: "ToPagedResult").
6. Add unit tests.
7. Fix all mistakes in the code.
8. If you have any ideas for improvement, changes, refactor, feel free to implement them, for exampele new C# features.
9. Use general programming good practices and coding standards.

If have any questions please let us know: abronowicki@openskydata.com, mlinke@openskydata.com

``` Project link
https://bookshopmvc-fgdmc9aqeyeucsc6.polandcentral-01.azurewebsites.net
```


//TODO: create alises for this command
```bash
>> create migration
dotnet ef migrations add <MigrationName> --project BookShop.Infrastructure --startup-project BookShop.Web
>> update DB
dotnet ef database update --project BookShop.Infrastructure --startup-project BookShop.Web 

```


