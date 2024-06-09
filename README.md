## Supermarket checkout implementation

This project has been developed as a proposal solution for the supermarket checkout challenge.
Although there is a very basic functionality, my main focus was, using DDD principles, develop a simple solution
keeping business logic inside the domain layer.

Basically, Checkout object has the responsability for handling scan items and then apply pricing rules based on these items.
Then, using composition, it is possible to define two concrete pricing rules that can be customized to create multiple pricing rules.

## Next Steps

- Add persistence layer implementing repositories using whatever database system.
- Expose checkout functionality using REST Api
- Integration tests
