# Task 07 - Interview Answers

Short answers based on Phase 01 work. Written in my own words.

---

**1. What is the difference between class and object?**  
A class is the blueprint. An object is an actual instance created from that class. In the bank system, `BankAccount` is the class and each created account is an object.

**2. What is encapsulation?**  
Encapsulation means hiding internal data and controlling access through methods. In Task 02, balance is changed only through `Deposit()` and `Withdraw()`, not by setting balance directly.

**3. Why should account balance not be public?**  
Because any code could set it to an invalid value like a negative balance. Methods let us validate each change.

**4. What is the difference between field and property?**  
A field stores data directly. A property wraps a field with get/set logic. In `BankAccount`, balance uses a private setter through methods instead of public assignment.

**5. Why do we use constructors?**  
Constructors initialize object state when it is created. They make sure required values are set from the start.

**6. What is the purpose of a service class?**  
A service class holds business logic and coordinates operations. `BankService` and `EmployeeService` handle rules so the menu/UI stays simple.

**7. Why should we avoid huge Main methods?**  
Large Main methods are hard to read, test, and maintain. Splitting into methods/classes makes responsibilities clearer. Drill 20 practiced this.

**8. What is the difference between List and Array?**  
Both store collections. Arrays have fixed size after creation. Lists can grow/shrink and have helpful methods like Add and Remove.

**9. When would you use Dictionary?**  
When you need fast lookup by key. Drill 16 used a dictionary to count how many times each number appears.

**10. What is LINQ used for?**  
LINQ queries collections in a readable way. Task 04 used it to filter, sort, group, and project product data.

**11. What is the difference between Where and Select?**  
`Where` filters items. `Select` transforms/projection items into a new shape. Example: filter available products, then project to summary DTOs.

**12. What is GroupBy used for?**  
It groups items by a key, like grouping products by category or counting products per supplier.

**13. What are Skip and Take used for?**  
They simulate pagination. Skip jumps rows, Take limits how many rows return.

**14. What is a primary key?**  
A primary key uniquely identifies each row in a table, like `CustomerId` or `ProductId` in Task 06.

**15. What is a foreign key?**  
A foreign key links one table to another. Example: `Orders.CustomerId` references `Customers.CustomerId`.

**16. What is a one-to-many relationship?**  
One record in a parent table relates to many records in a child table. One customer has many orders.

**17. Why do we use JOIN?**  
JOIN combines related tables in one query so we can show meaningful data together, like order details with customer and product names.

**18. What is the difference between table and entity?**  
A table is how data is stored in the database. An entity is the object/class that represents that data in code.

**19. Why do we use GitHub?**  
For version control, backup, collaboration, and submitting training work with commit history.

**20. What makes a README useful?**  
It explains what the project does, how to run it, folder structure, and what was completed. Reviewers should not guess.

**21. Why are multiple commits better than one final commit?**  
They show real progress over time and make it easier to track what changed in each step.

**22. Why is professional delivery important?**  
Backend work is not only code. Clear structure, docs, and evidence show you can deliver work others can review and run.
