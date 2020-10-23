# Providing an API to a document repository

Your task is to provide an initial search interface for an existing document repository. These kind of tasks are done as part of an effort to better understand the existing systems and their constraints. If providing a search interface is considered worthwhile, then depending on the size of the system and the business requirements, documents will probably be indexed into an existing search engine. So most likely API method signatures will remain the same, but how you interact with the document repository will change.

As a general way of thinking, this is currently a case of streaming search: documents are not indexed beforehand, but instead scanned through for every query. One additional part that you do not need to implement here is relevance scoring.

Now, to the technical matters and setting out the expectations for this task. As a first point, this README does not provide guidance in setting up your development environment, as this can be very different depending on the OS, IDE and your workflow. One way to check the correctness of your environment if developing on the command line is to see if `dotnet watch --project RepositorySearch.Api.Tests test` works for you from the solution root folder.

You should:

- implement the solution by using idiomatic C#
- wire up the necessary services using dependency injection in `RepositorySearch.Api`
- have all the tests pass

You can:

- add more tests
- reuse existing code
- use other ASP.NET Core libraries 
- use a JSON handling library of your own choice. So for handling JSON, Newtonsoft library is a valid option here

You must not:

- modify the `Resources/data.json` file
- modify the existing tests
- modify the infrastructure expectations of the solution. For example, by adding a dependency on a database server

One hint that we will provide for you is reading in the JSON file. As it is embedded inside the compiled executable, here is one way to access the contents:

```csharp
var fileName = "RepositorySearch.Api.Resources.data.json";
var assembly = Assembly.GetExecutingAssembly();
using (var stream = assembly.GetManifestResourceStream(fileName))
{
    // deserialization code goes here
}
```
