# Task 05 - Debug & Refactor Pack

## Structure

- `original-bad-code/Program.cs` - messy version kept for comparison
- `OrderRefactor/` - cleaned up version

## Run refactored app

```bash
cd phase-01-backend-foundations/task-05-debug-refactor-pack/OrderRefactor
dotnet run
```

## What changed (10+ improvements)

1. Renamed unclear variables (`c`, `p`, `pr`) to meaningful names
2. Extracted `Customer` and `Order` models
3. Moved calculations to `OrderCalculator`
4. Added `ValidationHelper` for price, quantity, and names
5. Replaced magic numbers with constants (tax 14%, shipping 50)
6. Used `decimal` instead of `double` for money
7. Replaced `Parse` with `TryParse` to avoid crashes
8. Separated UI input from business logic
9. Added `ReceiptPrinter` for cleaner output
10. Used enum for customer type instead of raw strings
11. Split discount/tax/shipping into separate methods
12. Added README documenting before/after

Behavior matches the original rules: discount before tax, shipping after tax, free shipping at 1000+.
