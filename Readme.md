# EF7-ManyToManyBug
This project has been made to report a bug with EF7 (beta8). The bug appears when using `Skip(anyNumber)` and `Take(>=60)` when using EF7 LINQ queries to access data structures with many-to-many relationships.

The bug results in the incorrect number of entities being loaded (and displayed).

The project contains the following:
  - Entity classes `Student`and `Course`with a many-to-many relationship `StudentCourse`,
  - a `DemoContext`linking the two entities,
  - a `StudentsController` demonstrating the bug.

#### Usage
 - Edit the connection string in `Startup.cs`to point to your database (I have been using SQL Server Express 2014) or switch the flag during startup to load the in memory database,
 - when using SQL Server, run the migrations command `dnx ef database update` to create the database if it doesn`t exist,
 - run the application and navigate to `api/students`

 Example query: `http://localhost:port/api/students?offset=0&limit=60`

#### Scenario
The database is seeded with 100 students and courses. Each student is then assigned 50 courses. So the expected output of the controller is to see 50 course IDs for each student in the JSON file.

#### Bug symptoms
When the `Take()`exceeds 60, the number of course IDs for each student is far below the expected fifty.
UPDATE: I can confirm that this bug does NOT appear when using the in memory database.

#### Controller methods
The `StudentsController` has four methods, all with integer request parameters offset & limit, showing different approaches to demonstrate the bug.

#### Notes
The bug is clearly related to using `Skip()` and/or `Take()` with EF7 LINQ queries. The magic number for `Take()` seems to be 60. Everything before that works fine.

#### SQL output
The SQL queries generated by EF7 are the following where `<LIMIT>` denotes the number used for `Take()`:

    SELECT [s].[StudentId], [s].[CourseId], [c].[Id], [c].[Name]
    FROM [StudentCourse] AS [s]
    INNER JOIN (
        SELECT DISTINCT [t0].*
        FROM (
            SELECT [s].[Id], [s].[Name]
            FROM [Student] AS [s]
            WHERE 1 = 1
          ORDER BY [s].[Id]
          OFFSET 0 ROWS FETCH NEXT <LIMIT> ROWS ONLY
       ) AS [t0]
    ) AS [s0] ON [s].[StudentId] = [s0].[Id]
    INNER JOIN [Course] AS [c] ON [s].[CourseId] = [c].[Id]
    go

However, using SQL Server Express 2014, when the limit is 60 or higher, the order of the output from the server changes. See the screenshots (50orLess.png & 60orHigher.png) for details.
The information contained in these queries remains the same.
