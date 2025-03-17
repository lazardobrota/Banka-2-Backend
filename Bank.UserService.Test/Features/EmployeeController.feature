Feature: Employee controller testing

    Scenario: Create Employee
        Given employee create request
        When employee is created in the database
        And employee is fetched by Id
        Then employee details should match the created employee