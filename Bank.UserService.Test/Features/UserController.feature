Feature: User controller testing

    Scenario: Successful user login
        Given user login request
        When user sends valid login request
        Then login should be successful
        And user should get valid jwt