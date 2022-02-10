# DeadlockAspNetTest

Sandbox to demonstrate various situations in which ASP.Net MVC 5 (not Core) may fail

## Brief Summary

Use `async/await`

Using `GetAwaiter().GetResult()` could result if deadlocks if the synchronization context that is synchronously waiting for the result is also the synchronization context that has to run the continuation of the async API call.

Using `ConfigureAwait(false)` can prevent the deadlock as it allows continuations to run on the default synchronization context, but may cause problems if the continuation callback references something the synchronization context (e.g. `HttpContext.Current`)

## Useful links

- [Understanding Async, Avoiding Deadlocks in C#](https://medium.com/rubrikkgroup/understanding-async-avoiding-deadlocks-e41f8f2c6f5d)
- [Await, and UI, and deadlocks! Oh my!](https://devblogs.microsoft.com/pfxteam/await-and-ui-and-deadlocks-oh-my/)
- [ConfigureAwait FAQ](https://devblogs.microsoft.com/dotnet/configureawait-faq/)
