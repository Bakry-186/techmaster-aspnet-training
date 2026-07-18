# Task 07 - Interview Answers

Short answers based on Phase 02 work.

---

**1. What is REST?**  
REST is a style for building APIs using HTTP resources and standard verbs. In Phase 02, `/api/students` and `/api/products` are resources accessed with GET, POST, PUT, PATCH, and DELETE.

**2. What is the difference between GET and POST?**  
GET reads data and should not change server state. POST creates a new resource. Example: `GET /api/students/1` reads a student, `POST /api/students` creates one.

**3. When do you use PUT vs PATCH?**  
PUT replaces the full resource. PATCH updates part of it. In Task 02, `PUT /api/students/{id}` updates all student fields, while `PATCH /api/students/{id}/status` only changes `IsActive`.

**4. What status code should create return?**  
201 Created. Task 02 uses `CreatedAtAction` after POST /api/students.

**5. What status code should delete return when successful?**  
204 No Content. Task 02 DELETE /api/students/{id} returns 204 with no body.

**6. What is the difference between 400 and 404?**  
400 means the request is invalid (bad input). 404 means the target resource was not found. Task 01 drill 15 and Task 02 error responses demonstrate both.

**7. What is route parameter vs query string?**  
Route parameters are part of the URL path like `/api/students/5`. Query strings are optional filters like `?search=abdel&pageNumber=1`. Task 01 drills 02 and 03 taught both.

**8. Why use DTOs instead of exposing entity models directly?**  
DTOs control what clients send and receive. CreateStudentDto does not expose Id or EnrollmentDate on create, which prevents invalid client input.

**9. What is the role of a controller?**  
Controllers handle HTTP concerns: routing, status codes, and request/response mapping. Business logic should stay in services like `StudentService`.

**10. Why use a service layer?**  
Services hold business rules and data access. This keeps controllers thin and makes logic reusable and testable. Task 03 ProductService validates category existence before creating products.

**11. What is Dependency Injection?**  
DI means the framework provides dependencies instead of classes creating them manually. In Program.cs, `AddSingleton<IStudentService, StudentService>()` registers the service for controllers.

**12. When should you use Singleton vs Scoped?**  
Singleton lives for the app lifetime. Scoped lives per request. In-memory stores in Phase 02 use Singleton so data persists between requests during development.

**13. What is Swagger used for?**  
Swagger documents and tests APIs interactively. Every Phase 02 project exposes `/swagger` in Development to explore endpoints quickly.

**14. What is Postman used for?**  
Postman is an API client for manual testing and collections. Task 05 exports a collection to replay Student, Product, and Book Store requests with evidence screenshots.

**15. How does pagination work in your APIs?**  
Clients send `pageNumber` and `pageSize`. The service uses Skip/Take and returns total counts. Task 02 GET /api/students and Task 04 GET /api/books both support this.

**16. Why validate input on the API?**  
To reject bad data early and return clear errors. Examples: unique email in Task 02, positive price in Task 03, unique ISBN in Task 04.

**17. What is a one-to-many relationship in the Book Store API?**  
One author can have many books. Books store `AuthorId` as a foreign key reference, similar to database design in Phase 01 Task 06.

**18. What did Task 06 teach about bad API design?**  
Bad APIs use wrong verbs, wrong status codes, and put everything in controllers. The refactor pack shows how to fix routing, DTOs, services, and error responses.

**19. What is the standard error shape used in Phase 02?**  
`{ success: false, message, code, details[] }`. This keeps client error handling consistent across Student, Product, and Book Store APIs.

**20. How does Phase 02 prepare you for Phase 03 (EF Core)?**  
Phase 02 builds the API structure (controllers, DTOs, services, validation). Phase 03 will replace in-memory lists with database persistence using the same architecture.

**21. Why are multiple commits important in training delivery?**  
They show step-by-step progress and make review easier. Phase 02 tasks are designed to be committed in small batches like drills 01–05 in Task 01.

**22. What makes a Phase 02 README useful?**  
It lists endpoints, run commands, business rules, and evidence checklist so reviewers can test the API without guessing project structure.
