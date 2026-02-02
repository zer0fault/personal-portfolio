# Integration Tests

Focused integration tests for database operations and key workflows.

## Test Coverage

### DatabaseIntegrationTests (4 tests)
- Database seeding with all entities
- Idempotent seeding (no duplicates)
- CRUD operations work end-to-end
- Audit fields (CreatedDate, ModifiedDate) auto-populate

### RepositoryIntegrationTests (5 tests)
- Soft delete support for Project, Employment, and Settings
- Display order sorting works correctly
- Complex queries (filter by status + order by display order)

## Design Philosophy

These tests verify **essential integration points** without over-testing:
- Database context initialization works
- Seeding works as designed
- Soft delete behavior works
- Query operations work with real EF Core

The API functions are thin wrappers around MediatR handlers (already unit tested), so end-to-end API testing would be redundant.

## Total Test Suite

- **Domain.Tests**: 39 tests
- **Application.Tests**: 218 tests
- **Infrastructure.Tests**: 13 tests
- **Integration.Tests**: 9 tests
- **Total**: 279 tests ✅
