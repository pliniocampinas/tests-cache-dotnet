# tests-cache-dotnet
Testing memory cache for web apps

### Running tests

- With node v14+ and dotnet v6+.
- Run the api with <code>dotnet run</code>
- Run test client with <code>cd test-client | npm run start</code>

### The examples handles the following scenarios:

- Prevent Cache stampede with a Semaphore.
- Cleaning cache with a time hosted service. Dotnet garbage collector does not clean expired cache if it is not read again.
- Limits growth with a max number of entries.
